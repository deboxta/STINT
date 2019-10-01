using UnityEngine;
using Harmony;
using Random = System.Random;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class RandomSeed : MonoBehaviour
    {
        [SerializeField] private bool useRandomSeed = true;
        [SerializeField] private int initialSeed = 0;

        private int? seed = null;

        public int Seed
        {
            get
            {
                if (!seed.HasValue) seed = useRandomSeed ? UnityEngine.Random.Range(int.MinValue, int.MaxValue) : initialSeed;
                return seed.Value;
            }
        }

        public Random CreateRandom()
        {
            return new Random(Seed);
        }
    }
}