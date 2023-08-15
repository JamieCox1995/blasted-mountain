using System;
using Utilities;
using UnityEngine;

namespace Navigation.NodeGrid
{
    [Serializable]
    public class Node : IHeapItem<Node>
    {
        // The coordinates in the Grid
        public int X, Y;

        // World Position Coordinates
        public Vector3 WorldPosition;

        public bool IsWalkable;
        public int MovementPenalty;

        private int gCost;
        private int hCost;

        public Node PreviousNode;
        private int heapIndex;

        public Node(int _X, int _Y, Vector3 _WorldPosition, bool _IsWalkable, int _Penalty)
        {
            X = _X;
            Y = _Y;
            WorldPosition = _WorldPosition;
            IsWalkable = _IsWalkable;
            MovementPenalty = _Penalty;
        }

        public int GetGCost()
        {
            return gCost;
        }

        public int GetHCost()
        {
            return hCost;
        }

        public int GetFCost()
        {
            return gCost + hCost;
        }

        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(Node _Comparitor)
        {
            int fCost = GetFCost().CompareTo(_Comparitor.GetFCost());

            if(fCost == 0)
            {
                fCost = GetHCost().CompareTo(_Comparitor.GetHCost());
            }

            return -fCost;
        }

        public void SetGCost(int cost)
        {
            gCost = cost;
        }

        public void SetHCost(int cost)
        {
            hCost = cost;
        }
    }
}

