using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Misc;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class healTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // 10 seconds (milliseconds)

        public healTimer(GameClient Session)
        {
            this.Session = Session;

            int time = 1;
            timeLeft = time * 200;

            startTimer();
        }

        public void startTimer()
        {
            TimerCallback timerFinished = timerDone;

            timer = new Timer(timerFinished, null, 200, Timeout.Infinite);
        }

        public void timerDone(object info)
        {
            try
            {
                timeLeft -= 200;

                #region Conditions
                if (Session == null)
                { stopTimer(); return; }

                if (timeLeft > 0)
                {
                    timer.Change(200, Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute
                Random Rand = new Random();
                int plushealthmin = Convert.ToInt32(RPDataHandler.GetData("rp_healing", "min_hp"));
                int plushealthmax = Convert.ToInt32(RPDataHandler.GetData("rp_healing", "max_hp"));
                int PlusHealth = Rand.Next(plushealthmin, plushealthmax);

                if (Session.GetRoleplay().CurHealth + PlusHealth >= Session.GetRoleplay().MaxHealth)
                {
                    Session.GetRoleplay().CurHealth = Session.GetRoleplay().MaxHealth;
                    Session.GetRoleplay().SaveQuickStat("curhealth", "" + Session.GetRoleplay().CurHealth);
                    Session.SendWhisper("You are fully healed!");
                    Session.GetRoleplay().BeingHealed = false;
                    Session.GetRoleplay().inhosp = false;
                }
                else
                {
                    Session.GetRoleplay().CurHealth += PlusHealth;
                    Session.GetRoleplay().SaveQuickStat("curhealth", "" + Session.GetRoleplay().CurHealth);
                }
                if (Session.GetRoleplay().CurHealth != Session.GetRoleplay().MaxHealth && Session.GetRoleplay().BeingHealed)
                {
                    Session.GetRoleplay().healTimer = new healTimer(Session);
                }
                stopTimer();
                #endregion
            }
            catch { stopTimer(); }
        }

        public int getTime()
        {
            int minutesRemaining = timeLeft / 200;
            return minutesRemaining;
        }

        public void stopTimer()
        {
            timer.Dispose();
        }
    }
}
