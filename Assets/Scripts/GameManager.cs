using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private GameObject pointsTextHolder;
    [SerializeField] private bool isGameStarted;
    [SerializeField] private UpdateUi updateUi;
    [SerializeField] private float[] timesToIncreaseSimultaneousTargets;
    private TargetGeneration targetGeneration;
    private TimeManager timeManager;
    private float startingTime;
    private float totalPoints;
    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 1f;
        if (pointsText == null || pointsTextHolder == null)
            Debug.LogError("Components not assigned in Gamemanager");
        totalPoints = 0;
        isGameStarted = false;
        targetGeneration = GetComponent<TargetGeneration>();
        timeManager = GetComponent<TimeManager>();
        pointsTextHolder.SetActive(isGameStarted);
        startingTime = timeManager.GetTime();
    }

    // Update is called once per frame
    private void Update()
    {
        pointsTextHolder.SetActive(isGameStarted);
        pointsText.text = totalPoints.ToString();
        if (isGameStarted)
        {
            float currentTime = timeManager.GetTime();
            float remainingTime = startingTime - currentTime +1;
            Debug.Log(remainingTime);
            if (2 < timesToIncreaseSimultaneousTargets.Length && remainingTime > timesToIncreaseSimultaneousTargets[2])
                targetGeneration.setAllowedSimultaneousTarget(4);
            else if (1 < timesToIncreaseSimultaneousTargets.Length && remainingTime > timesToIncreaseSimultaneousTargets[1])
                targetGeneration.setAllowedSimultaneousTarget(3);
            else if (0 < timesToIncreaseSimultaneousTargets.Length && remainingTime > timesToIncreaseSimultaneousTargets[0])
                targetGeneration.setAllowedSimultaneousTarget(2);
        } 

    }

    public void updateTotalPoints(float amount = 20f)
    {
        totalPoints += amount;
        if (totalPoints <= 0)
            totalPoints = 0;
    }
    
    public float getTotalPoints()
    {
        return totalPoints;
    }
    public bool GetIsGameStarted()
    {
        return isGameStarted;
    }

    public void StartGame(RaycastHit hit)
    {
        Debug.Log(hit.transform.name);
        StartCoroutine(StartGameCoroutine(hit));
    }

    IEnumerator StartGameCoroutine(RaycastHit hit)
    {
        float timeBeforeStart = 3f;
        while (timeBeforeStart-1 > 0)
        {
            timeBeforeStart -= Time.deltaTime;
            updateUi.UpdateGameStartCounter(timeBeforeStart);
            yield return null;
        }
        Destroy(hit.transform.gameObject);
        isGameStarted = true;
    }
}
