﻿using System;
using System.Linq;
using System.Threading;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Roleplay.Minigames.Mafia_Wars;
using Plus.HabboHotel.PathFinding;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.Database.Manager.Database.Session_Details.Interfaces;
using Plus.Configuration;
using Plus.Database.Manager.Database.Session_Details.Interfaces;
using Plus.HabboHotel.Events;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Navigators;
using Plus.HabboHotel.Roleplay.Minigames;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using Plus.HabboHotel.Roleplay.Minigames.Colour_Wars;
using System.Data;
using System.Linq;
using Plus.HabboHotel.Roleplay.Jobs.Space;
using Plus.HabboHotel.Roleplay.Jobs.Cutting;
using Plus.HabboHotel.Roleplay.Apartments;
using Plus.HabboHotel.Roleplay.Jobs.Farming;
using Plus.HabboHotel.Roleplay.Casino.Slots;

namespace Plus.HabboHotel.RoomBots
{
    /// <summary>
    /// Class GenericBot.
    /// </summary>
    internal class PoliceBot : BotAI
    {
        /// <summary>
        /// The random
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// The _id
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// The _virtual identifier
        /// </summary>
        private readonly int _virtualId;

        /// <summary>
        /// The _is bartender
        /// </summary>
        private readonly bool _isBartender;

        /// <summary>
        /// The _action count
        /// </summary>
        private int _actionCount;

        /// <summary>
        /// The _speech interval
        /// </summary>
        private int _speechInterval;

        /// <summary>
        /// The _chat timer
        /// </summary>
        private Timer _chatTimer;

        /// <summary>
        /// Times the attack
        /// </summary>
        private Timer _AttackTimer;

        /// <summary>
        /// Times the cooldown
        /// </summary>
        private Timer _CoolDownTimer;

        /// <summary>
        /// The victim
        /// </summary>
        public RoomUser _Victim;



        /// <summary>
        /// Is the bot cooling down
        /// </summary>
        public bool _CoolingDown = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBot"/> class.
        /// </summary>
        /// <param name="roomBot">The room bot.</param>
        /// <param name="virtualId">The virtual identifier.</param>
        /// <param name="botId">The bot identifier.</param>
        /// <param name="type">The type.</param>
        /// <param name="isBartender">if set to <c>true</c> [is bartender].</param>
        /// <param name="speechInterval">The speech interval.</param>
        internal PoliceBot(RoomBot roomBot, int virtualId, int botId, AIType type, bool isBartender, int speechInterval)
        {
            _id = botId;
            _virtualId = virtualId;
            _isBartender = isBartender;
            _speechInterval = speechInterval < 2 ? 2000 : speechInterval * 1000;

            // Get random speach
            if (roomBot != null && roomBot.AutomaticChat && roomBot.RandomSpeech != null && roomBot.RandomSpeech.Any()) _chatTimer = new Timer(ChatTimerTick, null, _speechInterval, _speechInterval);
            _actionCount = Random.Next(10, 30 + virtualId);

            if (roomBot != null)
                _AttackTimer = new Timer(TryAttack, null, 2500, 2500);
        }

        /// <summary>
        /// Modifieds this instance.
        /// </summary>
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

        /// <summary>
        /// Called when [timer tick].
        /// </summary>
        internal override void OnTimerTick()

        {


            if (GetBotData() == null) return;
            if (_Victim == null)
            {
                LookForVictim();
                System.Drawing.Point Pos = GetRoom().GetGameMap().GetRandomValidWalkableSquare();
                GetRoomUser().MoveTo(Pos);
                return;
            }




            if (_Victim.Attacker == GetRoomUser())
                GetRoomUser().MoveTo(_Victim.SquareBehind);
            else
            {
                System.Drawing.Point Pos = GetRoom().GetGameMap().GetRandomValidWalkableSquare();
                GetRoomUser().MoveTo(Pos);
            }
            //if (_Victim == null)
            //{
            //    OnSelfLeaveRoom(true);
            //}
        }

        /// <summary>
        /// Called when [self enter room].
        /// </summary>
        internal override void OnSelfEnterRoom()
        {
            LookForVictim();
        }

        /// <summary>
        /// Called when [self leave room].
        /// </summary>
        /// <param name="kicked">if set to <c>true</c> [kicked].</param>
        internal override void OnSelfLeaveRoom(bool kicked)
        {

        }

