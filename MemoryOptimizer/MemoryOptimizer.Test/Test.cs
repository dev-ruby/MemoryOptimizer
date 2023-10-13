using MemoryOptimizer.Core;

namespace MemoryOptimizer.Test;

internal class Test
{
    private static async Task Main()
    {
        string[] processNames = { "firefox.exe" };

        var memoryCleaner = new MemoryCleaner(processNames, 500);

        var cancellationTokenSource = new CancellationTokenSource();
        await memoryCleaner.RunAsync(cancellationTokenSource.Token);
    }
}