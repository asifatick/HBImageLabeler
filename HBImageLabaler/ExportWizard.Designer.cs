namespace HBImageLabaler
{
    partial class ExportWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.clbClassList = new System.Windows.Forms.CheckedListBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.lstImages = new System.Windows.Forms.ListView();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // clbClassList
            // 
            this.clbClassList.Dock = System.Windows.Forms.DockStyle.Left;
            this.clbClassList.FormattingEnabled = true;
            this.clbClassList.Location = new System.Drawing.Point(0, 0);
            this.clbClassList.Name = "clbClassList";
            this.clbClassList.Size = new System.Drawing.Size(187, 559);
            this.clbClassList.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(250, 250);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lstImages
            // 
            this.lstImages.CheckBoxes = true;
            this.lstImages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstImages.LargeImageList = this.imageList;
            this.lstImages.Location = new System.Drawing.Point(187, 42);
            this.lstImages.Name = "lstImages";
            this.lstImages.Size = new System.Drawing.Size(911, 517);
            this.lstImages.TabIndex = 1;
            this.lstImages.UseCompatibleStateImageBehavior = false;
            this.lstImages.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstImages_ItemChecked);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(212, 13);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export To CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Checked = true;
            this.chkSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAll.Location = new System.Drawing.Point(631, 13);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(106, 21);
            this.chkSelectAll.TabIndex = 3;
            this.chkSelectAll.Text = "chkSelectAll";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ExportWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 559);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lstImages);
            this.Controls.Add(this.clbClassList);
            this.Name = "ExportWizard";
            this.Text = "ExportWizard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ExportWizard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbClassList;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ListView lstImages;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox chkSelectAll;
    }
}