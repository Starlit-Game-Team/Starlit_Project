using UnityEngine;

public class ScriptDanceFloorController : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [Header("Create Dance Floor")]
    [SerializeField] bool createFloor;
    [SerializeField] Vector2Int FloorSize = new Vector2Int(10, 10);
    [SerializeField] float distanceFromTiles = 1;

    GameObject[][] tileFloorMatrix;

    private void Awake()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("TilePrefab is null");
            return;
        }
        if (tilePrefab.GetComponent<ScriptFloorTile>() == null)
        {
            Debug.LogError("Invalid Action: TilePregab doesn't have ScriptFloorTile");
            return;
        }
        if (createFloor)
        {
            tileFloorMatrix = F_SetMatrixNeighbours(F_CreateFloor(FloorSize, distanceFromTiles, tilePrefab));
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private GameObject[][] F_CreateFloor(Vector2Int size, float distance, GameObject go)
    {
        GameObject[][] matrix = new GameObject[size.y][];
        for (int i = 0; i < size[0]; i++)
        {
            matrix[i] = new GameObject[size.x];
            for (int j = 0; j < size[1]; j++)
            {
                Vector3 dist = new Vector3((j* distance) -(distance * (size.x-1)/2),
                    0.05f,
                    (i * distance) - (distance * (size.y - 1) / 2));
                GameObject goinst = Instantiate(go, dist, Quaternion.identity, transform);
                goinst.name = $"Tile [{i},{j}]";
                matrix[i][j] = goinst;
            }
        }


        return matrix;
    }

    GameObject[][] F_SetMatrixNeighbours(GameObject[][] matrix)
    {
        for(int i =0; i< matrix.Length; i++)
        {
            for(int j = 0;j< matrix[i].Length; j++)
            {
                 /*
                 * 0 --> Front
                 * 1 --> Right
                 * 2 --> Back
                 * 3 --> Left
                 */

                GameObject[] neighborarr =
                {
                    (i>0)? matrix[i-1][j]: null,
                    (j>0)? matrix[i][j-1]: null,
                    (i<matrix.Length-1)? matrix[i+1][j]: null,
                    (j<matrix[i].Length-1)? matrix[i][j+1]: null,

                };

                ScriptFloorTile sft = matrix[i][j].GetComponent<ScriptFloorTile>();
                sft.setNeigbours(neighborarr);
                sft.setController(this);




            }
        }
        return matrix;
    }

    public GameObject getAnyTile()
    {
        return tileFloorMatrix[Random.Range(0, tileFloorMatrix.Length)][Random.Range(0, tileFloorMatrix[0].Length)];
    }
    public GameObject getNextMoveTile(GameObject tile, Vector2 move)
    {

        //OJO esta es una forma de acceder a las celdas a traves del controlador, pero tambien se puede hacer a trabes de las celdas mismas
        //nombre de los tiles --> "Tile [i,j]"
        string[] splitName = tile.name.Split('[', ',', ']');
        int y = int.Parse(splitName[1]) + (int)move.y;
        int x = int.Parse(splitName[2]) + (int)move.x;
        Debug.Log($"y: {y}, x: {x}");
        if(y>=0 && y<tileFloorMatrix.Length && x >= 0 && x < tileFloorMatrix[y].Length)
        {
            return tileFloorMatrix[y][x];
        }
        return tile;

    }
}
