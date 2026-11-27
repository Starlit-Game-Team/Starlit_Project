using UnityEngine;

public class SpawnBasciEnemies : MonoBehaviour
{
    [SerializeField] GameObject[] basicEnemies;
    [SerializeField] int spawnRateInBeats = 2;
    int i = 67;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        i = spawnRateInBeats;
        
    }
    private void OnEnable()
    {
        ScriptMusicBeat.halfBeat += trySpawnEnemy;
    }
    private void OnDisable()
    {
        ScriptMusicBeat.halfBeat -= trySpawnEnemy;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    private void trySpawnEnemy() //aqui el input es inutil
    {
        i--;
        if (i <= 0)
        {
            Instantiate(basicEnemies[Random.Range(0, basicEnemies.Length)]);
            i = spawnRateInBeats;
        }
        
    }
}
