using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThisCard : MonoBehaviour
{
    public Card thisCard;
    public int thisID;

    public int ID;
    public string CardName;
    public int Power;
    public Position CardPosition;
    public string CardEffect;
    public string CardFaction;

    public Sprite thisSprite;
    public Image thisImage;
    public bool showBack;

    //public GameObject Hand;
    
    // Start is called before the first frame update
    void Start()
    {
        thisCard = (thisID<10) ? CardDatabase.SeabornDeck[thisID] : CardDatabase.WhalerDeck[thisID-10];
    }

    // Update is called once per frame
    void Update()
    {
        //Hand = GameObject.Find("P1_Hand");
        //if(this.transform.parent == Hand.transform.parent) showBack = false;
        
        ID = thisCard.ID;
        CardName = thisCard.CardName;
        Power = thisCard.Power;
        CardPosition = thisCard.CardPosition;
        CardEffect = thisCard.CardEffect;
        CardFaction = thisCard.CardFaction;

        if (!showBack) thisSprite = thisCard.CardImage;
        else thisSprite = Resources.Load<Sprite>("DisplaySize_Back");
        thisImage.sprite = thisSprite;
    }
}
