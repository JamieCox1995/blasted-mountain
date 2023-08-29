using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace InputHandling
{
    public class InputService : Singleton<InputService>
    {
        private Vector2 mousePosition;          // This is the screen position of the mouse.
        private Vector3 worldMousePosition;

        private Camera mainCamera;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            //mousePosition = Input.mousePosition;

            Ray worldRay = mainCamera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0));

            if(Physics.Raycast(worldRay, out RaycastHit hit ,mainCamera.farClipPlane))
            {
                worldMousePosition = hit.point;
            }
            else
            {
                worldMousePosition = Vector3.negativeInfinity;
            }
        } 

        public Vector3 GetWorldMousePosition()
        {
            return worldMousePosition;
        }

        #region Input Action Messages
        #region Mouse Actions
        public void OnMouseDelta(InputValue value)
        {
            Vector2 vect = value.Get<Vector2>();

            mousePosition = vect;

            onMousePositionDelta?.Invoke(mousePosition);
        }

        public void OnLeftClick()
        {
            onMouseLeftClick?.Invoke();
        }

        public void OnRightClick()
        {
            onMouseRightClick?.Invoke();
        }
        #endregion
        #endregion

        #region Events and Delegates

        public delegate void OnMousePositionDelta(Vector2 mouseDelta);
        public static OnMousePositionDelta onMousePositionDelta;

        public delegate void OnMouseLeftClick();
        public static OnMouseLeftClick onMouseLeftClick;

        public delegate void OnMouseRightClick();
        public static OnMouseRightClick onMouseRightClick;

        #endregion
    }
}