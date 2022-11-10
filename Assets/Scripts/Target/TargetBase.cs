using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class TargetBase : MonoBehaviour
{
    [SerializeField] protected int durability = 100;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected int points = 20;
    protected GameManager gameManager;
    protected virtual void MoveTarget()
    {
        // To be implemented in child classes
    }

    protected virtual void DestroyTarget(Vector3 lastHitPoint)
    {
        Destroy(gameObject);
    }
}
