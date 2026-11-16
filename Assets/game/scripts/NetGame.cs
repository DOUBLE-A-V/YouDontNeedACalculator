using Unity.Netcode;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Video;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;

[Serializable]
public class SyncData
{
    public Vector3 pos;
    public int index;
}

public class NetGame : NetworkBehaviour
{
    public List<Entity> entities = new List<Entity>();
    public List<Unit> units = new List<Unit>();

    public List<Entity> allEntities = new List<Entity>();
    public List<Unit> allUnits = new List<Unit>();

    public bool ready = false;
    public NetworkVariable<int> keyPressed = new NetworkVariable<int>(-1);
    public int readyPlayersCount = 0;

    public int tickrate = 30;
    public float tickTimer = 0;

    public int entityIndexCount = 0;

    public Vector3 basePos;

    public override void OnNetworkSpawn()
    {
        PlayerConnectedRpc();
        if (IsOwner)M.multiplayerManager.netgame = this;
        if (IsOwner)M.netgame = this;
    }

    public async override void OnNetworkDespawn()
    {
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
    public void SpawnUnitRpc(string unitName)
    {
        Unit unit = Instantiate(M.prefabs.units[0]);
        unit.transform.position = basePos;
        unit.index = entityIndexCount;
        entityIndexCount++;

        units.Add(unit);
        entities.Add(unit);
        
        M.netgame.allUnits.Add(unit);
        M.netgame.allEntities.Add(unit);
    }

    public List<Vector3> ConvertOrderDataToMovePath(float[] data)
    {
        List<Vector3> result = new List<Vector3>();
        bool xswitch = true;
        float x = 0;
        foreach (float f in data)
        {
            if (xswitch)
            {
                x = f;
                xswitch = false;
            } else
            {
                xswitch = true;
                result.Add(new Vector3(x, f, 0));
            }
        }
        return result;
    }

    [Rpc(SendTo.NotOwner)]
    public void SendOrderRpc(string orderName, float[] orderArgs, int unitId)
    {
        if (orderName == "move")
        {
            Unit unit = M.netgame.allUnits.Find(x => x.index == unitId);
            if (unit) unit.movePath = ConvertOrderDataToMovePath(orderArgs);
        }
    }

    [Rpc(SendTo.Everyone)]
    public void StartGameRpc()
    {
        M.multiplayerManager.logger.Log("starting game", Color.white);
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

    public List<SyncData> ConvertFloatArrayToSyncData(float[][] arr)
    {
        List<SyncData> result = new List<SyncData>();
        foreach (float[] dataArray in arr)
        {
            List<float> data = new List<float>(dataArray);
            result.Add(
                new SyncData()
                {
                    pos = new Vector3(data[1], data[2], 0),
                    index = Mathf.RoundToInt(data[0])
                }
            );
        }
        return result;
    }

    [Rpc(SendTo.NotServer)]
    public void SynchronizeRpc(float[][] syncEntitiesArray)
    {
        try
        {
            List<SyncData> syncEntities = ConvertFloatArrayToSyncData(syncEntitiesArray);
            M.multiplayerManager.syncPool = syncEntities;
            foreach (Entity entity in M.netgame.allEntities)
            {
                SyncData serverEntity = syncEntities.Find(x => x.index == entity.index);
                if (serverEntity != null) entity.Synchronize(serverEntity);
            }
        } catch (Exception ex)
        {
            
        }
    }

    public float[][] GetEntitySyncData()
    {
        List<float[]> resultRaw = new List<float[]>();
        foreach (Entity entity in allEntities)
        {
            resultRaw.Add(
                new float[3]{entity.index, entity.transform.position.x, entity.transform.position.y}
            );
        }
        
        return resultRaw.ToArray();
    }
    
    public void UpdateAllUnits()
    {
        foreach (Unit unit in allUnits)
        {
            unit.UpdateUnit();
        }
    }

    public void Update()
    {
        if (!IsOwner) return;
        UpdateAllUnits();

        if (IsServer)
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
            {
                tickTimer = 1f / tickrate;
                SynchronizeRpc(GetEntitySyncData());
            }
        }
    }
}
