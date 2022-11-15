using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator anim;
    private PlayerShooting playerShooting;
    [SerializeField] private AudioClip reloadClip;
    private AudioSource reloadAudioSource;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerShooting = GetComponent<PlayerShooting>();
        reloadAudioSource = GetComponents<AudioSource>()[3];
    }
    public void GunSetAnimation(float input)
    {
        if (input > Mathf.Epsilon && !playerShooting.getIsReloading())
            anim.SetBool("Shooting", true);
        else
            anim.SetBool("Shooting", false);
    }
    public void reload()
    {
        if(playerShooting.getIsReloading())
        {
            if(!reloadAudioSource.isPlaying)
                reloadAudioSource.PlayOneShot(reloadClip);
            anim.SetTrigger("Reload");
        }
    }
}
