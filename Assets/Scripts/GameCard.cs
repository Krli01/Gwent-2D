using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class GameCard : MonoBehaviour
{
    public bool showBack;
    public Card thisCard;
    public float Power;

    public Image CardImage;
    public Image FactionCoat;
    public Image PowerNum;
    public Image Border;
    public Image Role;
    public Text CardName;
    public Text EffectText;

    //public GameObject Hand;
    
    // Start is called before the first frame update
    void Start()
    {
        //thisCard = (thisID<10) ? CardDatabase.SeabornDeck[thisID] : CardDatabase.WhalerDeck[thisID-10]; el deck es una pila wiiii
    }

    // Update is called once per frame
    void Update()
    {
        if(showBack)
        {}
        
        //Hand = GameObject.Find("P1_Hand");

        //if(this.transform.parent == Hand.transform.parent) showBack = false;
        
        CardImage.sprite = thisCard.CardImage;
        FactionCoat.sprite = thisCard.FactionCoat;
        PowerNum.sprite = thisCard.PowerNum;
        Border.sprite = thisCard.Border;
        Role.sprite = thisCard.Role;
        CardName.text = thisCard.CardName;
        EffectText.text = thisCard.EffectText;

    }

    public void OnMouseOver()
    {
        
    }
}
