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

namespace SkeletonGameMaker
{
    /// <summary>
    /// Interaction logic for DoorMenu.xaml
    /// </summary>
    public partial class DoorMenu : UserControl
    {
        public int RoomID, TargetRoomID;
        public LocationDirection PrimaryRoomDirection;
        public LocationDirection SecondaryRoomDirection
        {
            get
            {
                return PrimaryRoomDirection.GetOpposite();
            }
        }
        public event EventHandler OnDoorCreation;

        public DoorMenu()
        {
            InitializeComponent();
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
                Item primaryDoor = new Item();
                Item secondaryDoor = new Item();
                primaryDoor.ID = Saves.FindFreeID(2001, 9999, Saves.Items.GetIDs());
                primaryDoor.Name = ((ListViewItem)LvDoorColours.SelectedItem).Content.ToString().ToLower() + " door";
                primaryDoor.Description = "It is a " + primaryDoor.Name;
                primaryDoor.Location = RoomID;
                primaryDoor.Status = ((ComboBoxItem)CbDoorStatus.SelectedItem).Content.ToString().ToLower();
                primaryDoor.Commands = "open,close";
                primaryDoor.Results = nameof(PrimaryRoomDirection).ToLower() + "," + TargetRoomID.ToString() +
                    ";" + nameof(PrimaryRoomDirection).ToLower() + ",0";

                secondaryDoor.ID = primaryDoor.ID + 10000;
                secondaryDoor.Name = primaryDoor.Name;
                secondaryDoor.Description = "It is a " + secondaryDoor.Name;
                secondaryDoor.Location = TargetRoomID;
                secondaryDoor.Status = primaryDoor.Status;
                secondaryDoor.Commands = primaryDoor.Commands;
                secondaryDoor.Results = nameof(SecondaryRoomDirection).ToLower() + "," + RoomID.ToString() +
                    ";" + nameof(SecondaryRoomDirection).ToLower() + ",0";

                Saves.Items.Add(primaryDoor);
                Saves.Items.Add(secondaryDoor);

                OnDoorCreation?.Invoke(sender, EventArgs.Empty);
            }
        }
    }
}
