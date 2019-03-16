using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameMaker
{
    public class Place
    {
        public string Description;
        public int id, North, East, South, West, Up, Down;

        public Place() { }

        public Place(int[] idList)
        {
            id = Saves.FindFreeID(1, 1999, idList);
            Description = "An empty room";
        }

        public List<Item> GetItems(List<Item> allItems)
        {
            List<Item> placeItems = new List<Item>();
            foreach (Item item in allItems)
            {
                if (item.Location == id)
                {
                    placeItems.Add(item);
                }
            }
            return placeItems;
        }

        public int AddItem(int itemid, string name, string description, List<string> status, List<string> commands, List<string[]> results)
        {
            Item newItem = new Item();

            if (itemid == -1)
            {
                int[] idList = Saves.Items.GetIDs();
                newItem.ID = Saves.FindFreeID(2000, 9999, idList);
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

        public void AddDoor(string name, string description, LocationDirection direction, Place placeToJoinWith)
        {
            int[] idList = Saves.Items.GetIDs();
            int thisSideId = Saves.FindFreeID(2000, 9999, idList);
            int otherSideId = thisSideId + 10000;
        }
    }
}
