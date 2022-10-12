using System;
using CommandLine;
using CommandLine.Text;

namespace ETWParser
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('t', "trace", Required = true, HelpText = "etl trace.")]
        public string Trace { get; set; }
    }
}