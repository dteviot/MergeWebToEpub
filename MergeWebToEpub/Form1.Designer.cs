namespace MergeWebToEpub
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appendToEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCheckedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeWebpToJpegToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextMissingChapterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortChaptersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewEpubItems = new System.Windows.Forms.ListView();
            this.contextMenuStripSpine = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cutItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteItemssToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertAfterSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renumberIDsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSpine = new System.Windows.Forms.TabPage();
            this.tabPageToC = new System.Windows.Forms.TabPage();
            this.tabPageImages = new System.Windows.Forms.TabPage();
            this.listViewThumbs = new System.Windows.Forms.ListView();
            this.contextMenuStripThumbs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.informationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListThumbs = new System.Windows.Forms.ImageList(this.components);
            this.runCleanersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripSpine.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSpine.SuspendLayout();
            this.tabPageImages.SuspendLayout();
            this.contextMenuStripThumbs.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.appendToEndToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // appendToEndToolStripMenuItem
            // 
            this.appendToEndToolStripMenuItem.Name = "appendToEndToolStripMenuItem";
            this.appendToEndToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.appendToEndToolStripMenuItem.Text = "Append to End";
            this.appendToEndToolStripMenuItem.Click += new System.EventHandler(this.appendToEndToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(176, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteCheckedToolStripMenuItem,
            this.changeWebpToJpegToolStripMenuItem,
            this.nextMissingChapterToolStripMenuItem,
            this.sortChaptersToolStripMenuItem,
            this.runCleanersToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // deleteCheckedToolStripMenuItem
            // 
            this.deleteCheckedToolStripMenuItem.Name = "deleteCheckedToolStripMenuItem";
            this.deleteCheckedToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.deleteCheckedToolStripMenuItem.Text = "Delete Selected";
            this.deleteCheckedToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedToolStripMenuItem_Click);
            // 
            // changeWebpToJpegToolStripMenuItem
            // 
            this.changeWebpToJpegToolStripMenuItem.Name = "changeWebpToJpegToolStripMenuItem";
            this.changeWebpToJpegToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.changeWebpToJpegToolStripMenuItem.Text = "Convert Webp to Jpeg";
            this.changeWebpToJpegToolStripMenuItem.Click += new System.EventHandler(this.changeWebpToJpegToolStripMenuItem_Click);
            // 
            // nextMissingChapterToolStripMenuItem
            // 
            this.nextMissingChapterToolStripMenuItem.Name = "nextMissingChapterToolStripMenuItem";
            this.nextMissingChapterToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.nextMissingChapterToolStripMenuItem.Text = "Next Error Chapter";
            this.nextMissingChapterToolStripMenuItem.Click += new System.EventHandler(this.nextMissingChapterToolStripMenuItem_Click);
            // 
            // sortChaptersToolStripMenuItem
            // 
            this.sortChaptersToolStripMenuItem.Name = "sortChaptersToolStripMenuItem";
            this.sortChaptersToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.sortChaptersToolStripMenuItem.Text = "Sort Chapters";
            this.sortChaptersToolStripMenuItem.Click += new System.EventHandler(this.sortChaptersToolStripMenuItem_Click);
            // 
            // listViewEpubItems
            // 
            this.listViewEpubItems.ContextMenuStrip = this.contextMenuStripSpine;
            this.listViewEpubItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewEpubItems.HideSelection = false;
            this.listViewEpubItems.Location = new System.Drawing.Point(3, 3);
            this.listViewEpubItems.Name = "listViewEpubItems";
            this.listViewEpubItems.Size = new System.Drawing.Size(707, 369);
            this.listViewEpubItems.TabIndex = 2;
            this.listViewEpubItems.UseCompatibleStateImageBehavior = false;
            this.listViewEpubItems.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewEpubItems_RetrieveVirtualItem);
            // 
            // contextMenuStripSpine
            // 
            this.contextMenuStripSpine.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutItemsToolStripMenuItem,
            this.pasteItemssToolStripMenuItem,
            this.deleteItemToolStripMenuItem,
            this.insertAfterSelectedToolStripMenuItem,
            this.renumberIDsToolStripMenuItem});
            this.contextMenuStripSpine.Name = "contextMenuStripSpine";
            this.contextMenuStripSpine.Size = new System.Drawing.Size(180, 114);
            this.contextMenuStripSpine.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripSpine_Opening);
            // 
            // cutItemsToolStripMenuItem
            // 
            this.cutItemsToolStripMenuItem.Name = "cutItemsToolStripMenuItem";
            this.cutItemsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.cutItemsToolStripMenuItem.Text = "Cut Item(s)";
            this.cutItemsToolStripMenuItem.Click += new System.EventHandler(this.cutItemsToolStripMenuItem_Click);
            // 
            // pasteItemssToolStripMenuItem
            // 
            this.pasteItemssToolStripMenuItem.Name = "pasteItemssToolStripMenuItem";
            this.pasteItemssToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.pasteItemssToolStripMenuItem.Text = "Paste Items(s)";
            this.pasteItemssToolStripMenuItem.Click += new System.EventHandler(this.pasteItemssToolStripMenuItem_Click);
            // 
            // deleteItemToolStripMenuItem
            // 
            this.deleteItemToolStripMenuItem.Name = "deleteItemToolStripMenuItem";
            this.deleteItemToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.deleteItemToolStripMenuItem.Text = "Delete Item";
            this.deleteItemToolStripMenuItem.Click += new System.EventHandler(this.deleteItemToolStripMenuItem_Click);
            // 
            // insertAfterSelectedToolStripMenuItem
            // 
            this.insertAfterSelectedToolStripMenuItem.Name = "insertAfterSelectedToolStripMenuItem";
            this.insertAfterSelectedToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.insertAfterSelectedToolStripMenuItem.Text = "Insert After Selected";
            this.insertAfterSelectedToolStripMenuItem.Click += new System.EventHandler(this.insertAfterSelectedToolStripMenuItem_Click);
            // 
            // renumberIDsToolStripMenuItem
            // 
            this.renumberIDsToolStripMenuItem.Name = "renumberIDsToolStripMenuItem";
            this.renumberIDsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.renumberIDsToolStripMenuItem.Text = "Renumber IDs";
            this.renumberIDsToolStripMenuItem.Click += new System.EventHandler(this.renumberIDsToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageSpine);
            this.tabControl1.Controls.Add(this.tabPageToC);
            this.tabControl1.Controls.Add(this.tabPageImages);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(721, 401);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageSpine
            // 
            this.tabPageSpine.Controls.Add(this.listViewEpubItems);
            this.tabPageSpine.Location = new System.Drawing.Point(4, 22);
            this.tabPageSpine.Name = "tabPageSpine";
            this.tabPageSpine.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSpine.Size = new System.Drawing.Size(713, 375);
            this.tabPageSpine.TabIndex = 0;
            this.tabPageSpine.Text = "Spine";
            this.tabPageSpine.UseVisualStyleBackColor = true;
            // 
            // tabPageToC
            // 
            this.tabPageToC.Location = new System.Drawing.Point(4, 22);
            this.tabPageToC.Name = "tabPageToC";
            this.tabPageToC.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageToC.Size = new System.Drawing.Size(713, 375);
            this.tabPageToC.TabIndex = 1;
            this.tabPageToC.Text = "Table of Contents";
            this.tabPageToC.UseVisualStyleBackColor = true;
            // 
            // tabPageImages
            // 
            this.tabPageImages.Controls.Add(this.listViewThumbs);
            this.tabPageImages.Location = new System.Drawing.Point(4, 22);
            this.tabPageImages.Name = "tabPageImages";
            this.tabPageImages.Size = new System.Drawing.Size(713, 375);
            this.tabPageImages.TabIndex = 2;
            this.tabPageImages.Text = "Images";
            this.tabPageImages.UseVisualStyleBackColor = true;
            // 
            // listViewThumbs
            // 
            this.listViewThumbs.ContextMenuStrip = this.contextMenuStripThumbs;
            this.listViewThumbs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewThumbs.HideSelection = false;
            this.listViewThumbs.LargeImageList = this.imageListThumbs;
            this.listViewThumbs.Location = new System.Drawing.Point(0, 0);
            this.listViewThumbs.Name = "listViewThumbs";
            this.listViewThumbs.Size = new System.Drawing.Size(713, 375);
            this.listViewThumbs.TabIndex = 0;
            this.listViewThumbs.UseCompatibleStateImageBehavior = false;
            // 
            // contextMenuStripThumbs
            // 
            this.contextMenuStripThumbs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.informationToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripThumbs.Name = "contextMenuStripThumbs";
            this.contextMenuStripThumbs.Size = new System.Drawing.Size(138, 48);
            this.contextMenuStripThumbs.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripThumbs_Opening);
            // 
            // informationToolStripMenuItem
            // 
            this.informationToolStripMenuItem.Name = "informationToolStripMenuItem";
            this.informationToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.informationToolStripMenuItem.Text = "Information";
            this.informationToolStripMenuItem.Click += new System.EventHandler(this.informationToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // imageListThumbs
            // 
            this.imageListThumbs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListThumbs.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListThumbs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // runCleanersToolStripMenuItem
            // 
            this.runCleanersToolStripMenuItem.Name = "runCleanersToolStripMenuItem";
            this.runCleanersToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.runCleanersToolStripMenuItem.Text = "Run Cleaners";
            this.runCleanersToolStripMenuItem.Click += new System.EventHandler(this.runCleanersToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 425);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Merge WebToEpub";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripSpine.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSpine.ResumeLayout(false);
            this.tabPageImages.ResumeLayout(false);
            this.contextMenuStripThumbs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appendToEndToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ListView listViewEpubItems;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCheckedToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSpine;
        private System.Windows.Forms.TabPage tabPageToC;
        private System.Windows.Forms.TabPage tabPageImages;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSpine;
        private System.Windows.Forms.ToolStripMenuItem deleteItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertAfterSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteItemssToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renumberIDsToolStripMenuItem;
        private System.Windows.Forms.ListView listViewThumbs;
        private System.Windows.Forms.ImageList imageListThumbs;
        private System.Windows.Forms.ToolStripMenuItem changeWebpToJpegToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextMissingChapterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortChaptersToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripThumbs;
        private System.Windows.Forms.ToolStripMenuItem informationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runCleanersToolStripMenuItem;
    }
}

