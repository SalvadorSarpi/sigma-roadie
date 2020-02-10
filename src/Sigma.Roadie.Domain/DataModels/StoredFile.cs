using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class StoredFile
    {
        [Key]
        public Guid StoredFileId { get; set; }
        public Guid SceneId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Url1 { get; set; }
        [Required]
        public string Url2 { get; set; }

        [ForeignKey(nameof(SceneId))]
        [InverseProperty("StoredFile")]
        public virtual Scene Scene { get; set; }
    }
}
