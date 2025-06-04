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
            progressBar.Value = percentage;
            lblStatus.Text = message;
        }
    }
}