        /// <summary>
        /// Called when [User enter room].
        /// </summary>
        /// <param name="User">The User.</param>
        internal override void OnUserEnterRoom(RoomUser User)
        {
            if (_Victim == null)
            {
                GameClient Session = User.GetClient();
                if (User == null
                    || Session == null || Session.GetRoleplay() == null)
                    return;

                if (Session.GetRoleplay().Wanted > 0 && User.Attacker == null)
                {
                    _Victim = User;
                    _Victim.Attacker = GetRoomUser();
                }
            }
        }

        /// <summary>
        /// Called when [User leave room].
        /// </summary>
        /// <param name="client">The client.</param>
        internal override void OnUserLeaveRoom(GameClient client)
        {
            if (_Victim == null)
            {
                LookForVictim();
                return;
            }

            _Victim.Attacker = null;
            _Victim = null;
            LookForVictim();
        }

        /// <summary>
        /// Called when [User say].
        /// </summary>
        /// <param name="User">The User.</param>
        /// <param name="message">The message.</param>
        internal override void OnUserSay(RoomUser User, string message)
        {

        }

        /// <summary>
        /// Called when [User shout].
        /// </summary>
        /// <param name="User">The User.</param>
        /// <param name="message">The message.</param>
        internal override void OnUserShout(RoomUser User, string message)
        {
            if (_isBartender)
            {
                GetRoomUser()
                    .Chat(null, "I do not rely you screaming. If you want me to serve something, say it well.", false, 0, 0);
            }
        }

        /// <summary>
        /// Attacks the victim
        /// </summary>
        internal void TryAttack(object StateInfo)
        {
            try
            {
                if (GetRoomUser() == null || GetRoom() == null
                                || _AttackTimer == null || _Victim == null
                                || _Victim.RoomId != GetRoom().RoomId)
                { _Victim = null; return; }

                if (GetBotData().CoolDown != null)
                    return;

                GameClient Session = _Victim.GetClient();
                if (Session == null || Session.GetHabbo() == null || Session.GetRoleplay() == null || Session.GetHabbo().CurrentRoom == null)
                { _AttackTimer.Change(2000, 2000); _Victim = null; return; }

                Vector2D VictimPOS = new Vector2D(_Victim.X, _Victim.Y);
                Vector2D MyPOS = new Vector2D(GetRoomUser().X, GetRoomUser().Y);

                if (RoleplayManager.Distance(MyPOS, VictimPOS) <= 2)
                {
                    int Time = Session.GetRoleplay().Wanted;

                    // Kill this punk

                    if (Session.GetRoleplay().Wanted > 0)
                    {
                      
                        GetRoomUser().Chat(null, "*Takes out their cuffs from their belt and attaches them to " + _Victim.GetUserName() + "'s wrist*", true, 1);
                        
                        RoomUser User = Session.GetHabbo().GetRoomUser();
                        User.ApplyEffect(0);
                        Session.GetRoleplay().StopWork();

                        if (Plus.GetGame().JailBreak != null)
                        {
                            Plus.GetGame().JailBreak.StopJailbreak();
                        }
                        if (Session.GetHabbo().CurrentRoom.RoomData.Jail)
                        {
                            User.SetPos(1, 14, 0);
                            User.ClearMovement();
                            User.Frozen = false;
                            User.UpdateNeeded = true;
                        }
                        if (Session.GetRoleplay().usingCar == true || Session.GetRoleplay().usingPlane == true)
                        {
                            Session.GetRoleplay().usingCar = false;
                            Session.GetRoleplay().usingPlane = false;
                            Session.GetHabbo().GetRoomUser().FastWalking = false;
                        }
                       
                        //Session.GetRoleplay().Masked = false;
                        Session.GetRoleplay().Wanted = 0;
                        Session.GetRoleplay().SaveQuickStat("wanted", "0");
                        Session.GetRoleplay().SaveQuickStat("weed", "0");
                        Session.GetRoleplay().Weed = 0;
                        string wantedJunk = Session.GetHabbo().UserName.ToLower();
                        //RoleplayManager.WantedListData.TryRemove(Session.GetHabbo().UserName.ToLower(), out wantedJunk);
                        //Session.GetRoleplay().UnEquip(false);
                        User.CanWalk = true;
                        User.Frozen = false;
                        Session.GetRoleplay().Cuffed = false;
                        //Session.GetRoleplay().UnEquip(false);
                        GetRoomUser().Chat(null, "*Arrests " + Session.GetHabbo().UserName + " for " + Time + " minute(s)*", true, 1);
                        Session.GetRoleplay().Cuffed = false;
                        Session.SendNotif("You have been arrested by BOT-" + GetBotData().Name + " for " + Time + " minute(s)");
                        Session.GetRoleplay().JailFigSet = false;
                        Session.GetRoleplay().JailedSeconds = 60;
                        Session.GetRoleplay().JailTimer = Time;
                        Session.GetRoleplay().Jailed = true;
                        //Session.GetRoleplay().SaveQuickStat("wanted_for", " ");
                        /*if (RoleplayManager.PoliceBotCalled && GetBotData().Name != "Dude")
                        {
                            RoleplayManager.KickBotFromRoom(base.GetRoomUser().BotData.Name, base.GetBotData().RoomId);
                            RoleplayManager.PoliceBotCalled = false;
                        }*/
                        //RoleplayManager.KickBotFromRoom(GetBotData().Name, GetBotData().RoomId);



                        //Session.GetRoleplay().Wanted = 0;


                        //Fines
                        int intFines = Time / 5;
                        Session.GetHabbo().Credits = Session.GetHabbo().Credits - intFines;
                        Session.GetHabbo().UpdateCreditsBalance();
                        
                        if (Session.GetRoleplay().Working)
                        {
                            Session.GetRoleplay().StopWork();
                        }
                        Session.GetRoleplay().SaveStatusComponents("jailed");
                        RoomUser Pet1 = Session.GetHabbo().GetRoomUser().MyPet;
                        if (Session.GetHabbo().GetRoomUser().MyPet != null)
                        {
                            Pet1.BotAI.OnSelfLeaveRoom(true);
                        }
                    }

                    LookForVictim();
                    GetBotData().ApplyCooldown();
                    _AttackTimer.Change(1000, 1000);
                    return;
                }

                if (RoleplayManager.Distance(MyPOS, VictimPOS) >= 1 && RoleplayManager.Distance(MyPOS, VictimPOS) <= 5)
                {
                    // Miss
                    
                    GetRoomUser().Chat(null, "*Fires there stun-gun at " + _Victim.GetUserName() + "*", true, 1);
                    Session.GetRoleplay().StunnedSeconds = 10;
                    	Session.GetRoleplay().EffectSeconds = 10;
                    Session.GetRoleplay().EffectSeconds = 10;
                    Session.GetRoleplay().StunnedSeconds = 10;
                    //Session.GetRoleplay().CuffedSeconds = 10;
                    Session.GetHabbo().GetRoomUser().ApplyEffect(53);
                    Session.GetHabbo().GetRoomUser().Frozen = false;
                    Session.GetHabbo().GetRoomUser().Frozen = true;
                    Session.GetHabbo().GetRoomUser().ClearMovement();

                    GetBotData().ApplyCooldown(2000);
                    _AttackTimer.Change(1000, 1000);
                    return;
                }

                if (RoleplayManager.Distance(MyPOS, VictimPOS) >= 5)
                {
                    _AttackTimer.Change(2000, 2000);
                    GetRoomUser().PathStep++;
                    return; // Don't even bother
                }
            }
            catch { }
        }

