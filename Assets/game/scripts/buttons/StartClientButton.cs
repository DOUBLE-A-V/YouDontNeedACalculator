using Unity.Netcode;
using UnityEngine;

public class StartClientButton : ElasticButton
{
    public override void OnButtonPressed()
    {
        M.game.serverNM.OnClientStarted += () =>
        {
            Debug.Log("dildo client started???");
        };
        M.game.serverNM.StartClient();
    }
}