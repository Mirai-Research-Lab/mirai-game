using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform t_origin;
    [SerializeField] private Transform t_trailOrigin;
    [SerializeField] private float f_fireRate = 5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float maxDistance = 2000f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem hitEffectVfx;
    private Ray ray;
    private RaycastHit hit;
    private float f_shootInterval = 0f;
    public void fire(float input)
    {
        Weapon wep = GetComponentInChildren<Weapon>();

        if(input > Mathf.Epsilon && Time.timeSinceLevelLoad > f_shootInterval && wep != null)
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
            t_trailOrigin = GameObject.FindGameObjectWithTag("Muzzle").transform;
            TrailRenderer trail = Instantiate(trailRenderer, t_trailOrigin.position, Quaternion.identity);
            if(trail)
                StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while(time < 0.5f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        // Instantiate impact particle system
        Instantiate(hitEffectVfx, hit.point, Quaternion.LookRotation(hit.normal));
    }
}
