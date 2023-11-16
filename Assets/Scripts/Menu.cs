using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject levelComplete;

    public void OnMenuButon()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    
    public void OnQuitButton()
    {
        Application.Quit();
    }
    public void OnLoadNextLevelButton()
    {
        // Find the LevelManager object in the scene
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.ClearLevel();

        // If LevelManager is found, call the LoadLevel method
        if (levelManager != null)
        {
            levelComplete.SetActive(false);
            // Assuming you want to load the next level (currentLevelIndex + 1)
            levelManager.LoadLevel(levelManager.currentLevelIndex + 1);
        }
        else
        {
            Debug.LogError("LevelManager not found in the scene. Make sure it exists and is active.");
        }


    }
}
