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

    /// <summary>
    /// Sets the player's current respawn point to a given position
    /// </summary>
    /// <param name="respawnPoint">The position of the respawn point</param>
    public void SetRespawnPoint(Vector3 respawnPoint)
    {
        this.respawnPoint = respawnPoint;
    }

    /// <summary>
    /// Gets the player's current respawn point
    /// </summary>
    /// <returns>A Vector3 containing the respawn point's position</returns>
    public Vector3 GetRespawnPoint()
    {
        return respawnPoint;
    }

    /// <summary>
    /// Keeps track of a lightable object the player has interacted with to be reset in the event of death
    /// </summary>
    /// <param name="unsavedLightable">The lightable object to keep track of</param>
    public void AddUnsavedLightable(Lightable unsavedLightable)
    {
        if(unsavedLightable is not LightableNPC) currentlyLit.Add(unsavedLightable);
    }

    /// <summary>
    /// Removes all lightable objects currently being tracked
    /// </summary>
    public void ClearUnsavedLightables()
    {
        currentlyLit.Clear();
    }

    /// <summary>
    /// Changes every lightable currently being tracked to be unrevealed/unlit
    /// </summary>
    public void ResetUnsavedLightables()
    {
        foreach (Lightable lightable in currentlyLit) lightable.ChangeLightableState(false);
    }
}
