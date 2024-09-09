
using System.Text;
using System.Text.RegularExpressions;

namespace FClevrCSV;

public class BrokenClevrCSVParser : IDisposable
{
    private string _Filename = string.Empty;
    int maxRescueAttempts = 25;
    private bool replaceNewLines = false;
    private readonly string regex_pattern_capture = @"""(?<one>.*?)"",""(?<two>.*?)"",""(?<three>.*?)"",""(?<four>.*?)"",""(?<five>.*?)"",""(?<six>.*?)"",""(?<seven>.*?)"",""(?<eight>.*?)"",""(?<nine>.*?)"",""(?<ten>.*?)"",""(?<eleven>.*?)""";

    private StreamReader _streamReader = new StreamReader(new MemoryStream());

    public BrokenClevrCSVParser(string Filename)
    {
        this._Filename = Filename;
        this._streamReader = new StreamReader(this._Filename);
    }

    public BrokenClevrCSVParser(string Filename, bool ReplaceNewlines)
    {
        this._Filename = Filename;
        this._streamReader = new StreamReader(this._Filename);
        this.replaceNewLines = ReplaceNewlines;
    }

    public bool EOF { get { return _streamReader.EndOfStream; } }


    private bool matchesRegex(string InputText)
    {
        Regex regex = new Regex(regex_pattern_capture, RegexOptions.Singleline);
        Match regexMatch = regex.Match(InputText);
        return regexMatch.Success;
    }

    private string SanitizeForCSV(string InputText)
    {        
        string returnMe = InputText
            .Replace("\"", "\"\"")
            .Replace("&amp;","&")
            .Replace("–","-")
            .Replace('’','\'')
            .Replace('“', '\"');

        if (this.replaceNewLines)
        {
            returnMe = returnMe.Replace('\n','|');
        }

        return returnMe;
    }

    private ClevrCSVRecord ParseObject(string InputText)
    {
        Regex regex = new Regex(regex_pattern_capture, RegexOptions.Singleline);
        Match regexMatch = regex.Match(InputText);

        if (regexMatch.Success)
        {
            ClevrCSVRecord newRecord = new ClevrCSVRecord()
            {
                FirstName = SanitizeForCSV(regexMatch.Groups["one"].Value),
                LastName = SanitizeForCSV(regexMatch.Groups["two"].Value),
                Number = SanitizeForCSV(regexMatch.Groups["three"].Value),
                School = SanitizeForCSV(regexMatch.Groups["four"].Value),
                DateOfContact = SanitizeForCSV(regexMatch.Groups["five"].Value),
                Grade = SanitizeForCSV(regexMatch.Groups["six"].Value),
                Counsellor = SanitizeForCSV(regexMatch.Groups["seven"].Value),
                Comments = SanitizeForCSV(regexMatch.Groups["eight"].Value),
                CounsellorNotes = SanitizeForCSV(regexMatch.Groups["nine"].Value),
                Files = SanitizeForCSV(regexMatch.Groups["ten"].Value),
                Links = SanitizeForCSV(regexMatch.Groups["eleven"].Value)
            };

            return newRecord;

        } else {
            throw new Exception($"ParseObject - Failed to parse:\n---\n{InputText.ToString()}\n---\n");
        }
    }

    public ClevrCSVRecord GetNextObject()
    {
        StringBuilder rawText = new StringBuilder();

        int attemptCounter = 0;
        while(!matchesRegex(rawText.ToString()))
        {
            attemptCounter++;

            rawText.Append(this.ReadNextRow());

            if (attemptCounter > maxRescueAttempts)
            {
                throw new Exception($"GetNextObject - Failed to parse:\n---\n{rawText.ToString()}\n---\n");
            }
        }

        return ParseObject(rawText.ToString());
    }

    private string ReadNextRow()
    {
        StringBuilder _returnMe = new StringBuilder();

        // Read until we get to a " then a carriage return
        // Or until we reach the max number of rows to try before giving up

        int current_character_byte;

        char previous_character = (char)0;
        while ((current_character_byte = this._streamReader.Read()) != -1)
        {
            char current_character = (char)current_character_byte;
            _returnMe.Append(current_character);

            // If the current character is a newline, check if the previous character was a double quote
            // BUT if the previous-previous character is also a double quote, then continue
            if ((previous_character == '\"') && (current_character == '\n'))
            {
                break;
            }

            previous_character = current_character;
        }

        // If nothing was read and we reached the end, return null
        if (_returnMe.Length == 0 && current_character_byte == -1)
        {
            return null;
        }

        // Strip the final newline off the end
        if (_returnMe.Length > 0)
        {
            if (_returnMe[_returnMe.Length - 1] == '\n')
            {
                _returnMe.Remove(_returnMe.Length - 1, 1);
            }
        }

        return _returnMe.ToString();
    }

    public void Dispose()
    {
        this._streamReader.Close();
    }
}
