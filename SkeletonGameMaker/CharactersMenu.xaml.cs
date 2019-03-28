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
    /// Interaction logic for CharactersMenu.xaml
    /// </summary>
    public partial class CharactersMenu : UserControl
    {
        private Character CharacterSelected;

        public CharactersMenu()
        {
            InitializeComponent();
        }

        private void UpdateCharacters()
        {            
            LvCharacters.Items.Clear();
            foreach (Character character in Saves.Characters)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Content = character.Name;
                lvi.Tag = character.ID;
                LvCharacters.Items.Add(lvi);
            }

            CbLocation.Items.Clear();
            foreach (Place room in Saves.Places)
            {
                CbLocation.Items.Add(room.id);
            }
        }

        private void UpdateCharacterDetails()
        {
            TbName.Text = CharacterSelected.Name;
            TbDescription.Text = CharacterSelected.Description;
            CbLocation.SelectedItem = CharacterSelected.CurrentLocation;
            UpdateCharacterInventoryDetails();
        }

        private void UpdateCharacterInventoryDetails()
        {
            LvInventory.Items.Clear();
            foreach (Item thing in Saves.Items)
            {
                if (thing.Location == CharacterSelected.ID)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Content = thing.Name;
                    lvi.Tag = thing.ID;
                    LvInventory.Items.Add(lvi);
                }
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                UpdateCharacters();
            }
        }

        private void LvCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvCharacters.SelectedItems.Count != 0)
            {
                int id = Convert.ToInt16(((ListViewItem)LvCharacters.SelectedItem).Tag);
                CharacterSelected = Saves.Characters.GetObjectFromID(id);
                UpdateCharacterDetails();
                GrdDetails.Visibility = Visibility.Visible;
            }
            else
            {
                CharacterSelected = null;
                GrdDetails.Visibility = Visibility.Collapsed;
            }
        }
    }
}
