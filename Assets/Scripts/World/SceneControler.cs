using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControler : MonoBehaviour
{
    SaveLoad saveLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            saveLoad = new SaveLoad();
            int numberScene = SceneManager.GetActiveScene().buildIndex;
            
            Player player 
                = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            saveLoad.SaveData(player, numberScene + 1);
            SceneManager.LoadScene(numberScene + 1);
        }
    }
}
