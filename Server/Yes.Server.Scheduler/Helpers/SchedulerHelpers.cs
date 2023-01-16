using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Yes.Server.Scheduler.Helpers
{
    /// <summary>
    /// Static class providing a library of functions for specific actions in this assembly
    /// </summary>
    public static class SchedulerHelpers
    {
        /// <summary>
        /// Function to generate a random ordered numbers string
        /// </summary>
        /// <param name="regex">Regex matching a draw combination of 6 numbers, composed with two digits, separated with comas</param>
        /// <returns>Expected string</returns>
        public static string RandomOrderedCombinationGenerator(Regex regex)
        {
            // Declaring empty string container
            string generatedString = string.Empty;

            // Loop while the result not matching provided regex
            while (!regex.IsMatch(generatedString))
            {
                // Instanciate a new Random instance
                Random rnd = new Random();

                // Declaration and instanciation of a List containing ints
                List<int> combinationMembers = new List<int>();

                // Generate 6 random numbers between 1 and 49, includes, and adding them to the previous declared list
                do
                {
                    int toAdd = rnd.Next(1, 50);

                    if (!combinationMembers.Contains(toAdd))
                    {
                        combinationMembers.Add(toAdd);
                    }
                }
                while (combinationMembers.Count < 6);

                // Sorted the generated numbers
                combinationMembers.Sort();

                // Creating a formated array of strings
                string[] combination = combinationMembers.Select(n => n.ToString().Length == 1 ? '0' + n.ToString() : n.ToString()).ToArray();

                // Join all strings in array with comas
                generatedString = string.Join(',', combination);
            }

            // Return the generated string
            return generatedString;
        }
    }
}
