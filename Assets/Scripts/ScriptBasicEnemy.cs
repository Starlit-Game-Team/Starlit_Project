using UnityEngine;
[RequireComponent(typeof(ScriptMovimiento))]


public class ScriptBasicEnemy : MonoBehaviour
{
    
    [Header("Behaviour")]

    [SerializeField] Direction directionMovement;
    [SerializeField] bool contactDamage = true;
    [SerializeField] int damage = 1;

    [Header("FXS")]

    [SerializeField] Material tileMaterial;


    ScriptMovimiento scriptMovimiento;
    ScriptMovimiento player;

    GameObject lastTile;


    private void Awake()
    {
        

    }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scriptMovimiento = GetComponent<ScriptMovimiento>();
        player = FindAnyObjectByType<ScriptPlayerController>().GetComponent<ScriptMovimiento>();
        ScriptMusicBeat.halfBeat += move;
        scriptMovimiento.moveToTile(FindAnyObjectByType<ScriptDanceFloorController>().getTileOpositeDirectionBorder(Utilities.fromDirectionToVector2(directionMovement)), tileMaterial);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        ScriptMusicBeat.halfBeat -= move;
    }


    private void move()
    {
        if (lastTile == player.getTileCurrentlyOn())
        {
            hurtPlayer();
            Destroy(gameObject);
            return;
        }

        scriptMovimiento.MoveDirection(Utilities.fromDirectionToVector2(directionMovement), tileMaterial);
        GameObject currentTile = scriptMovimiento.getTileCurrentlyOn();
        
        if(currentTile == player.getTileCurrentlyOn())
        {
            hurtPlayer();
            Destroy(gameObject);
        }
        if(currentTile == lastTile)
        {
            //ha llegado al final
            Destroy(gameObject);
        }
        lastTile = currentTile;
    }

    

    private void hurtPlayer()
    {
        ScriptLive sL = FindAnyObjectByType<ScriptLive>();
        if (sL != null)
        {
            sL.addLive(damage*-1);
        }
    }
}
