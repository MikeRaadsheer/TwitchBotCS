using System;

namespace MyShittyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot shit = new Bot();

            shit.Connect(false);

            Console.ReadLine();

            shit.Disconnect();
        }
    }
}
