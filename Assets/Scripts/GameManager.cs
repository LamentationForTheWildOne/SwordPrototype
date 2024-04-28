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
    public GameObject skills;
    public PlayerControl p1control;
    public PlayerControl p2control;
    public TextDisplay textDisplay;
    public Image p1hdisplay;
    public Image p1bdisplay;
    public Image p1ldisplay;
    public Image p2hdisplay;
    public Image p2bdisplay;
    public Image p2ldisplay;

    public List<Sprite> HeadSprites;
    public List<Sprite> ChestSprites;
    public List<Sprite> TorsoSprites;

    public AudioSource SoundPlayer1;
    public AudioSource SoundPlayer2;
    public List<AudioClip> SFX;
    //SFX Guide:
    //(Note: Audio clips not final; require editing)
    //SFX[0] = Attack
    //SFX[1] = Feint
    //SFX[2] = Hit
    //SFX[3] = Block
    //SFX[4] = Round Start
    //SFX[5] = Round Timer
    //SFX[6] = Time Out (Not implemented)
    //SFX[7] = Fatal Hit (Not implemented)
    //SFX[8] = Ring Out (Not implemented)



    public int MaxRoundTime;
    public int NewRoundTime;

    public bool paused;

    public int p1head;
    public int p1body;
    public int p1legs;
    public Vector2 startPos;

    public int p2head;
    public int p2body;
    public int p2legs;

    public bool p1dead = false;
    public bool p2dead = false;
    private AbilityList p1Abilities;
    private AbilityList p2Abilities;


    public bool p1follow = false;
    public bool p2follow = false;
    public float followSpeed;



    // Start is called before the first frame update
    void Start()
    {
        
        p1control = p1.GetComponent<PlayerControl>();
        p2control = p2.GetComponent<PlayerControl>();
        p1Abilities = p1.GetComponent<AbilityList>();
        p2Abilities = p2.GetComponent<AbilityList>();
        SoundPlayer1 = p1.GetComponent<AudioSource>();
        SoundPlayer2 = p2.GetComponent<AudioSource>();

        //SoundPlayer1.clip = SFX[0];
        //SoundPlayer1.Play();
    }

    public void Reposition(int playernum, int damage) 
    {
        if (p1follow)
        {
            transform.position = startPos;
            p1follow = false;
        }
        if (p2follow)
        {
            transform.position = startPos;
            p2follow = false;
        }  
        if (playernum == 1)
        {
            direction = 1;
        }
        else 
        {
            direction = -1;
        }
        pos = ((damage * scaling) * direction);
        transform.position = new Vector2(transform.position.x + pos, transform.position.y);
        p1.transform.position = new Vector2(transform.position.x - 2.5f, p1.transform.position.y);
        p2.transform.position = new Vector2(transform.position.x + 2.5f, p2.transform.position.y);
    }

    public void VoidShake() {
        Camera.main.GetComponent<ScreenShake>().StartShake();
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

                textDisplay.P1Off.text = p1Abilities.offensive +": " + p1control.offcool;
                textDisplay.P2Off.text = p2Abilities.offensive +": " + p2control.offcool;
                textDisplay.P1Def.text = p1Abilities.defensive +": "+ p1control.defcool;
                textDisplay.P2Def.text = p2Abilities.defensive +": " + p2control.defcool;

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
                p1hdisplay.sprite = HeadSprites[0];
                break;
            case 1:
                p1hdisplay.sprite = HeadSprites[1];
                break;
            case 2:
                p1hdisplay.sprite = HeadSprites[2];
                break;
            case 3:
                p1dead = true;
                p1hdisplay.sprite = HeadSprites[3];
                break;
        }
        switch (p1body)
        {
            case 0:
                p1bdisplay.sprite = ChestSprites[0];
                break;
            case 1:
                p1bdisplay.sprite = ChestSprites[1];
                break;
            case 2:
                p1bdisplay.sprite = ChestSprites[2];
                break;
            case 3:
                p1dead = true;
                p1bdisplay.sprite = ChestSprites[3];
                break;
        }
        switch (p1legs)
        {
            case 0:
                p1ldisplay.sprite = TorsoSprites[0];
                break;
            case 1:
                p1ldisplay.sprite = TorsoSprites[1];
                break;
            case 2:
                p1ldisplay.sprite = TorsoSprites[2];
                break;
            case 3:
                p1dead = true;
                p1ldisplay.sprite = TorsoSprites[3];
                break;
        }

        switch (p2head)
        {
            case 0:
                p2hdisplay.sprite = HeadSprites[0];
                break;
            case 1:
                p2hdisplay.sprite = HeadSprites[1];
                break;
            case 2:
                p2hdisplay.sprite = HeadSprites[2];
                break;
            case 3:
                p2dead = true;
                p2hdisplay.sprite = HeadSprites[3];
                break;
        }
        switch (p2body)
        {
            case 0:
                p2bdisplay.sprite = ChestSprites[0];
                break;
            case 1:
                p2bdisplay.sprite = ChestSprites[1];
                break;
            case 2:
                p2bdisplay.sprite = ChestSprites[2];
                break;
            case 3:
                p2dead = true;
                p2bdisplay.sprite = ChestSprites[3];
                break;
        }
        switch (p2legs)
        {
            case 0:
                p2ldisplay.sprite = TorsoSprites[0];
                break;
            case 1:
                p2ldisplay.sprite = TorsoSprites[1];
                break;
            case 2:
                p2ldisplay.sprite = TorsoSprites[2];
                break;
            case 3:
                p2dead = true;
                p2ldisplay.sprite = TorsoSprites[3];
                break;
        }

        


    }

    public void Cache() {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Main");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0;
                SkillMenu();
            } else if (paused) 
            {
                StartCoroutine(Unpause());
                paused = false;
                SkillMenu();
            }
        }

        Vector2 targetPosition = transform.position;
        if (p1follow)
        {
            targetPosition = new Vector2(p1control.transform.position.x, p1control.transform.position.y);
        }
        else if (p2follow)
        {
            targetPosition = new Vector2(p2control.transform.position.x, p2control.transform.position.y);
        }
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        
    }
    public void SkillMenu()
    {
        if(paused == true )
        {
           skills.SetActive(true);
        }
        else
        {
            skills.SetActive(false);
        }

    }

    IEnumerator Unpause() 
    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
    }
}
