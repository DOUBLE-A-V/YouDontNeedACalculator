using Unity.Netcode;
using UnityEngine;

public class StartClientButton : ElasticButton
{
    public async override void OnButtonPressed()
    {
        await M.game.JoinSessionByCode(M.game.codeInputField.text);
    }
}