using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_DungeonSystem
{
    public class SecretRoom : MonoBehaviour
    {
        [SerializeField] private List<GameObject> relics;
        [SerializeField] private GameObject generatedRelic;

        void Start()
        {
            generatedRelic = generateRelic();
        }

        public GameObject generateRelic() 
        {
            return relics[Random.Range(0,relics.Count)];
        }

    }
}
