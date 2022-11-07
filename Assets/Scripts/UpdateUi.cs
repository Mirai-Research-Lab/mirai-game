using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateUi : MonoBehaviour
{
    [SerializeField] private GameObject ammoBox;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Transform playerTransform;
    private Weapon weapon;

    private void Start()
    {
        if (ammoBox == null || ammoText == null || playerTransform == null)
            Debug.LogError("UI elements not assigned");
        ammoBox.SetActive(false);
    }

    private void LateUpdate()
    {
        weapon = playerTransform.GetComponentInChildren<Weapon>();
        if(weapon != null)
        {
            ammoBox.SetActive(true);
            ammoText.text = weapon.getCurrentAmmo().ToString();
        }
        else
        {
            ammoBox.SetActive(false);
        }
    }
}
