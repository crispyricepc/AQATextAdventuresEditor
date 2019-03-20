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
            UcDoorMenu.OnDoorCreation += new EventHandler(UcDoorMenu_Closed);
        }
        
        private void UpdateList()
        {
            LvRoomsList.Items.Clear();

            foreach (Place place in Saves.Places)
            {
                LvRoomsList.Items.Add(place.id.ToString());
            }
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

        private void BtnNewDoor_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            LocationDirection direction;
            int targetid;

            switch (btn.Name)
            {
                case "BtnNorthDoor":
                    direction = LocationDirection.North;
                    targetid = North;
                    break;
                case "BtnSouthDoor":
                    direction = LocationDirection.South;
                    targetid = South;
                    break;
                case "BtnEastDoor":
                    direction = LocationDirection.East;
                    targetid = East;
                    break;
                case "BtnWestDoor":
                    direction = LocationDirection.West;
                    targetid = West;
                    break;
                case "BtnUpDoor":
                    direction = LocationDirection.Up;
                    targetid = Up;
                    break;
                case "BtnDownDoor":
                    direction = LocationDirection.Down;
                    targetid = Down;
                    break;
                default:
                    throw new Exception("Can't find button name");
            }

            UcDoorMenu.Visibility = Visibility.Collapsed;
            UcDoorMenu.RoomID = Room.id;
            UcDoorMenu.TargetRoomID = targetid;
            UcDoorMenu.PrimaryRoomDirection = direction;

            if (btn.Content.ToString().ToLower().Contains("create"))
            {
                LvItemsList.Visibility = Visibility.Collapsed;
                UcDoorMenu.Create = true;
                UcDoorMenu.Visibility = Visibility.Visible;
            }
            else if (btn.Content.ToString().ToLower().Contains("modify"))
            {
                LvItemsList.Visibility = Visibility.Collapsed;
                UcDoorMenu.Create = false;
                UcDoorMenu.PrimaryDoorID = Room.GetDoors()[direction].ID;
                Place targetRoom = Saves.Places.GetObjectFromID(targetid);
                UcDoorMenu.SecondaryDoorID = targetRoom.GetDoors()[direction.GetOpposite()].ID;
                UcDoorMenu.Visibility = Visibility.Visible;
            }
        }
        private void UcDoorMenu_Closed(object sender, EventArgs e)
        {
            UcDoorMenu.Visibility = Visibility.Collapsed;
            LvItemsList.Visibility = Visibility.Visible;
            UpdateGrdDetails(LvRoomsList.SelectedIndex);
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

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GrdDetails.Visibility = Visibility.Hidden;
            UpdateList();
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

        /// <summary>
        /// Updates the locally stored variables that allow ALL rooms to be visible, including ones behind closed doors
        /// </summary>
        /// <param name="room"></param>
        private void CheckForDoors(ref Place room)
        {
            foreach (Item item in room.GetItems())
            {
                int doorCounterpart = item.GetDoorCounterpart(Saves.Items);

                if (doorCounterpart != -1) // if item is a door
                {
                    List<string[]> itemResults = item.GetResults();

                    switch (LocalConvert.ToLocationDirection(itemResults[0][0]))
                    {
                        case LocationDirection.North:
                            room.NorthDoor = item.ID;
                            North = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case LocationDirection.East:
                            room.EastDoor = item.ID;
                            East = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case LocationDirection.South:
                            room.SouthDoor = item.ID;
                            South = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case LocationDirection.West:
                            room.WestDoor = item.ID;
                            West = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case LocationDirection.Up:
                            room.UpDoor = item.ID;
                            Up = Convert.ToInt16(itemResults[0][1]);
                            break;
                        case LocationDirection.Down:
                            room.DownDoor = item.ID;
                            Down = Convert.ToInt16(itemResults[0][1]);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Updates all the room related buttons, including displaying the correct text depending on doors etc.
        /// </summary>
        /// <param name="room"></param>
        private void UpdateButtons(Place room)
        {
            // North door
            if (North != 0)
            {
                BtnNorth.SetValue(Grid.ColumnSpanProperty, 1);
                BtnNorthDoor.Visibility = Visibility.Visible;
                BtnNorth.Content = "View North Room";
                if (Room.NorthDoor != 0)
                {
                    BtnNorthDoor.Content = "Modify North Door";
                }
                else
                {
                    BtnNorthDoor.Content = "Create North Door";
                }
            }
            else
            {
                BtnNorthDoor.Visibility = Visibility.Collapsed;
                BtnNorth.SetValue(Grid.ColumnSpanProperty, 2);
                BtnNorth.Content = "Create North Room";
            }

            // South door
            if (South != 0)
            {
                BtnSouth.SetValue(Grid.ColumnSpanProperty, 1);
                BtnSouthDoor.Visibility = Visibility.Visible;
                BtnSouth.Content = "View South Room";
                if (Room.SouthDoor != 0)
                {
                    BtnSouthDoor.Content = "Modify South Door";
                }
                else
                {
                    BtnSouthDoor.Content = "Create South Door";
                }
            }
            else
            {
                BtnSouthDoor.Visibility = Visibility.Collapsed;
                BtnSouth.SetValue(Grid.ColumnSpanProperty, 2);
                BtnSouth.Content = "Create South Room";
            }
                  
            // East door
            if (East != 0)
            {
                BtnEast.SetValue(Grid.ColumnSpanProperty, 1);
                BtnEastDoor.Visibility = Visibility.Visible;
                BtnEast.Content = "View East Room";
                if (Room.EastDoor != 0)
                {
                    BtnEastDoor.Content = "Modify East Door";
                }
                else
                {
                    BtnEastDoor.Content = "Create East Door";
                }
            }
            else
            {
                BtnEastDoor.Visibility = Visibility.Collapsed;
                BtnEast.SetValue(Grid.ColumnSpanProperty, 2);
                BtnEast.Content = "Create East Room";
            }

            // West door
            if (West != 0)
            {
                BtnWest.SetValue(Grid.ColumnSpanProperty, 1);
                BtnWestDoor.Visibility = Visibility.Visible;
                BtnWest.Content = "View West Room";
                if (room.WestDoor != 0)
                {
                    BtnWestDoor.Content = "Modify West Door";
                }
                else
                {
                    BtnWestDoor.Content = "Create West Door";
                }
            }
            else
            {
                BtnWestDoor.Visibility = Visibility.Collapsed;
                BtnWest.SetValue(Grid.ColumnSpanProperty, 2);
                BtnWest.Content = "Create West Room";
            }

            // Up door
            if (Up != 0)
            {
                BtnUp.SetValue(Grid.ColumnSpanProperty, 1);
                BtnUpDoor.Visibility = Visibility.Visible;
                BtnUp.Content = "View Above Room";
                if (room.UpDoor != 0)
                {
                    BtnUpDoor.Content = "Modify Above Door";
                }
                else
                {
                    BtnUpDoor.Content = "Create Above Door";
                }
            }
            else
            {
                BtnUpDoor.Visibility = Visibility.Collapsed;
                BtnUp.SetValue(Grid.ColumnSpanProperty, 2);
                BtnUp.Content = "Create Above Room";
            }

            // Down door
            if (Down != 0)
            {
                BtnDown.SetValue(Grid.ColumnSpanProperty, 1);
                BtnDownDoor.Visibility = Visibility.Visible;
                BtnDown.Content = "View Below Room";
                if (room.DownDoor != 0)
                {
                    BtnDownDoor.Content = "Modify Below Door";
                }
                else
                {
                    BtnDownDoor.Content = "Create Below Door";
                }
            }
            else
            {
                BtnDownDoor.Visibility = Visibility.Collapsed;
                BtnDown.SetValue(Grid.ColumnSpanProperty, 2);
                BtnDown.Content = "Create Below Room";
            }
        }

        /// <summary>
        /// Updates all of the controls in GrdDetails to reflect the newly selected room
        /// </summary>
        /// <param name="placeIndex"></param>
        private void UpdateGrdDetails(int placeIndex)
        {
            Room = Saves.Places[placeIndex];
            North = Room.North;
            South = Room.South;
            East = Room.East;
            West = Room.West;
            Up = Room.Up;
            Down = Room.Down;

            CheckForDoors(ref Room);

            // Update Description
            TbDescription.Text = Room.Description;

            UpdateButtons(Room);

            // Update LvItemsList
            LvItemsList.Items.Clear();
            List<Item> items = Room.GetItems();
            foreach (Item item in items)
            {
                if (item.GetDoorCounterpart(Saves.Items) == -1)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Content = item.Name;
                    lvi.Tag = item.ID;

                    LvItemsList.Items.Add(lvi);
                }
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
