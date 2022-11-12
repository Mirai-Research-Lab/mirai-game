using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class WebRequestHandler : MonoBehaviour
{
    [SerializeField] string SIGNIN_ROUTE = "http://localhost:3001/api/auth/signin";
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
    public void submitSignIn(string email, string password, TextMeshProUGUI warningText)
    {
        // API call
        StartCoroutine(signIn(email, password, warningText));
    }   
    IEnumerator signIn(string email, string password, TextMeshProUGUI warningText)
    {
        if(email == "" || email == null || password == "" || password == null)
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
                    break;
                yield return null;
            }
            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(req.downloadHandler.text);
                var jsonResponse = JsonUtility.FromJson<PlayerModel>(req.downloadHandler.text);
                Debug.Log(jsonResponse.email);
                PlayerPrefs.SetString("Email", jsonResponse.email);
                SceneLoader.instance.LoadNextSceneAsync();
            }
            else
            {
                warningText.text = "ERROR! User Does Not Exist! Please go to our website and sign up first!";
            }
            req.Dispose();
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
