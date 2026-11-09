using Unity.Netcode;
using UnityEngine;

public class StartServerButton : ElasticButton
{
    public override void OnButtonPressed()
    {
        M.game.serverNM.OnServerStarted += () =>
        {
            Debug.Log("dildo host connected???");
        };
        M.game.serverNM.StartHost();
    }
}