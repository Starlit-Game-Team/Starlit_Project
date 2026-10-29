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
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool onTempo()
    {
        if(musicPlayer.clip != null)
        {
            float BPMratio = 60f / BPM;
            float tempo = ((musicPlayer.time - delay) % BPMratio);
            Debug.Log(tempo);
            //Debug.Log("GREAT!!!");
            bool result = (tempo <= (BPMratio * acceptancePercentage / 200f) || tempo >= BPMratio - (BPMratio * acceptancePercentage / 200f));
            Debug.Log(result ? "GREAT!!!" : "Fail");
            return result;
        }
        
        return false;
    }

    
    

    public int getBPM()
    {
        return BPM;
    }
}
