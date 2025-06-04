using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using ConvertFactory.Media;

namespace ConvertFactory
{
    /// <summary>
    /// The main form of the application that handles the user interface and file conversion operations.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Array of file extensions that are allowed for conversion.
        /// </summary>
        string[] allowedExtensions = MediaTypeData.GetAllMediaTypes();

        /// <summary>
        /// The manager that handles the conversion queue and operations.
        /// </summary>
        private ConvertManager _convertManager;
        
        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            
            _convertManager = new ConvertManager(OnChangeEventHandler);
            
            _queuePanel.AllowDrop = true;
            _queuePanel.DragEnter += QueuePanel_DragEnter;
            _queuePanel.DragDrop += QueuePanel_DragDrop;

            _selectFilesLabel.Click += SelectFilesLabel_Click;

            _runButton.Click += RunButton_Click;
            _settingsButton.Click += SettingsButton_Click;
        }

        /// <summary>
        /// Event handler for changes in the conversion queue.
        /// </summary>
        private void OnChangeEventHandler()
        {
            if (_convertManager.Count() == 0)
            {
                _selectFilesLabel.Visible = true;
                _runButton.Enabled = false;
            }
        }

        /// <summary>
        /// Adds a file to the conversion queue.
        /// </summary>
        /// <param name="file">The path to the file to be added to the queue.</param>
        private async void AddToQueue(string file)
        {
            try
            {
                Conversion conversion = Conversion.Create(file, _convertManager);
                if (conversion == null) return;
                
                if (_convertManager.Count() == 0)
                {
                    _selectFilesLabel.Visible = false;
                    _runButton.Enabled = true;
                }
                
                await _convertManager.AddAsync(conversion);

                var item = new QueueItem(conversion);
                conversion.ProgressBar = item._progressBar;
                
                _queuePanel.Controls.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding file to queue: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler for the settings button click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new SettingsDialog())
            {
                dlg.ShowDialog(this);
            }
        }
        
        /// <summary>
        /// Event handler for the run button click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        private async void RunButton_Click(object sender, EventArgs e)
        {
            _runButton.Enabled = false;
            _queuePanel.Enabled = false;

            try
            {
                await _convertManager.RunQueueAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _queuePanel.Enabled = true;
                _runButton.Enabled = true;

                if (AppSettingsManager.PlayMusicDuringQuery)
                {
                    PlayFinnishAudio();
                }
            }
        }

        /// <summary>
        /// Event handler for the select files label click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        private void SelectFilesLabel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = MediaTypeData.BuildFilter();

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        AddToQueue(file);
                    }
                }
            }
        }

        /// <summary>
        /// Plays a sound when the conversion is complete.
        /// </summary>
        private void PlayFinnishAudio()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(@"./sound.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing sound: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Event handler for when files are dragged over the queue panel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A DragEventArgs that contains the event data.</param>
        private void QueuePanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                bool isValidFile = false;
                foreach (var file in files)
                {
                    string extension = Path.GetExtension(file).ToLower();
                    if (Array.Exists(allowedExtensions, ext => ext == extension))
                    {
                        isValidFile = true;
                        break;
                    }
                }

                if (isValidFile)
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            
            e.Effect = DragDropEffects.None;
        }
        
        /// <summary>
        /// Event handler for when files are dropped onto the queue panel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A DragEventArgs that contains the event data.</param>
        private void QueuePanel_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (var file in files)
            {
                AddToQueue(file);
            }
        }
    }
}