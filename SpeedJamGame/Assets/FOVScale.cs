using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FOVScale : MonoBehaviour
{

    public Rigidbody2D playerRigidbody;
    public CinemachineVirtualCamera virtualcamera;
    private float minOrthoSize = 5f;
    private float maxOrthoSize = 10f;
    private float scaleSpeed = 1f;
    
    private void Update()
    {
        if(playerRigidbody != null && virtualcamera != null)
        {
            float playerVelocityMagnitude = playerRigidbody.velocity.magnitude;
            float targetOrthoSize = Mathf.Clamp(playerVelocityMagnitude * scaleSpeed, minOrthoSize, maxOrthoSize);

            virtualcamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualcamera.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime);
        }
    }
}
