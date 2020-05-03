using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Plus.Purge

using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Roleplay.Minigames.Purge;
using Plus.HabboHotel.Roleplay.Misc;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class StatTimeTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // Based on whoever senthome the user (milliseconds)

        public StatTimeTimer(GameClient Session)
        {
            this.Session = Session;

            int time = Session.GetRoleplay().SendHomeTimer;
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

                if (timeLeft > 0)
                {
                    int minutesRemaining = timeLeft / 60000;
                    Session.GetRoleplay().SendHomeTimer = minutesRemaining;
                    
                    Session.SendWhisper("The purge will end in " + minutesRemaining + " minute(s)!");
                    timer.Change(60000, Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute

                Session.SendWhisper("The purge is over...");
                PurgeManager.Running = false;
                RoleplayManager.PurgeTime = false;

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
