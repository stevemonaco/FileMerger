using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            string mergedFileName = "merged.bin";
            string mergedLogName = "mergelog.txt";
            string dataDirectory = "Data";

            Console.WriteLine($"Scanning '{dataDirectory}' directory for files");

            List<FileStream> files = new List<FileStream>();
            var fileNames = Directory.EnumerateFiles("Data");

            Console.WriteLine($"Found {fileNames.Count()} files to merge");

            foreach (var fileName in fileNames)
                files.Add(new FileStream(fileName, FileMode.Open, FileAccess.Read));

            var mergedOutput = new FileStream(mergedFileName, FileMode.Create, FileAccess.Write);
            var fileMerger = new FileMerger();
            fileMerger.MergeFilesByMaxFrequency(files, mergedOutput);

            var logOutput = new StreamWriter(mergedLogName);
            logOutput.WriteLine($"Merge results for {mergedFileName} - {DateTime.Now}");
            logOutput.WriteLine($"Conflict resolution count: {fileMerger.Resolutions.Count}");
            logOutput.WriteLine();

            foreach(var resolution in fileMerger.Resolutions)
            {
                logOutput.Write($"{resolution.Offset:X8} - ");
                foreach(var pair in resolution.MergeChoices)
                {
                    logOutput.Write($"[{pair.Value:X2}, {pair.Count}] ");
                }
                logOutput.WriteLine();
            }

            logOutput.Close();
            mergedOutput.Close();
            foreach (var file in files)
                file.Close();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
