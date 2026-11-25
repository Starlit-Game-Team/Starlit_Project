using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptMovimiento : MonoBehaviour
{
    

    GameObject tileCurrentlyOn;
    ScriptDanceFloorController controller;
    ScriptMusicBeat musicController;


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

    }

    

    public void MoveDirection(Vector2 direction)
    {
        GameObject nextTile = controller.getNextMoveTile(tileCurrentlyOn, direction);
        moveToTile(nextTile);


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
