using System;
using System.Collections;
using System.Linq;
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
using Plus.HabboHotel.Roleplay.Combat;
using System.Text;
using System.Threading.Tasks;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Rooms.RoomInvokedItems;
using Plus.HabboHotel.Users;
using Plus.HabboHotel.PathFinding;
using Plus.HabboHotel.Roleplay.Combat;
using Plus.HabboHotel.RoomBots;
using Plus.HabboHotel.Pets;
using Plus.Messages;
using System.Drawing;
using Plus.Util;
using System.Threading;
using Plus.HabboHotel.Roleplay.Minigames.Colour_Wars;
using Plus.HabboHotel.Groups;
using Plus.HabboHotel.Roleplay.Misc;
using Plus.HabboHotel.Roleplay.Minigames;
using Plus.HabboHotel.Roleplay.Jobs;
using Plus.HabboHotel.Roleplay.Combat.WeaponExtras;
using System.Net;
using Plus.Database.Manager.Database.Session_Details.Interfaces;
using Plus.Messages.Parsers;
using Plus.HabboHotel.Roleplay.Timers;
using System.Collections.Concurrent;
using Plus.HabboHotel.Misc;

namespace Plus.HabboHotel.Roleplay.Instance
{
    public class RoleplayInstance : IDisposable
    {

        #region Variables
        private uint mUserId;
        private int mCurHealth;
        private int mMaxHealth;

        internal bool InMafiaWars = false;
        internal string TeamString = String.Empty;
        internal bool MKnockedOut = false;

        internal bool IsBoxing = false;
        internal Room BoxingRoom;


        /// <summary>
        /// Is the user using the pet
        /// </summary>
        /// 
        internal Dictionary<string, uint> mMyPets;
        internal string LastPetName = null;
        internal bool PetArrested;
        internal int PetArresttimeLeft;
        internal bool PetDead;
        internal int PetDeadtimeLeft;
        internal bool UsingPet = false;
        internal uint mPetID = 0;
        private int mEnergy;
        private int mBankAccount;
        private int mBankCard;
        private int mBankNumber;
        private int mHunger;
        private int mHygiene;
        private int mDeaths;
        private int mKills;
        private int mPunches;
        private int mArrested;
        private int mArrests;
        private int mWanted;
        private int mslotrobbery;
        private bool mDead;
        private bool mJailed;
        private bool mArmored;
        private int mDeadTimer;
        private int mJailTimer;
        private int mLastX;
        private int mLastY;
        private int unSavedStrength;
        private double mLastZ;
        private GameClient mClient;
        private bool mIsNoob;
        private bool mNoobWarned;
        private int mStamina;
        private int mConstitution;
        private int mStrength;
        private int mWeed;
        private int mCarrots;
        private bool mSentHome;
        private int mSendHomeTimer;
        private int mLoandTimer;
        private int mwantedTimer;
        private int mIntelligence;
        private bool mCheckingMultiCooldown;
        private int mCoolDown;
        private int mUpdateStats;
        private int mJobId;
        private int mJobRank;
        private int mGangId;
        private int mGangRank;
        private int mPhone;
        private int mPhone_Credit;
        private int mHasWelfare;
        private bool mLoaned;
        private int mLoan;
        private int mBullets;
        private int mVests;
        private int mArmor;
        private ConcurrentDictionary<string, HabboHotel.Roleplay.Combat.Weapon> mWeapons;
        private int mWorkoutTimer_Done;
        private int mWorkoutTimer_ToDo;
        private int mWeightLiftTimer_Done;
        private int mWeightLiftTimer_ToDo;
        private bool mRobbery;
        private bool mATMRobbery;
        private bool msjailed;
        private bool mSlotRobbery;
        private bool mLearning;
        private bool mForceFieldMode;
        private bool mNPA;
        private int mPlane;
        private int mFuel;
        private int mCar;
        private int mBombs;
        private int mMarried_To;
        private int mCrowbar;
        private int mMeleeKills;
        private int mPunchKills;
        private int mGunKills;
        private int mBombKills;
        private int mSpaceXP;
        private int mspacelevel;
        private int mWoodXP;
        private int mGunLicense;
        private int mWoodLevel;
        private int mFarmingXP;
        private int mCurrentXP;
        private int mCurrentLevel;
        private int mFarmingLevel;
        private int mColorWarsPts;
        private int mMafiaWarsPts;
        private string mLastKilled;
        private string mClassChoice;
        private bool mHungerDecrement;
        private bool mHygieneDecrement;
        private List<uint> mBlockedTexters;
        private int mShiftsCompleted;
        private int mGas;
        private bool boolStrBoost = false;
        private int mGym;
        private int mTotalBill;
        private int mgym_np;
        private bool mwas_trialed;
        private string mlast_coords;
        private DateTime mjailedat;
        #region CoolDowns
        private int mVaultCoolDown;
        private int mATMRobCoolDown;
        private int mCheckVaultCoolDown;
        private int mFlashCoolDown;
        private int mWeedCoolDown;
        private int mWorkCoolDown;
        private int mSlotRobCoolDown;
        private int mUserRobCoolDown;
       #endregion
        #region Weapon Inv Attatchements
        private bool mFMJEq = false;
        private int mFMJInv = 0;
        private bool mRDSEq = false;
        private int mRDSInv = 0;
        private int mFastMagInv = 0;
        private bool mFastMagEq = false;
        #endregion
        public nukeTimer npaTimer;
        public welfaresignup welfaresignup;
        public bankRobTimer bankRobTimer;
        #region Court Shit
        public juryTimer juryTimer;
        public juryVoteTimer juryVoteTimer;
        public juryJudgeTimer juryJudgeTimer;
        public juryArrivedTimer juryArrivedTimer;
        #endregion
        public LoanTimer LoanTimer;
        public ATMRobTimer ATMRobTimer;
        public wantedTimer wantedTimer;
        public SlotRobTimer SlotRobTimer;
        public learningTimer learningTimer;
        public fftimer fftimer;
        public workingTimer workingTimer;
        public sendHomeTimer sendHomeTimer;
        public taxiTimer taxiTimer;
        public weedTimer weedTimer;
        public gangCaptureTimer gangCaptureTimer;
        public healTimer healTimer;
        public mediTimer mediTimer;
        public broteinTimer broteinTimer;
        public massageTimer massageTimer;
        public relaxTimer relaxTimer;
        public hungerTimer hungerTimer;
        public hygieneTimer hygieneTimer;
        public CoolDownTimer CoolDownTimer;
        public UNHANDLEDTIMERS UNHANDLEDTIMERS;
        public planeTimer planeTimer;
       // public signupwelfaretimer WelfareTimer;



        //Unfinished
        public workoutTimer workoutTimer;
        public weightliftTimer weightliftTimer;

        internal RoomItem Bag;
        internal Timer BagTimer;

        public FreezeRay FreezeRay = null;
        public FrozenTimer FrozenTimer = null;
        public bool UsingFreezeRay = false;
        public bool RayFrozen = false;
        public int RayFrozenSeconds = 0;
        public bool CardInsert = false;
        public int ATMTime = 0;
        public int WithDrawCoolDown = 0;
        public int DepositCoolDown = 0;
        public bool RobVaultCheck = false;
        public int UpdateCoordsTime = 0;
        public bool Crafting = false;
        public int CraftTimer = 0;
        public int CraftSeconds = 0;
        public bool CraftingPhone = false;
        public bool CraftingWeapon = false;
        public bool MottoUpdate = false;

        public bool BannedFromVIPAlert;
        internal void BagTimerDone(object StateInfo)
        {
            if (Bag != null && Client != null)
            {
                Random Randomer = new Random();
                int EnergyToAdd = Randomer.Next(3, 5);

                if (Energy + EnergyToAdd < 100)
                {
                    Energy += EnergyToAdd;
                    BagTimer.Change(20000, Timeout.Infinite);
                    Client.SendWhisper("You have gained +" + EnergyToAdd + " energy!");
                }
                else
                {
                    Energy = 100;
                    Client.SendWhisper("Your energy is full!");
                }
            }
            else
            {

            }
        }

        #region Offering
        public Dictionary<string, Offer> OfferData = new Dictionary<string, Offer>();
        public string WeaponOfferedSell = null;
        public GameClient WeaponOfferedSell_By = null;
        public bool WeaponOffered = false;
        public int WeaponOfferedPrice = 0;
        #endregion

        #region Working
        public bool Working = false;
        public bool WorkInvalidRoomReminder = false;
        public string FigBeforeWork = null;
        public string FigWork = null;
        public string MottBeforeWork = null;
        public bool Cuffed = false;

        #endregion
        #region Gang
        public int GangInvitedTo = 0;
        public bool GangInvited = false;
        public bool GangCapturing = false;
        public int CheckTurfSpot = 0;
        #endregion

        #region Other Misc
        public bool RadioOff;
        public int GatheringSeconds = 0;
        public bool Gathering = false;
        public bool Debug_Furni = false;
        public bool DisabledTexts = false;
        public int WorkoutSeconds = 0;
        public bool WorkingOut = false;
        public int WeightLiftSeconds = 0;
        public bool WeightLifting = false;
        public int L_MessageWarn = 0;
        public int L_MessageTimer = 0;
        public string L_Message4 = null;
        public string L_Message3 = null;
        public string L_Message2 = null;
        public string L_Message1 = null;
        public bool SpamMuted = false;
        public bool StaffMuted = false;
        public int MuteSeconds = 0;
        public int EffectSeconds = 0;
        public int StunnedSeconds = 0;
        public int Cuffedseconds = 0;
        public bool BulletVest = false;
        public string ActionLast = null;
        public GameClient LastHit = null;
        public RoomUser LastHitBot = null;
        public int UsingWeed_Bonus = 0;
        public bool UsingWeed = false;
        public bool DebugStacking = false;
        public double DebugStack = 0;
        public bool DebugMultiFurni = false;
        public int DebugMFurni = 0;
        public int GunShots = 0;
        public string Equiped2 = null;
        public string Equiped = null;
        public Dictionary<string, int> MultiCoolDown = new Dictionary<string, int>();
        public string FigBeforeSpecial = null;
        public double UpdateCount = 0;
        public string MottBeforeSpecial = null;
        public bool BeingHealed = false;
        public bool UsingMedkit = false;
        public bool BeingMassaged = false;
        public bool Relaxing = false;
        public bool inATM = false;
        public bool inSlotMachine = false;
        public bool RigJackpot = false;
        public bool usingPlane;
        public bool loadedPoison;
        public int planeUsing;
        public int robbingStore;
        public bool usingCar = false;
        public bool StaffDuty = false;
        public bool InShower = false;
        public int ShowerSeconds = 0;
        public RoomItem Shower = null;
        public bool OnMine = false;
        #endregion
        #region Banking
        public RoomItem InteractingAtm = null;
        public int AtmSetAmount = 20;
        private int mBank = 0;
        public int WithdrawDelay = 0;
        public bool Withdraw_Via_Phone = false;
        #endregion
        #region Taxis
        public bool RequestedTaxi = false;
        public Room RequestedTaxiDestination = null;
        public bool RequestedTaxi_Arrived = false;
        public bool HideTaxiMsg = false;
        public int RequestedTaxi_WaitTime = 0;
        public bool RecentlyCalledTaxi = false;
        public int RecentlyCalledTaxi_Timeout = 0;
        public string TaxiArriveMsg = null;
        public GameClient LastTexter = null;
        #endregion
        #region Arrows
        public bool UsingArrow = false;
        #endregion
        #region Dead
        public int DeadSeconds = 0;
        public bool DeadAlerted = false;
        public bool DeadFigSet = false;
        #endregion
        #region Jailed
        public int JailedSeconds = 60;
        public bool AdminJailed = false;
        public bool JailedAlerted = false;
        public bool JailFigSet = false;
        #endregion
        #region Loaning System
        //public bool
        #endregion
        #region Armored
        public bool ArmoredFigSet = false;
        #endregion
        #region Games
        public bool NPA = false;
        internal bool inBrawl;
        public bool Brawl;

