using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sigma.Roadie.Domain
{

    public interface IOrchestratorHub
    {

        Task PlayMedia(string json);

        Task StopMedia(Guid mediaFileId);

        Task StopAll();

        Task StatusUpdate(string json);

    }

}
