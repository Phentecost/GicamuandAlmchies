using Code_DungeonSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class PiedraF : Reliquia
    {
        public bool F;
        public override void SetUp()
        {
            ready = true;
        }

        public override void ConsumeRelic()
        {
            if (F) 
            {
                GameUIManager.instance.Win();
            }

            Destroy(gameObject);
        }
    }
}
