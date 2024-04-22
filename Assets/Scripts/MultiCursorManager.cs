using UnityEngine;

public class MultiCursorManager : MonoBehaviour
{
    public GameObject cursorPrefab;
    private int maxControllers = 2; 

    void Start()
    {
        
        for (int i = 1; i <= maxControllers; i++)
        {
            Debug.Log(IsControllerActive(i));
            if (IsControllerActive(i))
            {
                GameObject cursor = Instantiate(cursorPrefab, transform);
                cursor.GetComponent<CursorController>().controllerId = i;
            }
        }
    }

    private bool IsControllerActive(int controllerId)
    {
        return Input.GetJoystickNames().Length >= controllerId && !string.IsNullOrEmpty(Input.GetJoystickNames()[controllerId - 1]);
    }
}
