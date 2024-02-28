using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{

    public int spd;
    private Vector3 startPos;
    public int flip;
    // Start is called before the first frame update
    void Start()
    {

        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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

    public void Feint()
    {
        StartCoroutine(FeintTimer());
    }

    IEnumerator AttackTimer() 
    {
        spd = 10;
        yield return new WaitForSeconds(0.3f);
        spd = 0;
        transform.position = startPos;
    }

    IEnumerator FeintTimer()
    {
        spd = 10;
        yield return new WaitForSeconds(0.2f);
        spd = 0;
        transform.position = startPos;
    }
}
