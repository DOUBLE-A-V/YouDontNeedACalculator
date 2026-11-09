using Unity.Netcode;
using UnityEngine;
using TMPro;

public class NetGame : NetworkBehaviour
{
    public NetworkVariable<int> keyPressed = new NetworkVariable<int>(-1);

    public override void OnNetworkSpawn()
    {
        // Subscribe to changes so every machine reacts to the new value
        keyPressed.OnValueChanged += OnKeyPressed;
    }

    public override void OnNetworkDespawn()
    {
        keyPressed.OnValueChanged -= OnKeyPressed;
    }

    public void OnKeyPressed(int prev, int cur)
    {
        if (cur == 0) M.game.text.text = "0";
        if (cur == 1) M.game.text.text = "1";
        if (cur == 2) M.game.text.text = "2";
    }

    public void Update()
    {
        if (!IsHost) return;
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            keyPressed.Value = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            keyPressed.Value = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            keyPressed.Value = 2;
        }
    }
}
