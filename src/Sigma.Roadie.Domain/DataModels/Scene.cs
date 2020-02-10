using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class Scene
    {
        public Scene()
        {
            MediaFile = new HashSet<MediaFile>();
            SetlistScene = new HashSet<SetlistScene>();
        }

        [Key]
        public Guid SceneId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Scene")]
        public virtual ICollection<MediaFile> MediaFile { get; set; }
        [InverseProperty("Scene")]
        public virtual ICollection<SetlistScene> SetlistScene { get; set; }
    }
}
