using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace SkeletonGameMaker
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool closed = false;
            do
            {
                if (e.Args.Length > 0)
                {
                    Saves.Filename = e.Args[0];
                }
                else
                {
                    GetFileName getFileName = new GetFileName();
                    getFileName.ShowDialog();
                }
                MainWindow window = new MainWindow();

                try
                {
                    Saves.LoadGame(Saves.Filename, Saves.Characters, Saves.Items, Saves.Places);
                    window.ShowDialog();
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show("File not found\n" + ex.Message, "Error");
                }
            }
            while (!closed);
        }
    }
}
