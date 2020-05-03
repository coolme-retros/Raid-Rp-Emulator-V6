using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users;
using System;
using System.Collections.Generic;
using Plus.HabboHotel.Roleplay.Misc;
using System.Linq;
using System.Text;
using Plus.HabboHotel.Roleplay.Jobs.Space;
using Plus.HabboHotel.Roleplay.Jobs.Farming;
using Plus.HabboHotel.Roleplay.Jobs;
using Plus.HabboHotel.Roleplay.Gangs;
using Plus.HabboHotel.Roleplay.Combat;
using Plus.HabboHotel.Roleplay.Timers;
using Plus.HabboHotel.Roleplay.Radio;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users;
using System;
using System.Collections.Generic;
using Plus.HabboHotel.Roleplay.Misc;
using System.Linq;
using System.Text;
using Plus.HabboHotel.Roleplay.Jobs.Space;
using Plus.HabboHotel.Roleplay.Jobs.Farming;
using Plus.HabboHotel.Roleplay.Jobs;
using Plus.HabboHotel.Roleplay.Gangs;
using Plus.HabboHotel.Roleplay.Combat;
using Plus.HabboHotel.Roleplay.Timers;
using Plus.HabboHotel.Roleplay.Radio;
using Plus.HabboHotel.Roleplay.Jobs.Cutting;
using Plus.Database.Manager.Database.Session_Details.Interfaces;
using System.Data;
using System.Threading;
using Plus.HabboHotel.Misc;
using Plus.HabboHotel.PathFinding;

namespace Plus.HabboHotel.Items.Interactor
{
    public class InteractorRP : IFurniInteractor
    {
        public void OnUserWalk(GameClient session, RoomItem item, RoomUser user)
        {

        }

        public void OnPlace(GameClients.GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            Item.UpdateNeeded = true;
        }

        public void OnRemove(GameClients.GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
        }

        public void OnTrigger(GameClients.GameClient Session, RoomItem Item, int Request, bool HasRights)
        {
            if (Item.GetBaseItem().Name.ToLower().Contains("atm"))
            {
                //HandleATM(Session, Item, Request, HasRights);
                Session.SendWhisperBubble("You MUST use :insertcard to use the ATM!",1);
            }

            if (Item.GetBaseItem().Name.ToLower().Contains("md_limukaappi"))
            {
                //GetARefreshingSoda(Session, Item, Request, HasRights);
            }

            if (Item.BaseItem == 1943) // prison_stones baseID
            {
                handleRock(Session, Item);
            }

            if (Item.BaseItem == 1737) // dirt nest baseID
            {
                handleFarmingSpot(Session, Item);
            }
            if (Item.BaseItem == 3514)
            {
                handleEnergyDrinkPurchase(Session, Item);
            }
            if (Item.BaseItem == 8039)
            {
                handleMedKitPurchase(Session, Item);
            }
            if (Item.BaseItem == 4495)
            {
                handleBroteinPurchase(Session, Item);
            }

                if (Item.GetBaseItem().Name.ToLower().Contains("wf_floor_switch1"))
            {
                HandlePullTheHandleForPolice(Session, Item, Request, HasRights);
            }

            if (Item.GetBaseItem().Name.ToLower().Contains("hc_machine"))
            {
                HandleNPA(Session, Item, Request, HasRights);
            }

            uint Modes = Item.GetBaseItem().Modes - 1;

            if (Session == null || !HasRights || Modes <= 0)
            {
                return;
            }

            Plus.GetGame().GetQuestManager().ProgressUserQuest(Session, HabboHotel.Quests.QuestType.FurniSwitch);

            int CurrentMode = 0;
            int NewMode = 0;

            if (!int.TryParse(Item.ExtraData, out CurrentMode))
            {

            }

            if (CurrentMode <= 0)
            {
                NewMode = 1;
            }
            else if (CurrentMode >= Modes)
            {
                NewMode = 0;
            }
            else
            {
                NewMode = CurrentMode + 1;
            }

            Item.ExtraData = NewMode.ToString();
            Item.UpdateState();

        }

