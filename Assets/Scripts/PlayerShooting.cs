using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform t_origin;
    [SerializeField] private Transform t_trailOrigin;
    [SerializeField] private float f_fireRate = 5f;
    [SerializeField] private float penaltyForMiss = 5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float maxDistance = 2000f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem hitEffectVfx;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioClip akAudioClip;
    [SerializeField] private AudioClip bullAudioClip;

    private WeaponRecoil recoilScript;
    private AnimationManager animManager;
    private Ray ray;
    private RaycastHit hit;
    private float f_shootInterval = 0f;
    private bool isReloading = false;
    private int shotsFired = 0;
    private int shotsOnTarget = 0;
    private AudioSource gunAudio;
    private Weapon wep;
    private void Start()
    {
        animManager = GetComponent<AnimationManager>();
        gunAudio = GetComponents<AudioSource>()[0];
        recoilScript = GetComponentInChildren<WeaponRecoil>();
    }
    private void Update()
    {
        wep = GetComponentInChildren<Weapon>();
        if (wep != null && wep.getCurrentAmmo() <= 0)
        {
            animManager.reload();
            isReloading = true;
        }
    }
    public void fire(float input)
    {
        if (wep != null)
            f_fireRate = wep.getFireRate();
        if (input > Mathf.Epsilon && Time.timeSinceLevelLoad > f_shootInterval && wep != null && !isReloading)
        {
            if (wep.getCurrentAmmo() > 0)
            {
                wep.reduceAmmo();
                f_shootInterval = Time.timeSinceLevelLoad + (1 / f_fireRate);
                processShooting();
                gunAudio.PlayOneShot(wep.getAudioClip());
            }
        }
    }
    private void processShooting()
    {
        ray = new Ray(t_origin.position, t_origin.forward);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            //functionality
            if (gameManager.GetIsGameStarted())
                shotsFired++;

            recoilScript.RecoilFire();
            if (hit.transform.GetComponent<StartGame>())
            {
                // Play some kind of animation
                // Play some kind of sound
                gameManager.StartGame(hit);
            }
            NormalTarget target = hit.transform.GetComponent<NormalTarget>();
            if (target != null)
            {
                target.reduceDurability(wep.getDamageAmount(), hit.point);
                shotsOnTarget++;
            }
            else
            {
                gameManager.updateTotalPoints(-penaltyForMiss);
            }
            t_trailOrigin = GameObject.FindGameObjectWithTag("Muzzle").transform;
            TrailRenderer trail = Instantiate(trailRenderer, t_trailOrigin.position, Quaternion.identity);
            if (trail != null)
                StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 0.5f)
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
    public int getShotsOnTarget()
    {
        return shotsOnTarget;
    }
    public int getShotsFired()
    {
        return shotsFired;
    }
}
