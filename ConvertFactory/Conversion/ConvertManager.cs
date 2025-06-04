using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertFactory
{
    /// <summary>
    /// Manages a queue of media conversion operations and handles their execution.
    /// </summary>
    public class ConvertManager
    {
        /// <summary>
        /// The list of conversions waiting to be processed.
        /// </summary>
        private readonly List<Conversion> _convertQueue;

        /// <summary>
        /// Callback action to be invoked when the queue changes.
        /// </summary>
        private readonly Action _onChange;

        /// <summary>
        /// Semaphore used to synchronize access to the conversion queue.
        /// </summary>
        private readonly SemaphoreSlim _queueLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Token source for cancelling conversion operations.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Indicates whether the queue is currently being processed.
        /// </summary>
        private bool _isProcessing;

        /// <summary>
        /// Event raised when a conversion operation starts.
        /// </summary>
        public event EventHandler<ConversionEventArgs> ConversionStarted;

        /// <summary>
        /// Event raised when a conversion operation completes successfully.
        /// </summary>
        public event EventHandler<ConversionEventArgs> ConversionCompleted;

        /// <summary>
        /// Event raised when a conversion operation fails.
        /// </summary>
        public event EventHandler<ConversionEventArgs> ConversionFailed;

        /// <summary>
        /// Event raised when a conversion operation is cancelled.
        /// </summary>
        public event EventHandler<ConversionEventArgs> ConversionCancelled;

        /// <summary>
        /// Initializes a new instance of the ConvertManager class.
        /// </summary>
        /// <param name="onChange">Callback action to be invoked when the queue changes.</param>
        /// <exception cref="ArgumentNullException">Thrown when onChange is null.</exception>
        public ConvertManager(Action onChange)
        {
            _onChange = onChange ?? throw new ArgumentNullException(nameof(onChange));
            _convertQueue = new List<Conversion>();
        }

        /// <summary>
        /// Processes all conversions in the queue asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is already being processed.</exception>
        public async Task RunQueueAsync()
        {
            if (_isProcessing)
                throw new InvalidOperationException("Queue is already being processed");
            try
            {
                _isProcessing = true;
                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;

                await _queueLock.WaitAsync();
                try
                {
                    var tasks = _convertQueue.Select(conversion => ProcessConversionAsync(conversion, token)).ToList();
                    await Task.WhenAll(tasks);
                }
                finally
                {
                    _queueLock.Release();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Conversion queue was cancelled");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing queue: {ex.Message}");
                throw;
            }
            finally
            {
                _isProcessing = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Processes a single conversion operation asynchronously.
        /// </summary>
        /// <param name="conversion">The conversion to process.</param>
        /// <param name="token">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessConversionAsync(Conversion conversion, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                OnConversionStarted(conversion);

                await conversion.Convert();

                if (conversion.Status == ConversionStatus.Completed)
                    OnConversionCompleted(conversion);
                else if (conversion.Status == ConversionStatus.Failed)
                    OnConversionFailed(conversion);
            }
            catch (OperationCanceledException)
            {
                conversion.UpdateStatus(ConversionStatus.Cancelled);
                OnConversionCancelled(conversion);
                throw;
            }
            catch (Exception ex)
            {
                conversion.UpdateStatus(ConversionStatus.Failed, ex.Message);
                OnConversionFailed(conversion);
                throw;
            }
        }

        /// <summary>
        /// Cancels all pending conversion operations.
        /// </summary>
        public void CancelQueue()
        {
            _cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Removes a conversion from the queue asynchronously.
        /// </summary>
        /// <param name="conversion">The conversion to remove.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RemoveAsync(Conversion conversion)
        {
            await _queueLock.WaitAsync();
            try
            {
                if (_convertQueue.Remove(conversion))
                {
                    _onChange.Invoke();
                }
            }
            finally
            {
                _queueLock.Release();
            }
        }

        /// <summary>
        /// Adds a conversion to the queue asynchronously.
        /// </summary>
        /// <param name="conversion">The conversion to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when conversion is null.</exception>
        public async Task AddAsync(Conversion conversion)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));

            await _queueLock.WaitAsync();
            try
            {
                _convertQueue.Add(conversion);
                _onChange.Invoke();
            }
            finally
            {
                _queueLock.Release();
            }
        }

        /// <summary>
        /// Clears all conversions from the queue asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ClearQueueAsync()
        {
            await _queueLock.WaitAsync();
            try
            {
                _convertQueue.Clear();
                _onChange.Invoke();
            }
            finally
            {
                _queueLock.Release();
            }
        }

        /// <summary>
        /// Gets the number of conversions in the queue.
        /// </summary>
        /// <returns>The number of conversions in the queue.</returns>
        public int Count()
        {
            return _convertQueue.Count;
        }

        /// <summary>
        /// Gets whether the queue is currently being processed.
        /// </summary>
        public bool IsProcessing => _isProcessing;

        /// <summary>
        /// Raises the ConversionStarted event.
        /// </summary>
        /// <param name="conversion">The conversion that started.</param>
        protected virtual void OnConversionStarted(Conversion conversion)
        {
            ConversionStarted?.Invoke(this, new ConversionEventArgs(conversion));
        }

        /// <summary>
        /// Raises the ConversionCompleted event.
        /// </summary>
        /// <param name="conversion">The conversion that completed.</param>
        protected virtual void OnConversionCompleted(Conversion conversion)
        {
            ConversionCompleted?.Invoke(this, new ConversionEventArgs(conversion));
        }

        /// <summary>
        /// Raises the ConversionFailed event.
        /// </summary>
        /// <param name="conversion">The conversion that failed.</param>
        protected virtual void OnConversionFailed(Conversion conversion)
        {
            ConversionFailed?.Invoke(this, new ConversionEventArgs(conversion));
        }

        /// <summary>
        /// Raises the ConversionCancelled event.
        /// </summary>
        /// <param name="conversion">The conversion that was cancelled.</param>
        protected virtual void OnConversionCancelled(Conversion conversion)
        {
            ConversionCancelled?.Invoke(this, new ConversionEventArgs(conversion));
        }
    }

    /// <summary>
    /// Provides data for conversion-related events.
    /// </summary>
    public class ConversionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the conversion associated with the event.
        /// </summary>
        public Conversion Conversion { get; }

        /// <summary>
        /// Initializes a new instance of the ConversionEventArgs class.
        /// </summary>
        /// <param name="conversion">The conversion associated with the event.</param>
        /// <exception cref="ArgumentNullException">Thrown when conversion is null.</exception>
        public ConversionEventArgs(Conversion conversion)
        {
            Conversion = conversion ?? throw new ArgumentNullException(nameof(conversion));
        }
    }
}