using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PG
{
    public class GameplaySystem : MonoBehaviour
    {
        [SerializeField] private OVRPlayerController playerController;
        [SerializeField] private CharacterController charactorController;
        [SerializeField] private ProcessController psController;
        [SerializeField] private ProcessController psControllerOverhua;
        [SerializeField] private ToolsSelectSystem toolsSelectSystem;
        [SerializeField] private RankingProcess rankingProcess;
        [SerializeField] private int setTime;
        [SerializeField] private int goTo;
        [SerializeField] private int engineTest;
        [SerializeField] private Text txtTimer;
        [SerializeField] private GameObject rankingPanel;
        [SerializeField] private GameObject gameSelectPanel;
        [SerializeField] private GameObject loginPanel;
        [SerializeField] private GameObject[] engine;
        [SerializeField] private Transform spawnP;
        [SerializeField] private Transform posEnd;
        [SerializeField] private GameObject teleport;
        [SerializeField] private GameObject interfacePanel;
        private int timer;
        private float timefloat;
        private bool startTimer;
        public void Clicked_Login(TMPro.TMP_InputField nameInput)
        {
            if (!string.IsNullOrEmpty(nameInput.text))
            {
                PlayerPrefs.SetString("PlayerName", nameInput.text);
                gameSelectPanel.SetActive(true);
                loginPanel.SetActive(false);
            }
            else return;
        }
        public void Clicked_Play(int num)
        {
            if (num > 1 && SceneManager.GetActiveScene().name == "setup_hyper_compresser")
            {
                PlayerPrefs.SetInt("GameSelect", num);
                SceneManager.LoadScene(1);
            }
            else
            {
                playerController.gameObject.transform.position = spawnP.position;
                charactorController.enabled = true;
                playerController.enabled = true;
                teleport.SetActive(true);
                SelectProcess(num);
            }
        }
        public void Clicked_ClearRank() => rankingProcess.ClearRank();
        public void Clicked_Home() => SceneManager.LoadScene(0);
        public void Endgame(int num) 
        {
            playerController.gameObject.transform.position = posEnd.position;
            charactorController.enabled = false;
            playerController.enabled = false;
            teleport.SetActive(false);
            print("work");
            if (num < 2)
            {
                startTimer = false;
                rankingPanel.SetActive(true);
                rankingProcess.ScoreEnd(timer, num);
            }
            else
            {
                rankingPanel.SetActive(true);
            }
        }
        private void SelectProcess(int process)
        {
            switch (process)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        engine[i].SetActive(true);
                        engine[i + 3].SetActive(false);
                    }
                    psController.GetProcess(0,goTo);
                    startTimer = true;
                    txtTimer.gameObject.SetActive(true);
                    toolsSelectSystem.GetEngine(0);
                    break;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        engine[i].SetActive(true);
                        engine[i + 3].SetActive(false);
                    }
                    psController.GetProcess(1,goTo);
                    toolsSelectSystem.GetEngine(0);
                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        engine[i].SetActive(false);
                        engine[i + 3].SetActive(true);
                    }
                    psControllerOverhua.GetProcess(0,goTo);
                    startTimer = true;
                    txtTimer.gameObject.SetActive(true);
                    toolsSelectSystem.GetEngine(1);
                    break;
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        engine[i].SetActive(false);
                        engine[i + 3].SetActive(true);
                    }
                    psControllerOverhua.GetProcess(1,goTo);
                    toolsSelectSystem.GetEngine(1);
                    break;
            }
        }
        private void Start()
        {
            print(SceneManager.GetActiveScene().name);
            print(PlayerPrefs.GetInt("GameSelect"));
            if (SceneManager.GetActiveScene().name == "Blower")
            {
                Clicked_Play(PlayerPrefs.GetInt("GameSelect"));
            }
            if (engineTest < 4)
            {
                SelectProcess(engineTest);
                Time.timeScale = setTime;
                playerController.gameObject.transform.position = spawnP.position;
                charactorController.enabled = true;
                playerController.enabled = true;
                teleport.SetActive(true);
                interfacePanel.SetActive(false);
            }
        }
        private void Update()
        {
            if (startTimer)
            {
                timefloat += Time.deltaTime;
                timer = Convert.ToInt32(timefloat);
                txtTimer.text = timer.ToString();
            }
        }
    }
}
