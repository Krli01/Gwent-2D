using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    public int ID;
    public int Power;
    public string CardName;
    public Position CardPosition;
    public string CardEffect;
    public string CardFaction;

    public Sprite CardImage;

    public Card()
    {

    }

    public Card(int id, int power, string cardName, Position position, string cardEffect, string faction, Sprite image)
    {
        ID = id;
        Power = power;
        CardName = cardName;
        CardPosition = position;
        CardEffect = cardEffect;
        CardFaction = faction;
        CardImage = image;
    }

    public void Summon()
    {

    }

    public void Destroy()
    {
        
    }

}

/*public enum Faction
{
    Pirate,
    Seaborn,
    Whaler,
    None
}*/

public enum Position
{
    Melee,
    Range,
    Siege,
    Agile,
    Weather,
    Horn,
    Leader
}

/*public class LeaderCard : Card
{
    public LeaderCard(int id, int power, string cardName, Position position, string cardEffect, Faction faction, Sprite image)
    {
        ID = id;
        Power = power;
        CardName = cardName;
        CardPosition = Position.Leader;
        CardEffect = cardEffect;
        CardFaction = faction;
        CardImage = image;
    }
}

public class UnitCard : Card
{
    public bool Gold;
    public UnitCard(int id, int power, string cardName, Position position, string cardEffect, Faction faction, Sprite image, bool gold)
    {
        ID = id;
        Power = power;
        CardName = cardName;
        CardPosition = position;
        CardEffect = cardEffect;
        CardFaction = faction;
        CardImage = image;
        Gold = gold;
    }
}*/