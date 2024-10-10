using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    public static SkillsManager Instance { get; private set; }

    public enum Skill { None, Woodworker, Metalworker, Scavenger, NaturalLeader }
    public Skill skill;

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

    public Skill GetSkill()
    {
        skill = (Skill)Random.Range(0, System.Enum.GetValues(typeof(Skill)).Length);

        return skill;
    }
}
