using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    //Players
    public PlayerControl Player1;
    public PlayerControl Player2;
    public GameManager gameManager;


    //Text boxes
    public TMP_Text RoundAndTimerDisplay;
    public TMP_Text Scoreboard;
    public TMP_Text P1Block;
    public TMP_Text P2Block;
    public TMP_Text Advantage;
    public GameObject Info;

    public TMP_Text P1Off;
    public TMP_Text P2Off;
    public TMP_Text P1Def;
    public TMP_Text P2Def;

    public TMP_Text P1Hit;
    public TMP_Text P2Hit;


    //Relevant stats to display
    public int MaxTime;
    public int RoundNum;
    public int P1Score;
    public int P2Score;

    //Round RoundTimer stats
    public int RoundTimer;
    public int MaxRoundTimer;
    public float RoundCountdown;

    //Block RoundTimer stats
    bool BlockTextVisible;
    public float MaxBlockTimer;
    public float BlockTimer;

    public bool Counting = true;



    // Start is called before the first frame update
    void Start()
    {
        
        ResetDisplays();
    }

    // Update is called once per frame
    void Update()
    {

        
        UpdateRoundTimer();
       
        //UpdateDisplay();
        if (BlockTextVisible)
        {
            BlockTimer -= Time.deltaTime;
            if (BlockTimer <= 0)
            {
                BlockTextVisible = false;
                P1Block.text = "";
                P2Block.text = "";
                P1Hit.text = ""; 
                P2Hit.text = ""; 
                BlockTimer = 0;
            }
        }
    }


    //Updates the display
    void UpdateDisplay()
    {
        if (RoundTimer <= 0 && RoundCountdown <= 0)
        {
            Player1.Switch();
            Player2.Switch();
            BlockTimer = MaxBlockTimer;
            BlockTextVisible = true;
            Counting = false;
            gameManager.NewRound();

            
        }
        else if (Counting)
        {
            RoundAndTimerDisplay.text = "Round " + RoundNum + ":\n" + RoundTimer;
            Scoreboard.text = P1Score + " - " + P2Score;
        }
    }

    //Updates the RoundTimer
    void UpdateRoundTimer()
    {
        if (Counting)
        {
            RoundCountdown -= Time.deltaTime;
           

            if (RoundCountdown <= 0)
            {
              
             
                RoundTimer -= 1;
                UpdateDisplay();
                RoundCountdown = 1;
            }
        }
    }

    //Resets displays to default state
    public void ResetDisplays()
    {
        //Resets variables
        RoundNum = 1;
        RoundTimer = MaxTime;
        RoundCountdown = 1;
        P1Score = 0;
        P2Score = 0;
        BlockTimer = 0;
        P1Block.text = "";
        P2Block.text = "";
        P1Hit.text = "";
        P2Hit.text = "";


        //Resets displays
        UpdateDisplay();
    }

    //Updates the score and round # and resets the RoundTimer when a strike is landed

    public void StrikeLanded(bool Player1Attacker)
    {
        if (Player1Attacker)
        {
            P1Score += 1;
            P1Hit.text = "Hit!"; 
            P2Hit.text = ""; 
        }
        else
        {
            P2Score += 1;
            P2Hit.text = "Hit!"; 
            P1Hit.text = ""; 
        }

        BlockTimer = MaxBlockTimer;
        BlockTextVisible = true;
        UpdateDisplay();
        Counting = false;

    }


    //Updates the round # and resets the RoundTimer when a strike is blocked
    public void StrikeBlocked(bool Player1Attacker)
    {
        Debug.Log("Strike was blocked!");
        if (Player1Attacker) P2Block.text = "Block!";
        else P1Block.text = "Block!";
        BlockTextVisible = true;
        BlockTimer = MaxBlockTimer;
        UpdateDisplay();
        Counting = false;

    }
}
