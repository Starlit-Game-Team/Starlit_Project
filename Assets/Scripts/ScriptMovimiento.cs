using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptMovimiento : MonoBehaviour
{
    [SerializeField] InputActionReference move;

    GameObject tileCurrentlyOn;
    ScriptDanceFloorController controller;
    ScriptMusicBeat musicController;


    float moveCooldown = 1f;
    private void Awake()
    {
        controller = FindAnyObjectByType<ScriptDanceFloorController>();
        musicController = FindAnyObjectByType<ScriptMusicBeat>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileCurrentlyOn = controller.getAnyTile();
    }

    // Update is called once per frame
    void Update()
    {
        moveCooldown -= Time.deltaTime;
    }

    private void OnEnable()
    {
        move.action.started += Move;
        //move.action.performed += Move;
        //move.action.canceled += Move;
    }
    private void OnDisable()
    {
        move.action.started -= Move;
        //move.action.performed -= Move;
        //move.action.canceled -= Move;
    }

    private void Move(InputAction.CallbackContext context)
    {

        if (musicController.onTempo() && moveCooldown < 0)
        {
            
            Vector2 movementInput = move.action.ReadValue<Vector2>();
            movementInput = Mathf.Abs(movementInput.x) >= (Mathf.Abs(movementInput.y)) ? new Vector2((Mathf.Abs(movementInput.x)/ movementInput.x), 0) : new Vector2(0, (Mathf.Abs(movementInput.y) / movementInput.y));
            GameObject nextTile = controller.getNextMoveTile(tileCurrentlyOn, movementInput);
            //Debug.Log($"Current tile: {tileCurrentlyOn.name}, Next tile: {nextTile.name}");
            moveToTile(nextTile);
        }
        else
        {
            if(moveCooldown < 0)
            {
                moveCooldown = 60f / musicController.getBPM() * 0.75f;
            }
            
        }
        
    }

    private void moveToTile(GameObject tile)
    {
        tileCurrentlyOn = tile;
        Vector3 post = tile.transform.position;
        transform.position = new Vector3(post.x, transform.position.y, post.z);
        ScriptFloorTile sft = tile.GetComponent<ScriptFloorTile>();
        float BPMratio = 60f / musicController.getBPM();
        sft.changeColor(BPMratio/4f, 0f, null, tile, true);
        sft.changeSize(BPMratio/4f, 0f, tile, true);
    }
}
