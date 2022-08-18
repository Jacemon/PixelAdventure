using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] waypoints;

    [SerializeField]
    private float speed = 3f;

    private Animator animator;
    private SpriteRenderer sr;
    private int currIndex = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (Vector2.Distance(waypoints[currIndex].transform.position, transform.position) < .1f)
        {
            currIndex = (currIndex + 1) % 2;

            if (currIndex == 0)
                sr.flipX = false;
            else
                sr.flipX = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currIndex].transform.position, speed * Time.deltaTime);
    }
}
