using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game;
using Harmony;
using UnityEngine;

//Author : Yannick Cote
namespace Game
{
    public class NoiseController : MonoBehaviour
    { 
        private float primaryAmplitude; 
        private float primaryFrequency;
        private float secondaryAmplitude;
        private float secondaryFrequency;
        
        private CinemachineVirtualCamera virtualCamera;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private TimelineController timelineController;
        private CinemachineBasicMultiChannelPerlin noiseSettings;

        void Awake()
        {
            timelineController = Finder.TimelineController;
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            noiseSettings = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += TimelineChanged;
        }


        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= TimelineChanged;
        }

        private void TimelineChanged()
        {
            SetCameraNoiseSettings(timelineController.CurrentTimeline);
        }

        public void SetNoiseSettings(float primAmp, float primFreq, float secAmp, float secFreq)
        {
            primaryAmplitude = primAmp;
            primaryFrequency = primFreq;
            secondaryAmplitude = secAmp;
            secondaryFrequency = secFreq;
            
            SetCameraNoiseSettings(timelineController.CurrentTimeline);
        }

        private void SetCameraNoiseSettings(Timeline timeline)
        {
            switch (timeline)
            {
                case Timeline.Primary:
                    SetPrimaryNoise();
                    break;
                case Timeline.Secondary:
                    SetSecondaryNoise();
                    break;
            }
        }

        private void SetPrimaryNoise()
        {
            noiseSettings.m_AmplitudeGain = primaryAmplitude;
            noiseSettings.m_FrequencyGain = primaryFrequency;
        }

        private void SetSecondaryNoise()
        {
            noiseSettings.m_AmplitudeGain = secondaryAmplitude;
            noiseSettings.m_FrequencyGain = secondaryFrequency;
        }
    }
}
