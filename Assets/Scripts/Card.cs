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
    public Sprite Role;
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
        if (faction == null) throw new System.Exception ("Leader must belong to a faction");
        //else if () si la faccion no existe lanzar error. Desea crearla?
        else CardFaction = faction;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardImage = image;
        Role = null;
        //FactionCoat = poner si faccion existe
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
    public Position[] CardPosition;
    public Unit (int id, int power, string cardName, string effectText, string cardEffect, string faction, Sprite image, bool gold, Position[] position)
    {
        ID = id;
        Power = power;
        CardName = cardName;
        if (faction == null) throw new System.Exception ("Leader must belong to a faction");
        //else if () si la faccion no existe lanzar error. Desea crearla?
        else CardFaction = faction;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardImage = image;
        Role = null;
        //FactionCoat = poner si faccion existe
        Gold = gold;
        CardPosition = position;
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
    public Position[] CardPosition;
    public Decoy (int id, string cardName, string effectText, string cardEffect, string faction, Sprite image, Position[] position)
    {
        ID = id;
        Power = 0;
        CardName = cardName;
        if (faction == null) throw new System.Exception ("Leader must belong to a faction");
        //else if () si la faccion no existe lanzar error. Desea crearla?
        else CardFaction = faction;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardImage = image;
        Role = null;
        //FactionCoat = poner si faccion existe
        CardPosition = position;
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
    public Position[] CardPosition;
    public Booster (int id, string cardName, string effectText, string cardEffect, string faction, Sprite image, Position[] position)
    {
        ID = id;
        Power = 0;
        CardName = cardName;
        if (faction == null) throw new System.Exception ("Leader must belong to a faction");
        //else if () si la faccion no existe lanzar error. Desea crearla?
        else CardFaction = faction;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardImage = image;
        Role = null;
        //FactionCoat = poner si faccion existe
        CardPosition = position;
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

public enum Position
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