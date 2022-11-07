using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetBase : MonoBehaviour
{
    [SerializeField] protected int durability = 100;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected int points = 20;

    protected virtual void MoveTarget()
    {
        // To be implemented in child classes
    }

    protected virtual void DestroyTarget()
    {
        // To be implemented in child classes
    }

}
