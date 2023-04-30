using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_DungeonSystem
{
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager instance { get; private set; } = null;


        [Header("Dungeon Configuration")]
        [Space(10)]
        [SerializeField] private int numbersOfRooms;
        [SerializeField] private float initialSecretRoomProbability;
        [SerializeField] private float incrisingProbabilityRate;
        [SerializeField]private float currentProbability;
        [Header("Rooms Collection")]
        [Space(10)]
        [SerializeField] private GameObject spawnRoom;
        [SerializeField] private List<GameObject> enemyRooms = new List<GameObject>();
        [SerializeField] private GameObject bossRoom;
        [SerializeField] private GameObject secretRoom;
        [SerializeField] private List<GameObject> dungeonRooms;
        [SerializeField] private List<GameObject> secretRooms;
        [Header("Characters")]
        [SerializeField] private GameObject gicamu;
        [SerializeField] private GameObject alchies;
        

        void Start()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            } 

            instance = this;

            currentProbability = initialSecretRoomProbability;
            GenerateDungeon();
        }

        private void GenerateDungeon() 
        {
            Vector3 spawnRoomPosition= Vector3.zero;
            GameObject r;
            r = Instantiate(spawnRoom,spawnRoomPosition,Quaternion.identity);
            r.transform.parent = null;
            dungeonRooms.Add(r);
            spawnRoomPosition = new Vector3(spawnRoomPosition.x + r.transform.localScale.x + 15, spawnRoomPosition.y);
            Shuffle(enemyRooms);

            int count = 0;

            for (int i = 0; i < enemyRooms.Count; i++)
            {

                r = Instantiate(enemyRooms[i], spawnRoomPosition, Quaternion.identity);
                r.transform.parent = null;
                r.GetComponent<Room>().ID = count;
                dungeonRooms.Add(r);
                spawnRoomPosition = new Vector3(spawnRoomPosition.x + r.transform.localScale.x + 15, spawnRoomPosition.y);
                int x = Random.Range(0, 101);
                if (x<= currentProbability)
                {
                    count++;
                    Vector3 spawnSecretRoomPosition = new Vector3(r.transform.position.x,-r.transform.localScale.y-5);
                    r = Instantiate(secretRoom, spawnSecretRoomPosition, Quaternion.identity);
                    r.transform.parent = null;
                    r.GetComponent<Room>().ID = count;
                    secretRooms.Add(r);
                    currentProbability = initialSecretRoomProbability;
                }
                else
                {
                    currentProbability += incrisingProbabilityRate;
                }

                count++;
            }

            r = Instantiate(bossRoom, spawnRoomPosition, Quaternion.identity);
            r.transform.parent = null;
            dungeonRooms.Add(r);
            spawnRoomPosition = new Vector3(spawnRoomPosition.x + r.transform.localScale.x + 15, spawnRoomPosition.y);

        }

        public void Shuffle(List<GameObject> list) 
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject temp = list[i];
                int rand = Random.Range(i, list.Count);
                list[i] = list[rand];
                list[rand] = temp;
            }
        }
    }
}
