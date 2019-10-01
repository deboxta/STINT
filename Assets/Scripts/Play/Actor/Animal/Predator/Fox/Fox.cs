namespace Game
{
    public class Fox : Animal, IPredator
    {
        private ISensor<IEatable> foodSensor;
        private ISensor<IDrinkable> drinkSensor;
        private ISensor<IEntity> threatSensor;
        private ISensor<Animal> mateSensor;

        public override ISensor<IEatable> FoodSensor => foodSensor;
        public override ISensor<IDrinkable> DrinkSensor => drinkSensor;
        public override ISensor<IEntity> ThreatSensor => threatSensor;
        public override ISensor<Animal> MateSensor => mateSensor;

        private new void Awake()
        {
            base.Awake();

            var sensor = Sensor;
            foodSensor = sensor.For<IPrey>();
            drinkSensor = sensor.For<IDrinkable>();
            threatSensor = sensor.ForNothing<IEntity>();
            mateSensor = sensor.For<Fox>();
        }
    }
}