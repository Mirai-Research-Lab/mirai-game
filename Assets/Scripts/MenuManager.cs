using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI addressText;
    [SerializeField] private TextMeshProUGUI tokenText;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Account"))
            addressText.text = PlayerPrefs.GetString("Account");    
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("Token"))
            tokenText.text = PlayerPrefs.GetString("Token");
    }
}
