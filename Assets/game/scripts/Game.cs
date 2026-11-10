using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Multiplayer;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public List<Button> buttons;
    public bool paused = false;
    public bool buttonsActive = true;

    public TMP_Text connectingText;
    public TMP_InputField codeInputField;
    public TMP_Text infoText;

    public bool waitingForPlayer = false;

    public async Task StartSessionAsHost()
    {
        Debug.Log("Starting host...");
        var options = new SessionOptions
        {
            MaxPlayers = 2
        }.WithRelayNetwork(); // or WithDistributedAuthorityNetwork() to use Distributed Authority instead of Relay
        var session = await MultiplayerService.Instance.CreateSessionAsync(options);
        Debug.Log($"Session {session.Id} created! Join code: {session.Code}");

        StartWaitingForPlayer();
    }

    public void StartWaitingForPlayer()
    {
        waitingForPlayer = true;

    }

    public async Task JoinSessionByCode(string joinCode)
    {
        try
        {
            ISession session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);
            Debug.Log($"Joined session {session.Id}. Network state: {session.Network.State:G}");
            session.Network.StateChanged += state => Debug.Log($"Network state changed: {state:G}");
        }
        catch (SessionException e)
        {
            Debug.LogError($"Failed to join session: {e.Error}");
        }
    }

    public async void InitMultiplayer()
    {
        try
	    {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
	    }
	    catch (Exception e)
        {
            Debug.LogException(e);
	    }
    }

    public void StartGame()
    {
        InitMultiplayer();
    }

    public void UpdateButtons()
    {
        foreach (Button button in buttons)
        {
            if (button.active) button.UpdateButton();
        }
    }
    public void Update()
    {
        if (paused) return;
        if (buttonsActive)UpdateButtons();
    }
}
