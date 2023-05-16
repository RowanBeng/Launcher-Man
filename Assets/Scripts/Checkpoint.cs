using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Checkpoint : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI WinLockTxt;
    [SerializeField] GameObject victoryCondition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && CollectGem.gemCount == 7)
        {
            victoryCondition.SetActive(false);
            Destroy(other.gameObject);
            SceneManager.LoadScene("Win");
        }
        else
        {
            if (other.tag == "Player" && CollectGem.gemCount == 0)
            {
                Debug.Log("Collect all gems before exiting!");
                victoryCondition.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        victoryCondition.SetActive(false);
    }
}
