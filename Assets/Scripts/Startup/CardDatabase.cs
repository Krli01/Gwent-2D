using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

#region Load
public class CardDatabase : MonoBehaviour
{
    public static Dictionary<string,List<Card>> AvailableDecks = new Dictionary<string, List<Card>>();
    public static Dictionary<string, LeaderCard> FactionLeaders = new Dictionary<string, LeaderCard>();
    public static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
    
    //Visual assets
    public static Dictionary<string, Sprite> factionImages = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> factionCoats = new Dictionary<string, Sprite>();
    public static Dictionary<int, Sprite> powerImages = new Dictionary<int, Sprite>();
    public static Dictionary<Role, Sprite> roleImages = new Dictionary<Role, Sprite>();

    public static CardDatabase Instance { get; private set; }
    public int id = 0;
    
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAssets();
            LoadCards();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadAssets()
    {
        factionImages.Add("Seaborn", Resources.Load<Sprite> ("small/Seaborn"));
        factionImages.Add("Whaler", Resources.Load<Sprite> ("small/Whaler"));
        factionImages.Add("Pirate", Resources.Load<Sprite> ("small/Pirate"));

        factionCoats.Add("Seaborn", Resources.Load<Sprite> ("small/Seaborn_diamond"));
        factionCoats.Add("Whaler", Resources.Load<Sprite> ("small/Whaler_diamond"));
        factionCoats.Add("Pirate", Resources.Load<Sprite> ("small/Pirate_diamond"));
        factionCoats.Add("Neutral", Resources.Load<Sprite> ("small/Neutral_diamond"));

        powerImages.Add(0, Resources.Load<Sprite> ("small/p0"));
        powerImages.Add(1, Resources.Load<Sprite> ("small/p1"));
        powerImages.Add(2, Resources.Load<Sprite> ("small/p2"));
        powerImages.Add(3, Resources.Load<Sprite> ("small/p3"));
        powerImages.Add(4, Resources.Load<Sprite> ("small/p4"));
        powerImages.Add(5, Resources.Load<Sprite> ("small/p5"));
        powerImages.Add(6, Resources.Load<Sprite> ("small/p6"));
        powerImages.Add(7, Resources.Load<Sprite> ("small/p7"));
        powerImages.Add(8, Resources.Load<Sprite> ("small/p8"));
        powerImages.Add(9, Resources.Load<Sprite> ("small/p9"));
        powerImages.Add(10, Resources.Load<Sprite> ("small/p10"));
        powerImages.Add(11, Resources.Load<Sprite> ("small/p11"));
        
        roleImages.Add(Role.Leader, Resources.Load<Sprite> ("small/role leader"));
        roleImages.Add(Role.Mele, Resources.Load<Sprite> ("small/role mele"));
        roleImages.Add(Role.Agile, Resources.Load<Sprite> ("small/role agile"));
        roleImages.Add(Role.Range, Resources.Load<Sprite> ("small/role range"));
        roleImages.Add(Role.Siege, Resources.Load<Sprite> ("small/role siege"));
        roleImages.Add(Role.Booster, Resources.Load<Sprite> ("small/role booster"));
        roleImages.Add(Role.Decoy, Resources.Load<Sprite> ("small/role decoy"));
        roleImages.Add(Role.Weather, Resources.Load<Sprite> ("small/role weather"));
        roleImages.Add(Role.Clearing, Resources.Load<Sprite> ("small/role clearing"));
    }

