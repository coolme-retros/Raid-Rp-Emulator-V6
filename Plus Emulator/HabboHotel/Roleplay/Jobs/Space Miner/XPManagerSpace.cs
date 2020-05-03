﻿#region Librarys

using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading;
using Plus.Connection.ServerManager;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.Rooms;

#endregion


namespace Plus.HabboHotel.Roleplay.Jobs.Space
{
    public static class xpManagerSpace
    {
        public static readonly Dictionary<int, int> levels = new Dictionary<int, int>
        {
            {1, 0}, // Do not change this, feel free 2 change all the others u fuckin fag
            {2, 10000},
            {3, 17500},
            {4, 30000},
        };

        public static void addXP(GameClient Session, int amount = 15)
        {
            try
            {
                if (Session != null)
                {
                    Session.GetRoleplay().SpaceXP += amount;
                    Session.GetRoleplay().UpdateStats = 10;

                    if (levelUp(Session))
                    {
                        
                        Session.GetRoleplay().UpdateStats = 10;
                        RoleplayManager.Shout(Session, "*Levels up to a level " + Session.GetRoleplay().spacelevel + " space miner*");
                    }
                    else
                    {
                        Session.SendWhisper("You've received XP for space mining! [" + Session.GetRoleplay().SpaceXP + " / " + levels[Session.GetRoleplay().spacelevel + 1] + "]");
                    }
                }
            }
            catch { }
        }

        public static bool levelUp(GameClient Session)
        {
            try
            {
                if (Session != null)
                {
                    int spaceXP = Session.GetRoleplay().SpaceXP;
                    int spacelevel = Session.GetRoleplay().spacelevel + 1;

                    if (spacelevel <= 5)
                    {
                        if (spaceXP >= levels[spacelevel])
                        {
                            return true;
                        }

                    }
                    return false;
                }
                return false;
            }
            catch { return false; }   
        }
    }
}
