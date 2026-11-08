using System;
using System.Collections;
using UnityEngine;

public class ScriptMusicBeat : MonoBehaviour
{
    [SerializeField] AudioClip musicClip;
    [SerializeField] int BPM;
    [Range(0.01f, 100f)]
    [SerializeField] int acceptancePercentage;
    [Range (0f, 1f)]
    [SerializeField] float delay;


    public static event Action audioStarted;
    public static event Action<float> beatStarted;

    AudioSource musicPlayer;
    private void Awake()
    {
        musicPlayer = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicPlayer.clip = musicClip;
        musicPlayer.loop = true;
        musicPlayer.Play();
        StartCoroutine(invokeAudioStarted());

    }
    IEnumerator invokeAudioStarted()
    {
        yield return null;
        audioStarted?.Invoke();
    }

    // Update is called once per frame
    float saveTimer = 0;
    void Update()
    {
        float actualTempo = getTempo();
        if (saveTimer < actualTempo)
        {
            beatStarted?.Invoke(actualTempo);
        }
        saveTimer = actualTempo;
    }

    public bool onTempo()
    {
        if(musicPlayer.clip != null)
        {
            float BPMratio = 60f / BPM;
            float tempo = getTempo();
            Debug.Log(tempo);
            //Debug.Log("GREAT!!!");
            bool result = (tempo <= (BPMratio * acceptancePercentage / 200f) || tempo >= BPMratio - (BPMratio * acceptancePercentage / 200f));
            Debug.Log(result ? "<color=green>GREAT!!!</color>" : "<color=red>Fail</color>");
            return result;
        }
        
        return false;
    }

    
    

    public int getBPM()
    {
        return BPM;
    }
    public float getDelay()
    {
        return delay;
    }
    public float getTempo() //devuelve el tiempo que falta en segundos para que termine el beat actual
    {
        float BPMratio = 60f / BPM;
        float tempo = BPMratio-((musicPlayer.time - delay) % BPMratio);
        return tempo;
    }
}
