using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
public enum Phase { ATTACKING, PREROUND, DEFENDING, GAMEOVER }

public class PlayerControl : MonoBehaviour
{
    float counter;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 midPos;
    public int direction;

    public Animator animator;

    public GameObject opponent;
    public GameManager gameManager;
    public AbilityList abilityList;

    public int offcool;
    public int defcool;

    // Game objects for various sword/shield heights
    public GameObject swordH;
    public GameObject swordM;
    public GameObject swordL;
    public GameObject shieldH;
    public GameObject shieldM;
    public GameObject shieldL;

    public KeyCode Up;
    public KeyCode Down;
    public Button ActionB;
    public Button FeintB;
    public KeyCode Action;
    public KeyCode Feint;

    public string playerAxes;
    public int playerCount;

    //Bools to hold buffs
    public bool fury = false;
    public bool tc = false;
    public bool an = false;
    int baseDamage = 10;

    // Variable holding which sword/shield the player has raised
    public GameObject active;

    // Variable regulating phases
    public Phase state;

    // Variable setting height
    public int height;
    int hitHeight;

    // Bool to prevent player from changing height while attacking/defending
    public bool acting;

    public bool first;

    // Variable referencing the Text Display
    public TextDisplay TheTextDisplay;

    private Phase moveto;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 

        counter = 1.0f;
        startPos = transform.position;
        endPos = transform.position;
        midPos = startPos + (endPos -startPos)/2 +Vector3.up * 5.0f;

        // Sets middle sword as active by default for player one and middle shield for player 2
        if (first)
        {
            state = Phase.ATTACKING;
            active = swordM;
            animator.SetInteger("SwordHeight", 0);
            animator.SetBool("Attacking", true);
        }
        else
        {
            state = Phase.DEFENDING;
            active = shieldM;
            animator.SetInteger("BlockHeight", 0);
            animator.SetBool("Attacking", false);
        }
        active.SetActive(true);
        height = 1;

