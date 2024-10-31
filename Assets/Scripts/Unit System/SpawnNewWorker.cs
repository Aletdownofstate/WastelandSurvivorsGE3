using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewWorker : MonoBehaviour
{
    [SerializeField] private GameObject maleWorkerA, maleWorkerB, femaleWorker;    
    [SerializeField] private GameObject spawnPointA, spawnPointB, spawnPointC, spawnPointD;

    [HideInInspector] public bool canSpawn = false;

    public void SpawnWorker()
    {
        GameObject workerPrefab = null;

        int i = Random.Range(0, 3);

        switch (i)
        {
            case (0):
                workerPrefab = maleWorkerA;
                break;
            case (1):
                workerPrefab = maleWorkerB;
                break;
            case (2):
                workerPrefab = femaleWorker;
                break;
        }

        if (PopulationManager.Instance.population <= PopulationManager.Instance.populationCurrentCap && canSpawn)
        {
            GameObject worker = Instantiate(workerPrefab, spawnPointD.transform.position, Quaternion.identity);
            WorkerNavmesh workerNavmesh = worker.GetComponent<WorkerNavmesh>();

            GameObject homePoint = null;
            int spawn = Random.Range(0, 3);

            switch (spawn)
            {
                case (0):
                    homePoint = spawnPointA;
                    break;
                case (1):
                    homePoint = spawnPointB;
                    break;
                case (2):
                    homePoint = spawnPointC;
                    break;
            }
            workerNavmesh.MoveToDestination(homePoint.transform.position);

            Debug.Log($"Spawning new worker and moving to spawnpoint {spawn}");

            StartCoroutine(SpawnDelay());
        }
        else
        {
            Debug.Log("Cannot spawn - Population cap reached or delay isn't completed");
            return;
        }

    }

    private IEnumerator SpawnDelay()
    {
        Debug.Log("Starting spawn delay - 60s remaining");
        canSpawn = false;
        yield return new WaitForSeconds(60.0f);
        canSpawn = true;
        Debug.Log("Spawn delay complete");
    }
}