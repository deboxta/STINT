//Author : Yannick Cote

using Harmony;

namespace Game
{
    public class Model : IModel
    {
        //BETA : implement the Model once the serialization saving system is implemented.
        private SaveSystem saveSystem;
        private Dispatcher dispatcher;

        private Model()
        {
            dispatcher = Finder.Dispatcher;
        }
        public void CreateNewGameFile(string name)
        {
            dispatcher.DataCollector.Name = name;
            saveSystem.SaveGame();
        }
    }
}