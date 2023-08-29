using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Combat.Units;

namespace Combat
{
    public class CombatService : Singleton<CombatService>
    {
        private List<CombatantGroup> combatantGroups = new List<CombatantGroup>();
        private int currentlyActiveGroup = -1;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void Initialize()
        {

        }

        public void SpawnUnit()
        {

        }

        public bool GetLineOfSight(Vector3 _Start, Vector3 _End)
        {
            return true;
        }

        public Unit GetCurrentlyActiveUnit()
        {
            Unit active = null;

            CombatantGroup activeGroup = combatantGroups[currentlyActiveGroup];

            active = activeGroup.GetRemainingUnits().FirstOrDefault(u => u.IsCurrentlyActive);

            return active;
        }

        public Unit GetClosestUnit(Vector3 _WorldPosition)
        {
            Vector3 closestPosition;
            float closestDistance = float.MaxValue;
            Unit closestUnit = null;

            List<Unit> units = new List<Unit>();

            foreach(CombatantGroup group in combatantGroups)
            {
                units.AddRange(group.GetUnits());
            }

            float distance = 0f;
            Vector3 currentPosition = Vector2.zero;

            for(int index = 0; index < units.Count; index++)
            {
                currentPosition = units[index].GetWorldPosition();

                distance = Vector3.Distance(currentPosition, _WorldPosition);

                if(distance < closestDistance)
                {
                    closestUnit = units[index];
                    closestDistance = distance;
                    closestPosition = currentPosition;
                }
            }

            return closestUnit;
        }

        public Unit GetClosestFriendlyUnit(Vector3 _WorldPosition)
        {
            Vector3 closestPosition;
            float closestDistance = float.MaxValue;
            Unit closestUnit = null;

            List<Unit> units = new List<Unit>();

            float distance = 0f;
            Vector3 currentPosition = Vector2.zero;

            for (int index = 0; index < units.Count; index++)
            {
                currentPosition = units[index].GetWorldPosition();

                distance = Vector3.Distance(currentPosition, _WorldPosition);

                if (distance < closestDistance)
                {
                    closestUnit = units[index];
                    closestDistance = distance;
                    closestPosition = currentPosition;
                }
            }

            return closestUnit;
        }

        public Unit GetClosestEnemyUnit(Vector3 _WorldPosition)
        {
            return null;
        }
    }
}
