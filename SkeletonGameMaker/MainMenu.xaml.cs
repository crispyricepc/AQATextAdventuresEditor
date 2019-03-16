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

        public MainMenu()
        {
            InitializeComponent();
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
    }
}
