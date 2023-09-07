using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation.NodeGrid;
using Utilities;

namespace Navigation.Pathfinding
{


    public class Pathfinding : MonoBehaviour
    {
        // the grid for the current level
        private NodeGrid.Grid grid;

        public void Initialize()
        {
            grid = NodeGrid.Grid.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_StartLocation"></param>
        /// <param name="_EndLocation"></param>
        public void FindPath(Vector3 _StartLocation, Vector3 _EndLocation)
        {
            StartCoroutine(CalculatPath(_StartLocation, _EndLocation));
        }

        private IEnumerator CalculatPath(Vector3 _StartLocation, Vector3 _EndLocation)
        {
            if (_StartLocation != new Vector3(float.MaxValue, float.MaxValue, float.MaxValue) && _EndLocation != new Vector3(float.MaxValue, float.MaxValue, float.MaxValue))
            {

                List<Vector3> path = new List<Vector3>();
                bool pathFound = false;

                // Getting the nodes at the start and end nodes
                Node startNode = grid.GetCell(_StartLocation);
                Node endNode = grid.GetCell(_EndLocation);

                // There is no point in calculating the path if it is not possible to get to the start or the end
                if (startNode.IsWalkable && endNode.IsWalkable)
                {
                    // Creating a heap so that we add to the list and take entries from the start.
                    Heap<Node> openNodes = new Heap<Node>(grid.GetMaxSize());
                    HashSet<Node> closedNodes = new HashSet<Node>();

                    // Adding the node at the startof the path
                    openNodes.Add(startNode);

                    // iterate over all of the nodes we want to check
                    while (openNodes.Count > 0)
                    {
                        // grabbing the first node and removing it from the heap
                        Node currentNode = openNodes.RemoveFirst();
                        closedNodes.Add(currentNode);

                        // if the node we are now checking is the node at the end of the path
                        // we can say that we have found the path, and we can stop searching
                        if (currentNode == endNode)
                        {
                            pathFound = true;
                            break;
                        }

                        // we want to check all of the nodes around our current node
                        foreach (Node node in grid.GetNeighbours(currentNode))
                        {
                            // if the node is not walkable, or it is already in the list of checked nodes, we want to skip it
                            if (!node.IsWalkable || closedNodes.Contains(node))
                            {
                                continue;
                            }

                            // getting the cost to the nearby node.
                            int cost = currentNode.GetGCost() + DistanceCost(currentNode, node);

                            if (cost < node.GetGCost() || !openNodes.Contains(node))
                            {
                                node.SetGCost(cost);
                                node.SetHCost(DistanceCost(node, endNode));
                                node.PreviousNode = currentNode;

                                if (!openNodes.Contains(node))
                                {
                                    openNodes.Add(node);
                                }
                                else
                                {
                                    openNodes.UpdateItem(node);
                                }
                            }
                        }
                    }
                }

                yield return true;

                if (pathFound)
                {
                    path = BuildPath(endNode);
                }

                // TODO: Call the event to let the unit that the path has been calculated.
                PathRequestResult result = new PathRequestResult
                {
                    Path = path.ToArray(),
                    Success = pathFound
                };

                NavigationService.Instance.FinishProcessingRequest(result);
            }
        }

        private List<Vector3> BuildPath(Node _EndNode)
        {
            List<Vector3> path = new List<Vector3>();

            Node currentNode = _EndNode;

            while(currentNode.PreviousNode != null)
            {
                path.Add(new Vector3(currentNode.X, 0f, currentNode.Y));
                currentNode = currentNode.PreviousNode;
            }

            path.Reverse();

            foreach(Node node in grid.GetCells())
            {
                node.PreviousNode = null;
            }

            return path;
        }

        private int DistanceCost(Node _Start, Node _End)
        {
            int xDist = Mathf.Abs(_Start.X - _End.X);
            int yDist = Mathf.Abs(_Start.Y - _End.Y);

            int remaining = Mathf.Abs(xDist - yDist);

            return Constants.DiagonalMovementCost * Mathf.Min(xDist, yDist) + Constants.DefaultMovementCost * remaining;
        }
    }

}