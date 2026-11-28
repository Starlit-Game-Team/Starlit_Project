using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptCastSpells : MonoBehaviour
{
    [SerializeField] InputActionReference castInput;
    [SerializeField] Direction[] shieldCastDirections;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject[] prefabOrbsUDLR;
    List<GameObject> orbsAlive = new List<GameObject>(); 
    ScriptMusicBeat musicController;
    List<Direction> cast = new List<Direction>();


    float rotation = 0f;
    private void Awake()
    {
        
        musicController = FindAnyObjectByType<ScriptMusicBeat>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation += Time.deltaTime* 360f;
        rotation %= 360f;
        //Debug.Log(rotation);
        int numberOfOrbs = orbsAlive.Count;
        for(int i = 0; i< numberOfOrbs; i++)
        {
            orbsAlive[i].transform.rotation  = Quaternion.Euler(0, rotation +(360f* ((float)i/numberOfOrbs)), 0);
        }
    }
    private void OnEnable()
    {
        castInput.action.started += castOrb;
    }
    private void OnDisable()
    {
        castInput.action.started += castOrb;
    }

    private void castOrb(InputAction.CallbackContext context)
    {
        if (musicController.onTempo())
        {
            addOrb(Utilities.formVector2ToDirection(context.ReadValue<Vector2>()));
        }
        else
        {
            ResetOrbs();
        }

    }

    private void addOrb(Direction OrbDir)
    {
        cast.Add(OrbDir);
        orbsAlive.Add(Instantiate(prefabOrbsUDLR[Utilities.fromDirectionToInt(OrbDir)], transform));
        if(!matchCast(shieldCastDirections, cast, castShield))
        {
            ResetOrbs();
        }
        


    }

    private bool matchCast(Direction[] finalCast, List<Direction>directionCast, Action castFunction)
    {
        
        for (int i = 0; i < directionCast.Count; i++)
        {
            if (finalCast[i] != directionCast[i])
            {
                return false;
            }
        }
        if(finalCast.Length == directionCast.Count)
        {
            castFunction?.Invoke();
        }
        return true;
    }

    private void ResetOrbs()
    {
        foreach(GameObject orb in orbsAlive)
        {
            Destroy(orb);
        }
        orbsAlive.Clear();
        cast.Clear();
    }

    Coroutine saveShieldCoroutine = null;

    private void castShield()
    {
        if (saveShieldCoroutine != null)
        {
            StopCoroutine(saveShieldCoroutine);
        }
        saveShieldCoroutine = StartCoroutine(shiledCoroutine());
    }
    IEnumerator shiledCoroutine()
    {
        ResetOrbs(); //esto hay que cambiarlo por la animacion que vaya a hacer el escudo para consumir los orbes
        shield.SetActive(true);
        int numberOfBeatsDuration = 12;
        yield return new WaitForSeconds((float)numberOfBeatsDuration * (60f / musicController.getBPM()));
        shield.SetActive(false);
        saveShieldCoroutine = null;
    }
}
