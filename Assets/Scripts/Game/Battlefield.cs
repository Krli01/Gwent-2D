using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    public GameObject Mele;
    public GameObject Range;
    public GameObject Siege;        
    public GameObject weatherZone;
    
    CardSlot[] MeleRow;
    CardSlot[] RangeRow;
    CardSlot[] SiegeRow;
    CardSlot[] WeatherZone;
    
    public TextMeshProUGUI MelePoints;
    public TextMeshProUGUI RangePoints;
    public TextMeshProUGUI SiegePoints;
    public float score;

    void Start()
    {
        MeleRow = Mele.GetComponentsInChildren<CardSlot>();
        RangeRow = Range.GetComponentsInChildren<CardSlot>();
        SiegeRow = Siege.GetComponentsInChildren<CardSlot>();
        WeatherZone = weatherZone.GetComponentsInChildren<CardSlot>();
    }

    public void EnableZones(Role role)
    {
        switch (role)
        {
             case Role.Mele:
                CardSlot[][] MeleZone = new CardSlot[][] {MeleRow};
                Enable(MeleZone, false);
                break;

             case Role.Range:
                CardSlot[][] RangeZone = new CardSlot[][] {RangeRow};
                Enable(RangeZone, false);
                break;

            case Role.Siege:
                CardSlot[][] SiegeZone = new CardSlot[][] {SiegeRow};
                Enable(SiegeZone, false);
                break;

            case Role.Agile:
                CardSlot[][] AgileZones = new CardSlot[][] {MeleRow, RangeRow};
                Enable(AgileZones, false);
                break;

            case Role.Booster:
                CardSlot[][] AllZones = new CardSlot[][] {MeleRow, RangeRow, SiegeRow};
                Enable(AllZones, true);
                break;
            
            case Role.Decoy or Role.Clearing:
                DisableAllZones();
                break;

            case Role.Weather:
                CardSlot[][] weatherZone = new CardSlot[][] {WeatherZone};
                Enable(weatherZone, false);
                break;
        }
    }
    void Enable(CardSlot[][] targetZones, bool booster)
    {
        if (targetZones != null)
        {
            foreach (CardSlot[] zone in targetZones)
            {
                if (booster)
                {
                    foreach (CardSlot slot in zone)
                    {
                        if (slot.isBooster) slot.Available = true;
                        else slot.Available = false;
                    }
                }
                else
                {
                    foreach (CardSlot slot in zone)
                    {
                        if (!slot.isBooster) slot.Available = true;
                        else slot.Available = false;
                    } 
                }
                   
            }
            DisableOtherZones(targetZones, booster);
        }
        else Debug.LogError("targettargetZones is empty");
    } 
    void DisableOtherZones(CardSlot[][] enabledZones, bool booster)
    {
        CardSlot[][] allZones = {MeleRow, RangeRow, SiegeRow, WeatherZone};

        foreach (CardSlot[] zone in allZones)
        {
            if (!enabledZones.Contains(zone))
            {
                if (booster)
                {
                    foreach (CardSlot slot in zone)
                    {
                        if (!slot.isBooster) slot.Available = false;
                    } 
                }
                else
                {
                    foreach (CardSlot slot in zone)
                    {
                        //Debug.Log(slot.name);
                        if (slot.isBooster) slot.Available = false;
                    } 
                }
            }
        }
    }

    public void DisableAllZones()
    {
        CardSlot[][] alltargetZones = new CardSlot[][] {MeleRow, RangeRow, SiegeRow, WeatherZone};

        foreach (CardSlot[] zone in alltargetZones)
        {
            foreach (CardSlot slot in zone)
            {
                //Debug.Log(slot.name);
                slot.Available = false;
            }
        }
        //Debug.Log("All zonesdisabled");
    }

    public void Clear(Graveyard graveyard)
    {
        CardSlot[][] alltargetZones = new CardSlot[][] {MeleRow, RangeRow, SiegeRow, WeatherZone};

        foreach (CardSlot[] zone in alltargetZones)
        {
            foreach (CardSlot slot in zone)
            {
                if (slot.transform.childCount > 0)
                {
                    GameCard c = slot.GetComponentInChildren<GameCard>();
                    graveyard.SendToGraveyard(c);
                }
            }
        }
        UpdatePoints();
    }
    
    public void UpdatePoints()
    {
        float MP = SumRow(MeleRow);
        float RP = SumRow(RangeRow);
        float SP = SumRow(SiegeRow);
        score = MP + RP + SP;
        
        MelePoints.text = MP.ToString();
        RangePoints.text = RP.ToString();
        SiegePoints.text = SP.ToString();
    }

    float SumRow(CardSlot[] row)
    {
        float sum = 0;
        foreach (CardSlot slot in row)
        {
            if (slot.transform.childCount > 0)
            {
                GameCard c = slot.GetComponentInChildren<GameCard>();
                sum += c.Power;
            }
        }
        return sum;
    }
}
