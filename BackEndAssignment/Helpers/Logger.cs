using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BackEndAssignment.Helpers
{
    public class Logger
    {
        private const string FILE_NAME = @"C:\tmp\logFile.log";
        private const int BUFFER_SIZE = 9000;
        private const int LOG_WRITE_INTERVAL = 500;


        // ------ Thread-safe Singleton fields and properties ----------
        private static readonly object _objLock = new object();
        private static Logger _instance = null;

        /// <summary>
        /// Gets a Logger instance
        /// </summary>
        public static Logger Instance
        {
            get
            {
                lock (_objLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                    return _instance;
                }

            }
        }
        // ------------------------------------------------------------


        // ------ Instance fields and properties  ------- 

        private int _currentBuffer;
        private PriorityQueue<string, int> msgQueue;


        private Logger()
        {
            msgQueue = new PriorityQueue<string, int>();
            _currentBuffer = 0;

            //Not awaiting this. Runs in the background.
            Task.Factory.StartNew(() => DoWriteAsync());
        }

        /// <summary>
        /// Write a message to the log file
        /// </summary>
        /// <param name="priority">Priority of the message. Higher priority will be written first</param>
        /// <param name="message">Message to be written.</param>
        public static void Write(int priority, string message)
        {
            Instance.EnqueueMessage(priority, message);
        }
        
        private void EnqueueMessage(int priority, string message)
        {
            msgQueue.Enqueue(message, priority);
        }

        /// <summary>
        /// Flushes the log queue to the log file.
        /// </summary>
        ~Logger()
        {
            EndQueue();
        }
        
        private void EndQueue()
        {
            // -------- DEBUG --------
            //using (StreamWriter sw = new StreamWriter(FILE_NAME, true, Encoding.Default, BUFFER_SIZE))
            //{
            //    sw.WriteLine("Flushing queue");
            //}
            WriteCurrentQueue();

        }

        private void WriteCurrentQueue()
        {
            using (StreamWriter sw = new StreamWriter(FILE_NAME, true, Encoding.Default, BUFFER_SIZE))
            {
                try
                {
                    // -------- DEBUG --------
                    //sw.WriteLine("Start");
                    while (msgQueue.Count > 0)
                    {
                        string msg = msgQueue.Dequeue();
                        if (_currentBuffer + msg.Length >= BUFFER_SIZE)
                        { //Flush the buffer if the next message exceeds it in order to avoid data loss.
                            _currentBuffer = 0;
                            sw.Flush();
                        }
                        _currentBuffer += msg.Length;
                        sw.WriteLine(msg);
                    }

                }
                catch (Exception)
                {
                }
                finally
                {
                    // -------- DEBUG --------
                    //sw.WriteLine("End");
                    sw.Flush();
                }
            }

        }


        private async Task DoWriteAsync()
        {
            while (true)
            {
                WriteCurrentQueue();
                await Task.Delay(LOG_WRITE_INTERVAL);
            }
        }
    }
}