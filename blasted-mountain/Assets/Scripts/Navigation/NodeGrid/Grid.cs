using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation.NodeGrid
{
    [Serializable]
    public class Grid
    {
        public static Grid Instance;

        private int width, height;
        private Vector3 worldOrigin;
        private float cellSize;

        private Node[,] cells;

        public Grid(int _Width, int _Height, float _CellSize, Vector3 _Origin)
        {
            // initializing the singleton
            if(Instance != null)
            {
                return;
            }

            Instance = this;

            width = _Width;
            height = _Height;
            worldOrigin = _Origin;
            cellSize= _CellSize;

            cells = new Node[width, height];

            // now we are going to initialize the cells for the grid
            for(int xIndex = 0; xIndex < width; xIndex++)
            {
                for (int yIndex = 0; yIndex < height; yIndex++)
                {
                    Vector3 worldPosition = new Vector3(xIndex, 0f, yIndex) * cellSize + worldOrigin + new Vector3(cellSize / 2f, 0f, cellSize / 2f);

                    bool isWalkable = true;
                    int movementPenalty = 0;

                    cells[xIndex, yIndex] = new Node(xIndex, yIndex, worldPosition, isWalkable, movementPenalty);
                }
            }
        }

        public int GetMaxSize()
        {
            return width * height;
        }

        public Node[,] GetCells()
        {
            return cells;
        }

        public List<Node> GetNeighbours(Node _Node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    int checkX, checkY;

                    checkX = _Node.X + x;
                    checkY = _Node.Y + y;

                    if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                    {
                        neighbours.Add(cells[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        #region Getting Cells
        public Node GetCell(int _X, int _Y)
        {
            if (!IsCoordinatesVaild(_X, _Y)) return null;

            return cells[_X, _Y];
        }

        public Node GetCell(Vector3 _WorldPosition)
        {
            ConvertWorldPositionToCoordinates(_WorldPosition, out int x, out int y);

            return GetCell(x, y);
        }
        #endregion

        #region Update Cells
        public void UpdateCell(Node _Node, int _X, int _Y)
        {
            cells[_X, _Y] = _Node;
        }

        public void UpdateCellWalkable(int _X, int _Y, bool _IsWalkable)
        {
            cells[_X, _Y].IsWalkable = _IsWalkable;
        }

        public void UpdateCellWalkable(Vector3 _WorldPosition, bool _IsWalkable)
        {
            ConvertWorldPositionToCoordinates(_WorldPosition, out int X, out int Y);

            UpdateCellWalkable(X, Y, _IsWalkable);
        }
        #endregion

        public void ConvertWorldPositionToCoordinates(Vector3 _WorldPosition, out int _X, out int _Y)
        {
            _X = Mathf.FloorToInt((_WorldPosition - worldOrigin).x / cellSize);
            _Y = Mathf.FloorToInt((_WorldPosition - worldOrigin).z / cellSize);
        }

        public void ConvertCoordinatesToWorldPosition(int _X, int _Y, out Vector3 _WorldPosition)
        {
            _WorldPosition = new Vector3(_X, 0, _Y) * cellSize + worldOrigin + new Vector3(cellSize / 2f, 0f, cellSize / 2f);
        }

        public bool IsCoordinatesVaild(int _X, int _Y)
        {
            return (_X >= 0 && _X < width && _Y >= 0 && _Y < height);
        }
    }
}