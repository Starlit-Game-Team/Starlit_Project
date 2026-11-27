using UnityEngine;
[RequireComponent(typeof(ScriptMovimiento))]


public class ScriptBasicEnemy : MonoBehaviour
{
    private enum Direction { 
        Up, Down, Left, Right
    }
    [Header("Behaviour")]

    [SerializeField] Direction directionMovement;
    [SerializeField] bool contactDamage = true;
    [SerializeField] float damage = 1;

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
        scriptMovimiento.moveToTile(FindAnyObjectByType<ScriptDanceFloorController>().getTileOpositeDirectionBorder(translateDirection(directionMovement)), tileMaterial);

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
            //hacer daño al jugador y...
            Destroy(gameObject);
            return;
        }

        scriptMovimiento.MoveDirection(translateDirection(directionMovement), tileMaterial);
        GameObject currentTile = scriptMovimiento.getTileCurrentlyOn();
        
        if(currentTile == player.getTileCurrentlyOn())
        {
            //hacer daño al jugador y...
            Destroy(gameObject);
        }
        if(currentTile == lastTile)
        {
            //ha llegado al final
            Destroy(gameObject);
        }
        lastTile = currentTile;
    }

    private Vector2 translateDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector2(0f, 1f);
            case Direction.Down:
                return new Vector2(0f, -1f);
            case Direction.Left:
                return new Vector2(-1f, 0f);
            case Direction.Right:
                return new Vector2(1f, 0f);
        }
        return new Vector2(0f, 0f);
    }
}
