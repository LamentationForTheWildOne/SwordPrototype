using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int pos = 0;
    public int scaling;
    public int round;
    public int direction;
    public GameObject p1;
    public GameObject p2;
    public PlayerControl p1control;
    public PlayerControl p2control;
    public TextDisplay textDisplay;
    public int MaxRoundTime;
    public int NewRoundTime;

    // Start is called before the first frame update
    void Start()
    {
        p1control = p1.GetComponent<PlayerControl>();
        p2control = p2.GetComponent<PlayerControl>();
    }

    public void Reposition(int playernum) 
    {
        if (playernum == 1)
        {
            direction = 1;
        }
        else 
        {
            direction = -1;
        }
        pos = ((10 * scaling) * direction);
        transform.position = new Vector2(transform.position.x + pos, transform.position.y);
        p1.transform.position = new Vector2(transform.position.x - 3, p1.transform.position.y);
        p2.transform.position = new Vector2(transform.position.x + 3, p2.transform.position.y);
    }

    public void NewRound() 
    {
        textDisplay.P1Off.text = "Offensive: " + p1control.offcool;
        textDisplay.P2Off.text = "Offensive: " + p2control.offcool;
        textDisplay.P1Def.text = "Defensive: " + p1control.defcool;
        textDisplay.P2Def.text = "Defensive: " + p2control.defcool;

        textDisplay.Advantage.text = "" + transform.position.x;
        textDisplay.RoundAndTimerDisplay.text = "PREROUND";
        textDisplay.Info.SetActive(true);
        NewRoundTime = MaxRoundTime;
        StartCoroutine(Preround());
    
    }

    public void Impatient() 
    {
        NewRoundTime -= 5;
    }

    IEnumerator Preround() 
    {
    yield return new WaitForSeconds(2);
        textDisplay.RoundCountdown = 1;
        textDisplay.RoundNum += 1;
        textDisplay.RoundTimer = NewRoundTime;
        textDisplay.Info.SetActive(false);
        textDisplay.Counting = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