        public int RobJailed;

        public bool safefromNuke;
        #endregion
        #region Robbery/Learning
        public bool Robbery = false;
        public bool SlotRobbery = false;
        public bool ATMRobbery = false;
        public bool sjailed = false;
        public bool Learning = false;
        #endregion
        #region Hunger/Hygiene
        public bool HungerDecrement = false;
        public bool HygieneDecrement = false;
        #endregion
        #region Dragon Powerups
        public int Green_Dragon;
        public int Blue_Dragon;
        public int Silver_Dragon;
        public int EroSphere1_Dragon;
        public int EroSphere2_Dragon;
        public bool usingBlue = false;
        public bool usingSilver = false;
        public bool usingEroSphere1_Dragon = false;
        public bool usingEroSphere2_Dragon = false;
        public bool Poisoned = false;
        public bool Frozen = false;
        public int FrozenSeconds = 0;
        #endregion
        #region Super Powers
        public bool ForceFieldMode = false;
        public bool DeflectionMode = false;
        public bool FreezeMode = false;
        #endregion
        #region Minigames
        public bool InMiniGame = false;
        public bool HungerGames = false;
        public bool HungerGames_Dead = false;
        public Dictionary<string, int> HungerGames_Inventory = new Dictionary<string, int>();
        public string HungerGames_Item_Wielding = null;
        public int HungerGames_Pts = 0;
        public int HungerGames_Cash = 0;
        public int Brawl_Pts;
        public int Infection_Pts;
        public bool Infected = false;
        public bool inZombieInfection = false;
        #endregion
        #region Pet
        internal Pet MyPet1 = null;
        public bool Pet_Test_1 = false;
        public bool Pet_Test_2 = false;
        public bool Pet_Test_3 = false;
        public GameClient Assigned_Attacking = null;
        public string Pet_Activated_Stance = null;
        public uint Pet_Summoned = 0;
        public bool Pet_Attacking = false;
        public string Pet_Activated_Mode = null;
        public uint Cached_PetId = 0;
        public bool Cached_Pet = false;
        #endregion
        #region Marriage/Divorce
        public bool marryReq = false;
        public uint marryReqer;
        #endregion
        #region Colour wars
        public string LastCwTeam;
        /// <summary>
        /// Boolean to represent if the session is in colour wars
        /// </summary>
        public bool inColourWars = false;

        /// <summary>
        /// The sessions team for colour wars
        /// </summary>
        public Team ColourWarTeam;

        /// <summary>
        /// The players look before they went to war >:D
        /// </summary>
        public string figBeforeWar;

        /// <summary>
        /// Checks if player is capturing
        /// </summary>
        public bool Capturing = false;

        /// <summary>
        /// Represents if the user is knocked out
        /// </summary>
        public bool KnockedOut = false;
        #endregion
        #region Other Components
        public bool Changing = false;
        #endregion
        #region Court
        public bool jurySent = false;
        public bool partofJury = false;

        #endregion


        #endregion

        #region Constructor
        public RoleplayInstance(uint UserId, int CurHealth, int MaxHealth, int Energy, int Hunger, int Hygiene,
            int Deaths, int Kills, int Punches, int Arrested, int Arrests, bool Dead, bool Jailed, bool Armored, int DeadTimer,
            int JailTimer, int LastX, int LastY, double LastZ, int WorkTimer, bool IsNoob, int Stamina, int Constitution, int Strength,
            int Intelligence, int JobId, int JobRank, int SendHomeTimer, int GangId, int GangRank, int Phone,
            int Phone_Credit, int Bank, int Weed, ConcurrentDictionary<string, HabboHotel.Roleplay.Combat.Weapon> Weapons,
            int Bullets, int Vests, int Armor, int Wanted, int WorkoutTimer_Done, int WorkoutTimer_ToDo, int WeightLiftTimer_Done, int WeightLiftTimer_ToDo,
            bool Robbery, bool ATMRobbery, bool Learning, int Plane, int Fuel, int Car, int Green_Dragon, int Blue_Dragon,
            int Silver_Dragon, int EroSphere1_Dragon, int EroSphere2_Dragon, int Bombs, int Brawl_Pts, int Infection_Pts, int Married_To, int Crowbar,
            int MeleeKills, int PunchKills, int GunKills, int BombKills, string LastKilled,
            int woodLevel, int woodXP, int gunlicenses, int spacelevel, int spaceXP, int farmingLevel, int farmingXP, int Carrots,
            string ClassChoice, List<uint> BlockedTexters, bool VIPABanned, string LastCwTeam, int Shifts, int Gas, bool sjailed, string wanted_for,
            int HasWelfare, int currentlevel, int currentxp, int Loan, int LoandTimer, int bankaccount, int bankcard, int banknumber, int Gym, int gym_np, int TotalBill, int FMJInv, int RDSInv,
            bool Was_Trialed, string Last_Coords, int FastMagInv, int VaultCoolDown, int ATMRobCoolDown, int VaultCheckCoolDown, int FlashBangCoolDown, int WeedCoolDown, int SlotRObCoolDown, int WorkCoolDown, int UserRobCoolDown,
            Dictionary<string, uint> MyPets)
        {
            this.mUserId = UserId;
            this.mCurHealth = CurHealth;
            this.mMyPets = MyPets;
            this.mMaxHealth = MaxHealth;
            this.mEnergy = Energy;
            this.mHunger = Hunger;
            this.mHygiene = Hygiene;
            this.mDeaths = Deaths;
            this.mKills = Kills;
            this.mPunches = Punches;
            this.mArrested = Arrested;
            this.mArrests = Arrests;
            this.mDead = Dead;
            this.mJailed = Jailed;
            this.mArmored = Armored;
            this.mDeadTimer = DeadTimer;
            this.mJailTimer = JailTimer;
            this.mLastX = LastX;
            this.mLastY = LastY;
            this.mLastZ = LastZ;
            this.mIsNoob = IsNoob;
            this.mStamina = Stamina;
            this.mConstitution = Constitution;
            this.mStrength = Strength;
            this.mIntelligence = Intelligence;
            this.mJobId = JobId;
            this.mJobRank = JobRank;
            this.mSendHomeTimer = SendHomeTimer;
            this.mGangId = GangId;
            this.mGangRank = GangRank;
            this.mPhone = Phone;
            this.mPhone_Credit = Phone_Credit;
            this.mBank = Bank;
            this.mWeed = Weed;
            this.mCarrots = Carrots;
            this.mWeapons = Weapons;
            this.mBullets = Bullets;
            this.mVests = Vests;
            this.mArmor = Armor;
            this.mWanted = Wanted;
            this.mWorkoutTimer_Done = WorkoutTimer_Done;
            this.mWorkoutTimer_ToDo = WorkoutTimer_ToDo;
            this.mWeightLiftTimer_Done = WeightLiftTimer_Done;
            this.mWeightLiftTimer_ToDo = WeightLiftTimer_ToDo;
            this.mRobbery = Robbery;
            this.mATMRobbery = ATMRobbery;
            this.mLearning = Learning;
            this.mPlane = Plane;
            this.mFuel = Fuel;
            this.mCar = Car;
            this.Blue_Dragon = Blue_Dragon;
            this.Green_Dragon = Green_Dragon;
            this.Silver_Dragon = Silver_Dragon;
            this.EroSphere1_Dragon = EroSphere1_Dragon;
            this.EroSphere2_Dragon = EroSphere2_Dragon;
            this.Bombs = Bombs;
            this.Brawl_Pts = Brawl_Pts;
            this.Infection_Pts = Infection_Pts;
            this.mMarried_To = Married_To;
            this.mCrowbar = Crowbar;
            this.mMeleeKills = MeleeKills;
            this.mPunchKills = PunchKills;
            this.mGunKills = GunKills;
            this.mBombKills = BombKills;
            this.mLastKilled = LastKilled;
            this.mspacelevel = spacelevel;
            this.mSpaceXP = spaceXP;
            this.mWoodXP = woodXP;
            this.mBankAccount = bankaccount;
            //this.se
            this.mCurrentXP = currentxp;
            this.mCurrentLevel = currentlevel;
            this.mGunLicense = GunLicense;
            this.mWoodLevel = woodLevel;
            this.mFarmingLevel = farmingLevel;
            this.mFarmingXP = farmingXP;
            //
            this.mClassChoice = ClassChoice;
            this.mHungerDecrement = HungerDecrement;
            this.mHygieneDecrement = HygieneDecrement;
            this.RadioOff = false;
            this.mBlockedTexters = BlockedTexters;
            this.BannedFromVIPAlert = VIPABanned;
            this.LastCwTeam = LastCwTeam;
            this.mShiftsCompleted = Shifts;
            this.mGas = Gas;
            this.msjailed = sjailed;
            this.mwanted_for = wanted_for;
            this.mHasWelfare = HasWelfare;
            this.mLoan = Loan;
            this.mLoandTimer = LoandTimer;
            this.mBankCard = bankcard;
            this.mBankCard = bankcard;
            this.mBankNumber = banknumber;
            this.mGym = Gym;
            this.mgym_np = gym_np;
            this.mTotalBill = TotalBill;
            this.mjailedat = jailedat;
            this.mFMJInv = FMJInv;
            this.mRDSInv = RDSInv;
            this.mwas_trialed = Was_Trialed;
            this.mlast_coords = Last_Coords;
            this.mFastMagInv = FastMagInv;
            this.mVaultCoolDown = VaultCoolDown;
            this.mATMRobCoolDown = ATMRobCoolDown;
            this.mCheckVaultCoolDown = VaultCheckCoolDown;
            this.mFlashCoolDown = FlashBangCoolDown;
            this.mWorkCoolDown = WorkCoolDown;
            this.mWeedCoolDown = WeedCoolDown;
            this.mSlotRobCoolDown = SlotRObCoolDown;
            this.mUserRobCoolDown = UserRobCoolDown;
        }

