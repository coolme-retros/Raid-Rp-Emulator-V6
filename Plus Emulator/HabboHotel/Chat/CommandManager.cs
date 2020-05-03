/*using System.Linq;
using System.Text;
using System.Collections.Generic;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Roleplay.Jobs;
using Plus.Database.Manager.Database.Session_Details.Interfaces;

namespace Plus.HabboHotel.Chat
{
    public class CommandManager
    {
        /// <summary>
        /// Command Prefix only applies to custom commands.
        /// </summary>
        private string _prefix = ":";

        /// <summary>
        /// Commands registered for use.
        /// </summary>
        private readonly Dictionary<string, IChatCommand> _commands;
        private readonly Dictionary<string, IChatCommand> _jobcommands;
        private readonly Dictionary<string, IChatCommand> _gangcommands;
        private readonly Dictionary<string, IChatCommand> _staffcommands;
        private readonly Dictionary<string, IChatCommand> _ambassadorcommands;
        private readonly Dictionary<string, IChatCommand> _loggedcommands;
        private readonly Dictionary<string, IChatCommand> _vipcommands;
        private readonly Dictionary<string, IChatCommand> _eventcommands;
        private List<string> _aliases;

        /// <summary>
        /// The default initializer for the CommandManager
        /// </summary>
        public CommandManager(string Prefix)
        {
            this._prefix = Prefix;
            this._commands = new Dictionary<string, IChatCommand>();
            this._jobcommands = new Dictionary<string, IChatCommand>();
            this._gangcommands = new Dictionary<string, IChatCommand>();
            this._ambassadorcommands = new Dictionary<string, IChatCommand>();
            this._staffcommands = new Dictionary<string, IChatCommand>();
            this._loggedcommands = new Dictionary<string, IChatCommand>();
            this._vipcommands = new Dictionary<string, IChatCommand>();
            this._eventcommands = new Dictionary<string, IChatCommand>();
            this._aliases = new List<string>();

            this.RegisterUsers();
            this.RegisterUsersGangs();
            this.RegisterUsersJobs();
            this.RegisterVIP();
            this.RegisterAmbassadors();
            this.RegisterTrialModerators();
            this.RegisterModerators();
            this.RegisterSeniorModerators();
            this.RegisterAdministrators();
            this.RegisterManagers();
            this.RegisterDevelopers();
            this.RegisterOwners();
            this.RegisterSpecialRights();
        }


        /// <summary>
        /// Request the text to parse and check for commands that need to be executed.
        /// </summary>
        /// <param name="Session">Session calling this method.</param>
        /// <param name="Message">The message to parse.</param>
        /// <returns>True if parsed or false if not.</returns>
        public bool Parse(GameClient Session, string Message)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null)
                return true;

            if (!Message.StartsWith(_prefix))
                return true;
        
            #region Commands List

            #region :commands
            if (Message.ToLower() == _prefix + "commands")
            {
                StringBuilder List = new StringBuilder();
                List.Append("This is the list of commands you have available:\n\n");
                List.Append(":jobcommands - Provides you a list of all available job commands.\n");
                List.Append(":gangcommands - Provides you a list of all available gang commands.\n");
                List.Append(":vipcommands - Provides you a list of all available VIP commands.\n");
                List.Append(":staffcommands - Provides you a list of all available staff commands.\n\n");

                foreach (var CmdList in _commands.ToList())
                {
                    if (_aliases.Contains(CmdList.Key.ToLower()))
                        continue;

                    if (!string.IsNullOrEmpty(CmdList.Value.PermissionRequired))
                    {
                        if (!Session.GetHabbo().GotCommand(CmdList.Value.PermissionRequired))
                            continue;
                    }

                    List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                }
                Session.SendNotifWithScroll(List.ToString());
                return true;
            }
            #endregion

            #region :jobcommands
            if (Message.ToLower() == _prefix + "jobcommands")
            {
                StringBuilder List = new StringBuilder();
                List.Append("This is the list of job commands you have available:\n\n");
                foreach (var CmdList in _jobcommands.ToList())
                {
                    if (_aliases.Contains(CmdList.Key.ToLower()))
                        continue;

                    if (CmdList.Key == "startwork" || CmdList.Key == "stopwork" || CmdList.Key == "corplist" || CmdList.Key == "corpinfo")
                        List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    else if (CmdList.Key == "promote" || CmdList.Key == "demote")
                    {
                        if (Session.GetHabbo().HasFuse("fuse_corp_manager"))
                            List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    }
                    else if (CmdList.Key == "ra" || CmdList.Key == "togglera")
                    {
                        if (Session.GetHabbo().HasFuse("fuse_corp_manager") || Session.GetRoleplay().JobHasRights(CmdList.Key.ToLower()))
                            List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    }
                    else
                    {
                        if (Session.GetRoleplay().JobHasRights(CmdList.Key.ToLower()))
                            List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    }
                }
                Session.SendNotifWithScroll(List.ToString());
                return true;
            }
            #endregion

            #region :gangcommands
            if (Message.ToLower() == _prefix + "gangcommands")
            {
                StringBuilder List = new StringBuilder();
                List.Append("This is the list of gang commands you have available:\n\n");
                foreach (var CmdList in _gangcommands.ToList())
                {
                    if (_aliases.Contains(CmdList.Key.ToLower()))
                        continue;

                    if (CmdList.Key == "ginfo" || CmdList.Key == "glist" || CmdList.Key == "turfs" || CmdList.Key == "gcreate" || CmdList.Key == "gleave")
                        List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    else if (CmdList.Key == "gcapture" && Session.GetRoleplay().GangId > 1000)
                        List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    else if (Session.GetRoleplay().GangId > 0)
                        List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                }
                Session.SendNotifWithScroll(List.ToString());
                return true;
            }
            #endregion

            #region :staffcommands
            if (Message.ToLower() == _prefix + "staffcommands")
            {
                if (!Session.GetHabbo().HasFuse("fuse_mod"))
                {
                    Session.SendWhisper("You are not a staff member!", false, 1);
                    return true;
                }
                else
                {
                    StringBuilder List = new StringBuilder();
                    List.Append("This is the list of staff commands you have available:\n\n");
                    foreach (var CmdList in _staffcommands.ToList())
                    {
                        if (_aliases.Contains(CmdList.Key.ToLower()))
                            continue;

                        if (!string.IsNullOrEmpty(CmdList.Value.PermissionRequired))
                        {
                            if (!Session.GetHabbo().GotCommand(CmdList.Value.PermissionRequired))
                                continue;
                        }

                        List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    }
                    Session.SendNotifWithScroll(List.ToString());
                    return true;
                }
            }
            #endregion

            #region :vipcommands
            if (Message.ToLower() == _prefix + "vipcommands")
            {
                if (Session.GetHabbo().VIP && !Session.GetHabbo().HasFuse("fuse_mod"))
                {
                    Session.SendWhisper("You do not have VIP!", false, 1);
                    return true;
                }
                else
                {
                    StringBuilder List = new StringBuilder();
                    List.Append("This is the list of vip commands you have available:\n\n");
                    foreach (var CmdList in _vipcommands.ToList())
                    {
                        if (_aliases.Contains(CmdList.Key.ToLower()))
                            continue;

                        List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    }
                    Session.SendNotifWithScroll(List.ToString());
                    return true;
                }
            }
        }
            #endregion

            #endregion

            Message = Message.Substring(1);
            string[] Split = Message.Split(' ');

            if (Split.Length == 0)
                return false;

            IChatCommand Cmd = null;
            IChatCommand LogCmd = null;
            if (_commands.TryGetValue(Split[0].ToLower(), out Cmd))
            {
                _loggedcommands.TryGetValue(Split[0].ToLower(), out LogCmd);

                if (Cmd == LogCmd)
                {
                    string UserName = Session.GetHabbo().UserName;
                    if (_staffcommands.ContainsKey(Split[0].ToLower()))
                        this.LogCommand(Session.GetHabbo().Id, Message, Plus.GetGame().GetClientManager().GetClientByUserName(Session.GetHabbo().UserName).MachineId, "staff");
                    else if (_ambassadorcommands.ContainsKey(Split[0].ToLower()))
                        this.LogCommand(Session.GetHabbo().Id, Message, (Plus.GetGame().GetClientManager().GetClientByUserName(Session.GetHabbo().UserName).MachineId), "ambassador");
                    else if (_jobcommands.ContainsKey(Split[0].ToLower()))
                        this.LogCommand(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId, "job");
                    else if (_vipcommands.ContainsKey(Split[0].ToLower()))
                        this.LogCommand(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId, "vip");
                    else if (_eventcommands.ContainsKey(Split[0].ToLower()))
                        this.LogCommand(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId, "event");
                    else
                        this.LogCommand(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId, "user");
                }
                if (!string.IsNullOrEmpty(Cmd.PermissionRequired))
                {
                    if (Split[0].ToLower() == "push")
                    {
                        if (Session.GetRoleplay().Game == null || Session.GetRoleplay().Team == null)
                        {
                            if (!Session.GetHabbo().GetPermissions().HasCommand(Cmd.PermissionRequired))
                                return false;
                        }
                    }
                    else
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand(Cmd.PermissionRequired))
                            return false;
                    }
                }

                Session.GetHabbo().IChatCommand = Cmd;
                Session.GetHabbo().CurrentRoom.GetWired().TriggerEvent(WiredBoxType.TriggerUserSaysCommand, Session.GetHabbo(), this);

                Cmd.Execute(Session, Session.GetHabbo().CurrentRoom, Split);
                return true;
            }
            return false;
        }

        #region Commands
        /// <summary>
        /// User set of commands
        /// </summary>
        private void RegisterUsers()
        {
            // General
            //this.Register("about", new AboutCommand());
            this.Register("changelog", new ChangeLogCommand());
            this.Register("gamemap", new GameMapCommand());
            this.Register("map", new GameMapCommand(), "", true);
            this.Register("stats", new StatsCommand());
            this.Register("farmingstats", new FarmingStatsCommand());
            this.Register("farming", new FarmingStatsCommand(), "", true);
            this.Register("weapons", new WeaponsCommand());
            this.Register("whosonline", new OnlineCommand());
            this.Register("online", new OnlineCommand(), "", true);
            this.Register("hotrooms", new HotRoomsCommand());
            this.Register("hr", new HotRoomsCommand(), "", true);
            this.Register("taxi", new TaxiCommand());
            this.Register("stoptaxi", new StopTaxiCommand());
            this.Register("roomid", new RoomIDCommand());
            this.Register("roominfo", new RoomInfoCommand());
            this.Register("regenmaps", new RegenMapsCommand());
            this.Register("emptyitems", new EmptyItemsCommand());
            this.Register("poof", new PoofCommand());
            this.Register("logout", new LogOutCommand());
            this.Register("changeclass", new ChangeClassCommand());
            this.Register("arrow", new ArrowCommand());
            this.Register("walk", new ArrowCommand());

            // Timers & Cooldowns
            this.Register("timeleft", new TimeLeftCommand());
            this.Register("cooldowns", new CooldownsCommand());
            this.Register("cds", new CooldownsCommand(), "", true);
            this.Register("cd", new CooldownsCommand(), "", true);

            // Banking
            this.Register("balance", new BalanceCommand());
            this.Register("deposit", new DepositCommand());
            this.Register("withdraw", new WithdrawCommand());

            // Criminal Activity
            this.Register("laws", new LawsCommand());
            this.Register("rob", new RobCommand());
            this.Register("snort", new SnortCommand());
            this.Register("dispose", new DisposeCommand());
            this.Register("smoke", new SmokeCommand());

            // Purchasing Goods
            this.Register("buybullets", new BuyBulletsCommand());
            this.Register("bullets", new BuyBulletsCommand(), "", true);
            this.Register("buycredit", new BuyCreditCommand());
            this.Register("credit", new BuyCreditCommand(), "", true);
            this.Register("buyfuel", new BuyFuelCommand());
            this.Register("fuel", new BuyFuelCommand(), "", true);
            this.Register("buyticket", new BuyTicketCommand(), "userlog");
            this.Register("ticket", new BuyTicketCommand(), "userlog", true);

            // Combat
            this.Register("combatmode", new CombatModeCommand());
            this.Register("cmode", new CombatModeCommand(), "", true);
            this.Register("hit", new HitCommand());
            this.Register("equip", new EquipCommand());
            this.Register("eq", new EquipCommand(), "", true);
            this.Register("unequip", new UnEquipCommand());
            this.Register("uneq", new UnEquipCommand(), "", true);
            this.Register("shoot", new ShootCommand());
            this.Register("reload", new ReloadGunCommand());

            // Offers
            this.Register("give", new GiveCommand(), "userlog");
            this.Register("offers", new OffersCommand());
            this.Register("offer", new OfferCommand());
            this.Register("accept", new AcceptCommand());
            this.Register("decline", new DeclineCommand());

            // Police Related
            this.Register("callpolice", new CallPoliceCommand());
            this.Register("911", new CallPoliceCommand(), "", true);
            this.Register("bail", new BailCommand());
            this.Register("surrender", new SurrenderCommand());
            this.Register("wantedlist", new WantedListCommand());
            this.Register("wl", new WantedListCommand(), "", true);
            this.Register("escort", new EscortCommand(), "");

            // Court
            this.Register("trial", new TrialCommand());
            this.Register("vote", new VoteCommand());

            // Toggles
            this.Register("toggletexts", new ToggleTextsCommand());
            this.Register("disablewhispers", new DisableWhispersCommand());
            this.Register("disablemimic", new DisableMimicCommand());

            // Self Interactions
            this.Register("drive", new DriveCommand());
            this.Register("sit", new SitCommand());
            this.Register("stand", new StandCommand());
            this.Register("lay", new LayCommand());
            this.Register("dance", new DanceCommand());
            this.Register("me", new MeCommand());

            // Item Interaction
            this.Register("calldelivery", new CallDeliveryCommand());
            this.Register("eat", new EatCommand());
            this.Register("drink", new DrinkCommand());
            this.Register("workout", new WorkoutCommand());
            this.Register("plant", new PlaceCommand());
            this.Register("place", new PlaceCommand());
            this.Register("jailbreak", new PlaceCommand());
            this.Register("repair", new PlaceCommand());

            // Marriage Interaction
            this.Register("marry", new MarryCommand());
            this.Register("propose", new MarryCommand(), "", true);
            this.Register("divorce", new DivorceCommand());
            this.Register("sex", new SexCommand());

            // User Interaction
           4 this.Register("slap", new SlapCommand());
            this.Register("kiss", new KissCommand());
            this.Register("hug", new HugCommand());
            this.Register("rape", new RapeCommand());

            // Apartment
            this.Register("kick", new KickCommand());
            this.Register("roomkick", new RoomKickCommand());
            this.Register("pickall", new PickAllCommand(), "userlog");
            this.Register("setprice", new SetPriceommand(), "userlog");

            // Events
            this.Register("eventstore", new PurchaseEventCommand(), "eventlog");
            this.Register("estore", new PurchaseEventCommand(), "eventlog", true);
            this.Register("purchase", new PurchaseEventCommand(), "eventlog", true);
            this.Register("leavegame", new LeaveGameCommand(), "eventlog");
            this.Register("quitgame", new LeaveGameCommand(), "eventlog", true);
            this.Register("teamalert", new TeamAlertCommand(), "eventlog");
            this.Register("ta", new TeamAlertCommand(), "eventlog", true);
            this.Register("t", new TeamAlertCommand(), "eventlog", true);
            this.Register("capture", new CaptureCommand(), "eventlog");

            // Gambling
            this.Register("bet", new GamblingCommand(), "eventlog");
            this.Register("pass", new GamblingCommand(), "eventlog", true);

            // Soloqueue event
            this.Register("soloqueue", new SoloQueueCommand(), "eventlog");
            this.Register("joinqueue", new SoloQueueCommand(), "eventlog", true);
            this.Register("solo", new SoloQueueCommand(), "eventlog", true);

            // Bounties
            this.Register("setbounty", new AddBountyCommand(), "userlog");
            this.Register("setb", new AddBountyCommand(), "userlog", true);
            this.Register("addbounty", new AddBountyCommand(), "userlog", true);
            this.Register("addb", new AddBountyCommand(), "userlog", true);
            this.Register("removebounty", new RemoveBountyCommand(), "userlog");
            this.Register("removeb", new RemoveBountyCommand(), "userlog", true);
            this.Register("bountylist", new BountyListCommand(), "");
            this.Register("bl", new BountyListCommand(), "", true);
            this.Register("blist", new BountyListCommand(), "", true);

            // Translation
            this.Register("translate", new TranslateCommand());
            this.Register("trans", new TranslateCommand(), "", true);
            this.Register("stoptranslate", new StopTranslateCommand());
            this.Register("stranslate", new StopTranslateCommand(), "", true);
            this.Register("strans", new StopTranslateCommand(), "", true);

            // Misc
            this.Register("makechat", new MakeChatCommand(), "userlog", true);
            this.Register("joinchat", new JoinChatCommand(), "userlog", true);
            this.Register("chats", new ChatsCommand(), "userlog", true);
            this.Register("downloadapp", new DownloadAppCommand(), "userlog", true);
            this.Register("help", new HelpCommand());
            this.Register("helpamb", new AmbassadorHelpCommand());
            this.Register("ride", new RideCommand());

        }

        /// <summary>
        /// User set of gang commands
        /// </summary>
        private void RegisterUsersGangs()
        {
            this.Register("ginfo", new GangInfoCommand(), "gang");
            this.Register("glist", new GangListCommand(), "gang");
            this.Register("turfs", new GangTurfsCommand(), "gang");
            this.Register("gcreate", new GangCreateCommand(), "gang");
            this.Register("ginvite", new GangInviteCommand(), "gang");
            this.Register("gleave", new GangLeaveCommand(), "gang");
            this.Register("gcapture", new GangCaptureCommand(), "gang");
            this.Register("gbackup", new GangBackupCommand(), "gang");
            this.Register("grank", new GangRankCommand(), "gang");
            this.Register("gtransfer", new GangTransferCommand(), "gang");
            this.Register("gheal", new GangHealCommand(), "gang");
        }

        /// <summary>
        /// User set of job commands
        /// </summary>
        private void RegisterUsersJobs()
        {
            // General
            this.Register("startwork", new StartWorkCommand(), "joblog");
            this.Register("stopwork", new StopWorkCommand(), "joblog");
            this.Register("corplist", new CorpListCommand(), "joblog");
            this.Register("clist", new CorpListCommand(), "joblog", true);
            this.Register("corpinfo", new CorpInfoCommand(), "joblog");
            this.Register("cinfo", new CorpInfoCommand(), "joblog", true);
            this.Register("promote", new PromoteCommand(), "joblog");
            this.Register("demote", new DemoteCommand(), "joblog");
            this.Register("sendhome", new SendhomeCommand(), "joblog");
            this.Register("hire", new HireCommand(), "joblog");
            this.Register("fire", new FireCommand(), "joblog");
            this.Register("checkminutes", new CheckMinutesCommand(), "joblog");
            this.Register("checkmins", new CheckMinutesCommand(), "joblog", true);

            // Hospital
            this.Register("discharge", new DischargeCommand(), "joblog");
            this.Register("heal", new HealCommand(), "joblog");

            // Police
            this.Register("radio", new RadioAlertCommand(), "joblog");
            this.Register("ra", new RadioAlertCommand(), "joblog", true);
            this.Register("r", new RadioAlertCommand(), "joblog", true);
            this.Register("toggleradio", new ToggleRadioAlertCommand(), "joblog");
            this.Register("togglera", new ToggleRadioAlertCommand(), "joblog", true);
            this.Register("toggler", new ToggleRadioAlertCommand(), "joblog", true);
            this.Register("law", new LawCommand(), "joblog");
            this.Register("unlaw", new UnLawCommand(), "joblog");
            this.Register("stun", new StunCommand(), "joblog");
            this.Register("unstun", new UnStunCommand(), "joblog");
            this.Register("spray", new StunCommand(), "joblog");
            this.Register("unspray", new UnStunCommand(), "joblog");
            this.Register("cuff", new CuffCommand(), "joblog");
            this.Register("uncuff", new UnCuffCommand(), "joblog");
            this.Register("search", new SearchCommand(), "joblog");
            this.Register("arrest", new ArrestCommand(), "joblog");
            this.Register("release", new ReleaseCommand(), "joblog");
            this.Register("ptrial", new PoliceTrialCommand(), "joblog");
            this.Register("unptrial", new PoliceTrialCommand(), "joblog", true);
            this.Register("clearwanted", new ClearWantedCommand(), "joblog");
            this.Register("cw", new ClearWantedCommand(), "joblog", true);
            this.Register("flashbang", new FlashBangCommand(), "joblog");
            this.Register("backup", new BackupCommand(), "joblog");

            // Restaurant & Cafe
            this.Register("serve", new ServeCommand(), "joblog");

            // Banking
            this.Register("openaccount", new OpenAccountCommand(), "joblog");
            this.Register("account", new OpenAccountCommand(), "joblog", true);
            this.Register("checkbalance", new CheckBalanceCommand(), "joblog");

            // Clothing
             this.Register("discount", new DiscountCommand(), "joblog");
        }

        /// <summary>
        /// VIP set of commands
        /// </summary>
        private void RegisterVIP()
        {
            this.Register("push", new PushCommand(), "vip");
            this.Register("pull", new PullCommand(), "vip");
            this.Register("flagme", new FlagMeCommand(), "vip");
            this.Register("setsh", new SetSHCommand(), "vip");
            this.Register("stopsh", new StopSHCommand(), "vip");
            this.Register("opendimmer", new OpenDimmerCommand(), "vip");
            this.Register("odimmer", new OpenDimmerCommand(), "vip", true);
            this.Register("vipa", new VIPAlertCommand(), "vip");
            this.Register("va", new VIPAlertCommand(), "vip", true);
            this.Register("v", new VIPAlertCommand(), "vip", true);
            this.Register("togglevipa", new ToggleVIPAlertCommand(), "vip");
            this.Register("toggleva", new ToggleVIPAlertCommand(), "vip", true);
            this.Register("togglev", new ToggleVIPAlertCommand(), "vip", true);
            this.Register("moonwalk", new MoonwalkCommand(), "vip", true);
        }

        /// <summary>
        /// Ambassador set of commands
        /// </summary>
        private void RegisterAmbassadors()
        {
            this.Register("aa", new AmbassadorAlertCommand(), "ambassadorlog");
            this.Register("aonduty", new AmbassadorOnDutyCommand(), "ambassadorlog");
            this.Register("aoffduty", new AmbassadorOffDutyCommand(), "ambassadorlog");
        }

        /// <summary>
        /// Trial Moderator set of commands
        /// </summary>
        private void RegisterTrialModerators()
        {
            this.Register("sa", new StaffAlertCommand(), "stafflog");
            this.Register("onduty", new OnDutyCommand(), "stafflog");
            this.Register("offduty", new OffDutyCommand(), "stafflog");
        }

        /// <summary>
        /// Moderator set of commands
        /// </summary>
        private void RegisterModerators()
        {
            this.Register("alert", new AlertCommand(), "stafflog");
            this.Register("ban", new BanCommand(), "stafflog");
            this.Register("mute", new MuteCommand(), "stafflog");
            this.Register("unmute", new UnmuteCommand(), "stafflog");
            this.Register("userinfo", new UserInfoCommand(), "stafflog");
            this.Register("update", new UpdateCommand(), "stafflog");
        }

        /// <summary>
        /// Senior Moderator set of commands
        /// </summary>
        private void RegisterSeniorModerators()
        {
            this.Register("ha", new HotelAlertCommand(), "stafflog");
            this.Register("wha", new WhisperHotelAlertCommand(), "stafflog");
            this.Register("nha", new NoticeHotelAlertCommand(), "stafflog");
            this.Register("ipban", new IPBanCommand(), "stafflog");
            this.Register("roomalert", new RoomAlertCommand(), "stafflog");
            this.Register("roommute", new RoomMuteCommand(), "stafflog");
            this.Register("roomunmute", new RoomUnmuteCommand(), "stafflog");
            this.Register("summon", new SummonCommand(), "stafflog");
            this.Register("follow", new FollowCommand(), "stafflog");
            this.Register("unload", new UnloadCommand(), "stafflog");
            this.Register("senduser", new SendUserCommand(), "stafflog");
            this.Register("superhire", new SuperHireCommand(), "stafflog");
        }

        /// <summary>
        /// Administrator set of commands
        /// </summary>
        private void RegisterAdministrators()
        {
            this.Register("at", new AdminTaxiCommand(), "stafflog");
            this.Register("hal", new HALCommand(), "stafflog");
            this.Register("mip", new MIPCommand(), "stafflog");
            this.Register("rpstats", new RPStatsCommand(), "stafflog");
            this.Register("rpweapons", new RPWeaponsCommand(), "stafflog");
            this.Register("rpfarming", new RPFarmingStatsCommand(), "stafflog");
            this.Register("override", new OverrideCommand(), "stafflog");
            this.Register("teleport", new TeleportCommand(), "stafflog");
            this.Register("spull", new SuperPullCommand(), "stafflog");
            this.Register("spush", new SuperPushCommand(), "stafflog");
            this.Register("eventha", new EventAlertCommand(), "stafflog");
            this.Register("restore", new RestoreCommand(), "stafflog");
            this.Register("adminrelease", new AdminReleaseCommand(), "stafflog");
            this.Register("adminjail", new AdminJailCommand(), "stafflog");
            this.Register("roomrestore", new RoomRestoreCommand(), "stafflog");
            this.Register("roomrelease", new RoomReleaseCommand(), "stafflog");
            this.Register("roomheal", new RoomHealCommand(), "stafflog");
            this.Register("warptome", new WarpToMeCommand(), "stafflog");
            this.Register("warpmeto", new WarpMeToCommand(), "stafflog");
            this.Register("blacklist", new BlackListCommand(), "stafflog");
            this.Register("unblacklist", new UnBlackListCommand(), "stafflog");
        }

        /// <summary>
        /// Manager set of commands
        /// </summary>
        private void RegisterManagers()
        {

            this.Register("givebadge", new GiveBadgeCommand(), "stafflog");
            this.Register("roombadge", new RoomBadgeCommand(), "stafflog");
            this.Register("massbadge", new MassBadgeCommand(), "stafflog");
            this.Register("freeze", new FreezeCommand(), "stafflog");
            this.Register("unfreeze", new UnFreezeCommand(), "stafflog");
            this.Register("flagother", new FlagOtherCommand(), "stafflog");
            this.Register("flag", new FlagOtherCommand(), "stafflog", true);
            this.Register("mimic", new MimicCommand(), "staff");
            this.Register("togglewhispers", new ToggleWhispersCommand(), "staff");
            this.Register("disconnect", new DisconnectCommand(), "stafflog");
            this.Register("dc", new DisconnectCommand(), "stafflog", true);
            this.Register("startevent", new StartEventCommand(), "stafflog");
            this.Register("forcestart", new ForceStartCommand(), "stafflog");
            this.Register("stopevent", new StopEventCommand(), "stafflog");
            this.Register("checklottery", new StopEventCommand(), "stafflog", true);
            this.Register("event", new StartEventCommand(), "stafflog", true);
            this.Register("accountcheck", new AccountCheckCommand(), "stafflog");
            this.Register("checkaccount", new AccountCheckCommand(), "stafflog", true);
            this.Register("namecheck", new NameCheckCommand(), "stafflog");
            this.Register("checkname", new NameCheckCommand(), "stafflog", true);
            this.Register("summonstaff", new SummonStaffCommand(), "stafflog");
            this.Register("checkpoll", new CheckPollCommand(), "stafflog");
            this.Register("pollcheck", new CheckPollCommand(), "stafflog", true);
            this.Register("warpalltome", new WarpAllToMeCommand(), "stafflog");
            this.Register("sendroom", new SendRoomCommand(), "stafflog");
            this.Register("freezeroom", new FreezeRoomCommand(), "stafflog");
            this.Register("unfreezeroom", new UnFreezeRoomCommand(), "stafflog");
            this.Register("wonline", new WOnlineCommand(), "stafflog");
            this.Register("makebota", new MakeBotActionCommand(), "stafflog");
            this.Register("banchatter", new BanChatterCommand(), "stafflog");
            this.Register("unbanchatter", new UnBanChatterCommand(), "stafflog");
            this.Register("deletechat", new DeleteChatCommand(), "stafflog");
            this.Register("tlock", new TLockCommand(), "stafflog");
            this.Register("assign", new EventAssignCommand(), "stafflog");
        }

        /// <summary>
        /// Developer set of commands
        /// </summary>
        private void RegisterDevelopers()
        {
           *this.Register("bubble", new BubbleCommand(), "staff");
            this.Register("handitem", new HandItemCommand(), "staff");
            this.Register("enable", new EnableCommand(), "staff");
            this.Register("coords", new CoordsCommand(), "staff");
            this.Register("setspeed", new SetSpeedCommand(), "stafflog");
            this.Register("startquestion", new StartQuestionCommand(), "staff");
            this.Register("kickbots", new KickBotsCommand(), "stafflog");
            this.Register("kickpets", new KickPetsCommand(), "stafflog");
            this.Register("disablediagonal", new DisableDiagonalCommand(), "stafflog");
            this.Register("room", new RoomCommand(), "stafflog");
            this.Register("bot", new BotCommand(), "stafflog");
            this.Register("activebots", new ActiveBotsCommand(), "stafflog");
            this.Register("fixweapons", new FixWeaponsCommand(), "stafflog");
            this.Register("whispertile", new SetWhisperTileCommand(), "stafflog");
            this.Register("page", new HtmlPageCommand(), "stafflog");
            this.Register("upage", new HtmlUPageCommand(), "stafflog");
            this.Register("uipage", new HtmlUIPageCommand(), "stafflog");
            this.Register("rpage", new HtmlRPageCommand(), "stafflog");
            this.Register("maintenance", new MaintenanceCommand(), "stafflog");
            this.Register("maint", new MaintenanceCommand(), "stafflog", true);

            this.Register("todo", new ToDoCommand());
            this.Register("todoadd", new ToDoCommand());
            this.Register("addtodo", new ToDoCommand(), "", true);
            this.Register("tda", new ToDoCommand(), "", true);
            this.Register("tododel", new ToDoCommand());
            this.Register("tododelete", new ToDoCommand(), "", true);
            this.Register("deltodo", new ToDoCommand(), "", true);
            this.Register("deletetodo", new ToDoCommand(), "", true);
            this.Register("tdd", new ToDoCommand(), "", true);

        }

        /// <summary>
        /// Owner set of commands
        /// </summary>
        private void RegisterOwners()
        {
            *this.Register("fastwalk", new FastwalkCommand(), "stafflog");
            this.Register("forcesit", new ForceSitCommand(), "stafflog");
            this.Register("forcelay", new ForceLayCommand(), "stafflog");
            this.Register("allaroundme", new AllAroundMeCommand(), "stafflog");
            this.Register("alleyesonme", new AllEyesOnMeCommand(), "stafflog");
            this.Register("massdance", new MassDanceCommand(), "stafflog");
            this.Register("massenable", new MassEnableCommand(), "stafflog");
            this.Register("summonall", new SummonAllCommand(), "stafflog");
            this.Register("releaseall", new ReleaseAllCommand(), "stafflog");
            this.Register("restoreall", new RestoreAllCommand(), "stafflog");
            this.Register("invisible", new InvisibleCommand(), "stafflog");
            this.Register("visible", new VisibleCommand(), "stafflog");
            this.Register("massact", new MassActionCommand(), "stafflog");
            this.Register("unidle", new UnIdleCommand(), "stafflog");
            this.Register("unban", new UnBanCommand(), "stafflog");
        }

        /// <summary>
        /// Special Right set of commands
        /// </summary>
        private void RegisterSpecialRights()
        {
            *this.Register("makepet", new MakePetCommand(), "stafflog");
            this.Register("transformall", new TransformAllCommand(), "stafflog");
            this.Register("roomtransform", new RoomMakePetCommand(), "stafflog");
            this.Register("summonpets", new SummonPetsCommand(), "stafflog");
            this.Register("pet", new PetTransformCommand(), "stafflog");
            this.Register("colour", new ColourChangeCommand(), "stafflog");
            this.Register("color", new ColourChangeCommand(), "stafflog", true);
            this.Register("changeuclass", new ChangeUClassCommand(), "stafflog");
            this.Register("sfastwalk", new SuperFastwalkCommand(), "stafflog");
            this.Register("makesay", new MakeSayCommand(), "stafflog");
            this.Register("sayall", new SayAllCommand(), "stafflog");
            this.Register("coins", new GiveCoinsCommand(), "stafflog");
            this.Register("duckets", new GiveDucketsCommand(), "stafflog");
            this.Register("diamonds", new GiveDiamondsCommand(), "stafflog");
            this.Register("epoints", new GiveEventPointsCommand(), "stafflog");
            this.Register("rank", new GiveRankCommand(), "stafflog");
            this.Register("kill", new KillCommand(), "stafflog");
            this.Register("setstat", new SetStatCommand(), "stafflog");
            this.Register("sethp", new SetStatCommand(), "stafflog", true);
            this.Register("snap", new KillCommand(), "stafflog");
            this.Register("setenergy", new SetStatCommand(), "stafflog", true);
            this.Register("sethunger", new SetStatCommand(), "stafflog", true);
            this.Register("sethygiene", new SetStatCommand(), "stafflog", true);
            this.Register("givevip", new GiveVIPCommand(), "stafflog");
            this.Register("takevip", new TakeVIPCommand(), "stafflog");
            this.Register("banvip", new BanVIPCommand(), "stafflog");
            this.Register("unbanvip", new UnBanVIPCommand(), "stafflog");
        }
        #endregion

        /// <summary>
        /// Registers a Chat Command.
        /// </summary>
        /// <param name="CommandText">Text to type for this command.</param>
        /// <param name="Command">The command to execute.</param>
        public void Register(string CommandText, IChatCommand Command, string Type = "", bool IsAlias = false)
        {
            if (IsAlias && !this._aliases.Contains(CommandText))
                this._aliases.Add(CommandText);

            switch (Type.ToLower())
            {
                case "job":
                    {
                        this._commands.Add(CommandText, Command);
                        this._jobcommands.Add(CommandText, Command);
                        break;
                    }
                case "gang":
                    {
                        this._commands.Add(CommandText, Command);
                        this._gangcommands.Add(CommandText, Command);
                        break;
                    }
                case "vip":
                    {
                        this._commands.Add(CommandText, Command);
                        this._vipcommands.Add(CommandText, Command);
                        this._loggedcommands.Add(CommandText, Command);
                        break;
                    }
                case "staff":
                    {
                        this._commands.Add(CommandText, Command);
                        this._staffcommands.Add(CommandText, Command);
                        break;
                    }
                case "stafflog":
                    {
                        this._commands.Add(CommandText, Command);
                        this._staffcommands.Add(CommandText, Command);
                        this._loggedcommands.Add(CommandText, Command);
                        break;
                    }
                case "ambassadorlog":
                    {
                        this._commands.Add(CommandText, Command);
                        this._ambassadorcommands.Add(CommandText, Command);
                        this._loggedcommands.Add(CommandText, Command);
                        break;
                    }
                case "userlog":
                    {
                        this._commands.Add(CommandText, Command);
                        this._loggedcommands.Add(CommandText, Command);
                        break;
                    }
                case "joblog":
                    {
                        this._commands.Add(CommandText, Command);
                        this._jobcommands.Add(CommandText, Command);
                        this._loggedcommands.Add(CommandText, Command);
                        break;
                    }
                case "eventlog":
                    {
                        this._commands.Add(CommandText, Command);
                        this._eventcommands.Add(CommandText, Command);
                        this._loggedcommands.Add(CommandText, Command);
                        break;
                    }
                default:
                    {
                        this._commands.Add(CommandText, Command);
                        break;
                    }
            }
        }

        public static string MergeParams(string[] Params, int Start)
        {
            var Merged = new StringBuilder();
            for (int i = Start; i < Params.Length; i++)
            {
                if (i > Start)
                    Merged.Append(" ");
                Merged.Append(Params[i]);
            }

            return Merged.ToString();
        }

        public static string GenerateRainbowText(string Name)
        {
            StringBuilder NewName = new StringBuilder();

            string[] Colours = { "FF0000", "FFA500", "FFFF00", "008000", "0000FF", "800080" };

            int Count = 0;
            int Count2 = 0;
            while (Count < Name.Length)
            {
                NewName.Append("<font color='#" + Colours[Count2] + "'>" + Name[Count] + "</font>");

                Count++;
                Count2++;

                if (Count2 >= 6)
                    Count2 = 0;
            }

            return NewName.ToString();
        }

        public void LogCommand(int UserId, string Data, string MachineId, string Type)
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                if (Type.ToLower() == "staff")
                    dbClient.SetQuery("INSERT INTO `command_logs_staff` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                else if (Type.ToLower() == "ambassador")
                    dbClient.SetQuery("INSERT INTO `command_logs_ambassador` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                else if (Type.ToLower() == "job")
                    dbClient.SetQuery("INSERT INTO `command_logs_jobs` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                else if (Type.ToLower() == "vip")
                    dbClient.SetQuery("INSERT INTO `command_logs_vip` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                else if (Type.ToLower() == "event")
                    dbClient.SetQuery("INSERT INTO `command_logs_events` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                else
                    dbClient.SetQuery("INSERT INTO `command_logs_users` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                dbClient.AddParameter("UserId", UserId);
                dbClient.AddParameter("Data", Data);
                dbClient.AddParameter("MachineId", MachineId);
                dbClient.AddParameter("Timestamp", Plus.GetUnixTimestamp());
                dbClient.RunQuery();
            }
        }

        public bool TryGetCommand(string Command, out IChatCommand IChatCommand)
        {
            return this._commands.TryGetValue(Command, out IChatCommand);
        }
    }
}*/