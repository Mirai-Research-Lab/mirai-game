using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Weapon Stats
    [SerializeField] private Gun gunProps;
    private int totalAmmo;
    private int currentAmmo;
    // Reference Objects
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private GameObject lights;
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float fireRate = 5f;
    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        totalAmmo = gunProps.ammo;
        currentAmmo = totalAmmo;
    }
    private void Update()
    {
        if (InputManager.isShooting && currentAmmo > 0)
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
    public Gun getGunScriptable()
    {
        return gunProps;
    }
    public void reduceAmmo()
    {
        currentAmmo--;
    }
    public void resetAmmo()
    {
        currentAmmo = totalAmmo;
    }
    public int getCurrentAmmo()
    {
        return currentAmmo;
    }
    public int getDamageAmount()
    {
        return damageAmount;
    }
    public float getFireRate()
    {
        return fireRate;
    }
}
