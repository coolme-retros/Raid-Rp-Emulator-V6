﻿#region

using System;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.Messages;
using Plus.Messages.Parsers;

#endregion

namespace Plus.HabboHotel.Items.Interactor
{
    internal class InteractorWalkInternalLink : IFurniInteractor
    {
        public void OnPlace(GameClient session, RoomItem item)
        {
        }

        public void OnRemove(GameClient session, RoomItem item)
        {
        }

        public void OnTrigger(GameClient session, RoomItem item, int request, bool hasRights)
        {
        }

        public void OnUserWalk(GameClient session, RoomItem item, RoomUser user)
        {
            if (item == null || user == null) return;
            var data = item.ExtraData.Split(Convert.ToChar(9));
            if (item.ExtraData == "" || data.Length < 4) return;
            var message = new ServerMessage(LibraryParser.OutgoingRequest("InternalLinkMessageComposer"));
            message.AppendString(data[3]);
            session.SendMessage(message);
        }

        public void OnWiredTrigger(RoomItem item)
        {
        }
    }
}