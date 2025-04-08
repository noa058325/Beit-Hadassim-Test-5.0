using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogAnalyzer
{
    public class LogProcessor
    {
        private readonly string _logFilePath;
        private readonly string _outputDirectory;
        private readonly int _linesPerFile;
        private readonly int _topN;

        public LogProcessor(string logFilePath, string outputDirectory, int linesPerFile, int topN)
        {
            _logFilePath = logFilePath;
            _outputDirectory = outputDirectory;
            _linesPerFile = linesPerFile;
            _topN = topN;
        }

        public void ProcessLogs()
        {
            if (!File.Exists(_logFilePath))
            {
                Console.WriteLine($" קובץ הלוגים לא נמצא: {_logFilePath}");
                return;
            }

            Console.WriteLine(" מתחיל פיצול קובץ הלוגים...");
            SplitLogFile();

            Console.WriteLine("\n מתחיל ספירת קודי השגיאה...");
            Dictionary<string, int> mergedCounts = CountErrorsFromParts();

            Console.WriteLine("\n קודי השגיאה השכיחים ביותר:");
            var topErrors = GetTopNErrors(mergedCounts);

            foreach (var error in topErrors)
            {
                Console.WriteLine($"{error.Key}: {error.Value}");
            }
        }

        private void SplitLogFile()
        {
            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }

            int fileIndex = 1;
            int lineCounter = 0;
            StreamWriter writer = new StreamWriter(Path.Combine(_outputDirectory, $"log_part_{fileIndex}.txt"));

            using (StreamReader reader = new StreamReader(_logFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    writer.WriteLine(line);
                    lineCounter++;

                    if (lineCounter >= _linesPerFile)
                    {
                        writer.Close();
                        Console.WriteLine($" נוצר קובץ: log_part_{fileIndex}.txt");

                        fileIndex++;
                        lineCounter = 0;
                        writer = new StreamWriter(Path.Combine(_outputDirectory, $"log_part_{fileIndex}.txt"));
                    }
                }
            }

            writer.Close();
            Console.WriteLine(" פיצול הסתיים");
        }

        private Dictionary<string, int> CountErrorsFromParts()
        {
            var mergedCounts = new Dictionary<string, int>();

            foreach (var file in Directory.GetFiles(_outputDirectory, "log_part_*.txt"))
            {
                var errorCounts = CountErrors(file);
                foreach (var kvp in errorCounts)
                {
                    if (!mergedCounts.ContainsKey(kvp.Key))
                        mergedCounts[kvp.Key] = 0;

                    mergedCounts[kvp.Key] += kvp.Value;
                }

                Console.WriteLine($" נספרו שגיאות מתוך {file}");
            }

            return mergedCounts;
        }

        private Dictionary<string, int> CountErrors(string filePath)
        {
            var errorCounts = new Dictionary<string, int>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string errorCode = ExtractErrorCode(line);
                    if (!string.IsNullOrEmpty(errorCode))
                    {
                        if (!errorCounts.ContainsKey(errorCode))
                            errorCounts[errorCode] = 0;

                        errorCounts[errorCode]++;
                    }
                }
            }

            return errorCounts;
        }

        private string ExtractErrorCode(string logLine)
        {
            
            int index = logLine.IndexOf("Error: ");
            if (index != -1)
            {
                string[] parts = logLine.Substring(index + 7).Split(',');
                return parts[0].Trim();
            }
            return string.Empty;
        }

        private List<KeyValuePair<string, int>> GetTopNErrors(Dictionary<string, int> mergedCounts)
        {
        
            var heap = new SortedList<int, string>(new DescendingComparer<int>());

            foreach (var kvp in mergedCounts)
            {
                heap.Add(kvp.Value, kvp.Key);

                if (heap.Count > _topN)
                {
                    heap.RemoveAt(heap.Count - 1); 
                }
            }

            return heap.Select(kvp => new KeyValuePair<string, int>(kvp.Value, kvp.Key)).ToList();
        }
    }


    public class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }
}
