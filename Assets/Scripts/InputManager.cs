using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private PlayerShooting playerShooting;
    private AnimationManager animManager;
    public static bool isShooting = false;
    private void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent
            <PlayerShooting>();
        playerLook = GetComponent<PlayerLook>();
        animManager = GetComponent<AnimationManager>();
        onFoot.Jump.performed += ctx => playerMovement.Jump();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        playerMovement.processMovement(onFoot.Movement.ReadValue<Vector2>());
    }
    private void Update()
    {
        playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        PlayerShooting();
    }

    private void PlayerShooting()
    {
        float shootValue = onFoot.Shoot.ReadValue<float>();
        playerShooting.fire(shootValue);
        animManager.GunSetAnimation(shootValue);
        if (shootValue > Mathf.Epsilon)
            isShooting = true;
        else
            isShooting = false;
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
