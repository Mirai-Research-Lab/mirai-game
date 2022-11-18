using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pointsTextHolder;
    [SerializeField] private bool isGameStarted;
    [SerializeField] private UpdateUi updateUi;
    [SerializeField] private float[] timesToIncreaseSimultaneousTargets;
    [SerializeField] private InputManager inputManager;
    private TargetGeneration targetGeneration;
    private TimeManager timeManager;
    private float startingTime;
    private float totalPoints;
    private float pointsEarnedInGame;
    private bool isPaused;
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }
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
        isPaused = false;
        pauseMenu.SetActive(isPaused);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateGameState();
    }
    private void UpdateGameState()
    {
        pointsTextHolder.SetActive(isGameStarted);
        pointsText.text = pointsEarnedInGame.ToString();
        if (isGameStarted)
        {
            float currentTime = timeManager.GetTime();
            float remainingTime = startingTime - currentTime + 1;
            if (2 < timesToIncreaseSimultaneousTargets.Length && remainingTime > timesToIncreaseSimultaneousTargets[2])
                targetGeneration.setAllowedSimultaneousTarget(4);
            else if (1 < timesToIncreaseSimultaneousTargets.Length && remainingTime > timesToIncreaseSimultaneousTargets[1])
                targetGeneration.setAllowedSimultaneousTarget(3);
            else if (0 < timesToIncreaseSimultaneousTargets.Length && remainingTime > timesToIncreaseSimultaneousTargets[0])
                targetGeneration.setAllowedSimultaneousTarget(2);
        }
    }
    
    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            pauseMenu.SetActive(isPaused);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
            pauseMenu.SetActive(isPaused);
        }
    }
    public void updateTotalPoints(float amount = 0f)
    {
        totalPoints += amount;
        pointsEarnedInGame = totalPoints;
        if (pointsEarnedInGame <= 0)
            pointsEarnedInGame = 0;
        if (totalPoints <= 0)
            totalPoints = 0;
    }
    
    public void addBonusPoints(float amount)
    {
        totalPoints += amount;
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
        if(hit.transform != null)
            Destroy(hit.transform.gameObject);
        isGameStarted = true;
    }

    public bool getIsPaused()
    {
        return isPaused;
    }
}
