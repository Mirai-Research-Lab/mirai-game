using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NormalTarget : TargetBase
{
    [SerializeField] private float selfDestructTime = 3f;
    [SerializeField] private GameObject shatteredTarget;
    [SerializeField] private GameObject intactTarget;
    private bool isItDestroyed = false;
    private void Start()
    {
        shatteredTarget.SetActive(false);
        intactTarget.SetActive(true);
        StartCoroutine(selfDestruct());
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTime);
        if(!isItDestroyed)
            Destroy(gameObject);
    }
    protected override void DestroyTarget(Vector3 lastHitPoint)
    {
        isItDestroyed = true;
        // set some animation
        gameManager.updateTotalPoints(points);
        intactTarget.SetActive(false);
        shatteredTarget.SetActive(true);
        GetComponent<SphereCollider>().enabled = false;
        foreach(Transform child in shatteredTarget.transform)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRb))
            {
                childRb.AddExplosionForce(100f, lastHitPoint, 4f);
            }
        }
        Destroy(gameObject, 0.2f);   
    }

    public void reduceDurability(int amount, Vector3 lastHitPoint)
    {
        durability -= amount;
        if (durability <= 0)
            DestroyTarget(lastHitPoint);
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
