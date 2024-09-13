using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using Unity.Mathematics;

public class DisplayCard : MonoBehaviour
{
    public Image CardImage;
    public Image FactionCoat;
    public Image PowerNum;
    public Image PowerExtra;
    public Image Border;
    public Image Role;
    public Image Base;
    public TextMeshProUGUI CardName;
    public TextMeshProUGUI EffectText;
    public TextMeshProUGUI powerExtra;

    
    public static DisplayCard Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = FindObjectOfType<DisplayCard>();
    }
    
    void Start()
    {
        Reset();
    }

    public void Show(Card baseCard)
    {
        CardImage.enabled = true;
        FactionCoat.enabled = true;
        if(baseCard.PowerNum != null) PowerNum.enabled = true;
        Border.enabled = true;
        Role.enabled = true;
        Base.enabled = true;
        if(powerExtra.text != "") PowerExtra.enabled = true;

        CardImage.sprite = Resources.Load<Sprite>($"big/{baseCard.img}");
        FactionCoat.sprite = DisplayCardDatabase.factionCoats[baseCard.CardFaction];
        PowerNum.sprite = baseCard.thisRole == global::Role.Leader ? 
            DisplayCardDatabase.factionImages[baseCard.CardFaction] : DisplayCardDatabase.powerImages[baseCard.intPower];
        Border.sprite = DisplayCardDatabase.borderImages[baseCard.Border];
        Role.sprite = DisplayCardDatabase.roleImages[baseCard.thisRole];
        CardName.text = baseCard.CardName;
        EffectText.text = baseCard.EffectText;
    }

    public void Reset()
    {
        CardImage.enabled = false;
        FactionCoat.enabled = false;
        PowerNum.enabled = false;
        Border.enabled = false;
        Role.enabled = false;
        Base.enabled = false;
        PowerExtra.enabled = false;
        CardName.text = "";
        EffectText.text = "";
        powerExtra.text = "";
    }

}