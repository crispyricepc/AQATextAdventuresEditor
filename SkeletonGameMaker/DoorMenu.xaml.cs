using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace SkeletonGameMaker
{
    /// <summary>
    /// Interaction logic for DoorMenu.xaml
    /// </summary>
    public partial class DoorMenu : UserControl
    {
        static class StringColors
        {
            public static List<string> ColorsList = new List<string>();
            public static bool IsInitialized = false;

            public static void Initialise()
            {
                IsInitialized = true;
                Type colorType = typeof(System.Drawing.Color);
                PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
                foreach (PropertyInfo propInfo in propInfos)
                {
                    ColorsList.Add(AddSpacesToSentence(propInfo.Name));
                }
            }

            private static string AddSpacesToSentence(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return "";
                StringBuilder newText = new StringBuilder(text.Length * 2);
                newText.Append(text[0]);
                for (int i = 1; i < text.Length; i++)
                {
                    if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                        newText.Append(' ');
                    newText.Append(text[i]);
                }
                return newText.ToString();
            }
        }

        public int RoomID, TargetRoomID;
        public int PrimaryDoorID, SecondaryDoorID;
        public LocationDirection PrimaryRoomDirection;
        public LocationDirection SecondaryRoomDirection
        {
            get
            {
                return PrimaryRoomDirection.GetOpposite();
            }
        }
        public bool Create;

        public event EventHandler OnDoorCreation;

        public DoorMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the selection in the listview and the combobox to be the same as the door selected
        /// </summary>
        private void UpdateDetails()
        {
            Item primarydoor = Saves.Items.GetObjectFromID(PrimaryDoorID);
            List<string> lowerCaseLv = new List<string>();
            List<string> lowerCaseCb = new List<string>();
            foreach (string dooritem in LvDoorColours.Items)
            {
                lowerCaseLv.Add(dooritem.ToLower());
            }
            foreach (ComboBoxItem status in CbDoorStatus.Items)
            {
                lowerCaseCb.Add(status.Content.ToString().ToLower());
            }
            string doorColour = primarydoor.GetDoorColour().ToLower();
            LvDoorColours.SelectedIndex = lowerCaseLv.IndexOf(doorColour);
            string doorStatus = primarydoor.GetStatus()[0];
            CbDoorStatus.SelectedIndex = lowerCaseCb.IndexOf(doorStatus);
        }

        /// <summary>
        /// Ensures everything is ready each time the control is visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                if (!StringColors.IsInitialized)
                {
                    StringColors.Initialise();
                    LvDoorColours.Items.Clear();
                    foreach (string color in StringColors.ColorsList)
                    {
                        LvDoorColours.Items.Add(color);
                    }
                }

                if (PrimaryDoorID != 0 && SecondaryDoorID != 0)
                {
                    UpdateDetails();
                }
            }
        }

        private void BtnDoorCreate_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";

            if (LvDoorColours.SelectedItems.Count != 1)
            {
                errorMessage += "Please choose a door colour\n";
            }
            if (CbDoorStatus.SelectedItem == null)
            {
                errorMessage += "Please choose a door status";
            }
            if (errorMessage.Length != 0)
            {
                MessageBox.Show(errorMessage, "Failed to add door");
            }
            else
            {
                // Create/edit the primary door
                Item primaryDoor = new Item();
                Item secondaryDoor = new Item();
                if (Create)
                {
                    primaryDoor.ID = Saves.FindFreeID(2001, 9999, Saves.Items.GetIDs());
                    secondaryDoor.ID = primaryDoor.ID + 10000;
                }
                else if (PrimaryDoorID != 0 && SecondaryDoorID != 0)
                {
                    primaryDoor.ID = PrimaryDoorID;
                    secondaryDoor.ID = SecondaryDoorID;
                }
                else
                {
                    throw new Exception("Pre-existing doors must be specified with an ID");
                }
                primaryDoor.Name = LvDoorColours.SelectedItem.ToString().ToLower() + " door";
                primaryDoor.Description = "It is a " + primaryDoor.Name;
                primaryDoor.Location = RoomID;
                primaryDoor.Status = ((ComboBoxItem)CbDoorStatus.SelectedItem).Content.ToString().ToLower();
                primaryDoor.Commands = "open,close";
                primaryDoor.Results = PrimaryRoomDirection.LocToString().ToLower() + "," + TargetRoomID.ToString() +
                    ";" + PrimaryRoomDirection.LocToString().ToLower() + ",0";

                // Create the secondary door
                secondaryDoor.Name = primaryDoor.Name;
                secondaryDoor.Description = primaryDoor.Description;
                secondaryDoor.Location = TargetRoomID;
                secondaryDoor.Status = primaryDoor.Status;
                secondaryDoor.Commands = primaryDoor.Commands;
                secondaryDoor.Results = SecondaryRoomDirection.LocToString().ToLower() + "," + RoomID.ToString() +
                    ";" + SecondaryRoomDirection.LocToString().ToLower() + ",0";

                Saves.Items.Add(primaryDoor);
                Saves.Items.Add(secondaryDoor);

                switch (((ComboBoxItem)CbDoorStatus.SelectedItem).Content.ToString().ToLower())
                {
                    case "open":
                        Saves.Places[Saves.Places.GetIndexFromID(RoomID)].SetDirection(PrimaryRoomDirection, TargetRoomID);
                        Saves.Places[Saves.Places.GetIndexFromID(TargetRoomID)].SetDirection(SecondaryRoomDirection, RoomID);
                        break;
                    case "close":
                        Saves.Places[Saves.Places.GetIndexFromID(RoomID)].SetDirection(PrimaryRoomDirection, 0);
                        Saves.Places[Saves.Places.GetIndexFromID(TargetRoomID)].SetDirection(SecondaryRoomDirection, 0);
                        break;
                    case "locked":
                        Saves.Places[Saves.Places.GetIndexFromID(RoomID)].SetDirection(PrimaryRoomDirection, 0);
                        Saves.Places[Saves.Places.GetIndexFromID(TargetRoomID)].SetDirection(SecondaryRoomDirection, 0);
                        break;
                    default:
                        throw new InputException(((ComboBoxItem)CbDoorStatus.SelectedItem).Content.ToString().ToLower() + " is not a valid status");
                }

                OnDoorCreation?.Invoke(sender, EventArgs.Empty);
            }
        }
    }
}
