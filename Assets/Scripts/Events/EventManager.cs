using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public enum EventType { None, Resource, Illness, Social }
    public EventType currentEventType;

    private bool canEvent = true;

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

    private void Start()
    {
        currentEventType = EventType.None;
    }

    private void Update()
    {
        if (canEvent)
        {
            RandomEvent();
        }
    }

    private void RandomEvent()
    {
        canEvent = false;

        int eventChance = Random.Range(0, 99);

        if (eventChance > 75)
        {
            currentEventType = (EventType)Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
        }

        switch (currentEventType)
        {
            case EventType.None:
                break;
            case EventType.Resource:
                break;
            case EventType.Illness:
                break;
            case EventType.Social:
                break;
        }

        StartCoroutine(EventDelay());
    }

    private IEnumerator EventDelay()
    {
        yield return new WaitForSeconds(120.0f);
        canEvent = true;
    }
}