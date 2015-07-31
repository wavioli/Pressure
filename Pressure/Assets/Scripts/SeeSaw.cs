using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SeeSaw : MonoBehaviour {

    // sprites and animation
    public Text whichButton, scoreText;

    //which player?
    public enum Player { P1, P2 }

    public Player WhichPlayer;

    public int score = 0;
    private string playerLeft, playerRight;

    //what side is active?
    private enum ActiveSide { left, right }
    private ActiveSide sideActive;
    private bool allowInput = true;

    // current time between switches
    
    
    public float Timer { get; set; }

    /// <summary>
    /// Switches between the left and right side
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchTimer()
    {
        Debug.Log("timer"+Timer);
        yield return new WaitForSeconds(Timer);
        switch (sideActive)
        {
            case ActiveSide.left:
                sideActive=ActiveSide.right;
                whichButton.text = "R";
                allowInput = true;
                break;
            case ActiveSide.right:
                sideActive= ActiveSide.left;
                whichButton.text = "L";
                allowInput = true;
                break;
        }
        UnityEngine.Debug.Log("activeside: "+sideActive);
        StartCoroutine("SwitchTimer");

    }

    /// <summary>
    /// Sent from GameManager to start the game
    /// </summary>
    public void StartRocking()
    {
        switch (WhichPlayer)
        {
            case Player.P1:
                Debug.Log("player1Set");
                playerLeft = "p1Left";
                playerRight = "p1Right";
                break;
            case Player.P2:
                Debug.Log("player2Set");
                playerLeft = "p2Left";
                playerRight = "p2Right";
                break;
        }
        StartCoroutine("SwitchTimer");
    }

    /// <summary>
    /// default method to stop the game
    /// </summary>
    void StopRocking()
    {
        StopCoroutine("SwitchTimer");
    }

    void Update()
    {
        Debug.Log(playerLeft + "playerLeft");
        Debug.Log(playerRight + "playerright");
        if (Input.GetButtonDown(playerLeft))
        { // user pressed left
            
            switch (sideActive)
            {
                case ActiveSide.left:
                    Debug.Log("UserPressed LEFT");
                    allowInput = false;
                    score++;
                    scoreText.text = score.ToString();
                    Timer *= 0.9f;
                    break;
                case ActiveSide.right:
                    Debug.Log("UserPressed LEFT");
                    allowInput = false;
                    Timer *= 1.1f;
                    break;
            }
        }
        else if (Input.GetButtonDown(playerRight))
        {
            
            switch (sideActive)
            {
                case ActiveSide.left:
                    Timer *= 1.1f;
                    Debug.Log("UserPressed RIGHT");
                    allowInput = false;
                    break;
                case ActiveSide.right:
                    Debug.Log("UserPressed RIGHT");
                    allowInput = false;
                    score++;
                    scoreText.text = score.ToString();
                    Timer *= 0.9f;
                    break;
            }
        }


    }
}
