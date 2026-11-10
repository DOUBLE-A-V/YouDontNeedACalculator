using UnityEngine;
using TMPro;

public class InputFieldButton : Button
{
    public TMP_InputField inputField;

    public override void OnReleased()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }
}
