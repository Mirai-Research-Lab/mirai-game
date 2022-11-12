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

    private BigInteger DECIMALS = BigInteger.Parse("1000000000000000000");
    private string GAME_CONTRACT_ABI = "";
    private bool isAlreadySignedUp = false;
    private void Awake()
    {
        string gameContractAbi = File.ReadAllText(Application.dataPath + "/Constants/FrontEndAbiLocation/GameContract.json");
        print(gameContractAbi.ToString());
        GAME_CONTRACT_ABI = gameContractAbi.ToString();
    }
    private async void Start()
    {
        await CheckIfSignedUp();
        if(!isAlreadySignedUp)
        {
            await SignUp();
        }
        await GetTokenBalance();
    }

    private async Task SignUp()
    {
        if (!isWebGl)
        {
            await InitialSignInWithWebWallet();
        }
        else
        {
            await InitialSignInWithWebGlWallet();
        }
    }

    private async Task CheckIfSignedUp()
    {
        string method = "getPlayerInfo";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string response = "There was error while fethcing data!";
        try
        {
            response = await EVM.Call(chain, network, GAME_CONTRACT_ADDRESS, GAME_CONTRACT_ABI, method, args);
            isAlreadySignedUp = true;
        }
        catch (Exception e)
        {
            print(e);
        }
        finally
        {
            print(response);
        }
    }
    private async Task InitialSignInWithWebWallet()
    {
        string method = "signIn";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string data = await EVM.CreateContractData(GAME_CONTRACT_ABI, method, args);
        string response = "There is some kind of error!";
        try
        {
            response = await Web3Wallet.SendTransaction(chainId, GAME_CONTRACT_ADDRESS, "0", data, "", "");
        }
        catch (Exception e)
        {
            print(e.ToString());
            await GetTokenBalance();
        }
        finally
        {
            print(response);
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
            response = await Web3GL.SendContract(method, GAME_CONTRACT_ABI, GAME_CONTRACT_ADDRESS, args, "0", "", "");
        }
        catch(Exception e)
        {
            print(e);
        }
        finally
        {
            print(response);
        }
    }
    public async Task BuyToken(string weiAmount)
    {
        if (!isWebGl)
        {
            await BuyTokenWalletConnect(weiAmount);
        }
        else
        {
            await BuyTokenWebGlConnect(weiAmount);
        }
    }

    private async Task BuyTokenWalletConnect(string weiAmount)
    {
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
            print(e);
        }
        finally
        {
            print(response);
            await GetTokenBalance();
        }
    }
    private async Task BuyTokenWebGlConnect(string weiAmount)
    {
        string method = "buyToken";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string response = "Transaction Failed! Unable To Buy Token!";
        try
        {
            response = await Web3GL.SendContract(method, GAME_CONTRACT_ABI, GAME_CONTRACT_ADDRESS, args, weiAmount, "", "");
        }
        catch (Exception e)
        {
            print(e);
        }
        finally
        {
            print(response);
            await GetTokenBalance();
        }
    }
    public async Task BurnToken()
    {
        if (!isWebGl)
            await BurnTokenWalletConnect();
        else
            await BurnTokenWebGlConnect();
    }

    private async Task BurnTokenWalletConnect()
    {
        string method = "burn";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string data = await EVM.CreateContractData(GAME_CONTRACT_ABI, method, args);
        string response = "Transaction Failed! You don't have enought MRI Tokens!";
        try
        {
            response = await Web3Wallet.SendTransaction(chainId, GAME_CONTRACT_ADDRESS, "0", data, "", "");
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            Debug.Log(response);
            await GetTokenBalance();
        }
    }

    private async Task BurnTokenWebGlConnect()
    {
        string method = "burn";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string response = "Transaction Failed! You don't have enought MRI Tokens!";
        try
        {
            response = await Web3GL.SendContract(method, GAME_CONTRACT_ABI, GAME_CONTRACT_ADDRESS, args, "0", "", "");
        }
        catch (Exception e)
        {
            print(e);
        }
        finally
        {
            print(response);
            await GetTokenBalance();
        }
    }
    public async Task GetTokenBalance()
    {
        try
        {
            string account = PlayerPrefs.GetString("Account");
            BigInteger balanceOf = await ERC20.BalanceOf(chain, network, MIRAI_TOKEN_ADDRESS, account);
            float balanceOfInteger = (float)BigInteger.Divide(balanceOf, DECIMALS);
            PlayerPrefs.SetFloat("Token", balanceOfInteger);
            print(balanceOfInteger.ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
