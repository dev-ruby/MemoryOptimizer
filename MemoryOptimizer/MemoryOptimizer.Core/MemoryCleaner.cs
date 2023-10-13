#define DEBUG

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MemoryOptimizer.Core;

public class MemoryCleaner
{
    public readonly List<string> ProcessNames;

    public MemoryCleaner(IEnumerable<string> processNames, int delay)
    {
        Delay = delay;
        ProcessNames = processNames.ToList();
    }

    public int Delay { get; set; }


    [DllImport("psapi.dll")]
    private static extern bool EmptyWorkingSet(IntPtr hProcess);

    [DllImport("kernel32.dll")]
    private static extern int GetLastError();

    public async Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            long succeed = 0;
            long failed = 0;
            HashSet<int> errorCodes = new();

            foreach (var process in ProcessNames.SelectMany(_GetProcessByName))
            {
                var result = EmptyWorkingSet(process.Handle);
                var errorCode = GetLastError();

                if (result)
                {
                    succeed++;
                }
                else
                {
                    failed++;
                    errorCodes.Add(errorCode);
                }
            }

            Console.WriteLine("======================");
            Console.WriteLine($"{succeed} succeed / {failed} failed");
            Console.WriteLine(errorCodes.Count == 0? "No Errors Founded" : "Error Codes: " + string.Join(", ", errorCodes));
            Console.WriteLine("======================\n");
            await Task.Delay(Delay);
        }
    }

    private static IEnumerable<Process> _GetProcessByName(string processName)
    {
        processName = Path.GetFileNameWithoutExtension(processName);
        return Process.GetProcessesByName(processName);
    }
}