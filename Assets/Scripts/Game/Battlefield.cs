using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Battlefield
{
    public CardSlot[] MeleRow;
    public CardSlot[] RangeRow;
    public CardSlot[] SiegeRow;
    
    GameObject RowCount;
    TextMeshProUGUI MelePoints;
    TextMeshProUGUI RangePoints;
    TextMeshProUGUI SiegePoints;
    public float score;

    public Battlefield(string player, GameObject rowCount)
    {
        GameObject Mele = GameObject.Find($"{player} Mele");
        GameObject Range = GameObject.Find($"{player} Range");
        GameObject Siege = GameObject.Find($"{player} Siege");

        MeleRow = Mele.GetComponentsInChildren<CardSlot>();
        RangeRow = Range.GetComponentsInChildren<CardSlot>();
        SiegeRow = Siege.GetComponentsInChildren<CardSlot>();

        RowCount = rowCount;
        TextMeshProUGUI[] counts = rowCount.GetComponentsInChildren<TextMeshProUGUI>();
        MelePoints = System.Array.Find(counts, c => c.name == "Mele");
        RangePoints = System.Array.Find(counts, c => c.name == "Range");
        SiegePoints = System.Array.Find(counts, c => c.name == "Siege");
    }

    //MOVE ENABLES HERE
    public void EnableZone(Role role)
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
                CardSlot[][] WeatherZone = new CardSlot[][] {Game.WeatherZone};
                Enable(WeatherZone, false);
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
        CardSlot[][] allZones = {MeleRow, RangeRow, SiegeRow, Game.WeatherZone};

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
                        if (slot.isBooster) slot.Available = false;
                    } 
                }
            }
        }
    }

    public void DisableAllZones()
    {
        CardSlot[][] alltargetZones = new CardSlot[][] {MeleRow, RangeRow, SiegeRow, Game.WeatherZone};

        foreach (CardSlot[] zone in alltargetZones)
        {
            foreach (CardSlot slot in zone)
            {
                slot.Available = false;
            }
        }
        Debug.Log("All zonesdisabled");
    }

    public void Clear()
    {
        CardSlot[][] alltargetZones = new CardSlot[][] {MeleRow, RangeRow, SiegeRow, Game.WeatherZone};

        foreach (CardSlot[] zone in alltargetZones)
        {
            foreach (CardSlot slot in zone)
            {
                if (slot.transform.childCount > 0)
                {
                    GameCard c = slot.GetComponentInChildren<GameCard>();
                    //c.SendToGraveyard();
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