        public void handleRock(GameClient Session, RoomItem Item)
        {
            RoomUser User = Session.GetHabbo().GetRoomUser();
            Rock theRock = spaceManager.getRockByItem(Item);

            if (!JobManager.JobRankData[Session.GetRoleplay().JobId, Session.GetRoleplay().JobRank].hasRights("spaceminer"))
            {
                Session.SendWhisperBubble("You must be a space miner to mine this rock!", 1);
                return;
            }

            if (!Session.GetRoleplay().Working)
            {
                Session.SendWhisperBubble("You must be working to do this!", 1);
                return;
            }
            if (theRock != null && spaceManager.isUserNearRock(theRock, User))
            {
                if (theRock.beingMined == false)
                {
                    mineTimer timer = new mineTimer(Session, theRock);
                    timer.startTimer();
                }
                else
                {
                    Session.SendWhisperBubble("This rock is already being mined!", 1);
                }
            }
            else
            {
                Session.SendWhisperBubble("You aren't close enough to the rock!", 1);
            }
        }
        public void handleConsole(GameClient Session, RoomItem Item) //1623
        {
            string help = "================== Room Help & Commands ===================\n\n";
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `rp_roomhelp` WHERE `roomid` = '" + Session.GetHabbo().CurrentRoomId + "'");
                DataTable Table = dbClient.GetTable();
                foreach (DataRow Row in Table.Rows)
                {
                    int HelpType = Convert.ToInt32(Row["type"]);
                    if (HelpType == 1)
                    {
                        help += "" + Row["details"] + "\n";
                    }
                    else
                    {
                        help += "" + Row["details"] + "\n\n";
                    }
                }
            }
            Session.SendNotifWithScroll(help);
        }
        public void handleBroteinPurchase(GameClient Session, RoomItem Item) //4495
        {
            //+2 STR Health
            if (Session.GetHabbo().Credits < 20)
            {
                Session.SendWhisper("Brotein costs $20, you do not have enough money");
                return;
            }
            if (Session.GetHabbo().CurrentRoomId != 6)
            {
                return;
            }
            if (RoleplayManager.Distance(new Vector2D(Item.X, Item.Y), new Vector2D(Session.GetHabbo().GetRoomUser().X, Session.GetHabbo().GetRoomUser().Y)) > 2)
            {
                Session.SendWhisper("You must be closer to the object you wish to purchase");
                return;
            }
            Session.GetHabbo().Credits -= 20;
            Session.GetHabbo().UpdateCreditsBalance();
            Session.Shout("*Purchases Brotein for $20 [-$20]*");
            Session.GetHabbo().GetInventoryComponent().AddNewItem(0u, Item.BaseItem, "0", 0u, true, false, 0, 0, string.Empty);
            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
        }
        public void handleMedKitPurchase(GameClient Session, RoomItem Item) //8039
        {
            //80 Health
            if (Session.GetHabbo().Credits < 20)
            {
                Session.SendWhisper("This kit costs $20, you do not have enough money");
                return;
            }
            if (Session.GetHabbo().CurrentRoomId != 1)
            {
                return;
            }
            if (RoleplayManager.Distance(new Vector2D(Item.X, Item.Y), new Vector2D(Session.GetHabbo().GetRoomUser().X, Session.GetHabbo().GetRoomUser().Y)) > 2)
            {
                Session.SendWhisper("You must be closer to the object you wish to purchase");
                return;
            }
            Session.GetHabbo().Credits -= 20;
            Session.GetHabbo().UpdateCreditsBalance();
            Session.Shout("*Purchases Medkit for $20 [-$20]*");
            Session.GetHabbo().GetInventoryComponent().AddNewItem(0u, Item.BaseItem, "0", 0u, true, false, 0, 0, string.Empty);
            Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
        }
        public void handleEnergyDrinkPurchase(GameClient Session, RoomItem Item) //3514
        {
            // 25 health
            if (Session.GetHabbo().Credits < 4)
            {
                Session.SendWhisper("This drink costs $4, you do not have enough money");
                return;
            }
            if (Session.GetHabbo().CurrentRoomId != 6)
            {
                return;
            }
            if (RoleplayManager.Distance(new Vector2D(Item.X, Item.Y), new Vector2D(Session.GetHabbo().GetRoomUser().X, Session.GetHabbo().GetRoomUser().Y)) > 2)
            {
                Session.SendWhisper("You must be closer to the object you wish to purchase");
                return;
            }
            Session.GetHabbo().Credits -= 4;
            Session.GetHabbo().UpdateCreditsBalance();
            Session.Shout("*Purchases Energy Drink for $4 [-$4]*");
            Session.GetHabbo().GetInventoryComponent().AddNewItem(0u, Item.BaseItem, "0", 0u, true, false, 0, 0, string.Empty);
            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
        }

