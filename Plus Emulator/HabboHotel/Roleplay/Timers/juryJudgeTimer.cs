using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using System.Threading;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class juryJudgeTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // 10 minutes (milliseconds)

        public juryJudgeTimer(GameClient Session)
        {
            this.Session = Session;

            int time = 3;
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
                    Session.SendWhisperBubble("There is " + minutesRemaining + " minutes left until the votes are complete.", 5);
                    timer.Change(60000, Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute
                Session.SendWhisperBubble("Votes have been placed! Type :viewvotes to see the outcome", 5);
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
