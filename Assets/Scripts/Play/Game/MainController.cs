using System;
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class MainController : MonoBehaviour
    {
        [SerializeField] private float fixedDeltaTime = 0.02f;
        
        private void Update()
        {
#if UNITY_EDITOR
            // Change game speed for testing
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
                Time.timeScale *= 2;
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
                Time.timeScale /= 2;
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
#endif
        }
    }
}