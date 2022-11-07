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
    [SerializeField] private WeaponRecoil recoilScript;

    private AnimationManager animManager;
    private Ray ray;
    private RaycastHit hit;
    private float f_shootInterval = 0f;
    private bool isReloading = false;

    private void Start()
    {
        animManager = GetComponent<AnimationManager>();
        recoilScript = GetComponentInChildren<WeaponRecoil>();
    }
    private void Update()
    {
        Weapon wep = GetComponentInChildren<Weapon>();

        if (wep != null && wep.getCurrentAmmo() <= 0) 
        {
            animManager.reload();
            isReloading = true;
        }
    }
    public void fire(float input)
    {
        Weapon wep = GetComponentInChildren<Weapon>();
        if(input > Mathf.Epsilon && Time.timeSinceLevelLoad > f_shootInterval && wep != null && !isReloading)
        {
            if (wep.getCurrentAmmo() > 0)
            {
                wep.reduceAmmo();
                f_shootInterval = Time.timeSinceLevelLoad + (1 / f_fireRate);
                processShooting();
            }
        }
    }
    private void processShooting()
    {
        ray = new Ray(t_origin.position, t_origin.forward);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            recoilScript.RecoilFire();
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
        Destroy(Instantiate(hitEffectVfx, hit.point, Quaternion.LookRotation(hit.normal)).gameObject, 1f);
        Destroy(trail.gameObject);
    }
    public void reload()
    {
        Weapon wep = GetComponentInChildren<Weapon>();
        isReloading = false;
        wep.resetAmmo();
    }
    public bool getIsReloading()
    {
        return isReloading;
    }
}
