﻿using MergeWebToEpub;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using UnitTestMergeWebToEpub;

namespace BatchCheckEpubs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //ScanEpubsInDirectory(dlg.SelectedPath);
                    ScanEpubsInDirectory(@"D:\ToBackup\EPUBS");
                }
            }
        }

        public void ScanEpubsInDirectory(string directory)
        {
            var skipList = LogFile.GetSkipList(directory);
            using (var logFile = new LogFile(directory))
            {
                foreach (var fileName in Directory.EnumerateFiles(directory, "*.epub", SearchOption.AllDirectories))
                {
                    LogEntry entry = null;
                    if (skipList.TryGetValue(fileName, out entry))
                    {
                        logFile.LogResults(entry);
                    }
                    else
                    {
                        CheckEpub(fileName, logFile);
                    }
                }
            }
            MessageBox.Show("Done");
        }


        public void CheckEpub(string fileName, LogFile logFile)
        {
            System.Diagnostics.Trace.WriteLine("Checking " + fileName);
            var epub = new Epub();
            try
            {
                try
                {
                    epub.ReadFile(fileName);
                }
                catch(Exception ex1)
                {
                    System.Diagnostics.Trace.WriteLine(ex1.Message);
                    logFile.LogResults(fileName, "Failed to read EPUB", false);
                    return;
                }
                var errors = epub.ValidateXhtml();
                if (0 < errors.Count)
                {
                    logFile.LogResults(fileName, "Invalid XHTML", false);
                    return;
                }

                if (!TestPrettyPrint(epub))
                {
                    logFile.LogResults(fileName, "Failed Pretty Print", false);
                    return;
                }

                errors = epub.CheckForMissingChapters().ToList();
                if (0 < errors.Count)
                {
                    logFile.LogResults(fileName, "Possibly Missing Chapters", false);
                    System.Diagnostics.Trace.WriteLine(String.Join(", ", errors));
                    return;
                }

                errors = epub.ValidateImages().ToList();
                if (0 < errors.Count)
                {
                    logFile.LogResults(fileName, "Webp images", false);
                    return;
                }
            }
            catch (Exception ex2)
            {
                System.Diagnostics.Trace.WriteLine(ex2.Message);
                logFile.LogResults(fileName, "Threw exception", false);
                return;
            }

            // if get here, all good
            logFile.LogResults(fileName, "<none>", true);
        }

        private bool TestPrettyPrint(Epub epub)
        {
            foreach (var item in epub.Opf.Spine)
            {
                XDocument doc = item.RawBytes.ToXhtml();
                var xml = Encoding.UTF8.GetString(item.RawBytes);
                var fromAgility = HtmlAgilityPackUtils.PrettyPrintXhtml(xml);
                XDocument agilityDoc = Encoding.UTF8.GetBytes(fromAgility).ToXhtml();
                var delta = XmlCompare.ElementSame(agilityDoc.Root, doc.Root);
                if (!delta.AreSame)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
