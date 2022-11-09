using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    [Header("Ending Prompt Objects")]
    [SerializeField] private GameObject endContainer;
    [SerializeField] TextMeshProUGUI scorePointsText;
    [SerializeField] TextMeshProUGUI shotsHitText;
    [SerializeField] TextMeshProUGUI totalShotsText;
    [SerializeField] TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI bonusPointsText;
    private const float BONUS_POINT_MULTIPLIER = 10f;
    private int addBonus = 0;

    private Weapon weapon;
    private void Start()
    {
        if (ammoBox == null || ammoText == null || playerTransform == null)
            Debug.LogError("UI elements not assigned");
        ammoBox.SetActive(false);
        timeContainer.SetActive(false);
    }

    private void Update()
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
        float accuracy = ((float)playerShooting.getShotsOnTarget() / (float)playerShooting.getShotsFired() * 100f);
        float truncatedAccuracy = Mathf.Round(accuracy * 100f) / 100f;
        if (addBonus == 0)
        {
            timeManager.GetComponent<GameManager>().updateTotalPoints(truncatedAccuracy * BONUS_POINT_MULTIPLIER);
            addBonus++;
        }
        accuracyText.text = truncatedAccuracy.ToString("F2")+"%";
        bonusPointsText.text = (truncatedAccuracy * BONUS_POINT_MULTIPLIER).ToString();
        scorePointsText.text = (timeManager.GetComponent<GameManager>().getTotalPoints()+ truncatedAccuracy * BONUS_POINT_MULTIPLIER).ToString();
        Time.timeScale = 0f;
    }
}
