using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityList : MonoBehaviour
{
    public int oSkill;
    public int dSkill;
    public int cSkill;
    
    public int oCool;
    public int dCool;
    public bool fury;
    public bool charge;
    public bool imp;
    public bool tower;
    public bool tc;
    public bool an;
    public bool p;

    // Start is called before the first frame update
    void Start()
    {
        if (fury) {
            oSkill = 0;
            oCool = 3;
        } else if (charge){
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

        if (an)
        {
            cSkill = 1;
        }

        if (p) 
        {
            cSkill = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
