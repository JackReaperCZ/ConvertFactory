using System.ComponentModel;

namespace ConvertFactory
{
    partial class QueueItem
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._mainPanel = new System.Windows.Forms.Panel();
            this._removeButton = new System.Windows.Forms.Button();
            this._inputPanel = new System.Windows.Forms.Panel();
            this._extencionLabel = new System.Windows.Forms.Label();
            this._filePath = new System.Windows.Forms.Label();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._outputPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._outputFormatComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._outputFolderComboBox = new System.Windows.Forms.ComboBox();
            this._mainPanel.SuspendLayout();
            this._inputPanel.SuspendLayout();
            this._outputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainPanel
            // 
            this._mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this._mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._mainPanel.Controls.Add(this._removeButton);
            this._mainPanel.Controls.Add(this._inputPanel);
            this._mainPanel.Controls.Add(this._outputPanel);
            this._mainPanel.Location = new System.Drawing.Point(3, 3);
            this._mainPanel.Name = "_mainPanel";
            this._mainPanel.Size = new System.Drawing.Size(842, 135);
            this._mainPanel.TabIndex = 0;
            // 
            // _removeButton
            // 
            this._removeButton.ForeColor = System.Drawing.Color.DarkRed;
            this._removeButton.Location = new System.Drawing.Point(1, 6);
            this._removeButton.Name = "_removeButton";
            this._removeButton.Size = new System.Drawing.Size(35, 35);
            this._removeButton.TabIndex = 9;
            this._removeButton.Text = "X";
            this._removeButton.UseVisualStyleBackColor = true;
            // 
            // _inputPanel
            // 
            this._inputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this._inputPanel.Controls.Add(this._extencionLabel);
            this._inputPanel.Controls.Add(this._filePath);
            this._inputPanel.Controls.Add(this._progressBar);
            this._inputPanel.Location = new System.Drawing.Point(39, 4);
            this._inputPanel.Name = "_inputPanel";
            this._inputPanel.Size = new System.Drawing.Size(558, 126);
            this._inputPanel.TabIndex = 8;
            // 
            // _extencionLabel
            // 
            this._extencionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._extencionLabel.BackColor = System.Drawing.SystemColors.ControlLight;
            this._extencionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._extencionLabel.Location = new System.Drawing.Point(480, 8);
            this._extencionLabel.Name = "_extencionLabel";
            this._extencionLabel.Size = new System.Drawing.Size(66, 22);
            this._extencionLabel.TabIndex = 2;
            this._extencionLabel.Text = "EXT";
            this._extencionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _filePath
            // 
            this._filePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this._filePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._filePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this._filePath.Location = new System.Drawing.Point(3, 8);
            this._filePath.Name = "_filePath";
            this._filePath.Size = new System.Drawing.Size(471, 22);
            this._filePath.TabIndex = 1;
            this._filePath.Text = "FILE_PATH";
            this._filePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this._progressBar.Location = new System.Drawing.Point(3, 91);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(543, 30);
            this._progressBar.TabIndex = 0;
            this._progressBar.UseWaitCursor = true;
            // 
            // _outputPanel
            // 
            this._outputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._outputPanel.Controls.Add(this.label3);
            this._outputPanel.Controls.Add(this.label2);
            this._outputPanel.Controls.Add(this._outputFormatComboBox);
            this._outputPanel.Controls.Add(this.label1);
            this._outputPanel.Controls.Add(this._outputFolderComboBox);
            this._outputPanel.Location = new System.Drawing.Point(604, 4);
            this._outputPanel.Name = "_outputPanel";
            this._outputPanel.Size = new System.Drawing.Size(233, 127);
            this._outputPanel.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(-1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(226, 32);
            this.label3.TabIndex = 6;
            this.label3.Text = "Output";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(2, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "Format:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _outputFormatComboBox
            // 
            this._outputFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._outputFormatComboBox.FormattingEnabled = true;
            this._outputFormatComboBox.Location = new System.Drawing.Point(93, 35);
            this._outputFormatComboBox.Name = "_outputFormatComboBox";
            this._outputFormatComboBox.Size = new System.Drawing.Size(128, 21);
            this._outputFormatComboBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "Folder";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _outputFolderComboBox
            // 
            this._outputFolderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._outputFolderComboBox.FormattingEnabled = true;
            this._outputFolderComboBox.Location = new System.Drawing.Point(93, 62);
            this._outputFolderComboBox.Name = "_outputFolderComboBox";
            this._outputFolderComboBox.Size = new System.Drawing.Size(128, 21);
            this._outputFolderComboBox.TabIndex = 3;
            // 
            // QueueItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Controls.Add(this._mainPanel);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "QueueItem";
            this.Size = new System.Drawing.Size(848, 142);
            this._mainPanel.ResumeLayout(false);
            this._inputPanel.ResumeLayout(false);
            this._outputPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        
        private System.Windows.Forms.Button _removeButton;

        private System.Windows.Forms.Label _filePath;
        private System.Windows.Forms.Label _extencionLabel;

        public System.Windows.Forms.ProgressBar _progressBar;

        private System.Windows.Forms.Panel _inputPanel;

        private System.Windows.Forms.Panel _outputPanel;

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.ComboBox _outputFolderComboBox;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.ComboBox _outputFormatComboBox;

        private System.Windows.Forms.Panel _mainPanel;

        #endregion
    }
}