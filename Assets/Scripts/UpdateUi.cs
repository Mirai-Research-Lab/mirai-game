using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using System.Collections;
public class UpdateUi : MonoBehaviour
{
    [SerializeField] private GameObject ammoBox;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject timeContainer;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private TextMeshPro startTimeCountdownText;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private GameObject loader;
    [SerializeField] private GameObject warningBox;
    [Header("Pause Menu Objects")]
    [SerializeField] private Slider sensiSlider;
    [SerializeField] private TextMeshProUGUI sensiText;
    [Header("Ending Prompt Objects")]
    [SerializeField] private GameObject endContainer;
    [SerializeField] TextMeshProUGUI scorePointsText;
    [SerializeField] TextMeshProUGUI shotsHitText;
    [SerializeField] TextMeshProUGUI totalShotsText;
    [SerializeField] TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI bonusPointsText;
    [SerializeField] TextMeshProUGUI prevHighestScoreText;
    [SerializeField] AudioClip clip;
    [SerializeField] private bool isUsingWeb = true;
    private AudioSource source;
    private const float BONUS_POINT_MULTIPLIER = 10f;
    private int addBonus = 0;
    private PlayerLook playerLook;
    private Weapon weapon;
    private int count = 0;
    public static UpdateUi instance;
    private float truncatedAccuracy;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
    private void Start()
    {
        if (ammoBox == null || ammoText == null || playerTransform == null)
            Debug.LogError("UI elements not assigned");
        ammoBox.SetActive(false);
        timeContainer.SetActive(false);
        playerLook = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>();
        source = GetComponent<AudioSource>();
        loader.SetActive(false);
        endContainer.SetActive(false);
        warningBox.SetActive(false);
    }

    private void Update()
    {
        UpdateTimeUi();
        UpdateSensitivityUsingUI();
        if(count == 0 && TimeManager.instance.GetTime() <= 0f)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                StartCoroutine(WarningPopUp());
                return;
            }
            count++;
            float accuracy = ((float)playerShooting.getShotsOnTarget() / (float)playerShooting.getShotsFired() * 100f);
            truncatedAccuracy = Mathf.Round(accuracy * 100f) / 100f;
            timeManager.GetComponent<GameManager>().addBonusPoints(truncatedAccuracy * BONUS_POINT_MULTIPLIER);
            addBonus++;
            if (isUsingWeb)
                WebRequestHandler.instance.postSeissionData(loader, warningBox);
            else
                ShowEndPrompt();
        }
    }

    private void UpdateSensitivityUsingUI()
    {
        if (GameManager.instance.getIsPaused())
        {
            double sensitivityAmount = (double) sensiSlider.value * 10f;
            float valueToShow = (float) Math.Truncate(100 * sensiSlider.value) / 100;
            playerLook.setSensitivity((float)sensitivityAmount);
            sensiText.text = valueToShow.ToString();
        }
    }

    private void UpdateTimeUi()
    {
        if (timeManager.transform.GetComponent<GameManager>().GetIsGameStarted())
        {
            timeContainer.SetActive(true);
            float timeOfPlay = timeManager.GetTime();
            timeText.text = FormatTime(timeOfPlay);
        }
    }

    private void LateUpdate()
    {
        weapon = playerTransform.GetComponentInChildren<Weapon>();
        if(weapon != null)
        {
            ammoBox.SetActive(true);
            ammoText.text = weapon.getCurrentAmmo().ToString();
        }
        else
        {
            ammoBox.SetActive(false);
        }
    }
    private string FormatTime(float timeAmount)
    {
        int minutesLeft = Mathf.FloorToInt(timeAmount / 60);
        int secondsLeft = Mathf.FloorToInt(timeAmount % 60);
        string formattedString = string.Format("{0:00}:{1:00}", minutesLeft, secondsLeft);
        return formattedString;
    }

    public void UpdateGameStartCounter(float timeBeforeStart)
    {
        if (startTimeCountdownText != null)
            startTimeCountdownText.text = "0"+string.Format("{0:0}", (timeBeforeStart % 60));
    }

    public void ShowEndPrompt()
    {
        Cursor.lockState = CursorLockMode.None;
        endContainer.SetActive(true);
        shotsHitText.text = playerShooting.getShotsOnTarget().ToString();
        totalShotsText.text = playerShooting.getShotsFired().ToString();
        prevHighestScoreText.text = PlayerPrefs.GetString("PrevHighestScore");
        accuracyText.text = truncatedAccuracy.ToString("F2")+"%";
        bonusPointsText.text = (truncatedAccuracy * BONUS_POINT_MULTIPLIER).ToString();
        scorePointsText.text = (timeManager.GetComponent<GameManager>().getTotalPoints()).ToString();
    }
    
    public void PlayClickAudio()
    {
        source.PlayOneShot(clip);
    }
    public void LoadMenu()
    {
        SceneLoader.instance.LoadMenuAsync();
    }

    IEnumerator WarningPopUp()
    {
        warningBox.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningBox.SetActive(false);
        ShowEndPrompt();
    }
}
