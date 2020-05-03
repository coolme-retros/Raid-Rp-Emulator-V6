/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Roleplay.Combat;
using Plus.HabboHotel.Roleplay.

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class gunreloadtimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // 10 minutes (milliseconds)

        public gunreloadtimer(GameClient Session)
        {
            this.Session = Session;

            int time = + WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].wep_reload_time;
            timeLeft = time * 1000;

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
                timeLeft -= 1000;

                #region Conditions
                if (Session == null)
                { stopTimer(); return; }

                if (timeLeft > 0)
                {
                    int minutesRemaining = timeLeft / 60000;

                    
                    Session.SendWhisper("You have " + minutesRemaining + " minutes left until your force field is depleted.");
                    timer.Change(60000, Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute


                RoleplayManager.Shout(Session, "*Yor gun has successfully finished reloading*");
                Session.GetHabbo().GetRoomUser().IsGunReloaded

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
*/