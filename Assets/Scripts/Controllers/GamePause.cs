using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public static GamePause _GPInstance;
    UIController uiController;

    public bool gameIsPaused = false;

    private void Awake()
    {
        if(_GPInstance != null && _GPInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _GPInstance = this;
        }
    }

    private void Start() {
        uiController = UIController._UICInstance;
        Resume();
    }

    public void Resume()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        for(int i = 0; i < uiController.uiGameObjects.Length; i++)
        {
            uiController.uiGameObjects[i].SetActive(true);
        }
    }

    public void Pause()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        if(uiController != null)
        {
            for(int i = 0; i < uiController.uiGameObjects.Length; i++)
            {
                uiController.uiGameObjects[i].SetActive(false);
            }
        }
        
    }
}
