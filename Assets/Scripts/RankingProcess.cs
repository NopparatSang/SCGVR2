using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RankingProcess : MonoBehaviour
{
    [SerializeField] private GameObject rankPrefs;
    [SerializeField] private GameObject[] panelRank;
    [SerializeField] private Transform parentRankOne, parentRankTwo;
    private List<string> nameListOne, nameListTwo;
    private List<int> timeListOne, timeListTwo;

    private void Awake()
    {
        nameListOne = new List<string>();
        timeListOne = new List<int>();
        nameListTwo = new List<string>();
        timeListTwo = new List<int>();
        int f = 1000;
        for (int i = 0; i < f; i++)
        {
            if (PlayerPrefs.HasKey($"1Name{i}"))
            {
                nameListOne.Add(PlayerPrefs.GetString($"1Name{i}"));
                timeListOne.Add(PlayerPrefs.GetInt($"1Time{i}"));
            }
            else { f = i; SetupRank(0); }
        }
        int g = 1000;
        for (int i = 0; i < g; i++)
        {
            if (PlayerPrefs.HasKey($"2Name{i}"))
            {
                nameListTwo.Add(PlayerPrefs.GetString($"2Name{i}"));
                timeListTwo.Add(PlayerPrefs.GetInt($"2Time{i}"));
            }
            else { g = i; SetupRank(1); }
        }
    }
    private void SetupRank(int num)
    {
        if (num == 0)
        {
            for (int i = 0; i < nameListOne.Count; i++)
            {
                GameObject ins = Instantiate(rankPrefs, parentRankOne, false);
                ins.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = i.ToString();
                ins.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = nameListOne[i].ToString();
                ins.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = timeListOne[i].ToString();
            }
        }
        else
        {
            for (int i = 0; i < nameListTwo.Count; i++)
            {
                GameObject ins = Instantiate(rankPrefs, parentRankTwo, false);
                ins.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = i.ToString();
                ins.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = nameListTwo[i].ToString();
                ins.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = timeListTwo[i].ToString();
            }
        }
    }
    public void ScoreEnd(int timer, int selected)
    {
        print(selected + "" + timer);
        switch (selected)
        {
            case 0:
                panelRank[0].SetActive(true);
                if (nameListOne.Count == 0)
                {
                    nameListOne.Add(PlayerPrefs.GetString("PlayerName"));
                    timeListOne.Add(timer);
                    RefreshScore();
                    return;
                }
                else if (nameListOne.Contains(PlayerPrefs.GetString("PlayerName")))
                {
                    for (int i = 0; i < nameListOne.Count; i++)
                    {
                        if (nameListOne[i].Contains(PlayerPrefs.GetString("PlayerName")))
                        {
                            nameListOne.RemoveAt(i);
                            timeListOne.RemoveAt(i);
                            int count = 0;
                            for (int j = 0; j < timeListOne.Count; j++)
                            {
                                if (timeListOne[i] > timer)
                                {
                                    nameListOne.Insert(i, PlayerPrefs.GetString("PlayerName"));
                                    timeListOne.Insert(i, timer);
                                    RefreshScore();
                                    return;
                                }
                                else
                                {
                                    count++;
                                    print(count + "-" + timeListOne.Count);
                                    if (count == timeListOne.Count)
                                    {
                                        print("do");
                                        nameListOne.Add(PlayerPrefs.GetString("PlayerName"));
                                        timeListOne.Add(timer);
                                        RefreshScore();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    int count = 0;
                    for (int i = 0; i < timeListOne.Count; i++)
                    {
                        if (timeListOne[i] > timer)
                        {
                            nameListOne.Insert(i, PlayerPrefs.GetString("PlayerName"));
                            timeListOne.Insert(i, timer);
                            RefreshScore();
                            return;
                        }
                        else
                        {
                            count++;
                            print(count + "-" + timeListOne.Count);
                            if (count == timeListOne.Count)
                            {
                                print("do");
                                nameListOne.Add(PlayerPrefs.GetString("PlayerName"));
                                timeListOne.Add(timer);
                                RefreshScore();
                                return;
                            }
                        }
                    }
                }
                break;
            case 1:
                panelRank[1].SetActive(true);
                if (nameListTwo.Count == 0)
                {
                    nameListTwo.Add(PlayerPrefs.GetString("PlayerName"));
                    timeListTwo.Add(timer);
                    return;
                }
                else if (nameListTwo.Contains(PlayerPrefs.GetString("PlayerName")))
                {
                    print("do1");
                    for (int i = 0; i < nameListTwo.Count; i++)
                    {
                        if (nameListTwo[i].Contains(PlayerPrefs.GetString("PlayerName")))
                        {
                            print("do2");
                            nameListTwo.RemoveAt(i);
                            timeListTwo.RemoveAt(i);
                            int count = 0;
                            for (int j = 0; j < timeListTwo.Count; j++)
                            {
                                if (timeListTwo[i] > timer)
                                {
                                    print("do3");
                                    nameListTwo.Insert(i, PlayerPrefs.GetString("PlayerName"));
                                    timeListTwo.Insert(i, timer);
                                    RefreshScore();
                                    return;
                                }
                                else
                                {
                                    count++;
                                    print(count + "-" + timeListTwo.Count);
                                    if (count == timeListTwo.Count)
                                    {
                                        print("do");
                                        nameListTwo.Add(PlayerPrefs.GetString("PlayerName"));
                                        timeListTwo.Add(timer);
                                        RefreshScore();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    int count = 0;
                    for (int i = 0; i < timeListTwo.Count; i++)
                    {
                        if (timeListTwo[i] > timer)
                        {
                            nameListTwo.Insert(i, PlayerPrefs.GetString("PlayerName"));
                            timeListTwo.Insert(i, timer);
                            RefreshScore();
                            return;
                        }
                        else
                        {
                            count++;
                            print(count + "-" + timeListTwo.Count);
                            if (count == timeListTwo.Count)
                            {
                                print("do");
                                nameListTwo.Add(PlayerPrefs.GetString("PlayerName"));
                                timeListTwo.Add(timer);
                                RefreshScore();
                                return;
                            }
                        }
                    }
                }
                break;
        }
    }
    private void RefreshScore()
    {
        for (int f = 0; f < parentRankOne.childCount; f++)
        {
            print($"des{f}");
            Destroy(parentRankOne.GetChild(f).gameObject);
        }
        for (int g = 0; g < nameListOne.Count; g++)
        {
            PlayerPrefs.SetString($"1Name{g}", nameListOne[g]);
            PlayerPrefs.SetInt($"1Time{g}", timeListOne[g]);
            GameObject ins = Instantiate(rankPrefs, parentRankOne, false);
            ins.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = g.ToString();
            ins.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = nameListOne[g];
            ins.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = timeListOne[g].ToString();
        }
        for (int f = 0; f < parentRankTwo.childCount; f++)
        {
            print($"des{f}");
            Destroy(parentRankTwo.GetChild(f).gameObject);
        }
        for (int g = 0; g < nameListTwo.Count; g++)
        {
            PlayerPrefs.SetString($"2Name{g}", nameListTwo[g]);
            PlayerPrefs.SetInt($"2Time{g}", timeListTwo[g]);
            GameObject ins = Instantiate(rankPrefs, parentRankTwo, false);
            ins.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = g.ToString();
            ins.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = nameListTwo[g];
            ins.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = timeListTwo[g].ToString();
        }
    }
    public void ClearRank()
    {
        PlayerPrefs.DeleteAll();
        for (int f = 0; f < parentRankOne.childCount; f++)
        {
            print($"des{f}");
            Destroy(parentRankOne.GetChild(f).gameObject);
        }
        for (int f = 0; f < parentRankTwo.childCount; f++)
        {
            print($"des{f}");
            Destroy(parentRankTwo.GetChild(f).gameObject);
        }
    }
}
