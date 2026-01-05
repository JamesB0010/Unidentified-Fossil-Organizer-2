using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//i would like it to be known that i dont like this class or what im about to do whatsoever

//but for some reason when i build and run the ui is really weird until you go to the credits screen and then back to the main menu

//so this script loads the credits then straight away loads the main menu and it fixed the problem 

//    :(
public class LoadCreditsThenMainMenuWeirdScript : MonoBehaviour
{
    private bool hasDoneTheWeirdthingBefore = false;
    private static LoadCreditsThenMainMenuWeirdScript instance = null;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null && hasDoneTheWeirdthingBefore == false)
        {
            hasDoneTheWeirdthingBefore = true;
            instance = this;
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded += DeleteSelf;
        SceneManager.sceneLoaded -= SceneLoaded;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void DeleteSelf(Scene scene, LoadSceneMode mode)
    {
        if(this == null)
            return;
            
        SceneManager.sceneLoaded -= DeleteSelf;
        Destroy(this.gameObject);
    }
}
