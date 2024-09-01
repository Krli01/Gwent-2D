using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Player Player1;
    public static Player Player2;
    public static CardSlot[] WeatherZone;
    public static DisplayCard displayCard;
    
    public static List<GameCard> Selected = new List<GameCard>();
    
    void Start()
    {
        //Debug.Log("started");
        GameObject weatherZone = GameObject.Find("WeatherZone");
        displayCard = FindObjectOfType<DisplayCard>();
        WeatherZone = weatherZone.GetComponentsInChildren<CardSlot>();
        Player1 = new Player("Player 1", "Seaborn", "P1");
        Player2 = new Player("Player 2", "Whaler", "P2");
        StartCoroutine(Player1.Deck.DrawCards(Player1.thisHand, 2));
        StartCoroutine(Player2.Deck.DrawCards(Player2.thisHand, 1));
    }

    public static void EnableZone(Player player, Role role)
    {
        CardSlot[][] targetZones = null;

        switch (role)
        {
             case Role.Mele:
                targetZones = new CardSlot[][] {player.battlefield.MeleRow};
                Enable(targetZones, false);
                break;

             case Role.Range:
                targetZones = new CardSlot[][] {player.battlefield.RangeRow};
                Enable(targetZones, false);
                break;

            case Role.Siege:
                targetZones = new CardSlot[][] {player.battlefield.SiegeRow};
                Enable(targetZones, false);
                break;

            case Role.Agile:
                targetZones = new CardSlot[][] {player.battlefield.MeleRow, player.battlefield.RangeRow};
                Enable(targetZones, false);
                break;

            case Role.Booster:
                targetZones = new CardSlot[][] {player.battlefield.MeleRow, player.battlefield.RangeRow, player.battlefield.SiegeRow};
                Enable(targetZones, true);
                break;
            
            case Role.Decoy:
                targetZones = new CardSlot[][] {player.battlefield.MeleRow, player.battlefield.RangeRow, player.battlefield.SiegeRow};
                Enable(targetZones, false);
                break;

            case Role.Weather or Role.Clearing:
                targetZones = new CardSlot[][] {WeatherZone};
                Enable(targetZones, false);
                break;
        }

        //DisableOthertargetZones(player, targetZones);
    }

    static void Enable(CardSlot[][] targetZones, bool booster)
    {
        if (targetZones != null)
        {
            foreach (CardSlot[] zone in targetZones)
            {
                if (booster)
                {
                    foreach (CardSlot slot in zone)
                    {
                        if (slot.isBooster) 
                        {
                            slot.Available = true;
                            //Debug.Log($"{slot} enabled");
                        }
                    } 
                }
                else
                {
                    foreach (CardSlot slot in zone)
                    {
                        if (!slot.isBooster) 
                        {
                            slot.Available = true;
                            //Debug.Log($"{slot} enabled");
                        }
                    } 
                }
                   
            }
        }
        else Debug.LogWarning("targettargetZones is empty");
    } 

    public static void DisableAllZones(Player player)
    {
        CardSlot[][] alltargetZones = {player.battlefield.MeleRow, player.battlefield.RangeRow, player.battlefield.SiegeRow, WeatherZone};

        foreach (CardSlot[] zone in alltargetZones)
        {
            foreach (CardSlot slot in zone)
            {
                slot.Available = false;
            }
        }
        Debug.Log("All zonesdisabled");
    }

}
