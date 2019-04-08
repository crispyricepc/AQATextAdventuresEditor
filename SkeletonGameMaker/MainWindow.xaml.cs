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
using System.IO;
using Microsoft.Win32;
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

        private void OpenGame()
        {
            try
            {
                Saves.LoadGame(Saves.Filename, Saves.Characters, Saves.Items, Saves.Places);
                Saves.AddToRecents(new GameFileData(Saves.Filename, System.IO.Path.GetFileName(Saves.Filename), DateTime.Now));
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("File not found\n" + ex.Message, "Error");
            }
        }

        public void StartUp(bool fileSelected, bool createNew)
        {
            if (fileSelected)
            {
                OpenGame();
            }

            ShowDialog();
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            UcMainMenu.OnBtnSaveClick += new EventHandler(MainMenuBtnSave_Click);
            UcMainMenu.OnBtnNewClick += new EventHandler(MainMenuBtnNew_Click);
            UcMainMenu.OnLvRecentsClick += new EventHandler(MainMenuLvRecents_Click);
            UcMainMenu.OnBtnLoadClick += new EventHandler(MainMenuBtnLoad_Click);

            UcRoomsMenu.OnBtnNewItemClick += new EventHandler(RoomsMenuBtnCreateNewItem_Click);
            UcRoomsMenu.OnSelectItemClick += new EventHandler(RoomsMenuBtnViewItem_Click);
        }

        private void MainMenuBtnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Skeleton Game Files (*.gme)|*.gme";
            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName != null)
                {
                    Saves.Filename = ofd.FileName;
                    OpenGame();
                }
            }
        }

        private void MainMenuLvRecents_Click(object sender, EventArgs e)
        {
            OpenGame();
        }

        private void MainMenuBtnNew_Click(object sender, EventArgs e)
        {
            Saves.Filename = "newgme.tmp";
            Saves.Characters = new List<Character>();
            Saves.Items = new List<Item>();
            Saves.Places = new List<Place>();

            Place startRoom = new Place();
            startRoom.id = Saves.FindFreeID(1, 1000, Saves.Places.GetIDs());
            startRoom.Description = "A new room";
            startRoom.North = 0;
            startRoom.South = 0;
            startRoom.East = 0;
            startRoom.West = 0;
            startRoom.Up = 0;
            startRoom.Down = 0;
            Saves.Places.Add(startRoom);

            OpenGame();
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
            RbItems.IsChecked = true;
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

        private void WindowMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
