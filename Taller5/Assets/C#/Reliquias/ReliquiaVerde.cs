using Code_DungeonSystem;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Code
{
    public class ReliquiaVerde : Reliquia
    {
       
        public override void ConsumeRelic()
        {
            DungeonManager.instance.Gicamu.GetComponent<PlayerController>().plusHealth();
            DungeonManager.instance.Alchies.GetComponent<PlayerController>().plusHealth();
            Destroy(gameObject);
        }
    }
}
