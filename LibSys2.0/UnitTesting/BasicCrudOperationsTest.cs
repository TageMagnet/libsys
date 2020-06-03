using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using LibrarySystem.Models;
using Microsoft.Win32;
using LibrarySystem.ViewModels;
using LibrarySystem.ViewModels.Backend;
using System.Collections.Generic;
using LibrarySystem;

namespace UnitTesting
{
    [TestClass]
    public class BasicCrudOperationsTest
    {

        [TestMethod]
        public void ListOfISBNcode_IsValid_ExpectAllToBeTrue()
        {
            // Input ISBN codes
            List<string> codes = new List<string>() {
                "9789113069760",
                "9780816043132",
                "9789127136991",
                "9780393971682",
                "9789113107370",
                "9789185957132",
                "9789176291139",
                "9789187301629",
                "9789187043635",
                "9789187043161"
            };

            // List of boolean result after validation
            List<bool> result = new List<bool>();

            // List of expected result
            List<bool> expectedResult = new List<bool>();
            foreach (var _ in codes)
            {
                // Match the number of elements in codes;
                expectedResult.Add(true);
            }

            foreach (string code in codes)
            {
                var isbn = new ISBN();
                bool wasValid = isbn.IsValid(code);
                //
                result.Add(wasValid);
            }

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
