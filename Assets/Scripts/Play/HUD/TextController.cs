using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Harmony;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI primary;
    [SerializeField] private TextMeshProUGUI secondary;

    private float primaryPositionY;
    private float secondaryPositionY;
    
    private TimelineChangedEventChannel timelineChangedEventChannel;

    
    // Start is called before the first frame update
    void Awake()
    {
        primaryPositionY = primary.transform.position.y;
        secondaryPositionY = secondary.transform.position.y;
        timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
    }

    private void OnEnable()
    {
        timelineChangedEventChannel.OnTimelineChanged += TimelineChange;
    }

    private void OnDisable()
    {
        timelineChangedEventChannel.OnTimelineChanged += TimelineChange;
    }

    private void TimelineChange()
    {
        switch (Finder.TimelineController.CurrentTimeline)
        {
            case Timeline.Main:
                primary.text = "1984";
                primary.fontSize = 32;
                primary.faceColor = Color.red;

                secondary.text = "3024";
                secondary.fontSize = 16;
                secondary.faceColor = Color.black;
                break;
            case Timeline.Secondary:
                primary.text = "3024";
                primary.faceColor = Color.black;
                primary.fontSize = 16;
                
                secondary.text = "1984";
                secondary.fontSize = 32;
                secondary.faceColor = Color.red;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
