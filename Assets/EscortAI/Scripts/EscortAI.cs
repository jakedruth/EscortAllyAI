using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EscortAI : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public float acceleration;
    public float otherVariables;

    // Start is called before the first frame update
    void Start()
    {

        NavMeshPath navMeshPath = new NavMeshPath();
        NavMesh.CalculatePath(new Vector3(-4, -0.5f, 4), new Vector3(4, -0.5f, -4), NavMesh.AllAreas, navMeshPath);
        List<Vector3> path = navMeshPath.corners.ToList().GetRange(1, navMeshPath.corners.Length - 1);

        for (int i = 0; i < path.Count - 1; i++)
        {
            int j = i + 1;
            Debug.DrawLine(path[i], path[j], Color.red, 5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
