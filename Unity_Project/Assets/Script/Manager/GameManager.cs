using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public enum GameState { GameState_InGame, GameState_Paused, GameState_InMenu, GameState_Dead }
    public GameState gameState = GameState.GameState_InGame;

    void Start () {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        InitGame();
	}

    void InitGame()
    {

    }

    void Update ()
    {
	
	}
}
