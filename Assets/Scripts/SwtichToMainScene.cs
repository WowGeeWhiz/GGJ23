using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwtichToMainScene : MonoBehaviour
{
    public Texture2D normalCursor;

    SceneManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    public void playGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void playCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void QuitGame()
    {
        EditorApplication.ExitPlaymode();
        Application.Quit();
    }
}