using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class CustomWeb3Login : MonoBehaviour
{
    [SerializeField] private bool isWebGl = true;
    [SerializeField] private GameObject loader;
    private WalletLogin walletLogin;
    //Uncomment when building for WebGl
    //private WebLogin webLogin;
    void Start()
    {
        loader.SetActive(false);
        walletLogin = GetComponent<WalletLogin>();
        //webLogin = GetComponent<WebLogin>();
    }
    public void LoginWithWeb3()
    {
        loader.SetActive(true);
        if (!isWebGl)
            walletLogin.OnLogin();
        //else
            //webLogin.OnLogin();
    }
}
