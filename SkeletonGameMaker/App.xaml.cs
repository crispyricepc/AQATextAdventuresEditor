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
        MainWindow WindowMain;

        /// <summary>
        /// Ensures the correct files exist in AppData before continuing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">This is passed into SelectFile</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WindowMain = new MainWindow();
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Saves.ApplicationDataPath = Path.Combine(appDataPath, @"SkeletonGameMaker\");
            if (!Directory.Exists(Saves.ApplicationDataPath))
            {
                Directory.CreateDirectory(Saves.ApplicationDataPath);
            }
            SelectFile(e);
        }

        public void ShowException(string miniMessage, Exception ex)
        {
            WindowMain.SbMain.MessageQueue.Enqueue(miniMessage, "VIEW DETAILS", () => MessageBox.Show(ex.Message));
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

            WindowMain.StartUp(fileSelected, createNew);

            Current.Shutdown();
        }
    }
}
