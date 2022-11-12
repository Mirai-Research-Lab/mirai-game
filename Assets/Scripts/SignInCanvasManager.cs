using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SignInCanvasManager : MonoBehaviour
{
    [SerializeField] private WebRequestHandler webRequestHandler;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TextMeshProUGUI warningText;

    public void SignIn()
    {
        if (!emailField || !passwordField)
        {
            Debug.LogError("Attach Components in SignInCanvasManager");
        }
        webRequestHandler.submitSignIn(emailField.text, passwordField.text, warningText);
    }
}
