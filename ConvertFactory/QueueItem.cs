using System;
using System.IO;
using System.Windows.Forms;
using ConvertFactory.Media;

namespace ConvertFactory
{
    /// <summary>
    /// Represents a single item in the conversion queue with its associated controls and functionality.
    /// </summary>
    public partial class QueueItem : UserControl
    {
        /// <summary>
        /// The conversion operation associated with this queue item.
        /// </summary>
        private Conversion _conversion;

        /// <summary>
        /// Flag to prevent recursive events in the folder combo box.
        /// </summary>
        private bool _suppressFolderComboEvent = false;
        
        /// <summary>
        /// Label for the default folder option.
        /// </summary>
        private const string DefaultFolderLabel = "Default";

        /// <summary>
        /// Label for the browse folder option.
        /// </summary>
        private const string BrowseFolderLabel = "Browse";

        /// <summary>
        /// Initializes a new instance of the QueueItem class.
        /// </summary>
        /// <param name="conversion">The conversion operation to associate with this queue item.</param>
        public QueueItem(Conversion conversion)
        {
            InitializeComponent();
            _conversion = conversion;
            
            _filePath.Text = _conversion.FilePath;

            _outputFormatComboBox.Items.AddRange(_conversion.ConvertTo);
            _outputFormatComboBox.SelectedItem = Path.GetExtension(_conversion.FilePath);
            _outputFormatComboBox.SelectedIndexChanged += OutputFormatComboBox_SelectedIndexChanged;

            _conversion.OutputFormat = Path.GetExtension(_conversion.FilePath);
            _extencionLabel.Text = _conversion.OutputFormat.ToUpper();

            _outputFolderComboBox.Items.AddRange(new[] { DefaultFolderLabel, BrowseFolderLabel });
            _outputFolderComboBox.SelectedItem = DefaultFolderLabel;
            _outputFolderComboBox.SelectedIndexChanged += OutputFolderComboBox_SelectedIndexChanged;

            _removeButton.Click += RemoveButtonClick_SelectedIndexChanged;

            this.Dock = DockStyle.Top;
            _filePath.Click += filePath_Click;
        }

        /// <summary>
        /// Removes this item from the queue and disposes of its resources.
        /// </summary>
        public async void Remove()
        {
            if (Parent != null)
            {
                Parent.Controls.Remove(this);
                await _conversion.Manager.RemoveAsync(_conversion);
                Dispose();
            }
        }

        /// <summary>
        /// Updates the data associated with this queue item.
        /// </summary>
        /// <param name="file">The new file path to update to.</param>
        private async void UpdateData(string file)
        {
            _filePath.Text = file;
            string previousOutputDirectory = _conversion.OutputDirectory;

            await _conversion.Manager.RemoveAsync(_conversion);
            _conversion = Conversion.Create(file, _conversion.GetManager());
            await _conversion.Manager.AddAsync(_conversion);

            _outputFormatComboBox.Items.Clear();
            _outputFormatComboBox.Items.AddRange(_conversion.ConvertTo);
            _outputFormatComboBox.SelectedItem = Path.GetExtension(file);

            _conversion.OutputFormat = Path.GetExtension(file);
            _extencionLabel.Text = _conversion.OutputFormat.ToUpper();

            _outputFolderComboBox.Items.Clear();
            _outputFolderComboBox.Items.AddRange(new[] { DefaultFolderLabel, BrowseFolderLabel });

            if (!string.IsNullOrEmpty(previousOutputDirectory))
            {
                _outputFolderComboBox.Items.Add(previousOutputDirectory);
                _outputFolderComboBox.SelectedItem = previousOutputDirectory;
                _conversion.OutputDirectory = previousOutputDirectory;
            }
            else
            {
                _outputFolderComboBox.SelectedItem = DefaultFolderLabel;
                _conversion.OutputDirectory = null;
            }
        }

        /// <summary>
        /// Event handler for when the output folder selection changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        private void OutputFolderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressFolderComboEvent) return;

            if (!(sender is ComboBox comboBox) || comboBox.SelectedItem == null)
                return;

            string selected = comboBox.SelectedItem.ToString();

            if (selected != BrowseFolderLabel && selected != DefaultFolderLabel && !Directory.Exists(selected))
                return;

            if (selected == BrowseFolderLabel)
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        _suppressFolderComboEvent = true;

                        _outputFolderComboBox.Items.Clear();
                        _outputFolderComboBox.Items.AddRange(new[] { DefaultFolderLabel, BrowseFolderLabel });

                        _conversion.OutputDirectory = folderDialog.SelectedPath;
                        _outputFolderComboBox.Items.Add(folderDialog.SelectedPath);
                        _outputFolderComboBox.SelectedItem = folderDialog.SelectedPath;

                        _suppressFolderComboEvent = false;

                        Console.WriteLine(_conversion.ToString()); // Debug
                        return;
                    }

                    _suppressFolderComboEvent = true;
                    _outputFolderComboBox.SelectedItem = DefaultFolderLabel;
                    _suppressFolderComboEvent = false;
                    return;
                }
            }

            _suppressFolderComboEvent = true;
            _outputFolderComboBox.Items.Clear();
            _outputFolderComboBox.Items.AddRange(new[] { DefaultFolderLabel, BrowseFolderLabel });
            _outputFolderComboBox.SelectedItem = DefaultFolderLabel;
            _conversion.OutputDirectory = null;
            _suppressFolderComboEvent = false;

            Console.WriteLine(_conversion.ToString()); // Debug
        }

        /// <summary>
        /// Event handler for when the remove button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        private void RemoveButtonClick_SelectedIndexChanged(object sender, EventArgs e)
        {
            Remove();
        }

        /// <summary>
        /// Event handler for when the output format selection changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        private void OutputFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                _conversion.OutputFormat = comboBox.SelectedItem.ToString();
                Console.WriteLine(_conversion.ToString()); // Debug
            }
        }

        /// <summary>
        /// Event handler for when the file path is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        private void filePath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = MediaTypeData.BuildFilter();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    UpdateData(ofd.FileName);
                }
            }
        }
    }
}
