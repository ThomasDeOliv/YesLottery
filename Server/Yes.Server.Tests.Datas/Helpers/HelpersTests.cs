using Yes.Server.Datas.Business.Helpers;

namespace Yes.Server.Tests.Datas.Helpers
{
    public class HelpersTests
    {
        [Theory]
        [InlineData("01,02,03,04,05,06", "01,02,03,04,05,06", 1)]
        [InlineData("01,02,03,04,05,06", "01,02,03,04,05,07", 2)]
        [InlineData("01,02,03,04,05,06", "01,02,03,04,07,08", 3)]
        [InlineData("01,02,03,04,05,06", "01,02,03,07,08,09", 4)]
        [InlineData("01,02,03,04,05,06", "07,08,09,10,11,12", 4)]
        public void CalculateRankAsync_UseThisUserStringCombinationAndTakeDrawCombination_ReturnExpectedRank(string userCombination, string drawCombination, int expected)
        {
            // ARRANGE *****************************************************************


            // ACT *********************************************************************

            var result = BusinessHelpers.CalculateRankAsync(userCombination, drawCombination);

            // ASSERT ******************************************************************

            Assert.Equal(expected, result);
        }
    }
}
