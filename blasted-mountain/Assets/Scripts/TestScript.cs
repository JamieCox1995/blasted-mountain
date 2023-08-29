using InputHandling;
using Navigation;
using Navigation.NodeGrid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private bool foundPath = false;
    private Vector3[] path;
    private Vector3 offset = new Vector3(0, 0.5f, 0);

    private InputService inputService;
    private Navigation.NodeGrid.Grid grid;

    private Node currentNode;

    private void Start()
    {
        grid = Navigation.NodeGrid.Grid.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            NavigationService.RequestPath(new Vector3(2, 0, 2), new Vector3(45, 0, 35), OnPathFound);
        }

        //if (inputService.GetWorldMousePosition() != Vector3.negativeInfinity)
        //{
        //    currentNode = grid.GetCell(inputService.GetWorldMousePosition());

        //    if (currentNode != null) Debug.Log($"Current Node... X:{currentNode.X}, Y: {currentNode.Y}, World Pos: {currentNode.WorldPosition}");
        //}
    }

    public void OnPathFound(Vector3[] _Path, bool _Success)
    {
        if(_Success)
        {
            foundPath = true;
            path = _Path;
        }
    }

    private void OnDrawGizmos()
    {
        if(foundPath)
        {
            for (int index = 0; index < path.Length - 1; index++)
            {
                Gizmos.DrawLine(path[index] + offset, path[index + 1] + offset);
            }
        }
    }
}
