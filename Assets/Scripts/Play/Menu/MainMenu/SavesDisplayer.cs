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
        [SerializeField] private GameObject deleteSave1Button;
        [SerializeField] private Button save2;
        [SerializeField] private GameObject deleteSave2Button;
        [SerializeField] private Button save3;
        [SerializeField] private GameObject deleteSave3Button;

        private SaveSystem saveSystem;
        private List<Button> savesButtons;
        private List<GameObject> deleteButtons;

        private void Awake()
        {
           saveSystem = new SaveSystem();
           savesButtons = new List<Button>();
           deleteButtons = new List<GameObject>();

            savesButtons.Add(save1);
            savesButtons.Add(save2);
            savesButtons.Add(save3);

            deleteButtons.Add(deleteSave1Button);
            deleteButtons.Add(deleteSave2Button);
            deleteButtons.Add(deleteSave3Button);
            
            foreach (var button in savesButtons)
            {
                button.GetComponent<Image>().enabled = false;
                button.enabled = false;
            }

            foreach (var deleteButton in deleteButtons)
            {
                deleteButton.SetActive(false);
            }


            LoadSavesNames();
        }
        
        public void LoadSavesNames()
        {
            List<DataCollector> dataInfo = saveSystem.GetSaves();
            int index = 0;
            foreach (var data in dataInfo)
            {
                savesButtons[index].GetComponent<Image>().enabled = true;
                savesButtons[index].GetComponentInChildren<Text>().text = data.Name;
                savesButtons[index].GetComponentInChildren<Button>().enabled = true;
                deleteButtons[index].SetActive(true);
                savesButtons[index].enabled = true;
                index++;
            }
        }
    }
}