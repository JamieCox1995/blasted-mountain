using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Navigation.Pathfinding;
using Navigation.NodeGrid;
using System.Linq;

namespace Navigation
{
    [RequireComponent(typeof(Pathfinding.Pathfinding))]
    public class NavigationService : Singleton<NavigationService>
    {
        private Queue<PathRequest> requestQueue = new Queue<PathRequest>();
        private PathRequest currentRequest;

        private Pathfinding.Pathfinding pathfinding;                            // Reference to the pathfinding system so we can make path requests.
        private BoxCollider collider;                                           // This collider is what the input system will raycast against.
        private NodeGrid.Grid grid;                                             // Reference to the grid.

        private bool isProcessingRequest = false;
        public bool DebugGrid = false;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            int height = 40, width = 50;

            grid = new NodeGrid.Grid(width, height, 1f, Vector3.zero);

            pathfinding = GetComponent<Pathfinding.Pathfinding>();
            pathfinding.Initialize();

            collider = GetComponentInChildren<BoxCollider>();

            if(collider == null )
            {
                // if there is no collider attached, create an empty child and add a box collider
                GameObject child = new GameObject("_GridCollider", new Type[] { typeof(BoxCollider) });
                child.transform.parent = transform;
            };

            collider.size = new Vector3(width, 0f, height);
            collider.center = new Vector3(width / 2f - 0.5f, 0f, height / 2f - 0.5f);
        }

        public static void RequestPath(Vector3 _PathStart, Vector3 _PathEnd, Action<PathRequestResult> _Callback)
        {
            PathRequest request = new PathRequest(_PathStart, _PathEnd, _Callback);

            Instance.requestQueue.Enqueue(request);
            Instance.ProcessNextRequest();
        }

        private void ProcessNextRequest()
        {
            // If we are currently processing a request, or there are no more requests, we want to just stop this method.
            if(isProcessingRequest || requestQueue.Count == 0)
            {
                return;
            }

            currentRequest = requestQueue.Dequeue();
            isProcessingRequest = true;
            pathfinding.FindPath(currentRequest.PathStart, currentRequest.PathEnd);
        }

        public void FinishProcessingRequest(PathRequestResult _Result)
        {
            currentRequest.PathFoundCallback(_Result);
            isProcessingRequest = false;

            ProcessNextRequest();
        }

        #region Utility Methods
        public bool GetIsCellWalkable(Vector3 _Position)
        {
            grid.ConvertWorldPositionToCoordinates(_Position, out int x, out int y);
            Node cellToCheck = grid.GetCell(x, y);

            return cellToCheck.IsWalkable;
        }

        public List<Vector3> GetWalkableCellsAround(Vector3 _Position)
        {
            grid.ConvertWorldPositionToCoordinates(_Position, out int x, out int y);
            Node cellToCheck = grid.GetCell(x, y);

            List<Node> nodes = grid.GetNeighbours(cellToCheck);

            List<Vector3> walkablePositions = new List<Vector3>();

            foreach(Node node in nodes)
            {
                if(node.IsWalkable)
                {
                    walkablePositions.Add(node.WorldPosition);
                }
            }

            return walkablePositions;
        }

        #endregion

        private void OnDrawGizmos()
        {
            if(DebugGrid)
            {
                if (grid == null || grid.GetCells() == null) return;

                // we want to go over all of the cells in the Grid and draw a cube
                foreach (Node node in grid.GetCells())
                {
                    //Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                    Gizmos.color = Color.black;
                    Gizmos.color = (node.IsWalkable) ? Gizmos.color : Color.red;
                    Gizmos.DrawWireCube(new Vector3(node.WorldPosition.x, 0f, node.WorldPosition.z), new Vector3(1f, 0f, 1f));
                }
            }
        }
    }
}
