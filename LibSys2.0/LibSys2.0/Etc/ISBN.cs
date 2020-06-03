using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    /// <summary>
    /// Validation of ISBN 13-character number
    /// </summary>
    public class ISBN
    {
        public bool IsValid(string isbnNumber)
        {
            // Incorrect length
            if (isbnNumber.Length != 13)
                return false;

            // Store the original last digit
            int originalCheckdigit = Int32.Parse(isbnNumber.Substring(isbnNumber.Length - 1));

            // Truncated to 12 characters
            isbnNumber = isbnNumber.Substring(0, isbnNumber.Length - 1);

            // Sum of modulus calculated
            int sum = GetMod10(isbnNumber);

            // Checkdigit is the remainder to the closest base-10
            var checkdigit = 10 - (sum % 10);

            // 10 == 0
            if (checkdigit == 10)
                checkdigit = 0;

            // Check against input check digit
            if (originalCheckdigit != checkdigit)
                return false;

            return true;
        }

        public int GetMod10(string numbers)
        {
            // Sum of modulus
            int sum = 0;
            int counter = 0;

            foreach (char c in numbers)
            {
                int x = Int32.Parse(c.ToString());

                if (counter % 2 == 0)
                    sum += x * 1;
                else
                    sum += x * 3;

                counter++;
            }
            return sum;
        }
    }
}
