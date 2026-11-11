using UnityEngine;

public class MultiplayerPlayButton : ElasticButton
{
    public override void OnButtonPressed()
    {
        M.netgame.ReadyPressed();
    }
}
