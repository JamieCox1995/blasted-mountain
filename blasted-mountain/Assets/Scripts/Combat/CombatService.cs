using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Combat.Units;

namespace Combat
{
    public class CombatService : Singleton<CombatService>
    {
        private List<CombatantGroup> combatantGroups = new List<CombatantGroup>();

        // Start is called before the first frame update
        void Start()
        {

        }

        public void Initialize()
        {

        }

        public bool GetLineOfSight(Vector3 _Start, Vector3 _End)
        {
            return true;
        }

        public Unit GetClosestUnit(Vector3 _Position)
        {
            return null;
        }

        public Unit GetClosestFriendlyUnit(Vector3 _Position)
        {
            return null;
        }

        public Unit GetClosestEnemyUnit(Vector3 _Position)
        {
            return null;
        }
    }
}