        public void handleFarmingSpot(GameClient Session, RoomItem Item)
        {
            RoomUser User = Session.GetHabbo().GetRoomUser();
            FarmingSpot theFarmingSpot = farmingManager.getFarmingSpotByItem(Item);

            if (!JobManager.JobRankData[Session.GetRoleplay().JobId, Session.GetRoleplay().JobRank].hasRights("farming"))
            {
                if (theFarmingSpot.type == "weed") { Session.SendWhisperBubble("You must be a farmer to plant weed here!", 1); }
                else if (theFarmingSpot.type == "carrot") { Session.SendWhisperBubble("You must be a farmer to plant carrots here!", 1); }
                else { Session.SendWhisperBubble("You must be a farmer to plant here!", 1); }
                return;
            }

            if (!Session.GetRoleplay().Working)
            {
                if (theFarmingSpot.type == "weed") { Session.SendWhisperBubble("You must be working to plant weed here!", 1); }
                else if (theFarmingSpot.type == "carrot") { Session.SendWhisperBubble("You must be working to plant carrots here!", 1); }
                else { Session.SendWhisperBubble("You must be working to plant here!", 1); }
                return;
            }

            if (theFarmingSpot != null && farmingManager.isUserNearFarmingSpot(theFarmingSpot, User))
            {
                if (theFarmingSpot.beingFarmed2 == false && theFarmingSpot.Part1Complete == true)
                {
                    farmingTimer2 timer = new farmingTimer2(Session, theFarmingSpot);
                    timer.startTimer();
                }
                else if (theFarmingSpot.beingFarmed == false)
                {
                    farmingTimer1 timer = new farmingTimer1(Session, theFarmingSpot);
                    timer.startTimer();
                }
                else
                {
                    Session.SendWhisperBubble("This spot is already being farmed!", 1);
                }
            }
            else
            {
                Session.SendWhisperBubble("You aren't close enough to the farming spot!", 1);
            }
        }

        public void HandleATM(GameClients.GameClient Session, RoomItem Item, int Request, bool HasRights)
        {
            RoomUser User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            int Price = 3;
            double IncomeTaxMath = Price * 0.13;
            double SalesTaxMath = Price * 0.50;
            int SalesTax = 0;
            int IncomeTax = 0;
            int TotalPrice = 0;
            int TotalIncome = 0;
            IncomeTax = (int)Math.Round(IncomeTaxMath, 0);
            SalesTax = (int)Math.Round(SalesTaxMath, 0);
            TotalPrice = Price + SalesTax;
            TotalIncome = Price - IncomeTax;
            if (Item.InteractingUser2 != User.UserId)
                Item.InteractingUser2 = User.UserId;

            if (User == null)
            {
                return;
            }

            if (User.Coordinate != Item.SquareInFront && User.CanWalk)
            {
                User.MoveTo(Item.SquareInFront);
                return;
            }
            if (Session.GetRoleplay().BankAccount != 1)
            {
                Session.SendWhisperBubble("Oof, you need a bank account to do this!");
                return;
            }
            if (Session.GetRoleplay().inATM == true)
            {
                Session.SendWhisperBubble("[ATM MSG] You are already logged in!", 1);
                return;
            }

            Session.GetRoleplay().inATM = true;

            Session.SendWhisperBubble("[ATM MSG] Processing transaction please wait...", 1);

            int amount = Session.GetRoleplay().AtmSetAmount;

            if (amount > Session.GetRoleplay().Bank)
            {
                Session.SendWhisperBubble("[ATM MSG] Transaction failed, insufficient funds!", 1);
                Session.GetHabbo().GetRoomUser().UnlockWalking();
                RoleplayManager.GiveMoney(Session, -TotalPrice);
                JobManager.JobData[4].Balance += TotalPrice;
                JobManager.JobData[6].Balance += IncomeTax + SalesTax;
                RoleplayManager.SaveCorpBalance(4);
                RoleplayManager.SaveCorpBalance(6);
                JobManager.init();
                Session.GetRoleplay().inATM = false;
                return;
            }
            else
            {

                System.Threading.Thread.Sleep(2000);

                Session.SendWhisperBubble("[ATM MSG] Transaction succesful!", 1);
                RoleplayManager.Shout(Session, "*Uses the ATM to withdraw $" + amount + " from their account [+$" + amount + "]*, 4");
                Session.GetHabbo().GetRoomUser().UnlockWalking();
                Session.GetRoleplay().inATM = false;
                Session.GetRoleplay().Bank -= amount;
                Session.GetRoleplay().SaveQuickStat("bank", "" + Session.GetRoleplay().Bank);
                RoleplayManager.GiveMoney(Session, +amount);
                RoleplayManager.GiveMoney(Session, -TotalPrice);
                JobManager.JobData[4].Balance += TotalPrice;
                JobManager.JobData[6].Balance += IncomeTax + SalesTax;
                RoleplayManager.SaveCorpBalance(4);
                RoleplayManager.SaveCorpBalance(6);
                JobManager.init();
                Session.GetRoleplay().AtmSetAmount = 20;
            }
        }

