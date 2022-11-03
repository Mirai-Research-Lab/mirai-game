using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpPower = 3f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        isGrounded = controller.isGrounded;
    }
    public void processMovement(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        controller.Move(transform.TransformDirection(moveDir * speed * Time.deltaTime));
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -1.5f;
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);
    }

    public void Jump()
    {
        Debug.Log("Jump was called!");
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -jumpPower * gravity);
        }
    }
}
