using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewWorker : MonoBehaviour
{
    [SerializeField] private GameObject maleWorkerA, maleWorkerB, femaleWorker;    
    [SerializeField] private GameObject spawnPointA, spawnPointB, spawnPointC, spawnPointD;

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
    }
}