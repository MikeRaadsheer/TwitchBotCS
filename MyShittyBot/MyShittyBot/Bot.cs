using System;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace MyShittyBot
{
    internal class Bot
    {
        ConnectionCredentials creds = new ConnectionCredentials(TwitchInfo.ChannelName, TwitchInfo.BotToken);
        TwitchClient client;


        internal void Connect(bool isLogging)
        {
            client = new TwitchClient();
            client.Initialize(creds, TwitchInfo.ChannelName);

            Console.WriteLine("[Bot]: Connecting...");

            if (isLogging)
                client.OnLog += Client_OnLog;

            client.OnError += Client_OnError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;

            client.Connect();
            client.OnConnected += Client_OnConnected;
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            switch (e.Command.CommandText.ToLower())
            {
                case "dice":
                    client.SendMessage(TwitchInfo.ChannelName, "Ain't got no dice");
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
    }
}