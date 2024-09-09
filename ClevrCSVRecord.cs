
using System.Reflection.PortableExecutable;

namespace FClevrCSV;

// "First Name","Last Name","Number","School,"Date of contact","Grade","Counsellor","Comments","Counsellor Notes","Files","Links"
public class ClevrCSVRecord {
    private const int max_comment_length = 5000;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public string DateOfContact { get; set; } = string.Empty;
    public string Grade { get ;set ;} = string.Empty;
    public string Counsellor { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
    public string CounsellorNotes { get; set ; } = string.Empty;
    public string Files { get; set; } = string.Empty;
    public string Links { get; set; } = string.Empty;

    public bool Check() 
    {

        if (string.IsNullOrEmpty(this.FirstName))
        {
            throw new Exception("FirstName is blank");
        }

        if (string.IsNullOrEmpty(this.LastName))
        {
            throw new Exception("LastName is blank");
        }
        if (string.IsNullOrEmpty(this.Number))
        {
            throw new Exception("Missing number");
        }
        if (this.FirstName.Length < 2)
        {
            throw new Exception("FirstName is fewer than 2 characters");
        }
        if (this.LastName.Length < 3)
        {
            throw new Exception("LastName is fewer than 3 characters");
        }
        if (this.FirstName.Length > 20)
        {
            throw new Exception("FirstName is more than 20 characters");
        }
        if (this.LastName.Length > 30)
        {
            throw new Exception("LastName is more than 30 characters");
        }
        if (this.Number.Length < 2)
        {
            throw new Exception("Number is fewer than 2 characters");
        }
        if (this.CounsellorNotes.Length > max_comment_length)
        {
            throw new Exception($"CounsellorNotes contains more than {max_comment_length} characters ({this.CounsellorNotes.Length})");
        }

        if (!ContainsAlphaNumericCharacters(this.FirstName))
        {
            throw new Exception("FirstName contains no alphanumeric characters");
        }
        if (!ContainsAlphaNumericCharacters(this.LastName))
        {
            throw new Exception("LastName contains no alphanumeric characters");
        }
        if (ContainsNonAlphaCharacters(this.FirstName))
        {
            throw new Exception("FirstName contains at least one suspicious non-alpha character");
        }
        if (ContainsNonAlphaCharacters(this.LastName))
        {
            throw new Exception("LastName contains at least one suspicious non-alpha character");
        }

        return true;
    }

    public override string ToString()
    {
        return $"{{\n FirstName: {this.FirstName}\n LastName: {this.LastName}\n Number: {this.Number}\n School: {this.School}\n DateOfContact: {this.DateOfContact}\n Grade: {this.Grade}\n Consellor: {this.Counsellor}\n Comment: {this.Comments.Replace('\n','|')}\n CounsellorNotes: {this.CounsellorNotes.Replace('\n','|')}\n Files: {this.Files}\n Links: {this.Links}\n}}";
    }

    private const string alphaCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private bool ContainsAlphaNumericCharacters(string inputString) 
    {
        foreach(char c in inputString) {
            if (alphaCharacters.Contains(c)) {
                return true;
            }
        }
        return false;
    }

    private const string allowed_special_characters = " -\'.";
    private bool ContainsNonAlphaCharacters(string inputString) 
    {
        foreach(char c in inputString) {
            if (!alphaCharacters.Contains(c)) {
                if (!allowed_special_characters.Contains(c))
                {
                    return true;
                }
            }
        }
        return false;
    }
}