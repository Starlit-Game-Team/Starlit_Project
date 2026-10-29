using System;
using System.Collections;
using UnityEngine;

public class ScriptFloorTile : MonoBehaviour
{
    [SerializeField]
    Material[] Materials = new Material[2];
    ScriptDanceFloorController controller;
    [SerializeField]
    AnimationCurve sizeCurve;
    GameObject[] NeighbourTiles = new GameObject[4];
    /*
     * 0 --> Front
     * 1 --> Right
     * 2 --> Back
     * 3 --> Left
     */


    Vector3 saveSize;
    MeshRenderer renderer;
    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        saveSize = transform.localScale;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        changeColor(0.05f, 0f, Materials[1], gameObject, true);
    }

    #region neigbour setting

    public void setNeigbours(GameObject[] neighbours)
    {
        if( neighbours.Length != 4)
        {
            Debug.LogError($"Invalid array length. Expected: 4; Received: {neighbours.Length}");
        }
        for(int i =0; i< neighbours.Length; i++)
        {
            if (neighbours[i] != null)
            {
                if (neighbours[i].GetComponent<ScriptFloorTile>() != null)
                {
                    NeighbourTiles[i] = neighbours[i];
                }
                else
                {
                    Debug.LogError("Invalid action: Tried to assign a gameobject without the floorTile script as a neighbour");
                }
                
            }
        }
    }
    public void setController(ScriptDanceFloorController controller)
    {
        this.controller = controller;
    }

    #endregion


    #region effects
    #region color
    Coroutine ChangeColorCoroutineSaver;
    public void changeColor(float time, float delay, Material material, GameObject originN,  bool repeatWave)
    {
        if(material == null)
        {
            material = Materials[1];
        }
        if (ChangeColorCoroutineSaver == null)
        {
            ChangeColorCoroutineSaver = StartCoroutine(ChangeColorCoroutine(time, delay, material, originN, repeatWave));
        }
    }
    IEnumerator ChangeColorCoroutine(float time, float delay, Material material, GameObject originN,  bool repeatWave)
    {
        yield return new WaitForSeconds(delay);
        ChangeMaterial(material);
        yield return new WaitForSeconds(time);
        ChangeMaterial(Materials[0]);
        if (repeatWave)
        {
            if (originN != null)
            {
                Vector3 normalVectorFromOrigin = (originN.transform.position - transform.position).normalized;
                
                //esta parte del codigo es para que los tiles con un eje en comun lideren la onda hacia el exterior (eje de las y)
                if (normalVectorFromOrigin.x == 0) //el origen esta en la misma linea horizontal
                {
                    if (normalVectorFromOrigin.z >= 0) // el origen esta atras
                    {
                        if (NeighbourTiles[0] != null)
                        {
                            NeighbourTiles[0].GetComponent<ScriptFloorTile>().changeColor(time, delay, material, originN, repeatWave);
                        }
                        
                    }
                    if (normalVectorFromOrigin.z <= 0) // el orifen esta delante
                    {
                        if (NeighbourTiles[2] != null)
                        {
                            NeighbourTiles[2].GetComponent<ScriptFloorTile>().changeColor(time, delay, material, originN, repeatWave);
                        }
                    }
                }

                //esta parte es para repetir la expansion sobre el eje de las x
                //para que te hagas una idea esto solo hace una onda expansiba en horizontal
                if (normalVectorFromOrigin.x >= 0)
                {
                    if (NeighbourTiles[1] != null)
                    {
                        NeighbourTiles[1].GetComponent<ScriptFloorTile>().changeColor(time, delay, material, originN, repeatWave);
                    }
                }
                if (normalVectorFromOrigin.x <= 0)
                {
                    if (NeighbourTiles[3] != null)
                    {
                        NeighbourTiles[3].GetComponent<ScriptFloorTile>().changeColor(time, delay, material, originN, repeatWave);
                    }
                }

            }
            else
            {
                Debug.LogError("Invalid action: neighbor is null while repeatWave is true ");
            }

        }
        ChangeColorCoroutineSaver = null;
    }


    #endregion

    #region Size
    Coroutine ChangeSizeCoroutineSaver;
    public void changeSize(float time, float delay, GameObject originN, bool repeatWave)
    {
        if (ChangeSizeCoroutineSaver == null)
        {
            ChangeSizeCoroutineSaver = StartCoroutine(ChangeSizeCoroutine(time, delay, originN, repeatWave));
        }
    }
    IEnumerator ChangeSizeCoroutine(float time, float delay, GameObject originN, bool repeatWave)
    {
        yield return new WaitForSeconds(delay);
        float timer = 0;
        while(timer < time)
        {
            transform.localScale = new Vector3(saveSize.x, saveSize.y * sizeCurve.Evaluate(timer / time), saveSize.z);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = saveSize;

        if (repeatWave)
        {
            if (originN != null)
            {
                Vector3 normalVectorFromOrigin = (originN.transform.position - transform.position).normalized;

                //esta parte del codigo es para que los tiles con un eje en comun lideren la onda hacia el exterior (eje de las y)
                if (normalVectorFromOrigin.x == 0) //el origen esta en la misma linea horizontal
                {
                    if (normalVectorFromOrigin.z >= 0) // el origen esta atras
                    {
                        if (NeighbourTiles[0] != null)
                        {
                            NeighbourTiles[0].GetComponent<ScriptFloorTile>().changeSize(time, delay, originN, repeatWave);
                        }

                    }
                    if (normalVectorFromOrigin.z <= 0) // el orifen esta delante
                    {
                        if (NeighbourTiles[2] != null)
                        {
                            NeighbourTiles[2].GetComponent<ScriptFloorTile>().changeSize(time, delay, originN, repeatWave);
                        }
                    }
                }

                //esta parte es para repetir la expansion sobre el eje de las x
                //para que te hagas una idea esto solo hace una onda expansiba en horizontal
                if (normalVectorFromOrigin.x >= 0)
                {
                    if (NeighbourTiles[1] != null)
                    {
                        NeighbourTiles[1].GetComponent<ScriptFloorTile>().changeSize(time, delay, originN, repeatWave);
                    }
                }
                if (normalVectorFromOrigin.x <= 0)
                {
                    if (NeighbourTiles[3] != null)
                    {
                        NeighbourTiles[3].GetComponent<ScriptFloorTile>().changeSize(time, delay, originN, repeatWave);
                    }
                }

            }
            else
            {
                Debug.LogError("Invalid action: neighbor is null while repeatWave is true ");
            }

        }
        ChangeSizeCoroutineSaver = null;
    }


    #endregion

    void ChangeMaterial(Material material)
    {
        Material materialOld = renderer.material;
        renderer.material = material;
        Destroy(materialOld);
    }
    #endregion
}
