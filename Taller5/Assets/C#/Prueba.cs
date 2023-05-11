using Code_DungeonSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class Prueba : MonoBehaviour
    {
        public Room room;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                //room.ActivateEnemies();
            }
        }
    }
}
