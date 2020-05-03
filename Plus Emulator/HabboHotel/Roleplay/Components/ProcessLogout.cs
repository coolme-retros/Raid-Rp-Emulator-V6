using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Rooms.RoomInvokedItems;
using Plus.HabboHotel.Users;
using Plus.HabboHotel.PathFinding;
using Plus.HabboHotel.RoomBots;
using Plus.HabboHotel.Pets;
using Plus.Messages;
using System.Drawing;
using Plus.Util;
using System.Threading;
using Plus.HabboHotel.Rooms.Games;
using Plus.HabboHotel.Groups;
using Plus.HabboHotel.Roleplay;
using System.Net;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.Roleplay.Jobs;

namespace Plus.HabboHotel.Roleplay.Components
{
    /// <summary>
    /// This will do a countdown before the match starts
    /// </summary>
    public class ProcessLogout
    {
        /// <summary>
        /// Timer for our operation
        /// </summary>
        private Timer Timer;
        public Room Room;
        public bool On = false;
        public GameClient Session;
        public int LogoutSeconds;
        public int LogoutSecondsElapsed;
        public bool LoggedOut;
        public bool SaidLogout;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessLogout(GameClient Session)
        {
            if (Session.LoggingOut)
                return;

            this.Session = Session;
            Session.LoggingOut = true;
            this.LogoutSeconds = Convert.ToInt32(RoleplayData.Data["roleplay.logout.seconds"]);
            this.LogoutSecondsElapsed = 0;
            this.LoggedOut = false;
            this.SaidLogout = false;

            // Method to call when completed
            TimerCallback TimerCallback = Ticked;

            // Create a new instance of timer
            Timer = new Timer(TimerCallback, null, 1000, Timeout.Infinite);
        }

        /// <summary>
        /// Method is call when timer is finished
        /// </summary>
        /// <param name="info">The information</param>
        public void Ticked(object info)
        {
            try
            {
                if (Session == null)
                {
                    Logout(Session);
                    stopTimer("");
                    return;
                }

                if (Session.GetHabbo() == null)
                {
                    Logout(Session);
                    stopTimer("");
                    return;
                }

                if (!LoggedOut)
                {
                    if (SaidLogout)
                    {
                        if (LogoutSecondsElapsed < LogoutSeconds)
                        {
                            LogoutSecondsElapsed++;
                        }
                        else
                        {
                            LoggedOut = true;
                        }
                    }
                    else
                    {
                        if (Session != null)
                        {
                            if (Session.GetHabbo() != null)
                            {
                                if (Session.GetHabbo().CurrentRoom != null)
                                {
                                    Session.GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(13);
                                    Session.Shout("*Disconnecting in " + LogoutSeconds + " seconds*");
                                    #region Working Turn Offduty
                                    if (Session.GetRoleplay().Working)
                                    {
                                        Session.GetRoleplay().StopWork(false);
                                    }
                                    #endregion
                                    #region If a court is going on
                                    if (Session == RoleplayManager.Prosecutor || Session == RoleplayManager.Defendant)
                                    {
                                        RoleplayManager.Defendant = null;
                                        RoleplayManager.Prosecutor = null;
                                        RoleplayManager.CourtTrialIsStarting = false;
                                        RoleplayManager.CourtTrialStarted = false;
                                        RoleplayManager.CourtTrialBegining = false;
                                        RoleplayManager.CourtCase = null;
                                        RoleplayManager.Case = false;
                                        RoleplayManager.BeingViewed = false;
                                        RoleplayManager.CaseRequest = false;
                                        RoleplayManager.JuryArrived = false;
                                        RoleplayManager.InvitedUsersToJuryDuty.Clear();
                                        RoleplayManager.GuiltyVotes = 0;
                                        RoleplayManager.InnocentVotes = 0;
                                        RoleplayManager.RequestingVotes = false;
                                        RoleplayManager.ReadyToView = false;
                                        Session.GetRoleplay().juryArrivedTimer.stopTimer();
                                        lock (Plus.GetGame().GetClientManager().Clients.Values)
                                        {
                                            foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                                            {
                                                if (mClient == null)
                                                    continue;
                                                if (mClient.GetHabbo().GetRoomUser().IsBot)
                                                    continue;
                                                if (!RoleplayManager.OGInvitedUsers.Contains(mClient))
                                                    continue;

                                                mClient.SendNotif("It appears that <b>" + Session.GetHabbo().UserName + "</b> has gone offline\n\nThe case has been abandoned!");
                                                //user.GetClient().GetMessageHandler().PrepareRoomForUser(1, null);
                                                //mClient.GetRoleplay().juryArrivedTimer.stopTimer();
                                                mClient.GetRoleplay().juryTimer.stopTimer();
                                                RoleplayManager.OGInvitedUsers.Clear();
                                               // if (user.GetClient().GetRoleplay().)
                                                //user.GetClient().GetRoleplay().juryJudgeTimer.stopTimer();
                                                RoleplayManager.GiveMoney(Session, -50);
                                            }
                                        }
                                    }
                                    #endregion
                                    #region If they logout on a loan
                                    int amnt = new Random().Next(1, 100);
                                    
                                    if (Session.GetRoleplay().Loan > 0)
                                    {
                                        Session.GetRoleplay().Bank -= amnt;
                                        JobManager.JobData[4].Balance += amnt;
                                        JobManager.JobData[6].Balance += amnt;
                                        RoleplayManager.SaveCorpBalance(4);
                                        RoleplayManager.SaveCorpBalance(6);

                                        Session.GetRoleplay().SaveQuickStat("bank", "" + Session.GetRoleplay().Bank);
                                    }
                                    #endregion


                                }
                            }
                        }
                        SaidLogout = true;
                    }
                }
                else
                {
                    Logout(Session);
                    stopTimer("");
                    return;
                }

                Timer.Change(1000, Timeout.Infinite);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "|" + e.StackTrace);
            }
        }

        public void Logout(GameClient client)
        {
            try
            {
                if (client != null)
                {
                    client.LoggingOut = false;
                    client.Stop();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "|" + e.StackTrace);
            }
        }

        public void stopTimer(string error)
        {
            Session = null;
            Timer.Dispose();
            return;
        }

    }
}
