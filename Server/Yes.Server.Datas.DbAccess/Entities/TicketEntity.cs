using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.DbAccess.Entities
{
    /// <summary>
    /// Ticket table representation
    /// </summary>
    [Table("ticket"), ExcludeFromCodeCoverage]
    public class TicketEntity
    {
        /// <summary>
        /// Unique id of Ticket record
        /// </summary>
        [Key, Column("private_id")]
        public int Id { get; set; }

        /// <summary>
        /// Unique generated access code
        /// </summary>
        [Required, Column("access_code"), StringLength(maximumLength: 22, MinimumLength = 22)]
        public string AccessCode { get; set; }

        /// <summary>
        /// Played Numbers
        /// </summary>
        [Required, Column("played_numbers"), StringLength(maximumLength: 17, MinimumLength = 17)]
        public string PlayedNumbers { get; set; }

        /// <summary>
        /// DateTime of the play
        /// </summary>
        [Required, Column("played_datetime")]
        public DateTime GameDateTime { get; set; }

        /// <summary>
        /// Unique id of related Draw
        /// </summary>
        [Required, Column("fk_draw_id")]
        public int FKDrawId { get; set; }

        /// <summary>
        /// Related Draw
        /// </summary>
        [ForeignKey(nameof(FKDrawId))]
        public virtual DrawEntity Draw { get; set; }

        /// <summary>
        /// Unique id of related Rank
        /// </summary>
        [Required, Column("fk_rank_id")]
        public int FKRankId { get; set; }

        /// <summary>
        /// Related Rank
        /// </summary>
        [ForeignKey(nameof(FKRankId))]
        public virtual RankEntity Rank { get; set; }
    }
}
