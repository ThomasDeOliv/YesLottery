using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.Business.DTO
{
    /// <summary>
    /// Statistic DTO
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Statistic
    {
        /// <summary>
        /// Unique id of Statistic record
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Number of people by Rank
        /// </summary>
        [Required]
        public int PeopleByRank { get; set; }

        /// <summary>
        /// Unique id of related Draw
        /// </summary>
        [Required]
        public int FKDrawId { get; set; }

        /// <summary>
        /// Related Draw
        /// </summary>
        public Draw Draw { get; set; }

        /// <summary>
        /// Unique id of related Rank
        /// </summary>
        [Required]
        public int FKRankId { get; set; }

        /// <summary>
        /// Related Rank
        /// </summary>
        public Rank Rank { get; set; }
    }
}
