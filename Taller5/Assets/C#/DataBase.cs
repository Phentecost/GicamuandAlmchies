using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Code_EnemiesAndAI;
using System;
using Code_DungeonSystem;
using TMPro;

namespace Code_Core
{
    public class DataBase : MonoBehaviour 
    {
        public static DataBase Instance { get; set; } = null;

        private PlayerController _gicamu, _alchies;
        public PlayerController Gicamu { get => _gicamu; }
        public PlayerController Alchies { get => _alchies; }

        //Boss1
        //Boss2

        private int _currentRoom = 0;

        private List<Room> _rooms = new List<Room>();

        public List<Room> Rooms { get => _rooms;}


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }  

            Instance = this;
        }

        public void ChangeID(int i) 
        {
            _currentRoom= i;
        }

        public void AddRegister(PlayerController p, int id) 
        {
            if (id == 0)
            {
                _gicamu = p;
            }
            else if (id == 1)
            {
                _alchies = p;
            }
        }

        public void AddRegister(Room room) 
        {
            _rooms.Add(room);
        }
    }
}
