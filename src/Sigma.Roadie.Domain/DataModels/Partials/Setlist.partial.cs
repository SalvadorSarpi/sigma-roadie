using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace Sigma.Roadie.Domain.DataModels
{

    public partial class Setlist
    {

        [NotMapped]
        public TimeSpan TotalLenght => new TimeSpan(SetlistScene.Select(q => q.Scene.Duration.Ticks).Sum());

    }

}
