using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.Database.Manager.Database.Session_Details.Interfaces;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class ATMRobTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // 5 minutes (milliseconds)

        public ATMRobTimer(GameClient Session, int intRobTime)
        {
            this.Session = Session;

            int time = intRobTime;
            timeLeft = time * 60000;

            startTimer();
        }

        public void startTimer()
        {
            TimerCallback timerFinished = timerDone;

            timer = new Timer(timerFinished, null, 60000, Timeout.Infinite);
        }

        public void timerDone(object info)
        {
            try
            {
                timeLeft -= 60000;

                #region Conditions
                if (Session == null)
                { stopTimer(); return; }

                if (timeLeft == 4 * 60000)
                {
                    Session.Shout("*Hits the ATM with a crowbar, forming a small hole*", 3);
                }
                else if (timeLeft == 3 * 60000)
                {
                    Session.Shout("*Increases the hole size by prying it with crowbar*", 3);
                }
                else if (timeLeft == 2 * 60000)
                {
                    Session.Shout("*Stabs another hole into the ATM using the crowbar*", 3);
                }
                else if (timeLeft == 1 * 60000)
                {
                    Session.Shout("*Uses their strength of " + (Session.GetRoleplay().Strength + Session.GetRoleplay().savedSTR) + " to pry open the hole in the machine");
                }
                if (timeLeft > 0)
                {
                    int minutesRemaining = timeLeft / 60000;
                    Session.SendWhisper("You have " + minutesRemaining + " minutes until you rob the ATM!");
                    timer.Change(60000, Timeout.Infinite);
                    int xp = new Random().Next(1, 50);
                    LevelHandler.AddEXP(Session, xp);
                    return;
                }

                #endregion

                #region Execute
                Random rnd = new Random();
                int money = 0;
                if (Session.GetRoleplay().Intelligence < 5)
                {
                    money = rnd.Next(0, 100);
                }
                else if (Session.GetRoleplay().Intelligence < 15)
                {
                    money = rnd.Next(100, 225);
                }
                else
                {
                    money = rnd.Next(200, 400);
                }


                RoleplayManager.Shout(Session, "*Successfully steals from the ATM [+$" + money + "]*", 6);
                Session.GetRoleplay().SaveStats();
                RoleplayManager.GiveMoney(Session, +money);
                Session.GetRoleplay().ATMRobbery = false;
                LevelHandler.AddEXP(Session, rnd.Next(1, 100));

                stopTimer();
                #endregion
            }
            catch { stopTimer(); }
        }

        public int getTime()
        {
            int minutesRemaining = timeLeft / 60000;
            return minutesRemaining;
        }

        public void stopTimer()
        {
            timer.Dispose();
        }
    }
}
