using System;

namespace Game
{
    [Serializable]
    public class DataCollector
    {
        private int? activeScene;
        private float positionX, positionY;
        private int nbDeath;
        private string name;
        private int cameraConfinerIndex;
        private bool firstDeath;
        private bool wonGame;
        private bool wonWithoutDying;
        private bool saveNamedBen;
        private bool secretRoomFound;

        public int? ActiveScene
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

        public int CameraConfinerIndex
        {
            get => cameraConfinerIndex;
            set => cameraConfinerIndex = value;
        }
        
        public bool FirstDeath
        {
            get => firstDeath;
            set => firstDeath = value;
        }
        
        public bool WonWithoutDying
        {
            get => wonWithoutDying;
            set => wonWithoutDying = value;
        }
        
        public bool WonGame
        {
            get => wonGame;
            set => wonGame = value;
        }

        public bool SaveNamedBen
        {
            get => saveNamedBen;
            set => saveNamedBen = value;
        }
        
        public bool SecretRoomFound
        {
            get => secretRoomFound;
            set => secretRoomFound = value;
        }
    }
}