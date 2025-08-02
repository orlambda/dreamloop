using UnityEngine;
using System;
using Actions;
using System.Collections.Generic;
using UnityEngine.Events;


public class OrchestratedEvents : MonoBehaviour
{
    [System.Serializable]
    public class TimedEvent
    {
        public string eventName;
        public float timestamp;
        public UnityEvent eventToCall;
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

    public static OrchestratedEvents MusicLordInstance;

    private void Awake()
    {
        if (MusicLordInstance != null && MusicLordInstance != this)
        {
            Destroy(this);
        }
        else
        {
            MusicLordInstance = this;
        }
    }

    void Start()
    {
        actionsInstance = new Actions.OrchestratedActions();


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
                //cueSheet[eventName](new object[] { });
                eventList[nextTriggerIndex].eventToCall.Invoke();
                indexedEvents[nextTriggerIndex] = (eventName, timestamp, true);
                nextTriggerIndex++;
                Debug.Log("" + nextTriggerIndex);
            }
        }
    }

    public float GetCurrentMusicTime()
    {
        return instrumentTracks[0].time;
    }

    public void IncreaseIndex()
    {
        currentClipIndex = (currentClipIndex + 1) % instrumentTracks.Length;
    }

    public void StartMusic()
    {
        PlayClip(0);
    }
}