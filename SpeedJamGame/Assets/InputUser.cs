using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUser : MonoBehaviour
{
    public InputField inputField;

    private void Start()
    {
        
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnEndEdit(string userInput)
    {
        
        Debug.Log("Username entered: " + userInput);
    }
}
