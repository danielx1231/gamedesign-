using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{

    public FloatPlatform floatingPlatform;
    public int[] correctOrder = { 0, 1, 2, 3 };

    public GameObject runeStone; // 成功后触发动画用
    public GameObject[] objectsToActivateByCorrectStep;
    public GameObject[] objectsToDeactivateByCorrectStep;
    public GameObject[] objectsToActivateOnSuccess;
    public GameObject[] objectsToDeactivateOnSuccess;
    public Transform[] objectsToMoveOnSuccess;
    public Vector3 successMoveOffset = Vector3.zero;
    public float successMoveDuration = 0.75f;

    private List<int> playerInput = new List<int>();
    private bool isFinished = false;

    // Button 只需要调用这个，不要再传 callback
    public void OnButtonPress(int buttonIndex)
    {
        if (isFinished) return;

        int expectedIndex = playerInput.Count;
        if (expectedIndex >= correctOrder.Length) return;

        if (buttonIndex == correctOrder[expectedIndex])
        {
            playerInput.Add(buttonIndex);
            TriggerCorrectStepObjects(playerInput.Count - 1);

            if (playerInput.Count == correctOrder.Length)
            {
                Success();
            }
        }
        else
        {
            ResetPuzzle();
        }
    }

    private void TriggerCorrectStepObjects(int stepIndex)
    {
        if (objectsToActivateByCorrectStep != null && stepIndex < objectsToActivateByCorrectStep.Length)
        {
            GameObject target = objectsToActivateByCorrectStep[stepIndex];
            if (target != null) target.SetActive(true);
        }

        if (objectsToDeactivateByCorrectStep != null && stepIndex < objectsToDeactivateByCorrectStep.Length)
        {
            GameObject target = objectsToDeactivateByCorrectStep[stepIndex];
            if (target != null) target.SetActive(false);
        }
    }

    private void ResetPuzzle()
    {
        playerInput.Clear();

        // 全部渐隐并允许重新按（按钮脚本里已经把 isLit 设回 false）
        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();
        foreach (var btn in allButtons)
        {
            btn.TurnOffGlow();
        }
    }

    private void Success()
    {
        isFinished = true;
        Debug.Log("解密成功！执行闪烁效果");

        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();
        foreach (var btn in allButtons) btn.LockAndFlash();

        // ✅ 切换BGM（淡出→换歌→淡入）
        if (BGMController.Instance != null)
        {
            Debug.Log("Calling SwitchToAfterPuzzleBGM()");
            BGMController.Instance.SwitchToAfterPuzzleBGM();
        }

        if (runeStone != null)
        {
            Animator anim = runeStone.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Active");
        }

        foreach (GameObject target in objectsToActivateOnSuccess)
        {
            if (target != null) target.SetActive(true);
        }

        foreach (GameObject target in objectsToDeactivateOnSuccess)
        {
            if (target != null) target.SetActive(false);
        }

        foreach (Transform target in objectsToMoveOnSuccess)
        {
            if (target != null) StartCoroutine(MoveToSuccessPosition(target));
        }

        if (floatingPlatform != null) floatingPlatform.Activate();
    }

    private System.Collections.IEnumerator MoveToSuccessPosition(Transform target)
    {
        Vector3 start = target.position;
        Vector3 end = start + successMoveOffset;

        if (successMoveDuration <= 0f)
        {
            target.position = end;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < successMoveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / successMoveDuration);
            target.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        target.position = end;
    }
}
