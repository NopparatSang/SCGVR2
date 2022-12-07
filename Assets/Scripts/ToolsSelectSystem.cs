using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class ToolsSelectSystem : MonoBehaviour
    {
        [SerializeField] private GameObject[] panel;
        [SerializeField] private GameObject[] tools;
        [SerializeField] private Transform posTools;
        [SerializeField] private GameObject floor;
        private bool gamerunning = false;
        private int numengine;
        public void GetEngine(int num)
        {
            numengine = num;
            gamerunning = true;
        }
        private void Update()
        {
            if (gamerunning)
            {
                if (OVRInput.GetUp(OVRInput.Button.Three))
                {
                    if (!panel[numengine].activeSelf)
                    {
                        panel[numengine].SetActive(true);
                    }
                    else panel[numengine].SetActive(false);
                }
            }
        }
        public void SelectTool(int numTool)
        {
            tools[numTool].transform.position = posTools.position;
            panel[numengine].SetActive(false);
        }
    }
}
