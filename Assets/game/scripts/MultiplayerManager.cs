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
using Unity.VisualScripting;
using DG.Tweening;

public class MultiplayerManager : MonoBehaviour
{
    public TMP_InputField codeInputField;

    public TMP_Text roomText;
    public Logging logger;

    public List<Button> buttons;

    public Button playButton;

    public ISession currentSession;

    public bool opened = false;

    public bool waitingForPlayer = false;

    public async Task LeaveSession()
    {
        await currentSession.LeaveAsync();
    }

    public async Task StartSessionAsHost()
    {
        logger.Log("Starting host...", Color.white);
        var options = new SessionOptions
        {
            MaxPlayers = 2
        }.WithRelayNetwork(); // or WithDistributedAuthorityNetwork() to use Distributed Authority instead of Relay
        var session = await MultiplayerService.Instance.CreateSessionAsync(options);
        Debug.Log($"Session {session.Id} created! Join code: {session.Code}");
        logger.Log("room created", Color.green);
        currentSession = session;

        StartWaitingForPlayer();
        UpdateRoomText();
    }

    public void UpdateRoomText()
    {
        if (currentSession == null) return;
        roomText.text = currentSession.PlayerCount + "/" + currentSession.MaxPlayers + " players"
        + "\ncode: " + currentSession.Code;

        if (currentSession.PlayerCount == currentSession.MaxPlayers)
        {
            playButton.transform.DOScale(1, 0.5f);
            playButton.active = true;
        }
    }

    public void StartWaitingForPlayer()
    {
        waitingForPlayer = true;
        UpdateRoomText();
    }

    public async Task JoinSessionByCode(string joinCode)
    {
        try
        {
            logger.Log("joining room...", Color.white);
            ISession session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);
            currentSession = session;
            logger.Log("joined room", Color.green);
            Debug.Log($"Joined session {session.Id}. Network state: {session.Network.State:G}");
            session.Network.StateChanged += state => Debug.Log($"Network state changed: {state:G}");
            UpdateRoomText();
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

    public void Open()
    {
        gameObject.SetActive(true);
        foreach (Button button in buttons) button.active = true;

        roomText.text = "";
        playButton.active = false;
        playButton.transform.DOKill();
        playButton.transform.localScale = Vector3.zero;

        currentSession = null;
    }
    public void Quit()
    {
        foreach (Button button in buttons) button.active = false;

        gameObject.SetActive(false);
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
        UpdateButtons();
    }
}
