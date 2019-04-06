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
            Saves.ApplicationDataPath = Path.Combine(appDataPath, @"SkeletonGameMaker\");
            if (!Directory.Exists(Saves.ApplicationDataPath))
            {
                Directory.CreateDirectory(Saves.ApplicationDataPath);
            }
            SelectFile(e);
        }

        public void SelectFile(StartupEventArgs e)
        {
            bool closed = true;
            do
            {
                bool fileSelected = false;
                bool createNew = false;

                MainWindow window = new MainWindow();
                if (e.Args.Length > 0)
                {
                    Saves.Filename = e.Args[0];
                    fileSelected = true;
                }
                else
                {
                    GetFileName getFileName = new GetFileName();
                    getFileName.ShowDialog();
                    fileSelected = getFileName.FileSelected;
                    createNew = getFileName.CreateNew;
                }

                if (fileSelected)
                {
                    try
                    {
                        Saves.LoadGame(Saves.Filename, Saves.Characters, Saves.Items, Saves.Places);
                        Saves.AddToRecents(new GameFileData(Saves.Filename, Path.GetFileName(Saves.Filename), DateTime.Now));
                        window.ShowDialog();
                        closed = true;
                    }
                    catch (FileNotFoundException ex)
                    {
                        MessageBox.Show("File not found\n" + ex.Message, "Error");
                    }
                }
                else if (createNew)
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

                    window.ShowDialog();
                    closed = true;
                }
            }
            while (!closed);

            Current.Shutdown();
        }
    }
}
