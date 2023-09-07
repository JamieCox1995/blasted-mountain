using System;
using UnityEngine;

namespace Navigation.Pathfinding
{
    public class PathRequest
    {
        public Vector3 PathStart;
        public Vector3 PathEnd;
        public Action<PathRequestResult> PathFoundCallback;

        public PathRequest(Vector3 _PathStart, Vector3 _PathEnd, Action<PathRequestResult> _Callback)
        {
            PathStart = _PathStart;
            PathEnd = _PathEnd;
            PathFoundCallback = _Callback;
        }
    }
}
