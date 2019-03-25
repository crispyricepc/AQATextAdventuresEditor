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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            UcMainMenu.OnBtnRoomsClick += new EventHandler(MainMenuBtnRooms_Click);
            UcMainMenu.OnBtnCharactersClick += new EventHandler(MainMenuBtnCharacters_Click);
            UcMainMenu.OnBtnItemsClick += new EventHandler(MainMenuBtnItems_Click);
            UcMainMenu.OnBtnSaveClick += new EventHandler(MainMenuBtnSave_Click);
            UcMainMenu.OnBtnLoadClick += new EventHandler(MainMenuBtnLoad_Click);

            UcRoomsMenu.OnBtnNewItemClick += new EventHandler(RoomsMenuBtnCreateNewItem_Click);
            UcRoomsMenu.OnSelectItemClick += new EventHandler(RoomsMenuBtnViewItem_Click);
        }

        private void HideAll()
        {
            UcMainMenu.Visibility = Visibility.Collapsed;
            UcRoomsMenu.Visibility = Visibility.Collapsed;
            UcItemsMenu.Visibility = Visibility.Collapsed;
            UcCharactersMenu.Visibility = Visibility.Collapsed;
        }

        private void MainMenuBtnRooms_Click(object sender, EventArgs e)
        {
            HideAll();
            UcRoomsMenu.Visibility = Visibility.Visible;
        }

        private void MainMenuBtnCharacters_Click(object sender, EventArgs e)
        {
            HideAll();
            UcCharactersMenu.Visibility = Visibility.Visible;
        }

        private void MainMenuBtnItems_Click(object sender, EventArgs e)
        {
            HideAll();
            UcItemsMenu.Visibility = Visibility.Visible;
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

        private void MainMenuBtnLoad_Click(object sender, EventArgs e)
        {
            GetFileName getFileName = new GetFileName();
            getFileName.ShowDialog();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            UcMainMenu.Visibility = Visibility.Visible;
        }

        private void WindowMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                MainMenuBtnSave_Click(sender, EventArgs.Empty);
            }
        }
    }
}
