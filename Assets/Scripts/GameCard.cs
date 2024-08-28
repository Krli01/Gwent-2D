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
    
    // Start is called before the first frame update
    void Start()
    {
        CardImage = transform.Find("CardImage")?.GetComponent<Image>();
        FactionCoat = transform.Find("Faction")?.GetComponent<Image>();
        PowerNum = transform.Find("Power")?.GetComponent<Image>();
        Border = transform.Find("Border")?.GetComponent<Image>();
        Role = transform.Find("Role")?.GetComponent<Image>();

        CardName = transform.Find("Card Name")?.GetComponent<TextMeshProUGUI>();
        EffectText = transform.Find("Effect Text")?.GetComponent<TextMeshProUGUI>();
    }

    public void Assign (Card baseCard)
    {
        Power = baseCard.Power;
        CardImage.sprite = baseCard.CardImage;
        FactionCoat.sprite = baseCard.FactionCoat;
        PowerNum.sprite = baseCard.PowerNum;
        Border.sprite = baseCard.Border;
        Role.sprite = baseCard.CardRole;
        CardName.text = baseCard.CardName;
        EffectText.text = baseCard.EffectText;
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
