using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.DbAccess.Entities
{
    /// <summary>
    /// Draw table representation
    /// </summary>
    [Table("draw"), ExcludeFromCodeCoverage]
    public class DrawEntity
    {
        /// <summary>
        /// Unique id of Draw record
        /// </summary>
        [Key, Column("private_id")]
        public int Id { get; set; }

        /// <summary>
        /// Drawed numbers
        /// </summary>
        [Column("drawed_numbers"), StringLength(maximumLength: 17, MinimumLength = 17)]
        public string DrawedNumbers { get; set; }

        /// <summary>
        /// DateTime of Draw
        /// </summary>
        [Required, Column("start_datetime")]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Related Tickets collection
        /// </summary>
        [InverseProperty(nameof(TicketEntity.Draw))]
        public virtual ICollection<TicketEntity> Tickets { get; set; }

        /// <summary>
        /// Related Statistics collection
        /// </summary>
        [InverseProperty(nameof(StatisticEntity.Draw))]
        public virtual ICollection<StatisticEntity> Statistics { get; set; }
    }
}
