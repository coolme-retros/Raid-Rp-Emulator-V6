using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Roleplay.Jobs;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.Rooms;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class driveTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // Depends on type of taxi (milliseconds)

        public driveTimer(GameClient Session)
        {
            this.Session = Session;

            int time = Session.GetRoleplay().RequestedTaxi_WaitTime;
            timeLeft = time * 1000;

            startTimer();
        }

        public void startTimer()
        {
            TimerCallback timerFinished = timerDone;

            timer = new Timer(timerFinished, null, 1000, Timeout.Infinite);
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
                    int secondsRemaining = timeLeft / 1000;
                    timer.Change(1000, Timeout.Infinite);
                    return;
                }

                if (!Session.GetHabbo().GetRoomUser().CanWalk)
                {
                    Session.SendWhisper("The police stopped your car in the middle of driving!");
                    stopTimer();
                    return;
                }

                #endregion

                #region Execute
                /*Room Room = null;
                Room = RoleplayManager.GenerateRoom(RoomId);
                RoomName = Room.RoomData.Name;*/
                int SalesTax = 0;
                int IncomeTax = 0;
                int TotalPrice = 0;
                double IncomeTaxMath = TaxiManager.GetTaxiPrice() * 0.13;
                double SalesTaxMath = TaxiManager.GetTaxiPrice() * 0.20;
                IncomeTax = (int)Math.Round(IncomeTaxMath, 0);
                SalesTax = (int)Math.Round(SalesTaxMath, 0);
                //  TotalPrice = Price - IncomeTax;
                if (Session.GetRoleplay().pdriving && Session.GetRoleplay().Working)
                {
                    Session.Shout("*Hops in their partners Police Car, as they see them pull up*");
                    //Session.GetRoleplay().Driving = false;
                }
                if (Session.GetRoleplay().sdriving && !Session.GetRoleplay().Working)
                {
                    Session.Shout("*As the staff vehicle approaches they hop inside and gets transported to their destination*");

                }

                if (Session.GetRoleplay().Driving)
                {
                    Session.Shout("*As they make it to their destination, they park their car out*");
                    Session.GetHabbo().Credits -= TaxiManager.GetTaxiPrice() + SalesTax;
                    JobManager.JobData[6].Balance += SalesTax + IncomeTax;
                    JobManager.JobData[38].Balance += TaxiManager.GetTaxiPrice() - IncomeTax;
                    RoleplayManager.SaveCorpBalance(6);
                    RoleplayManager.SaveCorpBalance(38);
                    Session.GetHabbo().UpdateCreditsBalance();
                    JobManager.init();
                }

                Session.GetMessageHandler().PrepareRoomForUser(Session.GetRoleplay().RequestedTaxiDestination.RoomId, "");
                Session.GetRoleplay().RequestedTaxi_Arrived = true;
                Session.GetRoleplay().HideTaxiMsg = false;
                //Session.GetRoleplay().Driving = false;
                //Session.GetRoleplay().pdriving = false;

                stopTimer();
                #endregion
            }
            catch { stopTimer(); }
        }

        public int getTime()
        {
            int secondsRemaining = timeLeft / 1000;
            return secondsRemaining;
        }

        public void stopTimer()
        {
            timer.Dispose();
        }
    }
}