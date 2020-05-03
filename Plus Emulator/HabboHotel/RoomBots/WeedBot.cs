﻿using System;
using System.Linq;
using System.Threading;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using System.Collections.Generic;
using System.Drawing;
using Plus.HabboHotel.PathFinding;
using Plus.HabboHotel.Roleplay.Combat;
using Plus.HabboHotel.Roleplay.Misc;

namespace Plus.HabboHotel.RoomBots
{
    class WeedBot : BotAI
    {
        private static readonly Random Random = new Random();

        private readonly int _id;
        private readonly int _virtualId;
        private readonly bool _isBartender;

        private int _actionCount = 30;
        private int _speechInterval = 7;
        private Timer _chatTimer;

        internal WeedBot(RoomBot roomBot, int virtualId, int botId, AIType type, bool isBartender, int speechInterval)
        {
            _id = botId;
            _virtualId = virtualId;
            _isBartender = isBartender;
            _speechInterval = speechInterval < 2 ? 2000 : speechInterval * 1000;

            // Get random speach
            if (roomBot.AutomaticChat && roomBot.RandomSpeech != null && roomBot.RandomSpeech.Any()) _chatTimer = new Timer(ChatTimerTick, null, _speechInterval, _speechInterval);
            _actionCount = Random.Next(10, 30 + virtualId);
        }

        internal override void Modified()
        {
            if (GetBotData() == null) return;
            if (!GetBotData().AutomaticChat || GetBotData().RandomSpeech == null || !GetBotData().RandomSpeech.Any())
            {
                StopTimerTick();
                return;
            }
            _speechInterval = GetBotData().SpeechInterval < 2 ? 2000 : GetBotData().SpeechInterval * 1000;

            if (_chatTimer == null)
            {
                _chatTimer = new Timer(ChatTimerTick, null, _speechInterval, _speechInterval);
                return;
            }
            _chatTimer.Change(_speechInterval, _speechInterval);
        }

        internal override void OnSelfEnterRoom()
        {

        }

        internal override void OnSelfLeaveRoom(bool Kicked)
        {

        }

        internal override void OnUserEnterRoom(Rooms.RoomUser User)
        {

        }

        internal override void OnUserLeaveRoom(GameClients.GameClient Client)
        {

        }

        internal override void OnUserSay(Rooms.RoomUser User, string Message)
        {
            if (User.IsBot || User.GetClient() == null)
            {
                return;
            }
            if (User.GetClient().GetHabbo() == null)
            {
                return;
            }

            if (!User.GetClient().GetHabbo().CurrentRoom.RoomData.Description.Contains("DRUGDEPOT"))
            {
                return;
            }

            if (Message != "sell weed" && Message != "weed sell" && Message != "sell weed please")
            {
                return;
            }

            GameClients.GameClient Session = User.GetClient();

            if (Session.GetRoleplay().Phone == 0)
            {
                int Weed = Session.GetRoleplay().Weed;

                //base.GetRoomUser().Chat(null, "Hmm lets see, you have got " + Weed + "g of weed to sell.. I'll offer you $" + Weed * 1 + ", take it or leave it.", true, 1);
                //Session.SendWhisper("You have been offered $" + Weed * 1 + " for all of your weed. Type #accept to accept or #deny to deny!");
                //Session.GetRoleplay().OfferData.Add("weed", new Offer(Session, "weed", Weed, Weed * 1));
                base.GetRoomUser().Chat(null, "Offers " + Session.GetHabbo().UserName + " a phone for $20", true, 1);
                Session.SendWhisper("You have been offered a phone for $20. Type #accept to accept or #deny to deny.");
                //Session.GetRoleplay().OfferData.Add("weed", new Offer(Session, "weed", Weed, Weed * 1));
                Offer OfferPhone = new Offer(Session, "phone", 1, 20);

                Session.GetRoleplay().OfferData.Add("phone", OfferPhone);
               
                return;
            }
            else
                User.GetClient().SendWhisper("You already have a phone");
            return;
                    
                    
                
                /*
                if (Gamemap.TileDistance(GetRoomUser().X, GetRoomUser().Y, User.X, User.Y) > 8)
                {
                    return;
                }

                BotResponse Response = GetBotData().GetResponse(Message);

                if (Response == null)
                {
                    return;
                }

                switch (Response.ResponseType.ToLower())
                {
                    case "say":

                        GetRoomUser().Chat(null, Response.ResponseText, false, 0);
                        break;

                    case "shout":

                        GetRoomUser().Chat(null, Response.ResponseText, true, 0);
                        break;

                    case "whisper":

                        User.GetClient().SendMessage(new WhisperComposer(GetRoomUser(), Response.ResponseText, 0, 0));
                        break;
                }

                if (Response.ServeId >= 1)
                {
                    User.CarryItem(Response.ServeId);
                }*/
            
        }

