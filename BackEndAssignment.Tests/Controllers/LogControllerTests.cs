using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEndAssignment.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace BackEndAssignment.Controllers.Tests
{
    [TestClass()]
    public class LogControllerTests
    {
        private const string FILE_NAME = @"C:\tmp\logFile.log";
        private const int numberOfClients = 600000;
        private static Random rand = new Random();

        [TestMethod()]
        public async Task LogTest()
        {
            using (StreamWriter sw = new StreamWriter(FILE_NAME, false))
            {
                //Clear the file before starting the test
            }
            Task[] tasks = new Task[numberOfClients];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = LoadTestLogger();
            }

            await Task.WhenAll(tasks);
        }


        [TestMethod()]
        public void NumberOfLinesTest()
        {
            int lines = 0;
            using (StreamReader sr = new StreamReader(FILE_NAME))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    lines++;
                }
            }

            Assert.IsTrue(lines == numberOfClients);
        }


        private async Task LoadTestLogger()
        {
            var controller = new LogController();
            int p = rand.Next(0, 30);
            string sp = Convert.ToString(p);
            controller.Log(sp, sp + " Test " + DateTime.Now);
        }
    }
}