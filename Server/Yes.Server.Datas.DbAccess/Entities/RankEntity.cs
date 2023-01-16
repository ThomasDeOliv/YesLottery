using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.DbAccess.Entities
{
    /// <summary>
    /// Rank table representation
    /// </summary>
    [Table("rank"), ExcludeFromCodeCoverage]
    public class RankEntity
    {
        /// <summary>
        /// Unique id of Rank record
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Required, Column("descriptor"), StringLength(maximumLength: 50)]
        public string Descriptor { get; set; }

        /// <summary>
        /// Related Tickets collection
        /// </summary>
        [InverseProperty(nameof(TicketEntity.Rank))]
        public virtual ICollection<TicketEntity> Tickets { get; set; }

        /// <summary>
        /// Related Statistics collection
        /// </summary>
        [InverseProperty(nameof(StatisticEntity.Rank))]
        public virtual ICollection<StatisticEntity> Statistics { get; set; }
    }
}
