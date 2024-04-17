using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityList : MonoBehaviour
{
    public int oSkill, dSkill, cSkill;
    public int oCool, dCool;
    public bool fury, charge, imp, tower, tc, an;

    public Button furyButton, chargeButton, impButton, towerButton, tcButton, anButton;

    void Start()
    {
        UpdateSkills();  
    }

    public void ActivateFury()
    {
        SetAbility(ref fury, true, furyButton);
        SetAbility(ref charge, false, chargeButton);
    }

    public void ActivateCharge()
    {
        SetAbility(ref charge, true, chargeButton);
        SetAbility(ref fury, false, furyButton);
    }

    public void ActivateImp()
    {
        SetAbility(ref imp, true, impButton);
        SetAbility(ref tower, false, towerButton);
    }

    public void ActivateTower()
    {
        SetAbility(ref tower, true, towerButton);
        SetAbility(ref imp, false, impButton);
    }

    public void ActivateTc()
    {
        SetAbility(ref tc, true, tcButton);
        SetAbility(ref an, false, anButton);
    }

    public void ActivateAn()
    {
        SetAbility(ref an, true, anButton);
        SetAbility(ref tc, false, tcButton);
    }

   
    private void SetAbility(ref bool ability, bool state, Button button)
    {
        ability = state; 
        UpdateButtonColor(button, state);  
        UpdateSkills();  
    }

   
    void UpdateButtonColor(Button button, bool isActive)
    {
        button.GetComponent<Image>().color = isActive ? Color.red : Color.white;
    }


    void UpdateSkills()
    {
        oSkill = dSkill = cSkill = -1;  //resetting the cool down for testing.
        oCool = dCool = 0;

        if (fury)
        {
            oSkill = 0;
            oCool = 3;
        }
        else if (charge)
        {
            oSkill = 1;
            oCool = 3;
        }

        if (imp)
        {
            dSkill = 0;
            dCool = 3;
        }
        else if (tower)
        {
            dSkill = 1;
            dCool = 3;
        }

        if (tc)
        {
            cSkill = 0;
        }
        else if (an)
        {
            cSkill = 1;
        }
    }

    void Update()
    {
        
    }
}
