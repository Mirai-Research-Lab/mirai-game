using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI addressText;
    [SerializeField] private TextMeshProUGUI tokenText;
    [SerializeField] private TMP_InputField ethInputField;
    [SerializeField] private Web3Interactions web3Manager;
    [SerializeField] private GameObject shopContainer;
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private GameObject optionsContainer;
    [SerializeField] private GameObject Loader;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private GameObject warningBox;
    [SerializeField] private GameObject kickPrompt;
    [SerializeField] private GameObject warningBox2;
    [SerializeField] private GameObject warningBox3;
    private const float DECIMALS = 1000000000000000000;
    private Sound[] tracks;
    private void Start()
    {
        kickPrompt.SetActive(false);
        if (!PlayerPrefs.HasKey("Checked"))
        {
            PlayerPrefs.SetInt("Checked", 0);
            CheckIfAnotherUserExists();
        }
        else
        {
            if(PlayerPrefs.GetInt("Checked") == 0)
            {
                CheckIfAnotherUserExists();
            }
        }
        // Set up start screen UI
        ManageInitialUI();
    }

    private void CheckIfAnotherUserExists()
    {
        Loader.SetActive(true);
        WebRequestHandler.instance.CheckIfUserExists(warningBox, Loader, kickPrompt);
    }

    private void ManageInitialUI()
    {
        if (PlayerPrefs.HasKey("Account"))
        {
            string playerAddress = PlayerPrefs.GetString("Account");
            string formatedPlayerAddress = FormatAddressString(playerAddress);
            addressText.text = formatedPlayerAddress;
        }
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        }
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        Loader.SetActive(false);
        tracks = AudioManager.instance.GetTracks();
        warningBox.SetActive(false);
        warningBox2.SetActive(false);
        warningBox3.SetActive(false);
    }

    private static string FormatAddressString(string playerAddress)
    {
        string formatedPlayerAddress = "";
        for (int i = 0; i < 4; i++)
            formatedPlayerAddress += playerAddress[i];
        for (int i = 0; i < 3; i++)
            formatedPlayerAddress += ".";
        for (int i = playerAddress.Length - 5; i < playerAddress.Length; i++)
            formatedPlayerAddress += playerAddress[i];
        return formatedPlayerAddress;
    }

    private void Update()
    {
        AdjustMasterVolume();
        if (PlayerPrefs.HasKey("TokenString"))
            tokenText.text = "Token- " + PlayerPrefs.GetString("TokenString").ToString();
    }

    private void AdjustMasterVolume()
    {
        volumeText.text = (volumeSlider.value * 100f).ToString("0.00");
        if (tracks.Length > 0)
        {
            tracks[0].audioSource.volume = volumeSlider.value;
            tracks[1].audioSource.volume = volumeSlider.value;
        }
    }
    public void OnVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
    public async void buyToken()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            StartCoroutine(WarningPopUp());
            return;
        }
        string amount = "0.0";
        if (ethInputField.text != null && ethInputField.text != "")
            amount = ethInputField.text;
        float amountInFloat = float.Parse(amount);
        if(amountInFloat < 0.02f)
        {
            Debug.Log("You At Least Need to Send 0.01ETH");
            if (Loader != null)
                Loader.SetActive(false);
            return;
        }
        float amountInWei = (amountInFloat * DECIMALS);
        PlayerPrefs.SetString("TokenString", "Updating...");
        await web3Manager.BuyToken(amountInWei.ToString("0.##"), Loader);
    }

    public async void StartGame()
    {
        if (PlayerPrefs.HasKey("Token"))
        {
            if (PlayerPrefs.GetFloat("Token") >= 10f)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    warningText.text = "Transaction Error! Please Check Your Internet Connection!";
                    StartCoroutine(WarningPopUp());
                    return;
                }
                if (Loader != null)
                    Loader.SetActive(true);
                PlayerPrefs.SetString("TokenString", "Updating...");
                await web3Manager.BurnToken();
                if (Loader != null)
                    Loader.SetActive(false);
                if (web3Manager.getIsBurnTransactionSuccess())
                {
                    web3Manager.setIsBurnTransactionSuccess(false);
                    SceneLoader.instance.LoadNextSceneAsync();
                }
            }
            else
            {
                StartCoroutine(Warning3PopUp());
            }
        }
        else
        {
            StartCoroutine(Warning2PopUp());
        }
    }

    public async void ClaimFreeToken()
    {
        PlayerPrefs.SetString("TokenString", "Updating...");
        await web3Manager.SignUp();
    }
    public void StartDemoGame()
    {
        SceneLoader.instance.LoadSceneAsync(5);
    }
    public void OpenShop()
    {
        menuContainer.SetActive(false);
        optionsContainer.SetActive(false);
        shopContainer.SetActive(true);
    }

    public void OpenMenu()
    {
        shopContainer.SetActive(false);
        optionsContainer.SetActive(false);
        menuContainer.SetActive(true);
    }

    public void OpenOptions()
    {
        menuContainer.SetActive(false);
        shopContainer.SetActive(false);
        optionsContainer.SetActive(true);
    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteKey("Account");
        PlayerPrefs.DeleteKey("Token");
        PlayerPrefs.DeleteKey("Email");
        Application.Quit();
    }
    IEnumerator WarningPopUp()
    {
        warningBox.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningBox.SetActive(false);
    }
    IEnumerator Warning2PopUp()
    {
        warningBox2.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningBox2.SetActive(false);
    }
    IEnumerator Warning3PopUp()
    {
        warningBox3.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningBox3.SetActive(false);
    }
}
