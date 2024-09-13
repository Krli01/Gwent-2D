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
    bool isSelected;
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
        TurnSystem.Instance.Active.ActivateLeader.interactable = false;
        TurnSystem.Instance.TakeAction();
        DeSelect();
    } 

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter");
        if (transform.parent == TurnSystem.Instance.Active.thisHand.transform && !isSelected)
        {
            transform.position += popUpOnHover;
        }

        if(!showBack) DisplayCard.Instance.Show(BaseCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit");
        if (transform.parent == TurnSystem.Instance.Active.thisHand.transform && !isSelected)
        {
            transform.position -= popUpOnHover;    
        }

        if (!isSelected) 
        {
            if(Game.Selected.Count == 0) DisplayCard.Instance.Reset();
            else DisplayCard.Instance.Show(Game.Selected[0].BaseCard);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (showBack) return;
        
        if (Game.drawPhase)
        {
            if (Game.Selected.Contains(this))
            {
                Game.Selected.Remove(this);
                isSelected = false;
                if (Game.Selected.Count == 0)
                {
                    Button Discard = GameObject.Find("End Phase")?.GetComponent<Button>();
                    TextMeshProUGUI Text = Discard.GetComponentInChildren<TextMeshProUGUI>();
                    Text.text = "Continuar";
                }
            }
            else if (transform.parent == TurnSystem.Instance.Active.thisHand.transform && Game.Selected.Count < 2)
            {
                Game.Selected.Add(this);
                isSelected = true;
                if (Game.Selected.Count == 1)
                {
                    Button Discard = GameObject.Find("End Phase")?.GetComponent<Button>();
                    TextMeshProUGUI Text = Discard.GetComponentInChildren<TextMeshProUGUI>();
                    Text.text = "Descartar";
                }
            }
        }

        else if (role == global::Role.Leader)
        {
            if (Game.Selected.Contains(this))
            {
                DeSelect();
                Owner.HideLeaderButton();
            } 
            else 
            {
                Select();
                if (TurnSystem.Instance.Active == Owner) Owner.ShowLeaderButton();
                if(TurnSystem.Instance.ActionTaken) Owner.ActivateLeader.interactable = false;
            }
        }
        
        else if (transform.parent == TurnSystem.Instance.Active.thisHand.transform)
        {
            if(Game.Selected.Contains(this))
            {
                DeSelect();
                Owner.battlefield.DisableAllZones();
            }
            else
            {
                Select();
                if (!TurnSystem.Instance.ActionTaken) Owner.battlefield.EnableZones(role);
            }
        }

        else if (Game.Selected.Count > 0)
        {
            if(TurnSystem.Instance.ActionTaken)
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
        if (Game.Selected.Contains(this)) DeSelect();
        else Select();
    }
    void Select()
    {
        Debug.Log(TurnSystem.Instance.ActionTaken);
        // only 1 card can be selected
        if (Game.Selected.Count > 0)
        {
            for (int i = 0; i < Game.Selected.Count; i++)
            {
                GameCard c = Game.Selected[i];
                c.DeSelect();
            }
        }
        
        isSelected = true;
        Game.Selected.Add(this);
        Debug.Log("Selected");
    }

    public void DeSelect()
    {
        isSelected = false;
        if (transform.parent == TurnSystem.Instance.Active.thisHand.transform) transform.position -= popUpOnHover;
        Game.Selected.Remove(this);
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
        TurnSystem.Instance.TakeAction();
                
        //fix visual bug: clearingCard must be killed after 0.7s delay
        Owner.graveyard.SendToGraveyard(this);
        Owner.battlefield.UpdatePoints();
        clearingCard.Owner.graveyard.SendToGraveyard(clearingCard);
        clearingCard.Owner.battlefield.UpdatePoints();
    }

    void ReturnToHand()
    {
        Transform hand = TurnSystem.Instance.Active.thisHand.transform;
        Transform t = transform.parent;
        transform.SetParent(hand);
        GameCard card = Game.Selected[0];
        card.transform.SetParent(t);
        card.isSelected = false;                    
        card.transform.localPosition = Vector3.zero;            
        Game.Selected.Clear();
        TurnSystem.Instance.TakeAction();
    }

    public void ResetLeader()
    {
        if (role == global::Role.Leader)
        {
            BaseCard = null;
        }
    }
}