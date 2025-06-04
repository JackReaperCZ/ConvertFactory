using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConvertFactory.Media;

namespace ConvertFactory
{
    /// <summary>
    /// Base class for all media conversion operations.
    /// </summary>
    public abstract class Conversion
    {
        /// <summary>
        /// Gets or sets the path to the file being converted.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the desired output format for the conversion.
        /// </summary>
        public string OutputFormat { get; set; }

        /// <summary>
        /// Gets or sets the directory where the converted file will be saved.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets the list of formats this file can be converted to.
        /// </summary>
        public string[] ConvertTo { get; set; }

        /// <summary>
        /// Gets the conversion manager associated with this conversion.
        /// </summary>
        public ConvertManager Manager { get; }

        /// <summary>
        /// Gets the current status of the conversion.
        /// </summary>
        public ConversionStatus Status { get; protected set; }

        /// <summary>
        /// Gets any error message associated with the conversion.
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Gets the time when the conversion started.
        /// </summary>
        public DateTime StartTime { get; protected set; }

        /// <summary>
        /// Gets the time when the conversion ended, if completed.
        /// </summary>
        public DateTime? EndTime { get; protected set; }

        /// <summary>
        /// Gets the duration of the conversion, if completed.
        /// </summary>
        public TimeSpan? Duration => EndTime?.Subtract(StartTime);

        /// <summary>
        /// Performs the actual conversion operation.
        /// </summary>
        /// <returns>A task representing the asynchronous conversion operation.</returns>
        public abstract Task Convert();

        /// <summary>
        /// Gets or sets the progress bar associated with this conversion.
        /// </summary>
        public ProgressBar ProgressBar { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the Conversion class.
        /// </summary>
        /// <param name="file">The path to the file to be converted.</param>
        /// <param name="manager">The conversion manager handling this conversion.</param>
        /// <exception cref="ArgumentNullException">Thrown when file or manager is null.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the source file does not exist.</exception>
        protected Conversion(string file, ConvertManager manager)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentNullException(nameof(file), "File path cannot be null or empty");
            
            if (!File.Exists(file))
                throw new FileNotFoundException("Source file not found", file);
                
            if (manager == null)
                throw new ArgumentNullException(nameof(manager), "Convert manager cannot be null");

            FilePath = file;
            Manager = manager;
            Status = ConversionStatus.Pending;
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// Gets the conversion manager associated with this conversion.
        /// </summary>
        /// <returns>The ConvertManager instance.</returns>
        public ConvertManager GetManager()
        {
            return Manager;
        }
        
        /// <summary>
        /// Creates a new conversion instance based on the file type.
        /// </summary>
        /// <param name="file">The path to the file to be converted.</param>
        /// <param name="manager">The conversion manager to handle the conversion.</param>
        /// <returns>A new Conversion instance appropriate for the file type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when file is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when file has no extension.</exception>
        /// <exception cref="InvalidOperationException">Thrown when media type data is not loaded.</exception>
        /// <exception cref="NotSupportedException">Thrown when the file type is not supported.</exception>
        public static Conversion Create(string file, ConvertManager manager)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file))
                    throw new ArgumentNullException(nameof(file), "File path cannot be null or empty");

                string extension = Path.GetExtension(file)?.ToLower();
                if (string.IsNullOrWhiteSpace(extension))
                    throw new ArgumentException("File must have an extension", nameof(file));

                var data = MediaTypeData.Data;
                if (data == null)
                    throw new InvalidOperationException("Media type data has not been loaded");
                
                if (data.Image?.Any(i => i.Ext == extension) == true)
                    return new ImageConversion(file, manager);
                    
                if (data.Video?.Any(v => v.Ext == extension) == true)
                    return new VideoConversion(file, manager);
                    
                if (data.Audio?.Any(a => a.Ext == extension) == true)
                    return new AudioConversion(file, manager);
                
                throw new NotSupportedException($"Unsupported file type: {extension}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating conversion for file {file}: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Updates the status of the conversion.
        /// </summary>
        /// <param name="newStatus">The new status to set.</param>
        /// <param name="errorMessage">Optional error message if the status indicates failure.</param>
        public void UpdateStatus(ConversionStatus newStatus, string errorMessage = null)
        {
            Status = newStatus;
            ErrorMessage = errorMessage;
            
            if (newStatus == ConversionStatus.Completed || newStatus == ConversionStatus.Failed)
            {
                EndTime = DateTime.Now;
            }
        }
        
        /// <summary>
        /// Validates that the output path exists and creates it if necessary.
        /// </summary>
        /// <param name="outputPath">The path to validate.</param>
        /// <exception cref="ArgumentException">Thrown when outputPath is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when directory creation fails.</exception>
        protected void ValidateOutputPath(string outputPath)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Output path cannot be null or empty");

            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create output directory: {ex.Message}", ex);
                }
            }
        }
        
        /// <summary>
        /// Returns a string representation of the conversion.
        /// </summary>
        /// <returns>A string containing the conversion details.</returns>
        public override string ToString()
        {
            return $"File: {FilePath}, Output Format: {OutputFormat}, Output Directory: {OutputDirectory}, Status: {Status}, Duration: {Duration?.TotalSeconds:F2}s";
        }
    }

    /// <summary>
    /// Represents the possible states of a conversion operation.
    /// </summary>
    public enum ConversionStatus
    {
        /// <summary>
        /// The conversion is waiting to be processed.
        /// </summary>
        Pending,

        /// <summary>
        /// The conversion has completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// The conversion failed to complete.
        /// </summary>
        Failed,

        /// <summary>
        /// The conversion was cancelled.
        /// </summary>
        Cancelled
    }
}