        public RoleplayInstance(uint v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, int v9, int v10, int v11, bool v12, bool v13, bool v14, int v15, int v16, int v17, int v18, int v19, int v20, bool v21, int v22, int v23, int v24, int v25, int v26, int v27, int v28, int v29, int v30, int v31, int v32, int v33, int v34, ConcurrentDictionary<string, Weapon> weapons, int v35, int v36, int v37, int v38, int v39, int v40, int v41, int v42, bool v43, bool v44, bool v45, int v46, int v47, int v48, int v49, int v50, int v51, int v52, int v53, int v54, int v55, int v56, int v57, int v58, int v59, int v60, int v61, int v62, string v63, int v64, int v65, int v66, int v67, int v68, int v69, int v70, string v71, List<uint> blockedTexters, bool v72, string v73, int v74, int v75, int v76, Dictionary<string, uint> myPets)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.v5 = v5;
            this.v6 = v6;
            this.v7 = v7;
            this.v8 = v8;
            this.v9 = v9;
            this.v10 = v10;
            this.v11 = v11;
            this.v12 = v12;
            this.v13 = v13;
            this.v14 = v14;
            this.v15 = v15;
            this.v16 = v16;
            this.v17 = v17;
            this.v18 = v18;
            this.v19 = v19;
            this.v20 = v20;
            this.v21 = v21;
            this.v22 = v22;
            this.v23 = v23;
            this.v24 = v24;
            this.v25 = v25;
            this.v26 = v26;
            this.v27 = v27;
            this.v28 = v28;
            this.v29 = v29;
            this.v30 = v30;
            this.v31 = v31;
            this.v32 = v32;
            this.v33 = v33;
            this.v34 = v34;
            Weapons = weapons;
            this.v35 = v35;
            this.v36 = v36;
            this.v37 = v37;
            this.v38 = v38;
            this.v39 = v39;
            this.v40 = v40;
            this.v41 = v41;
            this.v42 = v42;
            this.v43 = v43;
            this.v44 = v44;
            this.v45 = v45;
            this.v46 = v46;
            this.v47 = v47;
            this.v48 = v48;
            this.v49 = v49;
            this.v50 = v50;
            this.v51 = v51;
            this.v52 = v52;
            this.v53 = v53;
            this.v54 = v54;
            this.v55 = v55;
            this.v56 = v56;
            this.v57 = v57;
            this.v58 = v58;
            this.v59 = v59;
            this.v60 = v60;
            this.v61 = v61;
            this.v62 = v62;
            this.v63 = v63;
            this.v64 = v64;
            this.v65 = v65;
            this.v66 = v66;
            this.v67 = v67;
            this.v68 = v68;
            this.v69 = v69;
            this.v70 = v70;
            this.v71 = v71;
            BlockedTexters = blockedTexters;
            this.v72 = v72;
            this.v73 = v73;
            this.v74 = v74;
            this.v75 = v75;
            this.v76 = v76;
            MyPets = myPets;
        }
        #endregion

        #region Getters & Setters
        public int FreezeTimeAbility
        {
            get
            {

                int str = mStrength;
                if (str <= 4)
                {
                    str = 5;
                }

                return new Random().Next(4, str);

            }
        }
        public int Married_To
        {
            get { return mMarried_To; }
            set { mMarried_To = value; }
        }

        public int WorkoutTimer_ToDo
        {
            get { return mWorkoutTimer_ToDo; }
            set { mWorkoutTimer_ToDo = value; }
        }
        public int WorkoutTimer_Done
        {
            get { return mWorkoutTimer_Done; }
            set { mWorkoutTimer_Done = value; }
        }
        public int savedSTR
        {
            get { return unSavedStrength; }
            set { unSavedStrength = value; }
        }
        public int WeightLiftTimer_ToDo
        {
            get { return mWeightLiftTimer_ToDo; }
            set { mWeightLiftTimer_ToDo = value; }
        }
        public int WeightLiftTimer_Done
        {
            get { return mWeightLiftTimer_Done; }
            set { mWeightLiftTimer_Done = value; }
        }
        public int SpaceXP
        {
            get { return mSpaceXP; }
            set { mSpaceXP = value; }
        }
        public int spacelevel
        {
            get { return mspacelevel; }
            set { mspacelevel = value; }
        }
        public int WoodXP
        {
            get { return mWoodXP; }
            set { mWoodXP = value; }
        }
        
        
        public int GunLicense
        {
            get { return mGunLicense; }
            set { mGunLicense = value; }
        }
        public int Gym
        {
            get { return mGym; }
            set { mGym = value; }
        }
        public int WoodLevel
        {
            get { return mWoodLevel; }
            set { mWoodLevel = value; }
        }
        public int FarmingXP
        {
            get { return mFarmingXP; }
            set { mFarmingXP = value; }
        }
        public int BankAccount
        {
            get { return mBankAccount; }
            set { mBankAccount = value; }
        }
        public int BankCard
        {
            get { return mBankCard; }
            set { mBankCard = value; }
        }
        public int BankNumber
        {
            get { return mBankNumber; }
            set { mBankNumber = value; }
        }
        public int gym_np
        {
            get { return mgym_np; }
            set { mgym_np = value; }
        }
        public int TotalBill
        {
            get { return mTotalBill; }
            set { mTotalBill = value; }
        }
        public int FarmingLevel
        {
            get { return mFarmingLevel; }
            set { mFarmingLevel = value; }
        }
        public int MafiaWarsPts
        {
            get { return mMafiaWarsPts; }
            set { mMafiaWarsPts = value; }
        }
        public int CurrentLevel
        {
            get { return mCurrentLevel; }
            set { mCurrentLevel = value; }
        }
        public int CurrentXP
        {
            get { return mCurrentXP; }
            set { mCurrentXP = value; }
        }
        public int ColorWarsPts
        {
            get { return mColorWarsPts; }
            set { mColorWarsPts = value; }
        }
        public int Bullets
        {
            get { return mBullets; }
            set { mBullets = value; }
        }
        public bool StrBonus
        {
            get { return boolStrBoost; }
            set { boolStrBoost = value; }
        }
        public bool Loaned
        {
            get { return mLoaned; }
            set { mLoaned = value; }
        }
        public int Vests
        {
            get { return mVests; }
            set { mVests = value; }
        }
        public int Armor
        {
            get { return mArmor; }
            set { mArmor = value; }
        }
        public int Bombs
        {
            get { return mBombs; }
            set { mBombs = value; }
        }
        public int Shifts
        {
            get { return mShiftsCompleted; }
            set { mShiftsCompleted = value; }
        }

        public string LastCoords
        {
            get { return mlast_coords;}
            set { mlast_coords = value; }
        }

        public int Gas
        {
            get { return mGas; }
            set { mGas = value; }
        }
        public Dictionary<string, uint> MyPets
        {
            get { return mMyPets; }
            set { mMyPets = value; }
        }
        public int CurHealth
        {
            get { return mCurHealth; }
            set
            {
                if (mDead)
                {
                    value = mMaxHealth;
                }

                mCurHealth = value;
            }
        }
        public int Wanted
        {
            get { return mWanted; }
            set { mWanted = value; }
        }
        public int MaxHealth
        {
            get { return mMaxHealth; }
            set { mMaxHealth = value; }
        }
        public int Energy
        {
            get { return mEnergy; }
            set { mEnergy = value; }
        }

        public int Hunger
        {
            get { return mHunger; }
            set { mHunger = value; }
        }
      /*  public int Blacklist
        {
            get { return mBlacklist; }
            set { mBlacklist = value; }
        }*/
        public int Hygiene
        {
            get { return mHygiene; }
            set { mHygiene = value; }
        }
        public int Deaths
        {
            get { return mDeaths; }
            set { mDeaths = value; }
        }
        public int Kills
        {
            get { return mKills; }
            set { mKills = value; }
        }
        public int Punches
        {
            get { return mPunches; }
            set { mPunches = value; }
        }
        public int Arrested
        {
            get { return mArrested; }
            set { mArrested = value; }
        }
        public ConcurrentDictionary<string, HabboHotel.Roleplay.Combat.Weapon> Weapons
        {
            get { return mWeapons; }
            set { mWeapons = value; }
        }
        public int Arrests
        {
            get { return mArrests; }
            set { mArrests = value; }
        }
        public int Plane
        {
            get { return mPlane; }
            set { mPlane = value; }
        }
        public int Fuel
        {
            get { return mFuel; }
            set { mFuel = value; }
        }
        
        public string wanted_for
        {
            get { return mwanted_for; }
            set { mwanted_for = value; }
        }

