using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUI : MonoBehaviour {

    public void NewGame()
    {
        //PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteKey("gold");
        PlayerPrefs.DeleteKey("lv");
        PlayerPrefs.DeleteKey("exp");
        PlayerPrefs.DeleteKey("scd");
        PlayerPrefs.DeleteKey("bcd");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OldGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
