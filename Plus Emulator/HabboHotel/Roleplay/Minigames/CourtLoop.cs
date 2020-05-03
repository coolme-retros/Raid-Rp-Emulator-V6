using System;
using System.Threading;

using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Roleplay.Misc;

namespace Plus.HabboHotel.Roleplay.Minigames.Purge
{
    /// <summary>
    /// This will do a countdown before the match starts
    /// </summary>
    public class CourtLoop
    {
        /// <summary>
        /// Timer for our operation
        /// </summary>
        private Timer Timer;

        public bool On = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CourtLoop()
        {
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

                if (CourtManager.Running)
                {

                    RoleplayManager.CourtJuryTime++;
                    Timer.Change(1000, Timeout.Infinite);
                }
                else
                {
                    RoleplayManager.CourtJuryTime = 0;
                    return;
                }
            }
            catch { }
        }
    }
}
