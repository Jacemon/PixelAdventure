using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerMaxLife = 8;
    public int playerLife;
    [FormerlySerializedAs("deathDamage")]
    [Space]
    public int trapDamage = 2;
    [FormerlySerializedAs("hitDamage")]
    public int enemyDamage = 1;
    
    [SerializeField]
    private GameObject[] characters;

    public int CharIndex { get; set; }

    private void Awake()
    { 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += InstantiateCharacter;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= InstantiateCharacter;
    }

    public void ReloadLives()
    {
        playerLife = playerMaxLife;
    }
    
    private void InstantiateCharacter(Scene scene, LoadSceneMode mode)
    {
        ReloadLives();
        if (scene.name != "MainMenu" && scene.name != "GameFinished" && scene.name != "GameOver")
            Instantiate(characters[CharIndex]);
    }

}
