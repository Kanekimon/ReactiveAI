using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Exit_view : UiView
{

    VisualElement exitContainer;
    Button resume;
    Button exit;

    private void Start()
    {
        exitContainer = Root.Q<VisualElement>("Exit_view").Q<VisualElement>("button-container");
        resume = exitContainer.Q<Button>("resume-game");
        exit = exitContainer.Q<Button>("exit-game");

        resume.clicked += ResumeGame;
        exit.clicked += ExitGame;
    }


    void ResumeGame()
    {
        UIManager.Instance.CloseOpenView();
    }

    void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


}

