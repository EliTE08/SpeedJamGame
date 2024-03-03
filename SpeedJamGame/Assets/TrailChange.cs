using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailChange : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    private int currentTier = 0;
    private Color[] tierColor = { new Color(180f / 255f, 79f / 255f, 95f / 255f), new Color(47f / 255f, 61f / 255f, 96f / 255f), new Color(2f / 255f, 66f / 255f, 86f / 255f) };
    private float[] trailWidth = { 0.4f, 0.7f, 1.1f };
    
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
        /*if(GetComponent<PlayerController>() != null)
          {
              return GetComponent<PlayerController>().GetCurrentTier();
          }
          else
          {
              Debug.LogError("PlayerController Script not found on object!!!");
              return 0;
          }
        */
        return 0;
    }
      
    private void UpdateTrailColorAndWidth()
    {
        trailRenderer.material.color = tierColor[currentTier];
        trailRenderer.widthMultiplier = trailWidth[currentTier];
    }
}
