using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.IO;

namespace SkeletonGameMaker
{
    public static class Saves
    {
        public static string Filename;

        public static List<Character> Characters = new List<Character>();
        public static List<Item> Items = new List<Item>();
        public static List<Place> Places = new List<Place>();

        public static void LoadGame(string filename, List<Character> characters, List<Item> items, List<Place> places) // Loads a binary file called .gme
        {
            int noOfCharacters, noOfPlaces, NoOfItems;
            using (BinaryReader Reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                characters.Clear();
                items.Clear();
                places.Clear();

                noOfCharacters = Reader.ReadInt32();
                for (int Count = 0; Count < noOfCharacters; Count++)
                {
                    Character tempCharacter = new Character();
                    tempCharacter.ID = Reader.ReadInt32();
                    tempCharacter.Name = Reader.ReadString();
                    tempCharacter.Description = Reader.ReadString();
                    tempCharacter.CurrentLocation = Reader.ReadInt32();
                    characters.Add(tempCharacter);
                }
                noOfPlaces = Reader.ReadInt32();
                for (int Count = 0; Count < noOfPlaces; Count++)
                {
                    Place tempPlace = new Place();
                    tempPlace.id = Reader.ReadInt32();
                    tempPlace.Description = Reader.ReadString();
                    tempPlace.North = Reader.ReadInt32();
                    tempPlace.East = Reader.ReadInt32();
                    tempPlace.South = Reader.ReadInt32();
                    tempPlace.West = Reader.ReadInt32();
                    tempPlace.Up = Reader.ReadInt32();
                    tempPlace.Down = Reader.ReadInt32();
                    places.Add(tempPlace);
                }
                NoOfItems = Reader.ReadInt32();
                for (int Count = 0; Count < NoOfItems; Count++)
                {
                    Item tempItem = new Item();
                    tempItem.ID = Reader.ReadInt32();
                    tempItem.Description = Reader.ReadString();
                    tempItem.Status = Reader.ReadString();
                    tempItem.Location = Reader.ReadInt32();
                    tempItem.Name = Reader.ReadString();
                    tempItem.Commands = Reader.ReadString();
                    tempItem.Results = Reader.ReadString();
                    items.Add(tempItem);
                }
            }
        }
        public static void MakeGame(string filename)
        {
            int noOfCharacters = Characters.Count;
            int noOfItems = Items.Count;
            int noOfPlaces = Places.Count;
            using (BinaryWriter Writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                Writer.Write(noOfCharacters);
                for (int Count = 0; Count < noOfCharacters; Count++)
                {
                    Character tempCharacter = Characters[Count];
                    Writer.Write(tempCharacter.ID);
                    Writer.Write(tempCharacter.Name);
                    Writer.Write(tempCharacter.Description);
                    Writer.Write(tempCharacter.CurrentLocation);
                }
                Writer.Write(noOfPlaces);
                for (int Count = 0; Count < noOfPlaces; Count++)
                {
                    Place tempPlace = Places[Count];
                    Writer.Write(tempPlace.id);
                    Writer.Write(tempPlace.Description);
                    Writer.Write(tempPlace.North);
                    Writer.Write(tempPlace.East);
                    Writer.Write(tempPlace.South);
                    Writer.Write(tempPlace.West);
                    Writer.Write(tempPlace.Up);
                    Writer.Write(tempPlace.Down);
                }
                Writer.Write(noOfItems);
                for (int Count = 0; Count < noOfItems; Count++)
                {
                    Item tempItem = Items[Count];
                    Writer.Write(tempItem.ID);
                    Writer.Write(tempItem.Description);
                    Writer.Write(tempItem.Status);
                    Writer.Write(tempItem.Location);
                    Writer.Write(tempItem.Name);
                    Writer.Write(tempItem.Commands);
                    Writer.Write(tempItem.Results);
                }
            }
        }

        public static int FindFreeID(int lower, int upper, int[] idList)
        {
            int possibleID = lower - 1;
            bool found;
            do
            {
                possibleID++;
                found = true;
                foreach (int objectID in idList)
                {
                    if (possibleID == objectID)
                    {
                        found = false;
                    }
                }
            }
            while (possibleID < upper + 1 && !found);

            return possibleID;
        }
    }

    public static class CollectionsExt
    {
        public static int[] GetIDs(this List<Place> places)
        {
            int[] idList = new int[places.Count];
            int count = 0;

            foreach (Place place in places)
            {
                idList[count] = place.id;
                count++;
            }

            return idList;
        }
        public static int GetIndexFromID(this List<Place> places, int idToFind)
        {
            for (int i = 0; i < places.Count; i++)
            {
                if (places[i].id == idToFind)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void UpdatePlace(this List<Place> places, Place placeToUpdate)
        {
            for (int i = 0; i < places.Count; i++)
            {
                if (places[i].id == placeToUpdate.id)
                {
                    places[i] = placeToUpdate;
                }
            }
        }

        public static int[] GetIDs(this List<Item> items)
        {
            int[] idList = new int[items.Count];
            int count = 0;

            foreach (Item item in items)
            {
                idList[count] = item.ID;
                count++;
            }

            return idList;
        }
        public static int GetIndexFromID(this List<Item> items, int idToFind)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == idToFind)
                {
                    return i;
                }
            }
            return -1;
        }
        public static Item GetObjectFromID(this List<Item> items, int idToFind)
        {
            return items[items.GetIndexFromID(idToFind)];
        }
    }

    public static class LocalConvert
    {
        public static LocationDirection ToLocationDirection(string direction)
        {
            switch (direction.ToLower())
            {
                case "north":
                    return LocationDirection.North;
                case "south":
                    return LocationDirection.South;
                case "east":
                    return LocationDirection.East;
                case "west":
                    return LocationDirection.West;
                case "up":
                    return LocationDirection.Up;
                case "down":
                    return LocationDirection.Down;
                default:
                    throw new ArgumentException(direction + " is not a valid direction");
            }
        }
    }
}
