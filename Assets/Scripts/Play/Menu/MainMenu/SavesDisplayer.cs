using System;
using System.Collections.Generic;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SavesDisplayer : MonoBehaviour
    {
        [SerializeField] private Text save1Text;
        [SerializeField] private Text save2Text;
        [SerializeField] private Text save3Text;
        
        private SaveSystem saveSystem;
        private List<Text> savesTexts;

        private void Awake()
        {
            saveSystem = new SaveSystem();
            savesTexts = new List<Text>();
            savesTexts.Add(save1Text);
            savesTexts.Add(save2Text);
            savesTexts.Add(save3Text);

            LoadSavesNames();
        }
        
        private void LoadSavesNames()
        {
            List<DataCollector> dataInfo = saveSystem.GetSaves();
            int index = 0;
            foreach (var data in dataInfo)
            {
                savesTexts[index].text = data.Name;
                index++;
            }

            savesTexts[0] = save1Text;
            savesTexts[1] = save2Text;
            savesTexts[2] = save3Text;
        }
    }
}