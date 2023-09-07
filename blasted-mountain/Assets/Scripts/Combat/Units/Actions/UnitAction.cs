using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Units.Actions
{
    [CreateAssetMenu(fileName = "New Action", menuName = "Combat/Units/Actions", order = 1)]
    public class UnitAction : ScriptableObject
    {
        public string ActionName = "New Action";
        public bool IsFreeAction = false;

        protected Unit invokingUnit;

        public virtual bool ActivateAction(MouseClickEventData _MouseData)
        {
            return true;
        }

        public void SetInvokingUnit(Unit _Unit)
        {
            invokingUnit = _Unit;
        }
    }
}
