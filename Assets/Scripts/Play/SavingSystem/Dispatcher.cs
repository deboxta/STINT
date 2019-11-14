using System;
using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class Dispatcher : MonoBehaviour
    {
        private Player player;
        private DataCollector dataCollector;
        private SavedDataLoadedEventChannel savedDataLoadedEventChannel;
        private LevelController levelController;

        public DataCollector DataCollector
        {
            get => dataCollector;
            set => dataCollector = value;
        }

        private void Awake()
        {
            savedDataLoadedEventChannel = Finder.SavedDataLoadedEventChannel;
            dataCollector = new DataCollector();
            levelController = Finder.LevelController;
        }

        public void GetData()
        {
            player = GameObject.FindWithTag(R.S.Tag.Player).GetComponent<Player>();
            dataCollector.PositionX = GetPlayer().Item1;
            dataCollector.PositionY = GetPlayer().Item2;
            dataCollector.ActiveScene = GetScene();
        }

        public void SetData()
        {
            SetScene();
        }

        private (float,float) GetPlayer()
        {
            var position = player.transform.position;
            return (position.x, position.y);
        }

        private void SetScene()
        {
            savedDataLoadedEventChannel.NotifySavedDataLoaded();
        }

        private int GetScene()
        {
            return levelController.CurrentLevel;
        }
    }
}