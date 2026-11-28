using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScriptRythmUIHelper : MonoBehaviour
{
    [SerializeField] InputActionReference move;
    [SerializeField] float endScale;
    float startScale;

    [SerializeField] GameObject aro;
    Image imageCenter;
    Image imageAro;

    ScriptMusicBeat musicBeat;
    float timer = 0;
    float lastTempoReceived = 1;


    [SerializeField] Color colorDefault = Color.red;
    [SerializeField] Color colorAccept = Color.green;


    bool debug=false;
    private void Awake()
    {
        imageCenter = GetComponent<Image>();
        imageAro = aro.GetComponent<Image>();
        musicBeat = FindAnyObjectByType<ScriptMusicBeat>();
        


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
        imageCenter.color = musicBeat.onTempo() ? colorAccept : colorDefault;
        


    }
    private void OnEnable()
    {
        ScriptMusicBeat.beatStarted += updateTempo;
        ScriptMusicBeat.beatStartedWithoutDelay += beatSound;
        move.action.started += checkInput;
    }
    private void OnDisable()
    {
        ScriptMusicBeat.beatStarted -= updateTempo;
        ScriptMusicBeat.beatStartedWithoutDelay -= beatSound;
        move.action.started -= checkInput;
    }

    /*private void startSync()
    {
        lastTempoReceived = musicBeat.getTempo();
        timer = lastTempoReceived;
        
        
    }*/
    private void updateTempo()
    {
        
        lastTempoReceived = musicBeat.getTempo();
        timer = lastTempoReceived;
        debug = true;
        
        
    }
    private void beatSound()
    {
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

    private void checkInput(InputAction.CallbackContext context)
    {
        imageAro.color = (imageCenter.color == colorDefault) ? Color.pink: Color.blue ;
    }

}
