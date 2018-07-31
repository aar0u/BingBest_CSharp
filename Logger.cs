using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace BingWallpaper
{
    public class Logger
    {
        private readonly BlockingCollection<string> _bc = new BlockingCollection<string>();

        // Constructor create the thread that wait for work on .GetConsumingEnumerable()
        public Logger(string file)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var message in _bc.GetConsumingEnumerable())
                    using (var streamWriter = new StreamWriter(file, true))
                    {
                        streamWriter.WriteLine($"{DateTime.Now} {message}");
                    }
            });
        }

        ~Logger()
        {
            // Free the writing thread
            _bc.CompleteAdding();
        }

        public void Info(string msg)
        {
            _bc.Add(msg);
        }
    }
}