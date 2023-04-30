using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Code_EnemiesAndAI;

namespace Code_Core
{
    public class DataBase : MonoBehaviour 
    {
        public static DataBase Instance { get; set; } = null;

        private Player _gicamu, _alchies;
        public Player Gicamu { get => _gicamu; }
        public Player Alchies { get => _alchies; }

        //Boss1
        //Boss2

        private Dictionary<int, List<Enemy>> _enemyPerRoom= new Dictionary<int, List<Enemy>>();

        public Dictionary<int, List<Enemy>> EnemyPerRoom { get => _enemyPerRoom;}
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }  

            Instance = this;
        }

        public void AddRegister(int id, Enemy enemy) 
        {
            if (!_enemyPerRoom.ContainsKey(id))
            {
                _enemyPerRoom.Add(id, new List<Enemy>());
                _enemyPerRoom[id].Add(enemy);
            }
            else
            {
                _enemyPerRoom[id].Add(enemy);
            }
        }

        public void AddRegister(Player p) 
        {
            /*if ()
            {

            }*/
        }
    }
}
