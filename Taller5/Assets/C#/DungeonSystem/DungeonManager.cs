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

        [Header("Rooms Collection")]
        [Space(10)]
        [SerializeField] private GameObject spawnRoom;
        [SerializeField] private List<GameObject> enemyRooms = new List<GameObject>();
        [SerializeField] private GameObject bossRoom;
        [SerializeField] private List<GameObject> dungeonRooms;
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

            GenerateDungeon();
        }

        private void GenerateDungeon() 
        {
            Vector3 spawnRoomPosition= Vector3.zero;
            GameObject r;
            r = Instantiate(spawnRoom,spawnRoomPosition,Quaternion.identity);
            r.transform.parent = null;
            dungeonRooms.Add(r);
            spawnRoomPosition = new Vector3(spawnRoomPosition.x + r.transform.localScale.x, spawnRoomPosition.y);
            Shuffle(enemyRooms);

            for (int i = 0; i < enemyRooms.Count; i++)
            {
                r = Instantiate(enemyRooms[i], spawnRoomPosition, Quaternion.identity);
                r.transform.parent = null;
                dungeonRooms.Add(r);
                spawnRoomPosition = new Vector3(spawnRoomPosition.x + r.transform.localScale.x, spawnRoomPosition.y);
            }

            r = Instantiate(bossRoom, spawnRoomPosition, Quaternion.identity);
            r.transform.parent = null;
            dungeonRooms.Add(r);
            spawnRoomPosition = new Vector3(spawnRoomPosition.x + r.transform.localScale.x, spawnRoomPosition.y);

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
