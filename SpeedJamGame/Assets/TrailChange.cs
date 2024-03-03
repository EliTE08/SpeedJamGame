using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailChange : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    private int currentTier = 0;
    private Color[] tierColor = {new Color(), new Color(51f / 255f, 241f / 255f, 241f / 255f), new Color(244f / 255f, 211f / 255f, 94f / 255f), new Color(219f / 255f, 80f / 255f, 74f / 255f), new Color(255f / 255f, 255f / 255f, 255f / 255f) };
    private float[] trailWidth = {0, 0.4f, 0.7f, 1.1f, 1.5f};
    private float[] trailLength = {0, 0.4f, 0.7f, 1.1f, 1.5f};

    void Start()
    {
        UpdateTrailColorAndWidth();
    }


    void Update()
    {
        int newTier = GetCurrentTier();
        if (newTier != currentTier)
        {
            currentTier = newTier;
            UpdateTrailColorAndWidth();
        }
    }

    private int GetCurrentTier()
    {
        if (GetComponent<PlayerController>() != null)
        {
            return GetComponent<PlayerController>().GetCurrentTier();
        }
        else
        {
            Debug.LogError("PlayerController Script not found on object!!!");
            return 0;
        }
    }

    private void UpdateTrailColorAndWidth()
    {
        trailRenderer.startColor = tierColor[currentTier];
        trailRenderer.endColor = tierColor[currentTier];
        trailRenderer.widthMultiplier = trailWidth[currentTier];
    }
}
