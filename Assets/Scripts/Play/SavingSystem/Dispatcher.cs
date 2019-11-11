using System;
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]

    public class Dispatcher : MonoBehaviour
    {
        private Player player;
        private DataCollector dataCollector;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private LevelController levelController;

        public DataCollector DataCollector
        {
            get => dataCollector;
            set => dataCollector = value;
        }

        private void Awake()
        {
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
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
            player = GameObject.FindWithTag(R.S.Tag.Player).GetComponent<Player>();

            SetScene();

            SetPlayer();
        }

        private void SetPlayer()
        {
            player.transform.position = new Vector3(dataCollector.PositionX,dataCollector.PositionY);
        }
        private (float,float) GetPlayer()
        {
            var position = player.transform.position;
            return (position.x, position.y);
        }

        private void SetScene()
        {
            levelController.LevelToLoad = dataCollector.ActiveScene;
            levelCompletedEventChannel.NotifyLevelCompleted();
        }

        private int GetScene()
        {
            return levelController.CurrentLevel;
        }
    }
}