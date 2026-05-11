using UnityEngine;
using System.Collections;

public class ColorRunePuzzleManager : MonoBehaviour
{
    [Header("Rune Stones")]
    public ColorRuneStone leftRune;
    public ColorRuneStone centerRune;
    public ColorRuneStone rightRune;

    [Header("Door")]
    public PuzzleDoorController doorToOpen;

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
        return leftRune != null && leftRune.IsLit()
            && centerRune != null && centerRune.IsLit()
            && rightRune != null && rightRune.IsLit();
    }

    private bool IsCorrect()
    {
        ColorRuneStone.RuneColor leftColor = leftRune.GetCurrentColor();
        ColorRuneStone.RuneColor centerColor = centerRune.GetCurrentColor();
        ColorRuneStone.RuneColor rightColor = rightRune.GetCurrentColor();

        return leftColor != centerColor
            && leftColor != rightColor
            && centerColor != rightColor;
    }

    private IEnumerator SuccessSequence()
    {
        isChecking = true;
        isSolved = true;

        ColorRuneStone[] runes =
        {
            leftRune,
            centerRune,
            rightRune
        };

        yield return StartCoroutine(FlashCurrentColors(runes));

        foreach (ColorRuneStone rune in runes)
        {
            if (rune != null)
            {
                rune.RestoreCurrentColor();
                rune.LockRune();
            }
        }

        if (doorToOpen != null)
        {
            doorToOpen.OpenDoor();
        }
        else
        {
            Debug.LogWarning("Door To Open is not assigned in ColorRunePuzzleManager.");
        }

        isChecking = false;
    }

    private IEnumerator FailSequence()
    {
        isChecking = true;

        ColorRuneStone[] runes =
        {
            leftRune,
            centerRune,
            rightRune
        };

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
}