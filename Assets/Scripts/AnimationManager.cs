using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }
    public void GunSetAnimation(float input)
    {
        if (input > Mathf.Epsilon)
            anim.SetBool("Shooting", true);
        else
            anim.SetBool("Shooting", false);
    }
}
