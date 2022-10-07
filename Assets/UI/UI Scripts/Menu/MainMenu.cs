using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    VisualElement root;
    Button start;
    Button exit;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        start = root.Q<VisualElement>("container").Q<VisualElement>("button-container").Q<Button>("start-game");
        exit = root.Q<VisualElement>("container").Q<VisualElement>("button-container").Q<Button>("exit-game");

        start.clicked += StartGame;
        exit.clicked += ExitGame;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}

