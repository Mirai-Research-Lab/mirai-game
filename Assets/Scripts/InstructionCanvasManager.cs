using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject page1;
    [SerializeField] GameObject page2;

    private void Start()
    {
        page1.SetActive(true);
        page2.SetActive(false);
    }
    public void NextPage()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }
}
