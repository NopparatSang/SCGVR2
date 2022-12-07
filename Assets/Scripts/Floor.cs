using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private Collider player;
    [SerializeField] private GameObject floor;
    [SerializeField] private bool playerIn;

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            floor.SetActive(true);
        }
        if (OVRInput.GetUp(OVRInput.Button.Two))
        {
            StartCoroutine(waitsec());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == player)
        {
            print("Out");
            floor.SetActive(false);
            playerIn = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == player)
        {
            playerIn = true;
        }
    }
    IEnumerator waitsec()
    {
        yield return new WaitForSeconds(1);
        SetFoor();
    }
    public void SetFoor()
    {
        if (!playerIn)
        {
            floor.SetActive(false);
        }
        playerIn = false;
    }
}
