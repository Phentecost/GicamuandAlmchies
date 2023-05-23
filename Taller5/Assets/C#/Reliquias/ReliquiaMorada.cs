using Code_DungeonSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class ReliquiaMorada : Reliquia
    {
        [SerializeField] int cooldown;

        public override void ConsumeRelic()
        {
            DungeonManager.instance.Gicamu.GetComponent<PlayerController>().lessCooldown(cooldown);
            DungeonManager.instance.Alchies.GetComponent<PlayerController>().lessCooldown(cooldown);
            Destroy(gameObject);
        }
    }
}
