using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Events;
using Utilities;

namespace InputHandling
{
    public class InputService : Singleton<InputService>
    {
        public LayerMask UnitLayerMask;

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
                worldMousePosition = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
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
            // On left click, we will see if there are any "units" under the mouse.

            Ray worldRay = mainCamera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0));
            GameObject clickedObject = null;

            if (Physics.Raycast(worldRay, out RaycastHit hit, mainCamera.farClipPlane))
            {
                // comparing the layer of the object under the raycast to the unit layer mask.
                if(((1<<hit.collider.gameObject.layer) & UnitLayerMask) != 0)
                {
                    // if they match, we want to add it to the click event.
                    clickedObject = hit.collider.gameObject;
                }
            }

            MouseClickEventData clickEvent = new MouseClickEventData
            {
                MousePosition = mousePosition,
                WorldPosition = worldMousePosition,
                ClickedObject = clickedObject
            };   

            onMouseLeftClick?.Invoke(clickEvent);
        }

        public void OnRightClick()
        {
            MouseClickEventData clickEvent = new MouseClickEventData
            {
                MousePosition = mousePosition,
                WorldPosition = worldMousePosition
            };

            onMouseRightClick?.Invoke(clickEvent);
        }
        #endregion
        #endregion

        #region Events and Delegates

        public delegate void OnMousePositionDelta(Vector2 mouseDelta);
        public static OnMousePositionDelta onMousePositionDelta;

        public delegate void OnMouseLeftClick(MouseClickEventData _MouseClickEventData);
        public static OnMouseLeftClick onMouseLeftClick;

        public delegate void OnMouseRightClick(MouseClickEventData _MouseClickEventData);
        public static OnMouseRightClick onMouseRightClick;

        #endregion
    }
}