using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Services.ResultProvider.Models
{
    /// <summary>
    /// Representation of a draw sended to client
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DrawModel
    {
        /// <summary>
        /// Drawed numbers
        /// </summary>
        [StringLength(maximumLength: 17, MinimumLength = 17)]
        public string DrawedNumbers { get; set; }

        /// <summary>
        /// DateTime of the game
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Dictionary with all related statistics of this draw
        /// </summary>
        public Dictionary<string,int> Statistics { get; set; }
    }
}