        StartCoroutine(Fill());
    }

    public void Ability() 
    {
        switch (abilityList.cSkill)
        {

            case 0:
                tc = true;
                baseDamage = 8;
                Debug.Log("Thousand Cuts");
                break;
            case 1:
                an = true;
                baseDamage = 12;
                Debug.Log("All or Nothing");
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!acting && state != Phase.PREROUND)
        {
            HandleMovementInput();

            // Action and Feint logic
            if (Input.GetButtonDown("Action" + playerCount) | Input.GetKeyDown(Action))
            {
                acting = true;
                if (state == Phase.ATTACKING)
                {
                    StartCoroutine(SwordCount());
                }
                else if (state == Phase.DEFENDING)
                {
                    StartCoroutine(ShieldCount());
                }
            }

            if (Input.GetButtonDown("Feint" + playerCount) | Input.GetKeyDown(Feint) && state == Phase.ATTACKING)
            {
                acting = true;
                StartCoroutine(FeintCount());
            }
        }
        if (state == Phase.PREROUND) {
            if (moveto == Phase.ATTACKING)
            {
                if ((Input.GetButtonDown("Action" + playerCount) | Input.GetKeyDown(Action)) && offcool == 0) 
                {
                    offcool = abilityList.oCool;
                    switch (abilityList.oSkill) {

                        case 0:
                            fury = true;
                            Debug.Log("Fury");
                            break;
                    
                    }
                
                }
            }
            else if (moveto == Phase.DEFENDING) 
            {
                if ((Input.GetButtonDown("Action" + playerCount) | Input.GetKeyDown(Action)) && defcool == 0)
                {
                    defcool = abilityList.dCool;
                    switch (abilityList.dSkill)
                    {

                        case 0:
                            gameManager.Impatient();
                            break;

                    }
                }

            }
        
        }

        if (counter < 1.0f) {
            counter += 1.0f * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp(startPos, midPos, counter);
            Vector3 m2 = Vector3.Lerp(midPos, endPos, counter);
            transform.position = Vector3.Lerp(m1, m2, counter);
        }
    }

    void HandleMovementInput()
    {
        // Adjust behavior for continuous input during DEFENDING phase
        if (state == Phase.DEFENDING)
        {
            if (Input.GetKey(Up) | Input.GetAxisRaw(playerAxes) > 0)
            {
                SetShieldPosition(2); // High position
                animator.SetInteger("BlockHeight", -1);
            }
            else if (Input.GetKey(Down) | Input.GetAxisRaw(playerAxes) < 0)
            {
                SetShieldPosition(0); // Low position
                animator.SetInteger("BlockHeight", 1);
            }
            else
            {
                ResetShieldPosition(); // Neutral/middle position
                
            }
        }
        else if (state == Phase.ATTACKING) // ATTACKING phase retains original one-time press behavior
        {
            if (Input.GetKey(Up) | Input.GetAxisRaw(playerAxes) > 0)
            {
                SetSwordPosition(2); // High position
                animator.SetInteger("SwordHeight", -1);
            }
            else if (Input.GetKey(Down) | Input.GetAxisRaw(playerAxes) < 0)
            {
                SetSwordPosition(0); // Low position
                animator.SetInteger("SwordHeight", 1);
            }
            else
            {
                ResetSwordPosition(); // Neutral/middle position
                
            }
        }
    }
    //deactivates the current active sword / shield and then activates the one above it
    void ResetShieldPosition()
    {
        if (height != 1)
        {
            height = 1;
            active.SetActive(false);
            active = shieldM;
            active.SetActive(true);
            animator.SetInteger("BlockHeight", 0);
        }
    }

    void SetShieldPosition(int newHeight)
    {
        if (height != newHeight)
        {
            active.SetActive(false);
            height = newHeight;
            active = (newHeight == 2) ? shieldH : (newHeight == 0) ? shieldL : shieldM;
            active.SetActive(true);
        }
    }

    void ResetSwordPosition()
    {
        if (height != 1)
        {
            height = 1;
            active.SetActive(false);
            active = swordM;
            active.SetActive(true);
            animator.SetInteger("SwordHeight", 0);
        }
    }

    void SetSwordPosition(int newHeight)
    {
        if (height != newHeight)
        {
            active.SetActive(false);
            height = newHeight;
            active = (newHeight == 2) ? swordH : (newHeight == 0) ? swordL : swordM;
            active.SetActive(true);
        }
    }

    public void StartPreround(Phase next) 
    {
        StartCoroutine(Preround(next));
    }

    public void Switch()
    {
        active.SetActive(false);
        if (state == Phase.DEFENDING)
        {
            if (offcool != 0) 
            {
                offcool -= 1; 
            }
            active = swordM;
            active.SetActive(true);
            animator.SetBool("Attacking", true);
            animator.SetInteger("SwordHeight", 0);
            height = 1;
            acting = false;
        }
        else if (state == Phase.ATTACKING)
        {
            fury = false;


            if (defcool != 0)
            {
                defcool -= 1;
            }
            active = shieldM;
            active.SetActive(true);
            animator.SetBool("Attacking", false);
            animator.SetInteger("BlockHeight", 0);
            height = 1;
            acting = false;
        }
    }

    public void Knockback() 
    {
        counter = 0.0f;
        startPos = transform.position;
        endPos = new Vector2(gameManager.transform.position.x + ((opponent.GetComponent<PlayerControl>().baseDamage * gameManager.scaling) * direction), gameManager.transform.position.y - 0.4f);
        midPos = startPos + (endPos - startPos) / 2 + Vector3.up * 5.0f;
    }


    // gives the guard a recovery time
    IEnumerator ShieldCount() {
        MeshRenderer meshRenderer = active.GetComponent<MeshRenderer>();
        Color originalColor = meshRenderer.material.color;
        meshRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        meshRenderer.material.color = originalColor;

        acting = false;
    }

    //give the attack some start up and then checks to see if the opponent is blocking correctly
    IEnumerator SwordCount()
    {
        // MeshRenderer meshRenderer = active.GetComponent<MeshRenderer>();
        //Color originalColor = meshRenderer.material.color;
        // meshRenderer.material.color = Color.red;
        if (!an && !tc)
        {
            active.GetComponent<SwordMovement>().Thrust();
            yield return new WaitForSeconds(0.3f);
        }
        if (an) 
        {
            active.GetComponent<SwordMovement>().ANThrust();
            yield return new WaitForSeconds(0.4f);
        }
        if (tc)
        {
            active.GetComponent<SwordMovement>().TCThrust();
            yield return new WaitForSeconds(0.2f);
        }

        if (opponent.GetComponent<PlayerControl>().height == height && opponent.GetComponent<PlayerControl>().acting)
        {
            Debug.Log("block");
            if (!fury)
            {
                TheTextDisplay.StrikeBlocked(first);
                StartCoroutine(Blockstun());
            }
            else 
            {
                StartCoroutine(Furystun());
            }
        }
        else
        {
            Debug.Log("hit");
            TheTextDisplay.StrikeLanded(first);
            StartCoroutine(Hitstun());
            StartCoroutine(Repo());
            opponent.GetComponent<PlayerControl>().Knockback();
            switch (height) 
            {
                case 0:
                    hitHeight = 3;
                    break;
                case 1:
                    hitHeight = 2;
                    break;
                case 2:
                    hitHeight = 1;
                    break;
            }


        }

        yield return new WaitForSeconds(0.1f); 

        //meshRenderer.material.color = originalColor;
    }

    //use a feint
    IEnumerator FeintCount()
    {
        //MeshRenderer meshRenderer = active.GetComponent<MeshRenderer>();
        //Color originalColor = meshRenderer.material.color;
        //meshRenderer.material.color = Color.yellow;
        Debug.Log("feint");
        active.GetComponent<SwordMovement>().Feint();

        yield return new WaitForSeconds(0.2f);

        //meshRenderer.material.color = originalColor;

        acting = false;
    }

    IEnumerator Hitstun() {
        opponent.GetComponent<PlayerControl>().acting = true;
        yield return new WaitForSeconds(0.2f);
        opponent.GetComponent<PlayerControl>().acting = true;
        yield return new WaitForSeconds(0.3f);
        active.GetComponent<SwordMovement>().Return();
        yield return new WaitForSeconds(1.5f);
        Switch();
        opponent.GetComponent<PlayerControl>().Switch();
        gameManager.NewRound();
    }

    IEnumerator Blockstun()
    {
        opponent.GetComponent<PlayerControl>().acting = true;
        yield return new WaitForSeconds(0.2f);
        opponent.GetComponent<PlayerControl>().acting = true;
        yield return new WaitForSeconds(0.3f);
        active.GetComponent<SwordMovement>().Return();
        yield return new WaitForSeconds(0.1f);
        Switch();
        opponent.GetComponent<PlayerControl>().Switch();
        gameManager.NewRound();
    }

    IEnumerator Furystun()
    {
        opponent.GetComponent<PlayerControl>().acting = true;
        yield return new WaitForSeconds(0.2f);
        opponent.GetComponent<PlayerControl>().acting = true;
        yield return new WaitForSeconds(0.3f);
        active.GetComponent<SwordMovement>().Return();
        acting = false;
        opponent.GetComponent<PlayerControl>().acting = false;

    }

    IEnumerator Repo() {
        yield return new WaitForSeconds(2.0f);
        gameManager.Reposition(playerCount, baseDamage);
        gameManager.PlayerHit(playerCount, hitHeight);
    }

    IEnumerator Preround(Phase next) {
        moveto = next;
        yield return new WaitForSeconds(2);
        state = next;
    }

    IEnumerator Fill() 
    {
        yield return new WaitForSeconds(0.1f);
        Ability();
    }


}
