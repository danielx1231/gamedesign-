using UnityEngine;
using System.Collections;

public class NineRunePuzzleManager : MonoBehaviour
{
    [Header("Rune Stones")]
    public ColorRuneStone rune11;
    public ColorRuneStone rune12;
    public ColorRuneStone rune13;

    public ColorRuneStone rune21;
    public ColorRuneStone rune22;
    public ColorRuneStone rune23;

    public ColorRuneStone rune31;
    public ColorRuneStone rune32;
    public ColorRuneStone rune33;

    [Header("Success Object")]
    public FloatPlatform2 floatingPlatform;

    [Header("Feedback")]
    public float flashDuration = 2f;
    public float flashInterval = 0.15f;

    private bool isSolved = false;
    private bool isChecking = false;

    public bool IsBusy()
    {
        return isChecking;
    }

    public bool IsSolved()
    {
        return isSolved;
    }

    public bool TryCheckPuzzle()
    {
        if (isSolved) return true;
        if (isChecking) return false;

        if (!AllRunesLit())
        {
            StartCoroutine(FailSequence());
            return false;
        }

        if (IsCorrect())
        {
            StartCoroutine(SuccessSequence());
            return true;
        }

        StartCoroutine(FailSequence());
        return false;
    }

    private bool AllRunesLit()
    {
        ColorRuneStone[] runes = GetAllRunes();

        foreach (ColorRuneStone rune in runes)
        {
            if (rune == null || !rune.IsLit())
            {
                return false;
            }
        }

        return true;
    }

    private bool IsCorrect()
    {
        // 上边：11 - 12 - 13
        if (!PathHasNoRepeat(rune11, rune12, rune13)) return false;

        // 左边：11 - 21 - 31
        if (!PathHasNoRepeat(rune11, rune21, rune31)) return false;

        // 下边：31 - 32 - 33
        if (!PathHasNoRepeat(rune31, rune32, rune33)) return false;

        // 右边：13 - 23 - 33
        if (!PathHasNoRepeat(rune13, rune23, rune33)) return false;

        // 中间上半部分：12 - 22
        if (!PathHasNoRepeat(rune12, rune22)) return false;

        // 左上到右下：11 - 22 - 33
        if (!PathHasNoRepeat(rune11, rune22, rune33)) return false;

        return true;
    }

    private bool PathHasNoRepeat(params ColorRuneStone[] path)
    {
        for (int i = 0; i < path.Length; i++)
        {
            if (path[i] == null) return false;

            for (int j = i + 1; j < path.Length; j++)
            {
                if (path[j] == null) return false;

                if (path[i].GetCurrentColor() == path[j].GetCurrentColor())
                {
                    return false;
                }
            }
        }

        return true;
    }

    private IEnumerator SuccessSequence()
    {
        isChecking = true;
        isSolved = true;

        ColorRuneStone[] runes = GetAllRunes();

        yield return StartCoroutine(FlashCurrentColors(runes));

        foreach (ColorRuneStone rune in runes)
        {
            if (rune != null)
            {
                rune.RestoreCurrentColor();
                rune.LockRune();
            }
        }

        if (floatingPlatform != null)
        {
            floatingPlatform.Activate();
        }
        else
        {
            Debug.LogWarning("Floating Platform is not assigned in NineRunePuzzleManager.");
        }

        isChecking = false;
    }

    private IEnumerator FailSequence()
    {
        isChecking = true;

        ColorRuneStone[] runes = GetAllRunes();

        yield return StartCoroutine(FlashWhite(runes));

        foreach (ColorRuneStone rune in runes)
        {
            if (rune != null)
            {
                rune.TurnOff();
            }
        }

        isChecking = false;
    }

    private IEnumerator FlashWhite(ColorRuneStone[] runes)
    {
        float timer = 0f;
        bool visible = false;

        while (timer < flashDuration)
        {
            visible = !visible;

            foreach (ColorRuneStone rune in runes)
            {
                if (rune != null)
                {
                    rune.SetGlowWhite(visible);
                }
            }

            timer += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }
    }

    private IEnumerator FlashCurrentColors(ColorRuneStone[] runes)
    {
        float timer = 0f;
        bool visible = false;

        while (timer < flashDuration)
        {
            visible = !visible;

            foreach (ColorRuneStone rune in runes)
            {
                if (rune != null)
                {
                    rune.RestoreCurrentColor();
                    rune.SetGlowVisible(visible);
                }
            }

            timer += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        foreach (ColorRuneStone rune in runes)
        {
            if (rune != null)
            {
                rune.RestoreCurrentColor();
            }
        }
    }

    private ColorRuneStone[] GetAllRunes()
    {
        return new ColorRuneStone[]
        {
            rune11, rune12, rune13,
            rune21, rune22, rune23,
            rune31, rune32, rune33
        };
    }
}