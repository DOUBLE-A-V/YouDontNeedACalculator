using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Logging : MonoBehaviour
{
    public TMP_Text logText;
    public int logsCount = 0;

    public void Log(string text, Color color)
    {
        logsCount++;
        if (logsCount > 18)
        {
            Clear();
            logsCount++;
        }
        logText.text += "<color=#" + color.ToHexString().Remove(5, 2) + ">" + text + "</color>\n";
    }
    public void Clear()
    {
        logsCount = 0;
        logText.text = "";
    }
}
