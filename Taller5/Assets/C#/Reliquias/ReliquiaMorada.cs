using Code_DungeonSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class ReliquiaMorada : Reliquia
    {
        public override void ConsumeRelic()
        {
            DungeonManager.instance.Gicamu.GetComponent<PlayerController>().lessCooldown();
            DungeonManager.instance.Alchies.GetComponent<PlayerController>().lessCooldown();
            Destroy(gameObject);
        }
    }
}
