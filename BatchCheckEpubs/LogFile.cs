using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BatchCheckEpubs
{
    public class LogEntry
    {
        public LogEntry(XmlReader reader)
        {
            FileName = reader.GetAttribute("fileName");
            Ignore = Convert.ToBoolean(reader.GetAttribute("ignore").ToLower());
            Error = reader.GetAttribute("error");
        }

        public string FileName { get; set; }
        public string Error { get; set; }
        public bool Ignore { get; set; }
    }

    public class LogFile : IDisposable
    {
        public LogFile(string directory)
        {
            var backup = Path.Combine(directory, LogFileBackupName);
            var logFile = Path.Combine(directory, LogFileName);

            File.Delete(backup);
            if (File.Exists(logFile))
            {
                File.Move(logFile, backup);
            }
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            writer = XmlWriter.Create(logFile, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Epubs");
        }

        public void LogResults(LogEntry entry)
        {
            LogResults(entry.FileName, entry.Error, entry.Ignore);
        }

        public void LogResults(string fileName, string errorType, bool ignore)
        {
            writer.WriteStartElement("epub");
            writer.WriteAttributeString("fileName", fileName);
            writer.WriteAttributeString("error", errorType);
            writer.WriteAttributeString("ignore", ignore.ToString());
            writer.WriteEndElement();
        }

        public void Dispose()
        {
            if (writer != null)
            {
                writer.WriteEndElement();
                writer.Close();
                writer.Dispose();
                writer = null;
            }
        }

        public static Dictionary<string, LogEntry> GetSkipList(string directory)
        {
            var skipList = new Dictionary<string, LogEntry>();
            var logFile = Path.Combine(directory, LogFileName);
            if (File.Exists(logFile))
            {
                using (var reader = XmlReader.Create(logFile))
                {
                    while (reader.ReadToFollowing("epub"))
                    {
                        var result = new LogEntry(reader);
                        if (result.Ignore)
                        {
                            skipList.Add(result.FileName, result);
                        }
                    }
                }
            }
            return skipList;
        }

        private XmlWriter writer { get; set; }


        private const string LogFileName = "CheckResults.xml";
        private const string LogFileBackupName = "CheckResults.Backup.xml";

    }
}
