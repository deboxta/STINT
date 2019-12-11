using Cinemachine;
using Harmony;
using UnityEditor;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class CameraConfiner : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField] private bool isShakeActive;
        [HideInInspector]
        [SerializeField] float primaryAmplitude;
        [HideInInspector]
        [SerializeField] float primaryFrequency;
        [HideInInspector]
        [SerializeField] float secondaryAmplitude;
        [HideInInspector]
        [SerializeField] float secondaryFrequency;

        
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private float zoomValue;

        private CompositeCollider2D compositeCollider2D;
        private NoiseController noiseController;
        private bool isPlayerInConfiner;
        private float defaultOrthographicValue;
        private const float ZERO_VALUE = 0;

        public bool IsShakeActive
        {
            get => isShakeActive;
            set => isShakeActive = value;
        }
        
        private void Awake()
        {
            compositeCollider2D = GetComponent<CompositeCollider2D>();
            noiseController = cinemachineVirtualCamera.GetComponent<NoiseController>();
            defaultOrthographicValue = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            var cinemachineConfiner = cinemachineVirtualCamera.GetComponent<CinemachineConfiner>();

            if (other.transform.CompareTag(R.S.Tag.Player))
            {
                isPlayerInConfiner = true;
                cinemachineConfiner.m_BoundingShape2D = compositeCollider2D;
            
                //Need to call this function when the confiner is change during runtime
                cinemachineConfiner.InvalidatePathCache();
                
                //Author : Yannick Cote
                //Adjust the zoom of the cam
                if (zoomValue == ZERO_VALUE)
                    zoomValue = defaultOrthographicValue;
                cinemachineVirtualCamera.m_Lens.OrthographicSize = zoomValue;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform.CompareTag(R.S.Tag.Player))
                isPlayerInConfiner = false;
        }

        private void Update()
        {
            //Author : Yannick Cote
            //For the camera shake effect
            if (isShakeActive && isPlayerInConfiner)
            {
                noiseController.SetNoiseSettings(primaryAmplitude,primaryFrequency, secondaryAmplitude, secondaryFrequency);
            }
            else if (!isShakeActive)
                noiseController.SetNoiseSettings(ZERO_VALUE,ZERO_VALUE,ZERO_VALUE,ZERO_VALUE);
        }
    }
    
    
#if UNITY_EDITOR    
    //Author : Yannick Cote
    //Useful for a more dynamic menu in unity ( camera shake options )
    [CustomEditor(typeof(CameraConfiner))]
    public class MyScriptEditor : Editor
    {
        private SerializedProperty primaryAmplitudeProperty;
        private SerializedProperty primaryFrequencyProperty;
        private SerializedProperty secondaryAmplitudeProperty;
        private SerializedProperty secondaryFrequencyProperty;

        private void OnEnable()
        {
            primaryAmplitudeProperty = serializedObject.FindProperty("primaryAmplitude");
            primaryFrequencyProperty = serializedObject.FindProperty("primaryFrequency");
            secondaryAmplitudeProperty = serializedObject.FindProperty("secondaryAmplitude");
            secondaryFrequencyProperty = serializedObject.FindProperty("secondaryFrequency");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            var cameraConfiner = target as CameraConfiner;
            cameraConfiner.IsShakeActive = GUILayout.Toggle(cameraConfiner.IsShakeActive, "Shake actived");
            if (cameraConfiner.IsShakeActive)
            {
                EditorGUILayout.PropertyField(primaryAmplitudeProperty);
                EditorGUILayout.PropertyField(primaryFrequencyProperty);
                EditorGUILayout.PropertyField(secondaryAmplitudeProperty);
                EditorGUILayout.PropertyField(secondaryFrequencyProperty);
            }
            else
            {
                primaryAmplitudeProperty.floatValue = 0;
                primaryFrequencyProperty.floatValue = 0;
                secondaryAmplitudeProperty.floatValue = 0;
                secondaryFrequencyProperty.floatValue = 0;
            }
            EditorUtility.SetDirty(cameraConfiner);
            serializedObject.ApplyModifiedProperties();
        }
        
        
    }
#endif 
}