using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG {
    public class ProcessSystem : MonoBehaviour
    {
        private Animator anim;
        private Collider colli;
        [SerializeField] private ProcessController ps;
        [SerializeField] private float screwSize;
        [SerializeField] private float workTimes;
        [SerializeField] public bool status;
        [SerializeField] private string workString;
        [SerializeField] private ScrewProcessType screwProcessType;
        [SerializeField] private WorkType workType;
        [SerializeField] private GameObject[] deActive;
        [SerializeField] private GameObject[] Active;
        private GameObject workObject;
        [SerializeField] private enum WorkType { Anim, Setup, Remove, Screw, BoltPlate,Locknut }
        public enum ScrewProcessType
        { 
            none,
            In,
            Out
        };

        private void Awake()
        {
            anim = GetComponent<Animator>();
            colli = GetComponent<Collider>();
        }
        public void Processing() => Process();
        public void RushProcess()
        {
            switch (workType)
            {
                case WorkType.Anim:
                    SelfAnimProcess(workString);
                    break;
                case WorkType.Setup:
                    SetupProcess(true);
                    break;
                case WorkType.Screw:
                    ScrewProcess();
                    break;
                case WorkType.BoltPlate:
                    BoltePlateProcess();
                    break;
                case WorkType.Remove:
                    RemoveProcess(true);
                    break;
                case WorkType.Locknut:
                    LocknutProcess();
                    break;
                default:
                    break;
            }
        }
        private void Process()
        {
            colli.enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        private void OnTriggerEnter(Collider other)
        {
            switch (workType)
            {
                case WorkType.Anim:
                    if (other.CompareTag("Hand"))
                    {
                        SelfAnimProcess(workString);
                    }
                    break;
                case WorkType.Setup:
                    if (other.CompareTag(workString))
                    {
                        workObject = other.gameObject;
                        SetupProcess(false);
                    }
                    break;
                case WorkType.Screw:
                    if (other.CompareTag(workString))
                    {
                        workObject = other.gameObject;
                        ScrewProcess();
                    }
                    break;
                case WorkType.BoltPlate:
                    if (other.CompareTag("Hammer"))
                    {
                        workObject = other.gameObject;
                        BoltePlateProcess();
                    }
                    break;
                case WorkType.Locknut:
                    if (other.CompareTag(workString))
                    {
                        workObject = other.gameObject;
                        LocknutProcess();
                    }
                    break;
                default:
                    break;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (workType == WorkType.Remove)
            {
                if (other.CompareTag(workString))
                {
                    RemoveProcess(false);
                }
            }
        }
        private void ScrewProcess()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            colli.enabled = false;
            switch (screwProcessType)
            {
                case ScrewProcessType.In:
                    StartCoroutine(ScrewProcessing("R"));
                    break;
                case ScrewProcessType.Out:
                    StartCoroutine(ScrewProcessing("L"));
                    break;
                default:
                    break;
            }
        }
        private IEnumerator ScrewProcessing(string type)
        {
            if (workObject != null)
            {
                workObject.transform.position = transform.position;
                workObject.transform.rotation = transform.localRotation;
                workObject.GetComponent<OVRGrabbable>().grabPoints[0].enabled = false;
                workObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = false;
                workObject.GetComponent<BoxCollider>().enabled = false;
                //workObject.GetComponent<Animator>().enabled = true;
                workObject.GetComponent<Animator>().SetBool(type, true);
                anim.SetBool(type, true);
            }

            float move = 0;
            Vector3 startPos = Vector3.zero;
            Vector3 stopPos = new Vector3(0, screwSize, 0);
            while (move < 1f)
            {
                move += 0.5f * Time.deltaTime;
                transform.GetChild(1).localPosition = Vector3.Lerp(startPos, stopPos, move);
                if (workObject != null) { workObject.transform.GetChild(0).localPosition = Vector3.Lerp(startPos, stopPos, move); }
                yield return null;
            }
            anim.SetBool(type, false); if (workObject != null) workObject.GetComponent<Animator>().SetBool(type, false);
            /*if (type == "R") { anim.SetBool("R", false); if (workObject != null) workObject.GetComponent<Animator>().SetBool("R", false); }
            else { anim.SetBool("L", false); if (workObject != null) workObject.GetComponent<Animator>().SetBool("L", false); }*/
            yield return new WaitForSeconds(workTimes);
            DoneProcess();
            if (type == "L") gameObject.SetActive(false);
            if (workObject != null)
            {
                workObject.transform.GetChild(0).localPosition = Vector3.zero;
                workObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                workObject.GetComponent<OVRGrabbable>().grabPoints[0].enabled = true;
                workObject.GetComponent<BoxCollider>().enabled = true;
                //workObject.GetComponent<Animator>().enabled = false;
            }

        }
        private void SetupProcess(bool rush)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            colli.enabled = false;
            if (rush)
            {
                transform.GetChild(1).gameObject.SetActive(true);
                DoneProcess();
            }
            else
            {
                workObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                DoneProcess();
            }
        }
        private void SelfAnimProcess(string a = "")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            colli.enabled = false;
            StartCoroutine(SelfAnimProcessing(a));
        }
        private IEnumerator SelfAnimProcessing(string type)
        {
            anim.SetTrigger(type);
            yield return new WaitForSeconds(workTimes);
            DoneProcess();
            //gameObject.SetActive(false);
        }
        private void RemoveProcess(bool rush)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            colli.enabled = false;
            DoneProcess();
        }
        private void DoneProcess()
        {
            if (deActive != null) { for (int i = 0; i < deActive.Length; i++) { deActive[i].SetActive(false); } }
            if (Active != null) { for (int i = 0; i < Active.Length; i++) { Active[i].SetActive(true); } }
            ps.DoneProcess(Convert.ToInt32(gameObject.name));
        }
        private void BoltePlateProcess()
        {
            switch (workString)
            {
                case "Out":
                    StartCoroutine(BoltePlateProcessing("BoltPlate","HammerFirst"));
                    break;
                case "FirstIn":
                    StartCoroutine(BoltePlateProcessing("BoltPlateFirstHit", "HammerSec"));
                    break;
                case "SecIn":
                    StartCoroutine(BoltePlateProcessing("BoltPlateSecHit", "HammerThird"));
                    break;
                case "CapTool":
                    StartCoroutine(BoltePlateProcessing("Captool", "HammerCap"));
                    break;
                default:
                    break;
            }
        }
        private IEnumerator BoltePlateProcessing(string type, string hit)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            colli.enabled = false;
            if (workObject != null)
            {
                workObject.GetComponent<OVRGrabbable>().grabPoints[0].enabled = false;
                workObject.transform.position = transform.position;
                workObject.transform.rotation = transform.rotation;
                workObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = false;
                workObject.GetComponent<BoxCollider>().enabled = false;
                workObject.GetComponent<Animator>().enabled = true;
                workObject.GetComponent<Animator>().SetTrigger(hit);
            }
            anim.SetTrigger(type);
            yield return new WaitForSeconds(workTimes);
            if (workObject != null)
            {
                workObject.transform.GetChild(0).localPosition = Vector3.zero;
                workObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                workObject.GetComponent<OVRGrabbable>().grabPoints[0].enabled = true;
                workObject.GetComponent<BoxCollider>().enabled = true;
                workObject.GetComponent<Animator>().enabled = false;
            }
            DoneProcess();
        }
        private void LocknutProcess()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            colli.enabled = false;
            switch (screwProcessType)
            {
                case ScrewProcessType.In:
                    StartCoroutine(LocknutProcessing("LocknutIN"));
                    break;
                case ScrewProcessType.Out:
                    StartCoroutine(LocknutProcessing("LocknutOut"));
                    break;
                default:
                    break;
            }
        }
        private IEnumerator LocknutProcessing(string type)
        {
            if (workObject != null)
            {
                workObject.transform.position = transform.position;
                workObject.transform.rotation = transform.localRotation;
                workObject.GetComponent<OVRGrabbable>().grabPoints[0].enabled = false;
                workObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = false;
                workObject.GetComponent<BoxCollider>().enabled = false;
                workObject.GetComponent<Animator>().SetBool(type, true);
                anim.SetBool(type, true);
            }

            float move = 0;
            Vector3 startPos = Vector3.zero;
            Vector3 stopPos = new Vector3(0, screwSize, 0);
            while (move < 1f)
            {
                move += 0.5f * Time.deltaTime;
                transform.GetChild(1).localPosition = Vector3.Lerp(startPos, stopPos, move);
                if (workObject != null) { workObject.transform.GetChild(0).localPosition = Vector3.Lerp(startPos, stopPos, move); }
                yield return null;
            }
            anim.SetBool(type, false); if (workObject != null) workObject.GetComponent<Animator>().SetBool(type, false);
            yield return new WaitForSeconds(workTimes);
            if (type == "LocknutOut") gameObject.SetActive(false);
            DoneProcess();
            if (workObject != null)
            {
                workObject.transform.GetChild(0).localPosition = Vector3.zero;
                workObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                workObject.GetComponent<OVRGrabbable>().grabPoints[0].enabled = true;
                workObject.GetComponent<BoxCollider>().enabled = true;
            }

        }
    }
}
