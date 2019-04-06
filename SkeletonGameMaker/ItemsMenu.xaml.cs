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
    /// Interaction logic for ItemsMenu.xaml
    /// </summary>
    public partial class ItemsMenu : UserControl
    {
        Item ItemSelected;

        public ItemsMenu()
        {
            InitializeComponent();
        }

        private void UpdateItemList()
        {
            LvItemsList.Items.Clear();

            foreach (Item item in Saves.Items)
            {
                if (item.GetDoorCounterpart(Saves.Items) == -1)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Content = item.Name;
                    lvi.Tag = item.ID;

                    LvItemsList.Items.Add(lvi);
                }
            }
        }
        private void UpdateStatusList()
        {
            LvStatus.Items.Clear();

            List<string> statuslist = ItemSelected.GetStatus();
            foreach (string str in statuslist)
            {
                LvStatus.Items.Add(str);
            }

            UpdateCommandList();
        }
        private void UpdateCommandList()
        {
            GrdCommandResult.Visibility = Visibility.Hidden;

            LvCommands.Items.Clear();

            List<string> commandlist = ItemSelected.GetCommands();
            try
            {
                if (commandlist[0] != "")
                {
                    foreach (string str in commandlist)
                    {
                        LvCommands.Items.Add(str);
                    }
                }
            }
            catch
            {

            }
        }
        private void UpdateResults(string command)
        {
            GrdCommandResult.Visibility = Visibility.Visible;

            int index = ItemSelected.GetCommands().IndexOf(command);
            List<string[]> resultList = ItemSelected.GetResults();
            string[] result = resultList[index];
            int i = 0;
            foreach (ComboBoxItem cbi in CbResults.Items)
            {
                if (cbi.Content.ToString().ToLower() == result[0])
                {
                    CbResults.SelectedIndex = i;
                }
                i++;
            }
            TbResult.Text = result[1];
        }

        private void UpdateItemInventory()
        {
            LvContainerItems.Items.Clear();

            if (ItemSelected.GetStatus().Contains("container"))
            {
                foreach (Item item in Saves.Items)
                {
                    if (item.Location == ItemSelected.ID)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Content = item.Name;
                        lvi.Tag = item.ID;
                        LvContainerItems.Items.Add(lvi);
                    }
                }
            }
        }

        public void ChangeSelection(int itemId, Place place)
        {
            if (itemId == 0)
            {
                itemId = CreateNewItem(place);
            }
            for (int i = 0; i < LvItemsList.Items.Count; i++)
            {
                ListViewItem lvi = (ListViewItem)LvItemsList.Items[i];
                if (Convert.ToInt16(lvi.Tag) == itemId)
                {
                    LvItemsList.SelectedIndex = i;
                }
            }
        }

        private int CreateNewItem(Place place)
        {
            int newItemId = place.AddItem(-1, "new item", "A new item", new List<string>(), new List<string>(), new List<string[]>());
            UpdateItemList();
            return newItemId;
        }

        private void LvItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvItemsList.SelectedItems.Count == 1)
            {
                GrdDetails.Visibility = Visibility.Visible;

                foreach (Item item in Saves.Items)
                {
                    if (item.GetDoorCounterpart(Saves.Items) == -1)
                    {
                        ListViewItem lvi = (ListViewItem)LvItemsList.SelectedItem;
                        if (item.ID == Convert.ToInt16(lvi.Tag))
                        {
                            ItemSelected = item;
                        }
                    }
                }
                UpdateGrdDetails();
            }
            else GrdDetails.Visibility = Visibility.Hidden;
        }

        private void UpdateGrdDetails()
        {
            UpdateStatusList();
            UpdateCommandList();
            TbDescription.Text = ItemSelected.Description;
            TbName.Text = ItemSelected.Name;
            UpdateCbLocation();
            UpdateItemInventory();
        }

        private void UpdateCbLocation()
        {
            string loc;
            if (ItemSelected.Location > 1000 && ItemSelected.Location < 2000)
            {
                loc = Saves.Characters[Saves.Characters.GetIndexFromID(ItemSelected.Location)].Name;
            }
            else if (ItemSelected.Location > 2000 && ItemSelected.Location < 10000)
            {
                loc = Saves.Items.GetObjectFromID(ItemSelected.Location).Name;
            }
            else
            {
                loc = ItemSelected.Location.ToString();
            }
            CbLocation.Text = loc;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GrdDetails.Visibility = Visibility.Collapsed;
            UpdateItemList();
        }

        private void TbDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = Saves.Items.GetIndexFromID(ItemSelected.ID);
            Saves.Items[index].Description = TbDescription.Text;
        }

        private void TbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = Saves.Items.GetIndexFromID(ItemSelected.ID);
            Saves.Items[index].Name = TbName.Text;
        }

        /// <summary>
        /// Writes to the selected item any new statuses that are added, and adds the status to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem cbi = (ComboBoxItem)CbStatus.SelectedItem;
                string newStatus = cbi.Content.ToString();

                if (ItemSelected != null && CbStatus.SelectedIndex != 0)
                {
                    Saves.Items[Saves.Items.GetIndexFromID(ItemSelected.ID)].AddStatus(newStatus);

                    UpdateStatusList();
                    UpdateCommandList();
                }

                if (newStatus.ToLower() == "container")
                {
                    UpdateCbLocationItems();
                }
            }
            catch (InputException ex)
            {
                MessageBox.Show("The status update failed for the following reason\n\n" + ex.Message, "Failed");
                CbStatus.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Fills the combobox with all the valid containers, including rooms, items and characters
        /// </summary>
        private void UpdateCbLocationItems()
        {
            CbLocation.Items.Clear();
            foreach (Place room in Saves.Places)
            {
                CbLocation.Items.Add(room.id);
            }
            foreach (Item item in Saves.Items)
            {
                if (item.GetStatus().Contains("container"))
                {
                    CbLocation.Items.Add(item.Name);
                }
            }
            foreach (Character character in Saves.Characters)
            {
                CbLocation.Items.Add(character.Name);
            }
        }

        /// <summary>
        /// Executed when the items menu is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                UpdateItemList();
                UpdateCbLocationItems();
            }
        }

        private void LvStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvStatus.SelectedItems.Count == 1)
            {
                BtnDeleteStatus.Visibility = Visibility.Visible;
            }
            else
            {
                BtnDeleteStatus.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnDeleteStatus_Click(object sender, RoutedEventArgs e)
        {
            if (LvStatus.SelectedItems.Count == 1)
            {
                ItemSelected.RemoveStatus(LvStatus.SelectedItem.ToString());
                UpdateStatusList();
                UpdateCommandList();
            }
        }

        private void CbCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCommands.SelectedIndex == 0)
            {
                return;
            }
            try
            {
                ComboBoxItem cbi = (ComboBoxItem)CbCommands.SelectedItem;
                string newCommand = cbi.Content.ToString().ToLower();

                if (ItemSelected != null && CbCommands.SelectedIndex != 0)
                {
                    string[] tmpResult;
                    tmpResult = new string[2];
                    tmpResult[0] = "say";
                    tmpResult[1] = "A new command";

                    Saves.Items[Saves.Items.GetIndexFromID(ItemSelected.ID)].AddCommand(newCommand, tmpResult);

                    UpdateCommandList();
                    UpdateResults(newCommand);
                }
            }
            catch (InputException ex)
            {
                MessageBox.Show("The command update failed for the following reason\n\n" + ex.Message, "Failed");
                CbCommands.SelectedIndex = 0;
            }
        }

        private void LvCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GrdCommandResult.Visibility = Visibility.Hidden;

            if (LvCommands.SelectedItems.Count == 1)
            {
                UpdateResults(LvCommands.SelectedItem.ToString());
            }
        }

        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                Saves.Items.Remove(ItemSelected);
                MessageBox.Show("Successfully Deleted", "Complete");
            }

            UpdateItemList();
            LvItemsList.SelectedIndex = -1;
        }

        private void BtnDeleteCommand_Click(object sender, RoutedEventArgs e)
        {
            ItemSelected.RemoveCommand(LvCommands.SelectedItem.ToString());
            UpdateCommandList();
        }

        private void CbResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OverwriteResults();
        }

        private void TbResult_TextChanged(object sender, TextChangedEventArgs e)
        {
            OverwriteResults();
        }

        private void OverwriteResults()
        {
            try
            {
                if (ItemSelected != null && LvCommands.SelectedIndex != -1)
                {
                    ComboBoxItem cbi = (ComboBoxItem)CbResults.SelectedItem;
                    string[] newResult = new string[2] { cbi.Content.ToString().ToLower(), TbResult.Text };
                    ItemSelected.UpdateCommandResult(LvCommands.SelectedItem.ToString(), newResult);
                }
            }
            catch (InputException ex)
            {
                MessageBox.Show("The program failed to update the text in the result field for the following reason:\n\n" + ex.Message, "Failed");
            }
        }

        /// <summary>
        /// Saves the changes to Saves.Items of the Item's new location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbLocation.SelectedItem != null)
            {
                string location = CbLocation.SelectedItem.ToString();
                int locID;
                if (!int.TryParse(location, out locID))
                {
                    try
                    {
                        locID = Saves.Items.GetObjectFromName(CbLocation.SelectedItem.ToString()).ID;
                    }
                    catch
                    {
                        locID = Saves.Characters.GetObjectFromName(CbLocation.SelectedItem.ToString()).ID;
                    }
                }
                ItemSelected.Location = locID;
                Saves.Items[Saves.Items.GetIndexFromID(ItemSelected.ID)] = ItemSelected;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (GrdDetails.Visibility != Visibility.Collapsed)
            {
                GvcolID.Width = LvItemsList.ActualWidth - 40;
                GvColStatus.Width = LvStatus.ActualWidth - 40;
                GvColCommands.Width = LvCommands.ActualWidth - 40;
                GvColContainer.Width = LvContainerItems.ActualWidth - 40;
            }
        }
    }
}
