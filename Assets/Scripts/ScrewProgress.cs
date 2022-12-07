using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewProgress : MonoBehaviour
{
    private Animator anim;
    private Animator otherAnim;
    public static Action<bool> OnMissionHit = delegate { };
    public static Action<GameObject> OnScrewStand = delegate { };
    public static Action<bool> OnInstall = delegate { };
    private GameObject hitObject;
    private bool working = false;
    public ScrewType type;
    public ToolType toolType;
    public AnimType animType;
    public Process process;
    public enum Process { Install, Remove };
    public enum ScrewType { Short,Standard,Long};
    public enum ToolType { WrenchL, WrenchCombine , WrenchBig, WrenchRing02};
    public enum AnimType { Up, Down ,Stand};

    private void Start()
    {
        anim = GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tools"))
        {
            if (other.name == toolType.ToString())
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                if (other.gameObject.transform.GetChild(0).GetComponent<BoxCollider>() != null)
                {
                    other.gameObject.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                }
                /*if (other.gameObject.GetComponent<OVRGrabbable>().grabPoints != null)
                {
                    other.gameObject.GetComponent<OVRGrabbable>().grabPoints[0].enabled = false;
                }*/
                hitObject = other.gameObject;
                if (!working)
                {
                    DoingWork();
                }
            }
            else
            {
                switch (process)
                {
                    case Process.Install:
                        OnInstall.Invoke(false);
                        break;
                    case Process.Remove:
                        OnMissionHit.Invoke(false);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        working = false;
    }
    private void DoingWork()
    {
        hitObject.transform.position = gameObject.transform.position;
        hitObject.transform.rotation = gameObject.transform.rotation;
        switch (type)
        {
            case ScrewType.Short:
                SetTranformAndAnim(0.1f);
                break;
            case ScrewType.Standard:
                SetTranformAndAnim(0.2f);
                break;
            case ScrewType.Long:
                SetTranformAndAnim(0.3f);
                break;
        }
    }
    private void SetTranformAndAnim(float pos)
    {
        otherAnim = hitObject.GetComponent<Animator>();
        working = true;
        switch (animType)
        {
            case AnimType.Up:
                anim.SetTrigger("Up");
                otherAnim.SetTrigger("Up");
                StartCoroutine(delayAnimation(pos));
                break;
            case AnimType.Down:
                anim.SetTrigger("Down");
                otherAnim.SetTrigger("Down");
                StartCoroutine(delayAnimation(-pos));
                break;
            case AnimType.Stand:
                anim.SetTrigger("Down");
                otherAnim.SetTrigger("Down");
                StartCoroutine(delayAnimation(0));
                break;
            default:
                break;
        }
    }
    IEnumerator delayAnimation(float posY)
    {
        float move = 0;
        Vector3 startPos = Vector3.zero;
        Vector3 stopPos = new Vector3(0, posY, 0);
        while (move<1f)
        {
            move += 0.5f * Time.deltaTime;
            transform.GetChild(1).localPosition = Vector3.Lerp(startPos, stopPos, move);
            hitObject.transform.GetChild(0).localPosition = Vector3.Lerp(startPos, stopPos, move);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        hitObject.transform.GetChild(0).localPosition = Vector3.zero;
        hitObject.GetComponent<BoxCollider>().enabled = true;
        if (hitObject.gameObject.transform.GetChild(0).GetComponent<BoxCollider>() != null)
        {
            hitObject.gameObject.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        }
        /*if (hitObject.gameObject.GetComponent<OVRGrabbable>().grabPoints != null)
        {
            hitObject.gameObject.GetComponent<OVRGrabbable>().grabPoints[0].enabled = true;
        }*/
        if (posY > 0)
        {
            gameObject.SetActive(false);
        }
        OnScrewStand.Invoke(gameObject);
        switch (process)
        {
            case Process.Install:
                OnInstall.Invoke(true);
                break;
            case Process.Remove:
                OnMissionHit.Invoke(true);
                break;
            default:
                break;
        }
    }
}
