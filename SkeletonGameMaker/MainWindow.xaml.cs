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
using MaterialDesignThemes.Wpf;

namespace SkeletonGameMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RbFile.IsChecked = true;
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            UcMainMenu.OnBtnSaveClick += new EventHandler(MainMenuBtnSave_Click);

            UcRoomsMenu.OnBtnNewItemClick += new EventHandler(RoomsMenuBtnCreateNewItem_Click);
            UcRoomsMenu.OnSelectItemClick += new EventHandler(RoomsMenuBtnViewItem_Click);
        }

        /// <summary>
        /// Hides all controls except the top menu bar
        /// </summary>
        private void HideAll()
        {
            UcMainMenu.Visibility = Visibility.Collapsed;
            UcRoomsMenu.Visibility = Visibility.Collapsed;
            UcItemsMenu.Visibility = Visibility.Collapsed;
            UcCharactersMenu.Visibility = Visibility.Collapsed;
        }

        private void RoomsMenuBtnCreateNewItem_Click(object sender, EventArgs e)
        {
            HideAll();
            UcItemsMenu.Visibility = Visibility.Visible;
            RoomsMenu uc = sender as RoomsMenu;
            UcItemsMenu.ChangeSelection(0, uc.Room);
        }

        private void RoomsMenuBtnViewItem_Click(object sender, EventArgs e)
        {
            HideAll();
            UcItemsMenu.Visibility = Visibility.Visible;
            RoomsMenu uc = sender as RoomsMenu;
            UcItemsMenu.ChangeSelection(uc.itemIDSelected, uc.Room);
        }

        private void MainMenuBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Saves.MakeGame(Saves.Filename);
                MessageBox.Show("Game successfully saved to " + Saves.Filename, "Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't save game correctly\n\n" + ex, "Failed");
            }
        }

        private void WindowMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                MainMenuBtnSave_Click(sender, EventArgs.Empty);
            }
        }

        private void RbRooms_Checked(object sender, RoutedEventArgs e)
        {
            HideAll();
            UcRoomsMenu.Visibility = Visibility.Visible;
        }

        private void RbItems_Checked(object sender, RoutedEventArgs e)
        {
            HideAll();
            UcItemsMenu.Visibility = Visibility.Visible;
        }

        private void RbCharacters_Checked(object sender, RoutedEventArgs e)
        {
            HideAll();
            UcCharactersMenu.Visibility = Visibility.Visible;
        }

        private void RbFile_Checked(object sender, RoutedEventArgs e)
        {
            HideAll();
            UcMainMenu.Visibility = Visibility.Visible;
        }
    }
}
