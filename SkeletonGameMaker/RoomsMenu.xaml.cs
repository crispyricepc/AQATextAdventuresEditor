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
    public enum LocationDirection
    {
        North,
        East,
        South,
        West,
        Up,
        Down
    }

    public partial class RoomsMenu : UserControl
    {
        public Place Room;
        public int itemIDSelected;
        private int North, South, East, West, Up, Down;

        public event EventHandler OnBtnNewItemClick;
        public event EventHandler OnSelectItemClick;

        public RoomsMenu()
        {
            InitializeComponent();
        }
        
        private void UpdateList()
        {
            LvRoomsList.Items.Clear();

            foreach (Place place in Saves.Places)
            {
                LvRoomsList.Items.Add(place.id.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GrdDetails.Visibility = Visibility.Hidden;
            UpdateList();
        }

        private void BtnNorth_Click(object sender, RoutedEventArgs e)
        {
            ChangeSelectionToDirection(North, Room, LocationDirection.North);
        }

        private void BtnEast_Click(object sender, RoutedEventArgs e)
        {
            ChangeSelectionToDirection(East, Room, LocationDirection.East);
        }

        private void BtnSouth_Click(object sender, RoutedEventArgs e)
        {
            ChangeSelectionToDirection(South, Room, LocationDirection.South);
        }

        private void BtnWest_Click(object sender, RoutedEventArgs e)
        {
            ChangeSelectionToDirection(West, Room, LocationDirection.West);
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            ChangeSelectionToDirection(Up, Room, LocationDirection.Up);
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            ChangeSelectionToDirection(Down, Room, LocationDirection.Down);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // this needs to delete all items in the room as well as all items contained in containers in the room AS WELL AS DOORS

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this room", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) Saves.Places.Remove(Room);

            LvRoomsList.SelectedIndex = -1;
            UpdateList();
        }

        private void BtnNewItem_Click(object sender, RoutedEventArgs e)
        {
            OnBtnNewItemClick?.Invoke(this, EventArgs.Empty);
        }

        private void TbDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LvRoomsList.SelectedItems.Count == 1)
            {
                int index = Saves.Places.GetIndexFromID(Convert.ToInt16(LvRoomsList.SelectedItems[0]));
                Saves.Places[index].Description = TbDescription.Text;
            }
        }

        private void ChangeSelectionToDirection(int roomId, Place room, LocationDirection direction)
        {
            if (roomId == 0)
            {
                roomId = CreateNewRoom(room, direction);
            }
            for (int i = 0; i < LvRoomsList.Items.Count; i++)
            {
                if (Convert.ToInt16(LvRoomsList.Items[i]) == roomId)
                {
                    LvRoomsList.SelectedIndex = i;
                }
            }
        }

        private void LvItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvItemsList.SelectedItems.Count == 1)
            {
                ListViewItem lvi = (ListViewItem)LvItemsList.SelectedItem;
                itemIDSelected = Convert.ToInt16(lvi.Tag);

                if (Saves.Items.GetObjectFromID(itemIDSelected).GetDoorCounterpart(Saves.Items) == -1)
                {
                    OnSelectItemClick?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private int CreateNewRoom(Place prevRoom, LocationDirection direction)
        {
            int[] idList = Saves.Places.GetIDs();
            Place newPlace = new Place(idList);
            switch (direction)
            {
                case LocationDirection.North:
                    // link the two places together
                    prevRoom.North = newPlace.id;
                    newPlace.South = prevRoom.id;
                    break;
                case LocationDirection.East:
                    prevRoom.East = newPlace.id;
                    newPlace.West = prevRoom.id;
                    break;
                case LocationDirection.South:
                    prevRoom.South = newPlace.id;
                    newPlace.North = prevRoom.id;
                    break;
                case LocationDirection.West:
                    prevRoom.West = newPlace.id;
                    newPlace.East = prevRoom.id;
                    break;
                case LocationDirection.Up:
                    prevRoom.Up = newPlace.id;
                    newPlace.Down = prevRoom.id;
                    break;
                case LocationDirection.Down:
                    prevRoom.Down = newPlace.id;
                    newPlace.Up = prevRoom.id;
                    break;
            }

            Saves.Places.UpdatePlace(prevRoom);
            Saves.Places.Add(newPlace);
            UpdateList();
            return newPlace.id;
        }

        private void CheckForDoors(Place room)
        {
            foreach (Item item in room.GetItems(Saves.Items))
            {
                int doorCounterpart = item.GetDoorCounterpart(Saves.Items);

                if (doorCounterpart != -1) // if item is a door
                {
                    List<string[]> itemResults = item.GetResults();

                    switch (itemResults[0][0])
                    {
                        case "north":
                            North = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case "east":
                            East = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case "south":
                            South = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case "west":
                            West = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case "up":
                            Up = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case "down":
                            Down = Convert.ToInt16(itemResults[0][1]);
                            break;
                    }
                }
            }
        }

        private void UpdateButtons(Place room)
        {
            if (North != 0) BtnNorth.Content = "View North Room";
            else BtnNorth.Content = "Create North Room";

            if (South != 0) BtnSouth.Content = "View South Room";
            else BtnSouth.Content = "Create South Room";

            if (East != 0) BtnEast.Content = "View East Room";
            else BtnEast.Content = "Create East Room";

            if (West != 0) BtnWest.Content = "View West Room";
            else BtnWest.Content = "Create West Room";

            if (Up != 0) BtnUp.Content = "View Above Room";
            else BtnUp.Content = "Create Above Room";

            if (Down != 0) BtnDown.Content = "View Below Room";
            else BtnDown.Content = "Create Below Room";
        }

        private void UpdateGrdDetails(int placeIndex)
        {
            Room = Saves.Places[placeIndex];
            North = Room.North;
            South = Room.South;
            East = Room.East;
            West = Room.West;
            Up = Room.Up;
            Down = Room.Down;

            // Updates the locally stored variables that allow ALL rooms to be visible, including ones behind closed doors
            // This doesn't affect how the rooms are stored in the save file, viewing from the game still prevents travel
            CheckForDoors(Room);

            // Update Description
            TbDescription.Text = Room.Description;

            UpdateButtons(Room);

            // Update LvItemsList
            LvItemsList.Items.Clear();
            List<Item> items = Room.GetItems(Saves.Items);
            foreach (Item item in items)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Content = item.Name;
                lvi.Tag = item.ID;

                LvItemsList.Items.Add(lvi);
            }

            GrdDetails.Visibility = Visibility.Visible;
        }

        private void LvRoomsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvRoomsList.SelectedItems.Count == 1)
            {
                UpdateGrdDetails(Saves.Places.GetIndexFromID(Convert.ToInt16(LvRoomsList.SelectedItems[0])));
            }
            else
            {
                GrdDetails.Visibility = Visibility.Hidden;
            }
        }
    }
}
