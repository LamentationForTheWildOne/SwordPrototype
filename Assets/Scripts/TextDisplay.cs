using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    //Text boxes
    public TMP_Text RoundAndTimerDisplay;
    public TMP_Text Scoreboard;

    //Relevant stats to display
    public int MaxTime;
    public int Timer;
    public int RoundNum;
    public int P1Score;
    public int P2Score;

    // Start is called before the first frame update
    void Start()
    {
        ResetDisplays();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Resets displays to default state
    public void ResetDisplays()
    {
        //Resets variables
        RoundNum = 1;
        Timer = MaxTime;
        P1Score = 0;
        P2Score = 0;

        //Resets displays
        RoundAndTimerDisplay.text = "Round " + RoundNum + ":\n" + Timer;
        Scoreboard.text = P1Score + " - " + P2Score;
    }
}
