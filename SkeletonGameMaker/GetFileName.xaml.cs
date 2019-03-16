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
using System.Windows.Shapes;

namespace SkeletonGameMaker
{
    /// <summary>
    /// Interaction logic for GetFileName.xaml
    /// </summary>
    public partial class GetFileName : Window
    {
        public GetFileName()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Saves.Filename = TbFilename.Text + ".gme";
            Close();
        }
    }
}
