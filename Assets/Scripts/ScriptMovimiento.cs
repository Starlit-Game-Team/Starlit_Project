using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptMovimiento : MonoBehaviour
{
    [SerializeField] InputActionReference move;

    GameObject tileCurrentlyOn;
    ScriptDanceFloorController controller;


    float moveCooldown = 1f;
    private void Awake()
    {
        controller = FindAnyObjectByType<ScriptDanceFloorController>();
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
        move.action.performed += Move;
        //move.action.canceled += Move;
    }
    private void OnDisable()
    {
        move.action.started -= Move;
        move.action.performed -= Move;
        //move.action.canceled -= Move;
    }

    private void Move(InputAction.CallbackContext context)
    {
        if (moveCooldown < 0)
        {
            moveCooldown = 0.3f;
            Vector2 movementInput = move.action.ReadValue<Vector2>();
            movementInput = Mathf.Abs(movementInput.x) >= (Mathf.Abs(movementInput.y)) ? new Vector2((Mathf.Abs(movementInput.x)/ movementInput.x), 0) : new Vector2(0, (Mathf.Abs(movementInput.y) / movementInput.y));
            GameObject nextTile = controller.getNextMoveTile(tileCurrentlyOn, movementInput);
            print($"Current tile: {tileCurrentlyOn.name}, Next tile: {nextTile.name}");
            moveToTile(nextTile);
        }
    }

    private void moveToTile(GameObject tile)
    {
        tileCurrentlyOn = tile;
        Vector3 post = tile.transform.position;
        transform.position = new Vector3(post.x, transform.position.y, post.z);
        tile.GetComponent<ScriptFloorTile>().changeColor(0.05f, 0f, null, tile, true);
    }
}
