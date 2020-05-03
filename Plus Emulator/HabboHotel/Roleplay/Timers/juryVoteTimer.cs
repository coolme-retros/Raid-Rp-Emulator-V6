using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class juryVoteTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // 10 minutes (milliseconds)

        public juryVoteTimer(GameClient Session)
        {
            this.Session = Session;

            int time = 1;
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
                    Session.SendWhisperBubble("You have " + minutesRemaining + " minutes to vote", 5);
                    timer.Change(60000, Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute
                Session.SendWhisperBubble("It's too late to vote now sorry!", 5);
                RoleplayManager.RequestingVotes = false;
                RoleplayManager.ReadyToView = true;

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
