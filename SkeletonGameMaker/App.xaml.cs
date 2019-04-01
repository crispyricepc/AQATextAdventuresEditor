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
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Saves.ApplicationDataPath = Path.Combine(appDataPath, @"test\");
            if (!Directory.Exists(Saves.ApplicationDataPath))
                Directory.CreateDirectory(Saves.ApplicationDataPath);

            bool closed = false;
            do
            {
                MainWindow window = new MainWindow();
                if (e.Args.Length > 0)
                {
                    Saves.Filename = e.Args[0];
                }
                else
                {
                    GetFileName getFileName = new GetFileName();
                    getFileName.ShowDialog();
                }

                try
                {
                    Saves.LoadGame(Saves.Filename, Saves.Characters, Saves.Items, Saves.Places);
                    window.ShowDialog();
                    closed = true;
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
