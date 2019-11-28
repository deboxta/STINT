using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SaveNamedBenSuccess : MonoBehaviour, ISuccess
    {
        public event SaveNamedBenSuccessEventHandler OnSaveNamedBen;

        public string successName { get; set; }

        private void Awake()
        {
            successName = "Save Named Ben Success";
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += NotifySaveNamedBen;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= NotifySaveNamedBen;
        }

        private void NotifySaveNamedBen(Scene scene, LoadSceneMode loadSceneMode) 
        {
            if (scene.name != R.S.Scene.Main_menu)
            {
                if (OnSaveNamedBen != null) OnSaveNamedBen();
            }
        }

        public void DestroySuccess()
        {
            Destroy(this);
        }
        
        public delegate void SaveNamedBenSuccessEventHandler();
    }
}