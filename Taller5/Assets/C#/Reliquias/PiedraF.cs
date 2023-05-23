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
            Destroy(gameObject);
        }
    }
}
