using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game;
using Harmony;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    [Header("Shake settings")] 
    [SerializeField] [Range(0,100)] private float amplitude;
    [SerializeField] [Range(0,100)] private float frequency;
    
    private CinemachineVirtualCamera virtualCamera;
    private TimelineChangedEventChannel timelineChangedEventChannel;
    private CameraShakeEnterEventChannel cameraShakeEnterEventChannel;
    private TimelineController timelineController;
    private CinemachineBasicMultiChannelPerlin noiseSettings;

    private const float MIN_VALUE_SHAKE = 0;

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

    private void SetCameraNoiseSettings(Timeline timeline)
    {
        switch (timeline)
        {
            case Timeline.Primary:
                NormalShake();
                break;
            case Timeline.Secondary:
                StopNoise();
                break;
        }
    }

    private void NormalShake()
    {
        noiseSettings.m_AmplitudeGain = amplitude;
        noiseSettings.m_FrequencyGain = frequency;
    }

    private void StopNoise()
    {
        noiseSettings.m_AmplitudeGain = MIN_VALUE_SHAKE;
        noiseSettings.m_FrequencyGain = MIN_VALUE_SHAKE;
    }

    public void CalledOnEventShake(float amp, float freq)
    {
        noiseSettings.m_AmplitudeGain = amp;
        noiseSettings.m_FrequencyGain = freq;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
