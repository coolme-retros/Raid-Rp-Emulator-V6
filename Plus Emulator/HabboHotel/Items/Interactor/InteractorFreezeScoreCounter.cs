﻿using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Rooms.Games;

namespace Plus.HabboHotel.Items.Interactor
{
    internal class InteractorFreezeScoreCounter : IFurniInteractor
    {
        public void OnPlace(GameClient session, RoomItem item)
        {
            if (item.Team == Team.none)
            {
                return;
            }
            item.ExtraData = item.GetRoom().GetGameManager().Points[(int)item.Team].ToString();
            item.UpdateState(false, true);
        }

        public void OnRemove(GameClient session, RoomItem item)
        {
        }

        public void OnTrigger(GameClient session, RoomItem item, int request, bool hasRights)
        {
            if (!hasRights)
            {
                return;
            }

            item.GetRoom().GetGameManager().Points[(int)item.Team] = 0;
            item.ExtraData = "0";
            item.UpdateState();
        }

        public void OnWiredTrigger(RoomItem item)
        {
        }

        public void OnUserWalk(GameClient session, RoomItem item, RoomUser user)
        {
        }
    }
}