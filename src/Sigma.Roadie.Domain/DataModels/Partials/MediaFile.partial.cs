using Sigma.Roadie.Domain.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class MediaFile
    {

        public MediaFileType TypeEnum
        {
            get => (MediaFileType)this.Type;
            set => this.Type = (short)value;
        }

    }
}
