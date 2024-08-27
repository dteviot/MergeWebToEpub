namespace MergeWebToEpub
{
    partial class NoelbinWatermarkForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPossibleWatermark = new System.Windows.Forms.TextBox();
            this.buttonIgnore = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxActualWatermark = new System.Windows.Forms.TextBox();
            this.listViewSimilar = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonRemoveAll = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Possible watermark:";
            // 
            // textBoxPossibleWatermark
            // 
            this.textBoxPossibleWatermark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPossibleWatermark.Location = new System.Drawing.Point(119, 12);
            this.textBoxPossibleWatermark.Multiline = true;
            this.textBoxPossibleWatermark.Name = "textBoxPossibleWatermark";
            this.textBoxPossibleWatermark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPossibleWatermark.Size = new System.Drawing.Size(669, 83);
            this.textBoxPossibleWatermark.TabIndex = 1;
            // 
            // buttonIgnore
            // 
            this.buttonIgnore.Location = new System.Drawing.Point(63, 101);
            this.buttonIgnore.Name = "buttonIgnore";
            this.buttonIgnore.Size = new System.Drawing.Size(75, 23);
            this.buttonIgnore.TabIndex = 2;
            this.buttonIgnore.Text = "Ignore";
            this.buttonIgnore.UseVisualStyleBackColor = true;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(157, 101);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Watermark:";
            // 
            // textBoxActualWatermark
            // 
            this.textBoxActualWatermark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxActualWatermark.Location = new System.Drawing.Point(102, 130);
            this.textBoxActualWatermark.Name = "textBoxActualWatermark";
            this.textBoxActualWatermark.Size = new System.Drawing.Size(672, 20);
            this.textBoxActualWatermark.TabIndex = 5;
            // 
            // listViewSimilar
            // 
            this.listViewSimilar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSimilar.HideSelection = false;
            this.listViewSimilar.Location = new System.Drawing.Point(37, 189);
            this.listViewSimilar.Name = "listViewSimilar";
            this.listViewSimilar.Size = new System.Drawing.Size(737, 217);
            this.listViewSimilar.TabIndex = 6;
            this.listViewSimilar.UseCompatibleStateImageBehavior = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Similar Candidates";
            // 
            // buttonRemoveAll
            // 
            this.buttonRemoveAll.Location = new System.Drawing.Point(63, 415);
            this.buttonRemoveAll.Name = "buttonRemoveAll";
            this.buttonRemoveAll.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveAll.TabIndex = 8;
            this.buttonRemoveAll.Text = "Remove all ticked";
            this.buttonRemoveAll.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(145, 415);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 9;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // NoelbinWatermarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRemoveAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listViewSimilar);
            this.Controls.Add(this.textBoxActualWatermark);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonIgnore);
            this.Controls.Add(this.textBoxPossibleWatermark);
            this.Controls.Add(this.label1);
            this.Name = "NoelbinWatermarkForm";
            this.Text = "NoelbinWatermarkForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPossibleWatermark;
        private System.Windows.Forms.Button buttonIgnore;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxActualWatermark;
        private System.Windows.Forms.ListView listViewSimilar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonRemoveAll;
        private System.Windows.Forms.Button buttonClose;
    }
}