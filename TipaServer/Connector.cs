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
    class Connector
    {
        public Connector()
        {
            adminContext db = new adminContext();
            var info = db.Infoes.Select(p => p.Port);

            foreach (int port in info)
            {
                Task.Run(() =>
                {
                    Connect(port);
                });
            }
        }

        private NetworkStream stream;

        public void Connect(int port)
        {
            var tcpClient = new TcpClient();
            while (true)
            {

                try
                {
                    if (tcpClient != null && tcpClient.Client != null && !tcpClient.Client.Connected)
                    {
                        tcpClient.Connect("127.0.0.1", port);
                        Console.WriteLine("Подключение к порту " + port + " " + DateTime.Now);
                        stream = tcpClient.GetStream();
                    }



                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(FromStringBitToEnum(message));
                }
                catch
                { }
            }
        }

        public enum Conversion
        {
            power,
            cycle,
            emergency,
            off
        }
        private string FromStringBitToEnum(string bit)
        {
            if (bit[2] == '1') //если последний элемент строки, которая пришла с клиента, равен 1, то вернем АВАРИЯ
            {
                return Conversion.emergency.ToString();
            }
            else if (bit[1] == ('1')) //если ПРЕДпоследний элемент строки, которая пришла с клиента, равен 1, то вернем ЦИКЛ
            {
                return Conversion.cycle.ToString();
            }
            else if (bit[0] == ('1')) //если ПЕРВЫЙ элемент строки, которая пришла с клиента, равен 1, то вернем ПИТАНИЕ
            {
                return Conversion.power.ToString();
            }
            else
            {
                return Conversion.off.ToString(); // иначе ВЫКЛЮЧЕНО
            }
        }

    }
}
