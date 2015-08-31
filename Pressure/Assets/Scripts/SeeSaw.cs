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
    public AudioClip plAudioShoutEvent, prAudioShoutEvent, MissAudioEvent, winSound;
    AudioSource audio;
    public bool showTimer;
    

    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        bubAnim = bubble.GetComponent<Animator>();
        plAnim = playerLeftsprite.GetComponent<Animator>();
        prAnim = playerRightSprite.GetComponent<Animator>();
        plungeAnim = plunger.GetComponent<Animator>();
        switch (WhichPlayer)
        {
            case Player.P1:
                playerLeft = "p1Left";
                playerRight = "p1Right";
                //plAudioShoutEvent = "p1LShout";
                //prAudioShoutEvent = "p1RShout";
                //plMissAudioEvent = "p1LMiss";
                //prMissAudioEvent = "p1RMiss";
                break;
            case Player.P2:
                playerLeft = "p2Left";
                playerRight = "p2Right";
                //plAudioShoutEvent = "p2LShout";
                //prAudioShoutEvent = "p2RShout";
                //plMissAudioEvent = "p2LMiss";
                //prMissAudioEvent = "p2RMiss";
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
        if (GameManager.instance.gameState == GameManager.GameState.InGame)
        {
            //Debug.Log("timer"+Timer);
            yield return new WaitForSeconds(Timer);
            switch (sideActive)
            {
                case ActiveSide.left:
                    sideActive = ActiveSide.right;
                    plAnim.SetTrigger("Normal");
                    prAnim.SetTrigger("Shout");
                    audio.PlayOneShot(plAudioShoutEvent);
                    // Debug.Log("audioEvent: " + plAudioShoutEvent);
                    // Debug.Log("RIGHT");
                    // whichButton.text = "R";
                    allowInput = true;
                    break;
                case ActiveSide.right:
                    sideActive = ActiveSide.left;
                    plAnim.SetTrigger("Shout");
                    prAnim.SetTrigger("Normal");
                    audio.PlayOneShot(prAudioShoutEvent);
                    //Debug.Log("audioEvent: " + prAudioShoutEvent);
                    // Debug.Log("LEFT");
                    // whichButton.text = "L";
                    allowInput = true;
                    break;
            }

            StartCoroutine("SwitchTimer");
        }
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
        if (GameManager.instance.gameState == GameManager.GameState.InGame)
        {
            if (showTimer)
                Debug.Log("Timer: " + Timer);
            if (Input.GetButtonDown(playerLeft))
            { // user pressed left

                switch (sideActive)
                {
                    case ActiveSide.left:

                        allowInput = false;
                        score++;
                        bubAnim.SetInteger("BubbleSize", score);
                        plungeAnim.SetTrigger("Plunge");
                       
                        //scoreText.text = score.ToString();
                        if (Timer > 0.3f)
                        {
                            Timer *= 0.95f;
                            audio.pitch = 1 + (1f - Timer);
                        }
                        if (score >= GameManager.instance.winScore)
                        {
                            Win();
                        }

                        break;
                    case ActiveSide.right:

                        prAnim.SetTrigger("Sad");
                        //Fabric.EventManager.Instance.
                        //Debug.Log("audioEvent: "+);
                        audio.PlayOneShot(MissAudioEvent);
                        allowInput = false;
                        if (Timer < 1f)
                        {
                            Timer *= 1.05f;
                            audio.pitch = 1 + (1f - Timer);
                        }

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
                        bubAnim.SetInteger("BubbleSize", score);
                        plAnim.SetTrigger("Sad");
                        if (Timer < 1f)
                        {
                            Timer *= 1.05f;
                            audio.pitch = 1 + (1f - Timer);
                        }
                        audio.PlayOneShot(MissAudioEvent);
                        allowInput = false;
                        break;
                    case ActiveSide.right:
                        //Debug.Log("UserPressed RIGHT");
                        allowInput = false;
                        score++;
                        plungeAnim.SetTrigger("Plunge");
                       
                        bubAnim.SetInteger("BubbleSize", score);
                        if (Timer > 0.3f)
                        {
                            Timer *= 0.95f;
                            audio.pitch = 1 + (1f - Timer);
                        }

                        //scoreText.text = score.ToString();
                        Timer *= 0.9f;
                        if (score >= GameManager.instance.winScore)
                        {
                            Win();
                        }
                        break;
                }
            }

        }
    }
    void Win()
    {
        StopAllCoroutines();
        plAnim.SetTrigger("Won");
        prAnim.SetTrigger("Won");
        audio.pitch = 1;
        audio.PlayOneShot(winSound);
        score = 0;
        GameManager.instance.GameOver(gameObject.name);
    }

    public void Lost()
    {
        StopAllCoroutines();
        plAnim.SetTrigger("Lost");
        prAnim.SetTrigger("Lost");
        audio.PlayOneShot(MissAudioEvent);
        score = 0;
    }
}
