using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
   public void PlayGame()
   {
      if(SceneManager.GetActiveScene().buildIndex == 3)
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
      }
      else if (SceneManager.GetActiveScene().buildIndex == 4)
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
      }
      else

         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   
   }

   public void QuitGame()
   {
    Application.Quit();
   }
}
