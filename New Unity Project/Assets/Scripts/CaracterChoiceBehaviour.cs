using Prototype.NetworkLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CaracterChoiceBehaviour : NetworkBehaviour {

    public Button[] caracterChoice;

    public GameObject[] caracterCards;

    public Sprite[] caracterCardSprite;

    public Sprite[] caracterCardName;

    public Sprite[] caracterClass;

    public GameObject Card;

    public GameObject[] Playerprefab;

    public Button back;

    public int chosenCaracter;

    public GameObject player;
    void Start()
    {
        back.interactable = true;
        back.onClick.RemoveAllListeners();
        back.onClick.AddListener(OnBackButton);
        caracterChoice[0].interactable = true;

        caracterChoice[0].onClick.RemoveAllListeners();
        caracterChoice[0].onClick.AddListener(OnClickButtonMordred);
        caracterChoice[1].interactable = true;

        caracterChoice[1].onClick.RemoveAllListeners();
        caracterChoice[1].onClick.AddListener(OnClickButtonArcher);

    }

    private void OnBackButton()
    {
        this.GetComponent<LobbyPlayer>().LobbyCaracterChoice.gameObject.SetActive(false);
    }

    private void OnClickButtonArcher()
    {
        PlayerCard card = Card.GetComponent<PlayerCard>();
        Card.gameObject.SetActive(true);
        card.Card.sprite = caracterCardSprite[1];
        card.Name.sprite = caracterCardName[1];
        card.Class.sprite = caracterClass[1];
        chosenCaracter = 1;
        CmdUpdateplayerChoice(chosenCaracter);
    }

    private void OnClickButtonMordred()
    {
        PlayerCard card = Card.GetComponent<PlayerCard>();
        Card.gameObject.SetActive(true);
        card.Card.sprite = caracterCardSprite[0];
        card.Name.sprite = caracterCardName[0];
        card.Class.sprite = caracterClass[0];
        chosenCaracter = 0;
        CmdUpdateplayerChoice(chosenCaracter);
    }

    [Command]
    public void CmdUpdateplayerChoice(int choice)
    {
        player.GetComponent<LobbyPlayer>().RpcUpdateplayerChoice(choice);
    }

}
