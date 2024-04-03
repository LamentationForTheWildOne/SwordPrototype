using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{

    public int spd;
    public GameObject Player;
    public int flip;
    public Vector3 startPos;
    // Start is called before the first frame update


    private void Awake()
    {
        startPos = transform.position;
        gameObject.SetActive(false);
    }
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = transform.position + new Vector3(flip * spd * Time.deltaTime,0);
    }

    public void Thrust() 
    {
        StartCoroutine(AttackTimer());    
    }

    public void TCThrust() 
    {
        StartCoroutine (TCAttackTimer());
    }
    public void ANThrust()
    {
        StartCoroutine(ANAttackTimer());
    }

    public void Feint()
    {
        StartCoroutine(FeintTimer());
    }

    public void Return() 
    {
        transform.position = new Vector3(Player.transform.position.x + (1.8f * flip), startPos.y);
    }

    IEnumerator AttackTimer() 
    {
        spd = 10;
        yield return new WaitForSeconds(0.3f);
        spd = 0;
    }

    IEnumerator TCAttackTimer()
    {
        spd = 13;
        yield return new WaitForSeconds(0.2f);
        spd = 0;
    }

    IEnumerator ANAttackTimer()
    {
        spd = 7;
        yield return new WaitForSeconds(0.4f);
        spd = 0;
    }

    IEnumerator FeintTimer()
    {
        spd = 10;
        yield return new WaitForSeconds(0.2f);
        spd = 0;
        Return();
    }
}
