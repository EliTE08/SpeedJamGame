using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.CompareTag("Player"))
        {

            //Make it call respawn animation too (Fast animation)

            //PlayerController.Instance.Invoke("Respawn", 1f);
            
            PlayerGhost.Instance.PlayRecorded();
        }
    }

}
