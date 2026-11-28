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
        if(tileCurrentlyOn == null)
        {

            //tileCurrentlyOn = controller.getAnyTile();
            moveToTile(controller.getAnyTile());
        }
        
        //Debug.Log($"{gameObject.name} object go to {tileCurrentlyOn.name}");
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void MoveDirection(Vector2 direction, Material material = null)
    {
        GameObject nextTile = controller.getNextMoveTile(tileCurrentlyOn, direction);
        if(tileCurrentlyOn == nextTile)
        {
            return;
        }
        moveToTile(nextTile, material);


    }

    


    public void moveToTile(GameObject tile, Material material = null)
    {
        tileCurrentlyOn = tile;
        Vector3 post = tile.transform.position;
        transform.position = new Vector3(post.x, transform.position.y, post.z);
        //esto de aqui es para hacer una ola de efecto de tamaño y color, pero por ahora voy a bloquearño a tan solo el jugador
        if (gameObject.CompareTag("Player"))
        {
            ScriptFloorTile sft = tile.GetComponent<ScriptFloorTile>();
            float BPMratio = 60f / musicController.getBPM();
            sft.changeColor(BPMratio / 4f, 0f, material, tile, true);
            sft.changeSize(BPMratio / 4f, 0f, tile, true);
        }
        if (material != null)
        {
            ScriptFloorTile sft = tile.GetComponent<ScriptFloorTile>();
            float BPMratio = 60f / musicController.getBPM();
            sft.changeColor(BPMratio, 0f, material, tile, false, true);
            sft.changeSize(BPMratio, 0f, tile, false, true);
        }

    }

    public GameObject getTileCurrentlyOn()
    {
        return tileCurrentlyOn;
    }
    
}
