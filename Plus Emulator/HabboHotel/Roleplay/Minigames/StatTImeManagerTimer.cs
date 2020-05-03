using System;
using System.Threading;

using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.HabboHotel.Roleplay.Minigames.Purge
{
    /// <summary>
    /// This will do a countdown before the match starts
    /// </summary>
    public class StatTimeManagerTimer
    {
        /// <summary>
        /// Timer for our operation
        /// </summary>
        private Timer Timer;

        public bool On = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public StatTimeManagerTimer()
        {
            // Method to call when completed
            TimerCallback TimerCallback = Ticked;

            // Create a new instance of timer
            Timer = new Timer(TimerCallback, null, 30000, Timeout.Infinite);
        }

        /// <summary>
        /// Method is call when timer is finished
        /// </summary>
        /// <param name="info">The information</param>
        public void Ticked(object info)
        {
            try
            {

                if (PurgeManager.Running)
                {
                        foreach (GameClient client in Plus.GetGame().GetClientManager().Clients.Values)
                        {
                            try
                            {
                                if (client == null)
                                {
                                    continue;
                                }

                                client.SendWhisper("[Automatic Maintenance Alert]: The hotel is currently in active maintenance to keep the hotel up to date stats will be saved automatically.");
                                

                            }
                            catch (Exception e)
                            {

                            }
                        }           

                    Timer.Change(30000, Timeout.Infinite);
                }
                else
                {

                    return;
                }
            }
            catch { }
        }
    }
}
