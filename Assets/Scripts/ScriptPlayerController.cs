using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
[RequireComponent(typeof(ScriptMovimiento))]
public class ScriptPlayerController : MonoBehaviour
{

    [SerializeField] InputActionReference move;




    ScriptMusicBeat musicController;

    ScriptMovimiento moveScript;


    float moveCooldown = 0f;

    private void Awake()
    {
        moveScript = GetComponent<ScriptMovimiento>();

        musicController = FindAnyObjectByType<ScriptMusicBeat>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveCooldown -= Time.deltaTime;
    }

    private void OnEnable()
    {
        move.action.started += tryMove;
        //move.action.performed += Move;
        //move.action.canceled += Move;
    }

    private void OnDisable()
    {
        move.action.started -= tryMove;
        //move.action.performed -= Move;
        //move.action.canceled -= Move;
    }

   

    private void tryMove(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        if (musicController.onTempo() && moveCooldown < 0)
        {
            movementInput = Mathf.Abs(movementInput.x) >= (Mathf.Abs(movementInput.y)) ? new Vector2((Mathf.Abs(movementInput.x) / movementInput.x), 0) : new Vector2(0, (Mathf.Abs(movementInput.y) / movementInput.y));
            moveScript.MoveDirection(movementInput);
        }
        else
        {
            if (moveCooldown < 0)
            {
                moveCooldown = 60f / musicController.getBPM() * 0.75f;
            }

        }
    }
}
