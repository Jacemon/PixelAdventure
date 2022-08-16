using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private GameObject[] characters;

    private int charIndex;
    private int playerLife = 300;

    public int CharIndex
    {
        get { return charIndex; }
        set { charIndex = value; }
    }

    public int PlayerLife
    {
        get { return playerLife; }
        set { playerLife = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    void InstantiateCharacter(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu" && scene.name != "GameFinished" && scene.name != "GameOver")
            Instantiate(characters[charIndex]);
    }

}
