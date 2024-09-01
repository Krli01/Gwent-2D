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
    public bool isSelected;
    public Role role;
    
    //public DisplayCard displayCard;

    public Image CardImage;
    public Image FactionCoat;
    public Image PowerNum;
    public Image Border;
    public Image Role;
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
        CardName = transform.Find("Card Name")?.GetComponent<TextMeshProUGUI>();
        EffectText = transform.Find("Effect Text")?.GetComponent<TextMeshProUGUI>();
        isSelected = false;
    }

    public void Assign (Card baseCard)
    {
        Power = baseCard.Power;
        CardImage.sprite = Resources.Load<Sprite>($"{baseCard.img}");
        FactionCoat.sprite = baseCard.FactionCoat;
        PowerNum.sprite = baseCard.PowerNum;
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
        if(!showBack)
        {
            //this.Border.sprite = 
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter");
        if (transform.parent == TurnSystem.GetActive().thisHand.transform && !isSelected)
        {
            transform.position += popUpOnHover;
        }

        Game.displayCard.Show(BaseCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit");
        if (transform.parent == TurnSystem.GetActive().thisHand.transform && !isSelected)
        {
            transform.position -= popUpOnHover;    
        }

        //Game.displayCard.Reset();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("clicked");
        
        if (transform.parent == TurnSystem.GetActive().thisHand.transform)
        {
            if(Game.Selected.Contains(this)) 
            {
                Game.Selected.Remove(this);
                Game.DisableAllZones(TurnSystem.GetActive());
                //Debug.Log("De-Selected");
            }
            else if (Game.Selected.Count > 0)
            {
                Game.Selected[0].isSelected = false;
                Game.Selected[0].transform.position -= popUpOnHover;            
                Game.Selected.Clear();
                Game.Selected.Add(this);
                Game.EnableZone(TurnSystem.GetActive(), role);
                //Debug.Log("Selected");
            }
            else
            {
                Game.Selected.Add(this);
                Game.EnableZone(TurnSystem.GetActive(), role);
                //Debug.Log("Selected");
            }
        }
        
        isSelected = !isSelected;
        //Debug.Log(isSelected);
    }

}
