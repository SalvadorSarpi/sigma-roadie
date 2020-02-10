using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class Setlist
    {
        public Setlist()
        {
            SetlistScene = new HashSet<SetlistScene>();
        }

        [Key]
        public Guid SetlistId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsActive { get; set; }

        [InverseProperty("Setlist")]
        public virtual ICollection<SetlistScene> SetlistScene { get; set; }
    }
}
