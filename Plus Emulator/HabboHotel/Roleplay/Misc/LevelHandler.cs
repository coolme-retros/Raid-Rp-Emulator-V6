using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Roleplay;
using Plus.HabboHotel.Roleplay.Gangs;
using Plus.Database.Manager.Database.Session_Details.Interfaces;


namespace Plus.HabboHotel.Roleplay.Misc
{
    class LevelHandler
    {
        #region Level Shit
        public static readonly Dictionary<int, int> Levels = new Dictionary<int, int>
        {
            {1,750},
            {2,1000},
            {3,1334},
            {4,1778},
            {5,2372},
            {6,3162},
            {7,3162},
            {8,5624},
            {9,7499},
            {10,10001},
            {11,13337},
            {12,17784},
            {13,23715},
            {14,31625},
            {15,42173},
            {16,56237},
            {17,74993},
            {18,100005},
            {19,133357},
            {20,177834},
            {21,237144},
            {22,316235},
            {23,421704},
            {24,562347},
            {25,749899},
            {26,1000000}
        };
        #endregion

        public static void AddEXP(GameClient Session, int amount)
        {
            try
            {
                Session.GetRoleplay().RefreshVals();
                amount = +amount;
                if (Session != null && Session.GetRoleplay() != null)
                {
                    Session.GetRoleplay().CurrentXP += amount;
                    //Session.GetRoleplay().SaveQuickStat("currentxp", +amount);
                    Session.GetRoleplay().SaveStats();

                    if (LevelUp(Session, "level"))
                    {
                        Session.GetRoleplay().CurrentLevel += 1;
                        Session.GetRoleplay().SaveQuickStat("currentlevel", +1);

                        RoleplayManager.Shout(Session, "*Ranks up to Level " + Session.GetRoleplay().CurrentLevel + " [-LXP] [+1L]*");
                        Session.SendWhisper("You have just leveled up! You are now Level: " + Session.GetRoleplay().CurrentLevel + ".");
                        if (Session.GetRoleplay().GangId >= 1)
                        {
                            Session.GetHabbo().Motto = "[Gang: " + GangManager.GangData[Session.GetRoleplay().GangId].Name + "] [Level: " + Session.GetRoleplay().CurrentLevel + "]";
                            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunFastQuery("UPDATE `users` SET `motto` = '[Gang: " + GangManager.GangData[Session.GetRoleplay().GangId].Name + "] [Level: " + Session.GetRoleplay().CurrentLevel + "]' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                                dbClient.RunQuery();
                                Session.GetRoleplay().RefreshVals();
                                Session.GetRoleplay().MottoUpdate = true;
                            }
                        }
                        else
                        {
                            Session.GetHabbo().Motto =
                                "[Gang: None] [Level: " +
                                Session.GetRoleplay().CurrentLevel + "]";
                            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunFastQuery("UPDATE `users` SET `motto` = '[Gang: " +
                                                      GangManager.GangData[Session.GetRoleplay().GangId].Name +
                                                      "] [Level: " + Session.GetRoleplay().CurrentLevel +
                                                      "]' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                                dbClient.RunQuery();
                                Session.GetRoleplay().RefreshVals();
                                Session.GetRoleplay().MottoUpdate = true;
                            }
                        }
                    }
                    else
                        Session.SendWhisper("A total of " + String.Format("{0:N0}", amount) + " XP has been awarded to your account, you now need " + String.Format("{0:N0}", (Levels[Session.GetRoleplay().CurrentLevel + 1] - Session.GetRoleplay().CurrentXP)) + " XP to to get to level " + (Session.GetRoleplay().CurrentLevel + 1));
                }
            }
            catch
            {

            }
        }

        public static bool LevelUp(GameClient Session, string Type)
        {
            try
            {
                if (Session != null && Session.GetRoleplay() != null)
                {
                    int Level = 1;
                    int EXP = 0;
                    Dictionary<int, int> Dictionary = null;

                    switch (Type.ToLower())
                    {
                        case "level":
                            {
                                Level = Session.GetRoleplay().CurrentLevel;
                                EXP = Session.GetRoleplay().CurrentXP;
                                Dictionary = Levels;
                                break;
                            }

                    }

                    if (Dictionary == null)
                        return false;

                    if (Dictionary.ContainsKey(Level + 1))
                    {
                        if (EXP >= Dictionary[Level + 1])
                            return true;
                    }
                    return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
