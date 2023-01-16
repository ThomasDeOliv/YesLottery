using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.Business.DTO
{
    /// <summary>
    /// Ticket DTO
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Ticket
    {
        /// <summary>
        /// Unique id of Ticket record
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Unique generated access code
        /// </summary>
        [Required, StringLength(maximumLength: 22, MinimumLength = 22)]
        public string AccessCode { get; set; }

        /// <summary>
        /// Played Numbers
        /// </summary>
        [Required, StringLength(maximumLength: 17, MinimumLength = 17)]
        public string PlayedNumbers { get; set; }

        /// <summary>
        /// DateTime of the play
        /// </summary>
        [Required]
        public DateTime GameDateTime { get; set; }

        /// <summary>
        /// Unique id of related Draw
        /// </summary>
        [Required]
        public int FKDrawId { get; set; }

        /// <summary>
        /// Related Draw
        /// </summary>
        public virtual Draw Draw { get; set; }

        /// <summary>
        /// Unique id of related Rank
        /// </summary>
        [Required]
        public int FKRankId { get; set; }

        /// <summary>
        /// Related Rank
        /// </summary>
        public virtual Rank Rank { get; set; }
    }
}
