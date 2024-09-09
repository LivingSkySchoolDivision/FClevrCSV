
using CommandLine; // https://github.com/commandlineparser/commandline

namespace FClevrCSV;

public class CommandLineOptions
{
        [Option('i',"in", Required = true, HelpText = "The input filename - the CSV file you got from Clevr that needs fixing")]
        public string InFilePath { get;set; } = string.Empty;

        [Option('o',"out", Required = true, HelpText = "The filename to save the output to (CSV)")]
        public string OutFilePath { get;set; } = string.Empty;

        [Option("replacenewlines", Required = false, HelpText = "Replace newline characters with pipes")]
        public bool ReplaceNewlines { get; set; } = false;
}