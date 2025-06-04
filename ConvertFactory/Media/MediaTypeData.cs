using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConvertFactory.Media
{
    /// <summary>
    /// Manages media type data and provides functionality for handling different media formats and their conversions.
    /// </summary>
    public class MediaTypeData
    {
        /// <summary>
        /// Gets the singleton instance of MediaTypeData.
        /// </summary>
        public static MediaTypeData Data { get; private set; }

        /// <summary>
        /// Lock object used for thread synchronization when accessing the Data property.
        /// </summary>
        private static readonly object DataLock = new object();

        /// <summary>
        /// JSON serializer options for handling media type data.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        
        /// <summary>
        /// Gets or sets the list of supported image conversion data.
        /// </summary>
        public List<ConversionData> Image { get; set; }

        /// <summary>
        /// Gets or sets the list of supported audio conversion data.
        /// </summary>
        public List<ConversionData> Audio { get; set; }

        /// <summary>
        /// Gets or sets the list of supported video conversion data.
        /// </summary>
        public List<ConversionData> Video { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the MediaTypeData class.
        /// </summary>
        public MediaTypeData()
        {
            Image = new List<ConversionData>();
            Audio = new List<ConversionData>();
            Video = new List<ConversionData>();
        }

        /// <summary>
        /// Gets the current media type data instance.
        /// </summary>
        /// <returns>The current MediaTypeData instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when media type data has not been loaded.</exception>
        public MediaTypeData GetData()
        {
            lock (DataLock)
            {
                if (Data == null)
                    throw new InvalidOperationException("Media type data has not been loaded");
                    
                return Data;
            }
        }

        /// <summary>
        /// Gets the conversion data for a specific file extension.
        /// </summary>
        /// <param name="extension">The file extension to look up.</param>
        /// <returns>The ConversionData for the specified extension, or null if not found.</returns>
        /// <exception cref="ArgumentException">Thrown when extension is null or empty.</exception>
        public ConversionData this[string extension]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(extension))
                    throw new ArgumentException("Extension cannot be null or empty", nameof(extension));

                extension = extension.ToLower();
                
                foreach (var i in Data.Image)
                {
                    if (i.Ext == extension)
                    {
                        return i;
                    }
                }

                foreach (var a in Data.Audio)
                {
                    if (a.Ext == extension)
                    {
                        return a;
                    }
                }

                foreach (var v in Data.Video)
                {
                    if (v.Ext == extension)
                    {
                        return v;
                    }
                }

                return null;
            }
        }
        
        /// <summary>
        /// Loads media type data from a JSON file asynchronously.
        /// </summary>
        /// <param name="file">The path to the JSON file containing media type data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when file path is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
        /// <exception cref="InvalidDataException">Thrown when the file is empty or contains invalid data.</exception>
        public static Task LoadDataAsync(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("File path cannot be null or empty", nameof(file));

            if (!File.Exists(file))
                throw new FileNotFoundException("Media type data file not found", file);

            try
            {
                string json = File.ReadAllText(file);
                if (string.IsNullOrWhiteSpace(json))
                    throw new InvalidDataException("Media type data file is empty");

                var mediaData = JsonSerializer.Deserialize<MediaTypeData>(json, JsonOptions);
                if (mediaData == null)
                    throw new InvalidDataException("Failed to deserialize media type data");

                ValidateMediaData(mediaData);

                lock (DataLock)
                {
                    Data = mediaData;
                }

                return Task.CompletedTask;
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException("Invalid JSON format in media type data file", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load media type data", ex);
            }
        }

        /// <summary>
        /// Validates the media type data for correctness and completeness.
        /// </summary>
        /// <param name="data">The media type data to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="InvalidDataException">Thrown when validation fails.</exception>
        private static void ValidateMediaData(MediaTypeData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var errors = new List<string>();

            if (data.Image != null)
            {
                foreach (var img in data.Image)
                {
                    if (string.IsNullOrWhiteSpace(img.Ext))
                        errors.Add("Image format extension cannot be empty");
                    if (img.To == null || !img.To.Any())
                        errors.Add($"No conversion targets specified for image format {img.Ext}");
                }
            }

            if (data.Audio != null)
            {
                foreach (var aud in data.Audio)
                {
                    if (string.IsNullOrWhiteSpace(aud.Ext))
                        errors.Add("Audio format extension cannot be empty");
                    if (aud.To == null || !aud.To.Any())
                        errors.Add($"No conversion targets specified for audio format {aud.Ext}");
                }
            }

            if (data.Video != null)
            {
                foreach (var vid in data.Video)
                {
                    if (string.IsNullOrWhiteSpace(vid.Ext))
                        errors.Add("Video format extension cannot be empty");
                    if (vid.To == null || !vid.To.Any())
                        errors.Add($"No conversion targets specified for video format {vid.Ext}");
                }
            }

            if (errors.Any())
                throw new InvalidDataException($"Media type data validation failed:\n{string.Join("\n", errors)}");
        }
        
        /// <summary>
        /// Gets all supported media type extensions.
        /// </summary>
        /// <returns>An array of all supported file extensions.</returns>
        /// <exception cref="InvalidOperationException">Thrown when media type data has not been loaded.</exception>
        public static string[] GetAllMediaTypes()
        {
            lock (DataLock)
            {
                if (Data == null)
                    throw new InvalidOperationException("Media type data has not been loaded");

                var mediaTypes = new HashSet<string>();

                if (Data.Image != null)
                    foreach (var img in Data.Image)
                        mediaTypes.Add(img.Ext);
                    
                if (Data.Audio != null)
                    foreach (var aud in Data.Audio)
                        mediaTypes.Add(aud.Ext);
                    
                if (Data.Video != null)
                    foreach (var vid in Data.Video)
                        mediaTypes.Add(vid.Ext);

                return mediaTypes.ToArray();
            }
        }
        
        /// <summary>
        /// Builds a filter string for file dialogs based on supported media types.
        /// </summary>
        /// <returns>A filter string for use in file dialogs.</returns>
        /// <exception cref="InvalidOperationException">Thrown when media type data has not been loaded.</exception>
        public static string BuildFilter()
        {
            if (Data == null)
                throw new InvalidOperationException("Media type data has not been loaded");

            var allPatterns = new HashSet<string>();
            var filterParts = new List<string>();

            if (Data.Image != null && Data.Image.Any())
            {
                var imagePatterns = Data.Image.Select(i => $"*{i.Ext.ToLower()}");
                var imagePattern = string.Join(";", imagePatterns);
                filterParts.Add($"Image Files ({imagePattern})|{imagePattern}");
                allPatterns.UnionWith(imagePatterns);
            }

            if (Data.Audio != null && Data.Audio.Any())
            {
                var audioPatterns = Data.Audio.Select(a => $"*{a.Ext.ToLower()}");
                var audioPattern = string.Join(";", audioPatterns);
                filterParts.Add($"Audio Files ({audioPattern})|{audioPattern}");
                allPatterns.UnionWith(audioPatterns);
            }

            if (Data.Video != null && Data.Video.Any())
            {
                var videoPatterns = Data.Video.Select(v => $"*{v.Ext.ToLower()}");
                var videoPattern = string.Join(";", videoPatterns);
                filterParts.Add($"Video Files ({videoPattern})|{videoPattern}");
                allPatterns.UnionWith(videoPatterns);
            }

            if (allPatterns.Any())
            {
                var allPattern = string.Join(";", allPatterns);
                filterParts.Add($"All Media Files ({allPattern})|{allPattern}");
            }

            return string.Join("|", filterParts);
        }

        /// <summary>
        /// Checks if a given file extension is a supported media type.
        /// </summary>
        /// <param name="extension">The file extension to check.</param>
        /// <returns>True if the extension is supported, false otherwise.</returns>
        public static bool IsValidMediaType(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return false;

            extension = extension.ToLower();
            
            lock (DataLock)
            {
                if (Data == null)
                    return false;

                return (Data.Image?.Any(i => i.Ext == extension) == true) ||
                       (Data.Audio?.Any(a => a.Ext == extension) == true) ||
                       (Data.Video?.Any(v => v.Ext == extension) == true);
            }
        }

        /// <summary>
        /// Gets the list of supported conversion formats for a given file extension.
        /// </summary>
        /// <param name="extension">The file extension to get supported conversions for.</param>
        /// <returns>An array of supported conversion formats.</returns>
        /// <exception cref="ArgumentException">Thrown when extension is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when media type data has not been loaded.</exception>
        public static string[] GetSupportedConversions(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException("Extension cannot be null or empty", nameof(extension));

            extension = extension.ToLower();
            
            lock (DataLock)
            {
                if (Data == null)
                    throw new InvalidOperationException("Media type data has not been loaded");

                var data = Data[extension];
                return data?.To.ToArray() ?? Array.Empty<string>();
            }
        }
    }
}