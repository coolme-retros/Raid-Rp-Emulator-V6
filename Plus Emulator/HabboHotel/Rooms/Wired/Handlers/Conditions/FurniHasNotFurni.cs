﻿using Plus.HabboHotel.Items;
using System.Collections.Generic;
using System.Linq;

namespace Plus.HabboHotel.Rooms.Wired.Handlers.Conditions
{
    internal class FurniHasNotFurni : IWiredItem
    {
        public FurniHasNotFurni(RoomItem item, Room room)
        {
            Item = item;
            Room = room;
            Items = new List<RoomItem>();
        }

        public Interaction Type
        {
            get { return Interaction.ConditionFurniHasNotFurni; }
        }

        public RoomItem Item { get; set; }

        public Room Room { get; set; }

        public List<RoomItem> Items { get; set; }

        public string OtherString
        {
            get { return ""; }
            set { }
        }

        public string OtherExtraString
        {
            get { return ""; }
            set { }
        }

        public string OtherExtraString2
        {
            get { return ""; }
            set { }
        }

        public bool OtherBool
        {
            get { return true; }
            set { }
        }

        public int Delay
        {
            get { return 0; }
            set { }
        }

        public bool Execute(params object[] stuff)
        {
            if (!Items.Any())
                return true;

            foreach (var current in Items.Where(current => current != null && Room.GetRoomItemHandler().FloorItems.ContainsKey(current.Id)))
            {
                if (
                    current.AffectedTiles.Values.Where(
                        current2 => Room.GetGameMap().SquareHasFurni(current2.X, current2.Y))
                        .Any(
                            current2 =>
                                Room.GetGameMap()
                                    .GetRoomItemForSquare(current2.X, current2.Y)
                                    .Any(current3 => current3.Id != current.Id && current3.Z >= current.Z)))
                    return false;
                if (!Room.GetGameMap().SquareHasFurni(current.X, current.Y))
                    continue;
                if (
                    Room.GetGameMap()
                        .GetRoomItemForSquare(current.X, current.Y)
                        .Any(current4 => current4.Id != current.Id && current4.Z >= current.Z))
                    return false;
            }

            return true;
        }
    }
}