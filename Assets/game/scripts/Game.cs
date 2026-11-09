using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public List<Button> buttons;
    public bool paused = false;
    public bool buttonsActive = true;

    public void StartGame()
    {
        
    }

    public void UpdateButtons()
    {
        foreach (Button button in buttons)
        {
            if (button.active) button.UpdateButton();
        }
    }
    public void Update()
    {
        if (paused) return;
        if (buttonsActive)UpdateButtons();
    }
}
