using UnityEngine;
using TMPro;
using System;

public class PapersUI : MonoBehaviour
{
    public TextMeshProUGUI papersText;

    public void UpdateText()
    {
        int count = Paper.papersLeft;

        if (count == 0)
        {
            papersText.text = "Find the exit";
            papersText.color = Color.red;
            return;
        }

        string paperWord = count == 1 ? "paper" : "papers";
        papersText.text = $"{count} {paperWord} left";

        // Färglogik
        if (count <= 3)
            papersText.color = Color.red;
        else if (count <= 6)
            papersText.color = new Color(1f, 0.6f, 0f); // orange
        else
            papersText.color = Color.white;

        StopAllCoroutines();
        StartCoroutine(Flicker());

    }

    System.Collections.IEnumerator Flicker()
    {
        papersText.enabled = false;
        yield return new WaitForSeconds(0.05f);
        papersText.enabled = true;
    }
}