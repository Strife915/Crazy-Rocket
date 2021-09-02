using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text HighScore;
    public rocket m_someOtherScriptOnAnotherGameObject;
    public void Start()
    {
        HighScoreTextReload();
    }

    public void HighScoreTextReload()
    {
        HighScore = GameObject.Find("High Score").GetComponent<Text>();
        HighScore.text = "High Score\n"+ rocket.HighScore.ToString();
    }
    public void StartButton()
    {
        Invoke("LoadScene1",2);
    }

    public void LoadScene1()
    {
        m_someOtherScriptOnAnotherGameObject.ApplyThrust();
        SceneManager.LoadScene("Level 1");
    }
    public void ExitButton()
    {
        Application.Quit();      
    }
    
    public void ResetButton()
    {
        PlayerPrefs.DeleteAll();

    }
}