        public int HasWelfare
        {
            get { return mHasWelfare; }
            set { mHasWelfare = value; }
        }
        public int Car
        {
            get { return mCar; }
            set { mCar = value; }
        }
        public bool Dead
        {
            get { return mDead; }

            set
            {

                if (value == true)
                {

                    mCurHealth = mMaxHealth;
                    mHunger = 0;
                    mHygiene = 100;

                    try
                    {

                        if (!DeadFigSet)
                        {
                            ApplySpecialStatus("dead");
                            DeadFigSet = true;
                        }

                        if (!mClient.GetHabbo().CurrentRoom.RoomData.Hospital)
                        {
                            // mClient.GetMessageHandler().PrepareRoomForUser(2, "");
                        }
                        else if (mClient.GetHabbo().CurrentRoom.RoomData.Hospital) 
                        {
                            RoomUser user = mClient.GetHabbo().GetRoomUser();
                            List<RoomItem> DeadBeds = new List<RoomItem>();

                            lock (mClient.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
                            {
                                foreach (RoomItem item in mClient.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
                                {
                                    #region Insert DeathBeds
                                    if (user.GetClient().GetRoleplay().Dead)
                                    {
                                        if (item.GetBaseItem().Name.Contains("hosptl_bed"))
                                        {
                                            if (!DeadBeds.Contains(item))
                                            {
                                                DeadBeds.Add(item);
                                            }
                                        }
                                    }
                                    #endregion
                                    #region Death bed spawner
                                    if (user.GetClient().GetRoleplay().Dead)
                                    {

                                        if (item == null)
                                            continue;

                                        if (item.GetBaseItem() == null)
                                            continue;

                                        #region Spawn on Bed if Dead
                                        RoomItem LandItem = null;


                                        if (DeadBeds.Count >= 1)
                                        {
                                            if (DeadBeds.Count == 1)
                                            {
                                                LandItem = DeadBeds.First<RoomItem>();
                                            }
                                            else
                                            {
                                                LandItem = DeadBeds[new Random().Next(0, DeadBeds.Count)];
                                            }
                                            user.X = LandItem.X;
                                            user.Y = LandItem.Y;
                                            #region Set Rotation & Sit Action
                                            if ((user.RotBody % 2) == 0)
                                            {

                                                try
                                                {
                                                    user.Statusses.Add("lay", "1.0 null");
                                                    user.Z = 0.75;
                                                    user.IsLyingDown = true;
                                                    user.UpdateNeeded = true;
                                                }
                                                catch { }
                                            }
                                            else
                                            {
                                                user.RotBody--;
                                                user.Statusses.Add("lay", "1.0 null");
                                                user.Z = 0.75;
                                                user.IsLyingDown = true;
                                                user.UpdateNeeded = true;
                                            }


                                            user.RotBody = LandItem.Rot;
                                            #endregion
                                            //   user.SetPos(LandItem.GetX, LandItem.GetY, LandItem.GetZ);
                                            user.UpdateNeeded = true;
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }

                            user.UpdateNeeded = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }

                mDead = value;
            }
        }
        public bool Jailed
        {
            get { return mJailed; }

            set
            {


                if (value == true)
                {


                    try
                    {
                        if (!JailFigSet)
                        {
                            ApplySpecialStatus("jailed");
                            JailFigSet = true;
                        }

                        if (!mClient.GetHabbo().CurrentRoom.RoomData.Description.Contains("JAIL"))
                        {
                            uint jailid = Convert.ToUInt32(RPDataHandler.GetData("prison_id", "id"));
                            mClient.GetMessageHandler().PrepareRoomForUser(jailid, "");

                        }
                        else if (mClient.GetHabbo().CurrentRoom.RoomData.Description.Contains("JAIL"))
                        {
                            RoomUser user = mClient.GetHabbo().GetRoomUser();
                            List<RoomItem> JailBeds = new List<RoomItem>();

                            lock (mClient.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
                            {
                                foreach (RoomItem item in mClient.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
                                {

                                    if (item == null)
                                        continue;
                                    if (item.GetBaseItem() == null)
                                        continue;

                                    #region Insert JailBeds
                                    if (user.GetClient().GetRoleplay().Jailed)
                                    {
                                        if (item.GetBaseItem().Name.Contains("army_c15_bed"))
                                        {
                                            if (!JailBeds.Contains(item))
                                            {
                                                JailBeds.Add(item);
                                            }
                                        }
                                    }
                                    #endregion
                                    #region Jail bed spawner
                                    if (user.GetClient().GetRoleplay().Jailed)
                                    {
                                        #region Spawn on Jail Bed if Jailed
                                        RoomItem LandItem = null;



                                        if (JailBeds.Count >= 1)
                                        {
                                            if (JailBeds.Count == 1)
                                            {
                                                LandItem = JailBeds.First<RoomItem>();
                                            }
                                            else
                                            {
                                                LandItem = JailBeds[new Random().Next(0, JailBeds.Count)];
                                            }

                                            user.X = LandItem.X;
                                            user.Y = LandItem.Y;

                                            #region Set Rotation & Sit Action
                                            if ((user.RotBody % 2) == 0)
                                            {

                                                try
                                                {
                                                    user.Statusses.Add("lay", "1.0 null");
                                                    user.Z = 0.75;
                                                    user.IsLyingDown = true;
                                                    user.UpdateNeeded = true;
                                                }
                                                catch { }
                                            }
                                            else
                                            {
                                                user.RotBody--;
                                                user.Statusses.Add("lay", "1.0 null");
                                                user.Z = 0.75;
                                                user.IsLyingDown = true;
                                                user.UpdateNeeded = true;
                                            }


                                            user.RotBody = LandItem.Rot;
                                            #endregion
                                            user.SetPos(LandItem.X, LandItem.Y, LandItem.Z);
                                            user.UpdateNeeded = true;
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }

                            user.UpdateNeeded = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }

                mJailed = value;

            }
        }
        public bool Armored
        {
            get { return mArmored; }
            set
            {
                if (value == true)
                {
                    try
                    {
                        if (!ArmoredFigSet)
                        {
                            ApplySpecialStatus("armored");
                            ArmoredFigSet = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                mArmored = value;
            }
        }
        public int JailTimer
        {
            get { return mJailTimer; }
            set
            {
                if (mClient == null)
                {
                    mJailTimer = value;
                    return;
                }


                if (mClient.GetHabbo() == null)
                {
                    mJailTimer = value;
                    return;
                }

                if (mClient.GetHabbo().Look == null)
                {
                    mJailTimer = value;
                    return;
                }

                if (!JailFigSet)
                {
                    ApplySpecialStatus("jailed");
                    JailFigSet = true;
                }

                mJailTimer = value;
            }
        }
        public int DeadTimer
        {
            get { return mDeadTimer; }
            set
            {
                mCurHealth = mMaxHealth;
                mEnergy = 100;


                if (!DeadFigSet)
                {
                    ApplySpecialStatus("dead");
                    DeadFigSet = true;
                }

                mDeadTimer = value;
            }
        }
        public int LastX
        {
            get { return mLastX; }
            set { mLastX = value; }
        }
        public int LastY
        {
            get { return mLastY; }
            set { mLastY = value; }
        }
        public double LastZ
        {
            get { return mLastZ; }
            set { mLastZ = value; }
        }
        public int Bank
        {
            get { return mBank; }
            set { mBank = value; }
        }
        public GameClient Client
        {
            get { return mClient; }
            set
            {
                mClient = Misc.RoleplayManager.GenerateSession(mUserId);
            }
        }

        public int Phone
        {
            get { return mPhone; }
            set { mPhone = value; }
        }
        public int Loan
        {
            get { return mLoan; }
            set { mLoan = value; }
        }
        public int Phone_Credit
        {
            get { return mPhone_Credit; }
            set { mPhone_Credit = value; }
        }
        public bool IsNoob
        {
            get { return mIsNoob; }
            set { mIsNoob = value; }
        }
        public int Stamina
        {
            get { return mStamina; }
            set { mStamina = value; }
        }
        public int Constitution
        {
            get { return mConstitution; }
            set { mConstitution = value; }
        }
        public int Strength
        {
            get { return mStrength; }
            set { mStrength = value; }
        }
        public int SendHomeTimer
        {
            get { return mSendHomeTimer; }
            set { mSendHomeTimer = value; }
        }
        public int LoandTimer
        {
        	get { return mLoandTimer; }
            set { mLoandTimer = value; }
        }
        public int Weed
        {
            get { return mWeed; }
            set { mWeed = value; }
        }
        public DateTime jailedat
        {
            get { return mjailedat; }
            set { mjailedat = value; }
        }
        public int Carrots
        {
            get { return mCarrots; }
            set { mCarrots = value; }
        }
        public bool SentHome
        {
            get { return mSentHome; }
            set { mSentHome = value; }
        }
        public int Intelligence
        {
            get { return mIntelligence; }
            set { mIntelligence = value; }
        }
        public int FMJInv
        {
            get { return mFMJInv; }
            set { mFMJInv = value; }
        }
        public int RDSInv
        {
            get { return mRDSInv; }
            set { mRDSInv = value; }
        }
        public bool RDSEq
        {
            get { return mRDSEq; }
            set { mRDSEq = value; }
        }
        public bool FMJEq
        {
            get { return mFMJEq; }
            set { mFMJEq = value; }
        }

        public int FastMagInv
        {
            get { return mFastMagInv; }
            set { mFastMagInv = value; }
        }

        public bool FastMagEq
        {
            get { return mFastMagEq; }
            set { mFastMagEq = value; }
        }
        public bool Was_Trialed
        {
            get { return mwas_trialed; }
            set { mwas_trialed = value; }
        }

        public int VaultCoolDown
        {
            get { return mVaultCoolDown; }
            set { mVaultCoolDown = value; }
        }

        public int ATMColDown
        {
            get { return mATMRobCoolDown; }
            set { mATMRobCoolDown = value; }
        }

        public int VaultCheckCoolDown
        {
            get { return mCheckVaultCoolDown; }
            set { mCheckVaultCoolDown = value; }

        }

        public int FlashBangCoolDown
        {
            get { return mFlashCoolDown; }
            set { mFlashCoolDown = value; }
        }
        public int UserRobCoolDown
        {
            get { return mUserRobCoolDown; }
            set { mUserRobCoolDown = value; }
        }
        public int SlotRobCoolDown
        {
            get { return mSlotRobCoolDown; }
            set { mSlotRobCoolDown = value; }
        }
        public int WorkCooldown
        {
            get { return mWorkCoolDown; }
            set { mWorkCoolDown = value; }
        }
        public int WeedCoolDown
        {
            get { return mWeedCoolDown; }
            set { mWeedCoolDown = value; }
        }
        public int CoolDown
        {
            get { return mCoolDown; }
            set
            {
                if (CoolDownTimer == null)
                {
                    CoolDownTimer = new CoolDownTimer(mClient);
                }

                CheckingCoolDown = true;
                mCoolDown = value;
            }
        }
        public bool CheckingCoolDown = false;

        internal bool slotrobbery;
       // private object slotrobTimer;
        internal int CuffedSeconds;

        //internal int rruse;
        private uint v1;
        private int v2;
        private int v3;
        private int v4;
        private int v5;
        private int v6;
        private int v7;
        private int v8;
        private int v9;
        private int v10;
        private int v11;
        private bool v12;
        private bool v13;
        private bool v14;
        private int v15;
        private int v16;
        private int v17;
        private int v18;
        private int v19;
        private int v20;
        private bool v21;
        private int v22;
        private int v23;
        private int v24;
        private int v25;
        private int v26;
        private int v27;
        private int v28;
        private int v29;
        private int v30;
        private int v31;
        private int v32;
        private int v33;
        private int v34;
        private int v35;
        private int v36;
        private int v37;
        private int v38;
        private int v39;
        private int v40;
        private int v41;
        private int v42;
        private bool v43;
        private bool v44;
        private bool v45;
        private int v46;
        private int v47;
        private int v48;
        private int v49;
        private int v50;
        private int v51;
        private int v52;
        private int v53;
        private int v54;
        private int v55;
        private int v56;
        private int v57;
        private int v58;
        private int v59;
        private int v60;
        private int v61;
        private int v62;
        private string v63;
        private int v64;
        private int v65;
        private int v66;
        private int v67;
        private int v68;
        private int v69;
        private int v70;
        private string v71;
        private bool v72;
        private string v73;
        private int v74;
        private int v75;
        private int v76;
       // internal bool Blacklist;
        //internal bool sjailed;
        internal bool Driving;
        internal int Restoreuse;
        internal bool pdriving;
        public int roomreleaseuse;
        public bool sdriving;
        public bool vipdriving;
        internal bool inhosp;

        public bool CheckingMultiCooldown
        {
            get
            {
                return mCheckingMultiCooldown;
            }
            set
            {

                if (CoolDownTimer == null)
                {
                    CoolDownTimer = new CoolDownTimer(mClient);
                }

                mCheckingMultiCooldown = value;
            }

        }



        public List<RoomUser> GetMyBots()
        {
            List<RoomUser> Bots = new List<RoomUser>();

            lock (mClient.GetHabbo().CurrentRoom.GetRoomUserManager().GetBots())
            {
                foreach (RoomUser Bot in mClient.GetHabbo().CurrentRoom.GetRoomUserManager().GetBots())
                {
                    if (Bot == null)
                        continue;

                    if (!Bot.IsBot)
                        continue;

                    if (Bot.BotData == null)
                        continue;

                    if (Bot.BotData.OwnerId != mClient.GetHabbo().Id)
                        continue;

                    Bots.Add(Bot);
                }
            }

            return Bots;
        }

        public bool NoobWarned
        {
            get { return mNoobWarned; }
            set { mNoobWarned = value; }
        }
        public int UpdateStats
        {
            get { return mUpdateStats; }
            set
            {
                if (value >= 10)
                {
                    using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunFastQuery("UPDATE rp_stats SET curhealth = " + mCurHealth + ", armor = " + mArmor + ", energy = " + mEnergy + ", hunger = " + mHunger + ", hygiene = " + mHygiene + ", constitution = " + mConstitution + ", stamina = " + mStamina + ", intelligence = " + mIntelligence + ", strength = " + mStrength + ", st_deaths = " + mDeaths + ", st_kills = " + mKills + ", st_punches = " + mPunches + ", st_arrested = " + mArrested + ", st_arrests = " + mArrests + ", dead = " + mDead + ", jailed = " + mJailed + ", jail_timer = " + mJailTimer + ", dead_timer = " + mDeadTimer + ", punchkills = " + mPunchKills + ", meleekills = " + mMeleeKills + ", gunkills = " + mGunKills + ", bombkills = " + mBombKills + ", lastkilled = '" + mLastKilled + "', wanted_for = '" + mwanted_for + "', woodXP = '" + mWoodXP + "', woodLevel = '" + mWoodLevel + "', spaceXP = '" + mSpaceXP + "', spaceLevel = '" + mspacelevel + "', colorwars_pts = '" + mColorWarsPts + "', gas = '" + mGas + "', gunlicense = '" + mGunLicense + "', fmj_inv = '" + mFMJInv + "', rds_inv = '" + mRDSInv + "' WHERE id = '" + mUserId + "'");
                  
                    }
                    value = 0;
                }
                else
                {
                    mUpdateStats = value;
                }
            }
        }
        public int JobId
        {
            get { return mJobId; }
            set { mJobId = value; }
        }
        
       /* public int Headquarters
        {
            get { return mHeadquarters; }
            set { mHeadquarters = value; }
        }*/
        public int JobRank
        {
            get { return mJobRank; }
            set { mJobRank = value; }
        }
        public int GangId
        {
            get { return mGangId; }
            set { mGangId = value; }
        }
        public int GangRank
        {
            get { return mGangRank; }
            set { mGangRank = value; }
        }
        public int Crowbar
        {
            get { return mCrowbar; }
            set { mCrowbar = value; }
        }
        public int MeleeKills
        {
            get { return mMeleeKills; }
            set { mMeleeKills = value; }
        }
        public int PunchKills
        {
            get { return mPunchKills; }
            set { mPunchKills = value; }
        }
        public int GunKills
        {
            get { return mGunKills; }
            set { mGunKills = value; }
        }
        public int BombKills
        {
            get { return mBombKills; }
            set { mBombKills = value; }
        }
        public string LastKilled
        {
            get { return mLastKilled; }
            set { mLastKilled = value; }
        }

        public List<uint> BlockedTexters
        {
            get { return mBlockedTexters; }
            set { mBlockedTexters = value; }
        }
        public string ClassChoice
        {
            get { return mClassChoice; }
            set { mClassChoice = value; }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            try
            {
                mUserId = 0;
                mCurHealth = 0;
                mMaxHealth = 0;

                InMafiaWars = false;
                TeamString = String.Empty;
                MKnockedOut = false;
                IsBoxing = false;
                BoxingRoom = null;

                mMyPets.Clear();
                LastPetName = null;
                PetArrested = false;
                PetArresttimeLeft = 0;
                PetDead = false;
                PetDeadtimeLeft = 0;
                UsingPet = false;
                mPetID = 0;

                mEnergy = 0;
                mHunger = 0;
                mHygiene = 0;
                mDeaths = 0;
                mKills = 0;
                mPunches = 0;
                mArrested = 0;
                mArrests = 0;
                mWanted = 0;
                mDead = false;
                mJailed = false;
                mArmored = false;
                mDeadTimer = 0;
                mJailTimer = 0;
                mLastX = 0;
                mLastY = 0;
                mLastZ = 0;
                mClient = null;
                mIsNoob = false;
                mNoobWarned = false;
                mStamina = 0;
                mConstitution = 0;
                mStrength = 0;
                mWeed = 0;
                mCarrots = 0;
                mSentHome = false;
                mLoaned = false;
                mSendHomeTimer = 0;
                mLoandTimer = 0;
                mwantedTimer = 0;
                mIntelligence = 0;
                mCheckingMultiCooldown = false;
                mCoolDown = 0;
                mUpdateStats = 0;
                mJobId = 0;
                mJobRank = 0;
                mGangId = 0;
                mGangRank = 0;
                mPhone = 0;
                mPhone_Credit = 0;
                mBullets = 0;
                mVests = 0;
                mArmor = 0;
                mWeapons.Clear();
                mWorkoutTimer_Done = 0;
                mWorkoutTimer_ToDo = 0;
                mWeightLiftTimer_Done = 0;
                mWeightLiftTimer_ToDo = 0;
                mRobbery = false;
                mATMRobbery = false;
                msjailed = false;
                mSlotRobbery = false;
                mLearning = false;
                mNPA = false;
                mPlane = 0;
                mFuel = 0;
                mCar = 0;
                mBombs = 0;
                mMarried_To = 0;
                mCrowbar = 0;
                mMeleeKills = 0;
                mPunchKills = 0;
                mGunKills = 0;
                mBombKills = 0;
                mSpaceXP = 0;
                mspacelevel = 0;
                mWoodXP = 0;
                mGunLicense = 0;
                mWoodLevel = 0;
                mCurrentXP = 0;
                mCurrentLevel = 0;
                mLoan = 0;
                mFarmingXP = 0;
                mFarmingLevel = 0;
                mColorWarsPts = 0;
                mMafiaWarsPts = 0;
                mLastKilled = null;
                mwanted_for = "";
                mClassChoice = null;
                mHungerDecrement = false;
                mHygieneDecrement = false;
                mBlockedTexters.Clear();
                mShiftsCompleted = 0;
                mGas = 0;
                mGym = 0;
                mgym_np = 0;
                mTotalBill = 0;


                npaTimer = null;
                bankRobTimer = null;
                ATMRobTimer = null;
                SlotRobTimer = null;
                learningTimer = null;
                fftimer = null;
                workingTimer = null;
                sendHomeTimer = null;
                //LoandTimer = null;
                wantedTimer = null;
                taxiTimer = null;
                weedTimer = null;
                gangCaptureTimer = null;
                healTimer = null;
                mediTimer = null;
                massageTimer = null;
                relaxTimer = null;
                hungerTimer = null;
                hygieneTimer = null;
                CoolDownTimer = null;
                UNHANDLEDTIMERS = null;
                planeTimer = null;

                workoutTimer = null;
                weightliftTimer = null;

                Bag = null;
                BagTimer = null;

                FreezeRay = null;
                FrozenTimer = null;
                UsingFreezeRay = false;
                RayFrozen = false;
                RayFrozenSeconds = 0;

                BannedFromVIPAlert = false;

                OfferData.Clear();
                WeaponOfferedSell = null;
                WeaponOfferedSell_By = null;
                WeaponOffered = false;
                WeaponOfferedPrice = 0;

                Working = false;
                WorkInvalidRoomReminder = false;
                FigBeforeWork = null;
                FigWork = null;
                MottBeforeWork = null;
                Cuffed = false;

                GangInvitedTo = 0;
                GangInvited = false;
                GangCapturing = false;
                CheckTurfSpot = 0;

                RadioOff = false;
                GatheringSeconds = 0;
                Gathering = false;
                Debug_Furni = false;
                DisabledTexts = false;
                WorkoutSeconds = 0;
                WorkingOut = false;
                WeightLiftSeconds = 0;
                WeightLifting = false;
                L_MessageWarn = 0;
                L_MessageTimer = 0;
                L_Message4 = null;
                L_Message3 = null;
                L_Message2 = null;
                L_Message1 = null;
                SpamMuted = false;
                StaffMuted = false;
                MuteSeconds = 0;
                EffectSeconds = 0;
                BulletVest = false;
                ActionLast = null;
                LastHit = null;
                LastHitBot = null;
                UsingWeed_Bonus = 0;
                UsingWeed = false;
                DebugStacking = false;
                DebugStack = 0;
                DebugMultiFurni = false;
                DebugMFurni = 0;
                GunShots = 0;
                Equiped2 = null;
                Equiped = null;
                MultiCoolDown.Clear();
                FigBeforeSpecial = null;
                UpdateCount = 0;
                MottBeforeSpecial = null;
                BeingHealed = false;
                UsingMedkit = false;
                BeingMassaged = false;
                Relaxing = false;
                inATM = false;
                inSlotMachine = false;
                RigJackpot = false;
                usingPlane = false;
                loadedPoison = false;
                planeUsing = 0;
                robbingStore = 0;
                usingCar = false;
                StaffDuty = false;
                InShower = false;
                ShowerSeconds = 0;
                Shower = null;
                OnMine = false;

                InteractingAtm = null;
                AtmSetAmount = 0;
                mBank = 0;
                WithdrawDelay = 0;
                Withdraw_Via_Phone = false;

                RequestedTaxi = false;
                RequestedTaxiDestination = null;
                RequestedTaxi_Arrived = false;
                HideTaxiMsg = false;
                RequestedTaxi_WaitTime = 0;
                RecentlyCalledTaxi = false;
                RecentlyCalledTaxi_Timeout = 0;
                TaxiArriveMsg = null;
                LastTexter = null;

                UsingArrow = false;

                DeadSeconds = 0;
                DeadAlerted = false;
                DeadFigSet = false;

                JailedSeconds = 0;
                AdminJailed = false;
                JailedAlerted = false;
                JailFigSet = false;

                ArmoredFigSet = false;

                NPA = false;
                inBrawl = false;
                Brawl = false;
                RobJailed = 0;
                safefromNuke = false;

                Robbery = false;
                ATMRobbery = false;
                sjailed = false;
                slotrobbery = false;
                Learning = false;

                HungerDecrement = false;
                HygieneDecrement = false;

                Green_Dragon = 0;
                Blue_Dragon = 0;
                Silver_Dragon = 0;
                EroSphere1_Dragon = 0;
                EroSphere2_Dragon = 0;
                usingBlue = false;
                usingSilver = false;
                usingEroSphere1_Dragon = false;
                usingEroSphere2_Dragon = false;
                Poisoned = false;
                Frozen = false;
                FrozenSeconds = 0;

                ForceFieldMode = false;
                DeflectionMode = false;
                FreezeMode = false;

                InMiniGame = false;
                HungerGames = false;
                HungerGames_Dead = false;
                HungerGames_Inventory.Clear();
                HungerGames_Item_Wielding = null;
                HungerGames_Pts = 0;
                HungerGames_Cash = 0;
                Brawl_Pts = 0;
                Infection_Pts = 0;
                Infected = false;
                inZombieInfection = false;

                MyPet1 = null;
                Pet_Test_1 = false;
                Pet_Test_2 = false;
                Pet_Test_3 = false;
                Assigned_Attacking = null;
                Pet_Activated_Stance = null;
                Pet_Summoned = 0;
                Pet_Attacking = false;
                Pet_Activated_Mode = null;
                Cached_PetId = 0;
                Cached_Pet = false;

                marryReq = false;
                marryReqer = 0;

                LastCwTeam = null;
                inColourWars = false;
                ColourWarTeam = null;
                figBeforeWar = null;
                Capturing = false;
                KnockedOut = false;

                Changing = false;

            }
            catch (Exception e)
            {
                Out.WriteLine(e.StackTrace, "", ConsoleColor.Red);
            }
        }

        private static object NewMethod()
        {
            return null;
        }

        public void SaveJobComponents()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET job_id = " + mJobId + ", job_rank = " + mJobRank + " WHERE id = " + mUserId + "");
            }
            try
            {
                setRoleplayComponents();
            }
            catch (Exception e)
            {

            }
        }
        public void SaveGangComponents()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET gang_id = " + mGangId + ", gang_rank = " + mGangRank + " WHERE id = " + mUserId + "");
            }
        }
        public void SaveStatusComponents(string status)
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {

                switch (status)
                {
                    case "robbery":


                        //Do nothing cause they ddont fucking need shit to be done
                        break;
                    case "dead":
                        dbClient.RunFastQuery("UPDATE rp_stats SET dead_timer = " + mDeadTimer + ", dead = " + Convert.ToInt32(mDead) + " WHERE id = " + mUserId + "");
                        break;
                    case "sjailed":
                        dbClient.RunFastQuery("UPDATE rp_stats SET sjailed = 1, jailed = 1 WHERE id = " + mUserId + "");
                        break;
                    case "jailed":
                        dbClient.RunFastQuery("UPDATE rp_stats SET jail_timer = " + mJailTimer + ", jailed = " + Convert.ToInt32(mJailed) + " WHERE id = " + mUserId + "");
                        break;
                    case "learning":
                        //Doesnt need a query...
                        //dbClient.RunFastQuery("UPDATE rp_stats SET learning = " + Convert.ToInt32(mLearning) + " WHERE id = " + mUserId + "");
                        break;
                    case "working":
                        GameClient Session;
                        dbClient.RunFastQuery("UPDATE rp_stats SET work_timer = " + workingTimer.getTime() + ", dead = " + Convert.ToInt32(mDead) + " WHERE id = " + mUserId + "");
                        break;
                }
            }
        }

        public void setRoleplayComponents(bool SetTimers = false)
        {
            if (SetTimers)
            {
                if (UNHANDLEDTIMERS == null)
                {
                    UNHANDLEDTIMERS = new UNHANDLEDTIMERS(mClient, mClient.GetHabbo().UserName);
                }
                else
                {
                    if (RoleplayData.Data["debug.show.stop.timer.msg"] == "true")
                    {
                        Console.WriteLine("The shit wasnt null man " + mClient.GetHabbo().UserName);
                    }
                }
            }
            else
            {
                if (mClient == null)
                    return;
                if (mClient.GetHabbo() == null)
                    return;
                if (mClient.GetHabbo().GetRoomUser() == null)
                    return;

                if (mClient.GetHabbo().FavouriteGroup != mClient.GetRoleplay().JobId)
                {
                    mClient.GetHabbo().FavouriteGroup = Convert.ToUInt32(mClient.GetRoleplay().JobId);
                }
            }


        }

        public void RefreshVals()
        {
            if (mClient == null)
                return;

            if (mClient.GetHabbo() == null)
                return;

            if (mClient.GetHabbo().GetRoomUser() == null)
                return;

            if (mClient.GetMessageHandler() == null)
                return;

            if (mClient.GetMessageHandler().GetResponse() == null)
                return;

            mClient.GetMessageHandler()
                .GetResponse()
                .Init(LibraryParser.OutgoingRequest("UpdateAvatarAspectMessageComposer"));
            mClient.GetMessageHandler().GetResponse().AppendString(mClient.GetHabbo().Look);
            mClient.GetMessageHandler().GetResponse().AppendString(mClient.GetHabbo().Gender.ToUpper());
            mClient.GetMessageHandler().SendResponse();
            mClient.GetMessageHandler()
                .GetResponse()
                .Init(LibraryParser.OutgoingRequest("UpdateUserDataMessageComposer"));
            mClient.GetMessageHandler().GetResponse().AppendInteger(-1);
            mClient.GetMessageHandler().GetResponse().AppendString(mClient.GetHabbo().Look);
            mClient.GetMessageHandler().GetResponse().AppendString(mClient.GetHabbo().Gender.ToLower());
            mClient.GetMessageHandler().GetResponse().AppendString(mClient.GetHabbo().Motto);
            mClient.GetMessageHandler().GetResponse().AppendInteger(mClient.GetHabbo().AchievementPoints);
            mClient.GetMessageHandler().SendResponse();

            Room Room = mClient.GetHabbo().CurrentRoom;

            if (Room == null)
            {
                return;
            }

            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("UpdateUserDataMessageComposer"));
            serverMessage.AppendInteger(mClient.GetHabbo().GetRoomUser().VirtualId); //BUGG
            //serverMessage.AppendInt32(-1);
            serverMessage.AppendString(mClient.GetHabbo().GetRoomUser().GetClient().GetHabbo().Look);
            serverMessage.AppendString(mClient.GetHabbo().GetRoomUser().GetClient().GetHabbo().Gender.ToLower());
            serverMessage.AppendString(mClient.GetHabbo().GetRoomUser().GetClient().GetHabbo().Motto);
            serverMessage.AppendInteger(mClient.GetHabbo().GetRoomUser().GetClient().GetHabbo().AchievementPoints);
            Room.SendMessage(serverMessage);

            try
            {

                setRoleplayComponents();

            }
            catch (Exception e)
            {

            }
        }
        public void SaveCurrentRoom()
        {
            mClient = Misc.RoleplayManager.GenerateSession(mUserId);

            if (mClient == null)
                return;

            if (mUserId == 0)
                return;

            if (mClient.GetHabbo() == null)
                return;

            if (mClient.GetHabbo().CurrentRoomId == 0)
                return;

            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE users SET home_room = " + mClient.GetHabbo().CurrentRoomId + " WHERE id = " + mUserId + "");
            }
        }
        public void SaveQuickStat(string Stat, string Newval)
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET " + Stat + " = '" + Newval + "' WHERE id = " + mUserId + "");
            }
        }
        public void SaveCoolDown(string CoolDown_Name)
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("INSERT INTO rp_cooldowns (id,cooldown) VALUES (" + mUserId + ",'" + CoolDown_Name + "')");
            }
            //RoleplayManager.LoadCoolDowns();
        }
        public void DeleteCoolDown(string CoolDown_Name)
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("DELETE FROM rp_cooldowns WHERE id = " + mUserId + " AND cooldown = '" + CoolDown_Name + "'");
            }
            //RoleplayManager.LoadCoolDowns();
        }
        public void SaveAssaultStat()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Assault') WHERE id = " + mUserId + "");
            }
        }
        
        public void SaveJailbreakCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Jailbreak') WHERE id = " + mUserId + "");
            }
        }
        public void AddIllegalWeaponCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Illegalweapon') WHERE id = " + mUserId + "");
            }
        }
        public void AddMurderCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Murder') WHERE id = " + mUserId + "");
            }
        
        }
        public void AddCMurderCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' CopMurder') WHERE id = " + mUserId + "");
            }
        }
         

        public void AddSpammingCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Spamming') WHERE id = " + mUserId + "");
            }
        }
       
        public void AddharrasmentCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Harrasment') WHERE id = " + mUserId + "");
            }
        }

        public void AddtrespassingCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Tresspassing') WHERE id = " + mUserId + "");
            }
        }
        public void AddObstructionCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Obstruction') WHERE id = " + mUserId + "");
            }
        }

        public void AddPushPullCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Push/Pull') WHERE id = " + mUserId + "");
            }
        }

        public void AddnoncomplianceCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Non-Compliance') WHERE id = " + mUserId + "");
            }
        }

        public void AddevadingCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Evade') WHERE id = " + mUserId + "");
            }
        }

        public void AddlogoutCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Logging') WHERE id = " + mUserId + "");
            }
        }

        public void AddCassaultCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Copassault') WHERE id = " + mUserId + "");
            }
        }

        public void AddGenocideCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Genocide') WHERE id = " + mUserId + "");
            }
        }

        public void AddMugCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Robbery') WHERE id = " + mUserId + "");
            }
        }

        public void AddBankRobCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Bank Robbery') WHERE id = " + mUserId + "");
            }
        }

        public void AddAtmrobCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' ATM Robbery') WHERE id = " + mUserId + "");
            }
        }

        public void Add911Charge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' 911 Abuse') WHERE id = " + mUserId + "");
            }
        }

        public void AddImpersonationCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Impersonation') WHERE id = " + mUserId + "");
            }
        }

        public void AddRapeCharge()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET wanted_for = CONCAT(wanted_for, ' Sexual Harassement') WHERE id = " + mUserId + "");
            }
        }




        public void SaveQuickStat(string Stat, int Newval)
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET " + Stat + " = " + Newval + " WHERE id = " + mUserId + "");
            }
        }

       /* public void SaveQuickWeapon(string Stat, int Newval)
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET " + Stat + " = " + Newval + " WHERE id = " + mUserId + "");
            }
        }*/

        public void RefreshWeapons()
                {
                    DataTable Weps = null;
                    Weapons.Clear();

                    using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT * FROM rp_user_weapons WHERE id = '" + mUserId + "'");
                        Weps = dbClient.GetTable();

                        if (Weps != null)
                        {
                            foreach (DataRow Row in Weps.Rows)
                            {
                                if (!Weapons.ContainsKey(Convert.ToString(Row["weapon_data"])))
                                {
                                    string Name = HabboHotel.Roleplay.Combat.WeaponManager.GetWeaponName(Convert.ToString(Row["weapon_data"]));
                                    if (HabboHotel.Roleplay.Combat.WeaponManager.WeaponsData.ContainsKey(Name))
                                    {
                                        Weapons.TryAdd(Convert.ToString(Row["weapon_data"]), HabboHotel.Roleplay.Combat.WeaponManager.WeaponsData[Name]);
                                    }
                                }
                            }
                        }
                    }
                }

        public void SaveWeapons()
        {
            lock (mWeapons)
            {
                foreach (KeyValuePair<string, Weapon> Weap in mWeapons)
                {
                    DataRow CurWeapons = null;

                    if (Weap.Value == null)
                        continue;

                    using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT * FROM rp_user_weapons WHERE weapon_data LIKE '%" + Weap.Key + "%' AND id = '" + mClient.GetHabbo().Id + "'");
                        CurWeapons = dbClient.GetRow();
                        if (CurWeapons == null)
                        {
                            // Console.WriteLine("Inserting " + Weap.Key);
                            dbClient.RunFastQuery("INSERT INTO rp_user_weapons(id,weapon_data) VALUES(" + mClient.GetHabbo().Id + ",'" + Weap.Key + "')");
                        }
                        else
                        {
                            //  Console.WriteLine("Updating " + Weap.Key);
                            dbClient.RunFastQuery("UPDATE rp_user_weapons SET weapon_data = '" + Weap.Key + "' WHERE id = '" + mClient.GetHabbo().Id + "' AND weapon_data LIKE '%" + Weap.Key + "%'");
                        }
                    }
                }
            }
        }
        public void SaveRWeapons()
        {
            lock (mWeapons)
            {
                foreach (KeyValuePair<string, Weapon> Weap in mWeapons)
                {
                    DataRow CurWeapons = null;

                    if (Weap.Value == null)
                        continue;

                    using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT * FROM rp_user_weapons WHERE weapon_data LIKE '%" + Weap.Key + "%' AND id = '" + mClient.GetHabbo().Id + "'");
                        CurWeapons = dbClient.GetRow();
                        if (CurWeapons == null)
                        {
                            // Console.WriteLine("Inserting " + Weap.Key);
                            // dbClient.RunFastQuery("INSERT INTO rp_user_weapons(id,weapon_data) VALUES(" + mClient.GetHabbo().Id + ",'" + Weap.Key + "')");
                        }
                        else
                        {
                            //  Console.WriteLine("Updating " + Weap.Key);
                            dbClient.RunFastQuery("DELETE FROM rp_user_weapons WHERE weapon_data LIKE '%" + Weap.Key + "%' AND id = '" + mClient.GetHabbo().Id + "'");
                        }
                    }
                }
            }
        }
        public void addWeapon(string wep_name)
        {
            GameClient Session = mClient;

            Session.GetRoleplay().Weapons.TryAdd(wep_name + ":1", WeaponManager.WeaponsData[wep_name]);
            Session.GetRoleplay().SaveWeapons();
            Writer.Writer.RpDebug("Added a weapon to user: " + Session.GetHabbo().UserName + " with the weapon name of " +wep_name);
        }
        public void StopWork(bool SendSpeech = false)
        {
            GameClient Session = mClient;

            if (Session.GetRoleplay().FigBeforeWork != null)
            {
                Session.GetHabbo().Look = Session.GetRoleplay().FigBeforeWork;
                Session.GetHabbo().Motto = Session.GetRoleplay().MottBeforeWork;
            }
            if (Session.GetRoleplay().JobHasRights("hospital") && Session.GetRoleplay().Working)
            {
                RoleplayManager.HospOnDuty -= 1;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty Hospital Worker. Leaving " + RoleplayManager.HospOnDuty + " left OnDuty.", ConsoleColor.Blue));
                //Writer.Writer.RpDebug("Added an onduty worker.");
            }
            if (Session.GetRoleplay().JobHasRights("phone") && Session.GetRoleplay().Working)
            {
                RoleplayManager.PhoneOnDuty -= 1;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty Phone Store Worker. Leaving " + RoleplayManager.PhoneOnDuty + " left OnDuty.", ConsoleColor.Blue));
            }
            if (Session.GetRoleplay().JobHasRights("ammunation") && Session.GetRoleplay().Working)
            {
                RoleplayManager.AmmuOnDuty -= 1;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty Ammunation Worker. Leaving " + RoleplayManager.AmmuOnDuty + " left OnDuty.", ConsoleColor.Blue));
            }
            if (Session.GetRoleplay().JobHasRights("diner") && Session.GetRoleplay().Working)
            {
                RoleplayManager.FoodOnDuty -= 1;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty Diner Worker. Leaving " + RoleplayManager.FoodOnDuty + " left OnDuty.", ConsoleColor.Blue));
            }
            if (Session.GetRoleplay().JobHasRights("blackmarket") && Session.GetRoleplay().Working)
            {
                RoleplayManager.BlackOnDuty -= 1;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty BlackMarket Worker. Leaving " + RoleplayManager.BlackOnDuty + " left OnDuty.", ConsoleColor.Blue));
            }
            if (Session.GetRoleplay().JobHasRights("bank") && Session.GetRoleplay().Working)
            {
                RoleplayManager.BankOnDuty -= 1;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty Bank Worker. Leaving " + RoleplayManager.BankOnDuty + " left OnDuty.", ConsoleColor.Blue));
            }
            if (Session.GetRoleplay().JobHasRights("police") && Session.GetRoleplay().Working)
            {
                    RoleplayManager.PoliceOnDuty -= 1;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty Police Officer. Leaving " + RoleplayManager.PoliceOnDuty + " left OnDuty.", ConsoleColor.Blue));
            }
            if (Session.GetRoleplay().JobHasRights("carshop") && Session.GetRoleplay().Working)
            {
                RoleplayManager.CarOnDuty -= 1;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty Car Store Worker. Leaving " + RoleplayManager.CarOnDuty + " left OnDuty.", ConsoleColor.Blue));
            }
            if (Session.GetRoleplay().JobHasRights("gym") && Session.GetRoleplay().Working)
            {
                int sw_x = Convert.ToInt32(RPDataHandler.GetData("gym_stopwork_coords", "x"));
                int sw_y = Convert.ToInt32(RPDataHandler.GetData("gym_stopwork_coords", "y"));
                int sw_z = Convert.ToInt32(RPDataHandler.GetData("gym_stopwork_coords", "z"));
                Session.GetHabbo().GetRoomUser().ClearMovement();
                RoleplayManager.GymOnDuty -= 1;
                Session.GetHabbo().GetRoomUser().SetPos(sw_x, sw_y, sw_z);
                Session.GetHabbo().GetRoomUser().UpdateNeeded = true;
                Out.WriteLine(string.Format("[DEBUG] Removed an OnDuty Gym Worker. Leaving " + RoleplayManager.GymOnDuty + " left OnDuty.", ConsoleColor.Blue));
            }
            Session.GetRoleplay().FigBeforeWork = null;
            Session.GetRoleplay().FigWork = null;
            Session.GetRoleplay().Working = false;
            Session.GetRoleplay().RefreshVals();
            int MyJobId = Session.GetRoleplay().JobId;
            int MyJobRank = Session.GetRoleplay().JobRank;
            string JobName = JobManager.JobData[JobId].Name;
            string RankName = JobManager.JobRankData[JobId, JobRank].Name;

            if (Session.GetRoleplay().workingTimer != null)
            {
                Session.GetRoleplay().workingTimer.stopTimer();
            }

            if (SendSpeech)
            {
                Session.SendWhisper("You have ended your shift as a " + JobName + " " + RankName + "");
                // RoleplayManager.Shout(Session, "*Stops working*", 1);
            }
        }
        public void ApplySpecialStatus(string type)
        {
            string Fig = null;
            string Motto = null;

            GameClient Session = mClient;

            if (Session == null || mClient == null)
            {
                return;
            }

            FigBeforeSpecial = Session.GetHabbo().Look;
            MottBeforeSpecial = Session.GetHabbo().Motto;

            Fig = Session.GetHabbo().Look;
            Motto = Session.GetHabbo().Motto;

            switch (type)
            {
                case "dead":
                    Fig = Misc.RoleplayManager.SplitFigure(Session.GetHabbo().Look) + "lg-270-83.sh-290-83.ch-874-83";
                    Motto = "[Dead Patient]";
                    break;

                case "jailed":
                    Fig = Misc.RoleplayManager.SplitFigure(Session.GetHabbo().Look) + "ch-220-94.lg-280-94.sh-290-62";
                    Random rand = new Random();
                    Motto = "[Jailed Inmate #" + rand.Next(2736, 99999) + "]";
                    break;

                case "armored":
                    if (Session.GetRoleplay().Working)
                    {
                        Fig = Session.GetHabbo().Look;
                        Motto = Session.GetHabbo().Motto;
                    }
                    else
                    {
                        Fig = Session.GetHabbo().Look + ".cc-3420-1428";
                        Motto = Session.GetHabbo().Motto;
                    }
                    break;
            }
            Session.GetHabbo().Look = Fig;
            Session.GetHabbo().Motto = Motto;

            RefreshVals();
        }
        public bool JobHasRights(string right)
        {
            if (mClient == null)
                return false;
            if (mClient.GetRoleplay() == null)
                return false;
            if (!JobManager.validJob(mClient.GetRoleplay().JobId, mClient.GetRoleplay().JobRank))
                return false;
            if (!JobManager.JobRankData[mClient.GetRoleplay().JobId, mClient.GetRoleplay().JobRank].hasRights(right))
                return false;

            return true;
        }
        public void CallPolice()
        {
            lock (Plus.GetGame().GetClientManager().Clients.Values)
            {
                foreach (GameClient client in Plus.GetGame().GetClientManager().Clients.Values)
                {

                    if (client == null)
                        continue;
                    if (client.GetRoleplay() == null)
                        continue;
                    if (!JobManager.validJob(client.GetRoleplay().JobId, client.GetRoleplay().JobRank))
                        continue;
                    if (!client.GetRoleplay().JobHasRights("police"))
                        continue;
                    if (!client.GetRoleplay().Working)
                        continue;
                    if (mClient == null)
                        return;

                    if (Misc.RoleplayManager.PurgeTime)
                    {
                        mClient.SendWhisper("You cannot call the police during a purge!");
                        return;
                    }
                    if (mClient.GetHabbo() == null)
                        return;

                    if (mClient.GetHabbo().GetRoomUser() == null)
                        return;

                    if (mClient.GetHabbo().CurrentRoom.RoomData.Description.Contains("NOCOP"))
                    {
                        mClient.GetHabbo().GetRoomUser().LastBubble = 34;
                        mClient.SendWhisper("You cannot call the police in a 'NOCOP' zone!");
                        mClient.GetHabbo().GetRoomUser().LastBubble = 0;
                        return;
                    }

                    client.GetHabbo().GetRoomUser().LastBubble = 19;
                    client.SendWhisper(mClient.GetHabbo().UserName + " requires assistance in roomid " + mClient.GetHabbo().CurrentRoomId + "!");
                    client.GetHabbo().GetRoomUser().LastBubble = 0;
                }
            }
            Misc.RoleplayManager.Shout(mClient, "*Calls the police for help*");
        }
        public void CalculateWorkoutTimer()
        {
            mWorkoutTimer_ToDo = (mStrength * mStrength) * 6;
            SaveQuickStat("workout_need_timer", "" + mWorkoutTimer_ToDo);
        }
        public void CalculateWeightLiftTimer()
        {
            mWeightLiftTimer_ToDo = (mConstitution * mConstitution) * 10 + 8;
            SaveQuickStat("weightlift_need_timer", "" + mWeightLiftTimer_ToDo);
        }
        public void MakeZombie()
        {

            try
            {
                string ChosenLook = null;
                List<string> ZombieLooks = new List<string>();

                ZombieLooks.Add("ch-3237-75-1408.hd-190-1363.lg-275-75");
                ZombieLooks.Add("hd-195-1360.ca-3223-63.ch-3203-75.he-3239-63.lg-275-110");
                ZombieLooks.Add("hd-207-1392.he-1601-63.ca-3223-63.ch-235-75.lg-3138-74-1408");
                ZombieLooks.Add("hd-207-1392.he-1601-63.ca-3223-63.ch-235-75.lg-3138-74-1408");

                ChosenLook = ZombieLooks[new Random().Next(0, ZombieLooks.Count)];

                GameClient Session = mClient;

                FigBeforeSpecial = Session.GetHabbo().Look;
                MottBeforeSpecial = Session.GetHabbo().Motto;

                Session.GetHabbo().Look = ChosenLook;
                Session.GetHabbo().Motto = Session.GetHabbo().Motto + " [Infected Zombie]";

                RefreshVals();
            }
            catch (Exception e)
            {

            }
        }

        public RoomItem GetNearItem(string Item, int MaxDistance)
        {
            GameClient Session = mClient;
            RoomUser User = Session.GetHabbo().GetRoomUser();

            RoomItem Inter = null;
            lock (Session.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
            {
                foreach (RoomItem item in Session.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
                {

                    if (item == null)
                        continue;

                    if (item.GetBaseItem() == null)
                        continue;

                    HabboHotel.PathFinding.Vector2D Pos1 = new HabboHotel.PathFinding.Vector2D(item.X, item.Y);
                    HabboHotel.PathFinding.Vector2D Pos2 = new HabboHotel.PathFinding.Vector2D(User.X, User.Y);

                    if (RoleplayManager.Distance(Pos1, Pos2) <= MaxDistance)
                    {
                        if (!item.GetBaseItem().Name.Contains(Item))
                            continue;
                        Inter = item;
                    }
                }
            }

            return Inter;
        }
        public bool NearItem(string Item, int MaxDistance)
        {
            GameClient Session = mClient;
            RoomUser User = Session.GetHabbo().GetRoomUser();

            RoomItem Inter = null;
            lock (Session.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
            {
                foreach (RoomItem item in Session.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
                {

                    if (item == null)
                        continue;

                    if (item.GetBaseItem() == null)
                        continue;

                    HabboHotel.PathFinding.Vector2D Pos1 = new HabboHotel.PathFinding.Vector2D(item.X, item.Y);
                    HabboHotel.PathFinding.Vector2D Pos2 = new HabboHotel.PathFinding.Vector2D(User.X, User.Y);

                    if (RoleplayManager.Distance(Pos1, Pos2) <= MaxDistance)
                    {
                        if (!item.GetBaseItem().Name.Contains(Item))
                            continue;

                        Inter = item;

                    }
                }

                if (Inter != null)
                {
                    return true;
                }
            }
            return false;
        }
        public void SendToGang(string Msg, bool IncludeName = true)
        {
            lock (Plus.GetGame().GetClientManager().Clients.Values)
            {
                foreach (GameClient client in Plus.GetGame().GetClientManager().Clients.Values)
                {
                    if (client == null)
                        continue;

                    if (client.GetRoleplay() == null)
                        continue;

                    if (client.GetRoleplay().GangId != mGangId)
                        continue;

                    if (mClient == null)
                        continue;

                    if (mClient.GetHabbo() == null)
                        continue;

                    if (IncludeName)
                    {
                        client.SendWhisper("[Gang Alert][" + mClient.GetHabbo().UserName + "]: " + Msg + "");
                    }
                    else
                        client.SendWhisper("[Gang Alert]: " + Msg + "");
                }
            }
        }

        public void RemoveGodProtection()
        {

            GameClient Session = mClient;

            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET is_noob = 0 WHERE id = '" + Session.GetHabbo().Id + "'");
            }

            Session.SendWhisper("Your god protection has been disabled!");
            Session.GetRoleplay().IsNoob = false;
            Session.GetRoleplay().SaveQuickStat("is_noob", "0");

        }

        public void Transport(uint RoomId, int Timeout)
        {
            GameClient client = mClient;
            Room Room = RoleplayManager.GenerateRoom(RoomId);
            if (Room != null)
            {
                client.GetRoleplay().RequestedTaxi_Arrived = false;
                client.GetRoleplay().HideTaxiMsg = true;
                client.GetRoleplay().RecentlyCalledTaxi = true;
                client.GetRoleplay().RecentlyCalledTaxi_Timeout = Timeout;
                client.GetRoleplay().RequestedTaxiDestination = Room;
                client.GetRoleplay().RequestedTaxi = true;
                client.GetRoleplay().taxiTimer = new taxiTimer(client);
            }
        }
       /* public void notaxiTransport(uint RoomId)
        {
            GameClient client = mClient;
            Room Room = RoleplayManager.GenerateRoom(RoomId);
            if (Room != null)
            {
                client.GetMessageHandler().PrepareRoomForUser(Room);
            }
        }*/
        public void WarpToPos(int X, int Y, bool poverride)
        {
            if (mClient == null)
                return;

            if (mClient.GetHabbo() == null)
                return;

            if (mClient.GetHabbo().GetRoomUser() == null)
                return;

            if (mClient.GetHabbo().CurrentRoom == null)
                return;

            if (poverride)
            {
                mClient.GetHabbo().GetRoomUser().TeleportEnabled = true;
            }

            mClient.GetHabbo().GetRoomUser().MoveTo(X, Y, poverride);

            if (poverride)
            {
                mClient.GetHabbo().GetRoomUser().TeleportEnabled = false;
            }

        }
        public void WarptoItem(string item_name, bool poverride)
        {
            RoomItem retItem = null;
            if (mClient == null)
                return;

            if (mClient.GetHabbo() == null)
                return;

            if (mClient.GetHabbo().GetRoomUser() == null)
                return;

            if (mClient.GetHabbo().CurrentRoom == null)
                return;

            lock (mClient.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
            {
                foreach (RoomItem Item in mClient.GetHabbo().CurrentRoom.GetRoomItemHandler().FloorItems.Values)
                {
                    if (Item == null)
                        continue;
                    if (Item.GetBaseItem() == null)
                        continue;
                    if (!Item.GetBaseItem().Name.Contains(item_name))
                        continue;

                    retItem = Item;
                }
            }
            if (retItem == null)
                return;


            if (poverride)
            {
                mClient.GetHabbo().GetRoomUser().TeleportEnabled = true;
            }

            mClient.GetHabbo().GetRoomUser().MoveTo(retItem.X, retItem.Y, poverride);


            if (poverride)
            {
                mClient.GetHabbo().GetRoomUser().TeleportEnabled = false;
            }

        }

        public void GiveMafiaWarPoints(int Amnt)
        {
            MafiaWarsPts += Amnt;
            mClient.GetRoleplay().SaveQuickStat("mafiawars_pts", MafiaWarsPts);
        }
        public void GiveColorWarPoints(int Amnt)
        {
            ColorWarsPts += Amnt;
            mClient.GetRoleplay().SaveQuickStat("colorwars_pts", ColorWarsPts);
        }
        public void SaveStats()
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_stats SET curhealth = " + mCurHealth + ", armor = " + mArmor + ", energy = " + mEnergy + ", hunger = " + mHunger + ", hygiene = " + mHygiene + ", constitution = " + mConstitution + ", stamina = " + mStamina + ", intelligence = " + mIntelligence + ", strength = " + mStrength + ", st_deaths = " + mDeaths + ", st_kills = " + mKills + ", st_punches = " + mPunches + ", st_arrested = " + mArrested + ", st_arrests = " + mArrests + ", dead = " + mDead + ", jailed = " + mJailed + ", jail_timer = " + mJailTimer + ", dead_timer = " + mDeadTimer + ", punchkills = " + mPunchKills + ", meleekills = " + mMeleeKills + ", gunkills = " + mGunKills + ", bombkills = " + mBombKills + ", lastkilled = '" + mLastKilled + "', woodXP = '" + mWoodXP + "', woodLevel = '" + mWoodLevel + "', spaceXP = '" + mSpaceXP + "', spaceLevel = '" + mspacelevel + "', farmingXP = '" + mFarmingXP + "', farmingLevel = '" + mFarmingLevel + "', currentlevel = '" + mCurrentLevel + "', currentxp = '" + mCurrentXP + "', wanted = '" + mWanted + "', colorwars_pts = '" + mColorWarsPts + "', onwelfare = '" + mHasWelfare + "', loan = '" + mLoan + "', bankaccount = '" + mBankAccount + "', wanted_for = '"+ mwanted_for + "', last_x = '" + mLastX + "', last_y = '" + mLastY + "', last_z = '" + mLastZ + "', gym = '" + mGym +"', gym_np = '" + mgym_np +"', vault_cd = '" + mVaultCoolDown + "'  WHERE id = '" + mUserId + "'");
            
            }
        }
        public void SaveJobBalance(int ammount, int corp)
        {
            using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunFastQuery("UPDATE rp_jobs SET bank_balance = " + ammount + " WHERE id = '" + corp + "'");
            }
        }

        
        public void Freeze(GameClient By)
        {
            int Seconds = FreezeTimeAbility;

            UnEquip();
            mClient.GetHabbo().GetRoomUser().ApplyEffect(0);
            RayFrozen = true;
            RayFrozenSeconds = Seconds;
            mClient.GetHabbo().GetRoomUser().Frozen = true;
            mClient.GetHabbo().GetRoomUser().ApplyEffect(12);

            if (FrozenTimer == null)
            {
                FrozenTimer = new FrozenTimer(mClient);
            }
        }

        public void UnFreeze()
        {
            UnEquip();
            mClient.GetHabbo().GetRoomUser().ApplyEffect(0);
            mClient.GetHabbo().GetRoomUser().Frozen = true;
            RayFrozen = false;
            mClient.GetHabbo().GetRoomUser().Frozen = false;
            RayFrozenSeconds = 0;
            FrozenTimer = null;
        }

        public void OfferDrink(GameClient TargetSession, uint DrinkId)
        {
            Food Drink = Substances.GetDrinkById(DrinkId);
            int price = Substances.SubstanceData[Drink.Item_Name].Item_Price;

            TargetSession.GetRoleplay().OfferData.Add("drink_" + DrinkId, new Offer(mClient, "drink_" + DrinkId, 1, price));
            TargetSession.SendWhisper(mClient.GetHabbo().UserName + " offered you a glass of " + Drink.DisplayName + " for $" + price + ". Type #accept to accept or #deny to deny!");
        }

        public void UnEquip()
        {
            GameClient Session = mClient;

            if (Session.GetRoleplay().Equiped == null)
                return;


            RoomUser User = Session.GetHabbo().GetRoomUser();

            User.CarryItem(0);
            User.ApplyEffect(0);
            RoleplayManager.Shout(Session, "*Un-Equips their " + WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].DisplayName + "*");


            switch (Session.GetRoleplay().Equiped)
            {
                case "freezeray":
                    {

                        //Session.GetRoleplay().FreezeRay.On = false;
                        Session.GetRoleplay().FreezeRay.On = false;
                        Session.GetRoleplay().FreezeRay.stopTimer();
                        Session.GetRoleplay().FreezeRay = null;


                    }
                    break;
            }

            Session.GetRoleplay().Equiped = null;

        }

        public void UnblockTexter(uint Id)
        {
            if (mBlockedTexters.Contains(Id))
            {
                using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunFastQuery("DELETE FROM rp_user_textblocks WHERE receiver_id = " + mClient.GetHabbo().Id + " AND blocked_id = " + Id + "");
                }

                mBlockedTexters.Remove(Id);
            }



            return;
        }
        public void UpdateBlockedTexters()
        {
            lock (mBlockedTexters)
            {
                foreach (uint Texter in mBlockedTexters)
                {
                    DataRow CurBlocked = null;

                    if (Texter == 0)
                        continue;

                    using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT * FROM rp_user_textblocks WHERE receiver_id = " + mClient.GetHabbo().Id + " AND blocked_id = " + Texter + "");
                        CurBlocked = dbClient.GetRow();
                        if (CurBlocked == null)
                        {
                            // Console.WriteLine("Inserting " + Weap.Key);
                            dbClient.RunFastQuery("INSERT INTO rp_user_textblocks VALUES(" + mClient.GetHabbo().Id + "," + Texter + ")");
                        }
                    }
                }
            }
        }
        public void BlockTexter(uint Id)
        {
            if (!mBlockedTexters.Contains(Id))
            {
                mBlockedTexters.Add(Id);
            }

            UpdateBlockedTexters();
        }
        #endregion


        public string mwanted_for { get; set; }
        public bool Trialled { get; internal set; }
    }
}


