using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Backbone.IO
{
    public static class Conversion
    {
        /// <summary>
        /// Converts a decimal number to a base-26 number, with each digit represented by a letter from A to Z.
        /// For example, 0 is converted to "A", 25 is converted to "Z", 26 is converted to "AA", and so on.
        /// </summary>
        /// <param name="number">The decimal number to convert to a base-26 number.</param>
        /// <returns>The base-26 representation of the input number, as a string of letters from A to Z.</returns>
        public static String IntToAlphabeticId(int number)
        {
            String id = "";
            bool firstTime = true;
            while (number > 0 || firstTime)
            {
                id = (char)('A' + (number % 26)) + id;
                number /= 26;
                firstTime = false;
            }
            return id;
        }

        /// <summary>
        /// Converts a base-26 number represented as a string of letters from A to Z to the corresponding decimal number.
        /// For example, "A" is converted to 1, "Z" is converted to 26, "AA" is converted to 27, and so on.
        /// </summary>
        /// <param name="id">The base-26 number to convert to a decimal number, as a string of letters from A to Z.</param>
        /// <returns>The decimal representation of the input base-26 number.</returns>
        public static int AlphabeticIdToInt(string id)
        {
            int number = 0;
            int index = 0;
            foreach (char c in id.Reverse())
            {
                number += (int)Math.Pow(26, index) * (c - 'A' + 1);
                index += 1;
            }
            return number;
        }

        /// <summary>
        /// Converts a string with a number at the end into a tuple that contains the first part of the string
        /// for the first item and the ending number as the second item. Assumes a positive integer at the end.
        /// If it can't find a number it returns just the input string and a -1 value for the number.
        /// </summary>
        /// <param name="input">A string value with or without a number at the end of it.</param>
        /// <returns>A tuple with the first item being the first part of the string, and the second item the number at the end, or -1 if no number.</returns>
        public static Tuple<string, int> SeparateStringWithNumberAtEnd(string input)
        {
            // Create a regular expression to match the pattern of the string
            Regex regex = new Regex(@"\d+$");
            Match match = regex.Match(input);

            // Check if the string matches the pattern
            if (!match.Success)
            {
                // If it doesn't match, return a default tuple with empty string and -1
                return Tuple.Create(input, -1);
            }

            // Get the first and second parts of the matched string
            string part1 = regex.Replace(input, string.Empty);
            int part2 = int.Parse(match.Groups[0].Value);

            // Create a tuple with the two parts of the string
            return Tuple.Create(part1, part2);
        }

        /// <summary>
        /// Gets the number of digits in an integer value. For example, 1234 would return 4 for four digits.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>Number of digits.</returns>
        public static int NumberOfDigits(int number)
        {
            int count = 0;
            while (number > 0)
            {
                number /= 10;
                count++;
            }
            return count;
        }
    }
}
