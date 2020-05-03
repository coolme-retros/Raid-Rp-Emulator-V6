using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.Roleplay.Jobs;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class LoanTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // Based on whoever senthome the user (milliseconds)

        public LoanTimer(GameClient Session)
        {
            this.Session = Session;

            int time = Session.GetRoleplay().LoandTimer;
            timeLeft = time * 3600000 ;
             
            startTimer();
        }

        public void startTimer()
        {
            TimerCallback timerFinished = timerDone;

            timer = new Timer(timerFinished, null, 3600000 , Timeout.Infinite);
        }

        public void timerDone(object info)
        {
            try
            {
                timeLeft -= 3600000 ;

                #region Conditions
                if (Session == null)
                { stopTimer(); return; }

                if (timeLeft > 0)
                {
                	int IntelBonusPay = 0;
                    int GovPay = 0;
                    double IntelMath = Session.GetRoleplay().Loan / 24 * 5;
                    double GovPayMath = Session.GetRoleplay().Loan * .23;
                        	IntelBonusPay = (int)Math.Round(IntelMath, 0);
                    GovPay = (int)Math.Round(GovPayMath, 0);
                    int minutesRemaining = timeLeft / 3600000 ;
                    int lol1 = new Random().Next(1, 50);
                    LevelHandler.AddEXP(Session, lol1);
                    Session.GetRoleplay().LoandTimer = minutesRemaining;
                    Session.GetRoleplay().SaveQuickStat("loan_timer", "" + Session.GetRoleplay().LoandTimer);
                    Session.SendWhisper("You have " + minutesRemaining + " hour(s) left until you pay off your loan/debt!");
                    Session.GetRoleplay().Bank -= IntelBonusPay;
                    Session.GetRoleplay().SaveQuickStat("bank", "" + (Session.GetRoleplay().Bank - IntelBonusPay));
                    Session.SendWhisper("Remember you owe $" + IntelBonusPay + "'s to the bank every hour!");
                    JobManager.JobData[4].Balance += IntelBonusPay;
                    JobManager.JobData[6].Balance += GovPay;
                    RoleplayManager.SaveCorpBalance(4);
                    RoleplayManager.SaveCorpBalance(6);
                    timer.Change(3600000 , Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute

                Session.SendWhisper("You have successfully paid off your debt/loan good job!");
                Session.GetRoleplay().LoandTimer = 0;
                int lol = new Random().Next(1, 100);
                Session.GetRoleplay().Bank = +lol;
                LevelHandler.AddEXP(Session, lol);
                Session.GetRoleplay().SaveQuickStat("loan_timer", "" + Session.GetRoleplay().SendHomeTimer);
                Session.GetRoleplay().Loaned = false;
                Session.GetRoleplay().Loan = 0;

                stopTimer();
                #endregion
            }
            catch { stopTimer(); }
        }

        public int getTime()
        {
            int minutesRemaining = timeLeft / 3600000 ;
            return minutesRemaining;
        }

        public void stopTimer()
        {
            timer.Dispose();
        }
    }
}
