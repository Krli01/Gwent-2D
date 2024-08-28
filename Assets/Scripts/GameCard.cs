using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class GameCard : MonoBehaviour
{
    public bool showBack;
    public float Power;
    
    //public DisplayCard displayCard;

    public Image CardImage;
    public Image FactionCoat;
    public Image PowerNum;
    public Image Border;
    public Image Role;
    public TextMeshProUGUI CardName;
    public TextMeshProUGUI EffectText;

    //public GameObject Hand;
    
    // Start is called before the first frame update
    void Start()
    { 
        
    }

    public void Assign (Card thisCard)
    {
        CardImage.sprite = thisCard.CardImage;
        FactionCoat.sprite = thisCard.FactionCoat;
        PowerNum.sprite = thisCard.PowerNum;
        Border.sprite = thisCard.Border;
        Role.sprite = thisCard.CardRole;
        CardName.text = thisCard.CardName;
        EffectText.text = thisCard.EffectText;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(showBack)
        {
            //this.Border.sprite = 
        }
        
        //Hand = GameObject.Find("P1_Hand");

        //if(this.transform.parent == Hand.transform.parent) showBack = false;
        

    }

    void OnMouseOver()
    {
        
    }

    void OnMouseExit()
    {
        //DisplayCard.Reset();
    }

    public void OnClick()
    {
        
    }
}
