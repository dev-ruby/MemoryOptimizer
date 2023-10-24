using MemoryOptimizer.Core;

namespace MemoryOptimizer.Test;

internal class Test
{
    
    private static async Task Main()
    {
        string[] processNames = { "firefox.exe" };

        var memoryCleaner = new MemoryCleaner(processNames, 500);

        using var cts = new CancellationTokenSource();
        
        var _ = memoryCleaner.RunAsync(cts.Token);
        
        await Task.Delay(1000);
        cts.Cancel();
    }
}