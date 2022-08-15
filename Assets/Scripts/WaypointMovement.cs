using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] waypoints;

    [SerializeField]
    private float speed = 3f;

    private Animator animator;
    private int currIndex = 0;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (Vector2.Distance(waypoints[currIndex].transform.position, transform.position) < .1f)
            currIndex = (currIndex + 1) % 2;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currIndex].transform.position, speed * Time.deltaTime);
    }
}
