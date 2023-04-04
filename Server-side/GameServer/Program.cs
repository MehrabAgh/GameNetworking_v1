using System;
using System.Text;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Server";

            Console.WriteLine("Hello Create Back-End Server with C#.Net ");

            Server.Start(10 , 26950);

            Console.ReadKey();
        }
       
    }
}
