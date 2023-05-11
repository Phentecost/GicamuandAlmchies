using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_DungeonSystem
{
    public class Portals : MonoBehaviour
    {
        private int id = 0;
        bool triger = false;
        public void SetUp(int id) 
        {
            this.id = id;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerController controller = collision.GetComponent<PlayerController>();
            if (controller != null) 
            {
                if (!triger)
                {
                    DungeonManager.instance.onChangingRoom(id);
                    triger = true;
                }
                
            }
        }
    }
}
