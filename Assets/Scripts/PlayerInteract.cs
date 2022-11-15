using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 2f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TextMeshProUGUI promptText;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction*distance);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            PickUp interactable = hitInfo.transform.GetComponent<PickUp>();
            if (interactable != null)
            {
                PickUp pickUp = hitInfo.transform.GetComponent<PickUp>();
                if (pickUp != null)
                {
                    promptText.gameObject.SetActive(true);
                    promptText.text = interactable.prompyMessage.ToString() + " " + pickUp.getGunScriptable().gunName;
                }
                if (inputManager.playerInput.OnFoot.Interact.triggered)
                {
                    interactable.BaseInteract(interactable.gameObject);
                }
            }
        }
        else
        {
            promptText.text = "";
            promptText.gameObject.SetActive(false);
        }
    }
}
