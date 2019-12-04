using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Yannick Cote
    [Findable(R.S.Tag.MainController)]

    public class SaveSystem : MonoBehaviour
    {
        private int nbOfSaves;
        private DataCollector localData;
        private Dispatcher dispatcher;
        private const string SAVE_FOLDER_NAME = "Saves/";
        private const string SAVE_FILE_EXT = ".binary";
        private bool isGameSaved;

        public int NbOfSaves => nbOfSaves;
        public bool IsGameSaved => isGameSaved;


        private void Awake()
        {
            dispatcher = Finder.Dispatcher;
            isGameSaved = false;
        }

        public List<DataCollector> GetSaves()
        {
            List<DataCollector> dataCollectors = new List<DataCollector>();
            BinaryFormatter formatter = new BinaryFormatter();
            List<FileStream> filesList = new List<FileStream>();
            var filesNames = Directory.GetFiles(SAVE_FOLDER_NAME).Where(file => !file.ToLower().Contains("desktop.ini")); //Inspired from : nerdshark https://www.reddit.com/r/csharp/comments/7uulwg/get_all_items_from_desktop_except_desktopini_am_i/
            nbOfSaves = 0;
            
            foreach (var filesName in filesNames)
            {
                filesList.Add(File.Open(filesName, FileMode.Open));
                nbOfSaves++;
            }

            foreach (var file in filesList)
            {
                localData = (DataCollector) formatter.Deserialize(file);
                dataCollectors.Add(localData);
                file.Close();
            }
            
            return dataCollectors;
        }

        public void DeleteSave(string saveToDelete)
        {
            var filesNames = Directory.GetFiles(SAVE_FOLDER_NAME).Where(file => !file.ToLower().Contains("desktop.ini")); //Inspired from : nerdshark https://www.reddit.com/r/csharp/comments/7uulwg/get_all_items_from_desktop_except_desktopini_am_i/
            foreach (var filesName in filesNames)
            {
                if (filesName == SAVE_FOLDER_NAME+saveToDelete+SAVE_FILE_EXT)
                {
                    File.Delete(filesName);
                    nbOfSaves--;
                }
            }
        }

        public void SaveGame()
        {
            if (!Directory.Exists(SAVE_FOLDER_NAME))
                Directory.CreateDirectory(SAVE_FOLDER_NAME);
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Create(SAVE_FOLDER_NAME + dispatcher.DataCollector.Name + SAVE_FILE_EXT);

            dispatcher.GetData();
            
            localData = dispatcher.DataCollector;
            
            formatter.Serialize(saveFile, localData);

            isGameSaved = true;
            
            saveFile.Close();
        }

        public void LoadData(string saveName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(SAVE_FOLDER_NAME + saveName + SAVE_FILE_EXT, FileMode.Open);

            localData = (DataCollector) formatter.Deserialize(saveFile);

            dispatcher.DataCollector = localData;
            dispatcher.SetData();

            saveFile.Close();
        }
    }
}