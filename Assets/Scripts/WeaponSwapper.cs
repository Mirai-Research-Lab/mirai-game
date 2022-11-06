using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapper : MonoBehaviour
{
    private static Transform weaponHolder;

    private void Start()
    {
        weaponHolder = GameObject.FindWithTag("Holder").transform;
    }
    public static void Interacted(GameObject interactable)
    {
        // Implement Code to Swap weapons
        Weapon[] weapons = weaponHolder.GetComponentsInChildren<Weapon>();
        Debug.Log(weapons.Length);
        if (weapons.Length == 0)
        {
            GameObject weapon = Instantiate(interactable.GetComponent<PickUp>().getGunScriptable().weaponPrefab,
                weaponHolder.position, weaponHolder.rotation);
            weapon.transform.parent = weaponHolder;
        }
        else
        {
            Gun currentGunScriptable = weapons[0].getGunScriptable();
            Gun newGunScriptable = interactable.GetComponent<PickUp>().getGunScriptable();
            if(currentGunScriptable.id != newGunScriptable.id)
            {
                foreach(Transform t in weaponHolder)
                {
                    Destroy(t.gameObject);
                }
                GameObject weapon = Instantiate(newGunScriptable.weaponPrefab,
                weaponHolder.position, weaponHolder.rotation);
                weapon.transform.parent = weaponHolder;
            }
        }
    }
}
