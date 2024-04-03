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
    public bool tc;
    public bool an;

    // Start is called before the first frame update
    void Start()
    {
        if (fury) {
            oSkill = 0;
            oCool = 3;
        }
       
        if (tc)
        {
            cSkill = 0;
        }

        if (an)
        {
            cSkill = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
