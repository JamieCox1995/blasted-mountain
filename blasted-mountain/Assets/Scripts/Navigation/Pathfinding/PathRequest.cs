using System;
using UnityEngine;

namespace Navigation.Pathfinding
{
    public class PathRequest
    {
        public Vector3 PathStart;
        public Vector3 PathEnd;
        public Action<Vector3[], bool> PathFoundCallback;

        public PathRequest(Vector3 _PathStart, Vector3 _PathEnd, Action<Vector3[], bool> _Callback)
        {
            PathStart = _PathStart;
            PathEnd = _PathEnd;
            PathFoundCallback = _Callback;
        }
    }
}
