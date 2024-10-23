using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WorkerTaskManager : MonoBehaviour
{
    private WorkerNavmesh workerNavmesh;
    private NavMeshAgent navMeshAgent;
    private UnitSelectionManager unitSelectionManager;
    private Transform resourceTarget;
    private Transform dropOffPoint;

    [SerializeField] private AudioSource woodSound, metalSound;
    [SerializeField] private Animator anim;
    [SerializeField] private Renderer axeObject;
    [SerializeField] private GameObject bottleObject;
    [SerializeField] private GameObject bucketObject;

    public enum WorkerState { Idle, MovingToResource, Gathering, ReturningToDropoff }
    public WorkerState currentWorkerState;

    public PersonalityManager.PersonalityType personalityType;
    private float gatheringBonus;

    public NameManager.WorkerName workerName;
    public SkillsManager.Skill workerSkill;
    private int woodSkillBonus = 0;
    private int metalSkillBonus = 0;
    private int foodSkillBonus = 0;

    public float gatherDuration = 10.0f;
    public int maxCarryAmount = 100;
    private int currentResources = 0;
    private string resourceType;    

    [HideInInspector] public bool isInterrupted = false;

    private void Awake()
    {
        workerNavmesh = GetComponent<WorkerNavmesh>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        unitSelectionManager = GameObject.FindGameObjectWithTag("UnitSelectionManager").GetComponent<UnitSelectionManager>();
        currentWorkerState = WorkerState.Idle;
    }

    private void Start()
    {
        workerName = NameManager.Instance.GetWorkerName();
        workerSkill = SkillsManager.Instance.GetSkill();
        personalityType = PersonalityManager.Instance.ChoosePersonality();

        switch (personalityType)
        {
            case PersonalityManager.PersonalityType.HardWorking:
                gatheringBonus = -2.0f;
                break;
            case PersonalityManager.PersonalityType.Lazy:
                gatheringBonus = 2.0f;
                break;
            case PersonalityManager.PersonalityType.Optimist:
                MoraleManager.Instance.IncreaseMorale();
                break;
            case PersonalityManager.PersonalityType.Pessmist:
                MoraleManager.Instance.DecreaseMorale();
                break;
            case PersonalityManager.PersonalityType.Strong:
                maxCarryAmount += 25;
                break;
            case PersonalityManager.PersonalityType.Weak:
                maxCarryAmount -= 25;
                break;
        }

        switch (workerSkill)
        {
            case SkillsManager.Skill.None:                
                break;
            case SkillsManager.Skill.Woodworker:
                woodSkillBonus = 25;
                metalSkillBonus = 0;
                foodSkillBonus = 0;
                break;
            case SkillsManager.Skill.Metalworker:
                woodSkillBonus = 0;
                metalSkillBonus = 25;
                foodSkillBonus = 0;
                break;
            case SkillsManager.Skill.Scavenger:
                woodSkillBonus = 0;
                metalSkillBonus = 0;
                foodSkillBonus = 25;
                break;
            case SkillsManager.Skill.NaturalLeader:
                MoraleManager.Instance.IncreaseMorale();
                break;
        }
    }

    private void Update()
    {
        if (isInterrupted)
        {
            return;
        }

        switch (MoraleManager.Instance.currentState)
        {
            case MoraleManager.MoraleState.High:
                gatherDuration = 8.0f + gatheringBonus;
                break;
            case MoraleManager.MoraleState.Neutral:
                gatherDuration = 10.0f + gatheringBonus;
                break;
            case MoraleManager.MoraleState.Low:
                gatherDuration = 12.0f + gatheringBonus;
                break;
        }        

        switch (currentWorkerState)
        {
            case WorkerState.MovingToResource:

                if (resourceTarget == null)
                {
                    FindNextAvailableResource();
                    if (resourceTarget == null)
                    {
                        currentWorkerState = WorkerState.Idle;
                        return;
                    }
                }

                Vector3 closestPoint = GetClosestPointOnResource(resourceTarget);
                workerNavmesh.MoveToDestination(closestPoint);

                if (workerNavmesh.HasReachedDestination())
                {
                    Destroy(unitSelectionManager.currentGroundMarker);
                    StartCoroutine(GatherResources());
                }
                break;

            case WorkerState.ReturningToDropoff:
                workerNavmesh.MoveToDestination(dropOffPoint.position);
                if (workerNavmesh.HasReachedDestination())
                {
                    DepositResources();
                }
                break;

            case WorkerState.Idle:
                break;
        }

        // Animations

        if (navMeshAgent.velocity != Vector3.zero)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (currentWorkerState != WorkerState.Gathering)
        {
            anim.SetBool("isPicking", false);
            anim.SetBool("isCollecting", false);
            anim.SetBool("isChopping", false);
        }
    }

    public void StartGathering(Transform resourceTransform, Transform dropOff, string resource)
    {
        GameObject[] dropOffPoints = GameObject.FindGameObjectsWithTag("DropOffPoint");
        GameObject closestDropOff = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject dropPoint in dropOffPoints)
        {
            float distance = Vector3.Distance(resourceTransform.transform.position, dropPoint.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestDropOff = dropPoint;
            }
        }

        resourceTarget = resourceTransform;
        dropOffPoint = closestDropOff.transform;
        resourceType = resource;
        currentWorkerState = WorkerState.MovingToResource;
    }

    private IEnumerator GatherResources()
    {
        currentWorkerState = WorkerState.Gathering;

        AudioSource gatheringSound = null;        

        switch (resourceTarget.tag)
        {
            case "Wood":
                gatheringSound = woodSound;
                anim.SetBool("isChopping", true);
                axeObject.enabled = true;
                bucketObject.SetActive(false);
                bottleObject.SetActive(false);
                break;

            case "Metal":
                gatheringSound = metalSound;
                gatheringSound.volume = 0.4f;
                anim.SetBool("isCollecting", true);
                axeObject.enabled = false;
                bucketObject.SetActive(false);
                bottleObject.SetActive(false);
                break;

            case "Food":
                anim.SetBool("isPicking", true);
                axeObject.enabled = false;
                bucketObject.SetActive(true);
                bottleObject.SetActive(false);
                break;

            case "Water":
                anim.SetBool("isPicking", true);
                axeObject.enabled = false;
                bucketObject.SetActive(false);
                bottleObject.SetActive(true);
                break;
        }        

        if (gatheringSound != null)
        {
            StartCoroutine(GatheringSoundDelay(gatheringSound));
        }

        yield return new WaitForSeconds(gatherDuration);        

        if (gatheringSound != null)
        {
            gatheringSound.Stop();
        }

        axeObject.enabled = false;
        bottleObject.SetActive(false);
        bucketObject.SetActive(false);

        switch (resourceTarget.tag)
        {
            case "Wood":
                WoodResource woodResource = resourceTarget.parent.gameObject.GetComponent<WoodResource>();

                if (woodResource.currentResourceState == WoodResource.ResourceState.Depleted)
                {
                    resourceTarget = null;
                    FindNextAvailableResource();
                }

                currentResources = maxCarryAmount + (foodSkillBonus + metalSkillBonus + woodSkillBonus);
                woodResource.DepleteResource(currentResources);

                currentWorkerState = WorkerState.ReturningToDropoff;
                break;

            case "Metal":
                MetalResource metalResource = resourceTarget.GetComponent<MetalResource>();

                if (metalResource.currentResourceState == MetalResource.ResourceState.Depleted)
                {
                    resourceTarget = null;
                    FindNextAvailableResource();
                }

                currentResources = maxCarryAmount + (foodSkillBonus + metalSkillBonus + woodSkillBonus);
                metalResource.DepleteResource(currentResources);

                currentWorkerState = WorkerState.ReturningToDropoff;

                break;

            case "Food":
                FoodResource foodResource = resourceTarget.GetComponent<FoodResource>();

                if (foodResource.currentResourceState == FoodResource.ResourceState.Depleted)
                {
                    resourceTarget = null;
                    FindNextAvailableResource();
                }

                currentResources = maxCarryAmount + (foodSkillBonus + metalSkillBonus + woodSkillBonus);
                foodResource.DepleteResource(currentResources);

                currentWorkerState = WorkerState.ReturningToDropoff;
                break;

            case "Water":
                currentResources = maxCarryAmount + (foodSkillBonus + metalSkillBonus + woodSkillBonus);
                currentWorkerState = WorkerState.ReturningToDropoff;
                break;
        }
    }

    private void FindNextAvailableResource()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag(resourceType);
        if (resources.Length == 0)
        {
            currentWorkerState = WorkerState.Idle;
            return;
        }

        float closestDistance = Mathf.Infinity;
        GameObject closestResource = null;

        foreach (GameObject resource in resources)
        {
            if (resource != null)
            {
                float distance = Vector3.Distance(transform.position, resource.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestResource = resource;
                }
            }
        }

        if (closestResource != null)
        {
            resourceTarget = closestResource.transform;
            currentWorkerState = WorkerState.MovingToResource;
        }
        else
        {
            currentWorkerState = WorkerState.Idle;
        }
    }

    private void DepositResources()
    {
        ResourceManager.Instance.AddResource(resourceType, currentResources);
        currentResources = 0;
        currentWorkerState = WorkerState.MovingToResource;
    }

    public void InterruptCurrentTask()
    {
        isInterrupted = true;
        StopAllCoroutines();
        currentWorkerState = WorkerState.Idle;
        isInterrupted = false;
    }

    private IEnumerator GatheringSoundDelay(AudioSource gatheringSound)
    {
        float delay = Random.Range(0.75f, 1.5f);
        float pitch = Random.Range(0.95f, 1.05f);

        gatheringSound.pitch = pitch;
        gatheringSound.Play();

        yield return new WaitForSeconds(delay);

        if (currentWorkerState == WorkerState.Gathering)
        {
            StartCoroutine(GatheringSoundDelay(gatheringSound));
        }
    }

    private Vector3 GetClosestPointOnResource(Transform targetTransform)
    {
        Collider targetCollider = targetTransform.GetComponent<Collider>();

        if (targetCollider != null)
        {
            return targetCollider.ClosestPoint(transform.position);
        }
        else
        {
            return targetTransform.position;
        }
    }
}