using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] private GameObject eventPopUp;
    [SerializeField] private TextMeshProUGUI eventExclamation, eventDescription, eventResult;

    public enum EventType { None, Resource, Wellbeing, Social }
    public EventType currentEventType;
    public enum ResourceEvent { Food, Water }
    public ResourceEvent currentResourceEvent;
    public enum WellbeingEvent { GenericIllness }
    public WellbeingEvent currentWellbeingEvent;
    public enum SocialEvent { Fight }
    public SocialEvent currentSocialEvent;

    private bool canEventTrigger = false;
    private bool isEventTriggered = false;

    private int randomAmount;

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
        eventPopUp.SetActive(false);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        currentEventType = EventType.None;        
        StartCoroutine(EventDelay());
    }

    private void Update()
    {
        if (canEventTrigger)
        {
            GetRandomEvent();
        }
    }

    private void GetRandomEvent()
    {
        canEventTrigger = false;

        int eventChance = Random.Range(0, 5);

        if (eventChance >= 3)
        {
            currentEventType = (EventType)Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
            isEventTriggered = true;

            if (currentEventType != EventType.None)
            {
                Debug.Log("Event triggered");
            }
            else 
            { 
                Debug.Log("No event triggered"); 
            }
        }

        if (isEventTriggered)
        {
            switch (currentEventType)
            {
                case EventType.None:
                    break;

                case EventType.Resource:
                    currentResourceEvent = (ResourceEvent)Random.Range(0, System.Enum.GetValues(typeof(ResourceEvent)).Length);                    
                    break;

                case EventType.Wellbeing:
                    
                    GameObject[] medicalTents = GameObject.FindGameObjectsWithTag("MedicalTent");

                    if (medicalTents.Length >= 1)
                    {
                        break;
                    }
                    else
                    {
                        currentWellbeingEvent = (WellbeingEvent)Random.Range(0, System.Enum.GetValues(typeof(WellbeingEvent)).Length);
                        break;
                    }

                case EventType.Social:
                    currentSocialEvent = (SocialEvent)Random.Range(0, System.Enum.GetValues(typeof(SocialEvent)).Length);
                    break;
            }            
        }

        if (currentEventType == EventType.Resource && isEventTriggered)
        {
            randomAmount = Random.Range(0, 151);

            switch (currentResourceEvent)
            {
                case ResourceEvent.Food:
                    InitialiseEventText($"EVENT!", $"Rats have gotten into our supplies!", $"{randomAmount} of food lost");
                    ShowEventUI();
                    break;
                case ResourceEvent.Water:
                    InitialiseEventText($"EVENT!", $"Our water has been contaminated!", $"{ randomAmount} of water lost");
                    ShowEventUI();
                    break;
            }
        }

        if (currentEventType == EventType.Wellbeing && isEventTriggered)
        {
            switch (currentWellbeingEvent)
            {
                case WellbeingEvent.GenericIllness:
                    InitialiseEventText($"EVENT!", $"Some people have fallen ill!", $"Morale has slightly decreased");
                    ShowEventUI();
                    break;
            }
        }

        if (currentEventType == EventType.Social && isEventTriggered)
        {
            switch (currentSocialEvent)
            {
                case SocialEvent.Fight:
                    InitialiseEventText($"EVENT!", $"A fight has broken out!", $"Morale has slightly decreased");
                    ShowEventUI();
                    break;
            }
        }

        StartCoroutine(EventDelay());
    }

    public void InitialiseEventText(string exclamation, string description, string result)
    {
        eventExclamation.text = exclamation;
        eventDescription.text = description;
        eventResult.text = result;
    }

    public void ShowEventUI()
    {
        Time.timeScale = 0;
        eventPopUp.SetActive(true);
    }

    public void DismissEventUI()
    {
        Time.timeScale = 1;
        eventPopUp.SetActive(false);
        EventEffect();
        currentEventType = EventType.None;
    }

    private IEnumerator EventDelay()
    {
        float delay = Random.Range(60.0f, 90.0f);
        yield return new WaitForSeconds(delay);
        canEventTrigger = true;
    }

    private void EventEffect()
    {
        if (currentEventType == EventType.Resource)
        {
            switch (currentResourceEvent)
            {
                case ResourceEvent.Food:
                    ResourceManager.Instance.SubtractResource("Food", randomAmount);
                    break;
                case ResourceEvent.Water:
                    ResourceManager.Instance.SubtractResource("Water", randomAmount);
                    break;
            }
        }

        if (currentEventType == EventType.Wellbeing)
        {
            switch (currentWellbeingEvent)
            {
                case WellbeingEvent.GenericIllness:
                    MoraleManager.Instance.DecreaseMorale();
                    break;
            }
        }

        if (currentEventType == EventType.Social)
        {
            switch (currentSocialEvent)
            {
                case SocialEvent.Fight:
                    MoraleManager.Instance.DecreaseMorale();
                    break;
            }
        }
    }
}