using DG.Tweening;
using Harmony;
using UnityEngine;

namespace Game
{
    public class MainController : MonoBehaviour
    {
        [Header("Configuration")] [SerializeField] private bool generateTerrain = true;
        [SerializeField] private bool generateFlora = true;
        [SerializeField] private bool generateWater = true;
        [SerializeField] private bool generateNavigationMesh = true;
        [SerializeField] private bool generateFauna = true;
        [SerializeField] [Range(0f, 10f)] private float minTimeScale = 0;
        [SerializeField] [Range(0f, 10f)] private float maxTimeScale = 10;
        [SerializeField] [Range(0f, 10f)] private float timeScaleIncrement = 1;
        [Header("Key Mapping")] [SerializeField] private KeyCode timeScaleUpKey = KeyCode.KeypadPlus;
        [SerializeField] private KeyCode timeScaleDownKey = KeyCode.KeypadMinus;

        private TerrainGrid terrain;
        private TerrainGridGenerator terrainGenerator;
        private FloraGrid flora;
        private FloraGridGenerator floraGridGenerator;
        private WaterGenerator waterGenerator;
        private NavigationMesh navigationMesh;
        private NavigationMeshGenerator navigationMeshGenerator;
        private FaunaGenerator faunaGenerator;

        private void Awake()
        {
            terrain = Finder.TerrainGrid;
            terrainGenerator = Finder.TerrainGridGenerator;
            flora = Finder.FloraGrid;
            floraGridGenerator = Finder.FloraGridGenerator;
            waterGenerator = Finder.WaterGenerator;
            navigationMesh = Finder.NavigationMesh;
            navigationMeshGenerator = Finder.NavigationMeshGenerator;
            faunaGenerator = Finder.FaunaGenerator;

            DOTween.Init(false, false, LogBehaviour.ErrorsOnly);
            DOTween.SetTweensCapacity(200, 125);
        }

        private void Start()
        {
            GenerateWorld();
        }

        private void Update()
        {
            if (Input.GetKeyDown(timeScaleUpKey))
                Time.timeScale = Mathf.Clamp(Time.timeScale + timeScaleIncrement, minTimeScale, maxTimeScale);
            if (Input.GetKeyDown(timeScaleDownKey))
                Time.timeScale = Mathf.Clamp(Time.timeScale - timeScaleIncrement, minTimeScale, maxTimeScale);
        }

        private void GenerateWorld()
        {
            if (generateTerrain) terrain.Blocks = terrainGenerator.Generate();
            if (generateFlora) flora.Blocks = floraGridGenerator.Generate();
            if (generateWater) waterGenerator.Generate();
            if (generateNavigationMesh) navigationMesh.Graph = navigationMeshGenerator.Generate();
            if (generateFauna) faunaGenerator.Generate();
        }

        private static void WriteToDatabase()
        {
            //Voici un exemple d'écriture sur la base de données en SQLite C#.
            //N'oubliez pas de respecter les consignes de l'énoncé. Par exemple, ce code est loin
            //d'être propre et il n'utilise pas le patron "Repository".
            var connection = Finder.SqLiteConnectionFactory.GetConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO user(name,age) VALUES(?,?)";

            var userParameter = command.CreateParameter();
            userParameter.Value = "John Smith";
            command.Parameters.Add(userParameter);

            var ageParameter = command.CreateParameter();
            ageParameter.Value = 42;
            command.Parameters.Add(ageParameter);

            command.ExecuteNonQuery();
            transaction.Commit();

            connection.Close();
        }
    }
}