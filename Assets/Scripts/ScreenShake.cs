using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float duration;
    public AnimationCurve curve;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartShake() {
    StartCoroutine(Shake());
    }
    IEnumerator Shake() 
    {
    Vector3 startPosition = transform.position;
    float elaspsedTime = 0f;
        while (elaspsedTime < duration) 
        {
            elaspsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elaspsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

    }
}
