using System.Collections.Generic;
using UnityEngine;

public class LeverGroupActivator : MonoBehaviour
{
    public GateOpener[] levers;
    public Animator gateAnimator;
    public string gateObjectName = "Gate";
    public string gateTrigger = "Open";

    private readonly List<GateOpener> registeredLevers = new List<GateOpener>();
    private bool gateOpened;

    private void Awake()
    {
        ResolveReferences();
        Evaluate();
    }

    private void Start()
    {
        ResolveReferences();
        Evaluate();
    }

    public void RegisterLever(GateOpener lever)
    {
        if (lever == null || registeredLevers.Contains(lever)) return;

        registeredLevers.Add(lever);
        Evaluate();
    }

    public void NotifyLeverChanged()
    {
        Evaluate();
    }

    private void ResolveReferences()
    {
        if (levers == null || levers.Length == 0)
        {
            levers = FindObjectsOfType<GateOpener>(true);
        }

        foreach (GateOpener lever in levers)
        {
            if (lever != null && !registeredLevers.Contains(lever))
            {
                registeredLevers.Add(lever);
            }
        }

        if (gateAnimator == null)
        {
            GameObject gateObject = null;
            if (!string.IsNullOrEmpty(gateObjectName))
            {
                gateObject = GameObject.Find(gateObjectName);
            }

            if (gateObject == null)
            {
                gateObject = GameObject.Find("Gate (1)");
            }

            if (gateObject != null)
            {
                gateAnimator = gateObject.GetComponent<Animator>();
            }
        }
    }

    private void Evaluate()
    {
        bool allPulled = registeredLevers.Count > 0;
        foreach (GateOpener lever in registeredLevers)
        {
            if (lever == null || !lever.IsPulled)
            {
                allPulled = false;
                break;
            }
        }

        if (allPulled)
        {
            OpenGate();
        }
    }

    private void OpenGate()
    {
        if (gateOpened) return;
        gateOpened = true;

        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger(gateTrigger);
        }
    }
}
