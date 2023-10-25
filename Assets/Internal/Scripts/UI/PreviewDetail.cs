using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreviewDetail : MonoBehaviour
{
    public TextMeshProUGUI txtView;
    public void ChangeTextView(string txt)
    {
        txtView.text = txt;
    }
}
