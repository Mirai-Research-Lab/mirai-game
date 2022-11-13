using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class WebRequestHandler : MonoBehaviour
{
    [SerializeField] string SIGNIN_ROUTE = "http://localhost:3001/api/auth/signin";
    [SerializeField] string DATAPOST_ROUTE = "http://localhost:3001/api/game/updateScore";
    public static WebRequestHandler instance;
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
    public void submitSignIn(string email, string password, TextMeshProUGUI warningText, GameObject loaderComponent = null)
    {
        // API call
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
    public void postSeissionData(GameObject loaderComponent = null)
    {
        StartCoroutine(PostWinData(loaderComponent));
    }
    IEnumerator PostWinData(GameObject loaderComponent = null)
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
        }
        if(seissionData.email == null || seissionData.email == "")
        {
            if (loaderComponent != null)
                loaderComponent.SetActive(false);
            Debug.Log("not working!");
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
            req.Dispose();
            if (loaderComponent != null)
                loaderComponent.SetActive(false);
            UpdateUi.instance.ShowEndPrompt();
        }
    }
}
