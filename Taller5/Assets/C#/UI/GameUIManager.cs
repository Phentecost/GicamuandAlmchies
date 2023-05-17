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

        private void Awake()
        {
            if (instance != null) 
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        public void UpdateLife() 
        {
            int alchiesLife = DungeonManager.instance.Alchies.GetComponent<PlayerController>().Health;
            int gicamuLife = DungeonManager.instance.Gicamu.GetComponent<PlayerController>().Health;

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
