using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.Business.DTO
{
    /// <summary>
    /// Draw DTO
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Draw
    {
        /// <summary>
        /// Unique id of Draw record
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Drawed numbers
        /// </summary>
        [StringLength(maximumLength: 17, MinimumLength = 17)]
        public string DrawedNumbers { get; set; }

        /// <summary>
        /// DateTime of Draw
        /// </summary>
        [Required]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Related Tickets collection
        /// </summary>
        public virtual ICollection<Ticket> Tickets { get; set; }

        /// <summary>
        /// Related Statistics collection
        /// </summary>
        public virtual ICollection<Statistic> Statistics { get; set; }
    }
}
