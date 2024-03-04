using System;
using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "Imated";
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(col.gameObject);
            TransitionCanvas.Instance.Transition(mainSceneName);
        }
    }
}
