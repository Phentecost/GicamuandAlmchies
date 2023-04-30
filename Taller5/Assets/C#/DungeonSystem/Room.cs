using Code_Core;
using Code_EnemiesAndAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_DungeonSystem
{
    public class Room : MonoBehaviour
    {
        public Bounds roomBounds;
        public int ID;
        public GameObject wall;
        public bool secretRoom;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + roomBounds.center, roomBounds.size);
        }

        public void ActivateEnemies() 
        {
            foreach (Enemy enemy in DataBase.Instance.EnemyPerRoom[ID])
            {
                enemy.SetUp(DataBase.Instance.Gicamu,DataBase.Instance.Alchies);
            }
        }

        private void Update()
        {
            if (DataBase.Instance.EnemyPerRoom[ID].Count == 0) 
            {
                UnlockNextRoom();
            }
        }

        private void UnlockNextRoom() 
        {
            Destroy(wall);
        }
    }
}
