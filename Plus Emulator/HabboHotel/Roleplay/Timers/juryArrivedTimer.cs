using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Misc;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Roleplay.Minigames.Purge;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class juryArrivedTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // 10 minutes (milliseconds)

        public juryArrivedTimer(GameClient Session)
        {
            this.Session = Session;
            int JuryResponseTime = Convert.ToInt32(RPDataHandler.GetData("juryresponsetime", "time"));
            int time = JuryResponseTime;
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
                    Session.SendWhisperBubble("The trial will begin within " + minutesRemaining + " minutes!", 5);
                    timer.Change(60000, Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute
                Session.SendWhisperBubble("The trial has begun, you will be sent to the court room shortly!", 5);
                RoleplayManager.CourtTrialBegining = true;
                RoleplayManager.JuryArrived = true;
                CourtManager.Running = true;
                CourtManager.MainTimer = new CourtLoop();
                RoleplayManager.CourtJuryTime = 0;
                lock (Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUsers())
                {
                    foreach (RoomUser user in Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUsers())
                    {
                        if (user == null)
                            continue;
                        if (user.IsBot)
                            continue;
                        if (user.GetClient() == null)
                            continue;

                        user.GetClient().SendWhisperBubble("The Jury Request Timer has now passed, the trial is ready to begin!", 5);
                        
                    }
                }

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
