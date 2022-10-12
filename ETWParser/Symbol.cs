using Microsoft.Windows.EventTracing.Symbols;
using System;

namespace ETWParser
{
    internal class Symbol //internal: accessable in same dll
    {
        static public void LoadSymbol(ref ISymbolDataSource symbolData)
        {
            symbolData.LoadSymbolsForConsoleAsync(SymCachePath.Automatic, SymbolPath.Automatic).GetAwaiter().GetResult();
        }

        static public void SetEnv()
        {
            //refernece: https://docs.microsoft.com/en-us/windows-hardware/test/wpt/loading-symbols#symcache-path
            //SymCache Path
            Environment.SetEnvironmentVariable(NT_SYMCACHE_PATH, "C:\\SymCache", EnvironmentVariableTarget.Process);
            //Symbol Path
            Environment.SetEnvironmentVariable("NT_SYMBOL_PATH", "SRV*C:\\WINDOWS\\Symbols*http://msdl.microsoft.com/download/symbols", EnvironmentVariableTarget.Process);
        }

        static public void GetEnv()
        {
            Console.WriteLine("_NT_SYMCACHE_PATH : {0}", Environment.GetEnvironmentVariable(NT_SYMCACHE_PATH));
            Console.WriteLine("_NT_SYMBOL_PATH: {0}", Environment.GetEnvironmentVariable(NT_SYMBOL_PATH));
        }

        static private string NT_SYMCACHE_PATH { get; set; } = "_NT_SYMCACHE_PATH";
        static private string NT_SYMBOL_PATH { get; set; } = "_NT_SYMBOL_PATH";
    }
}
