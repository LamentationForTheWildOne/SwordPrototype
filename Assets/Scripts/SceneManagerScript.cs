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
            // This instance becomes the single global instance
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction on load
        }
        else if (Instance != this)
        {
            // Destroy this instance because another instance already exists
            Destroy(gameObject);
        }
    }
    // Call this method for OnClick event
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
        fadeAnimator.SetTrigger("Fade");
        
        yield return new WaitForSeconds(transitionTime);
        
        // Load the next scene
        SceneManager.LoadScene(sceneName);
        
        // Optionally, fade in from the next scene (this requires the fade panel to be persistent across scenes or recreated in the next scene)
        fadeAnimator.SetTrigger("Fadein");
        yield return new WaitForSeconds(transitionTime);
        
    }
    
}
