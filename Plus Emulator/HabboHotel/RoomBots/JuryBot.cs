using System;
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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Windows.Forms.VisualStyles;
using Plus.Connection.ServerManager;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Misc;
using Plus.HabboHotel.Roleplay.Jobs.Space;
using Plus.HabboHotel.Roleplay.Jobs.Cutting;
using Plus.HabboHotel.Roleplay.Apartments;
using Plus.HabboHotel.Roleplay.Jobs.Farming;
using Plus.HabboHotel.Roleplay.Casino.Slots;
using Plus.HabboHotel.Roleplay.Minigames.Purge;
using Plus.HabboHotel.Rooms.Wired.Handlers.Effects;

namespace Plus.HabboHotel.RoomBots
{
    /// <summary>
    /// Class GenericBot.
    /// </summary>
    internal class JuryBot : BotAI
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
        internal JuryBot(RoomBot roomBot, int virtualId, int botId, AIType type, bool isBartender, int speechInterval)
        {
            _id = botId;
            _virtualId = virtualId;
            _isBartender = isBartender;
            _speechInterval = speechInterval < 2 ? 2000 : speechInterval * 1000;

            // Get random speach
            if (roomBot != null && roomBot.AutomaticChat && roomBot.RandomSpeech != null && roomBot.RandomSpeech.Any())
                _chatTimer = new Timer(ChatTimerTick, null, _speechInterval, _speechInterval);
            _actionCount = Random.Next(10, 30 + virtualId);

            if (roomBot != null)
            {
                
                        _AttackTimer = new Timer(TryAttack, null, 1000, 1000);
                        return;
                    
                    
            }
            _AttackTimer.Change(1000, 1000);
            
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
            //base.GetRoomUser().Chat(null, "Hello", true, 1, 1);
           // RoleplayManager.CourtJuryTime++;

        }
        


        /// <summary>
        /// Called when [self enter room].
        /// </summary>
        internal override void OnSelfEnterRoom()
        {
            
                //_AttackTimer = new Timer(TryAttack, null, 2500, 2500);
            //LookForVictim();
            /*  GameClient Client = RoleplayManager.Defendant;
              uint CourtRoomId = 6;
              Room Room = RoleplayManager.GenerateRoom(CourtRoomId);
              if (RoleplayManager.Defendant == null)
              {
                  RoleplayManager.CourtVoteEnabled = false;
                  RoleplayManager.InnocentVotes = 0;
                  RoleplayManager.GuiltyVotes = 0;
                  DateTime? MyNullableDate;
                 // RoleplayManager.CourtJuryTime = MyNullableDate;
                  RoleplayManager.CourtTrialIsStarting = false;
                  RoleplayManager.CourtTrialStarted = false;
                  RoleplayManager.Defendant = null;
                  RoleplayManager.InvitedUsersToJuryDuty.Clear();

                  base.GetRoomUser().Chat(null, "Hey there as the defendant left the room, the trial is over.", true, 1);

                  RoleplayManager.KickBotFromRoom("Jury", CourtRoomId);
              }

             // RoleplayManager.CourtJuryTime = DateTime.Now;
              //int timetoend = Plus.GetUnixTimeStamp() + 200;
              //TimeSpan court_time = DateTime.Now - RoleplayManager.CourtJuryTime;

              base.GetRoomUser().SetPos(9,9,9);
              Client.GetHabbo().GetRoomUser().SetPos(5,5,5);
              if (RoleplayManager.CourtJuryTime < 200)
              {
                  if (RoleplayManager.CourtJuryTime == 5)
                  {
                      Client.GetMessageHandler().PrepareRoomForUser(CourtRoomId, "", false);
                      return;
                  }
                  else if (RoleplayManager.CourtJuryTime == 10)
                  {
                    base.GetRoomUser().Chat(null, "Hello, calling the case of Raid Police vs " + Client.GetHabbo().UserName + "", true ,1);
                    return;
                  }
                  else if (RoleplayManager.CourtJuryTime == 15)
                  {
                      base.GetRoomUser().Chat(null, "The defendant has been is accused of the following crimes" + Client.GetRoleplay().wanted_for.TrimEnd(',', ' ') + "", true, 1);
                      return;
                  }

              }*/
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
            
        }

