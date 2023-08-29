using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat.Units;
using System.Linq;

namespace Combat
{
    public class CombatantGroup : MonoBehaviour
    {
        // We are using a dictionary to keep track of all of the units attached to this group (key), and if they have activated (value) 
        private Dictionary<Unit, bool> units = new Dictionary<Unit, bool>();
        private int allegiance = -1;

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

        public List<Unit> GetUnits()
        {
            return units.Keys.ToList();
        }

        public List<Unit> GetRemainingUnits()
        {
            List<Unit> remaining = new List<Unit>();

            foreach (KeyValuePair<Unit, bool> kvp in units)
            {
                if (!kvp.Value)
                {
                    remaining.Add(kvp.Key);
                }
            }

            return remaining;
        }
    }
}
