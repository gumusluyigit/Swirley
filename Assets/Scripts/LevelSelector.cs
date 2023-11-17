using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void OnLevelButtonClicked(int index)
    {
        SceneManager.LoadScene(2);
        // Find the LevelManager object in the scene
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        // If LevelManager is found, call the LoadLevel method with the inferred level index
        if (levelManager != null)
        {

            // Get the sibling index of the button (assuming buttons are ordered in the hierarchy)
            int buttonIndex = index - 1;

            // Load the level based on the button index (add 1 to match the level numbering)
            levelManager.LoadLevel(buttonIndex);
        }
        else
        {
            Debug.LogError("LevelManager not found in the scene. Make sure it exists and is active.");
        }
    }
}
