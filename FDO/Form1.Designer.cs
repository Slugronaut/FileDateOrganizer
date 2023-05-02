
namespace FDO
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.textSrc = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSetDest = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textDest = new System.Windows.Forms.TextBox();
            this.buttonSetSrc = new System.Windows.Forms.Button();
            this.comboOperation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonPerformOp = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxRecursive = new System.Windows.Forms.CheckBox();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source";
            // 
            // textSrc
            // 
            this.textSrc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSrc.Location = new System.Drawing.Point(47, 28);
            this.textSrc.Name = "textSrc";
            this.textSrc.ReadOnly = true;
            this.textSrc.Size = new System.Drawing.Size(418, 20);
            this.textSrc.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonSetDest);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textDest);
            this.groupBox1.Controls.Add(this.buttonSetSrc);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textSrc);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 91);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Directories";
            // 
            // buttonSetDest
            // 
            this.buttonSetDest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetDest.Location = new System.Drawing.Point(471, 51);
            this.buttonSetDest.Name = "buttonSetDest";
            this.buttonSetDest.Size = new System.Drawing.Size(29, 23);
            this.buttonSetDest.TabIndex = 7;
            this.buttonSetDest.Text = "...";
            this.buttonSetDest.UseVisualStyleBackColor = true;
            this.buttonSetDest.Click += new System.EventHandler(this.buttonSetDest_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Dest";
            // 
            // textDest
            // 
            this.textDest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDest.Location = new System.Drawing.Point(47, 54);
            this.textDest.Name = "textDest";
            this.textDest.ReadOnly = true;
            this.textDest.Size = new System.Drawing.Size(418, 20);
            this.textDest.TabIndex = 6;
            // 
            // buttonSetSrc
            // 
            this.buttonSetSrc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetSrc.Location = new System.Drawing.Point(471, 25);
            this.buttonSetSrc.Name = "buttonSetSrc";
            this.buttonSetSrc.Size = new System.Drawing.Size(29, 23);
            this.buttonSetSrc.TabIndex = 2;
            this.buttonSetSrc.Text = "...";
            this.buttonSetSrc.UseVisualStyleBackColor = true;
            this.buttonSetSrc.Click += new System.EventHandler(this.buttonSetSrc_Click);
            // 
            // comboOperation
            // 
            this.comboOperation.FormattingEnabled = true;
            this.comboOperation.Items.AddRange(new object[] {
            "Move",
            "Copy"});
            this.comboOperation.Location = new System.Drawing.Point(82, 25);
            this.comboOperation.Name = "comboOperation";
            this.comboOperation.Size = new System.Drawing.Size(112, 21);
            this.comboOperation.TabIndex = 3;
            this.comboOperation.Text = "Copy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Operation";
            // 
            // buttonPerformOp
            // 
            this.buttonPerformOp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPerformOp.Location = new System.Drawing.Point(294, 149);
            this.buttonPerformOp.Name = "buttonPerformOp";
            this.buttonPerformOp.Size = new System.Drawing.Size(183, 23);
            this.buttonPerformOp.TabIndex = 5;
            this.buttonPerformOp.Text = "Do The Thing";
            this.buttonPerformOp.UseVisualStyleBackColor = true;
            this.buttonPerformOp.Click += new System.EventHandler(this.buttonPerformOp_Click);
            // 
            // checkBoxRecursive
            // 
            this.checkBoxRecursive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxRecursive.AutoSize = true;
            this.checkBoxRecursive.Checked = true;
            this.checkBoxRecursive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRecursive.Location = new System.Drawing.Point(26, 62);
            this.checkBoxRecursive.Name = "checkBoxRecursive";
            this.checkBoxRecursive.Size = new System.Drawing.Size(169, 17);
            this.checkBoxRecursive.TabIndex = 6;
            this.checkBoxRecursive.Text = "Include Sub-Folders In Search";
            this.checkBoxRecursive.UseVisualStyleBackColor = true;
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.comboOperation);
            this.groupBoxOptions.Controls.Add(this.checkBoxRecursive);
            this.groupBoxOptions.Controls.Add(this.label2);
            this.groupBoxOptions.Location = new System.Drawing.Point(12, 109);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(255, 100);
            this.groupBoxOptions.TabIndex = 7;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "Options";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 224);
            this.Controls.Add(this.groupBoxOptions);
            this.Controls.Add(this.buttonPerformOp);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "File Date Organizer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textSrc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSetSrc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboOperation;
        private System.Windows.Forms.Button buttonSetDest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textDest;
        private System.Windows.Forms.Button buttonPerformOp;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxRecursive;
        private System.Windows.Forms.GroupBox groupBoxOptions;
    }
}

