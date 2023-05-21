using Code_Core;
using Code_DungeonSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class GameUIManager : MonoBehaviour
    {
        public static GameUIManager instance {get; private set;} = null;

        [SerializeField] List<Image> imagesGicamu = new List<Image>();
        [SerializeField] List<Image> imagesAlchies = new List<Image>();

        [SerializeField] List<HealthDataStructure> _case = new List<HealthDataStructure>();

        [SerializeField] private GameObject LosePanel, WinPanel;

        private bool extraHeart = false;

        private void Awake()
        {
            if (instance != null) 
            {
                Destroy(this);
                return;
            }
            instance = this;

            LosePanel.SetActive(false);
        }

        private void Start()
        {
            PlayerController.OnChangeLife += UpdateLife;
            PlayerController.OnLosing += Lose;
            PlayerController.OnWining+= Win;
        }

        private void OnDestroy()
        {
            PlayerController.OnChangeLife -= UpdateLife;
            PlayerController.OnLosing -= Lose;
            PlayerController.OnWining -= Win;
        }
        private void UpdateLife() 
        {
            int alchiesLife = DungeonManager.instance.Alchies.GetComponent<PlayerController>().Health;
            int gicamuLife = DungeonManager.instance.Gicamu.GetComponent<PlayerController>().Health;

            if (!extraHeart && alchiesLife == 8) 
            {
                imagesAlchies[0].gameObject.SetActive(true);
                imagesGicamu[0].gameObject.SetActive(true);
                extraHeart = true;
            }
                
            foreach (HealthDataStructure dataCase in _case)
            {
                if (alchiesLife == dataCase.life)
                {
                    imagesAlchies[dataCase.index].sprite = dataCase.aimg;
                }

                if (gicamuLife == dataCase.life)
                {
                    imagesGicamu[dataCase.index].sprite = dataCase.gimg;
                }
            }
        }

        private void Lose() 
        {
            DungeonManager.instance.Alchies.GetComponent<PlayerController>().pauseControllers = true;
            DungeonManager.instance.Gicamu.GetComponent<PlayerController>().pauseControllers = true;

            LosePanel.SetActive(true);
        }

        private void Win()
        {
            DungeonManager.instance.Alchies.GetComponent<PlayerController>().pauseControllers = true;
            DungeonManager.instance.Gicamu.GetComponent<PlayerController>().pauseControllers = true;

            WinPanel.SetActive(true);
        }
    }

    [Serializable]
    public class HealthDataStructure 
    {
        public Sprite gimg;
        public Sprite aimg;
        public int life;
        public int index;
    }
}