        /*public void GetARefreshingSoda(GameClients.GameClient Session, RoomItem Item, int Request, bool HasRights)
        {
            int price = Convert.ToInt32(RPDataHandler.GetData("rp_vending", "soda_price"));
            RoomUser User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            int Price = price;
            double IncomeTaxMath = Price * 0.13;
            double SalesTaxMath = Price * 0.50;
            int SalesTax = 0;
            int IncomeTax = 0;
            int TotalPrice = 0;
            int TotalIncome = 0;
            IncomeTax = (int) Math.Round(IncomeTaxMath, 0);
            SalesTax = (int) Math.Round(SalesTaxMath, 0);
            TotalPrice = Price + SalesTax;
            TotalIncome = Price - IncomeTax;
            Random Rand = new Random();
            int xp = Rand.Next(1, 20);
            int cost = Rand.Next(10, 20);
            int energy = Rand.Next(1, 100);
            if (Item.InteractingUser2 != User.UserId)
                Item.InteractingUser2 = User.UserId;

            if (User == null)
            {
                return;
            }

            if (User.Coordinate != Item.SquareInFront && User.CanWalk)
            {
                User.MoveTo(Item.SquareInFront);
                return;
            }

            if (Session.GetHabbo().Credits < TotalPrice)
            {
                User.GetClient().SendWhisperBubble("You don't have enough credits to execute this action!");
                return;
            }

            int energy_limit = Convert.ToInt32(RPDataHandler.GetData("rp_vending", "vending_energy_limit"));
            if (Session.GetRoleplay().Energy > energy_limit)
            {
                Session.SendWhisperBubble("You must have more than " + energy_limit);
                return;
            }

            uint gym_id = Convert.ToUInt32(RPDataHandler.GetData("gym_info", "id"));
            if (Session.GetHabbo().CurrentRoomId == gym_id)
            {
                Session.SendWhisperBubble("[Cola Machine] Vending....", 1);
                Thread.Sleep(2000);
                Session.Shout("Gets them a nice refreshing drink from the cola machine [+" + energy + "E [-$" +
                              TotalPrice + "]");
                JobManager.JobData[6].Balance += TotalIncome;
                JobManager.JobData[13].Balance += TotalIncome;
                RoleplayManager.SaveCorpBalance(6);
                RoleplayManager.SaveCorpBalance(13);
                JobManager.init();
                RoleplayManager.GiveMoney(Session, -TotalPrice);
            }
            else
            {



                Session.SendWhisperBubble("[Cola Machine] Vending....", 1);
                Thread.Sleep(2000);
                Session.Shout("Gets them a nice refreshing drink from the cola machine [+" + energy + "E [-$" +
                              TotalPrice + "]");
                Session.GetRoleplay().Energy += energy;
                LevelHandler.AddEXP(Session, xp);
                RoleplayManager.GiveMoney(Session, -TotalPrice);
            }

        }*/

