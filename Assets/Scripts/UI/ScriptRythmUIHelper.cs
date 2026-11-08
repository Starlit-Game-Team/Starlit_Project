using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class ScriptRythmUIHelper : MonoBehaviour
{
    [SerializeField] float endScale;
    float startScale;

    [SerializeField] GameObject aro;

    ScriptMusicBeat musicBeat;
    float timer = 0;
    float lastTempoReceived = 1;


    bool debug=false;
    private void Awake()
    {
        musicBeat = FindAnyObjectByType<ScriptMusicBeat>();
        //ScriptMusicBeat.audioStarted += startSync;
        ScriptMusicBeat.beatStarted += updateTempo;


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = aro.transform.localScale.x;
        StartCoroutine(CoroutineAro());
    }
    int i = 0;
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
    }

    /*private void startSync()
    {
        lastTempoReceived = musicBeat.getTempo();
        timer = lastTempoReceived;
        
        
    }*/
    private void updateTempo(float tempo)
    {
        lastTempoReceived = musicBeat.getTempo();
        timer = lastTempoReceived;
        debug = true;
        musicBeat.onTempo();
        GetComponent<AudioSource>().Play();
        
    }
    IEnumerator CoroutineAro()
    {
        Vector3 startVector = new Vector3(startScale, startScale, 1);
        Vector3 endVector = new Vector3(endScale, endScale, 1);
        while (true)
        {
            float progress = Mathf.Clamp01(timer / lastTempoReceived);
            aro.transform.localScale = Vector3.Lerp(endVector, startVector, progress);
            if(progress == 0 && debug)
            {
                debug=false;
                //musicBeat.onTempo();
                
            }
            yield return null;
        }
        
    }

}
