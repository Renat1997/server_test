using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;


namespace TipaServer
{
    class Program
    {  
        public static void Main(string[] args)
        {
            Connector connector = new Connector();
            connector.Server();
          
        }

        
    }
  
}