        public void HandlePullTheHandleForPolice(GameClients.GameClient Session, RoomItem Item, int Request, bool HasRights)
        {

            RoomUser User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);


            if (Item.InteractingUser2 != User.UserId)
                Item.InteractingUser2 = User.UserId;

            if (User == null)
            {
                return;
            }

            if (User.Coordinate != Item.SquareInFront && User.CanWalk)
            {
                User.MoveTo(Item.SquareInFront);
                return;
            }

            if (!Session.GetRoleplay().MultiCoolDown.ContainsKey("rp_vswitch"))
            {
                Session.GetRoleplay().MultiCoolDown.Add("rp_vswitch", 0);
            }
            if (Session.GetRoleplay().MultiCoolDown["rp_vswitch"] > 0)
            {
                Session.SendWhisper("You must wait until you can pull the switch! [" + Session.GetRoleplay().MultiCoolDown["rp_vswitch"] + "/15]");
                return;
            }

            lock (Plus.GetGame().GetClientManager().Clients.Values)
            {
                foreach (GameClient client in Plus.GetGame().GetClientManager().Clients.Values)
                {

                    if (client == null)
                        continue;
                    if (client.GetRoleplay() == null)
                        continue;
                    if (!JobManager.validJob(client.GetRoleplay().JobId, client.GetRoleplay().JobRank))
                        continue;
                    if (!client.GetRoleplay().JobHasRights("police"))
                        continue;
                    if (!client.GetRoleplay().JobHasRights("swat"))
                        continue;
                    /*if (!client.GetRoleplay().JobHasRights("gov"))
                        continue;*/
                    if (!client.GetRoleplay().Working)
                        continue;

                    client.GetHabbo().GetRoomUser().LastBubble = 30;
                    client.SendWhisper("<b>[Police Radio]:</b> " + Session.GetHabbo().UserName + " has pulled the switch at " + Session.GetHabbo().CurrentRoomId + ", acquiring help!");
                    client.GetHabbo().GetRoomUser().LastBubble = 0;
                }
            }
            RoleplayManager.Shout(Session, "*Pulls the trigger of the switch, notifying the cops*");
            Radio.send(Session.GetHabbo().UserName + " has pulled the switch at " + Session.GetHabbo().CurrentRoomId + ", acquiring help!", Session, false);
            Session.GetRoleplay().MultiCoolDown["rp_vswitch"] = 15;
            Session.GetRoleplay().CheckingMultiCooldown = true;
        }

        public void HandleNPA(GameClients.GameClient Session, RoomItem Item, int Request, bool HasRights)
        {

            RoomUser User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);


            if (Item.InteractingUser2 != User.UserId)
                Item.InteractingUser2 = User.UserId;

            if (User == null)
            {
                return;
            }

            if (User.Coordinate != Item.SquareInFront && User.CanWalk)
            {
                User.MoveTo(Item.SquareInFront);
                return;
            }

            if (Item.OnNPAUsing)
            {
                User.GetClient().SendWhisperBubble("Someone is already using this machine to nuke the city!", 1);
                return;
            }

            if (RoleplayManager.NukesOccurred > 5)
            {
                User.GetClient().SendWhisperBubble("The system has reached a maximum amount of nukes per emulator reboot. Please try again later.", 1);
                return;
            }

            RoleplayManager.Shout(User.GetClient(), "*Starts the process in nuking the city*", 4);
            User.GetClient().GetRoleplay().npaTimer = new nukeTimer(User.GetClient());
            User.GetClient().GetRoleplay().NPA = true;
            User.GetClient().SendWhisperBubble("You have " + User.GetClient().GetRoleplay().npaTimer.getTime() + " minutes until you nuke the city.", 1);
            Item.OnNPAUsing = true;
            RoleplayManager.NukesOccurred++;


        }


        public void OnWiredTrigger(RoomItem Item)
        {
            if (Item.ExtraData == "0")
            {
                Item.ExtraData = "1";
                Item.UpdateState(false, true);
                Item.ReqUpdate(4, true);
            }
        }
    }
}