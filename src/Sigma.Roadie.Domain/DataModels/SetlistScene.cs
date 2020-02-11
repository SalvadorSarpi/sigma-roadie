using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class SetlistScene
    {
        [Key]
        public Guid SetlistId { get; set; }
        [Key]
        public Guid SceneId { get; set; }
        public short Index { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(SceneId))]
        [InverseProperty("SetlistScene")]
        public virtual Scene Scene { get; set; }
        [ForeignKey(nameof(SetlistId))]
        [InverseProperty("SetlistScene")]
        public virtual Setlist Setlist { get; set; }
    }
}
