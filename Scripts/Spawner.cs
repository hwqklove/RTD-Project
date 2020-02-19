using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This script spawns monster
/// each stages have 10 sec delay
/// </summary>

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public GameObject enemy;
    GameObject enemyChild;
    float spawnTime = 0.5f;
    int counter = 0;

    public Text Timer;
    public float timeSet = 10f;
    public Text StageNum;
    int StageCount = 1;

    public Text CostNum;
    public int cost;

    public float tempHealth;

    void Start()
    {
        instance = this;
        cost = 300;
    }

    private void Update()
    {
        StageNum.text = "Stage : " + StageCount;
        if (StageCount == 21)
        {
            SceneManager.LoadScene("Win");
        }
        CostNum.text = "Cost : " + cost;
        if (transform.childCount == 0)
        {
            if (timeSet >= 0)
            {
                Timer.text = "Start : " + timeSet.ToString("N0");
                timeSet -= Time.deltaTime;
                if (timeSet <= 0f)
                {
                    tempHealth += 30;
                    StartCoroutine(spawnMonster());
                    cost += 300;
                }
            }
        }
    }

    IEnumerator spawnMonster ()
    {
        while (true)
        {
            enemyChild = Instantiate(enemy, transform.position, Quaternion.identity);
            enemyChild.transform.SetParent(transform);
            yield return new WaitForSeconds(spawnTime);
            counter++;
            if (counter == 20)
            {
                timeSet = 5f;
                counter = 0;
                StageCount++;
                yield break;
            }
        }
    }
}
