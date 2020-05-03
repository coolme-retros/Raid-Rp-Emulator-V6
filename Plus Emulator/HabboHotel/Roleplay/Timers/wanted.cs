using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class wantedTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // 10 minutes (milliseconds)

        public wantedTimer(GameClient Session)
        {
            this.Session = Session;

            int time = 20;
            timeLeft = time * 5;

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

                if (timeLeft > 0)
                {
                    int minutesRemaining = timeLeft / 60000;
                   // Session.GetRoleplay().SaveStatusComponents("robbery");
                    Session.SendWhisper("You have " + minutesRemaining + " minute(s) left until you lose your wanted level!");
                    timer.Change(60000, Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute
              
                RoleplayManager.Shout(Session, "*Loses their wanted level*");
              //  Session.GetRoleplay().SaveStatusComponents("robbery");
                    Session.GetRoleplay().Wanted = 0;
                    Session.GetRoleplay().SaveQuickStat("wanted", Session.GetRoleplay().Wanted);
                    Session.GetRoleplay().SaveQuickStat("wanted_for", "");
                //string wantedJunk = Session.GetHabbo().UserName.ToLower();
                //RoleplayManager.WantedListData.TryRemove(Session.GetHabbo().UserName.ToLower(), out wantedJunk);
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
