using UnityEngine;
using UnityEngine.UI;

public class ScriptLive : MonoBehaviour
{
    [SerializeField] Sprite[] liveSprites;
    Image[] lives;
    int currentHp = 100;
    private void Awake()
    {
        lives = GetComponentsInChildren<Image>();
        if(lives == null)
        {
            Debug.LogError("ScriptLive is not' finding Image children");
        }
        currentHp = lives.Length;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void addLive(int hp)
    {
        
        int newHealth = Mathf.Clamp(currentHp + hp, 0, lives.Length); 
        while (newHealth != currentHp)
        {
            if(newHealth > currentHp)
            {
                currentHp++;
                lives[currentHp - 1].sprite = liveSprites[0];
            } else if (newHealth < currentHp)
            {

                lives[currentHp - 1].sprite = liveSprites[1];
                currentHp--;
            }
        }
        if (currentHp <= 0)
        {
            //TODO:hacer lo que sea necesario al matar al jugador
        }
    }
}
