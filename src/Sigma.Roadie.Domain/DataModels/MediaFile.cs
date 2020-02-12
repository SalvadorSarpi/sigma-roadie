using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class MediaFile
    {
        [Key]
        public Guid MediaFileId { get; set; }
        public Guid SceneId { get; set; }
        public short Type { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string LocalUri { get; set; }
        public TimeSpan? PlayAt { get; set; }

        [ForeignKey(nameof(SceneId))]
        [InverseProperty("MediaFile")]
        public virtual Scene Scene { get; set; }
    }
}
