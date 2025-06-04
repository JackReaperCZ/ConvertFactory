using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConvertFactory.Media;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace ConvertFactory
{
    /// <summary>
    /// The main entry point class for the application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Run the async initialization
                Task.Run(async () => await InitializeAsync()).GetAwaiter().GetResult();

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Application failed to start:\n{ex.Message}",
                    "Startup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Initializes the application asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when initialization fails.</exception>
        private static async Task InitializeAsync()
        {
            try
            {
                await SetupFFmpegAsync();
                await LoadMediaTypeDataAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize application", ex);
            }
        }

        /// <summary>
        /// Sets up FFmpeg by downloading and configuring the necessary binaries.
        /// </summary>
        /// <returns>A task representing the asynchronous setup operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when FFmpeg setup fails.</exception>
        private static async Task SetupFFmpegAsync()
        {
            try
            {
                ProgressForm progressForm = null;

                var formThread = new System.Threading.Thread(() =>
                {
                    progressForm = new ProgressForm();
                    Application.Run(progressForm);
                });

                formThread.SetApartmentState(System.Threading.ApartmentState.STA);
                formThread.Start();

                // Wait for the form to be created
                while (progressForm == null || !progressForm.IsHandleCreated)
                {
                    await Task.Delay(100);
                }

                var progress = new Progress<ProgressInfo>(info =>
                {
                    int percent = info.TotalBytes > 0
                        ? (int)(info.DownloadedBytes * 100 / info.TotalBytes)
                        : 0;

                    progressForm?.Invoke(new Action(() =>
                    {
                        progressForm.UpdateProgress(percent, $"Downloading FFmpeg... {percent}%");
                    }));
                });

                await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, progress);

                FFmpeg.SetExecutablesPath(Xabe.FFmpeg.FFmpeg.ExecutablesPath);

                progressForm?.Invoke(new Action(() => progressForm.Close()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading FFmpeg:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the media type data from the configuration file.
        /// </summary>
        /// <returns>A task representing the asynchronous loading operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when media type data loading fails.</exception>
        private static async Task LoadMediaTypeDataAsync()
        {
            try
            {
                Console.WriteLine("Loading conversion rules...");
                await MediaTypeData.LoadDataAsync("media-type-data.json");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load media type data", ex);
            }
        }
    }
}