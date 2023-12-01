using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject ui;

    public TMP_Text[] txts;
    public GameObject whiteBox;

    private bool paused;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
            ui.SetActive(paused);
            Cursor.visible = paused;
            if (paused) Time.timeScale = 0.001f;
            else Time.timeScale = 1;
        }
    }

    public void hoverButton(int id)
    {
        txts[0].transform.localScale = Vector3.one;
        txts[1].transform.localScale = Vector3.one;

        txts[id].transform.localScale = Vector3.one * 1.2f;
        whiteBox.transform.localPosition = new Vector3(whiteBox.transform.localPosition.x, txts[id].transform.localPosition.y, whiteBox.transform.localPosition.z);
    }

    public void ClickBtn(int id)
    {
        if (id == 0)
        {
            paused = false;
            ui.SetActive(paused);
            Cursor.visible = paused;
            Time.timeScale = 1;
        }
        else
        {
            // to main menu
        }
    }
}
