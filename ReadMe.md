# FClevr
The "F" stands for Fix!

A small utility that repairs the CSV exported out of Clevr's system for "School Counsellor Contact". This export may be incredibly specific to our organization, so this code may not be useful to you if you aren't Living Sky School Division. In the event it is useful, it's here for you to look at.

# Why this exists
We needed to export our student counselling records out of Clevr in order to import them into a different system. Clevr was able to provide us with an export of our data, but the file had quite a few formatting and encoding issues in it that needed to be addressed before the file could be used elsewhere. Notably, the records in the file included newlines, and double-quotes were not properly escaped in fields that contained newlines (the "sanitizing" stopped at the first newline character).

Rather than fixing 260,000 rows by hand, we made this script instead. It attempts to parse records from the input file, and continues to load extra lines from the source file as needed until it has enough data to rebuild the record. It then outputs a CSV file that conforms to RFC 4180, which properly escapes characters like double-quotes and optinally newlines, and can therefore actually be imported into other systems. 

Optionally this code allows you to replace the newline characters within fields with something else, because every system's CSV import capabilities are going to be slightly different, and this options makes it safer. It replaces newline characters with pipes `|`, which _probably_ don't exist in any of the imported data (didn't when I checked).


# Command line options
| Command                  | Description                                                                                    |
|--------------------------|------------------------------------------------------------------------------------------------|
| `-i`, `--in`             | Input filename (the file Clevr gave you)                                                       |
| `-o`, `--out`            | Output filename (where do you want to save the fixed CSV file?)                                |
| `--replacenewlines`      | Should this script replace newlines found WITHIN THE DATA with a pipe `\|` character? (Default: No). |


# Building
A procompiled binary is not provided here, because we only really needed to use this code once, and running it via the `dotnet` command was sufficient.

If you wanted to build it to an exe:
```
dotnet publish -r win-x64 -p:PublishSingleFile=true -c Release --self-contained true -p:PublishTrimmed=true
```
The finished exe can be found under bin/Release/net8.0/winx64/FClevr.exe


## Running via the `dotnet` command
```
dotnet run -- -i "path\to\input_file.csv -o "C:\my_output_file.csv"
```
```
dotnet run -- -i /home/user/broken_clevr_export.csv -o /home/user/happy_fixed_export.csv --replacenewlines
```