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
        private const string DEFAULT_SAVE_NAME = "empty";

        private void Awake()
        {
           saveSystem = Finder.SaveSystem;
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
            for (int index = -1; index < 3; index++)
            {
                if (dataInfo.Count != 0)
                {
                    foreach (var data in dataInfo)
                    {
                        index++;
                        savesButtons[index].GetComponent<Image>().enabled = true;
                        savesButtons[index].GetComponentInChildren<Text>().text = data.Name;
                        savesButtons[index].GetComponentInChildren<Button>().enabled = true;
                        deleteButtons[index].SetActive(true);
                        savesButtons[index].enabled = true;
                    }

                    dataInfo.Clear();
                }
                else if (index != -1)
                {
                    savesButtons[index].GetComponent<Image>().enabled = false;
                    savesButtons[index].GetComponentInChildren<Text>().text = DEFAULT_SAVE_NAME;
                    savesButtons[index].GetComponentInChildren<Button>().enabled = false;
                    deleteButtons[index].SetActive(false);
                    savesButtons[index].enabled = false;
                }
            }
        }
    }
}