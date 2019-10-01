namespace Game
{
    public class FoxOffspringCreator : OffspringCreator
    {
        protected override Animal CreateOffspringPrefab(Animal otherAnimal)
        {
            return PrefabFactory.CreateFox(Animal.Position, FaunaRoot);
        }
    }
}