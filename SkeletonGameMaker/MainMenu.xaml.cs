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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public event EventHandler OnBtnRoomsClick;
        public event EventHandler OnBtnCharactersClick;
        public event EventHandler OnBtnItemsClick;
        public event EventHandler OnBtnSaveClick;
        public event EventHandler OnBtnLoadClick;
        public event EventHandler OnBtnNewClick;
        public event EventHandler OnLvRecentsClick;

        public bool FileSelected;
        public bool CreateNew;

        public MainMenu()
        {
            InitializeComponent();

            FileSelected = false;
            CreateNew = false;
        }

        private void BtnRooms_Click(object sender, RoutedEventArgs e)
        {
            OnBtnRoomsClick?.Invoke(this, EventArgs.Empty);
        }

        private void BtnCharacters_Click(object sender, RoutedEventArgs e)
        {
            OnBtnCharactersClick?.Invoke(this, EventArgs.Empty);
        }

        private void BtnItems_Click(object sender, RoutedEventArgs e)
        {
            OnBtnItemsClick?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            OnBtnSaveClick?.Invoke(this, EventArgs.Empty);
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OnBtnLoadClick?.Invoke(this, EventArgs.Empty);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ColName.Width = (LvRecents.ActualWidth - 40) / 2;
            ColLastEdited.Width = (LvRecents.ActualWidth - 40) / 2;
        }
        private void LvRecents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvRecents.SelectedItems.Count == 1)
            {
                FileSelected = true;
                Saves.Filename = ((GameFileData)LvRecents.SelectedItem).Path;
                OnLvRecentsClick?.Invoke(this, EventArgs.Empty);
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            CreateNew = true;
            OnBtnNewClick?.Invoke(this, EventArgs.Empty);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            List<GameFileData> gfd = Saves.GetRecents();
            LvRecents.ItemsSource = gfd;
        }
    }
}
