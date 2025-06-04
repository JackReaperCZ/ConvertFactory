using System;
using System.Configuration;

/// <summary>
/// Manages application settings and provides access to configuration values.
/// </summary>
public static class AppSettingsManager
{
    /// <summary>
    /// Lock object used for thread synchronization when updating settings.
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    /// Gets or sets the default output path for converted files.
    /// </summary>
    /// <remarks>
    /// Returns an empty string if the setting is not found or if there's an error reading the configuration.
    /// </remarks>
    public static string DefaultOutputPath
    {
        get
        {
            try
            {
                return ConfigurationManager.AppSettings["DefaultOutputPath"] ?? string.Empty;
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.Error.WriteLine($"Error reading DefaultOutputPath: {ex.Message}");
                return string.Empty;
            }
        }
        set
        {
            UpdateAppSetting("DefaultOutputPath", value);
        }
    }

    /// <summary>
    /// Gets or sets whether music should be played during query operations.
    /// </summary>
    /// <remarks>
    /// Returns false if the setting is not found or if there's an error reading the configuration.
    /// </remarks>
    public static bool PlayMusicDuringQuery
    {
        get
        {
            try
            {
                string value = ConfigurationManager.AppSettings["PlayMusicDuringQuery"];
                return bool.TryParse(value, out bool result) && result;
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.Error.WriteLine($"Error reading PlayMusicDuringQuery: {ex.Message}");
                return false;
            }
        }
        set
        {
            UpdateAppSetting("PlayMusicDuringQuery", value.ToString().ToLower());
        }
    }

    /// <summary>
    /// Updates an application setting in the configuration file.
    /// </summary>
    /// <param name="key">The key of the setting to update.</param>
    /// <param name="value">The new value for the setting.</param>
    /// <remarks>
    /// This method is thread-safe and handles both updating existing settings and adding new ones.
    /// </remarks>
    private static void UpdateAppSetting(string key, string value)
    {
        lock (_lock)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;

                if (settings[key] != null)
                {
                    settings[key].Value = value;
                }
                else
                {
                    settings.Add(key, value);
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.Error.WriteLine($"Configuration error updating key '{key}': {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error updating key '{key}': {ex.Message}");
            }
        }
    }
}
