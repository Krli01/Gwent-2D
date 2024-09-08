using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class GameCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool showBack;
    public float Power;
    //private float PowerExtra;
    public bool isSelected;
    public Role role;

    public Image CardImage;
    public Image FactionCoat;
    public Image PowerNum;
    public Image Border;
    public Image Role;
    public Image CardBack;
    public TextMeshProUGUI CardName;
    public TextMeshProUGUI EffectText;
    private Card BaseCard;

    public Vector3 popUpOnHover = new (0, 10, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        CardImage = transform.Find("CardImage")?.GetComponent<Image>();
        FactionCoat = transform.Find("Faction")?.GetComponent<Image>();
        PowerNum = transform.Find("Power")?.GetComponent<Image>();
        Border = transform.Find("Border")?.GetComponent<Image>();
        Role = transform.Find("Role")?.GetComponent<Image>();
        CardBack = transform.Find("CardBack")?.GetComponent<Image>();
        CardName = transform.Find("Card Name")?.GetComponent<TextMeshProUGUI>();
        EffectText = transform.Find("Effect Text")?.GetComponent<TextMeshProUGUI>();
        isSelected = false;
        CardBack.enabled = false;
    }

    public void Assign (Card baseCard)
    {
        Power = baseCard.Power;
        CardImage.sprite = Resources.Load<Sprite>($"{baseCard.img}");
        FactionCoat.sprite = baseCard.FactionCoat;
        if(baseCard.PowerNum != null) PowerNum.sprite = baseCard.PowerNum;
        else PowerNum.enabled = false;
        Border.sprite = baseCard.Border;
        Role.sprite = baseCard.CardRole;
        CardName.text = baseCard.CardName;
        EffectText.text = baseCard.EffectText;
        role = baseCard.thisRole;
        BaseCard = baseCard;
    }
    

    // Update is called once per frame
    void Update()
    {
        CardBack.enabled = showBack ? true : false;      
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter");
        if (transform.parent == TurnSystem.Active.thisHand.transform && !isSelected)
        {
            transform.position += popUpOnHover;
        }

        if(!showBack) Game.displayCard.Show(BaseCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit");
        if (transform.parent == TurnSystem.Active.thisHand.transform && !isSelected)
        {
            transform.position -= popUpOnHover;    
        }

        if (!isSelected) 
        {
            if(Game.Selected.Count == 0) Game.displayCard.Reset();
            else Game.displayCard.Show(Game.Selected[0].BaseCard);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("clicked");
        if (role == global::Role.Leader)
        {
            //enablear boton de activar habilidad
        }
        
        else if (transform.parent == TurnSystem.Active.thisHand.transform)
        {
            if(Game.Selected.Contains(this)) 
            {
                Game.Selected.Remove(this);
                Game.DisableAllZones(TurnSystem.Active);
                //Debug.Log("De-Selected");
            }
            else if (Game.Selected.Count > 0)
            {
                Game.Selected[0].isSelected = false;
                Game.Selected[0].transform.position -= popUpOnHover;            
                Game.Selected.Clear();
                Game.Selected.Add(this);
                if (!TurnSystem.ActionTaken) Game.EnableZone(TurnSystem.Active, role);
                //Debug.Log("Selected");
            }
            else
            {
                Game.Selected.Add(this);
                if (!TurnSystem.ActionTaken) Game.EnableZone(TurnSystem.Active, role);
                //Debug.Log("Selected");
            }
        }

        else if (Game.Selected.Count > 0)
        {
            if(TurnSystem.ActionTaken) return;

            if (BaseCard is Weather && Game.Selected[0].role == global::Role.Clearing)
            {
                //substitute and eliminate weather card
                Transform t = transform.parent;
                TurnSystem.Active.graveyard.Cards.Add(BaseCard);
                transform.SetParent(null);
                GameCard card = Game.Selected[0];
                card.transform.SetParent(t);
                card.transform.localPosition = Vector3.zero;            
                Game.Selected.Clear();
                TurnSystem.ActionTaken = true;
                Destroy(this);
            }
            else if (BaseCard is Unit && Game.Selected[0].role == global::Role.Decoy)
            {
                Transform hand = TurnSystem.Active.thisHand.transform;
                Transform t = transform.parent;
                transform.SetParent(hand);
                GameCard card = Game.Selected[0];
                card.transform.SetParent(t);
                card.isSelected = false;
                card.transform.localPosition = Vector3.zero;            
                Game.Selected.Clear();
                TurnSystem.ActionTaken = true;
            }
        }
        
        isSelected = !isSelected;
        //Debug.Log(isSelected);
        Debug.Log($"Active player: {TurnSystem.Active.Name}");
    }

}
