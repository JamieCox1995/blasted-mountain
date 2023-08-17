using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat.Units;

namespace Combat
{
    public class CombatantGroup : MonoBehaviour
    {
        // We are using a dictionary to keep track of all of the units attached to this group (key), and if they have activated (value) 
        private Dictionary<Unit, bool> units = new Dictionary<Unit, bool>();

        public int GetUnitCount()
        {
            return units.Count;
        }

        public int GetActivatedUnitCount()
        {
            int count = 0;

            foreach(KeyValuePair<Unit, bool> kvp in units)
            {
                if(kvp.Value)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
