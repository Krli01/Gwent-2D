using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using System.Linq;

public class GameCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject prefab;
    public Card BaseCard {get; private set;}
    public Player Owner {get; private set;}

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
    }
    void Update()
    {
        CardBack.enabled = showBack ? true : false;      
    }

    public void SetOwner(Player owner)
    {
        Owner = owner;
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
    
    public void ActivateLeader()
    {
        if (BaseCard is LeaderCard) BaseCard.Activate();
        Owner.ActivateLeader.interactable = false;
        TurnSystem.ActionTaken = true;
        this.DeSelect(this);
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
        if (showBack) return;
        
        if (role == global::Role.Leader)
        {
            if (Game.Selected.Contains(this))
            {
                DeSelect(this);
                Owner.HideLeaderButton();
            } 
            else 
            {
                Select();
                if (TurnSystem.Active == Owner) Owner.ShowLeaderButton();
                if(TurnSystem.ActionTaken) Owner.ActivateLeader.interactable = false;
            }
        }
        
        else if (transform.parent == TurnSystem.Active.thisHand.transform)
        {
            if(Game.Selected.Contains(this))
            {
                DeSelect(this);
                Owner.battlefield.DisableAllZones();
            }
            else
            {
                Select();
                if (!TurnSystem.ActionTaken) Owner.battlefield.EnableZone(role);
            }
        }

        else if (Game.Selected.Count > 0)
        {
            if(TurnSystem.ActionTaken)
            {
                ChangeSelect();
                return;
            } 

            if (BaseCard is Weather && Game.Selected[0].role == global::Role.Clearing) ClearWeather();

            else if (BaseCard is Unit && Game.Selected[0].role == global::Role.Decoy)
            {
                if (Owner.Name == Game.Selected[0].Owner.Name) ReturnToHand();
                Owner.battlefield.UpdatePoints();
            }
        }

        else ChangeSelect();
    }

    void ChangeSelect()
    {
        if (Game.Selected.Contains(this)) DeSelect(this);
        else Select();
    }
    void Select()
    {
        // only 1 card can be selected
        if (Game.Selected.Count > 0)
        {
            for (int i = 0; i < Game.Selected.Count; i++)
            {
                GameCard c = Game.Selected[i];
                DeSelect(c);
            }
        }
        
        isSelected = true;
        Game.Selected.Add(this);
        Debug.Log("Selected");
    }

    public void DeSelect(GameCard card)
    {
        card.isSelected = false;
        if (card.gameObject != this.gameObject && card.transform.parent == TurnSystem.Active.thisHand.transform) card.transform.position -= popUpOnHover;
        Game.Selected.Remove(card);
        Debug.Log("De-Selected");
    }

    void ClearWeather()
    {
        Transform t = transform.parent;
        transform.SetParent(null);
        GameCard clearingCard = Game.Selected[0];
        clearingCard.transform.SetParent(t);
        clearingCard.transform.localPosition = Vector3.zero;            
        Game.Selected.Clear();
        TurnSystem.ActionTaken = true;
                
        //fix visual bug: clearingCard must be killed after 0.7s delay
        Owner.graveyard.SendToGraveyard(this);
        Owner.battlefield.UpdatePoints();
        clearingCard.Owner.graveyard.SendToGraveyard(clearingCard);
        clearingCard.Owner.battlefield.UpdatePoints();
    }

    void ReturnToHand()
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
