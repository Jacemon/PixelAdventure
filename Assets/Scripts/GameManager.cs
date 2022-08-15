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

    public int CharIndex
    {
        get { return charIndex; }
        set { charIndex = value; }
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
        if (scene.name != "MainMenu" && scene.name != "GameFinished")
            Instantiate(characters[charIndex]);
    }
}
