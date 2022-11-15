using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUser : MonoBehaviour
{
    [SerializeField] private bool resetUser;
    void Start()
    {
        if (resetUser)
        {
            PlayerPrefs.DeleteKey("Account");
        }
    }
}
