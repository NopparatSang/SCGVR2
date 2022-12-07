using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PG
{
    public class ProcessController : MonoBehaviour
    {
        [SerializeField] private GameplaySystem gameplaySystem;
        [SerializeField] private int engineNumber;
        [SerializeField] private Image boardMission;
        private int targetProcess;
        private int totalProcess;
        private int firstChild;
        private int numMission = 0;
        public void GetProcess(int num,int gt) => SetupProcess(num,gt);
        public void DoneProcess(int num) => CheckCurrentProcess(num);
        private void SetupProcess(int ps,int gt)
        {
            totalProcess = gameObject.transform.GetChild(0).childCount + gameObject.transform.GetChild(1).childCount;
            firstChild = transform.GetChild(0).childCount;
            print(totalProcess);
            print(ps);
            for (int i = 0; i < totalProcess; i++)
            {
                if (i < gameObject.transform.GetChild(0).childCount)
                {
                    gameObject.transform.GetChild(0).GetChild(i).name = i.ToString();
                    gameObject.transform.GetChild(0).GetChild(i).GetComponent<Collider>().enabled = false;
                    gameObject.transform.GetChild(0).GetChild(i).gameObject.SetActive(transform.GetChild(0).GetChild(i).GetComponent<ProcessSystem>().status);
                }
                else
                {
                    gameObject.transform.GetChild(1).GetChild(i - firstChild).name = i.ToString();
                    gameObject.transform.GetChild(1).GetChild(i - firstChild).GetComponent<Collider>().enabled = false;
                    transform.GetChild(1).GetChild(i - firstChild).gameObject.SetActive(transform.GetChild(1).GetChild(i - firstChild).GetComponent<ProcessSystem>().status);
                }
            }
            switch (ps)
            {
                case 0:
                    transform.GetChild(1).gameObject.SetActive(false);
                    if (gt == 0)
                    {
                        ShootProcess(0);
                    }
                    else
                    {
                        targetProcess = gt;
                        RushProcess(0);
                    }
                    break;
                case 1:

                    transform.GetChild(0).gameObject.SetActive(false);
                    if (gt < firstChild)
                    {
                        ShootProcess(firstChild);
                    }
                    else
                    {
                        targetProcess = gt;
                        RushProcess(firstChild);
                    }
                    break;
            }
        }
        private void CheckCurrentProcess(int currentps)
        {
            print("recipt" + currentps);
            if (currentps < firstChild-1) { transform.GetChild(1).gameObject.SetActive(false); }
            else { transform.GetChild(0).gameObject.SetActive(false); transform.GetChild(1).gameObject.SetActive(true); }
            if  (currentps < totalProcess-1)
            {
                currentps++;
                CheckMission(currentps);
                if (targetProcess > currentps)
                {
                    RushProcess(currentps);
                }
                else
                {
                    Time.timeScale = 1;
                    ShootProcess(currentps);
                }
            }
            else
            {
                print("END GAME");
                gameplaySystem.Endgame(engineNumber);
            }
        }
        private void ShootProcess(int ps)
        {
            print("current process" + ps);
            if (ps< firstChild)
            {
                print(transform.GetChild(0).GetChild(ps).GetComponent<ProcessSystem>());
                transform.GetChild(0).GetChild(ps).GetComponent<ProcessSystem>().Processing();
            }
            else
            {
                transform.GetChild(1).GetChild(ps - firstChild).GetComponent<ProcessSystem>().Processing();
            }
        }
        private void RushProcess(int ps)
        {
            print("Rush" + ps);
            if (ps< firstChild)
            {
                gameObject.transform.GetChild(0).GetChild(ps).GetComponent<ProcessSystem>().RushProcess();
            }
            else
            {
                gameObject.transform.GetChild(1).GetChild(ps - firstChild).GetComponent<ProcessSystem>().RushProcess();
            }
        }
        private void CheckMission(int num)
        {
            int[] ns = { };
            if (engineNumber == 0) { ns = new int[] { 2, 8, 18, 25, 26, 29, 37, 38, 42, 52, 56, 65, 80, 82, 86, 107, 109, 112, 129, 131, 144 }; }
            else { ns = new int[] { 40, 49, 43, 67, 81, 99, 127, 128, 161, 174, 199, 215, 218, 228, 233, 234, 235, 258 }; }
            foreach (var item in ns)
            {
                if (item == num)
                {
                    NextMissionPic();
                }
            }
        }

        private void NextMissionPic()
        {
            numMission++;
            if (engineNumber == 0) { boardMission.sprite = Resources.Load<Sprite>("MissionBoard/" + numMission); }
            else boardMission.sprite = Resources.Load<Sprite>("MissionBoard2/" + numMission);
        }
    }
}
