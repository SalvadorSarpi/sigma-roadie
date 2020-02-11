using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigma.Roadie.Domain.DataModels
{
    public partial class RoadieEntities
    {

        public void ResetContextState() => ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

    }
}
