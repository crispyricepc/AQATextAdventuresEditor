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
using Microsoft.Win32;

namespace SkeletonGameMaker
{
    /// <summary>
    /// Interaction logic for GetFileName.xaml
    /// </summary>
    public partial class GetFileName : Window
    {
        public bool FileSelected;

        public GetFileName()
        {
            InitializeComponent();

            FileSelected = false;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Skeleton Game Files (*.gme)|*.gme";
            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName != null)
                {
                    FileSelected = true;
                    Saves.Filename = ofd.FileName;
                    Close();
                }
            }
        }
    }
}
