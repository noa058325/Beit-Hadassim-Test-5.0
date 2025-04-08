using System;

namespace LogAnalyzer
{
    class Program
    {
        static void Main()
        {
            string logFilePath = @"logs.txt";  


            string outputDirectory = @"C:\מטלה\LogAnalyzer\LogParts";
            int linesPerFile = 100000;
            int topN = 5;

            LogProcessor logProcessor = new LogProcessor(logFilePath, outputDirectory, linesPerFile, topN);
            logProcessor.ProcessLogs();
        }
    }
}
