using Code_Core;
using Code_DungeonSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private List<CoolDownDataStructure> coolDownDataStructures= new List<CoolDownDataStructure>();

        private List<CoolDownDataStructure> cds = new List<CoolDownDataStructure>();

        [SerializeField] RectTransform t;

        [SerializeField] float p;

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
            WinPanel.SetActive(false);
        }

        private void Update()
        {
            if (cds.Count > 0)
            {
                foreach (CoolDownDataStructure cd in cds.ToList())
                {
                    cd.img.fillAmount += 1 / cd.cd * Time.deltaTime;

                    if (cd.img.fillAmount <= 1)
                    {
                        cd.pref.SetActive(false);
                        cds.Remove(cd);
                    }
                }
            }
        }

        private void cooldownUI(int i) 
        {
            
            cds.Add(coolDownDataStructures[i]);

            int h = cds.Count - 1;
            cds[h].pref.SetActive(true);
            if (cds.Count -1 > 0)
            {
                
                cds[h].pref.GetComponent<RectTransform>().position = cds[h - 1].pref.GetComponent<RectTransform>().position + new Vector3(40,0);

            }
            else
            {
                cds[h].pref.GetComponent<RectTransform>().position = t.position;
            }

            cds[h].img.fillAmount = 0;
            

        }

        private void Start()
        {
            PlayerController.OnChangeLife += UpdateLife;
            PlayerController.OnLosing += Lose;
            PlayerController.OnWining+= Win;
            PlayerController.CoolDown += cooldownUI;
        }

        private void OnDestroy()
        {
            PlayerController.OnChangeLife -= UpdateLife;
            PlayerController.OnLosing -= Lose;
            PlayerController.OnWining -= Win;
            PlayerController.CoolDown -= cooldownUI;
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
                if (alchiesLife <= dataCase.life)
                {
                    imagesAlchies[dataCase.index].sprite = dataCase.aimg;
                }

                if (gicamuLife <= dataCase.life)
                {
                    imagesGicamu[dataCase.index].sprite = dataCase.gimg;
                }
            }
        }

        public void SetLifeToFull() 
        {
            for (int i = 0; i < imagesAlchies.Count; i++)
            {
                imagesAlchies[i].sprite = _case[0].aimg;
                imagesGicamu[i].sprite = _case[0].gimg;
            }
        }

        private void Lose() 
        {
            DungeonManager.instance.Alchies.GetComponent<PlayerController>().pauseControllers = true;
            DungeonManager.instance.Gicamu.GetComponent<PlayerController>().pauseControllers = true;

            LosePanel.SetActive(true);
        }

        public void Win()
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

    [Serializable]
    public class CoolDownDataStructure 
    {
        public GameObject pref;
        public Image img;
        public float cd;
    }
}
