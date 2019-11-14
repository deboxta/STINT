namespace Game
{
    public interface IFreezable
    {
        //BC : Je trouve étrange que l'interface n'aie pas de méthode "Freeze" ou "Unfreeze".
        //     Il y a seulement une propriété qui indique si l'élément est "Frozen" ou non.
        //     Mal conçu.
        bool Frozen { get; }
    }
}