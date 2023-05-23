using Code_DungeonSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class PiedraF : Reliquia
    {
        public override void SetUp()
        {
            ready = true;
        }

        public override void ConsumeRelic()
        {
            DungeonManager.instance.Gicamu.GetComponent<PlayerController>().RP++;
            DungeonManager.instance.Alchies.GetComponent<PlayerController>().RP++;
            Destroy(gameObject);
        }
    }
}
