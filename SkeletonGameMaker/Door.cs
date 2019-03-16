using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameMaker
{
    public class Door : Item
    {
        public int PrimaryID, SecondaryID;
        public string DoorColor
        {
            get
            {
                string newColor = Name.Replace("door", "");
                if (newColor[newColor.Length - 1] == ' ')
                {
                    newColor.Remove(newColor.Length - 1);
                }

                return newColor;
            }
            set
            {
                Name = value + " door";

            }
        }

        public Door() : base()
        {
            if (GetDoorCounterpart(Saves.Items) != -1)
            {
                if (ID < 10000)
                {
                    PrimaryID = ID - 10000;
                    SecondaryID = ID;
                }
                else
                {
                    PrimaryID = ID;
                    SecondaryID = ID + 10000;
                }
            }
            else
            {
                throw new Exception("The item is not a door");
            }
        }

        public LocationDirection GetPrimaryDirection()
        {
            List<string[]> results = Saves.Items[Saves.Items.GetIndexFromID(PrimaryID)].GetResults();
            return LocalConvert.ToLocationDirection(results[0][0]);
        }

        public LocationDirection GetSecondaryDirection()
        {
            List<string[]> results = Saves.Items[Saves.Items.GetIndexFromID(SecondaryID)].GetResults();
            return LocalConvert.ToLocationDirection(results[0][0]);
        }
    }
}
