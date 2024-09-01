using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public abstract class Card
{
    public int ID;
    public float Power;
    public string CardName;
    public string CardFaction;
    public string EffectText; //texto del efecto vs efecto en codigo
    public string CardEffect;
    public Role thisRole;

    // Visual fields
    public int intPower;
    public string img;
    public Sprite CardImage;
    public Sprite CardRole;
    public Sprite FactionCoat;
    public Sprite PowerNum;
    public Sprite Border;

    // Effect activation method
    public abstract void Activate();
}

public class LeaderCard : Card
{
    public LeaderCard(int id, float power, string cardName, string effectText, string cardEffect, string faction, string image)
    {
        ID = id;
        Power = power;
        CardName = cardName;
        CardFaction = faction;
        EffectText = effectText;
        CardEffect = cardEffect;
        thisRole = Role.Leader;
        
        CardImage = Resources.Load<Sprite>(image);
        FactionCoat = CardDatabase.factionImages[faction];

        Border = Resources.Load <Sprite>("Gold");
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}

public class Unit : Card
{
    public bool Gold;
    public Unit (int id, int power, string cardName, string effectText, string cardEffect, string faction, string image, bool gold, Role role)
    {
        //Functional assignment
        ID = id;
        Power = power;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        Gold = gold;
        thisRole = role;

        //Visuals assignment
        intPower = power;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[role];
        FactionCoat = CardDatabase.factionImages[CardFaction];
        PowerNum = CardDatabase.powerImages[power];
        Border = gold ? Resources.Load <Sprite>("Gold") : Resources.Load <Sprite>("Silver");

    }

     public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}

public class Decoy : Card
{
    public Decoy (int id, string cardName, string effectText, string cardEffect, string faction, string image)
    {
        //Functional assignment
        ID = id;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        img = image;

        //Visuals assignment
        intPower = 0;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[Role.Decoy];
        FactionCoat = CardDatabase.factionImages[CardFaction];
        PowerNum = CardDatabase.powerImages[0];
        Border = Resources.Load<Sprite> ("Bronze");
    }

     public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}

public class Booster : Card
{
    public Booster (int id, string cardName, string effectText, string cardEffect, string faction, string image)
    {
        //Functional assignment
        ID = id;
        Power = 0;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        img = image;
        thisRole = Role.Booster;
        
        //Visuals assignment
        intPower = 0;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[thisRole];
        FactionCoat = CardDatabase.factionImages[faction];
        PowerNum = CardDatabase.powerImages[0];
        Border = Resources.Load<Sprite> ("Bronze");
    }

     public override void Activate()
    {
        throw new System.NotImplementedException();
    }

}

public enum Role
{
    Leader,
    Mele,
    Range,
    Siege,
    Agile, // Mele + Range
    Booster,
    Decoy,
    Weather,
    Clearing
}

public class Weather : Card
{
    public Weather(int id, string cardName, string effectText, string cardEffect, string faction, string image)
    {
        //Functional assignment
        ID = id;
        Power = 0;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        img = image;
        thisRole = Role.Weather;
        
        //Visuals assignment
        intPower = 0;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[thisRole];
        FactionCoat = CardDatabase.factionImages[faction];
        PowerNum = CardDatabase.powerImages[0];
        Border = Resources.Load<Sprite> ("Bronze");
    }
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

}

public class Clearing : Card
{
    public Clearing(string image)
    {
        thisRole = Role.Clearing;
        img = image;
    }
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

}