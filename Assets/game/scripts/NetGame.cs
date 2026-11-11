using Unity.Netcode;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Video;
using System.Threading.Tasks;
using System.Collections.Generic;

public class NetGame : NetworkBehaviour
{
    public List<Entity> entities;

    public bool ready = false;
    public NetworkVariable<int> keyPressed = new NetworkVariable<int>(-1);
    public int readyPlayersCount = 0;

    public override void OnNetworkSpawn()
    {
        keyPressed.OnValueChanged += OnKeyPressed;
        PlayerConnectedRpc();
        M.netgame = this;
    }

    public async override void OnNetworkDespawn()
    {
        keyPressed.OnValueChanged -= OnKeyPressed;
        if (IsClient) await M.multiplayerManager.LeaveSession();
    }

    public void OnKeyPressed(int prev, int cur)
    {
        M.multiplayerManager.logger.Log(DateTime.Now.Millisecond + "", Color.white);
        if (cur == 1) M.multiplayerManager.logger.Log("hello", Color.white);
        if (cur == 2) M.multiplayerManager.logger.Log("goodbye", Color.white);   
    }
    public void StartGame()
    {
        StartGameRpc();
    }

    [Rpc(SendTo.Everyone)]
    public async void StartGameRpc()
    {
        
    }
    
    [Rpc(SendTo.Server)]
    public void SetKeyPressedRpc(int newValue)
    {
        keyPressed.Value = newValue;
    }
    [Rpc(SendTo.Server)]
    public void PlayerConnectedRpc()
    {
        M.multiplayerManager.UpdateRoomText();
    }
    [Rpc(SendTo.Server)]
    public void ReadyPlayerRpc()
    {
        readyPlayersCount++;
        if (M.multiplayerManager.currentSession.PlayerCount == readyPlayersCount)
        {
            StartGame();
        }
    }
    [Rpc(SendTo.Server)]
    public void UnreadyPlayerRpc()
    {
        readyPlayersCount--;
    }

    public void ReadyPressed()
    {
        ReadyPlayerRpc();
    }
    
    public void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.E)) SetKeyPressedRpc(1);
        if (Input.GetKeyDown(KeyCode.Q)) SetKeyPressedRpc(2);
    }
}
