using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.Business.DTO
{
    /// <summary>
    /// Rank DTO
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Rank
    {
        /// <summary>
        /// Unique id of Rank record
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Required, StringLength(maximumLength: 50)]
        public string Descriptor { get; set; }

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
