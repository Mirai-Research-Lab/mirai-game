using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName ="ScriptableObjects/Gun", order  = 1)]
public class Gun : ScriptableObject
{
    public string gunName;
    public int id;
    public GameObject weaponPrefab;
    public GameObject pickUpPrefab;
}
