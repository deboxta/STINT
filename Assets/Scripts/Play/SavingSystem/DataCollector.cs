using System;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [Serializable]
    public class DataCollector
    {
        private int activeScene;
        private float positionX, positionY;
        private int nbDeath;
        private string name;

        public int ActiveScene
        {
            get => activeScene;
            set => activeScene = value;
        }

        public float PositionX
        {
            get => positionX;
            set => positionX = value;
        }

        public float PositionY
        {
            get => positionY;
            set => positionY = value;
        }

        public int NbDeath
        {
            get => nbDeath;
            set => nbDeath = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }
    }
}