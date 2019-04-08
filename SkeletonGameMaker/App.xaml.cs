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
        /// <summary>
        /// Ensures the correct files exist in AppData before continuing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">This is passed into SelectFile</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Saves.ApplicationDataPath = Path.Combine(appDataPath, @"SkeletonGameMaker\");
            if (!Directory.Exists(Saves.ApplicationDataPath))
            {
                Directory.CreateDirectory(Saves.ApplicationDataPath);
            }
            SelectFile(e);
        }

        /// <summary>
        /// Picks a file based on whether the program was launched from a .gme or from a .exe
        /// </summary>
        /// <param name="e">If empty, the GetFileName Window is shown</param>
        public void SelectFile(StartupEventArgs e)
        {
            bool fileSelected = false;
            bool createNew = false;

            if (e.Args.Length > 0)
            {
                Saves.Filename = e.Args[0];
                fileSelected = true;
            }

            MainWindow window = new MainWindow();
            window.StartUp(fileSelected, createNew);

            Current.Shutdown();
        }
    }
}
