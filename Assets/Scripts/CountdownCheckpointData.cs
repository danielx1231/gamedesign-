using UnityEngine;

public static class CountdownCheckpointData
{
    // 是否已经有倒计时复活点
    public static bool hasCheckpoint = false;

    // 下一次加载场景时，玩家是否应该出生在 checkpoint
    public static bool shouldRespawnAtCheckpoint = false;

    // checkpoint 的位置
    public static Vector3 checkpointPosition = Vector3.zero;

    // 是否已经完成过前置谜题，并解锁了倒计时挑战
    public static bool challengeUnlocked = false;

    // 下一次加载场景后，是否要直接启动倒计时挑战
    public static bool shouldStartChallengeOnLoad = false;

    public static void SaveCheckpoint(Vector3 position)
    {
        hasCheckpoint = true;
        checkpointPosition = position;
    }

    public static void PrepareRespawn()
    {
        if (hasCheckpoint)
        {
            shouldRespawnAtCheckpoint = true;
        }

        if (challengeUnlocked)
        {
            shouldStartChallengeOnLoad = true;
        }
    }

    public static void UnlockChallenge()
    {
        challengeUnlocked = true;
    }

    public static void PrepareChallengeRestart()
    {
        if (hasCheckpoint)
        {
            shouldRespawnAtCheckpoint = true;
        }

        challengeUnlocked = true;
        shouldStartChallengeOnLoad = true;
    }

    public static void ConsumeRespawnFlag()
    {
        shouldRespawnAtCheckpoint = false;
    }

    public static void ConsumeChallengeStartFlag()
    {
        shouldStartChallengeOnLoad = false;
    }

    public static void ResetAll()
    {
        hasCheckpoint = false;
        shouldRespawnAtCheckpoint = false;
        checkpointPosition = Vector3.zero;

        challengeUnlocked = false;
        shouldStartChallengeOnLoad = false;
    }
}