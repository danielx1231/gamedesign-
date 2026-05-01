using UnityEngine;
using System.Collections;

public class GateMover : MonoBehaviour
{
    [Header("Motion")]
    public Vector3 openOffset = new Vector3(0f, 2.5f, 0f);
    public float moveDuration = 0.4f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private Coroutine moveRoutine;

    private void Awake()
    {
        closedPos = transform.position;
        openPos = closedPos + openOffset;
    }

    // ✅ RuneStoneController 调用的名字
    public void OpenGate() => MoveTo(openPos);
    public void CloseGate() => MoveTo(closedPos);

    // ✅ 兼容我之前给你的另一版命名（如果你别处用了）
    public void Open() => OpenGate();
    public void Close() => CloseGate();

    private void MoveTo(Vector3 targetPos)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine(targetPos));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, t / moveDuration);
            yield return null;
        }

        transform.position = targetPos;
        moveRoutine = null;
    }
}