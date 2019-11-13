using System;
using System.Collections.Concurrent;


namespace Game
{
    // Author : not Mathieu Boutet
    // https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool
    public class ObjectPool<T>
    {
        private ConcurrentBag<T> objects;
        private Func<T> objectGenerator;

        public ObjectPool(Func<T> objectGenerator, int nbObjects = 0)
        {
            if (objectGenerator == null) throw new ArgumentNullException(nameof(objectGenerator));
            objects = new ConcurrentBag<T>();
            this.objectGenerator = objectGenerator;
            for (int i = 0; i < nbObjects; i++)
            {
                CreateObject();
            }
        }

        public T GetObject()
        {
            T item;
            if (objects.TryTake(out item)) return item;
            return objectGenerator();
        }

        public void PutObject(T item)
        {
            objects.Add(item);
        }

        public void CreateObject()
        {
            PutObject(objectGenerator());
        }
    }
}