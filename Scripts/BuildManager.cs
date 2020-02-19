using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The build manager script manages the overall tower construction and upgrade.
/// Also, this script is the parent of all nodes and contains the generated towers.
/// </summary>
public class BuildManager : MonoBehaviour
{
    public GameObject SelectNode;
    public static BuildManager instance;

    GameObject[] Tower1 = new GameObject[2];
    public GameObject tempobj;
    GameObject child;
    public GameObject Tu2;
    public GameObject Go2;

    GameObject UpChild;

    public void Start()
    {
        instance = this;
        // Load Level 1 towers (path : Resouces folder)
        Tower1[0] = Resources.Load("Turret/Turret_Lv1") as GameObject;
        Tower1[1] = Resources.Load("Turret/Golem_Lv1") as GameObject;
    }

    public void Update ()
    {
    }

    public void BuildToTower()
    {
        // Determine if tower exists on node, and enough cost
        if (SelectNode.transform.childCount == 0 && Spawner.instance.cost > 0)
        {
            // If node does not have child(tower), construct tower, and set parent
            // If node construct tower, substract 100 from the current cost
            child = Instantiate(Tower1[Random.Range(0, 2)], new Vector3(SelectNode.transform.position.x, SelectNode.transform.position.y + 0.28f, SelectNode.transform.position.z), Quaternion.identity);
            child.transform.SetParent(SelectNode.gameObject.transform);
            Spawner.instance.cost = Spawner.instance.cost - 100;
        }
        else if (Spawner.instance.cost == 0)
        {
            // Warning message if not enough cost
            GameObject.Find("Canvas").transform.Find("Warning").gameObject.SetActive(true);
            StartCoroutine(WaitForItMessage());
        }
        SelectNode = null;
    }

    public void UpgradeTower ()
    {
        // Check the tags of the children in the node and upgrade the appropriate tower
        if (SelectNode.transform.childCount == 1 && child.transform.tag == "Turret_1" && Spawner.instance.cost > 0)
        {
            UpChild = SelectNode.transform.GetChild(0).gameObject;
            Destroy(UpChild);
            UpChild = Instantiate(Tu2, new Vector3(SelectNode.transform.position.x, SelectNode.transform.position.y + 0.28f, SelectNode.transform.position.z), Quaternion.identity);
            UpChild.transform.SetParent(SelectNode.gameObject.transform);
            Spawner.instance.cost = Spawner.instance.cost - 100;
            SelectNode = null;
            UpChild = null;
        }
        else if (SelectNode.transform.childCount == 1 && child.transform.tag == "Golem_1" && Spawner.instance.cost > 0)
        {
            UpChild = SelectNode.transform.GetChild(0).gameObject;
            Destroy(UpChild);
            UpChild = Instantiate(Go2, new Vector3(SelectNode.transform.position.x, SelectNode.transform.position.y + 0.28f, SelectNode.transform.position.z), Quaternion.identity);
            UpChild.transform.SetParent(SelectNode.gameObject.transform);
            Spawner.instance.cost = Spawner.instance.cost - 100;
            SelectNode = null;
            UpChild = null;
        }
        else if (Spawner.instance.cost == 0)
        {
            GameObject.Find("Canvas").transform.Find("Warning").gameObject.SetActive(true);
            StartCoroutine(WaitForItMessage());
        }
    }

    IEnumerator WaitForItMessage ()
    {
        yield return new WaitForSeconds(4.0f);
        GameObject.Find("Canvas").transform.Find("Warning").gameObject.SetActive(false);
    }
}
