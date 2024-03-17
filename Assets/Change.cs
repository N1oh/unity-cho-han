using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("Main");
    }

    public void SceneChange1()
    {
        SceneManager.LoadScene("Scene");
    }

    public void SceneChange2()
    {
        SceneManager.LoadScene("Option");
    }

    public void SceneChange3()
    {
        SceneManager.LoadScene("Talk");
    }


    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
