using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympusCodeQuiz
{
    internal class FileLockLineWriter
    {
        public static object Lock = new object();

        private const int MAX_RETRIES = 5;
        private const double RETRY_INTERVAL_SECONDS = 1;

        private int _threadCount;
        private int _linesPerThread;
        private FileLineWriter _fileLineWriter;

        public FileLockLineWriter(int threadCount, int linesPerThread, FileLineWriter fileLineWriter)
        {
            _threadCount = threadCount;
            _linesPerThread = linesPerThread;
            _fileLineWriter = fileLineWriter;
        }

        public void WriteLines() 
        {
            var threads = new List<Thread>();
            for (int i = 0; i < _threadCount; i++)
            {
                var t = new Thread(new ThreadStart(WriteThreadLines));
                t.Start();
                threads.Add(t);
            }

            foreach (var t in threads)
            {
                t.Join();
            }
        }

        public void WriteThreadLines()
        {
            for (int i = 0; i < _linesPerThread; i++)
            {
                lock (Lock)
                {
                    try
                    {
                        RetryWriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                    
                }
            }
            
        }

        public void RetryWriteLine()
        {
            var exceptions = new List<Exception>();
            for (int i = 0; i < MAX_RETRIES; i++)
            {
                try 
                {
                    if(i > 0)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(RETRY_INTERVAL_SECONDS));
                    }
                    _fileLineWriter.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    return;
                } 
                catch (Exception ex) 
                {
                    exceptions.Add(ex);
                }
            }
            
        }
    }
}
