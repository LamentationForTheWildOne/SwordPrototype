using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{

    public int pos = 0;
    public int scaling;
    public int round;
    public int direction;
    public GameObject ControlGuide;
    public GameObject p1;
    public GameObject p2;
    public GameObject skills;
    public GameObject PauseButton;
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
    public GameObject restartButton;
    public GameObject pauseButton;
    public GameObject fadePanel;
    public GameObject HideUI1;
    public GameObject HideUI2;


    public bool p1follow = false;
    public bool p2follow = false;
    public float followSpeed;

    private Renderer p1render;
    private Renderer p2render;
    private bool PauseButtonClick = false;
    private bool firstOpen = true;
    

    
    

    // Start is called before the first frame update
    void Start()
    {
        
        p1control = p1.GetComponent<PlayerControl>();
        p2control = p2.GetComponent<PlayerControl>();
        p1Abilities = p1.GetComponent<AbilityList>();
        p2Abilities = p2.GetComponent<AbilityList>();
        SoundPlayer1 = p1.GetComponent<AudioSource>();
        SoundPlayer2 = p2.GetComponent<AudioSource>();
        p1render = p1.GetComponent<Renderer>();
        p2render = p2.GetComponent<Renderer>();

        PauseButton.SetActive(false);
        skills.SetActive(true);
        Time.timeScale = 0;
        paused = true;
        PauseButtonClick = false;
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
        if (p1control.attackInProgress || p2control.attackInProgress) {
            Debug.Log("Delayed");
            return; 
        }

        p1control.swordH.GetComponent<SwordMovement>().Return();
        p1control.swordM.GetComponent<SwordMovement>().Return();
        p1control.swordL.GetComponent<SwordMovement>().Return();

        p2control.swordH.GetComponent<SwordMovement>().Return();
        p2control.swordM.GetComponent<SwordMovement>().Return();
        p2control.swordL.GetComponent<SwordMovement>().Return();
        
        if (transform.position.x <= -45)
        {
            p1control.state = Phase.GAMEOVER;
            p2control.state = Phase.GAMEOVER;
            textDisplay.RoundAndTimerDisplay.text = "P2 WINS!";
        }
        else if (transform.position.x >= 45)
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
        if (p1control.state == Phase.GAMEOVER || p2control.state == Phase.GAMEOVER) {
            p1render.sortingOrder = 3;
            p2render.sortingOrder = 3;
            pauseButton.SetActive(false);
            HideUI1.SetActive(false);
            HideUI2.SetActive(false);
            restartButton.SetActive(true);
            fadePanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Main");
        }

        if (Input.GetKeyDown(KeyCode.P) || PauseButtonClick) // pause button active
        {
            if (!paused)
            {
                if(firstOpen)
                {
                    ControlGuide.SetActive(true);
                    StartCoroutine(Countdown());
                    firstOpen = false;
                }
                paused = true;
                Time.timeScale = 0;
                SkillMenu();
                PauseButtonClick = false;
            } else if (paused) 
            {
                if (firstOpen)
                {
                    ControlGuide.SetActive(true);
                    StartCoroutine(Countdown());
                    firstOpen = false;
                }

                StartCoroutine(Unpause());
                paused = false;
                SkillMenu();
                PauseButtonClick = false;
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


    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(5f);
        ControlGuide.SetActive(false);
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void SkillMenu()
    {
        if(paused == true )
        {
           skills.SetActive(true);
           PauseButton.SetActive(false);
        }
        else
        {
            skills.SetActive(false);
            PauseButton.SetActive(true);
        }

    }


    public void PauseTheButton()
    {
        
        if (PauseButtonClick == false)
        {
            PauseButtonClick = true;          
        }
        else
        {
            PauseButtonClick = false;    
        }

    }

    IEnumerator Unpause() 
    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
    }
    public void Restart(){
        SceneManager.LoadScene("Main");
    }
}
