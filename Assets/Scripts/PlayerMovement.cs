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
    [SerializeField] private AudioClip jumpClip;
    private AudioSource walkAudioSource;
    private AudioSource jumpAudioSource;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        walkAudioSource = GetComponents<AudioSource>()[1];
        jumpAudioSource = GetComponents<AudioSource>()[2];
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
        if ((Mathf.Abs(input.x) > Mathf.Epsilon || Mathf.Abs(input.y) > Mathf.Epsilon) && isGrounded)
        {
            if(!walkAudioSource.isPlaying)
                walkAudioSource.Play();
        }   
        else
            walkAudioSource.Stop();

        controller.Move(transform.TransformDirection(moveDir * speed * Time.deltaTime));
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -1.5f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -jumpPower * gravity);
            if (jumpClip != null)
                jumpAudioSource.PlayOneShot(jumpClip);
        }
    }
}
