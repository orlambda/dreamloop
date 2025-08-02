using UnityEngine;
using System;
using Actions;
using System.Collections.Generic;


public class OrchestratedEvents : MonoBehaviour
{
    [System.Serializable]
    public class TimedEvent
    {
        public string eventName;
        public float timestamp;
    }

    public List<TimedEvent> eventList = new List<TimedEvent>();
    public AudioSource audioSource;
    public AudioSource[] instrumentTracks;

    public AudioClip[] audioLoops;


    public GameObject targetObject;

    // Define your timestamps (in seconds)
    public float[] triggerTimes;

    public float clipDuration = 30f;

    public Actions.OrchestratedActions actionsInstance;

    private int nextTriggerIndex = 0;
    private int currentClipIndex = 0;
    private bool[] triggered;

    private Dictionary<string, Action<object[]>> cueSheet;

    private (string, float, bool)[] indexedEvents;

    private float timer = 0.0f;

    void Start()
    {
        actionsInstance = new Actions.OrchestratedActions();

        cueSheet = new Dictionary<string, Action<object[]>>
        {
            { "SpawnEnemy", actionsInstance.SpawnEnemies },
            { "MovePlatforms", actionsInstance.MovePlatforms },
            { "BeamLasers", actionsInstance.MovePlatforms }
        };

        if (eventList == null || eventList.Count == 0)
        {
            Debug.LogError("eventList is null or empty!");
            return;
        }
        eventList.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));

        indexedEvents = new (string, float, bool)[eventList.Count];
        for (int i = 0; i < eventList.Count; i++)
        {
            // TODO: Confirm there's an eventName that maps to cueSheet
            indexedEvents[i] = (eventList[i].eventName, eventList[i].timestamp, false);
        }
        PlayClip(0);

    }

    void PlayAllClipsUpToIndex(int finalIndex)
    {
        for (int i = 0; i <= finalIndex; i++)
        {
            PlayClip(i);
        }
    }

    void PlayClip(int index)
    {
        instrumentTracks[index].Play();
        // audioSource.clip = audioLoops[index];
        // audioSource.Play();
        Debug.Log($"Now playing: {instrumentTracks[index].name}");
    }

    void Update()
    {
        if (!instrumentTracks[0].isPlaying)
            return;

        // Get current time of the base loop
        timer = instrumentTracks[0].time;
        if (timer >= clipDuration)
        {

            currentClipIndex = (currentClipIndex + 1) % instrumentTracks.Length;
            PlayAllClipsUpToIndex(currentClipIndex);
            for (int i = 0; i < indexedEvents.Length; i++)
            {
                var (en, ts, _) = indexedEvents[i];
                indexedEvents[i] = (en, ts, false);
            }
            // Add event to reset world
            nextTriggerIndex = 0;
            timer = 0;
        }

        if (nextTriggerIndex < indexedEvents.Length)
        {
            // If we reached the next trigger
            var (eventName, timestamp, alreadyTriggered) = indexedEvents[nextTriggerIndex];
            if (timer >= timestamp && !alreadyTriggered)
            {
                cueSheet[eventName](new object[] { });
                indexedEvents[nextTriggerIndex] = (eventName, timestamp, true);
                nextTriggerIndex++;
                Debug.Log("" + nextTriggerIndex);
            }
        }


    }

    void DoSomething(int index)
    {
        // Customize this per timestamp index
        Debug.Log($"Trigger at {triggerTimes[index]} seconds!");

        switch (index)
        {
            case 0:
                targetObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 1:
                targetObject.transform.localScale *= 1.5f;
                break;
            case 2:
                targetObject.transform.position += Vector3.up * 2f;
                break;
            default:
                targetObject.transform.position += Vector3.up * 2f;
                break;
        }
    }
}
