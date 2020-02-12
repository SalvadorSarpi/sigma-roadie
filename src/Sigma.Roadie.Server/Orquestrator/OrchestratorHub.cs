using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Sigma.Roadie.Domain;
using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sigma.Roadie.Server.Orquestrator
{

    public class OrchestratorHub : Hub<IOrchestratorHub>
    {


    }

}
