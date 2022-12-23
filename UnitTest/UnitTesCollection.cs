using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UnitTestCollection
    {
        [TestMethod]
        public void ConvertTest()
        {
            string test1 = "00:00";
            int[] expected1 = { 0, 0 };
            Assert.IsTrue(expected1.SequenceEqual(ConvertToTime(test1)));

            string test2 = "21:47";
            int[] expected2 = { 21, 47 };
            Assert.IsTrue(expected2.SequenceEqual(ConvertToTime(test2)));

            // 31:01 simply does not exist
            string test3 = "31:01";
            int[] expected3 = null;
            Assert.AreEqual(expected3, ConvertToTime(test3));

            // aaaaaaaa is not a time formatted string
            string test4 = "aaaaaaaa";
            int[] expected4 = null;
            Assert.AreEqual(expected4, ConvertToTime(test4));

            // nice try, but ab:cd is not a time
            string test5 = "ab:cd";
            int[] expected5 = null;
            Assert.AreEqual(expected5, ConvertToTime(test5));
        }
        private int[] ConvertToTime(string timeString)
        {
            char[] timeLetters = timeString.ToArray();
            if (timeLetters[2].Equals(":"))
            {
                return null;
            }
            if (timeString.Length != 5)
            {
                return null;
            }

            char[] hourChar = { timeLetters[0], timeLetters[1] };
            string hourString = new string(hourChar);
            int hour;
            bool isValid = int.TryParse(hourString, out hour);
            if (!isValid || hour > 24)
            {
                return null;
            }

            char[] minuteChar = { timeLetters[3], timeLetters[4] };
            string minuteString = new string(minuteChar);
            int minute;
            isValid = int.TryParse(minuteString, out minute);
            if (!isValid || minute > 60)
            {
                return null;
            }

            int[] time = { hour, minute };
            return time;
        }
    }
}
