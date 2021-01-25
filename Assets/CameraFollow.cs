using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    CharacterController2D player;
    float followSpeed = 5f;
    void Start()
    {
        player = FindObjectOfType<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController2D player = FindObjectOfType<CharacterController2D>();

        if (player != null)
        {
            Vector3 targetPosition = player.transform.position;
            targetPosition.z = -10;
            transform.position = Vector3.Slerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
