using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public abstract class Card
{
    // Functional fields
    public int ID;
    public float Power;
    public string CardName;
    public string CardFaction;
    public string EffectText; //texto del efecto vs efecto en codigo
    public string CardEffect;

    // Visual fields
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
    public LeaderCard(int id, float power, string cardName, string effectText, string cardEffect, string faction, Sprite image)
    {
        ID = id;
        Power = power;
        CardName = cardName;
        CardFaction = faction;
        EffectText = effectText;
        CardEffect = cardEffect;
        
        
        CardImage = image;
        FactionCoat = CardDatabase.factionImages[faction];

        Border = Resources.Load <Sprite>("Gold");
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}

public abstract class PlayableCard : Card
{
    public abstract void Summon();
    public abstract void Kill();
}

public class Unit : PlayableCard
{
    public bool Gold;
    public Role thisRole;
    public Unit (int id, int power, string cardName, string effectText, string cardEffect, string faction, Sprite image, bool gold, Role role)
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
        CardImage = image;
        CardRole = CardDatabase.roleImages[role];
        FactionCoat = CardDatabase.factionImages[CardFaction];
        PowerNum = CardDatabase.powerImages[power];
        Border = gold ? Resources.Load <Sprite>("Gold") : Resources.Load <Sprite>("Silver");

    }

     public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Kill()
    {
        throw new System.NotImplementedException();
    }

    public override void Summon()
    {
        throw new System.NotImplementedException();
    }
}

public class Decoy : PlayableCard
{
    public Decoy (int id, string cardName, string effectText, string cardEffect, string faction, Sprite image)
    {
        ID = id;
        Power = 0;
        CardName = cardName;
        if (faction == null) throw new System.Exception ("Card must belong to a faction");
        //else if () si la faccion no existe lanzar error. Desea crearla?
        else CardFaction = faction;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardImage = image;
        //FactionCoat = poner si faccion existe
    }

     public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Kill()
    {
        throw new System.NotImplementedException();
    }

    public override void Summon()
    {
        throw new System.NotImplementedException();
    }
}

public class Booster : PlayableCard
{
    public Booster (int id, string cardName, string effectText, string cardEffect, string faction, Sprite image, Role role)
    {
        //Functional assignment
        ID = id;
        Power = 0;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        
        //Visuals assignment
        CardImage = image;
        CardRole = Resources.Load<Sprite> ("role mele");
        FactionCoat = CardDatabase.factionImages[faction];
        PowerNum = CardDatabase.powerImages[0];
        Border = Resources.Load<Sprite> ("Bronze");
    }

     public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Kill()
    {
        throw new System.NotImplementedException();
    }

    public override void Summon()
    {
        throw new System.NotImplementedException();
    }
}

public enum Role
{
    Melee,
    Range,
    Siege,
    Agile // Melee + Range
}

public class Weather : PlayableCard
{
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Kill()
    {
        throw new System.NotImplementedException();
    }

    public override void Summon()
    {
        throw new System.NotImplementedException();
    }
}

public class Clearing : PlayableCard
{
     public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Kill()
    {
        throw new System.NotImplementedException();
    }

    public override void Summon()
    {
        throw new System.NotImplementedException();
    }
}