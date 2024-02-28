using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Phase { ATTACKING, DEFENDING }

public class PlayerControl : MonoBehaviour
{
    public GameObject opponent;

    // Game objects for various sword/shield heights
    public GameObject swordH;
    public GameObject swordM;
    public GameObject swordL;
    public GameObject shieldH;
    public GameObject shieldM;
    public GameObject shieldL;

    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Action;
    public KeyCode Feint;

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
            if (Input.GetKeyDown(Action))
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

            if (Input.GetKeyDown(Feint) && state == Phase.ATTACKING)
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
            if (Input.GetKey(Up))
            {
                SetShieldPosition(2); // High position
            }
            else if (Input.GetKey(Down))
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
            if (Input.GetKeyDown(Up))
            {
                ShiftUp();
            }
            if (Input.GetKeyDown(Down))
            {
                ShiftDown();
            }
        }
    }
    //deactivates the current active sword / shield and then activates the one above it
    void ShiftUp()
    {
        if (height == 1)
        {
            height = 2;
            active.SetActive(false);
            if (state == Phase.ATTACKING)
            {
                active = swordH;
            }
            else if (state == Phase.DEFENDING)
            {
                active = shieldH;
            }
            active.SetActive(true);
        }
        else if (height == 0)
        {
            height = 1;
            active.SetActive(false);
            if (state == Phase.ATTACKING)
            {
                active = swordM;
            }
            else if (state == Phase.DEFENDING)
            {
                active = shieldM;
            }
            active.SetActive(true);
        }

    }

    // same as shift up except it does it in the opposite direction
    void ShiftDown()
    {
        if (height == 2)
        {
            height = 1;
            active.SetActive(false);
            if (state == Phase.ATTACKING)
            {
                active = swordM;
            }
            else if (state == Phase.DEFENDING)
            {
                active = shieldM;
            }
            active.SetActive(true);
        }
        else if (height == 1)
        {
            height = 0;
            active.SetActive(false);
            if (state == Phase.ATTACKING)
            {
                active = swordL;
            }
            else if (state == Phase.DEFENDING)
            {
                active = shieldL;
            }
            active.SetActive(true);
        }

    }
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


    public void Switch()
    {
        active.SetActive(false);
        if (state == Phase.DEFENDING)
        {
            state = Phase.ATTACKING;
            active = swordM;
            active.SetActive(true);
            height = 1;
        }
        else if (state == Phase.ATTACKING)
        {
            state = Phase.DEFENDING;
            active = shieldM;
            active.SetActive(true);
            height = 1;
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
        MeshRenderer meshRenderer = active.GetComponent<MeshRenderer>();
        Color originalColor = meshRenderer.material.color;
        meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.3f); 
        

        if (opponent.GetComponent<PlayerControl>().height == height && opponent.GetComponent<PlayerControl>().acting)
        {
            Switch();
            opponent.GetComponent<PlayerControl>().Switch();
            Debug.Log("block");
            TheTextDisplay.StrikeBlocked(first);
        }
        else
        {
            Switch();
            opponent.GetComponent<PlayerControl>().Switch();
            Debug.Log("hit");
            TheTextDisplay.StrikeLanded(first);
        }

       
        yield return new WaitForSeconds(0.1f); 

        meshRenderer.material.color = originalColor;

        acting = false;
    }

    //use a feint
    IEnumerator FeintCount()
    {
        MeshRenderer meshRenderer = active.GetComponent<MeshRenderer>();
        Color originalColor = meshRenderer.material.color;
        meshRenderer.material.color = Color.yellow;
        Debug.Log("feint");

        yield return new WaitForSeconds(0.2f);

        meshRenderer.material.color = originalColor;

        acting = false;
    }


}
