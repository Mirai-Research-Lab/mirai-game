using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGeneration : MonoBehaviour
{

    [SerializeField] private List<GameObject> targets;
    [SerializeField] private Transform targetParent;
    [SerializeField] private List<Transform> boundariesBox1;
    [SerializeField] [Range(0f, 100f)] private float badTargetBar = 15f;
    [SerializeField] [Range(0f, 100f)] private float goodTargetBar = 75f;
    [SerializeField] [Range(0f, 100f)] private float specialTargetBar = 10f;
    [SerializeField] private int allowedContinuosBadTarget = 2;
    [SerializeField] private int allowedContinuosSpecialTarget = 1;
    [SerializeField] private int allowedSimultaneousTarget = 1;
    [SerializeField] private float overLappingCheckSpherRadius = 3f;

    [SerializeField] private bool isOver = false; // Serialized For Debuggin purposes
    private int badTargetContinuousCounter = 0;
    private int specialTargetContinuousCounter = 0;
    private int existingSimultaneousTarget = 0;
    private GameManager gameManager;
    private void Start()
    {
        // StartCoroutine(GenerateTargets());
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        if(gameManager.GetIsGameStarted() && !isOver)
            SpawnTargets();
    }

    private void SpawnTargets()
    {
        if (targetParent != null)
            existingSimultaneousTarget = targetParent.childCount;
        if (existingSimultaneousTarget < allowedSimultaneousTarget)
        {
            int randomIndex = GetGeneratedIndex();
            float spawnX = Random.Range(boundariesBox1[0].position.x, boundariesBox1[1].position.x);
            float spawnY = Random.Range(boundariesBox1[2].position.y, boundariesBox1[3].position.y);
            float spawnZ = Random.Range(boundariesBox1[4].position.z, boundariesBox1[5].position.z);
            Vector3 spawnPoint = new Vector3(spawnX, spawnY, spawnZ);
            Collider[] colliders = Physics.OverlapSphere(spawnPoint, overLappingCheckSpherRadius);
            if (colliders.Length != 0)
                return;
            GameObject instanciatedTarget = Instantiate(targets[randomIndex], spawnPoint, targets[randomIndex].transform.rotation);
            if (targetParent != null)
                instanciatedTarget.transform.parent = targetParent;
        }
    }

    private int GetGeneratedIndex()
    {
        float randomProbablity = Random.Range(0f, 100f);
        if (randomProbablity >= 100f - specialTargetBar && specialTargetContinuousCounter < allowedContinuosSpecialTarget)
        {
            specialTargetContinuousCounter++;
            badTargetContinuousCounter = 0;
            return targets.Count - 1;
        }
        else if (randomProbablity >= 100f - goodTargetBar)
        {
            specialTargetContinuousCounter = 0;
            badTargetContinuousCounter = 0;
            int randIndx = Mathf.FloorToInt(Random.Range(0, targets.Count - 2));
            return randIndx;
        }
        else if (badTargetContinuousCounter <= allowedContinuosBadTarget && randomProbablity <= badTargetBar)
        {
            specialTargetContinuousCounter = 0;
            badTargetContinuousCounter++;
            return targets.Count-2;
        }
        else
        {
            specialTargetContinuousCounter = 0;
            badTargetContinuousCounter = 0;
            int randIndx = Mathf.FloorToInt(Random.Range(0, targets.Count - 2));
            return randIndx;
        }
    }

    // Modify Functions
    public void setAllowedSimultaneousTarget(int amount)
    {
        allowedSimultaneousTarget = amount;
    }

    public void setIsOver(bool over)
    {
        isOver = over;
    }

    public bool getIsOver()
    {
        return isOver;
    }
}
