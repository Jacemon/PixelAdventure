using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 tmpPos;

    [SerializeField]
    private float minX = 2.75f;

    [SerializeField]
    private float maxX = 30.25f;

    [SerializeField]
    private float minY = 1f;

    [SerializeField]
    private float maxY = 7f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (!player)
            return;

        tmpPos = transform.position;
        tmpPos.x = player.position.x;
        tmpPos.y = player.position.y;

        if (tmpPos.x < minX)
            tmpPos.x = minX;

        if (tmpPos.x > maxX)
            tmpPos.x = maxX;

        if (tmpPos.y < minY)
            tmpPos.y = minY;

        if (tmpPos.y > maxY)
            tmpPos.y = maxY;

        transform.position = tmpPos;
    }
}