        /// <summary>
        /// Called when [User leave room].
        /// </summary>
        /// <param name="client">The client.</param>
        internal override void OnUserLeaveRoom(GameClient client)
        {
           
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
            
        }

        /// <summary>
        /// Attacks the victim
        /// </summary>
        internal void TryAttack(object StateInfo)
        {
            //base.GetRoomUser().Chat(null, "Hello", true, 1, 1);
            //  return;
            //uint CourtRoomId = Convert.ToUInt32(RPDataHandler.GetData("courtid", "id"));
            bool trial_done = false;
            try
            {
                uint ct_id = Convert.ToUInt32(RPDataHandler.GetData("court_info", "id"));
                GameClient Client = RoleplayManager.Defendant;
                if (RoleplayManager.CourtTrialIsStarting)
                {
                    if (Client == null || Client.LoggingOut || Client.GetHabbo() == null ||
                        Client.GetRoleplay() == null)
                    {
                        Client.GetMessageHandler().PrepareRoomForUser(9, "");
                        RoleplayManager.CourtVoteEnabled = false;
                        RoleplayManager.InnocentVotes = 0;
                        RoleplayManager.GuiltyVotes = 0;
                        // DateTime? MyNullableDate;
                        // RoleplayManager.CourtJuryTime = MyNullableDate;
                        RoleplayManager.CourtTrialIsStarting = false;
                        RoleplayManager.CourtTrialStarted = false;
                        RoleplayManager.Defendant = null;
                        RoleplayManager.InvitedUsersToJuryDuty.Clear();
                        RoleplayManager.OGInvitedUsers.Clear();
                        RoleplayManager.CourtJuryTime = 0;
                        CourtManager.Running = false;
                        RoleplayManager.Defendant = null;
                        if (!base.GetBotData().WasPicked)
                        {
                            base.GetRoomUser().Chat(null,
                                "Hey there as the defendant left the case, the trial is over.",
                                true,
                                0);
                            //RoleplayManager.KickBotFromRoom(base.GetRoomUser().BotData.Name, base.GetBotData().RoomId);
                            //base.GetBotData().WasPicked = true;

                        }
                    }
                }

                if (RoleplayManager.InvitedUsersToJuryDuty.Capacity == 0 && RoleplayManager.CourtTrialBegining)
                {
                    base.GetRoomUser().Chat(null,
                        "Sorry, but due to the lack of users attending this trial, the trial has been cancelled.", true,
                        1, 0);
                    Client.SendNotif(
                        "<b>Trial Cancelled</b>\n\n No one showed up, therefore your trial was cancelled.");
                    RoleplayManager.CourtVoteEnabled = false;
                    RoleplayManager.InnocentVotes = 0;
                    RoleplayManager.GuiltyVotes = 0;
                    RoleplayManager.CourtTrialIsStarting = false;
                    RoleplayManager.CourtTrialStarted = false;
                    RoleplayManager.Defendant = null;
                    RoleplayManager.InvitedUsersToJuryDuty.Clear();
                    RoleplayManager.OGInvitedUsers.Clear();
                    RoleplayManager.JuryArrived = false;
                    RoleplayManager.CourtTrialBegining = false;
                    CourtManager.Running = false;
                }

                //RoleplayManager.CourtTrialBegining = true;
                if (RoleplayManager.CourtTrialBegining)
                {
                    lock (Plus.GetGame().GetClientManager().Clients.Values)
                    {
                        foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                        {
                            if (!RoleplayManager.InvitedUsersToJuryDuty.Contains(mClient))
                                continue;
                            RoleplayManager.CourtTrialStarted = true;
                            //return new TeleportToFurni(new RoomItem(2639,);
                            mClient.SendWhisperBubble("The trial has started, take a seat and listen!", 5);
                            mClient.GetHabbo().GetRoomUser().SetPos(16, 7, 0);
                            mClient.GetHabbo().GetRoomUser().SetRot(4);
                            mClient.GetHabbo().GetRoomUser().ClearMovement();
                            RoleplayManager.CourtTrialBegining = false;
                        }
                    }

                    RoleplayManager.CourtTrialBegining = false;
                }

                if (RoleplayManager.JuryArrived)
                {
                    if (RoleplayManager.CourtTrialStarted)
                    {
                       // _AttackTimer.Change(1000, 1000);
                        //RoleplayManager.CourtJuryTime++;

                        //_AttackTimer.Change(1000, 1000);

                        if (RoleplayManager.CourtJuryTime < 200)
                        {
                            /* if (RoleplayManager.CourtJuryTime == 5)
                          {
                              Client.GetMessageHandler().PrepareRoomForUser(CourtRoomId, "");
                              return;*/
                        }

                        if (RoleplayManager.CourtJuryTime == 5)
                        {
                            Client.GetMessageHandler().PrepareRoomForUser(490, null);
                        }
                        else if (RoleplayManager.CourtJuryTime == 8)
                        {
                            Client.GetHabbo().GetRoomUser().SetPos(2, 10, 0);
                            Client.GetHabbo().GetRoomUser().SetRot(0);
                            Client.GetHabbo().GetRoomUser().UpdateNeeded = true;
                            Client.GetHabbo().GetRoomUser().ClearMovement();
                            Client.GetRoleplay().StunnedSeconds = 1000000;
                            Client.GetHabbo().GetRoomUser().CanWalk = false;
                            Client.GetHabbo().GetRoomUser().Frozen = true;
                            Client.GetHabbo().GetRoomUser().ClearMovement();
                            //Client.GetHabbo().GetRoomUser().Frozen = true;
                        }

                        string morning = "Good Morning";
                        string afternoon = "Good Afternoon";
                        string evening = "Good Evening";
                        string rpname = Convert.ToString(RPDataHandler.GetData("rpinfo", "name"));
                        if (RoleplayManager.CourtJuryTime == 10)
                        {
                            DateTime TimeNow = DateTime.Now;
                            TimeSpan TimeOfDay = TimeNow.TimeOfDay;
                            if (TimeOfDay.Hours >= 05 && TimeOfDay.Hours <= 11)
                            {
                                base.GetRoomUser().Chat(null,
                                    "Good Morning ladies and gentlemen, calling the case of " + rpname +
                                    " justice vs " +
                                    Client.GetHabbo().UserName + ".", true, 1);
                            }

                            else if (TimeOfDay.Hours >= 12 && TimeOfDay.Hours <= 16)
                            {
                                base.GetRoomUser().Chat(null,
                                    "Good Afternoon ladies and gentlemen, calling the case of " + rpname +
                                    " justice vs " +
                                    Client.GetHabbo().UserName + ".", true, 1, 0);
                            }

                            else
                            {
                                base.GetRoomUser().Chat(null,
                                    "Good Evening ladies and gentlemen, calling the case of " + rpname +
                                    " justice vs " +
                                    Client.GetHabbo().UserName + ".", true, 1, 0);
                            }
                        }
                        else if (RoleplayManager.CourtJuryTime == 15)
                        {
                            base.GetRoomUser().Chat(null,
                                "The defendant has been accused of the following crimes: " +
                                Client.GetRoleplay().wanted_for.TrimEnd(',', ' ') + ".",
                                true, 1, 0);
                        }
                        else if (RoleplayManager.CourtJuryTime == 20)
                        {
                            base.GetRoomUser().Chat(null,
                                "The defendant is appealing the decision of the " + rpname +
                                " Justice System that they are guilty.",
                                true, 1, 0);
                        }
                        else if (RoleplayManager.CourtJuryTime == 25)
                        {
                            base.GetRoomUser().Chat(null,
                                Client.GetHabbo().UserName +
                                ", can you please explain to me, and the rest of the jury, exactly what happened?",
                                true, 1,
                                0);
                            Client.SendWhisper("You have 80 seconds to plead your innocence.");
                        }
                        else if (RoleplayManager.CourtJuryTime == 105)
                        {
                            base.GetRoomUser().Chat(null, "Thank you for your explanation.", true, 1, 0);
                        }
                        else if (RoleplayManager.CourtJuryTime == 110)
                        {
                            base.GetRoomUser().Chat(null,
                                "If the jury finds that you are innocent, you will be released from jail.", true, 1, 0);
                        }
                        else if (RoleplayManager.CourtJuryTime == 113)
                        {
                            base.GetRoomUser().Chat(null,
                                "However, if they find you guilty, you will spend the remaining of your sentence in jail.",
                                true, 1,
                                0);
                        }
                        else if (RoleplayManager.CourtJuryTime == 118)
                        {
                            base.GetRoomUser().Chat(null,
                                "Imaginary dude, can you please remove " + Client.GetHabbo().UserName +
                                " from the room?",
                                true, 1, 0);
                        }
                        else if (RoleplayManager.CourtJuryTime == 120)
                        {
                            Client.GetMessageHandler().PrepareRoomForUser(9, "");
                            //Client.SendNotif("You are being sent back to prison to wait for your final vote.");
                        }
                        //Prevents errors and prevents spamming when sent back to jail
                        else if (RoleplayManager.CourtJuryTime == 123)
                        {
                            Client.SendNotif("You are being sent back to prison to wait for your final vote.");


                        }
                        else if (RoleplayManager.CourtJuryTime == 125)
                        {
                            base.GetRoomUser().Chat(null,
                                "It's now up to you, the jury to determine the defendants fate.",
                                true, 1, 0);
                        }
                        else if (RoleplayManager.CourtJuryTime == 130)
                        {
                            base.GetRoomUser().Chat(null,
                                "Please vote by saying either :vote guilty or :vote innocent.",
                                true, 1, 0);
                            RoleplayManager.RequestingVotes = true;
                        }
                        else if (RoleplayManager.CourtJuryTime == 150)
                        {
                            int CourtResult = Math.Max(RoleplayManager.InnocentVotes, RoleplayManager.GuiltyVotes);
                            if (CourtResult == 0)
                            {
                                lock (Plus.GetGame().GetClientManager().Clients.Values)
                                {
                                    foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                                    {
                                        if (mClient == null)
                                            continue;
                                        if (mClient.GetHabbo() == null)
                                            continue;
                                        if (mClient.GetHabbo().CurrentRoom == null)
                                            continue;
                                        if (mClient.GetConnection() == null)
                                            continue;
                                        if (mClient.GetHabbo().GetRoomUser().IsAsleep)
                                            continue;
                                        if (mClient.GetHabbo().CurrentRoomId == 490)
                                            continue;
                                        if (RoleplayManager.Defendant == mClient)
                                            continue;

                                        mClient.SendWhisperBubble("[LiveFeed] " + Client.GetHabbo().UserName +
                                                                  " has been voted Guilty in court!");
                                        //TargetSession.GetRoleplay().Wanted = 0;
                                    }
                                }

                                base.GetRoomUser().Chat(null,
                                    "Thanks for you votes. The jury has found " + Client.GetHabbo().UserName +
                                    ", the defendant guilty of all crimes.", true, 1, 0);
                                Thread.Sleep(4000);
                                base.GetRoomUser().Chat(null,
                                    "The defendant will spend the rest of their sentence in prison", true, 1, 0);
                                Client.SendNotif(
                                    "You have been voted GUILTY in court. You will spend the rest of your sentence in jail.");
                                Client.GetRoleplay().Was_Trialed = true;
                                Client.GetRoleplay().SaveQuickStat("trialed", "1");
                            }


                            else if (CourtResult == RoleplayManager.GuiltyVotes)
                            {
                                lock (Plus.GetGame().GetClientManager().Clients.Values)
                                {
                                    foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                                    {
                                        if (mClient == null)
                                            continue;
                                        if (mClient.GetHabbo() == null)
                                            continue;
                                        if (mClient.GetHabbo().CurrentRoom == null)
                                            continue;
                                        if (mClient.GetConnection() == null)
                                            continue;
                                        if (mClient.GetHabbo().GetRoomUser().IsAsleep)
                                            continue;
                                        if (mClient.GetHabbo().CurrentRoomId == 490)
                                            continue;
                                        if (RoleplayManager.Defendant == mClient)
                                            continue;

                                        mClient.SendWhisperBubble("[LiveFeed] " + Client.GetHabbo().UserName +
                                                                  " has been voted Guilty in court!");
                                        //TargetSession.GetRoleplay().Wanted = 0;
                                    }
                                }

                                base.GetRoomUser().Chat(null,
                                    "Thanks for you votes. The jury has found " + Client.GetHabbo().UserName +
                                    ", the defendant guilty of all crimes.", true, 1, 0);
                                Thread.Sleep(4000);
                                base.GetRoomUser().Chat(null,
                                    "The defendant will spend the rest of their sentence in prison.", true, 1, 0);
                                Client.SendNotif(
                                    "<b>You have been voted GUILTY in court.</b>\n\n You will spend the rest of your sentence in jail.");
                                Client.GetRoleplay().Was_Trialed = true;
                                Client.GetRoleplay().SaveQuickStat("trialed", "1");
                                /*var request = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/webhooks/691408452823547925/z52Z-intEYuw69jrYyAB_snXyK_yLPsimFrd2Bt4mYIDFvhxo8MxsGaNIS8p0zMYhqy0");
    
                                var postData = "username=" + Uri.EscapeDataString("");
                                postData += "&content=" + Uri.EscapeDataString(@"```bash
                                                                               " + Client.GetHabbo().UserName + " has been voted guilty in court" +
                                                                               "```");
                                var data = Encoding.ASCII.GetBytes(postData);
    
                                request.Method = "POST";
                                request.ContentType = "application/x-www-form-urlencoded";
                                request.ContentLength = data.Length;
    
                                using (var stream = request.GetRequestStream())
                                {
                                    stream.Write(data, 0, data.Length);
                                }
    
                                var response = (HttpWebResponse)request.GetResponse();
    
                                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();*/
                            }
                            else
                            {
                                lock (Plus.GetGame().GetClientManager().Clients.Values)
                                {
                                    foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                                    {
                                        if (mClient == null)
                                            continue;
                                        if (mClient.GetHabbo() == null)
                                            continue;
                                        if (mClient.GetHabbo().CurrentRoom == null)
                                            continue;
                                        if (mClient.GetConnection() == null)
                                            continue;
                                        if (mClient.GetHabbo().GetRoomUser().IsAsleep)
                                            continue;
                                        if (mClient.GetHabbo().CurrentRoomId == 490)
                                            continue;
                                        if (RoleplayManager.Defendant == mClient)
                                            continue;

                                        mClient.SendWhisperBubble("[LiveFeed] " + Client.GetHabbo().UserName +
                                                                  " has been voted Innocent in court!");
                                        //TargetSession.GetRoleplay().Wanted = 0;
                                    }
                                }

                                base.GetRoomUser().Chat(null,
                                    "Thanks for you votes. The jury has found " + Client.GetHabbo().UserName +
                                    ", the defendant innocent of all crimes.", true, 1, 0);
                                Thread.Sleep(4000);
                                base.GetRoomUser().Chat(null,
                                    "Using my power as an ai, I hereby release " + Client.GetHabbo().UserName +
                                    " from prison and pardon them from their crimes.", true, 1, 0);
                                Client.SendNotif(
                                    "<b>Voted Innocent!</b>\n\nThe jury has found you innocent of all crimes\n\nWhat a lucky escape!");
                                // Client.GetRoleplay().JailTimer == 0;
                                Client.GetRoleplay().JailTimer = 0;
                                Client.GetRoleplay().JailedSeconds = 0;
                                Client.GetRoleplay().sjailed = false;
                                Client.GetRoleplay().SaveStatusComponents("jailed");
                                Client.GetRoleplay().wanted_for = "";
                                Client.GetRoleplay().Wanted = 0;
                                Client.GetRoleplay().Was_Trialed = false;
                                Client.GetRoleplay().SaveQuickStat("trialed", 0);
                            }
                        }
                        else if (RoleplayManager.CourtJuryTime == 160)
                        {
                            base.GetRoomUser().Chat(null, "Thanks, the jury is excused. Court is adjourned", true, 1,
                                0);
                        }
                        else if (RoleplayManager.CourtJuryTime == 162)
                        {
                            lock (Plus.GetGame().GetClientManager().Clients.Values)
                            {
                                foreach (RoomUser User in GetRoom().GetRoomUserManager().UserList.Values)
                                {
                                    Random Rand = new Random();
                                    if (!RoleplayManager.InvitedUsersToJuryDuty.Contains(User.GetClient()))
                                        continue;
                                    int courtcmin = Convert.ToInt32(RPDataHandler.GetData("courtrewards", "mincoins"));
                                    int courtcmax = Convert.ToInt32(RPDataHandler.GetData("courtrewards", "maxcoins"));
                                    int courtxpmin = Convert.ToInt32(RPDataHandler.GetData("courtrewards", "minxp"));
                                    int courtxpmax = Convert.ToInt32(RPDataHandler.GetData("courtrewards", "maxxp"));
                                    int credits_xp = Rand.Next(courtcmin, courtcmax);
                                    int xp = Rand.Next(courtxpmin, courtxpmax);
                                    User.GetClient().SendNotif(
                                        "<b>Thanks!</b>\n\nThank you for participating in the trial, to thank you we are awarding you the following:\n\n> " +
                                        xp + " XP\n> $" + credits_xp + "\n\nEnjoy!");
                                    User.GetClient().GetHabbo().GetRoomUser().SetPos(7,23,0);
                                    User.GetClient().GetHabbo().GetRoomUser().ClearMovement();


                                    RoleplayManager.GiveMoney(User.GetClient(), +credits_xp);
                                    LevelHandler.AddEXP(User.GetClient(), xp);
                                    User.GetClient().GetRoleplay().SaveQuickStat("currentxp", +xp);
                                    
                                    //mClient.GetMessageHandler().PrepareRoomForUser(1, null);
                                }

                                RoleplayManager.CourtJuryTime = 0;
                                RoleplayManager.CourtTrialBegining = false;
                                RoleplayManager.CourtTrialStarted = false;
                                RoleplayManager.CourtTrialIsStarting = false;
                                RoleplayManager.Defendant = null;
                                RoleplayManager.CaseRequest = false;
                                RoleplayManager.JuryArrived = false;
                                RoleplayManager.GuiltyVotes = 0;
                                RoleplayManager.InnocentVotes = 0;
                                RoleplayManager.InvitedUsersToJuryDuty.Clear();
                                RoleplayManager.RequestingVotes = false;
                                RoleplayManager.BeingViewed = false;
                                RoleplayManager.ReadyToView = false;
                                CourtManager.Running = false;
                                CourtManager.MainTimer = null;
                            }

                        }
                    }

                }
            }
            catch
            {

            }






            //RoleplayManager.CourtJuryTime++;
            //trial_done = true;

            // RoleplayManager.KickBotFromRoom("Jury", CourtRoomId);




        }
        


        private void LookForVictim()
        {
            
        }

        /// <summary>
        /// Disposes of all timers
        /// </summary>
        private void EndTimers()
        {
            
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