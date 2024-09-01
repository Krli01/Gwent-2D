using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class DisplayCard : MonoBehaviour
{
    public Image CardImage;
    public Image FactionCoat;
    public Image PowerNum;
    public Image Border;
    public Image Role;
    public TextMeshProUGUI CardName;
    public TextMeshProUGUI EffectText;
    private static Dictionary<string, Sprite> factionImages = new Dictionary<string, Sprite>();
    private static Dictionary<int, Sprite> powerImages = new Dictionary<int, Sprite>();
    private static Dictionary<Role, Sprite> roleImages = new Dictionary<Role, Sprite>();
    private static Dictionary<Sprite, Sprite> borderImages = new Dictionary<Sprite, Sprite>();
    
    void Awake()
    {
        CardImage = transform.Find("CardImage")?.GetComponent<Image>();
        FactionCoat = transform.Find("Faction")?.GetComponent<Image>();
        PowerNum = transform.Find("Power")?.GetComponent<Image>();
        Border = transform.Find("Border")?.GetComponent<Image>();
        Role = transform.Find("Role")?.GetComponent<Image>();
        CardName = transform.Find("Card Name")?.GetComponent<TextMeshProUGUI>();
        EffectText = transform.Find("Effect Text")?.GetComponent<TextMeshProUGUI>();

        roleImages.Add(global::Role.Mele, Resources.Load<Sprite>("DisplayCard/role mele"));
        roleImages.Add(global::Role.Range, Resources.Load<Sprite>("DisplayCard/role range"));
        roleImages.Add(global::Role.Siege, Resources.Load<Sprite>("DisplayCard/role siege"));
        roleImages.Add(global::Role.Agile, Resources.Load<Sprite>("DisplayCard/role agile"));
        roleImages.Add(global::Role.Weather, Resources.Load<Sprite>("DisplayCard/role weather"));
        roleImages.Add(global::Role.Clearing, Resources.Load<Sprite>("DisplayCard/role clearing"));
        roleImages.Add(global::Role.Booster, Resources.Load<Sprite>("DisplayCard/role booster"));
        roleImages.Add(global::Role.Decoy, Resources.Load<Sprite>("DisplayCard/role decoy"));

        factionImages.Add("Seaborn", Resources.Load<Sprite>("DisplayCard/Seaborn_diamond"));
        factionImages.Add("Whaler", Resources.Load<Sprite>("DisplayCard/Whaler_diamond"));
        factionImages.Add("Pirate", Resources.Load<Sprite>("DisplayCard/Pirate_diamond"));
        factionImages.Add("Neutral", Resources.Load<Sprite>("DisplayCard/Neutral_diamond"));

        powerImages.Add(0, Resources.Load<Sprite>("DisplayCard/p0"));
        powerImages.Add(1, Resources.Load<Sprite>("DisplayCard/p1"));
        powerImages.Add(2, Resources.Load<Sprite>("DisplayCard/p2"));
        powerImages.Add(3, Resources.Load<Sprite>("DisplayCard/p3"));
        powerImages.Add(4, Resources.Load<Sprite>("DisplayCard/p4"));
        powerImages.Add(5, Resources.Load<Sprite>("DisplayCard/p5"));
        powerImages.Add(6, Resources.Load<Sprite>("DisplayCard/p6"));
        powerImages.Add(7, Resources.Load<Sprite>("DisplayCard/p7"));
        powerImages.Add(8, Resources.Load<Sprite>("DisplayCard/p8"));
        powerImages.Add(9, Resources.Load<Sprite>("DisplayCard/p9"));
        powerImages.Add(10, Resources.Load<Sprite>("DisplayCard/p10"));
        powerImages.Add(11, Resources.Load<Sprite>("DisplayCard/p11"));
        
        borderImages.Add(Resources.Load <Sprite>("Gold"), Resources.Load <Sprite>("DisplayCard/Gold"));
        borderImages.Add(Resources.Load <Sprite>("Silver"), Resources.Load <Sprite>("DisplayCard/Silver"));
        borderImages.Add(Resources.Load <Sprite>("Bronze"), Resources.Load <Sprite>("DisplayCard/Bronze"));
    }
    public void Show(Card baseCard)
    {
        CardImage.sprite = Resources.Load<Sprite>($"DisplayCard/{baseCard.img}");
        FactionCoat.sprite = factionImages[baseCard.CardFaction];
        PowerNum.sprite = powerImages[baseCard.intPower];
        Border.sprite = borderImages[baseCard.Border];
        Role.sprite = roleImages[baseCard.thisRole];
        CardName.text = baseCard.CardName;
        EffectText.text = baseCard.EffectText;
    }

    public void Reset()
    {
    CardImage = null;
    FactionCoat = null;
    PowerNum = null;
    Border = null;
    Role = null;
    CardName = null;
    EffectText = null;
    }

}
