using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagerScript : MonoBehaviour
{
    public Animator fadeAnimator; 
    public float transitionTime = 1f; 
     public static SceneManagerScript Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
          
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != this)
        {

            Destroy(gameObject);
        }
    }

    public void LoadNextScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        fadeAnimator.SetTrigger("Fadeout");
        
        yield return new WaitForSeconds(transitionTime);
        
       
        SceneManager.LoadScene(sceneName);
        
       
        fadeAnimator.SetTrigger("Fadein");
        yield return new WaitForSeconds(transitionTime);
        
    }
    
}
