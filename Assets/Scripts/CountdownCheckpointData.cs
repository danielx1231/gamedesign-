using UnityEngine;

public static class CountdownCheckpointData
{
    public static bool hasCheckpoint = false;
    public static bool shouldRespawnAtCheckpoint = false;
    public static bool shouldShowPopupAfterReload = false;

    public static Vector3 checkpointPosition;

    public static void SaveCheckpoint(Vector3 position)
    {
        hasCheckpoint = true;
        checkpointPosition = position;
    }

    public static void PrepareRespawn()
    {
        if (!hasCheckpoint) return;

        shouldRespawnAtCheckpoint = true;
        shouldShowPopupAfterReload = true;
    }

    public static void ClearAll()
    {
        hasCheckpoint = false;
        shouldRespawnAtCheckpoint = false;
        shouldShowPopupAfterReload = false;
        checkpointPosition = Vector3.zero;
    }
}