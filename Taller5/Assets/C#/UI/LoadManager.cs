using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class LoadManager : MonoBehaviour
    {
        public static LoadManager instance;

        private void Awake()
        {
            if (instance != null) 
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void MainMenu() 
        {
            SceneManager.LoadScene(0);
        }

        public void GameScene() 
        {
            SceneManager.LoadScene(1);
        }

        public void EXIT() 
        {
            Application.Quit();
        }
    }
}
