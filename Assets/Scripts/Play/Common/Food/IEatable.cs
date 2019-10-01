namespace Game
{
    public interface IEatable : IEntity
    {
        bool IsEatable { get; }

        IEffect BeEaten();
    }
}