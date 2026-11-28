using UnityEngine;
using UnityEngine.InputSystem;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScriptMusicBeat : MonoBehaviour
{
    [SerializeField] InputActionReference debugInput;
    [SerializeField] AudioClip musicClip;
    [SerializeField] int BPM;
    [Range(0.01f, 100f)]
    [SerializeField] int acceptancePercentage;

    [Header("delay adjustments")]
    [SerializeField] bool useDelayInSeconds = true;
    [SerializeField] float delayInSeconds = 0;
    [Range(0f, 100f)]
    [SerializeField] float delay = 0;
    float delaySeconds = 0;


    public static event Action audioStarted;
    public static event Action beatStarted;
    public static event Action halfBeat;

    public static event Action beatStartedWithoutDelay;

    AudioSource musicPlayer;
    private void Awake()
    {
        musicPlayer = GetComponent<AudioSource>();
        debugInput.action.started += debugTempo;
    }
    List<float> listOfData = new List<float>();
    private void debugTempo(InputAction.CallbackContext context)
    {
        float BPMratio = 60f / BPM;
        float tempo = getTempo();
        bool result = (tempo <= (BPMratio * acceptancePercentage / 200f) || tempo >= BPMratio - (BPMratio * acceptancePercentage / 200f));
        

        Debug.Log(result ? "<color=green>GREAT!!!</color>" : "<color=red>Fail</color>");
        float debugDelay = (tempo / (60f / BPM)) * 100;
        /*if (debugDelay > 50f)
        {
            debugDelay -= 100;
        }*/
        //listOfData.Add(Math.Abs(debugDelay));
        listOfData.Add(debugDelay);

        Debug.Log($"<color=yellow>  your mark was {debugDelay}% off \n   average delay {listOfData.Average()}</color>");
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
    float saveTimerWithoutDelay = 0;
    void Update()
    {

        delaySeconds = useDelayInSeconds ? delayInSeconds : (60f / BPM) * (delay / 100);
        float actualTempo = getTempo();
        float actualTempoWithoutDelay = getTempoWithoutDelay();

        if (saveTimer < actualTempo)
        {
            beatStarted?.Invoke();
        }
        else if (saveTimer >= 60f / BPM / 2 && actualTempo <= 60f / BPM / 2)
        {
            halfBeat?.Invoke();
        }

        if (saveTimerWithoutDelay < actualTempoWithoutDelay) //esto es util para cosas como el metro que ayuda al jugador
        {
            beatStartedWithoutDelay?.Invoke();
        }
        saveTimer = actualTempo;
        saveTimerWithoutDelay = actualTempoWithoutDelay;
    }

    public bool onTempo()
    {
        if (musicPlayer.clip != null)
        {
            float BPMratio = 60f / BPM;
            float tempo = getTempo();
            bool result = (tempo <= (BPMratio * acceptancePercentage / 200f) || tempo >= BPMratio - (BPMratio * acceptancePercentage / 200f));
            
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
        return delaySeconds;
    }
    public float getTempo() //devuelve el tiempo que falta en segundos para que termine el beat actual
    {
        float BPMratio = 60f / BPM;
        float tempo = BPMratio - ((musicPlayer.time - delaySeconds) % BPMratio);
        return tempo;
    }
    public float getTempoWithoutDelay() //devuelve el tiempo que falta en segundos para que termine el beat actual
    {
        float BPMratio = 60f / BPM;
        float tempo = BPMratio - ((musicPlayer.time) % BPMratio);
        return tempo;
    }
    public float getAcceptance()
    {
        return (60f / BPM) * (acceptancePercentage / 100);
    }

}