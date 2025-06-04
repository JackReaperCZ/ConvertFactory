# 🎥 Convert Factory

Convert Factory is media conversion application that allows users to convert various types of media files (images, audio, and video) using FFmpeg. The application provides a user-friendly interface for managing conversion queues and customizing output settings.

## ✨ Features

- Support for multiple media types:
  - 🖼️ Images (various formats)
  - 🎵 Audio files
  - 🎬 Video files
- 📥 Drag and drop interface
- 📦 Batch conversion support
- 📁 Custom output directory selection
- 📊 Progress tracking
- 📋 Conversion queue management
- ⚙️ Configurable settings
- 🔔 Audio notification on completion

## 💻 System Requirements

- Windows 10 or later
- .NET Framework 4.8.1
- FFmpeg (automatically downloaded and configured by the application)
- Minimum 4GB RAM recommended
- 500MB free disk space for FFmpeg binaries

## 📥 Installation

1. Download the latest release from the releases page
2. Extract the ZIP file to your desired location
3. Run `ConvertFactory.exe`

The application will automatically download and configure FFmpeg on first run.

## 📖 User Manual

### 🚀 Getting Started

1. Launch the application
2. The main window will appear with a queue panel on the right and control panel on the left

### 📂 Adding Files to Convert

There are three ways to add files to the conversion queue:

1. **Drag and Drop**:
   - Simply drag files from Windows Explorer and drop them onto the queue panel
   - Only supported file types will be accepted

2. **File Selection Dialog**:
   - Click the "Select files" label in the queue panel
   - Use the file dialog to select one or multiple files
   - Click "Open" to add them to the queue

3. **Click to Change**:
   - For files already in the queue, click on the file path to change the source file

### ⚙️ Configuring Conversion Settings

For each file in the queue, you can configure:

1. **Output Format**:
   - Select the desired output format from the dropdown menu
   - Available formats depend on the input file type

2. **Output Directory**:
   - Choose between:
     - Default: Uses the default output path from settings
     - Browse: Select a custom output directory
     - Previously used directories (if any)

### 📋 Managing the Queue

- **Remove Items**: Click the "X" button on any queue item to remove it
- **Run Queue**: Click the "RUN QUERY" button to start processing all items
- **Progress**: Each item shows a progress bar during conversion
- **Status**: The status of each conversion is displayed in the queue item

### ⚙️ Application Settings

Access settings by clicking the "SETTINGS" button:

1. **Default Output Path**:
   - Set the default directory for converted files
   - Click "Browse" to select a directory

2. **Audio Notification**:
   - Toggle whether to play a sound when conversion completes
   - Requires `sound.wav` in the application directory

### 💡 Tips and Best Practices

1. **Batch Processing**:
   - Add multiple files of the same type for batch conversion
   - All files will use the same output format and directory settings

2. **Output Organization**:
   - Use the default output path for consistent file organization
   - Use custom directories for specific projects or file types

3. **Performance**:
   - The application processes conversions in parallel
   - System performance may affect conversion speed
   - Large files may take longer to process

## 🔧 Technical Documentation

### 📁 Project Structure

```
ConvertFactory/
├── Conversion/
│   ├── AudioConversion.cs
│   ├── ImageConversion.cs
│   ├── VideoConversion.cs
│   ├── Conversion.cs
│   └── ConvertManager.cs
├── Media/
│   ├── ConversionData.cs
│   └── MediaTypeData.cs
├── AppSettingsManager.cs
├── MainForm.cs
├── Program.cs
├── QueueItem.cs
└── media-type-data.json
```

### 🏗️ Key Components

1. **Conversion Classes**:
   - `Conversion`: Base class for all conversion types
   - `AudioConversion`: Handles audio file conversions
   - `ImageConversion`: Handles image file conversions
   - `VideoConversion`: Handles video file conversions

2. **Media Management**:
   - `MediaTypeData`: Manages supported media types and conversion rules
   - `ConversionData`: Represents conversion data for specific media types

3. **Queue Management**:
   - `ConvertManager`: Manages the conversion queue and process
   - `QueueItem`: UI component for queue items

4. **Settings**:
   - `AppSettingsManager`: Manages application settings

### ⚙️ Configuration

The application uses a JSON file (`media-type-data.json`) to define supported media types and their conversion options. The file structure is:

```json
{
  "Image": [
    {
      "Ext": ".jpg",
      "To": [".png", ".gif", ".bmp"]
    }
  ],
  "Audio": [
    {
      "Ext": ".mp3",
      "To": [".wav", ".ogg", ".flac"]
    }
  ],
  "Video": [
    {
      "Ext": ".mp4",
      "To": [".avi", ".mkv", ".webm"]
    }
  ]
}
```

## 🔍 Troubleshooting

### 🚨 Common Issues

1. **FFmpeg Download Fails**:
   - Check internet connection
   - Ensure sufficient disk space
   - Run application as administrator

2. **Conversion Fails**:
   - Verify input file is not corrupted
   - Check available disk space
   - Ensure output directory is writable

3. **Application Crashes**:
   - Check system requirements
   - Verify .NET Framework installation
   - Check application logs

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- FFmpeg for the conversion engine
- Xabe.FFmpeg for the .NET wrapper
- All contributors and users of the application 