using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;
public class WebRequestHandler : MonoBehaviour
{
    [SerializeField] string SIGNIN_ROUTE = "http://localhost:3001/api/auth/signin";
    [SerializeField] string DATAPOST_ROUTE = "http://localhost:3001/api/game/updateScore";
    [SerializeField] string CHECK_EMAIL_ROUTE = "https://mirai-backend-kappa.vercel.app/api/wallet/gameWalletCheck";
    [SerializeField] string SET_EMAIL_ROUTE = "https://mirai-backend-kappa.vercel.app/api/player/updateAddress";
    public static WebRequestHandler instance;

    private const string ADD_NEW_WALLET_RESPONSE = "Added New wallet";
    private const string WALLET_EXISTS = "Wallet exists";
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(this);
    }
    public class UserData
    {
        public string email;
        public string password;
    }

    public class SeissionData
    {
        public float score;
        public string email;
    }

    public class CheckData
    {
        public string email;
        public string address;
    }
    public void submitSignIn(string email, string password, TextMeshProUGUI warningText, GameObject loaderComponent = null)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            warningText.text = "ERROR! Make Sure You Are Connected To The Internet!";
            return;
        }
        StartCoroutine(signIn(email, password, warningText, loaderComponent));
    }   
    IEnumerator signIn(string email, string password, TextMeshProUGUI warningText, GameObject loaderComponent = null)
    {
        if (loaderComponent != null)
            loaderComponent.SetActive(true);
        if (email == "" || email == null || password == "" || password == null)
        {
            warningText.text = "ERROR! Invalid Credentials Passed! Please Enter Credentials Correctly and visit our website!";
            yield return null;
        }
        else
        {
            var user = new UserData();
            user.email = email;
            user.password = password;

            string jsonData = JsonUtility.ToJson(user);

            var req = new UnityWebRequest(SIGNIN_ROUTE, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

            req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            float timeStart = 0f;
            var handler = req.SendWebRequest();
            while (!handler.isDone)
            {
                timeStart += Time.deltaTime;
                if (timeStart > 10f)
                {
                    if (loaderComponent != null)
                        loaderComponent.SetActive(false);
                    req.Dispose();
                    break;
                }
                yield return null;
            }
            if (req.result == UnityWebRequest.Result.Success)
            {
                var jsonResponse = JsonUtility.FromJson<PlayerModel>(req.downloadHandler.text);
                PlayerPrefs.SetString("Email", jsonResponse.email);
                PlayerPrefs.SetString("PrevHighestScore", jsonResponse.highest_score.ToString());
                SceneLoader.instance.LoadNextSceneAsync();
                req.Dispose();
            }
            else
            {
                warningText.text = "ERROR! User Does Not Exist! Please go to our website and sign up first!";
                req.Dispose();
            }
            req.Dispose();
        }
        if (loaderComponent != null)
            loaderComponent.SetActive(false);
    }
    public void postSeissionData(GameObject loaderComponent = null, GameObject warningBox = null)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            StartCoroutine(WarningPopUp(warningBox));
            UpdateUi.instance.ShowEndPrompt();
            return;
        }
        StartCoroutine(PostWinData(loaderComponent, warningBox));
    }
    IEnumerator PostWinData(GameObject loaderComponent = null, GameObject warningBox = null)
    {
        if (loaderComponent != null)
            loaderComponent.SetActive(true);
        float totalPoints = GameManager.instance.getTotalPoints();
        var seissionData = new SeissionData();
        if (PlayerPrefs.HasKey("Email"))
        {
            seissionData.email = PlayerPrefs.GetString("Email");
            Debug.Log(seissionData.email);
        }
        else
        {
            Debug.LogError("No Email Found!");
            if(warningBox != null)
                StartCoroutine(WarningPopUp(warningBox));
        }
        if(seissionData.email == null || seissionData.email == "")
        {
            if (loaderComponent != null)
                loaderComponent.SetActive(false);
            Debug.Log("not working!");
            if (warningBox != null)
                StartCoroutine(WarningPopUp(warningBox));
            UpdateUi.instance.ShowEndPrompt();
            yield return null;
        }
        else
        {
            seissionData.score = totalPoints;
            string jsonData = JsonUtility.ToJson(seissionData);

            var req = new UnityWebRequest(DATAPOST_ROUTE, "PUT");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

            req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            float timeStart = 0f;
            var handler = req.SendWebRequest();
            while (!handler.isDone)
            {
                timeStart += Time.deltaTime;
                if (timeStart > 10f)
                {
                    if (loaderComponent != null)
                        loaderComponent.SetActive(false);
                    if (warningBox != null)
                        StartCoroutine(WarningPopUp(warningBox));
                    req.Dispose();
                    break;
                }
                yield return null;
            }
            if (req.result == UnityWebRequest.Result.Success)
            {
                var jsonResponse = JsonUtility.FromJson<PlayerModel>(req.downloadHandler.text);
                PlayerPrefs.SetString("PrevHighestScore", jsonResponse.highest_score.ToString());
                req.Dispose();
            }
            else
            {
                if (warningBox != null)
                    StartCoroutine(WarningPopUp(warningBox));
            }
            req.Dispose();
            if (loaderComponent != null)
                loaderComponent.SetActive(false);
            UpdateUi.instance.ShowEndPrompt();
        }
    }

    public void CheckIfUserExists(GameObject warningBox, GameObject loaderComponent=null, GameObject kickPrompt = null)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable && warningBox != null)
        {
            StartCoroutine(WarningPopUp(warningBox));
            UpdateUi.instance.ShowEndPrompt();
            return;
        }
        StartCoroutine(SendCheckRequest(loaderComponent, warningBox, kickPrompt));
    }

    IEnumerator SendCheckRequest (GameObject loaderComponent, GameObject warningBox, GameObject kickPrompt)
    {
        if (loaderComponent != null)
            loaderComponent.SetActive(true);
        var checkData = new CheckData();
        if (PlayerPrefs.HasKey("Email"))
        {
            checkData.email = PlayerPrefs.GetString("Email");
            Debug.Log(checkData.email);
        }
        checkData.address = PlayerPrefs.GetString("Account");
        string jsonData = JsonUtility.ToJson(checkData);

        var req = new UnityWebRequest(CHECK_EMAIL_ROUTE, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        float timeStart = 0f;
        var handler = req.SendWebRequest();
        while (!handler.isDone)
        {
            timeStart += Time.deltaTime;
            if (timeStart > 10f)
            {
                if (loaderComponent != null)
                    loaderComponent.SetActive(false);
                PlayerPrefs.SetInt("Checked", 0);
                req.Dispose();
                break;
            }
            yield return null;
        }
        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("The result is:" + req.downloadHandler.text);
            PlayerPrefs.SetInt("Checked", 1);
            if(req.downloadHandler.text == ADD_NEW_WALLET_RESPONSE)
            {
                StartCoroutine(PostNewWalletAdded(loaderComponent, warningBox, kickPrompt));
            }
        }
        else
        {
            Debug.Log("The error is : " + req.downloadHandler.text);
            PlayerPrefs.SetInt("Checked", 0);
            ShowOnExistPrompt(kickPrompt);
        }
        if (loaderComponent != null)
            loaderComponent.SetActive(false);
        req.Dispose();
    }

    IEnumerator PostNewWalletAdded(GameObject loaderComponent, GameObject warningBox, GameObject kickPrompt)
    {
        if (loaderComponent != null)
            loaderComponent.SetActive(true);
        var checkData = new CheckData();
        if (PlayerPrefs.HasKey("Email"))
        {
            checkData.email = PlayerPrefs.GetString("Email");
        }
        checkData.address = PlayerPrefs.GetString("Account");
        string jsonData = JsonUtility.ToJson(checkData);

        var req = new UnityWebRequest(SET_EMAIL_ROUTE, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        float timeStart = 0f;
        var handler = req.SendWebRequest();
        while (!handler.isDone)
        {
            timeStart += Time.deltaTime;
            if (timeStart > 10f)
            {
                if (loaderComponent != null)
                    loaderComponent.SetActive(false);
                StartCoroutine(WarningPopUp(warningBox));
                req.Dispose();
                break;
            }
            yield return null;
        }
        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sucessfully added");
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            Debug.Log("Error while adding");
        }
        if (loaderComponent != null)
            loaderComponent.SetActive(false);
        req.Dispose();
    }
    IEnumerator WarningPopUp(GameObject warningBox)
    {
        warningBox.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningBox.SetActive(false);
    }

    private void ShowOnExistPrompt(GameObject kickPrompt)
    {
        kickPrompt.SetActive(true);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Account");
        PlayerPrefs.DeleteKey("Email");
        PlayerPrefs.DeleteKey("Checked");
        PlayerPrefs.DeleteKey("Token");
    }
}
