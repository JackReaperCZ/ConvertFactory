using System.ComponentModel;

namespace ConvertFactory
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._queuePanel = new System.Windows.Forms.Panel();
            this._selectFilesLabel = new System.Windows.Forms.Label();
            this._constrollPanel = new System.Windows.Forms.Panel();
            this._settingsButton = new System.Windows.Forms.Button();
            this._runButton = new System.Windows.Forms.Button();
            this._queuePanel.SuspendLayout();
            this._constrollPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _queuePanel
            // 
            this._queuePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this._queuePanel.AutoScroll = true;
            this._queuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._queuePanel.Controls.Add(this._selectFilesLabel);
            this._queuePanel.Location = new System.Drawing.Point(247, 12);
            this._queuePanel.Name = "_queuePanel";
            this._queuePanel.Size = new System.Drawing.Size(734, 737);
            this._queuePanel.TabIndex = 0;
            // 
            // _selectFilesLabel
            // 
            this._selectFilesLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._selectFilesLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._selectFilesLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this._selectFilesLabel.Location = new System.Drawing.Point(290, 291);
            this._selectFilesLabel.Name = "_selectFilesLabel";
            this._selectFilesLabel.Size = new System.Drawing.Size(150, 150);
            this._selectFilesLabel.TabIndex = 1;
            this._selectFilesLabel.Text = "Select files";
            this._selectFilesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _constrollPanel
            // 
            this._constrollPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this._constrollPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._constrollPanel.Controls.Add(this._settingsButton);
            this._constrollPanel.Controls.Add(this._runButton);
            this._constrollPanel.Location = new System.Drawing.Point(12, 12);
            this._constrollPanel.Name = "_constrollPanel";
            this._constrollPanel.Size = new System.Drawing.Size(229, 736);
            this._constrollPanel.TabIndex = 1;
            // 
            // _settingsButton
            // 
            this._settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._settingsButton.Location = new System.Drawing.Point(3, 694);
            this._settingsButton.Name = "_settingsButton";
            this._settingsButton.Size = new System.Drawing.Size(219, 35);
            this._settingsButton.TabIndex = 1;
            this._settingsButton.Text = "SETTINGS";
            this._settingsButton.UseVisualStyleBackColor = true;
            // 
            // _runButton
            // 
            this._runButton.Enabled = false;
            this._runButton.Location = new System.Drawing.Point(3, 3);
            this._runButton.Name = "_runButton";
            this._runButton.Size = new System.Drawing.Size(219, 35);
            this._runButton.TabIndex = 0;
            this._runButton.Text = "RUN QUERY";
            this._runButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(993, 761);
            this.Controls.Add(this._constrollPanel);
            this.Controls.Add(this._queuePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(15, 15);
            this.MinimumSize = new System.Drawing.Size(800, 800);
            this.Name = "MainForm";
            this.Text = "Convert Factory";
            this._queuePanel.ResumeLayout(false);
            this._constrollPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button _settingsButton;

        private System.Windows.Forms.Button _runButton;

        private System.Windows.Forms.Panel _constrollPanel;

        private System.Windows.Forms.Label _selectFilesLabel;

        private System.Windows.Forms.Panel _queuePanel;

        #endregion
    }
}