    void LoadCards()
    {
        List<Card> PirateDeck = new List<Card>();
        List<Card> WhalerDeck = new List<Card>();
        List<Card> SeabornDeck = new List<Card>();
        List<Card> NeutralDeck = new List<Card>();
        
        FactionLeaders.Add("Seaborn", new LeaderCard(id, "Seaborn leader", "this is what this card can do", null, "Seaborn", "img 1"));
        FactionLeaders.Add("Whaler", new LeaderCard(id, "Whaler leader", "this is what this card can do", null, "Whaler", "img 1"));
        FactionLeaders.Add("Pirate", new LeaderCard(id, "Pirate leader", "this is what this card can do", null, "Pirate", "img 1"));
        
        SeabornDeck.Add(new Unit(id++, 9, "name seaborn 1", "this is what this card can do", null, "Seaborn", "img 1", true, Role.Mele));
        SeabornDeck.Add(new Unit(id++, 8, "name seaborn 2", "this is what this card can do", null, "Seaborn", "img 1", true, Role.Siege));
        SeabornDeck.Add(new Unit(id++, 10, "name seaborn 3", "this is what this card can do", null, "Seaborn", "img 1", true, Role.Mele));
        SeabornDeck.Add(new Unit(id++, 7, "name seaborn 4", "this is what this card can do", null, "Seaborn", "img 1", true, Role.Agile));
        SeabornDeck.Add(new Unit(id++, 3, "name seaborn 5", "this is what this card can do", null, "Seaborn", "img 1", false, Role.Siege));
        SeabornDeck.Add(new Unit(id++, 4, "name seaborn 6", "this is what this card can do", null, "Seaborn", "img 1", false, Role.Range));
        SeabornDeck.Add(new Unit(id++, 6, "name seaborn 7", "this is what this card can do", null, "Seaborn", "img 1", false, Role.Agile));
        SeabornDeck.Add(new Unit(id++, 5, "name seaborn 8", "this is what this card can do", null, "Seaborn", "img 1", false, Role.Mele));
        SeabornDeck.Add(new Unit(id++, 2, "name seaborn 9", "this is what this card can do", null, "Seaborn", "img 1", false, Role.Range));
        SeabornDeck.Add(new Unit(id++, 2, "name seaborn 10", "this is what this card can do", null, "Seaborn", "img 1", false, Role.Mele));
        SeabornDeck.Add(new Booster(id++, "name seaborn 11", "this is what this card can do", null, "Seaborn", "img 1"));
        SeabornDeck.Add(new Booster(id++, "name seaborn 12", "this is what this card can do", null, "Seaborn", "img 1"));
        SeabornDeck.Add(new Decoy(id++, "name seaborn 13", "this is what this card can do", null, "Seaborn", "img 1"));
        SeabornDeck.Add(new Decoy(id++, "name seaborn 14", "this is what this card can do", null, "Seaborn", "img 1"));
        SeabornDeck.Add(new Weather(id++, "name seaborn 15", "this is what this card can do", null, "Seaborn", "img 1"));
        SeabornDeck.Add(new Weather(id++, "name seaborn 16", "this is what this card can do", null, "Seaborn", "img 1"));
        SeabornDeck.Add(new Clearing(id++, "name seaborn 17", "this is what this card can do", null, "Seaborn", "img 1"));

        WhalerDeck.Add(new Unit(id++, 9, "name whaler", "this is what this card can do", null, "Whaler", "img 1", true, Role.Mele));
        WhalerDeck.Add(new Unit(id++, 8, "name whaler", "this is what this card can do", null, "Whaler", "img 1", true, Role.Siege));
        WhalerDeck.Add(new Unit(id++, 10, "name whaler", "this is what this card can do", null, "Whaler", "img 1", true, Role.Mele));
        WhalerDeck.Add(new Unit(id++, 7, "name whaler", "this is what this card can do", null, "Whaler", "img 1", true, Role.Agile));
        WhalerDeck.Add(new Unit(id++, 3, "name whaler", "this is what this card can do", null, "Whaler", "img 1", false, Role.Siege));
        WhalerDeck.Add(new Unit(id++, 4, "name whaler", "this is what this card can do", null, "Whaler", "img 1", false, Role.Range));
        WhalerDeck.Add(new Unit(id++, 6, "name whaler", "this is what this card can do", null, "Whaler", "img 1", false, Role.Agile));
        WhalerDeck.Add(new Unit(id++, 5, "name whaler", "this is what this card can do", null, "Whaler", "img 1", false, Role.Mele));
        WhalerDeck.Add(new Unit(id++, 2, "name whaler", "this is what this card can do", null, "Whaler", "img 1", false, Role.Range));
        WhalerDeck.Add(new Unit(id++, 2, "name whaler", "this is what this card can do", null, "Whaler", "img 1", false, Role.Mele));
        WhalerDeck.Add(new Booster(id++, "name whaler", "this is what this card can do", null, "Whaler", "img 1"));
        WhalerDeck.Add(new Booster(id++, "name whaler", "this is what this card can do", null, "Whaler", "img 1"));
        WhalerDeck.Add(new Decoy(id++, "name whaler", "this is what this card can do", null, "Whaler", "img 1"));
        WhalerDeck.Add(new Decoy(id++, "name whaler", "this is what this card can do", null, "Whaler", "img 1"));
        WhalerDeck.Add(new Weather(id++, "name whaler", "this is what this card can do", null, "Whaler", "img 1"));
        WhalerDeck.Add(new Weather(id++, "name whaler", "this is what this card can do", null, "Whaler", "img 1"));
        WhalerDeck.Add(new Clearing(id++, "name whaler", "this is what this card can do", null, "Whaler", "img 1"));

        PirateDeck.Add(new Unit(id++, 9, "name pirate", "this is what this card can do", null, "Pirate", "img 1", true, Role.Mele));
        PirateDeck.Add(new Unit(id++, 8, "name pirate", "this is what this card can do", null, "Pirate", "img 1", true, Role.Siege));
        PirateDeck.Add(new Unit(id++, 10, "name pirate", "this is what this card can do", null, "Pirate", "img 1", true, Role.Mele));
        PirateDeck.Add(new Unit(id++, 7, "name pirate", "this is what this card can do", null, "Pirate", "img 1", true, Role.Agile));
        PirateDeck.Add(new Unit(id++, 3, "name pirate", "this is what this card can do", null, "Pirate", "img 1", false, Role.Siege));
        PirateDeck.Add(new Unit(id++, 4, "name pirate", "this is what this card can do", null, "Pirate", "img 1", false, Role.Range));
        PirateDeck.Add(new Unit(id++, 6, "name pirate", "this is what this card can do", null, "Pirate", "img 1", false, Role.Agile));
        PirateDeck.Add(new Unit(id++, 5, "name pirate", "this is what this card can do", null, "Pirate", "img 1", false, Role.Mele));
        PirateDeck.Add(new Unit(id++, 2, "name pirate", "this is what this card can do", null, "Pirate", "img 1", false, Role.Range));
        PirateDeck.Add(new Unit(id++, 2, "name pirate", "this is what this card can do", null, "Pirate", "img 1", false, Role.Mele));
        PirateDeck.Add(new Booster(id++, "name pirate", "this is what this card can do", null, "Pirate", "img 1"));
        PirateDeck.Add(new Booster(id++, "name pirate", "this is what this card can do", null, "Pirate", "img 1"));
        PirateDeck.Add(new Decoy(id++, "name pirate", "this is what this card can do", null, "Pirate", "img 1"));
        PirateDeck.Add(new Decoy(id++, "name pirate", "this is what this card can do", null, "Pirate", "img 1"));
        PirateDeck.Add(new Weather(id++, "name pirate", "this is what this card can do", null, "Pirate", "img 1"));
        PirateDeck.Add(new Weather(id++, "name pirate", "this is what this card can do", null, "Pirate", "img 1"));
        PirateDeck.Add(new Clearing(id++, "name pirate", "this is what this card can do", null, "Pirate", "img 1"));

        NeutralDeck.Add(new Unit(id++, 9, "name neutral", "this is what this card can do", null, "Neutral", "img 1", true, Role.Range));
        NeutralDeck.Add(new Unit(id++, 7, "name neutral", "this is what this card can do", null, "Neutral", "img 1", true, Role.Mele));
        NeutralDeck.Add(new Unit(id++, 6, "name neutral", "this is what this card can do", null, "Neutral", "img 1", false, Role.Mele));
        NeutralDeck.Add(new Unit(id++, 4, "name neutral", "this is what this card can do", null, "Neutral", "img 1", false, Role.Siege));
        NeutralDeck.Add(new Unit(id++, 3, "name neutral", "this is what this card can do", null, "Neutral", "img 1", false, Role.Range));
        NeutralDeck.Add(new Unit(id++, 2, "name neutral", "this is what this card can do", null, "Neutral", "img 1", false, Role.Range));
        NeutralDeck.Add(new Decoy(id++, "name neutral", "this is what this card can do", null, "Neutral", "img 1"));
        NeutralDeck.Add(new Decoy(id++, "name neutral", "this is what this card can do", null, "Neutral", "img 1"));
        NeutralDeck.Add(new Weather(id++, "name neutral", "this is what this card can do", null, "Neutral", "img 1"));
        NeutralDeck.Add(new Clearing(id++, "name neutral", "this is what this card can do", null, "Neutral", "img 1"));
        NeutralDeck.Add(new Booster(id++, "name neutral", "this is what this card can do", null, "Neutral", "img 1"));
        NeutralDeck.Add(new Booster(id++, "name neutral", "this is what this card can do", null, "Neutral", "img 1"));

        AvailableDecks.Add("Seaborn", SeabornDeck);
        AvailableDecks.Add("Whaler", WhalerDeck);
        AvailableDecks.Add("Pirate", PirateDeck);
        AvailableDecks.Add("Neutral", NeutralDeck);
        //foreach (var item in AvailableDecks) Debug.Log($"{item.Key} deck available");
    }
}
#endregion

