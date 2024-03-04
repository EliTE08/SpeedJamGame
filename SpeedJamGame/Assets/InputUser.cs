using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputUser : MonoBehaviour
{
    public TMP_InputField inputField;

    private void Start()
    {
        
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnEndEdit(string userInput)
    {
        LootLockerManager.Instance.StartLogin();
    }
}
