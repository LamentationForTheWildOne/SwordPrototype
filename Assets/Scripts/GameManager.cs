using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public Image p1hdisplay;
    public Image p1bdisplay;
    public Image p1ldisplay;
    public Image p2hdisplay;
    public Image p2bdisplay;
    public Image p2ldisplay;

    public int MaxRoundTime;
    public int NewRoundTime;

    public int p1head;
    public int p1body;
    public int p1legs;

    public int p2head;
    public int p2body;
    public int p2legs;

    public bool p1dead = false;
    public bool p2dead = false;



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

        p1control.swordH.GetComponent<SwordMovement>().Return();
        p1control.swordM.GetComponent<SwordMovement>().Return();
        p1control.swordL.GetComponent<SwordMovement>().Return();

        p2control.swordH.GetComponent<SwordMovement>().Return();
        p2control.swordM.GetComponent<SwordMovement>().Return();
        p2control.swordL.GetComponent<SwordMovement>().Return();
        
        if (transform.position.x <= -40)
        {
            p1control.state = Phase.GAMEOVER;
            p2control.state = Phase.GAMEOVER;
            textDisplay.RoundAndTimerDisplay.text = "P2 WINS!";
        }
        else if (transform.position.x >= 40)
        {
            p1control.state = Phase.GAMEOVER;
            p2control.state = Phase.GAMEOVER;
            textDisplay.RoundAndTimerDisplay.text = "P1 WINS";
        }
        else
        {
            if (p1dead)
            {
                p1control.state = Phase.GAMEOVER;
                p2control.state = Phase.GAMEOVER;
                textDisplay.RoundAndTimerDisplay.text = "P2 WINS!";
            }
            else if (p2dead)
            {
                p1control.state = Phase.GAMEOVER;
                p2control.state = Phase.GAMEOVER;
                textDisplay.RoundAndTimerDisplay.text = "P1 WINS";
            }
            else
            {
                if (p1control.state == Phase.DEFENDING)
                {
                    p1control.state = Phase.PREROUND;
                    p1control.StartPreround(Phase.ATTACKING);
                }
                else if (p1control.state == Phase.ATTACKING)
                {
                    p1control.state = Phase.PREROUND;
                    p1control.StartPreround(Phase.DEFENDING);
                }

                if (p2control.state == Phase.DEFENDING)
                {
                    p2control.state = Phase.PREROUND;
                    p2control.StartPreround(Phase.ATTACKING);
                }
                else if (p2control.state == Phase.ATTACKING)
                {
                    p2control.state = Phase.PREROUND;
                    p2control.StartPreround(Phase.DEFENDING);
                }

                textDisplay.P1Off.text = "Offensive: " + p1control.offcool;
                textDisplay.P2Off.text = "Offensive: " + p2control.offcool;
                textDisplay.P1Def.text = "Defensive: " + p1control.defcool;
                textDisplay.P2Def.text = "Defensive: " + p2control.defcool;

                textDisplay.Advantage.text = "" + transform.position.x;
                textDisplay.RoundAndTimerDisplay.text = "PREROUND";
                textDisplay.Info.SetActive(true);
                NewRoundTime = MaxRoundTime;
                StartCoroutine(GMPreround());
            }
        }
    
    }

    public void Impatient() 
    {
        NewRoundTime -= 5;
        Debug.Log("Impatient");
    }

    IEnumerator GMPreround() 
    {
    yield return new WaitForSeconds(2);
        textDisplay.RoundCountdown = 1;
        textDisplay.RoundNum += 1;
        textDisplay.RoundTimer = NewRoundTime;
        textDisplay.Info.SetActive(false);
        textDisplay.Counting = true;
    }

    public void PlayerHit(int player, int location) 
    {
        if (player == 1)
        {
            switch (location)
            {
                case 1:
                    p2head += 1;
                    p1head = 0;
                    break;
                case 2:
                    p2body += 1;
                    p1body = 0;
                    break;
                case 3:
                    p2legs += 1;
                    p1legs = 0;
                    break;
            }


        }
        else if (player == 2)
        {
            switch (location)
            {
                case 1:
                    p1head += 1;
                    p2head = 0;
                    break;
                case 2:
                    p1body += 1;
                    p2body = 0;
                    break;
                case 3:
                    p1legs += 1;
                    p2legs = 0;
                    break;
            }
        }
            switch (p1head) 
            {
                case 0:
                    p1hdisplay.color = Color.white;
                    break;
                case 1:
                    p1hdisplay.color = Color.yellow;
                    break;
                case 2:
                    p1hdisplay.color = Color.red;
                    break;
                case 3:
                    p1dead = true;
                    break;
            }
            switch (p1body)
            {
                case 0:
                    p1bdisplay.color = Color.white;
                    break;
                case 1:
                    p1bdisplay.color = Color.yellow;
                    break;
                case 2:
                    p1bdisplay.color = Color.red;
                    break;
            case 3:
                p1dead = true;
                break;
        }
            switch (p1legs)
            {
                case 0:
                    p1ldisplay.color = Color.white;
                    break;
                case 1:
                    p1ldisplay.color = Color.yellow;
                    break;
                case 2:
                    p1ldisplay.color = Color.red;
                    break;
            case 3:
                p1dead = true;
                break;
        }

            switch (p2head)
            {
                case 0:
                    p2hdisplay.color = Color.white;
                    break;
                case 1:
                    p2hdisplay.color = Color.yellow;
                    break;
                case 2:
                    p2hdisplay.color = Color.red;
                    break;
            case 3:
                p2dead = true;
                break;
        }
            switch (p2body)
            {
                case 0:
                    p2bdisplay.color = Color.white;
                    break;
                case 1:
                    p2bdisplay.color = Color.yellow;
                    break;
                case 2:
                    p2bdisplay.color = Color.red;
                    break;
            case 3:
                p2dead = true;
                break;
        }
            switch (p2legs)
            {
                case 0:
                    p2ldisplay.color = Color.white;
                    break;
                case 1:
                    p2ldisplay.color = Color.yellow;
                    break;
                case 2:
                    p2ldisplay.color = Color.red;
                    break;
            case 3:
                p2dead = true;
                break;
        }

        


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Main");
        }
    }
}
