using Newtonsoft.Json;
using Sigma.Roadie.Domain.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class MediaFile
    {

        [JsonIgnore]
        [NotMapped]
        public MediaFileType TypeEnum
        {
            get => (MediaFileType)this.Type;
            set => this.Type = (short)value;
        }

    }
}
