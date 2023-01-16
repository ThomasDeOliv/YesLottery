using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.DbAccess.Entities
{
    /// <summary>
    /// Statistic table representation
    /// </summary>
    [Table("statistic"), ExcludeFromCodeCoverage]
    public class StatisticEntity
    {
        /// <summary>
        /// Unique id of Statistic record
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Number of people by Rank
        /// </summary>
        [Required, Column("people_by_rank")]
        public int PeopleByRank { get; set; }

        /// <summary>
        /// Unique id of related Draw
        /// </summary>
        [Required, Column("fk_draw_id")]
        public int FKDrawId { get; set; }

        /// <summary>
        /// Related Draw
        /// </summary>
        [ForeignKey(nameof(FKDrawId))]
        public DrawEntity Draw { get; set; }

        /// <summary>
        /// Unique id of related Rank
        /// </summary>
        [Required, Column("fk_rank_id")]
        public int FKRankId { get; set; }

        /// <summary>
        /// Related Rank
        /// </summary>
        [ForeignKey(nameof(FKRankId))]
        public RankEntity Rank { get; set; }
    }
}
