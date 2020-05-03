/*using MySql.Data.MySqlClient.Memcached;
using Plus.Configuration;
using Plus.Database.Manager.Database.Session_Details.Interfaces;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.PathFinding;
using Plus.HabboHotel.Pets;
using Plus.HabboHotel.Roleplay;
using Plus.HabboHotel.Roleplay.Apartments;
using Plus.HabboHotel.Roleplay.Bots;
using Plus.HabboHotel.Roleplay.Casino.Slots;
using Plus.HabboHotel.Roleplay.Combat;
using Plus.HabboHotel.Roleplay.Combat.WeaponExtras;
using Plus.HabboHotel.Roleplay.Gangs;
using Plus.HabboHotel.Roleplay.Instance;
using Plus.HabboHotel.Roleplay.Jobs;
using Plus.HabboHotel.Roleplay.Jobs.Cutting;
using Plus.HabboHotel.Roleplay.Jobs.Farming;
using Plus.HabboHotel.Roleplay.Jobs.Space;
using Plus.HabboHotel.Roleplay.Minigames.Colour_Wars;
using Plus.HabboHotel.Roleplay.Minigames.Colour_Wars;
using Plus.HabboHotel.Roleplay.Minigames.Purge;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.Roleplay.Radio;
using Plus.HabboHotel.Roleplay.Alert;
using Plus.HabboHotel.Roleplay.Timers;
using Plus.HabboHotel.Roleplay.Timers;
using Plus.HabboHotel.RoomBots;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users;
using Plus.Messages;
using Plus.Messages.Parsers;
using Plus.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
//Regex for string check
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Plus.HabboHotel.Chat.BasicCommands.AdminCommands
{
    public class FastWalk
    {
        public static void Execute()
        {

            static Boolean Parse(GameClient Session, string Input)
            {


                if (!RoleplayManager.BypassRights(Session))
                {

                    Session.SendWhisper("You're not allowed to do this..");
                    return true;
                }

                Room Room = Plus.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

                if (!User.FastWalking)
                {
                    User.FastWalking = true;
                }
                else
                {
                    User.FastWalking = false;
                }


                return true;
            }

        }
    }
}
*/
