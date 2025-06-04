using System;
using System.Windows.Forms;

namespace ConvertFactory
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public void UpdateProgress(int percentage, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateProgress(percentage, message)));
            }
            else
            {
                progressBar.Value = percentage;
                lblStatus.Text = message;
            }
        }

    }
}