using System;
using System.IO;
using System.Windows.Forms;

namespace ConvertFactory
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();
            
            txtOutputPath.Text = AppSettingsManager.DefaultOutputPath;
            chkPlayMusic.Checked = AppSettingsManager.PlayMusicDuringQuery;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select default output folder";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtOutputPath.Text))
            {
                MessageBox.Show("Please enter a valid output directory.", "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save settings
            AppSettingsManager.DefaultOutputPath = txtOutputPath.Text;
            AppSettingsManager.PlayMusicDuringQuery = chkPlayMusic.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}