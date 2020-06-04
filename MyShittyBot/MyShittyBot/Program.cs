using System;

namespace MyShittyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot shit = new Bot();

            shit.Connect(true);

            Console.ReadLine();

            shit.Disconnect();
        }
    }
}
