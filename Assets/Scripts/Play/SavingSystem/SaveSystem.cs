using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Harmony;
using UnityEngine;

namespace Game
{
    public class SaveSystem : MonoBehaviour
    {
        private DataCollector localData;
        private Dispatcher dispatcher;
        [SerializeField] private bool yolo = false;
        private const string SAVE_FOLDER_NAME = "Saves/";
        private const string SAVE_FILE_NAME = "save.binary";

        private void Awake()
        {
            dispatcher = Finder.Dispatcher;
        }

        public void SaveGame()
        {
            if (!Directory.Exists(SAVE_FOLDER_NAME))
                Directory.CreateDirectory(SAVE_FOLDER_NAME);
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Create(SAVE_FOLDER_NAME + SAVE_FILE_NAME);

            dispatcher.GetData();
            
            localData = dispatcher.DataCollector;
            
            formatter.Serialize(saveFile, localData);
            
            saveFile.Close();
        }

        private void Update()
        {
            if (yolo)
            {
                LoadData();
                yolo = false;
            }
        }

        public void LoadData()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(SAVE_FOLDER_NAME + SAVE_FILE_NAME, FileMode.Open);

            localData = (DataCollector) formatter.Deserialize(saveFile);

            dispatcher.DataCollector = localData;
            dispatcher.SetData();

            saveFile.Close();
        }
    }
}