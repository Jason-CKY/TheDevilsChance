using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Patrol : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    public float moveSpeed;

    private bool rotating;

    void Start()
    {
        transform.position = points[0].position;
        destPoint = 0;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, points[destPoint].position) < 0.5f)
        {
            transform.rotation *= Quaternion.Euler(0, 180f, 0);
            destPoint++;
        }

        if (destPoint >= points.Length)
        {
            destPoint = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, points[destPoint].position, moveSpeed * Time.deltaTime);
    }

}