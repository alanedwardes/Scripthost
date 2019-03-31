using CommandLine;
using System.IO;

namespace Scripthost.Options
{
    [Verb("run")]
    internal sealed class RunOptions
    {
        [Option('s', "script", HelpText = "Path to the script to run", Required = true)]
        public FileInfo Script { get; set; }

        [Option('t', "type", HelpText = "Fully qualified type to find in the newly compiled assembly", Required = true)]
        public string Type { get; set; }

        [Option('m', "method", HelpText = "Name of the method to execute in the above type. Must be static with no parameters.", Default = "Run", Required = true)]
        public string Method { get; set; }
    }
}
