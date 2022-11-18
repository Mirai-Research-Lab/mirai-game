using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Numerics;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using TMPro;


public class Web3Interactions : MonoBehaviour
{
    [SerializeField] bool isWebGl = false;
    [SerializeField] private string GAME_CONTRACT_ADDRESS = "0x6C383D63dd3DC47Fc36A756d440bF9C7E499e432";
    [SerializeField] private string MIRAI_TOKEN_ADDRESS = "0x20f1d31358308Bbb2d3CA014760FaffF1ec76Ce9";
    [SerializeField] private string network = "goerli";
    [SerializeField] private string chain = "ethereum";
    [SerializeField] private string chainId = "5";
    [SerializeField] private GameObject loadingComponent;
    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] GameObject warningBox;
    [SerializeField] GameObject claimTokenButton;
    private bool isBurnTransactionSuccess = false;
    private BigInteger DECIMALS = BigInteger.Parse("1000000000000000000");
    [SerializeField] private string GAME_CONTRACT_ABI = "";
    private bool isAlreadySignedUp = false;

    //Uncomment the commented Web3Gl when building for WebGl
    private void Awake()
    {
        warningBox.SetActive(false);
    }
    private async void Start()
    {
        claimTokenButton.SetActive(false);
        await CheckIfSignedUp();
        if(!isAlreadySignedUp)
        {
            claimTokenButton.SetActive(true);
        }
        await GetTokenBalance();
    }

    public async Task SignUp()
    {
        if (loadingComponent != null)
            loadingComponent.SetActive(true);
        await InitialSignInWithWebWallet();
        await GetTokenBalance();
        if (loadingComponent != null)
            loadingComponent.SetActive(false);
    }

    private async Task CheckIfSignedUp()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            StartCoroutine(WarningPopUp());
            return; 
        }
        loadingComponent.SetActive(true);
        string method = "getPlayerInfo";
        string account = PlayerPrefs.GetString("Account");
        Debug.Log("Account is : " + account);
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string response = "There was error while fethcing data!";
        try
        {
            response = await EVM.Call(chain, network, GAME_CONTRACT_ADDRESS, GAME_CONTRACT_ABI, method, args);
            Debug.Log(response);
            if(response != "[\"0\",\"0\"]")
                isAlreadySignedUp = true;
            loadingComponent.SetActive(false);
        }
        catch (Exception e)
        {
            print(e);
            Debug.Log(response);
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            loadingComponent.SetActive(false);
            StartCoroutine(WarningPopUp());
        }
        loadingComponent.SetActive(false);
    }
    private async Task InitialSignInWithWebWallet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            StartCoroutine(WarningPopUp());
            return;
        }
        loadingComponent.SetActive(true);
        string method = "signIn";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string data = await EVM.CreateContractData(GAME_CONTRACT_ABI, method, args);
        string response = "There is some kind of error!";
        try
        {
            response = await Web3Wallet.SendTransaction(chainId, GAME_CONTRACT_ADDRESS, "0", data, "", "");
            print(response);
            loadingComponent.SetActive(false);
            claimTokenButton.SetActive(false);
        }
        catch (Exception e)
        {
            print(e.ToString());
            print(response);
            loadingComponent.SetActive(false);
            claimTokenButton.SetActive(true);
        }
        finally
        {
            await GetTokenBalance();
            print(response);
            loadingComponent.SetActive(false);
        }

    }
    private async Task InitialSignInWithWebGlWallet()
    {
        string method = "signIn";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string response = "There is some kind of error!";
        try
        {
            //response = await Web3GL.SendContract(method, GAME_CONTRACT_ABI, GAME_CONTRACT_ADDRESS, args, "0", "", "");
        }
        catch(Exception e)
        {
            print(e);
        }
        finally
        {
            await GetTokenBalance();
            print(response);
        }
    }
    public async Task BuyToken(string weiAmount, GameObject loader = null)
    {
        if (!isWebGl)
        {
            await BuyTokenWalletConnect(weiAmount, loader);
        }
        else
        {
            await BuyTokenWebGlConnect(weiAmount, loader);
        }
    }

    private async Task BuyTokenWalletConnect(string weiAmount, GameObject loader = null)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            StartCoroutine(WarningPopUp());
            return;
        }
        loadingComponent.SetActive(true);
        string method = "buyToken";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string data = await EVM.CreateContractData(GAME_CONTRACT_ABI, method, args);
        string response = "Transaction Failed! Unable To Buy Token!";
        print(weiAmount);
        try
        {
            response = await Web3Wallet.SendTransaction(chainId, GAME_CONTRACT_ADDRESS, weiAmount, data, "", "");
        }
        catch (Exception e)
        {
            loadingComponent.SetActive(false);
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            StartCoroutine(WarningPopUp());
            print(e);
        }
        finally
        {
            print(response);
            loadingComponent.SetActive(true);
            await GetTokenBalance();
            loadingComponent.SetActive(false);
        }
    }
    private async Task BuyTokenWebGlConnect(string weiAmount, GameObject loader = null)
    {
        if (loader != null)
            loader.SetActive(true);
        string method = "buyToken";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string response = "Transaction Failed! Unable To Buy Token!";
        try
        {
            //response = await Web3GL.SendContract(method, GAME_CONTRACT_ABI, GAME_CONTRACT_ADDRESS, args, weiAmount, "", "");
        }
        catch (Exception e)
        {
            if (loader != null)
                loader.SetActive(false);
            print(e);
        }
        finally
        {
            print(response);
            if (loader != null)
                loader.SetActive(true);
            await GetTokenBalance();
            if (loader != null)
                loader.SetActive(false);
        }
    }
    public async Task BurnToken(GameObject loader = null)
    {
        if (!isWebGl)
            await BurnTokenWalletConnect(loader);
        else
            await BurnTokenWebGlConnect(loader);
    }

    private async Task BurnTokenWalletConnect(GameObject loader = null)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            StartCoroutine(WarningPopUp());
            return;
        }
        loadingComponent.SetActive(true);
        string method = "burn";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string data = await EVM.CreateContractData(GAME_CONTRACT_ABI, method, args);
        string response = "Transaction Failed! You don't have enought MRI Tokens!";
        try
        {
            response = await Web3Wallet.SendTransaction(chainId, GAME_CONTRACT_ADDRESS, "0", data, "", "");
            isBurnTransactionSuccess = true;
        }
        catch(Exception e)
        {
            isBurnTransactionSuccess = false;
            loadingComponent.SetActive(false);
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            StartCoroutine(WarningPopUp());
            Debug.Log(e);
        }
        finally
        {
            Debug.Log(response);
            loadingComponent.SetActive(true);
            await GetTokenBalance();
            loadingComponent.SetActive(false);
        }
    }

    private async Task BurnTokenWebGlConnect(GameObject loader = null)
    {
        if (loader != null)
            loader.SetActive(true);
        string method = "burn";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string response = "Transaction Failed! You don't have enought MRI Tokens!";
        try
        {
            //response = await Web3GL.SendContract(method, GAME_CONTRACT_ABI, GAME_CONTRACT_ADDRESS, args, "0", "", "");
            isBurnTransactionSuccess = true;
        }
        catch (Exception e)
        {
            if (loader != null)
                loader.SetActive(false);
            isBurnTransactionSuccess = false;
            print(e);
        }
        finally
        {
            print(response);
            if (loader != null)
                loader.SetActive(true);
            await GetTokenBalance();
            if (loader != null)
                loader.SetActive(false);
        }
    }
    public async Task GetTokenBalance()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            warningText.text = "Transaction Error! Please Check Your Internet Connection!";
            StartCoroutine(WarningPopUp());
            return;
        }
        loadingComponent.SetActive(true);
        if (PlayerPrefs.HasKey("Account"))
            try
            {
                await Task.Delay(15000);
                string account = PlayerPrefs.GetString("Account");
                BigInteger balanceOf = await ERC20.BalanceOf(chain, network, MIRAI_TOKEN_ADDRESS, account);
                float balanceOfInteger = (float)BigInteger.Divide(balanceOf, DECIMALS);
                PlayerPrefs.SetFloat("Token", balanceOfInteger);
                PlayerPrefs.SetString("TokenString", balanceOfInteger.ToString());
                print(balanceOfInteger.ToString());
                loadingComponent.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                loadingComponent.SetActive(false);
                warningText.text = "Transaction Error! Please Check Your Internet Connection!";
                StartCoroutine(WarningPopUp());
            }
    }

    IEnumerator WarningPopUp()
    {
        warningBox.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningBox.SetActive(false);
    }

    public bool getIsBurnTransactionSuccess()
    {
        return isBurnTransactionSuccess;
    }
    public void setIsBurnTransactionSuccess(bool value)
    {
        isBurnTransactionSuccess = value;
    }
}
