using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipManager : MonoBehaviour
{
    public void PlayClick()
    {
        AudioManager.instance.PlayOneShot("Click");
    }

    public void PlayAk()
    {
        AudioManager.instance.PlayOneShot("Ak");
    }

    public void PlayBull()
    {
        AudioManager.instance.PlayOneShot("Bull");
    }
}
