using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Repositories;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Datas.DbAccess;

namespace Yes.Tests.Datas.Business.Repositories
{
    [ExcludeFromCodeCoverage]
    public class DrawsRepositoryTests : IDisposable
    {
        private IMapper _mapper;
        private YesDbContext _ctx;
        private IDrawsRepository _drawsRepository;

        public DrawsRepositoryTests()
        {
            _mapper = TestsHelpers.GetTestsMapper();
            _ctx = TestsHelpers.GetTestsContext();

            _drawsRepository = new DrawsRepository(_mapper, _ctx);
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _drawsRepository = null;
        }

        [Fact]
        public async Task GetCurrentDrawAsync_TakeNoArgs_ReturnExpectedDraw()
        {
            // ARRANGE *****************************************************************

            var expected = new Draw()
            {
                Id = 3,
                DrawedNumbers = "03,15,19,27,35,37",
            };

            // ACT *********************************************************************

            var result = await _drawsRepository.GetCurrentDrawAsync();

            // ASSERT ******************************************************************

            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.DrawedNumbers, result.DrawedNumbers);
            Assert.True(result.StartDateTime < DateTime.UtcNow.AddMinutes(4));
            Assert.True(result.Statistics.Count == 5);
        }

        [Fact]
        public async Task CreateNewDrawAsync_TakeIDrawRepositoryInstance_CreateNewDraw()
        {
            // ARRANGE *****************************************************************

            int expectedId = 4;

            // ACT *********************************************************************

            var result = await _drawsRepository.CreateNewDrawAsync();
            var registredDraw = await _ctx.Draws.OrderBy(d => d).LastOrDefaultAsync();

            // ASSERT ******************************************************************

            Assert.Equal(expectedId, result);
            Assert.Equal(expectedId, registredDraw.Id);
            Assert.Null(registredDraw.DrawedNumbers);
            Assert.True(registredDraw.StartDateTime < DateTime.UtcNow);
        }

        [Fact]
        public async Task CloseCurrentDraw_TakeIDrowRepositoryAndRandomValidCombination_CloseTheCurrentDraw()
        {
            // ARRANGE *****************************************************************

            int expected = 1;
            int newDrawId = await _drawsRepository.CreateNewDrawAsync();
            string combination = "01,02,03,04,05,06";

            // ACT *********************************************************************

            var result = await _drawsRepository.CloseDrawAsync(newDrawId, combination);
            var registredDraw = await _ctx.Draws.FirstOrDefaultAsync(d => d.DrawedNumbers == combination);

            // ASSERT ******************************************************************

            Assert.Equal(expected, result);
            Assert.Equal(combination, registredDraw.DrawedNumbers);
        }
    }
}