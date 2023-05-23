using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class ButonInteraction : MonoBehaviour
    {
        public void OnChanginLevel(int i) 
        {
            if (i == 0)
                LoadManager.instance.MainMenu();
            else
                LoadManager.instance.GameScene();
        }
    }
}