#region Cards
public abstract class Card
{
    public int ID;
    public float Power;
    public string CardName;
    public string CardFaction;
    public string EffectText; //texto del efecto vs efecto en codigo
    public List<Effect> CardEffect;
    public Role thisRole;
    public int instancesLeft {get; set;}

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
    public bool Activated {get; set;}
    public LeaderCard(int id, string cardName, string effectText, List<Effect> cardEffect, string faction, string image)
    {
        //Functional assignment
        ID = id;
        Power = 0;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        img = image;
        thisRole = Role.Leader;
        Activated = false;
        
        //Visuals assignment
        intPower = 0;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[Role.Leader];
        FactionCoat = CardDatabase.factionCoats[faction];
        PowerNum = CardDatabase.factionImages[faction];
        Border = Resources.Load<Sprite> ("small/Gold");
    }

    public override void Activate()
    {
        Debug.Log("Activated leader");
    }
}

public class Unit : Card
{
    public bool Gold;

    public Unit (int id, int power, string cardName, string effectText, List<Effect> cardEffect, string faction, string image, bool gold, Role role)
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
        instancesLeft = Gold ? 1 : 3;

        //Visuals assignment
        img = image;
        intPower = power;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[role];
        FactionCoat = CardDatabase.factionCoats[CardFaction];
        PowerNum = CardDatabase.powerImages[power];
        Border = gold ? Resources.Load <Sprite>("small/Gold") : Resources.Load <Sprite>("small/Silver");

    }

     public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}

