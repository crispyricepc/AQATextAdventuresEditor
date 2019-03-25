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
        public CharactersMenu()
        {
            InitializeComponent();
        }

        private void UpdateCharacters()
        {
            foreach (Character character in Saves.Characters)
            {
                LvCharacters.Items.Add(character.Name);
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                UpdateCharacters();
            }
        }
    }
}
