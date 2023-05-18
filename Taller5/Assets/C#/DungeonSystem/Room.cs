using Code;
using Code_Boses;
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
        public GameObject pointA,pointB;
        public GameObject floor;
        public GameObject portal;
        public List<Enemy> enemies= new List<Enemy>();
        public bool clear = false;
        public bool boss;
        public BossStateManager bossOBJ;
        public GameObject relic;
        public float CameraScale;
        public float CamY;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + roomBounds.center, roomBounds.size);
        }

        public void addRegister(Enemy en) 
        {
            enemies.Add(en);
        }

        public void removeRegister(Enemy en) 
        {
            enemies.Remove(en);
        }

        public void ActivateEnemies(PlayerController Gicamu, PlayerController Alchies) 
        {
            if (boss)
            {
                bossOBJ.SetUp(Gicamu, Alchies, this);
                return;
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.SetUp(Gicamu, Alchies);
            }
        }

        private void Update()
        {
            if (!clear) 
            {
                if (enemies.Count == 0)
                {
                    SpawnPortals();
                    clear = true;
                }
            }
        }

        private void SpawnPortals() 
        {
            if (!boss)
            {
                if (secretRoom)
                {
                    Instantiate(portal, pointA.transform.position, Quaternion.identity).GetComponent<Portals>().SetUp(ID);
                    Instantiate(portal, pointB.transform.position, Quaternion.identity).GetComponent<Portals>().SetUp(ID + 1);

                }
                else
                {
                    Instantiate(portal, pointB.transform.position, Quaternion.identity).GetComponent<Portals>().SetUp(ID);
                }

                if (relic != null)
                {
                    Instantiate(relic, roomBounds.center + transform.position, Quaternion.identity);
                }
            }
        }
    }
}
