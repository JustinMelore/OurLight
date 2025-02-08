using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the player's current spawn point and which Lightables should be reset upon the player's death
/// </summary>
public class RespawnManager : MonoBehaviour
{
    private HashSet<Lightable> currentlyLit;
    private Vector3 respawnPoint;

    private void Start()
    {
        currentlyLit = new HashSet<Lightable>();
        respawnPoint = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void SetRespawnPoint(Vector3 respawnPoint)
    {
        this.respawnPoint = respawnPoint;
    }

    public Vector3 GetRespawnPoint()
    {
        return respawnPoint;
    }

    public void AddUnsavedLightable(Lightable unsavedLightable)
    {
        if(unsavedLightable is not LightableNPC) currentlyLit.Add(unsavedLightable);
    }

    public void ClearUnsavedLightables()
    {
        currentlyLit.Clear();
    }

    public void ResetUnsavedLightables()
    {
        foreach (Lightable lightable in currentlyLit) lightable.ChangeLightableState(false);
    }
}
