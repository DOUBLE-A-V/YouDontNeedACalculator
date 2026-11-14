using UnityEngine;

public class SpawnUnitButton : ElasticButton
{
    public override void OnButtonPressed()
    {
        M.netgame.SpawnUnitRpc("default");
    }
}
