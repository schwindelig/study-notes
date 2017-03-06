using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TPL
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePaths = GetFilePaths().ToList();
            System.Console.WriteLine($"Getting total size of {filePaths.Count()} files.");

            var parallelForResult = ParallelFor(filePaths);
            System.Console.WriteLine($"ParallelFor Result: {ConvertBytesToGigaBytes(parallelForResult)}GB");

            var parallelForEachResult = ParallelForEach(filePaths);
            System.Console.WriteLine($"ParallelForEach Result: {ConvertBytesToGigaBytes(parallelForEachResult)}GB");
        }

        static IEnumerable<string> GetFilePaths()
        {
            var directoryInfo = new DirectoryInfo("C:\\Program Files");
            var files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            return files.Select(f => f.FullName);
        }

        static long GetFileSize(string path)
        {
            return new FileInfo(path).Length;
        }

        static long ParallelFor(IEnumerable<string> filePaths)
        {
            long totalBytes = 0;
            Parallel.For(0, filePaths.Count(), (index) =>
            {
                Interlocked.Add(ref totalBytes, GetFileSize(filePaths.ElementAt(index)));
            });

            return totalBytes;
        }

        static long ParallelForEach(IEnumerable<string> filePaths)
        {
            long totalBytes = 0;
            Parallel.ForEach(filePaths, (filePath) =>
            {
                Interlocked.Add(ref totalBytes, GetFileSize(filePath));
            });

            return totalBytes;
        }

        static float ConvertBytesToGigaBytes(long bytes)
        {
            return bytes / 1024.0f / 1024.0f / 1024.0f;
        }
    }
}
