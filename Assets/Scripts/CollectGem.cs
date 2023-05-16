using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectGem : MonoBehaviour
{
    public static int gemCount = 0;
    public TextMeshProUGUI gemCounter;

   private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Gem")
        {
            gemCount++;
            Debug.Log("Gem was collected.");
            Destroy(other.gameObject);
        }
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        gemCounter.text = gemCount.ToString();
    }
}
