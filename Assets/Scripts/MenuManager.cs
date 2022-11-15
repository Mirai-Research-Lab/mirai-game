using UnityEngine.UI;
using UnityEngine;
using TMPro;
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
    private const float DECIMALS = 1000000000000000000;
    private Sound[] tracks;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Account"))
        {
            string playerAddress = PlayerPrefs.GetString("Account");
            string formatedPlayerAddress = FormatAddressString(playerAddress);
            addressText.text = formatedPlayerAddress;
        }
        if(!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        }
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        Loader.SetActive(false);
        tracks = AudioManager.instance.GetTracks();
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
        if (PlayerPrefs.HasKey("Token"))
            tokenText.text = "Token- " + PlayerPrefs.GetFloat("Token").ToString();
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
        await web3Manager.BuyToken(amountInWei.ToString("0.##"), Loader);
    }

    public async void StartGame()
    {
        if (Loader != null)
            Loader.SetActive(true);
        await web3Manager.BurnToken();
        if (Loader != null)
            Loader.SetActive(false);
        if(web3Manager.getIsBurnTransactionSuccess())
        {
            web3Manager.setIsBurnTransactionSuccess(false);
            SceneLoader.instance.LoadNextSceneAsync();
        }
            
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
}
