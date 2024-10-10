using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalityManager : MonoBehaviour
{
    public static PersonalityManager Instance { get; private set; }

    public enum PersonalityType { HardWorking, Lazy, Optimist, Pessmist, Strong, Weak }
    public PersonalityType personality;

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

    public PersonalityType ChoosePersonality()
    {
        personality = (PersonalityType)Random.Range(0, System.Enum.GetValues(typeof(PersonalityType)).Length);
        return personality;
    }
}