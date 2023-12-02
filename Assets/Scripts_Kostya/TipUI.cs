using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipUI : MonoBehaviour
{
    public TMP_Text keytxt;
    public TMP_Text tiptxt;
    public GameObject objTip;

    public void ShowTip(string key, string tip)
    {
        objTip.SetActive(true);
        keytxt.text = key;
        tiptxt.text = tip;
    }

    public void HideTip()
    {
        objTip.SetActive(false);
    }
}
