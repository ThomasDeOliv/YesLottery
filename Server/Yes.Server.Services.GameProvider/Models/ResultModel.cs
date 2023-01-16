using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Services.ResultProvider.Models
{
    /// <summary>
    /// Representation of a result sended to client
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ResultModel
    {
        /// <summary>
        /// Access Code
        /// </summary>
        [StringLength(maximumLength: 22, MinimumLength = 22)]
        public string AccessCode { get; set; }

        /// <summary>
        /// Played numbers
        /// </summary>
        [StringLength(maximumLength: 17, MinimumLength = 17)]
        public string PlayedNumbers { get; set; }

        /// <summary>
        /// Rank of the client game
        /// </summary>
        public int GameRank { get; set; }

        /// <summary>
        /// DateTime of the client game
        /// </summary>
        public DateTime GameDateTime { get; set; }

        /// <summary>
        /// Related draw
        /// </summary>
        public DrawModel Draw { get; set; }
    }
}