        private void LookForVictim()
        {
            try
            {
                Room MyRoom = GetRoom();
                if (MyRoom == null)
                    return;

                lock (MyRoom.GetRoomUserManager().UserList.Values)
                {
                    foreach (RoomUser RUser in MyRoom.GetRoomUserManager().UserList.Values.ToList())
                    {
                        if (RUser == null || RUser.GetClient() == null || RUser.GetClient().GetRoleplay() == null)
                            continue;
                        if (RUser.GetClient().GetRoleplay().Wanted > 99999 && RUser.Attacker == null)
                        {
                            _Victim = RUser;
                            _Victim.Attacker = GetRoomUser();
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Disposes of all timers
        /// </summary>
        private void EndTimers()
        {
            if (_AttackTimer == null)
                return;

            if (GetRoomUser() == null || GetBotData() == null)
            {
                _AttackTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _AttackTimer.Dispose();
                _AttackTimer = null;
            }
        }

        /// <summary>
        /// Stops the timer tick.
        /// </summary>
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
                            var User = GetRoomUser();
                            if (User.RotBody % 2 != 0) User.RotBody--;

                            User.Z = GetRoom().GetGameMap().SqAbsoluteHeight(User.X, User.Y);
                            if (!User.Statusses.ContainsKey("sit"))
                            {
                                User.UpdateNeeded = true;
                                User.Statusses.Add("sit", "0.55");
                            }
                            User.IsSitting = true;
                            return;
                        }
                    case ":stand":
                        {
                            var User = GetRoomUser();
                            if (User.IsSitting)
                            {
                                User.Statusses.Remove("sit");
                                User.IsSitting = false;
                                User.UpdateNeeded = true;
                            }
                            else if (User.IsLyingDown)
                            {
                                User.Statusses.Remove("lay");
                                User.IsLyingDown = false;
                                User.UpdateNeeded = true;
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