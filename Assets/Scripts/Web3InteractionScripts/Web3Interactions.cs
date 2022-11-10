using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Numerics;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

public class Web3Interactions : MonoBehaviour
{
    [SerializeField] bool isWebGl = false;
    private const string GAME_CONTRACT_ADDRESS = "0x6C383D63dd3DC47Fc36A756d440bF9C7E499e432";
    private const string MIRAI_TOKEN_ADDRESS = "0x20f1d31358308Bbb2d3CA014760FaffF1ec76Ce9";
    private string network = "goerli";
    private string chain = "ethereum";
    private string GAME_CONTRACT_ABI = "";
    private string chainId = "5";
    private void Awake()
    {
        string gameContractAbi = File.ReadAllText(Application.dataPath + "/Constants/FrontEndAbiLocation/GameContract.json");
        print(gameContractAbi.ToString());
        GAME_CONTRACT_ABI = gameContractAbi.ToString();
    }
    private async void Start()
    {
        await InitialSignIn();
    }

    private async Task InitialSignIn()
    {
        string method = "signIn";
        string account = PlayerPrefs.GetString("Account");
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string data = await EVM.CreateContractData(GAME_CONTRACT_ABI, method, args);
        print(data);
        string response = "";
        try
        {
            response = await Web3Wallet.SendTransaction(chainId, GAME_CONTRACT_ADDRESS, "0", data, "", "");
            print(response);
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
        finally
        {
            print(response);
            await GetTokenBalance();
        }
    }

    private async Task GetTokenBalance()
    {
        string account = PlayerPrefs.GetString("Account");
        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, MIRAI_TOKEN_ADDRESS, account);
        PlayerPrefs.SetString("Token", balanceOf.ToString());
        print(balanceOf.ToString());

    }
}
