using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class NormalTarget : TargetBase
{
    [SerializeField] private Animator anim;
    [SerializeField] private float selfDestructTime = 3f;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(selfDestruct());
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTime);
        Destroy(gameObject);
    }
    protected override void DestroyTarget()
    {
        if(anim)
        {
            // set some animation
            gameManager.updateTotalPoints(points);
            Destroy(gameObject);
        }
    }

    public void reduceDurability(int amount)
    {
        durability -= amount;
        if (durability <= 0)
            DestroyTarget();
    }

    public int GetDurability()
    {
        return durability;
    }
    public int GetPoints()
    {
        return points;
    }
}
