using UnityEngine;

public class FloatPlatform : MonoBehaviour
{
    [Header("Motion")]
    public float amplitude = 1.5f;   // 上下浮动高度（单位：Unity单位）
    public float speed = 1.0f;       // 浮动速度

    [Header("State")]
    public bool startOnEnable = false; // 解密前是否就启动

    private Vector3 startPos;
    private bool isActive;
    private float t;

    void Awake()
    {
        startPos = transform.position;
        isActive = startOnEnable;
    }

    void Update()
    {
        if (!isActive) return;

        t += Time.deltaTime * speed;
        float yOffset = Mathf.Sin(t) * amplitude;

        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }

    // 解密成功后调用这个
    public void Activate()
    {
        isActive = true;
        t = 0f; // 可选：每次启动从0开始
    }

    // 可选：如果你想停下并回到原位
    public void DeactivateAndReset()
    {
        isActive = false;
        transform.position = startPos;
    }
}