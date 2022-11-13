using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeManager : MonoBehaviour
{
    [SerializeField] [Tooltip("In seconds")] private float timeOfPlay = 180f;

    private TargetGeneration targetGeneration;
    private GameManager gameManager;
    public static TimeManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
    private void Start()
    {
        targetGeneration = GetComponent<TargetGeneration>();
        gameManager = GetComponent<GameManager>();
        timeOfPlay += 1f;
    }
    private void Update()
    {
        if(gameManager.GetIsGameStarted())
        {
            UpdateTime();
        }
    }

    private void UpdateTime()
    {
        if (timeOfPlay > 0)
        {
            timeOfPlay -= Time.deltaTime;
        }
        else
        {
            targetGeneration.setIsOver(true);
            timeOfPlay = 0;
        }
    }

    public float GetTime()
    {
        return timeOfPlay;
    }
}
