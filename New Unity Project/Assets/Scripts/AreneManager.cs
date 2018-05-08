
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityStandardAssets._2D;

public class AreneManager : MonoBehaviour {

    public GameObject caracter;
    public RuntimeAnimatorController animatorController;
    
    void Init()
    {
    }

    private void OnNewGame()
    {
    }


    // Use this for initialization
    void Awake()
    {
        //GameObject playerCaracter;
        //GameObject playerCaracter1;
        //playerCaracter = (GameObject)Instantiate(caracter);
        //playerCaracter.AddComponent<Animator>(); // add each prefab its own Animator component
        //animator = playerCaracter.GetComponent<Animator>();
        //animator.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(animatorController);


        //playerCaracter1 = (GameObject)Instantiate(caracter);
        //playerCaracter1.AddComponent<Animator>(); // add each prefab its own Animator component
        //animator = playerCaracter1.GetComponent<Animator>();
        //animator.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(animatorController);
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Close()
    {

    }
}

