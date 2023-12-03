using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewspaperIteraction : MonoBehaviour
{
    public TMP_Text newspaper;


    private NewspaperTextHolder holder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Newspaper") return;
        holder = other.GetComponent<NewspaperTextHolder>();
        newspaper.gameObject.SetActive(true);
        newspaper.text = holder.text;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Newspaper") return;
        newspaper.gameObject.SetActive(false);
    }
}
