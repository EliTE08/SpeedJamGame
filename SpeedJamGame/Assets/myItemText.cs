using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class myItemText : MonoBehaviour
{
    TextMeshProUGUI mytext;
    void Start()
    {
        mytext = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void setText(string name, int score)
    {
        mytext.SetText(name + ": " + score);
    }
}
