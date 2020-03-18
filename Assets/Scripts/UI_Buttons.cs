using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Buttons : MonoBehaviour
{
    public event Action<int> ButtonInput = delegate { };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitButton();
        }
    }

    public void ExitButton()
    {
        Application.Quit();
        ButtonInput(0);
    }

    public void RestartGame()
    {
        ButtonInput(1);
    }
}
