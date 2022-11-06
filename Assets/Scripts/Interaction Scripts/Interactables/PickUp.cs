using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    [SerializeField] private Gun gunType;

    public Gun getGunScriptable()
    {
        return gunType;
    }
    protected override void Interact(GameObject interactable = null)
    {
        WeaponSwapper.Interacted(interactable);
    }
}
