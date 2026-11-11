using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public List<Button> lockButtons = new List<Button>();
    public List<Button> buttons;

    public void UpdateButtons()
    {
        foreach (Button button in new List<Button>(buttons))
        {
            if (button.active && !lockButtons.Contains(button)) button.UpdateButton();
        }
    }

    public void Update()
    {
        UpdateButtons();
    }
}
