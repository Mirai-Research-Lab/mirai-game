using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform t_origin;
    [SerializeField] private float f_fireRate = 5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float maxDistance = 2000f;
    private Ray ray;
    private RaycastHit hit;
    private float f_shootInterval = 0f;
    public void fire(float input)
    {
        Debug.Log(input);
        if(input > Mathf.Epsilon && Time.timeSinceLevelLoad > f_shootInterval)
        {
            f_shootInterval = Time.timeSinceLevelLoad + (1 / f_fireRate);
            processShooting();
        }
    }
    private void processShooting()
    {
        ray = new Ray(t_origin.position, t_origin.forward);

        if (Physics.Raycast(ray, out hit, maxDistance))
        { 

        }
    }
}
