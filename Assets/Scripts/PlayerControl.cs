using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
public enum Phase { ATTACKING, DEFENDING }

public class PlayerControl : MonoBehaviour
{
    public GameObject opponent;
    public GameManager gameManager;

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

    // Variable holding which sword/shield the player has raised
    public GameObject active;

    // Variable regulating phases
    public Phase state;

    // Variable setting height
    public int height;

    // Bool to prevent player from changing height while attacking/defending
    public bool acting;

    public bool first;

    // Variable referencing the Text Display
    public TextDisplay TheTextDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Sets middle sword as active by default for player one and middle shield for player 2
        if (first)
        {
            state = Phase.ATTACKING;
            active = swordM;
        }
        else
        {
            state = Phase.DEFENDING;
            active = shieldM;
        }
        active.SetActive(true);
        height = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!acting)
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
    }

    void HandleMovementInput()
    {
        // Adjust behavior for continuous input during DEFENDING phase
        if (state == Phase.DEFENDING)
        {
            if (Input.GetKey(Up) | Input.GetAxisRaw(playerAxes) > 0)
            {
                SetShieldPosition(2); // High position
            }
            else if (Input.GetKey(Down) | Input.GetAxisRaw(playerAxes) < 0)
            {
                SetShieldPosition(0); // Low position
            }
            else
            {
                ResetShieldPosition(); // Neutral/middle position
            }
        }
        else // ATTACKING phase retains original one-time press behavior
        {
            if (Input.GetKey(Up) | Input.GetAxisRaw(playerAxes) > 0)
            {
                SetSwordPosition(2); // High position
            }
            else if (Input.GetKey(Down) | Input.GetAxisRaw(playerAxes) < 0)
            {
                SetSwordPosition(0); // Low position
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

    public void Switch()
    {
        active.SetActive(false);
        if (state == Phase.DEFENDING)
        {
            state = Phase.ATTACKING;
            active = swordM;
            active.SetActive(true);
            height = 1;
            acting = false;
        }
        else if (state == Phase.ATTACKING)
        {
            state = Phase.DEFENDING;
            active = shieldM;
            active.SetActive(true);
            height = 1;
            acting = false;
        }
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
        active.GetComponent<SwordMovement>().Thrust();

        yield return new WaitForSeconds(0.3f); 
        

        if (opponent.GetComponent<PlayerControl>().height == height && opponent.GetComponent<PlayerControl>().acting)
        {
            Debug.Log("block");
            TheTextDisplay.StrikeBlocked(first);
            StartCoroutine(Hitstun());
        }
        else
        {
            Debug.Log("hit");
            TheTextDisplay.StrikeLanded(first);
            StartCoroutine(Hitstun());
            StartCoroutine(Repo());

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
        yield return new WaitForSeconds(0.4f);
        active.GetComponent<SwordMovement>().Return();
        Switch();
        opponent.GetComponent<PlayerControl>().Switch();
    }

    IEnumerator Repo() {
        yield return new WaitForSeconds(0.6f);
        gameManager.Reposition(playerCount);
    }


}
