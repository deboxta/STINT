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
        [SerializeField] private Button save1;
        [SerializeField] private Button save2;
        [SerializeField] private Button save3;
        
        private SaveSystem saveSystem;
        private List<Button> savesButtons;

        private void Awake()
        {
           saveSystem = new SaveSystem();
           savesButtons = new List<Button>();

            savesButtons.Add(save1);
            savesButtons.Add(save2);
            savesButtons.Add(save3);
            foreach (var button in savesButtons)
            {
                button.GetComponent<Image>().enabled = false;
                button.enabled = false;
            }


            LoadSavesNames();
        }
        
        private void LoadSavesNames()
        {
            List<DataCollector> dataInfo = saveSystem.GetSaves();
            int index = 0;
            foreach (var data in dataInfo)
            {
                savesButtons[index].GetComponent<Image>().enabled = true;
                savesButtons[index].GetComponentInChildren<Text>().text = data.Name;
                savesButtons[index].enabled = true;
                index++;
            }

            savesButtons[0] = save1;
            savesButtons[1] = save2;
            savesButtons[2] = save3;
        }
    }
}