using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    /// <summary>
    /// For validating ISBN numbers
    /// </summary>
    public struct ISBN
    {
        // Holder for the user input
        private string inputcode;
        // Holder for the calculated sum
        private int sum;
        private int checkdigit;
        /// <summary>Clear string of non-numeric characters</summary>
        public string GetCleanedCodeString => System.Text.RegularExpressions.Regex.Replace(inputcode, "[^0-9]", "");
        /// <summary>Is 12 or 13 characters/digits</summary>
        public bool IsCorrectLength => GetCleanedCodeString.Length == 12 || GetCleanedCodeString.Length == 13 ? true : false;
        /// <summary>0 means code is invalid, other numbers between 1-9 are okay</summary>
        public bool IsValidCheckDigit => checkdigit > 0 && checkdigit < 10 ? true : false;
        /// <summary>
        /// After validation get the correct input back (13 numbers)
        /// </summary>
        public string GetCodeWith13Numbers => GetCleanedCodeString + checkdigit.ToString();

        public ISBN(string code)
        {
            inputcode = code;
            sum = 0;
            checkdigit = 0;
        }

        public bool IsValid()
        {
            // Replace non numbers
            string cleanedInput = GetCleanedCodeString;

            int length = cleanedInput.Length;

            // Invalid length, checks for ISBN with 13 numbers.
            if (!IsCorrectLength)
                return false;

            // Remove the last one and store as checkdigit for comparison
            if (length == 13)
            {
                checkdigit = Int32.Parse(cleanedInput.Substring(length - 1));
                cleanedInput = cleanedInput.Substring(0, cleanedInput.Length - 1);
                // Length remains at 13 for later checks
            }

            // String into numbers => then Mod10 sum => then retrieve checkdigit
            int[] numbers = SplitIntoNumbers(cleanedInput);
            sum = Mod10(numbers);
            int _checkdigit = GetCheckDigit(sum);

            // Compare digits, if the input string length was 13 (The last number is the checkdigit)
            if (checkdigit != _checkdigit && length == 13)
                return false;

            // Assign the correct check digit
            checkdigit = _checkdigit;

            if (!IsValidCheckDigit)
                return false;

            return true;
        }

        /// <summary><para>Checkdigit  from Mod10 calculation</para>
        /// <para>The checkdigit is the remainder between the sum and the next multiple of 10</para>
        /// <para>E.g. if sum is 127, the next 10multiple is 130. So 3 is the checkdigit</para></summary>
        private int GetCheckDigit(int n) => 10 - (n % 10) % 10;

        /// <summary>Return a calculated sum of incoming digits</summary>
        private int Mod10(params int[] numbers)
        {
            // Even since we start counting from 11
            bool IsEven;
            // Total sum
            int sum = 0;
            for (int j = numbers.Length - 1; j > -1; j--)
            {
                int n = numbers[j];

                // Get odd/even status since we shifting between multiplying 1 or 3
                IsEven = j % 2 == 0 ? true : false;

                if (IsEven)
                    sum += n * 1;
                else
                    sum += n * 3;
            }

            return sum;
        }

        /// <summary>String into int array of single digits</summary>
        private int[] SplitIntoNumbers(string code)
        {
            int i = 0;
            int[] nl = new int[12];
            foreach (char c in code)
            {
                nl[i] = Int32.Parse(c.ToString());
                i++;
            }
            return nl;
        }
    }
}
