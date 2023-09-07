using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Events;
using Utilities;
using Combat.Units;
using InputHandling;
using Combat.Units.Actions;

namespace Combat
{
    public class CombatService : Singleton<CombatService>
    {
        [Header("GameObjects: ")]
        public GameObject UnitMaster;

        [Header("Temporary Variables")]
        public GameObject testPrefab;
        public UnitAction action;


        private List<CombatantGroup> combatantGroups = new List<CombatantGroup>();
        private int currentlyActiveGroup = -1;

        private Unit currentlySelected;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            InitialiseEventListeners();

            SpawnUnit(new Vector3(15, 0, 5), 0);
        }

        private void InitialiseEventListeners()
        {
            InputService.onMouseLeftClick += OnLeftMouseClick;
            InputService.onMouseRightClick += OnRightMouseClick;
        }

        public void SpawnUnit(Vector3 _WorldPosition, int _Allegiance)
        {
            if(combatantGroups.Count < _Allegiance + 1)
            {
                combatantGroups.Add(new CombatantGroup(_Allegiance));
            }

            Unit unit = new Unit();
            unit.WorldPosition = _WorldPosition;
            unit.CreateMembers(1, testPrefab);
            unit.AvailableActions.Add(action);
            unit.SelectedAction = action;

            combatantGroups[_Allegiance].AddUnit(unit);
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
                currentPosition = units[index].WorldPosition;

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

        public Unit GetClosestFriendlyUnit(Vector3 _WorldPosition, int _CombatantGroup)
        {
            Vector3 closestPosition;
            float closestDistance = float.MaxValue;
            Unit closestUnit = null;

            List<Unit> units = new List<Unit>();

            units.AddRange(combatantGroups[_CombatantGroup].GetUnits());

            float distance = 0f;
            Vector3 currentPosition = Vector2.zero;

            for (int index = 0; index < units.Count; index++)
            {
                currentPosition = units[index].WorldPosition;

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

        #region Event Listeners
        public void OnLeftMouseClick(MouseClickEventData _Event)
        {
            if(_Event.ClickedObject == null)
            {
                return;
            }

            // TODO: When we click on an object, we want to make sure that the unit is part of the player's combatant group before setting the unit as selected.
            // TODO: We may also want to have an enum which stores the games current mouse state, as if the player is trying to select a target for an action we do not want to set the unit as selected. Although, we may have this logic on the right mouse button.

            GameObject clicked = _Event.ClickedObject;
            Unit unit = clicked.GetComponent<UnitMember>().Unit;

            if(unit != null)
            { 
                if(currentlySelected != null) currentlySelected.IsCurrentlyActive = false;

                unit.SelectedAction.SetInvokingUnit(unit);

                unit.IsCurrentlyActive = true;
                currentlySelected = unit;
            }
            else
            {
                currentlySelected.IsCurrentlyActive = false;
                currentlySelected = null;
            }
        }

        public void OnRightMouseClick(MouseClickEventData _Event)
        {
            if(currentlySelected != null)
            {
                if(currentlySelected.SelectedAction != null)
                {
                    currentlySelected.SelectedAction.ActivateAction(_Event);
                }
            }
        }
        #endregion
    }
}
