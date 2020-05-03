using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Roleplay.Misc;
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
using Plus.HabboHotel.Roleplay.Combat;
using System.Net;
using Plus.HabboHotel.Achievements.Composer;
using Plus.Messages.Parsers;
using Plus.Database.Manager.Database.Session_Details.Interfaces;
using Plus.HabboHotel.Roleplay.Timers;

namespace Plus.HabboHotel.Roleplay.Combat
{
    public class MeleeCombat
    {
        public static bool CanExecuteAttack(GameClient Session, GameClient TargetSession)
        {
            if (Session == null)
                return false;
            if (Session.GetHabbo() == null)
                return false;
            if (Session.GetHabbo().CurrentRoom == null)
                return false;
            if (Session.GetHabbo().GetRoomUser() == null)
                return false;
            if (Session.GetRoleplay().Equiped == null)
            {
                Session.SendWhisper("You have not equiped a weapon!");
                return false;
            }
            if (TargetSession == null)
            {
                Session.SendWhisper("This user was not found in this room!");
                return false;
            }
            if (TargetSession.GetHabbo() == null)
            {
                Session.SendWhisper("This user was not found in this room!");
                return false;
            }
            if (TargetSession.GetHabbo().GetRoomUser() == null)
            {
                Session.SendWhisper("This user was not found in this room!");
                return false;
            }
            if (TargetSession.GetHabbo().CurrentRoom == null)
            {
                Session.SendWhisper("This user was not found in this room!");
                return false;
            }
            if (TargetSession.GetRoleplay() == null)
            {
                return false;
            }
            if (TargetSession.GetHabbo().CurrentRoom != Session.GetHabbo().CurrentRoom)
            {
                Session.SendWhisper("This user was not found in this room!");
                return false;
            }
            Room Room = Session.GetHabbo().CurrentRoom;
            RoomUser RoomUser = Session.GetHabbo().GetRoomUser();
            RoomUser Target = TargetSession.GetHabbo().GetRoomUser();
            Vector2D Pos1 = new Vector2D(RoomUser.X, RoomUser.Y);
            Vector2D Pos2 = new Vector2D(Target.X, Target.Y);
            #region Distance
            if (!Room.RoomData.HitEnabled && RoleplayManager.PurgeTime == false)
            {
                Session.SendWhisper("Sorry, but this is a no melee zone!");
                return false;
            }
            if (Room.RoomData.SafeZone)
            {
                Session.SendWhisper("Sorry, but this is a safe zone!");
                return false;
            }
            if (Session.GetRoleplay().Equiped == null)
            {
                Session.SendWhisper("You do not have a weapon equipped!");
                return false;
            }
            if (TargetSession.GetRoleplay().usingPlane)
            {
                RoleplayManager.Shout(Session, "*Attempts to melee " + TargetSession.GetHabbo().UserName + ", but misses as they are in a plane*");
                Session.GetRoleplay().CoolDown = 3;
                return false;
            }
            if (Session.GetRoleplay().Intelligence < WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Min_Intel)
            {
                Session.SendWhisper("You must have an intelligence level of at least " + WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Min_Intel + " to use this weapon!");
                return false;
            }
            if ((Session.GetRoleplay().Strength + Session.GetRoleplay().savedSTR) < WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Need_Str)
            {
                Session.SendWhisper("You must have a strength level of at least " + WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Need_Str + " to use this weapon!");
                return false;
            }
            if (Session.GetRoleplay().CoolDown > 0)
            {
                Session.SendWhisper("You are cooling down! [" + Session.GetRoleplay().CoolDown + "/3]");
                return false;
            }
            if (RoleplayManager.Distance(Pos1, Pos2) > WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Range)
            {
                Session.Shout("*Swings their " + WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Name + " at " + TargetSession.GetHabbo().UserName + ", but misses*", 3, true);
                Session.GetRoleplay().CoolDown = 3;
                return false;
            }
            #endregion

            #region Status Conditions
            if (Session.GetRoleplay().Energy < WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Need_Energy)
            {
                Session.SendWhisper("You do not have enough energy to do this!");
                return false;
            }
            if (Session.GetRoleplay().inBrawl == true)
            {
                Session.SendWhisper("Unequip your melee weapon for the brawl!");
                return false;
            }
            if (Session.GetRoleplay().Dead)
            {
                Session.SendWhisper("Cannot complete this action while you are dead!");
                return false;
            }
            if (Session.GetRoleplay().Jailed)
            {
                Session.SendWhisper("Cannot complete this action while you are jailed!");
                return false;
            }
            if (TargetSession.GetRoleplay().Dead)
            {
                Session.SendWhisper("Cannot complete this action as this user is dead!");
                return false;
            }
            if (TargetSession.GetRoleplay().Jailed)
            {
                Session.SendWhisper("Cannot complete this action as this user is jailed!");
                return false;
            }
            if (RoomUser.Stunned)
            {
                Session.SendWhisper("Cannot complete this action while you are stunned!");
                return false;
            }
            if (Session.GetRoleplay().IsNoob)
            {
                if (!Session.GetRoleplay().NoobWarned)
                {
                    Session.SendNotif("If you choose to repeat this action, you will no longer be protected.");
                    Session.GetRoleplay().NoobWarned = true;
                    return false;
                }
                else
                {
                    using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunFastQuery("UPDATE rp_stats SET is_noob = 0 WHERE id = '" + Session.GetHabbo().Id + "'");
                    }

                    Session.SendWhisper("Your god protection has been disabled!");
                    Session.GetRoleplay().IsNoob = false;
                    Session.GetRoleplay().SaveQuickStat("is_noob", "0");
                }
            }

