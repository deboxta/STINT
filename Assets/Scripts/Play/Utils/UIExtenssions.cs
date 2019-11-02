﻿using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public static class UIExtenssions
    {
        public static Button SelectedButton =>             
            EventSystem
                .current
                .currentSelectedGameObject?
                .GetComponent<Button>();

        public static void SelectDown(this Button button)
        {
            button.navigation.selectOnDown?.Select();
        }
        public static void SelectUp(this Button button)
        {
            button.navigation.selectOnUp?.Select();
        }

        public static void Click(this Button button)
        {
            button.OnSubmit(new BaseEventData(EventSystem.current));
        }
    }
}