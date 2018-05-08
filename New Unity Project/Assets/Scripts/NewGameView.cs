using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NewGameView : MonoBehaviour {
    public Button newGameButton;


    void Init()
    {
    }

    private void OnNewGame()
    {
            SceneManager.LoadScene(1);
    }
    

    // Use this for initialization
    void Start () {

        newGameButton.onClick.AddListener(OnNewGame);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Close()
    {
        
    }
}
