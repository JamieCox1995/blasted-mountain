using UnityEngine;

namespace Combat.Units
{
    public class Unit
    {
        private Vector3 worldPosition;

        private bool isCurrentlyActive = false;

        public void SetCurrentlyActive()
        {
            isCurrentlyActive = true;
        }

        public void SetNotActive()
        {
            isCurrentlyActive = false;
        }

        public Vector3 GetWorldPosition() 
        {  
            return worldPosition; 
        }

        public bool IsCurrentlyActive
        {
            get { return isCurrentlyActive; }
        }
    }
}
