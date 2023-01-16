using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Services.GameProvider.Helpers;

namespace Yes.Tests.Services.GameProvider.Helpers
{
    [ExcludeFromCodeCoverage]
    public class GameProviderHelpersTests : IDisposable
    {
        private ITicketsRepository _ticketsRepository;

        public GameProviderHelpersTests()
        {
            Mock<ITicketsRepository> mock = new Mock<ITicketsRepository>();
            mock.SetupSequence(m => m.GetTicketByAccessCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Ticket>())
                .ReturnsAsync(It.IsAny<Ticket>())
                .ReturnsAsync((Ticket)null);

            _ticketsRepository = mock.Object;
        }

        public void Dispose()
        {
            _ticketsRepository = null;
        }

        [Fact]
        public async Task GenerateUniqueIdAsync_TakeITicketsRepository_ReturnExpectedString()
        {
            // ARRANGE *****************************************************************

            

            // ACT *********************************************************************

            string result = await GameProviderServiceHelpers.GenerateUniqueIdAsync(_ticketsRepository);

            // ASSERT ******************************************************************

            Assert.True(result.Length == 22);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, "01,02,03,04,05,06")]
        [InlineData(new int[] { 1, 12, 13, 54, 65, 6 }, "01,12,13,54,65,06")]
        [InlineData(new int[] { 21, 0, 3, 4, 55, 56 }, "21,00,03,04,55,56")]
        [InlineData(new int[] { 121, 0, 3, 4, 55, 56 }, "121,00,03,04,55,56")]
        [InlineData(new int[] { 21, 2000, 3, 4, 55, 56 }, "21,2000,03,04,55,56")]
        [InlineData(new int[] { 21, 0, 3, 4, 5501, 56 }, "21,00,03,04,5501,56")]
        public void ConvertArrayOfIntsToString_TakeIntsArray_ReturnExpectedString(int[] ints, string expected)
        {
            // ARRANGE *****************************************************************



            // ACT *********************************************************************

            string result = GameProviderServiceHelpers.ConvertArrayOfIntsToString(ints);

            // ASSERT ******************************************************************

            Assert.Equal(expected, result);
        }
    }
}