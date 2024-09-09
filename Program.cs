using System;
using System.Globalization;
using System.IO;
using CommandLine; // https://github.com/commandlineparser/commandline

namespace FClevrCSV;

class Program
{
    static void Main(string[] args)
    {
        // https://github.com/commandlineparser/commandline
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed<CommandLineOptions>(Options =>
            {
                if (string.IsNullOrEmpty(Options.InFilePath))
                {
                    throw new Exception("Input file not provided");
                }

                if (string.IsNullOrEmpty(Options.OutFilePath))
                {
                    throw new Exception("Output file not provided");
                }

                if (!File.Exists(Options.InFilePath))
                {
                    throw new Exception("Input file does not exist");
                }

                if (File.Exists(Options.OutFilePath))
                {
                    Console.WriteLine("Deleting old output file...");
                    File.Delete(Options.OutFilePath);
                }

                // Read the file in
                List<ClevrCSVRecord> parsedRecords = new List<ClevrCSVRecord>();
                using (BrokenClevrCSVParser parser = new BrokenClevrCSVParser(Options.InFilePath, Options.ReplaceNewlines))
                {
                    while (!parser.EOF)
                    {
                        ClevrCSVRecord record = parser.GetNextObject();
                        if (record != null)
                        {
                            try {
                                if (record.Check())
                                {
                                    parsedRecords.Add(record);
                                } else {
                                    throw new Exception($"Obj failed check: {record}");
                                }
                            }
                            catch(Exception ex)
                            {
                                throw new Exception($"Obj failed check: {ex.Message}\n{record}");
                            }
                        }
                    }
                }

                Console.WriteLine($"Successfully read {parsedRecords.Count} objects.");

                // Export the output file (if nothing crashed)
                Console.WriteLine("Writing output file...");
                using (StreamWriter writer = new StreamWriter(Options.OutFilePath))
                {
                    writer.WriteLine("FirstName,LastName,Number,School,DateOfContact,Grade,Counsellor,Comments,CounsellorNotes,Files,Links");

                    foreach(ClevrCSVRecord r in parsedRecords)
                    {
                        if (r.FirstName != "First Name")
                        {
                            writer.Write($"\"{r.FirstName}\",");
                            writer.Write($"\"{r.LastName}\",");
                            writer.Write($"\"{r.Number}\",");
                            writer.Write($"\"{r.School}\",");
                            writer.Write($"\"{r.DateOfContact}\",");
                            writer.Write($"\"{r.Grade}\",");
                            writer.Write($"\"{r.Counsellor}\",");
                            writer.Write($"\"{r.Comments}\",");
                            writer.Write($"\"{r.CounsellorNotes}\",");
                            writer.Write($"\"{r.Files}\",");
                            writer.Write($"\"{r.Links}\"");
                            writer.Write("\r\n");
                        }
                    }
                }

            });






        
    }
}
