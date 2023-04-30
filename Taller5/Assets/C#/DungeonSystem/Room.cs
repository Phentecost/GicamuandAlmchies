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
    }
}
