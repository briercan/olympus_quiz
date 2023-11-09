using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympusCodeQuiz
{
    internal class FileLineWriter
    {
        private string _directory;
        private string _fileName;

        private const string DATE_FORMAT = "hh:mm:ss.fff";

        public FileLineWriter(string directory,
            string fileName)
        {
            _directory = directory;
            _fileName = fileName;
        }

        public void WriteLine(int threadId)
        {
            var filePath = GetFilePath();
            var lines = File.ReadLines(filePath);
            var counter = 0;
            if (lines.Any())
            {
                var lastLine = lines.Last();
                counter = GetCounter(lastLine);
            }
           
            using (StreamWriter stream = File.AppendText(filePath))
            {
                counter++;
                var timestamp = DateTime.Now.ToString(DATE_FORMAT);
                var line = $"{counter},{threadId},{timestamp}";
                stream.WriteLine(line);
            }

        }

        public void InitFile()
        {
            Directory.CreateDirectory(_directory);

            var filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // add first line
            using(StreamWriter stream = File.AppendText(filePath))
            {
                var timestamp = DateTime.Now.ToString(DATE_FORMAT);
                stream.WriteLine($"0,0,{timestamp}");
            }
            
        }

        private int GetCounter(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return 1;
            }

            var counterString = line.Split(',')[0];
            return int.Parse(counterString);
                
        }

        private string GetFilePath()
        {
            return Path.Combine(_directory, _fileName);
        }
    }
}
