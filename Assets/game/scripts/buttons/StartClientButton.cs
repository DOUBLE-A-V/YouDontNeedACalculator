using Unity.Netcode;
using UnityEngine;

public class StartClientButton : ElasticButton
{
    public async override void OnButtonPressed()
    {
        await M.multiplayerManager.JoinSessionByCode(M.multiplayerManager.codeInputField.text);
    }
}