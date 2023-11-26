using System.Collections.Generic;

namespace Backbone.Speech
{
    /// <summary>
    /// Talon is an emerging language to command the computer via voice. It's not always clear what hex letter is being said, 
    /// so I figured I'd add Talon support to make it easier
    /// </summary>
    public static class TalonHelper
    {
        private static Dictionary<char, string> toWords = new Dictionary<char, string>()
        {
            { 'a', "air" },
            { 'b', "bat" },
            { 'c', "cap" },
            { 'd', "drum" },
            { 'e', "each" },
            { 'f', "fine" },
            { 'g', "gust" },
            { 'h', "harp" },
            { 'i', "sit" },
            { 'j', "jury" },
            { 'k', "crunch" },
            { 'l', "look" },
            { 'm', "made" },
            { 'n', "near" },
            { 'o', "odd" },
            { 'p', "pit" },
            { 'q', "quench" },
            { 'r', "red" },
            { 's', "sun" },
            { 't', "trap" },
            { 'u', "urge" },
            { 'v', "vest" },
            { 'w', "whale" },
            { 'x', "plex" },
            { 'y', "yank" },
            { 'z', "zip" }
        };

        private static Dictionary<string, char> toLetters = new Dictionary<string, char>()
        {
            { "air", 'a' },
            { "bat" , 'b' },
            { "cap" , 'c' },
            { "drum" , 'd' },
            { "each" , 'e' },
            { "fine" , 'f' },
            { "gust" , 'g' },
            { "harp" , 'h' },
            { "sit" , 'i' },
            { "jury" , 'j' },
            { "crunch" , 'k' },
            { "look" , 'l' },    
            { "made" , 'm' },
            { "near" , 'n' },
            { "odd" , 'o' },
            { "pit" , 'p' },
            { "quench" , 'q' },
            { "red" , 'r' },
            { "sun" , 's' },
            { "trap" , 't' },
            { "urge" , 'u' },
            { "vest" , 'v' },
            { "whale" , 'w' },
            { "plex" , 'x' },
            { "yank" , 'y' },
            { "zip" , 'z' }
        };

        public static string LetterToWord(string letter)
        {
            var letterChar = letter.ToLower().ToCharArray();
            if(letterChar != null & letterChar.Length  > 0)
            {
                return toWords[letterChar[0]];
            }

            return null;
        }

        public static char WordToLetter(string word)
        {
            var wordToCheck = word.ToLower();

            if (wordToCheck != null)
            {
                return toLetters[wordToCheck];
            }

            return '\0';
        }
    }
}
