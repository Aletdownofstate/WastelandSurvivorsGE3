using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance { get; private set; }

    public enum MaleWorkerName { Alexander, Andreas, Anton, Benjamin, Carlos, Daniel, David, Dimitri, Erik, Filip, Francesco, Gabriel, Henrik, Ivan, Jacob, Leon, Lukas, Mattias, Nikolai, Oliver, Rafael, Sam, Stefan, Thomas, Viktor }
    public enum FemaleWorkerName { Alice, Amelie, Anna, Bianca, Carla, Daria, Elena, Emilia, Eva, Francesca, Greta, Hanna, Ingrid, Isabella, Jana, Lara, Lucia, Maria, Nina, Olivia, Petra, Sofia, Tatiana, Viktoria, Zoe }

    public MaleWorkerName maleFirstName;
    public FemaleWorkerName femaleFirstName;

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

    public MaleWorkerName GetMaleWorkerName()
    {
        maleFirstName = (MaleWorkerName)Random.Range(0, System.Enum.GetValues(typeof(MaleWorkerName)).Length);

        return maleFirstName;
    }

    public FemaleWorkerName GetFemaleWorkerName()
    {
        femaleFirstName = (FemaleWorkerName)Random.Range(0, System.Enum.GetValues(typeof(FemaleWorkerName)).Length);

        return femaleFirstName;
    }
}