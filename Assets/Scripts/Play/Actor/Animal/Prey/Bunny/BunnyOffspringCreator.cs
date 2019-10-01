namespace Game
{
    public class BunnyOffspringCreator : OffspringCreator
    {
        protected override Animal CreateOffspringPrefab(Animal otherAnimal)
        {
            return PrefabFactory.CreateBunny(Animal.Position, FaunaRoot);
        }
    }
}