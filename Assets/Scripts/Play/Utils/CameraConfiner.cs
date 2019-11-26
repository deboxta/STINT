using System;
using System.Diagnostics;
using Cinemachine;
using Harmony;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace Game
{
    public class CameraConfiner : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField] private bool isShakeActive;
        [HideInInspector]
        [SerializeField] float amplitude;
        [HideInInspector]
        [SerializeField] float frequency;
        
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        
        private CompositeCollider2D compositeCollider2D;
        private NoiseController noiseController;
        private bool isPlayerInConfiner;

        public bool IsShakeActive
        {
            get => isShakeActive;
            set => isShakeActive = value;
        }
        
        private void Awake()
        {
            compositeCollider2D = GetComponent<CompositeCollider2D>();
            noiseController = cinemachineVirtualCamera.GetComponent<NoiseController>();
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
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            isPlayerInConfiner = false;
        }

        private void Update()
        {
            //Author : Yannick Cote
            //For the camera shake effect
            if (isShakeActive && isPlayerInConfiner)
            {
                noiseController.CalledOnEventShake(amplitude,frequency);
            }
        }
    }
    
    
    
    //Author : Yannick Cote
    //Useful for a more dynamic menu in unity ( camera shake options )
    [CustomEditor(typeof(CameraConfiner))]
    public class MyScriptEditor : Editor
    {
        private SerializedProperty amplitudeProperty;
        private SerializedProperty frequencyProperty;


        private void OnEnable()
        {
            amplitudeProperty = serializedObject.FindProperty("amplitude");
            frequencyProperty = serializedObject.FindProperty("frequency");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            var cameraConfiner = target as CameraConfiner;
            cameraConfiner.IsShakeActive = GUILayout.Toggle(cameraConfiner.IsShakeActive, "Shake actived");
            if (cameraConfiner.IsShakeActive)
            {
                EditorGUILayout.PropertyField(amplitudeProperty);
                EditorGUILayout.PropertyField(frequencyProperty);
            }
            else
            {
                amplitudeProperty.floatValue = 0;
                frequencyProperty.floatValue = 0;
            }
            EditorUtility.SetDirty(cameraConfiner);
            serializedObject.ApplyModifiedProperties();
        }
        
        
    }
}