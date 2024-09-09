using System;
using System.Globalization;
using System.IO;

namespace FClevrCSV;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "./test.csv";
        string outputFile = "./output.csv";

        List<ClevrCSVRecord> parsedRecords = new List<ClevrCSVRecord>();

        using (ClevrCSVParser parser = new ClevrCSVParser(filePath))
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

        if (File.Exists(outputFile))
        {
            Console.WriteLine("Deleting old output file...");
            File.Delete(outputFile);
        }     

        Console.WriteLine("Writing output file...");
        
        using (StreamWriter writer = new StreamWriter(outputFile))
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
    }
}
