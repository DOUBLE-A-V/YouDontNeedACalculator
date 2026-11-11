using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class StartServerButton : ElasticButton
{
    public async override void OnButtonPressed()
    {
        await M.multiplayerManager.StartSessionAsHost();
    }
}