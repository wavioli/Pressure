using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public enum GameState { Loading, StartScreen, InGame, WinScreen}
    public GameState gameState;
    public float startingTime;
    private Controls gameControls;
    public GameObject seeSaw1GO, seeSaw2GO;
    private SeeSaw seeSaw1, seeSaw2;
    public int winScore;
    public Text winText;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        gameState = GameState.Loading;
        InitGame();
    }

    void InitGame()
    {
        Debug.Log("Initialise game");
        winText.text = "";
        // spawn in players
       // seeSaw1GO = Instantiate(Resources.Load("SeeSaw"), new Vector3(0,0,0),Quaternion.identity ) as GameObject;
       // seeSaw2GO = Instantiate(Resources.Load("SeeSaw"), new Vector3(0, 0, 10), Quaternion.identity) as GameObject;
        gameState = GameState.InGame;
        seeSaw1 = seeSaw1GO.GetComponent<SeeSaw>();
        seeSaw2 = seeSaw2GO.GetComponent<SeeSaw>();
        seeSaw1.WhichPlayer = SeeSaw.Player.P1;
        seeSaw2.WhichPlayer = SeeSaw.Player.P2;
        seeSaw1.Timer = startingTime;
        seeSaw2.Timer = startingTime;
        seeSaw1.StartRocking();
        seeSaw2.StartRocking();
        // Initialise Controls
        //Initialise Sound engine
        //Load start menu 
    }

    public void GameOver(string winner)
    {
        switch (winner)
        {
            case "Player1":
                //p1 wins
                winText.text = "Player 1 Wins";
                seeSaw2.Lost();
                StartCoroutine("StartAgain");
                break;
            case "Player2":
                //p2 wins
                winText.text = "Player 2 Wins";
                seeSaw1.Lost();
                StartCoroutine("StartAgain");
                break;
        }
    }

    void Update()
    {
        if (gameState == GameState.WinScreen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InitGame();
            }
        }
    }

    IEnumerator StartAgain()
    {
        yield return new WaitForSeconds(3);
        winText.text = "Press Space To Play Again";
        gameState = GameState.WinScreen;
    }
}
