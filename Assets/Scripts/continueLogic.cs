using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class continueLogic : MonoBehaviour
{ 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene("SampleScene");
            Debug.Log("Changing scene");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("SampleScene"));
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
