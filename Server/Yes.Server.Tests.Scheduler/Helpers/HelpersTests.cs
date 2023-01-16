using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Yes.Server.Scheduler.Helpers;

namespace Yes.Azure.Tests.Function.Helpers
{
    [ExcludeFromCodeCoverage]
    public class HelpersTests
    {
        [Theory]
        [InlineData("12,17,12,11,01,03", true)]
        [InlineData("01,11,12,13,15,23", true)]
        [InlineData("00,17,12,11,01,49", false)]
        [InlineData("12,17,12,11,01,50", false)]
        [InlineData("12,17,12,11,01,03,", false)]
        [InlineData(",12,17,12,11,01,03", false)]
        [InlineData("01,12,17,12,11,01,03", false)]
        [InlineData("17,12,11,01", false)]
        [InlineData("12,17,12,11,1,03", false)]
        [InlineData(",0112,17,12,11,01", false)]
        [InlineData("01,12,17,12,1101,", false)]
        public void RegexUsedForGenerateOrderedCombination(string toTest, bool expected)
        {
            // ARRANGE *****************************************************************

            Regex regex = new Regex("^(0[1-9]|[1-4][0-9])+(,((0[1-9]|[1-4][0-9]))){5}$");

            // ACT *********************************************************************

            var result = regex.IsMatch(toTest);

            // ASSERT ******************************************************************

            Assert.Equal(expected, result);

            if (expected)
            {
                Assert.True(toTest.Length == 17);
                Assert.True(toTest.Count(c => c == ',') == 5);
                Assert.True(toTest.Count(char.IsNumber) == 12);
                Assert.DoesNotContain(",,", toTest);
                Assert.False(toTest.StartsWith(','));
                Assert.False(toTest.EndsWith(','));
            }
        }

        [Fact]
        public void RandomOrderedCombinationGenerator_NoArgs_ReturnExpectedCombinationStringOrderedAndDistinct()
        {
            // ARRANGE *****************************************************************

            Regex regex = new Regex("^(0[1-9]|[1-4][0-9])+(,((0[1-9]|[1-4][0-9]))){5}$");

            // ACT *********************************************************************

            var result = SchedulerHelpers.RandomOrderedCombinationGenerator(regex);

            // ASSERT ******************************************************************

            Assert.Matches(@"^(0[1-9]|[1-4][0-9])+(,((0[1-9]|[1-4][0-9]))){5}$", result);

            List<int> convertedResult = result.Split(',').Select(int.Parse).ToList();

            Assert.Equivalent(convertedResult, convertedResult.OrderBy(n => n));
            Assert.Equivalent(convertedResult, convertedResult.Distinct());
        }
    }
}