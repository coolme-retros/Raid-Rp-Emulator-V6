using Plus.Database.Manager.Database.Session_Details.Interfaces;
using Plus.HabboHotel.GameClients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plus.HabboHotel.Roleplay.Misc
{
    class Tickets
    {
        public static ConcurrentDictionary<string, int> TicketUsers;

        public static void LoadTicket()
        {
            TicketUsers = new ConcurrentDictionary<string, int>();
            TicketUsers.Clear();
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT user, amount FROM rp_tickets");

                foreach (DataRow Row in dbClient.GetTable().Rows)
                {
                    TicketUsers.TryAdd((string)Row["user"], (int)Row["amount"]);
                }
            }

        }

        public static void SaveTicket()
        {
            if (TicketUsers.Count > 0)
            {
                using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunFastQuery("TRUNCATE TABLE rp_tickets;");
                    foreach (string username in TicketUsers.Keys)
                    {
                        dbClient.RunFastQuery("INSERT INTO rp_tickets (user, amount) VALUES ('" + username + "', '" + TicketUsers[username] + "');");
                    }
                }
            }
        }

        public static int SetTicket(string username, int amount)
        {
            if (TicketUsers.ContainsKey(username))
            {
                TicketUsers[username] += amount;
            }
            else
            {
                TicketUsers.TryAdd(username, amount);
            }
            return TicketUsers[username];
        }

        public static void CheckTicket(GameClient user, string username)
        {
            if (TicketUsers.ContainsKey(username))
            {
                int junkTicket = TicketUsers[username];

                user.Shout("*Claims $" + TicketUsers[username] + " from " + username + "'s ticket*");
                user.SendWhisper("You have claimed $" + TicketUsers[username] + " from the ticket!");
                RoleplayManager.GiveMoney(user, +TicketUsers[username]);
                GameClient TargetSession = null;
                RoleplayManager.GiveMoney(TargetSession, -TicketUsers[username]);
                TicketUsers.TryRemove(username, out junkTicket);
            }
        }
    }
}
