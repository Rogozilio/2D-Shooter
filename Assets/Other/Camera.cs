using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position + new Vector3(0, 0, transform.position.z);
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
