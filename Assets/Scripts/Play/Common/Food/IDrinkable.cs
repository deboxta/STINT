namespace Game
{
    public interface IDrinkable : IEntity
    {
        IEffect Drink();
    }
}