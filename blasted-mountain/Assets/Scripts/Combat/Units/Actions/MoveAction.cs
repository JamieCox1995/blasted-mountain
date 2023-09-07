using System;
using Events;
using Navigation;
using Navigation.Pathfinding;
using UnityEngine;

namespace Combat.Units.Actions
{
    [CreateAssetMenu(fileName = "Move Action", menuName = "Combat/Units/Actions")]
    public class MoveAction : UnitAction
    {
        public override bool ActivateAction(MouseClickEventData _MouseData)
        {
            if(_MouseData.WorldPosition == new Vector3(float.MaxValue, float.MaxValue, float.MaxValue))
            {
                Debug.Log("HEEELP");
                return false;
            }

            // For the move action, we want to get the World Location that we want to move to.
            NavigationService.RequestPath(invokingUnit.WorldPosition, _MouseData.WorldPosition, OnPathFound);

            invokingUnit.WorldPosition = _MouseData.WorldPosition;

            return base.ActivateAction(_MouseData);
        }

        private void OnPathFound(PathRequestResult _Result)
        {
            invokingUnit.MoveMembers(_Result);
        }
    }
}
