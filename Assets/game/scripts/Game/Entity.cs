using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour
{
    public int index = 0;

    public void Synchronize(SyncData entity)
    {
        transform.position = entity.pos;
    }
}