using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DCamera : MonoBehaviour
{
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void LateUpdate()
    {
        Vector3 temp = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(1))
        {
            temp = player.transform.position + (mousePos - player.transform.position).normalized * 3;
            temp.z = mousePos.z;
        }
        else
        {
            temp.x = player.transform.position.x;
            temp.y = player.transform.position.y;
        }
        transform.position = temp;
    }
}
