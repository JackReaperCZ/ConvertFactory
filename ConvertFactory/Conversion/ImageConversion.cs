using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConvertFactory.Media;
using Xabe.FFmpeg;

namespace ConvertFactory
{
    /// <summary>
    /// Handles the conversion of image files using FFmpeg.
    /// </summary>
    public class ImageConversion : Conversion
    {
        /// <summary>
        /// Initializes a new instance of the ImageConversion class.
        /// </summary>
        /// <param name="file">The path to the image file to convert.</param>
        /// <param name="manager">The conversion manager handling this conversion.</param>
        public ImageConversion(string file, ConvertManager manager) : base(file, manager)
        {
            string extension = Path.GetExtension(file)?.ToLower();
            var data = MediaTypeData.Data;
            ConvertTo = data.Image.First(i => i.Ext == extension).To.ToArray();
        }

        /// <summary>
        /// Performs the image conversion operation asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous conversion operation.</returns>
        public override async Task Convert()
        {
            ProgressBar.Value = 0;
            
            if (string.IsNullOrWhiteSpace(OutputFormat))
            {
                Console.WriteLine("OutputFormat is not set.");
                return;
            }

            string outputDir = string.IsNullOrWhiteSpace(OutputDirectory)
                ? AppSettingsManager.DefaultOutputPath
                : OutputDirectory;

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            
            string outputFile = Path.Combine(
                outputDir,
                Path.GetFileNameWithoutExtension(FilePath) + OutputFormat
            );

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var conversion = await FFmpeg.Conversions.FromSnippet.Convert(FilePath, outputFile);

            conversion.OnProgress += (sender, args) =>
            {
                ProgressBar.Value = args.Percent;
            };
            
            await conversion.Start();
            
            ProgressBar.Value = 100;
        }
    }
}