using System;
using System.Collections.Generic;

using ChatSharp;
using ChatSharp.Events;
using Computer.API;

namespace ModuleIRC
{
    public class ModuleIRC : ModuleBase
    {
        [Setting]
        private string servaddy = @"irc.esper.net";
        [Setting]
        private string channel = "";
        [Setting]
        private string nick = "Guest12345";
        [Setting]
        private string password = "";
        [Setting]
        private string trigger = ".";


        private IrcClient client;

        public ModuleIRC()
        {
            
        }
        protected override void Load()
        {
            IrcUser user = new IrcUser(this.nick, "testing", this.password);
            this.client = new IrcClient(this.servaddy, user);
            this.client.ChannelMessageRecieved += ChannelMessageRecieved;
            this.client.ConnectionComplete += ConnectionComplete;
            this.client.UserJoinedChannel += UserJoinedChannel;
            

        }
        protected override void Start()
        {
            this.client.ConnectAsync();
        }

        private void ChannelMessageRecieved(object sender, PrivateMessageEventArgs e)
        {
            Log($"P){e.PrivateMessage.Message}");
            //Log($"I){e.IrcMessage.RawMessage}");
            Log($"U){e.PrivateMessage.User.Nick}");
            Log($"S){e.PrivateMessage.Source}");
            
        }

        private void ConnectionComplete(object sender, EventArgs e)
        {
            Log("Server joined");
            if (this.channel == "")
            {
                LogErr("No IRC channel currently set to join");
                return;
            }
            this.client.JoinChannel(this.channel);
            
        }

        private void UserJoinedChannel(object sender, ChannelUserEventArgs e)
        {
            
        }
    }
}
