using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private Material hlMat;
    private Renderer rend;
    private void Start()
    {
        if (gameObject.transform.childCount != 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                rend = gameObject.transform.GetChild(i).GetComponent<Renderer>();
                rend.enabled = true;
                rend.sharedMaterial = hlMat;
            }
        }
    }
}
