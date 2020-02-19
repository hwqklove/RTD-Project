using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    public Text lifeBoard;

    int life = 20;

    // Start is called before the first frame update
    void Start()
    {
        lifeBoard.text = "Life : " + life;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            life = --life;

            lifeBoard.text = "Life : " + life;

            if (life < 0)
            {
                SceneManager.LoadScene("over");
            } 
        }
    }
}
