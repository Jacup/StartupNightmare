using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using StartupNightmare.Model.People;

namespace StartupNightmare.Api.Interfaces
{
    internal interface IServer
    {
        Task<Player> GetPlayer (TcpClient tcpClient);
    }
}
