using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;
using Navigation.NodeGrid;
using Combat.Units.Actions;
using Navigation.Pathfinding;

namespace Combat.Units
{
    [System.Serializable]
    public class Unit
    {
        public List<UnitMember> UnitMembers = new List<UnitMember>();
        public Vector3 WorldPosition;

        public int Speed = 2; // Speed Levels: 1 (Shortest), 2 (Medium), 3 (Long)

        public List<UnitAction> AvailableActions = new List<UnitAction>();
        public UnitAction SelectedAction = null;

        private int remainingActions = 2;

        public bool IsCurrentlyActive = false;

        public void CreateMembers(int _MemberCount, GameObject _Template)
        {
            for (int index = 0; index < _MemberCount; index++)
            {
                // For the first member of each unit, we want to spawn them in at the centre (world position of the unit). All other members will be spawned within 2 squares of the "Leader"
                UnitMemberType type = index == 0 ? UnitMemberType.Leader : UnitMemberType.Trooper;         

                Vector3 spawnLocation = index == 0 ? WorldPosition : (Random.insideUnitSphere * 3f) + WorldPosition;

                Node node = Navigation.NodeGrid.Grid.Instance.GetCell(spawnLocation);

                // Spawning the Template Game Object
                GameObject spawned = GameObject.Instantiate(_Template, node.WorldPosition, Quaternion.identity);

                // Get the UnitMember component
                UnitMember member = spawned.GetComponent<UnitMember>();
                member.Unit = this;
                member.WorldPosition = node.WorldPosition;
                member.Type = type;
                member.GridPosition = new Vector2(node.X, node.Y);
                
                UnitMembers.Add(member);
            }
        }

        public void MoveMembers(PathRequestResult _Result)
        {
            // if the path could not be found, we do not want to tell the members to move
            if (!_Result.Success) return;

            foreach(UnitMember unit in UnitMembers)
            {
                unit.Move(_Result.Path);
            }

            remainingActions--;
        }
    }
}
