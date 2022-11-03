using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Weapon Stats
    [SerializeField] private int weaponId;
    [SerializeField] private string weaponName;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] GameObject lights;

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }
    private void Update()
    {
        if (InputManager.isShooting)
        {
            ps.Play();
            lights.SetActive(true);
        }
            
        else
        {
            ps.Stop();
            lights.SetActive(false);
        }
            
    }
    // Rest needs to be implemented

}