public class Decoy : Card
{
    public Decoy (int id, string cardName, string effectText, List<Effect> cardEffect, string faction, string image)
    {
        //Functional assignment
        ID = id;
        Power = 0;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        img = image;
        thisRole = Role.Decoy;
        instancesLeft = 2;

        //Visuals assignment
        intPower = 0;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[thisRole];
        FactionCoat = CardDatabase.factionCoats[CardFaction];
        PowerNum = CardDatabase.powerImages[0];
        Border = Resources.Load<Sprite> ("small/Bronze");
    }

     public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}

public class Booster : Card
{
    public Booster (int id, string cardName, string effectText, List<Effect> cardEffect, string faction, string image)
    {
        //Functional assignment
        ID = id;
        Power = 1;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        img = image;
        thisRole = Role.Booster;
        instancesLeft = 1;
        
        //Visuals assignment
        intPower = 1;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[thisRole];
        FactionCoat = CardDatabase.factionCoats[faction];
        PowerNum = CardDatabase.powerImages[1];
        Border = Resources.Load<Sprite> ("small/Bronze");
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
    public Weather(int id, string cardName, string effectText, List<Effect> cardEffect, string faction, string image)
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
        instancesLeft = 1;
        
        //Visuals assignment
        intPower = 0;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[thisRole];
        FactionCoat = CardDatabase.factionCoats[faction];
        PowerNum = null;
        Border = Resources.Load<Sprite> ("small/Bronze");
    }
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

}

public class Clearing : Card
{
    public Clearing(int id, string cardName, string effectText, List<Effect> cardEffect, string faction, string image)
    {
        //Functional assignment
        ID = id;
        Power = 0;
        CardName = cardName;
        EffectText = effectText;
        CardEffect = cardEffect;
        CardFaction = faction;
        img = image;
        thisRole = Role.Clearing;
        instancesLeft = 1;
        
        //Visuals assignment
        intPower = 0;
        CardImage = Resources.Load<Sprite>(image);
        CardRole = CardDatabase.roleImages[thisRole];
        FactionCoat = CardDatabase.factionCoats[faction];
        PowerNum = null;
        Border = Resources.Load<Sprite> ("small/Bronze");
    }
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

}
#endregion

#region  Effects

public class Effect
{
    public string Name {get; private set;}
    public Dictionary <string, string> Params {get; set;}
    AST_Action Action {get; set;}

    public Effect (string name, Dictionary <string, string> _params, AST_Action action)
    {
        Name = name;
        Params = _params;
        Action = action;
    }

    public void Activate()
    {
        
    }
}

#endregion