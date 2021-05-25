using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DCamera : MonoBehaviour
{
    private GameObject player;
    private SaveLoad saveLoad;
    private void Awake()
    {
        saveLoad = new SaveLoad();
        player = GameObject.FindGameObjectWithTag("Player");
        int numberLevel = 0;
        Player currentPlayer = player.GetComponent<Player>();
        if (saveLoad.LoadData(ref currentPlayer, ref numberLevel)
            && SceneManager.GetActiveScene().buildIndex != numberLevel)
        {
            SceneManager.LoadScene(numberLevel);
        }
    }
    void LateUpdate()
    {
        Vector3 temp = transform.position;
        temp.x = player.transform.position.x;
        temp.y = player.transform.position.y;
        transform.position = temp;
    }
}
