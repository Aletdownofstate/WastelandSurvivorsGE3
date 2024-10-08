using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance { get; private set; }

    public enum WorkerName { Alexander, Andreas, Anton, Benjamin, Carlos, Daniel, David, Dimitri, Erik, Filip, Francesco, Gabriel, Henrik, Ivan, Jacob, Leon, Lukas, Mattias, Nikolai, Oliver, Rafael, Sam, Stefan, Thomas, Viktor }
    public WorkerName firstName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public WorkerName GetWorkerName()
    {
        firstName = (WorkerName)Random.Range(0, System.Enum.GetValues(typeof(WorkerName)).Length);

        return firstName;
    }
}