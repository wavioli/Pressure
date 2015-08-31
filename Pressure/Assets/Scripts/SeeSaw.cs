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
    public GameObject bubble, playerLeftsprite, playerRightSprite, plunger;
    private Animator bubAnim, plAnim, prAnim, plungeAnim;
    private int bubbleState = 0;
    private string plAudioShoutEvent, prAudioShoutEvent, plMissAudioEvent, prMissAudioEvent;

    void Start()
    {
        Fabric.EventManager.Instance.PostEvent("p1LShout");
        bubAnim = bubble.GetComponent<Animator>();
        plAnim = playerLeftsprite.GetComponent<Animator>();
        prAnim = playerRightSprite.GetComponent<Animator>();
        plungeAnim = plunger.GetComponent<Animator>();
        switch (WhichPlayer)
        {
            case Player.P1:
                Debug.Log("player1Set");
                playerLeft = "p1Left";
                playerRight = "p1Right";
                plAudioShoutEvent = "p1LShout";
                prAudioShoutEvent = "p1RShout";
                plMissAudioEvent = "p1LMiss";
                prMissAudioEvent = "p1RMiss";
                break;
            case Player.P2:
                Debug.Log("player2Set");
                playerLeft = "p2Left";
                playerRight = "p2Right";
                plAudioShoutEvent = "p2LShout";
                prAudioShoutEvent = "p2RShout";
                plMissAudioEvent = "p2LMiss";
                prMissAudioEvent = "p2RMiss";
                break;
        }

    }

    // current time between switches
    
    
    public float Timer { get; set; }

    /// <summary>
    /// Switches between the left and right side
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchTimer()
    {
        //Debug.Log("timer"+Timer);
        yield return new WaitForSeconds(Timer);
        switch (sideActive)
        {
            case ActiveSide.left:
                sideActive=ActiveSide.right;
                plAnim.SetTrigger("Normal");
                prAnim.SetTrigger("Shout");
                Fabric.EventManager.Instance.PostEvent(plAudioShoutEvent, Fabric.EventAction.PlaySound, null, gameObject);
                Debug.Log("audioEvent: " + plAudioShoutEvent);
               // Debug.Log("RIGHT");
               // whichButton.text = "R";
                allowInput = true;
                break;
            case ActiveSide.right:
                sideActive= ActiveSide.left;
                plAnim.SetTrigger("Shout");
                prAnim.SetTrigger("Normal");
                Fabric.EventManager.Instance.PostEvent(prAudioShoutEvent, Fabric.EventAction.PlaySound, null, gameObject);
                Debug.Log("audioEvent: " + prAudioShoutEvent);
               // Debug.Log("LEFT");
               // whichButton.text = "L";
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
                    
                    allowInput = false;
                    score++;
                    bubAnim.SetInteger("BubbleSize",score); 
                    plungeAnim.SetTrigger("Plunge");
                    //scoreText.text = score.ToString();
                    Timer *= 0.9f;
                    break;
                case ActiveSide.right:
                   
                    prAnim.SetTrigger("Sad");
                    Fabric.EventManager.Instance.PostEvent(prMissAudioEvent, Fabric.EventAction.PlaySound, null, gameObject);
                    //Fabric.EventManager.Instance.
                    Debug.Log("audioEvent: "+prMissAudioEvent);
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
                    //Debug.Log("UserPressed RIGHT");
                    score--;
                    plAnim.SetTrigger("Sad");
                    Fabric.EventManager.Instance.PostEvent(plMissAudioEvent, Fabric.EventAction.PlaySound, null, gameObject);
                    allowInput = false;
                    break;
                case ActiveSide.right:
                    //Debug.Log("UserPressed RIGHT");
                    allowInput = false;
                    score++;
                    plungeAnim.SetTrigger("Plunge");
                    bubAnim.SetInteger("BubbleSize", score);
                    //scoreText.text = score.ToString();
                    Timer *= 0.9f;
                    break;
            }
        }


    }
}
