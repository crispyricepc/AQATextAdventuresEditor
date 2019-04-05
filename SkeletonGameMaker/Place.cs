using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameMaker
{
    /// <summary>
    /// Sometimes reffered to as room, contains items and characters
    /// </summary>
    public class Place
    {
        public string Description;
        /// <summary>
        /// The ID stored in the rooms list
        /// </summary>
        public int id;
        /// <summary>
        /// Determines whether the user is able to go in the specified direction
        /// </summary>
        public int North, East, South, West, Up, Down;
        /// <summary>
        /// The ID of the door store in the specified direction. If there is no door, it is -1
        /// </summary>
        public int NorthDoor, SouthDoor, EastDoor, WestDoor, UpDoor, DownDoor;

        public Place()
        {
            NorthDoor = 0;
            SouthDoor = 0;
            EastDoor = 0;
            WestDoor = 0;
            UpDoor = 0;
            DownDoor = 0;

            Description = "An empty room";
        }

        public Place(int[] idList)
        {
            new Place();

            id = Saves.FindFreeID(1, 1999, idList);
        }

        /// <summary>
        /// Gets all the items in a place
        /// </summary>
        /// <returns>A list of items including doors</returns>
        public List<Item> GetItems()
        {
            List<Item> placeItems = new List<Item>();
            foreach (Item item in Saves.Items)
            {
                if (item.Location == id)
                {
                    placeItems.Add(item);
                }
            }
            return placeItems;
        }
        public Dictionary<LocationDirection, Item> GetDoors()
        {
            Dictionary<LocationDirection, Item> valuePairs = new Dictionary<LocationDirection, Item>();
            foreach (Item item in GetItems())
            {
                if (item.GetDoorCounterpart(Saves.Items) != -1)
                {
                    LocationDirection direction = LocalConvert.ToLocationDirection(item.GetResults()[0][0]);
                    if (!valuePairs.ContainsKey(direction))
                    {
                        valuePairs.Add(direction, item);
                    }
                }
            }

            return valuePairs;
        }

        /// <summary>
        /// Adds an item to the Items list with the place as its location
        /// </summary>
        /// <param name="itemid"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="status"></param>
        /// <param name="commands"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public int AddItem(int itemid, string name, string description, List<string> status, List<string> commands, List<string[]> results)
        {
            Item newItem = new Item();

            if (itemid == -1)
            {
                int[] idList = Saves.Items.GetIDs();
                newItem.ID = Saves.FindFreeID(2001, 9999, idList);
            }
            else newItem.ID = itemid;
            newItem.Name = name;
            newItem.Description = description;
            newItem.Location = id;
            newItem.OverWriteStatus(status);
            newItem.OverWriteCommands(commands);
            newItem.OverWriteResults(results);

            Saves.Items.Add(newItem);

            return newItem.ID;
        }
        
        /// <summary>
        /// Sets a place's North/South/etc value to another Place's ID
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="targetId"></param>
        public void SetDirection(LocationDirection direction, int targetId)
        {
            switch (direction)
            {
                case LocationDirection.North:
                    North = targetId;
                    break;
                case LocationDirection.South:
                    South = targetId;
                    break;
                case LocationDirection.East:
                    East = targetId;
                    break;
                case LocationDirection.West:
                    West = targetId;
                    break;
                case LocationDirection.Up:
                    Up = targetId;
                    break;
                case LocationDirection.Down:
                    Down = targetId;
                    break;
                default:
                    throw new Exception("Location invalid");
            }
        }
    }
}
