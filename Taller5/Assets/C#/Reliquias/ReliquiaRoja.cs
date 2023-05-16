using Code_DungeonSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class ReliquiaRoja : Reliquia
    {
        [SerializeField] int atk;
        public override void ConsumeRelic()
        {
            DungeonManager.instance.Gicamu.GetComponent<PlayerController>().plusATK(atk);
            DungeonManager.instance.Alchies.GetComponent<PlayerController>().plusATK(atk);
            Destroy(gameObject);
        }
    }
}
