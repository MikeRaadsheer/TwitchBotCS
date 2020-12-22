using System;
using TwitchLib.Api.V5;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace MyShittyBot
{
    internal class Bot
    {
        ConnectionCredentials creds = new ConnectionCredentials(TwitchInfo.ChannelName, TwitchInfo.BotToken);
        TwitchClient client;

        char nl = Convert.ToChar(11);

        private string[] _bannedWords = new string[3] { "big follow", "retard", "js is best" };


        internal void Connect(bool isLogging)
        {
            client = new TwitchClient();
            client.Initialize(creds, TwitchInfo.ChannelName);
            client.OnConnected += Client_OnConnected;

            Console.WriteLine("[Bot]: Connecting...");

            if (isLogging)
                client.OnLog += Client_OnLog;

            client.OnError += Client_OnError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
            client.AddChatCommandIdentifier('$');

            client.Connect();
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            switch (e.Command.CommandText.ToLower())
            {
                case "roll":
                    string msg = $"{e.Command.ChatMessage.DisplayName} Rolled {RndInt(1, 6)}";
                    client.SendMessage(TwitchInfo.ChannelName, msg);
                    Console.WriteLine($"[Bot]: {msg}");
                    break;
                case "social":
                    client.SendMessage(TwitchInfo.ChannelName, "Here are all my social links! YouTube: http://bit.ly/3p01GJD Twitter: https://bit.ly/369XH5f Discord: http://bit.ly/36h7zMm.");
                    break;
                case "help":
                    client.SendMessage(TwitchInfo.ChannelName, "There is a panel with all commands and a link to the github repo below, please check it out!");
                    break;
            }

            if (e.Command.ChatMessage.DisplayName == TwitchInfo.ChannelName)
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "hi":
                        client.SendMessage(TwitchInfo.ChannelName, "Hi Boss");
                        break;
                }
            }
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            for (int i = 0; i < _bannedWords.Length; i++)
            {
                if (e.ChatMessage.Message.ToLower().Contains(_bannedWords[i]))
                {
                    client.BanUser(TwitchInfo.ChannelName, e.ChatMessage.Username);
                }
            }
            
            Console.WriteLine($"[{e.ChatMessage.DisplayName}]: {e.ChatMessage.Message}");
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private void Client_OnError(object sender, OnErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine("[Bot]: Connected");
        }

        internal void Disconnect()
        {
            Console.WriteLine("[Bot]: Disonnecting and closing application");

            client.Disconnect();
        }

        private int RndInt(int min, int max)
        {
            int value;

            Random rnd = new Random();

            value = rnd.Next(min, max + 1);

            return value;
        }
    }
}