            if (TargetSession.GetRoleplay().IsNoob)
            {
                Session.SendWhisper("Cannot complete this action as this user is under God Protection!");
                return false;
            }
            #endregion

            return true;
        }

        public static int CalculateDamage(GameClient Session, GameClient TargetSession)
        {
            int Damage = 0;
            string WeaponName = "";
            double WeaponLevelKills = Session.GetRoleplay().MeleeKills / 250;
            int WeaponLevel = (int)Math.Round(WeaponLevelKills, 0);
            int WeaponDamage = WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Max_Damage;
            foreach (KeyValuePair<string, Weapon> Weapon in Session.GetRoleplay().Weapons)
            {
                string WeaponData = Weapon.Key;
                WeaponName = WeaponManager.GetWeaponName(WeaponData);
            }
            Damage = new Random().Next(WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Min_Damage, WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Max_Damage);
            return Damage;
        }

        public static void ExecuteAttack(GameClient Session, GameClient TargetSession)
        {
            {
                int Damage = MeleeCombat.CalculateDamage(Session, TargetSession);
                string WeaponName = WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Name;
                if (TargetSession.GetRoleplay().Armor >= 1)
                {
                    TargetSession.GetRoleplay().Armor -= Damage;
                    #region Armor Broken
                    if (TargetSession.GetRoleplay().Armor <= 0 && TargetSession.GetRoleplay().Armored == true)
                    {
                        TargetSession.GetRoleplay().Armored = false;
                        TargetSession.GetRoleplay().ArmoredFigSet = false;
                        Session.Shout("*Body-armor breaks and they are no-longer protected from attack!*", 3, true);
                    }
                    #endregion
                }
                else
                {
                    TargetSession.GetRoleplay().CurHealth -= Damage;
                }
                Session.GetRoleplay().Energy -= WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Need_Energy;
                if (TargetSession.GetRoleplay().CurHealth <= 0)
                {
                    int score = new Random().Next(0, 10);
                    Session.Shout("*Swings their " + WeaponName + " at " + TargetSession.GetHabbo().UserName + ", causing " + Damage + " damage and killing them! [ " + score + " Points]*", 27, true);
                    RoleplayManager.HandleDeath(TargetSession, "You were meleed to death by " + Session.GetHabbo().UserName + ", and are being transported to the hospital.");
                    #region handle robbery
                    int lol = new Random().Next(1, 25);
                    if (TargetSession.GetHabbo().Credits > lol)
                    {
                        RoleplayManager.GiveMoney(Session, +lol);
                        Session.Shout("*Steals $" + lol + " from " + TargetSession.GetHabbo().UserName + "'s wallet*", 7, true);
                        RoleplayManager.GiveMoney(TargetSession, -lol);
                    }
                    //int gunchance = new Random().Next(1, 50);
                    //if (TargetSession.GetRoleplay().Equiped != null && gunchance == 2 && WeaponManager.WeaponsData[TargetSession.GetRoleplay().Equiped].Type.Contains("corp"))
                    //{
                    //    string gun = TargetSession.GetRoleplay().Equiped;
                    //    Dictionary<string, Weapon> Weaponss = new Dictionary<string, Weapon>();
                    //    Dictionary<string, Weapon> TWeaponss = new Dictionary<string, Weapon>();
                    //    Weaponss.Clear();
                    //    TWeaponss.Clear();
                    //    foreach (KeyValuePair<string, Weapon> Weaponlol in Session.GetRoleplay().Weapons)
                    //    {
                    //        TWeaponss.Add(WeaponManager.GetWeaponName(Weaponlol.Key), Weaponlol.Value);
                    //    }
                    //    if (TWeaponss.ContainsKey(gun.ToLower()))
                    //    {
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        Session.Shout("*Picks up " + TargetSession.GetHabbo().UserName + "'s " + gun + "*", 7, true);
                    //        Session.GetRoleplay().addWeapon(WeaponManager.WeaponsData[gun.ToLower()].Name);
                    //        TargetSession.GetRoleplay().removeWeapon(TargetSession, TargetSession.GetRoleplay().Equiped);
                    //    }
                    //}
                    if (TargetSession.GetRoleplay().Equiped != null)
                    {
                        
                    }
                    #endregion
                    #region Handle alert
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
                            mClient.SendWhisperBubble("[LiveFeed] " + Session.GetHabbo().UserName + " meleed " + TargetSession.GetHabbo().UserName + " to death", 1);
                        }
                    }
                    #endregion
                    if (Session != TargetSession)
                    {
                        Bounties.CheckBounty(Session, TargetSession.GetHabbo().UserName);
                    }
                    Plus.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_Fighter", 1);
                    Plus.GetGame().GetAchievementManager().ProgressUserAchievement(TargetSession, "ACH_Death", 1);
                    #region Gang Rewards
                    if (Session.GetRoleplay().GangId > 0 && Gangs.GangManager.validGang(Session.GetRoleplay().GangId, Session.GetRoleplay().GangRank))
                    {
                        Random _s = new Random();
                        using (IQueryAdapter dbClient = Plus.GetDatabaseManager().GetQueryReactor())
                        {
                            bool HasGang = false;
                            int gangpts = _s.Next((TargetSession.GetRoleplay().Strength + TargetSession.GetRoleplay().savedSTR) * 1 - (int)Math.Round((double)(TargetSession.GetRoleplay().Strength + TargetSession.GetRoleplay().savedSTR) / 3, 0), (TargetSession.GetRoleplay().Strength + TargetSession.GetRoleplay().savedSTR) * 2);
                            Gangs.GangManager.GangData[Session.GetRoleplay().GangId].Kills++;
                            Gangs.GangManager.GangData[Session.GetRoleplay().GangId].Points += gangpts;
                            if (TargetSession.GetRoleplay().GangId > 0 && Gangs.GangManager.validGang(TargetSession.GetRoleplay().GangId, TargetSession.GetRoleplay().GangRank))
                            {
                                HasGang = true;
                            }
                            if (HasGang)
                            {
                                if (Session.GetHabbo().Rank < 9)
                                {
                                    Gangs.GangManager.GangData[TargetSession.GetRoleplay().GangId].Deaths++;
                                }
                                dbClient.RunFastQuery("UPDATE rp_gangs SET deaths = " + Gangs.GangManager.GangData[TargetSession.GetRoleplay().GangId].Deaths + " WHERE id = " + TargetSession.GetRoleplay().GangId + "");
                            }
                            dbClient.RunFastQuery("UPDATE rp_gangs SET kills = " + Gangs.GangManager.GangData[Session.GetRoleplay().GangId].Kills + ", points = " + Gangs.GangManager.GangData[Session.GetRoleplay().GangId].Points + " WHERE id = " + Session.GetRoleplay().GangId + "");
                        }
                    }
                    #endregion
                    Session.GetHabbo().AchievementPoints += score;
                    Session.GetHabbo().UpdateActivityPointsBalance();
                    Session.GetRoleplay().Kills++;
                    Session.GetRoleplay().MeleeKills++;
                    Session.SendMessage(AchievementScoreUpdateComposer.Compose(Session.GetHabbo().AchievementPoints));
                    /* if (Session.GetRoleplay().Wanted < 10 && Session.GetRoleplay().Masked == false && !Session.GetRoleplay().Equiped.Contains("police"))
                     {
                         if (Session.GetRoleplay().wantedTimer != null)
                         {
                             Session.GetRoleplay().wantedTimer.stopTimer();
                         }
                         Session.GetRoleplay().wantedTimer = new wantedTimer(Session);
                         Session.GetRoleplay().Wanted = 10;
                         Session.GetRoleplay().spacelevel = "Murder";
                         Session.GetRoleplay().SaveQuickStat("wanted", "10");
                     }
                     if (Session.GetHabbo().Rank < 9)
                     {
                         TargetSession.GetRoleplay().Deaths++;
                     }

                 }
                 else
                 {


                     if (WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Speech == null || WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Speech == "")
                     {
                         Session.Shout("*Swings their " + WeaponName + " at " + TargetSession.GetHabbo().UserName + ", causing " + Damage + " damage [-" + WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Need_Energy + " Energy]*", 3, true);
                     }
                     else
                     {
                         string finalspeech = WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Speech.Replace("%energy%", Convert.ToString(WeaponManager.WeaponsData[Session.GetRoleplay().Equiped].Need_Energy));
                         Session.Shout(finalspeech, 3, true);
                     }

                     if (TargetSession.GetRoleplay().Armor >= 1)
                     {
                         TargetSession.Shout("*[" + TargetSession.GetRoleplay().Armor + "AP Left!]*", 6, true);
                     }
                     else
                     {
                         TargetSession.Shout("*[" + TargetSession.GetRoleplay().CurHealth + "/" + TargetSession.GetRoleplay().MaxHealth + "]*", 6, true);
                     }

                    /* if (Session.GetRoleplay().Wanted < 5 && Session.GetRoleplay().Masked == false)
                     {
                         if (Session.GetRoleplay().wantedTimer != null)
                         {
                             Session.GetRoleplay().wantedTimer.stopTimer();
                         }
                         Session.GetRoleplay().wantedTimer = new wantedTimer(Session); Session.GetRoleplay().SaveQuickStat("spacelevel", "Stabbings");
                         Session.GetRoleplay().Wanted = 7;
                         Session.GetRoleplay().spacelevel = "Stabbings";
                         Session.GetRoleplay().SaveQuickStat("wanted", "" + 7);
                     }

                 }
                 Session.GetRoleplay().CoolDown = 3;
                 Session.GetRoleplay().LastKilled = TargetSession.GetHabbo().UserName;
                 TargetSession.GetRoleplay().
                 ++;
                 Session.GetRoleplay().UpdateStats++;*/
                }

            }
        }
    }
}
