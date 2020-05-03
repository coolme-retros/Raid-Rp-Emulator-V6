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
using Plus.Database.Manager.Database.Session_Details.Interfaces;

namespace Plus.HabboHotel.Roleplay.Timers
{
    public class workingTimer
    {
        Timer timer;
        GameClient Session;
        int timeLeft; // Based on Job (milliseconds)

        public workingTimer(GameClient Session)
        {
            this.Session = Session;

            int JobId = Session.GetRoleplay().JobId;
            int JobRank = Session.GetRoleplay().JobRank;
            int time = JobManager.JobRankData[JobId, JobRank].PayTime;
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
                    int secondsRemaining = 60000 / 1000;
                    int lol = new Random().Next(1, 50);
                    LevelHandler.AddEXP(Session, lol);
                    Session.SendWhisper("You have " + minutesRemaining + " minute(s) and " + secondsRemaining + " seconds left until you receive your next paycheck!");
                    Session.GetRoleplay().SaveStatusComponents("working");
                    timer.Change(60000, Timeout.Infinite);
                    return;
                }

                #endregion

                #region Execute
                //tofix:ying
               
                int JobId = Session.GetRoleplay().JobId;
                int JobRank = Session.GetRoleplay().JobRank;
                if (!JobManager.validJob(JobId,JobRank))
                    return;

                int pay = JobManager.JobRankData[JobId, JobRank].Pay;
                //int balance = JobManager.JobData[JobId].Balance;

                #region Calculate Bonus Pay
                double IntelMath = Session.GetRoleplay().Intelligence / 5;
                double ArrestMath = Session.GetRoleplay().Arrests / 25;
                double LevelMath = Session.GetRoleplay().CurrentLevel / 1;
                
                int ToBePaid = 0;
                int BonusPay = 0;
                int ClassBonusPay = 0;
                int IntelBonusPay = 0;
                int ArrestBonusPay = 0;
                int LevelBonusPay = 0;
                
                LevelBonusPay = (int)Math.Round(LevelMath, 0);
                IntelBonusPay = (int)Math.Round(IntelMath, 0);
                ArrestBonusPay = (int)Math.Round(ArrestMath, 0);
                int IncomeTax = 0;
                double IncomeTaxMath = JobManager.JobRankData[JobId, JobRank].Pay + LevelBonusPay + IntelBonusPay + ArrestBonusPay;
                double IncomeTaxMathTotal = IncomeTaxMath * 0.30;
                IncomeTax = (int)Math.Round(IncomeTaxMathTotal, 0);
                // Bonus +$5 pay for being an entrepreneur
                if (Session.GetRoleplay().ClassChoice == Convert.ToString("Entrepreneur"))
                {
                    ClassBonusPay = 5;
                    Session.SendWhisper("You received a bonus of $5 due to being an Entrepreneur!");
                }
                // Bonus +$1 pay for every 5 intelligence
                if (!JobManager.JobRankData[Session.GetRoleplay().JobId, Session.GetRoleplay().JobRank].hasRights("police") && !JobManager.JobRankData[Session.GetRoleplay().JobId, Session.GetRoleplay().JobRank].hasRights("swat"))
                {
                    BonusPay = IntelBonusPay;
                    Session.SendWhisper("You received a bonus of $" + BonusPay + " due to your " + Session.GetRoleplay().Intelligence + " intelligence level.");
                }
                // Bonus +$1 pay to police/swat for every 25 arrests
                if (JobManager.JobRankData[Session.GetRoleplay().JobId, Session.GetRoleplay().JobRank].hasRights("police") || JobManager.JobRankData[Session.GetRoleplay().JobId, Session.GetRoleplay().JobRank].hasRights("swat"))
                {
                    BonusPay = ArrestBonusPay;
                    Session.SendWhisper("You received a bonus of $" + BonusPay + " due to your " + Session.GetRoleplay().Arrests + " total arrests.");
                }
                if (Session.GetRoleplay().JobId > 1)
                {
                    BonusPay = LevelBonusPay;
                    Session.SendWhisper("You received a bonus of $" + BonusPay + " due to your RP level of " + Session.GetRoleplay().CurrentLevel + "");
                }
                Session.SendWhisper("A total of $" + IncomeTax + " has been taken out of your paycheck for income tax");


                #endregion

                if (JobManager.JobData[JobId].Balance > JobManager.JobRankData[JobId, JobRank].Pay)
                {
                    ToBePaid = JobManager.JobRankData[JobId, JobRank].Pay + BonusPay + ClassBonusPay - IncomeTax;
                    Session.GetRoleplay().Bank += ToBePaid;
                    Session.GetRoleplay().SaveQuickStat("bank", "" + Session.GetRoleplay().Bank);
                    Session.GetRoleplay().Shifts++;
                    Session.GetRoleplay().SaveQuickStat("shifts_completed", "" + Session.GetRoleplay().Shifts);
                    RoleplayManager.Shout(Session, "*Receives their paycheck, and begins another working shift*", 1);
                    Session.GetRoleplay().SaveStatusComponents("working");
                    int amnt = new Random().Next(1, 100);
                    LevelHandler.AddEXP(Session, amnt);
                    Session.SendWhisper("You received your paycheck of $" + JobManager.JobRankData[JobId, JobRank].Pay + ". It has automatically been deposited into your bank account.");
                    Session.GetRoleplay().SaveJobComponents();
                    JobManager.JobData[JobId].Balance -= JobManager.JobRankData[JobId, JobRank].Pay;
                    JobManager.JobData[6].Balance += IncomeTax;
                    //Take the ba;ance out of the current corp
                    RoleplayManager.SaveCorpBalance(JobId);
                    RoleplayManager.SaveCorpBalance(6);
                    JobManager.init();
                }
                else
                {
                    Session.SendWhisperBubble("Sorry, the current corp you are apart of has ran out of funds therefore not being able to pay you. Sorry about that", 1);
                }
                
                // Restart Timer
                Session.GetRoleplay().workingTimer = new workingTimer(Session);
                #endregion
            }
            catch { stopTimer(); }
        }

        public int getTime()
        {
            int minutesRemaining = timeLeft / 60000;
            return minutesRemaining;
        }
        public int getSeconds()
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
