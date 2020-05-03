using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Catalogs;
using Plus.HabboHotel.Misc;

namespace Plus.HabboHotel.Roleplay.Timers
{
        public class juryTimer
        {
            Timer timer;
            GameClient Session;
            int timeLeft; // 10 minutes (milliseconds)

            public juryTimer(GameClient Session)
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
                        Session.SendWhisperBubble("You have " + minutesRemaining + " minutes to respond to jury duty", 5);
                        timer.Change(60000, Timeout.Infinite);
                        return;
                    }

                    #endregion

                    #region Execute
                    Session.SendWhisperBubble("You are too late for Jury Duty now, sorry!", 5);
                    Session.GetRoleplay().jurySent = false;
                    RoleplayManager.JuryArrived = true;

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
