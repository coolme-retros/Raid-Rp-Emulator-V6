﻿using Plus.HabboHotel.Quests;
using Plus.Messages.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plus.Messages.Handlers
{
    /// <summary>
    /// Class GameClientMessageHandler.
    /// </summary>
    internal partial class GameClientMessageHandler
    {
        /// <summary>
        /// Initializes the messenger.
        /// </summary>
        internal void InitMessenger()
        {
            
        }

        /// <summary>
        /// Friendses the list update.
        /// </summary>
        internal void FriendsListUpdate()
        {
            Session.GetHabbo().GetMessenger();
        }

        /// <summary>
        /// Removes the buddy.
        /// </summary>
        internal void RemoveBuddy()
        {
            if (Session.GetHabbo().GetMessenger() == null) return;
            var num = Request.GetInteger();
            for (var i = 0; i < num; i++)
            {
                var num2 = Request.GetUInteger();
                if (Session.GetHabbo().Relationships.ContainsKey(Convert.ToInt32(num2)))
                {
                    this.Session.SendNotif(Plus.GetLanguage().GetVar("buddy_error_1"));
                    return;
                }
                else Session.GetHabbo().GetMessenger().DestroyFriendship(num2);
            }
        }

        /// <summary>
        /// Searches the habbo.
        /// </summary>
        internal void SearchHabbo()
        {
            if (Session.GetHabbo().GetMessenger() == null) return;
            Session.SendMessage(Session.GetHabbo().GetMessenger().PerformSearch(Request.GetString()));
        }

        /// <summary>
        /// Accepts the request.
        /// </summary>
        internal void AcceptRequest()
        {
            if (Session.GetHabbo().GetMessenger() == null) return;

            if (Session.GetRoleplay().Phone == 0)
            {
                Session.SendNotif("You do not have a phone to accept contacts!");
                return;
            }

            var num = Request.GetInteger();
            for (var i = 0; i < num; i++)
            {
                var num2 = Request.GetUInteger();
                var request = Session.GetHabbo().GetMessenger().GetRequest(num2);
                if (request == null) continue;
                if (request.To != Session.GetHabbo().Id) return;
                if (!Session.GetHabbo().GetMessenger().FriendshipExists(request.To)) Session.GetHabbo().GetMessenger().CreateFriendship(request.From);
                Session.GetHabbo().GetMessenger().HandleRequest(num2);
            }
        }

        /// <summary>
        /// Declines the request.
        /// </summary>
        internal void DeclineRequest()
        {
            if (Session.GetHabbo().GetMessenger() == null) return;
            var flag = Request.GetBool();
            Request.GetInteger();
            if (!flag)
            {
                var sender = Request.GetUInteger();
                Session.GetHabbo().GetMessenger().HandleRequest(sender);
                return;
            }
            Session.GetHabbo().GetMessenger().HandleAllRequests();
        }

        /// <summary>
        /// Requests the buddy.
        /// </summary>
        internal void RequestBuddy()
        {
            if (Session.GetRoleplay().Phone == 0)
            {
                Session.SendNotif("You do not have a phone to request contact requests!");
                return;
            }

            if (Session.GetHabbo().GetMessenger() == null) return;
            if (Session.GetHabbo().GetMessenger().RequestBuddy(Request.GetString())) Plus.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SocialFriend, 0u);
        }

        /// <summary>
        /// Sends the instant messenger.
        /// </summary>
        internal void SendInstantMessenger()
        {
            var toId = Request.GetUInteger();
            var text = Request.GetString();
            if (Session.GetHabbo().GetMessenger() == null) return;
            if (!string.IsNullOrWhiteSpace(text)) Session.GetHabbo().GetMessenger().SendInstantMessage(toId, text);
        }

        internal void CallPolice()
        {

            #region Conditions
            if (!Session.GetRoleplay().MultiCoolDown.ContainsKey("call_police"))
            {
                Session.GetRoleplay().MultiCoolDown.Add("call_police", 0);
            }

            if (Session.GetRoleplay().MultiCoolDown["call_police"] > 0)
            {
                Session.SendWhisper("Cooldown [" + Session.GetRoleplay().MultiCoolDown["call_police"] + "/5]");
                return;
            }
            if (Session.GetRoleplay().Dead)
            {
                Session.SendWhisper("You cannot do this while you are dead!");
                return;
            }

            if (Session.GetRoleplay().Jailed)
            {
                Session.SendWhisper("You cannot do this while you are jailed!");
                return;
            }

            if (Session.GetRoleplay().IsNoob)
            {
                Session.SendWhisper("You cannot do this while you are a noob!");
                return;
            }

            if (HabboHotel.Roleplay.Misc.RoleplayManager.PurgeTime)
            {
                Session.SendWhisper("You cannot call the cops during purge!");
                return;
            }

            #endregion

            #region Execute

            Session.GetRoleplay().CallPolice();
            Session.GetRoleplay().MultiCoolDown["call_police"] = 5;
            Session.GetRoleplay().CheckingMultiCooldown = true;

            #endregion

        }

        /// <summary>
        /// Follows the buddy.
        /// </summary>
        internal void FollowBuddy()
        {
            if (Session.GetHabbo().Rank < 9) { Session.SendNotif("You are not allowed to follow your buddy!");  return; }
            var userId = Request.GetUInteger();
            var clientByUserId = Plus.GetGame().GetClientManager().GetClientByUserId(userId);

            if (clientByUserId == null || clientByUserId.GetHabbo() == null) return;
            if (clientByUserId.GetHabbo().GetMessenger() == null || clientByUserId.GetHabbo().CurrentRoom == null)
            {
                if (Session.GetHabbo().GetMessenger() == null) return;
                Response.Init(LibraryParser.OutgoingRequest("FollowFriendErrorMessageComposer"));
                Response.AppendInteger(2);
                SendResponse();
                Session.GetHabbo().GetMessenger().UpdateFriend(userId, clientByUserId, true);
                return;
            }
            if (Session.GetHabbo().Rank < 4 && Session.GetHabbo().GetMessenger() != null && !Session.GetHabbo().GetMessenger().FriendshipExists(userId))
            {
                Response.Init(LibraryParser.OutgoingRequest("FollowFriendErrorMessageComposer"));
                Response.AppendInteger(0);
                SendResponse();
                return;
            }

            var roomFwd = new ServerMessage(LibraryParser.OutgoingRequest("RoomForwardMessageComposer"));
            roomFwd.AppendInteger(clientByUserId.GetHabbo().CurrentRoom.RoomId);
            Session.SendMessage(roomFwd);
        }

        /// <summary>
        /// Sends the instant invite.
        /// </summary>
        internal void SendInstantInvite()
        {
            var num = Request.GetInteger();
            var list = new List<uint>();
            for (var i = 0; i < num; i++) list.Add(Request.GetUInteger());
            var s = Request.GetString();
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("ConsoleInvitationMessageComposer"));
            serverMessage.AppendInteger(Session.GetHabbo().Id);
            serverMessage.AppendString(s);
            foreach (var clientByUserId in (from current in list
                                            where Session.GetHabbo().GetMessenger().FriendshipExists(current)
                                            select Plus.GetGame().GetClientManager().GetClientByUserId(current))
                .TakeWhile(
                    clientByUserId => clientByUserId != null)) clientByUserId.SendMessage(serverMessage);
        }
    }
}