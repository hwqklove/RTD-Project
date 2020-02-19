using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color StartColor;
    public Color SelectColor;
    public Renderer rend;

    public void Start ()
    {
        rend = GetComponent<Renderer>();
        StartColor = rend.material.color;
    }

    public void OnMouseUp ()
    {
        rend.material.color = SelectColor;
        BuildManager.instance.SelectNode = gameObject;
        Invoke("ResetColor", 3);
    }

    void ResetColor ()
    {
        rend.material.color = Color.white;
    }
}
