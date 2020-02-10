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
            SetlistScene = new HashSet<SetlistScene>();
            StoredFile = new HashSet<StoredFile>();
        }

        [Key]
        public Guid SceneId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Scene")]
        public virtual ICollection<SetlistScene> SetlistScene { get; set; }
        [InverseProperty("Scene")]
        public virtual ICollection<StoredFile> StoredFile { get; set; }
    }
}
