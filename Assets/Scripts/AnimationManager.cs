using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator anim;
    private PlayerShooting playerShooting;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerShooting = GetComponent<PlayerShooting>();
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
            anim.SetTrigger("Reload");
        }
    }
}