        internal override void OnUserShout(Rooms.RoomUser User, string Message)
        {
            /*
            if (PlusEnvironment.GetRandomNumber(0, 10) >= 5)
            {
                GetRoomUser().Chat(null, LanguageLocale.GetValue("onusershout"), true); // shout nag
            }*/
        }

        internal override void OnTimerTick()
        {
            //string Message = "";
            if (base.GetBotData() != null)
            {

                /*if (base.GetBotData().Name.ToLower().Contains("weed"))
                {

                    if (base.GetRoomUser().RotBody != 4)
                    {
                        base.GetRoomUser().RotBody = 4;
                        base.GetRoomUser().RotHead = 4;
                      
                    }

                    if (!base.GetRoomUser().Statusses.ContainsKey("sit"))
                    {
                        base.GetRoomUser().Statusses.Add("sit", "1.0");
                        base.GetRoomUser().Z = 0.00;
                        base.GetRoomUser().IsSitting = true;
                        base.GetRoomUser().UpdateNeeded = true;
                    }
                }*/

               // base.GetRoomUser().Chat(null, "Lol", true,1);
            }
        }
        private void StopTimerTick()
        {
            if (_chatTimer == null) return;
            _chatTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _chatTimer.Dispose();
            _chatTimer = null;
        }

        /// <summary>
        /// Chats the timer tick.
        /// </summary>
        /// <param name="o">The o.</param>
        private void ChatTimerTick(object o)
        {
            if (GetBotData() == null || GetRoomUser() == null || GetBotData().WasPicked || GetBotData().RandomSpeech == null ||
                !GetBotData().RandomSpeech.Any())
            {
                StopTimerTick();
                return;
            }

            if (GetRoom() != null && GetRoom().MutedBots)
                return;

            var randomSpeech = GetBotData().GetRandomSpeech(GetBotData().MixPhrases);

            try
            {
                switch (randomSpeech)
                {
                    case ":sit":
                        {
                            var user = GetRoomUser();
                            if (user.RotBody % 2 != 0) user.RotBody--;

                            user.Z = GetRoom().GetGameMap().SqAbsoluteHeight(user.X, user.Y);
                            if (!user.Statusses.ContainsKey("sit"))
                            {
                                user.UpdateNeeded = true;
                                user.Statusses.Add("sit", "0.55");
                            }
                            user.IsSitting = true;
                            return;
                        }
                    case ":stand":
                        {
                            var user = GetRoomUser();
                            if (user.IsSitting)
                            {
                                user.Statusses.Remove("sit");
                                user.IsSitting = false;
                                user.UpdateNeeded = true;
                            }
                            else if (user.IsLyingDown)
                            {
                                user.Statusses.Remove("lay");
                                user.IsLyingDown = false;
                                user.UpdateNeeded = true;
                            }
                            return;
                        }
                }

                if (GetRoom() != null)
                {
                    randomSpeech = randomSpeech.Replace("%user_count%",
                        GetRoom().GetRoomUserManager().GetRoomUserCount().ToString());
                    randomSpeech = randomSpeech.Replace("%item_count%",
                        GetRoom().GetRoomItemHandler().TotalItems.ToString());
                    randomSpeech = randomSpeech.Replace("%floor_item_count%",
                        GetRoom().GetRoomItemHandler().FloorItems.Keys.Count.ToString());
                    randomSpeech = randomSpeech.Replace("%wall_item_count%",
                        GetRoom().GetRoomItemHandler().WallItems.Keys.Count.ToString());

                    if (GetRoom().RoomData != null)
                    {
                        randomSpeech = randomSpeech.Replace("%roomname%", GetRoom().RoomData.Name);
                        randomSpeech = randomSpeech.Replace("%owner%", GetRoom().RoomData.Owner);
                    }
                }
                if (GetBotData() != null) randomSpeech = randomSpeech.Replace("%name%", GetBotData().Name);

                GetRoomUser().Chat(null, randomSpeech, false, 0, 0);
            }
            catch (Exception e)
            {
                Writer.Writer.LogException(e.ToString());
            }
        }
    }
}