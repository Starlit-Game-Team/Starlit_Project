using UnityEngine;
using UnityEngine.Events;

public class Rhythm_Manager : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Intervals[] intervals;
    
    private void Update()
    {
        foreach (Intervals interval in intervals)
        {
            float sampledTime = audioSource.timeSamples / (audioSource.clip.frequency * interval.GetIntervalLength(bpm));
            interval.CheckNewInterval(sampledTime);
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float beatNumber;
    [SerializeField] private UnityEvent onBeat;
    private int lastInterval;
    // private float threshold = 0.1f;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * beatNumber);
    }
    public void CheckNewInterval(float interval)
    {
        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            onBeat.Invoke();
        }
        // float currentInterval = Mathf.Floor(audioSource.time / GetIntervalLength());
        // if (currentInterval > lastInterval)
        // {
        //     lastInterval = (int)currentInterval;
        //     onBeat.Invoke();
        // }
        // return currentInterval;
    }
}
