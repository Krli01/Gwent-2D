using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static Dictionary<string,List<Card>> AvailableDecks = new Dictionary<string, List<Card>>();
    public static List<Card> PirateDeck = new List<Card>();
    public static List<Card> WhalerDeck = new List<Card>();
    public static List<Card> SeabornDeck = new List<Card>();
    public static List<Card> NeutralDeck = new List<Card>();
    
    public static Dictionary<string, Sprite> factionImages = new Dictionary<string, Sprite>();
    public static Dictionary<int, Sprite> powerImages = new Dictionary<int, Sprite>();
    public static Dictionary<Role, Sprite> roleImages = new Dictionary<Role, Sprite>();


    private void Awake() 
    {
        int id = 0;
        
        //Preparing assets
        
        factionImages.Add("Seaborn", Resources.Load<Sprite> ("Seaborn_diamond"));
        factionImages.Add("Whaler", Resources.Load<Sprite> ("Whaler_diamond"));
        factionImages.Add("Pirate", Resources.Load<Sprite> ("Pirate_diamond"));
        factionImages.Add("Neutral", Resources.Load<Sprite> ("Neutral_diamond"));

        powerImages.Add(0, Resources.Load<Sprite> ("p0"));
        powerImages.Add(1, Resources.Load<Sprite> ("p1"));
        powerImages.Add(2, Resources.Load<Sprite> ("p2"));
        powerImages.Add(3, Resources.Load<Sprite> ("p3"));
        powerImages.Add(4, Resources.Load<Sprite> ("p4"));
        powerImages.Add(5, Resources.Load<Sprite> ("p5"));
        powerImages.Add(6, Resources.Load<Sprite> ("p6"));
        powerImages.Add(7, Resources.Load<Sprite> ("p7"));
        powerImages.Add(8, Resources.Load<Sprite> ("p8"));
        powerImages.Add(9, Resources.Load<Sprite> ("p9"));
        powerImages.Add(10, Resources.Load<Sprite> ("p10"));
        powerImages.Add(11, Resources.Load<Sprite> ("p11"));
        
        roleImages.Add(Role.Mele, Resources.Load<Sprite> ("role mele"));
        roleImages.Add(Role.Agile, Resources.Load<Sprite> ("role agile"));
        roleImages.Add(Role.Range, Resources.Load<Sprite> ("role range"));
        roleImages.Add(Role.Siege, Resources.Load<Sprite> ("role siege"));
        roleImages.Add(Role.Booster, Resources.Load<Sprite> ("role booster"));
        roleImages.Add(Role.Decoy, Resources.Load<Sprite> ("role decoy"));
        roleImages.Add(Role.Weather, Resources.Load<Sprite> ("role weather"));
        roleImages.Add(Role.Clearing, Resources.Load<Sprite> ("role clearing"));
        roleImages.Add(Role.Leader, Resources.Load<Sprite> ("role siege"));
        
        //Loading cards
        
        SeabornDeck.Add(new Unit(id, 9, "name seaborn 1", "this is what this card can do", "eff", "Seaborn", "img 1", true, Role.Mele));
        SeabornDeck.Add(new Unit(id++, 8, "name seaborn 2", "this is what this card can do", "eff", "Seaborn", "img 1", true, Role.Siege));
        SeabornDeck.Add(new Unit(id++, 10, "name seaborn 3", "this is what this card can do", "eff", "Seaborn", "img 1", true, Role.Mele));
        SeabornDeck.Add(new Unit(id++, 7, "name seaborn 4", "this is what this card can do", "eff", "Seaborn", "img 1", true, Role.Agile));
        SeabornDeck.Add(new Unit(id++, 3, "name seaborn 5", "this is what this card can do", "eff", "Seaborn", "img 1", false, Role.Siege));
        SeabornDeck.Add(new Unit(id++, 4, "name seaborn 6", "this is what this card can do", "eff", "Seaborn", "img 1", false, Role.Range));
        SeabornDeck.Add(new Unit(id++, 6, "name seaborn 7", "this is what this card can do", "eff", "Seaborn", "img 1", false, Role.Agile));
        SeabornDeck.Add(new Unit(id++, 5, "name seaborn 8", "this is what this card can do", "eff", "Seaborn", "img 1", false, Role.Mele));
        SeabornDeck.Add(new Unit(id++, 2, "name seaborn 9", "this is what this card can do", "eff", "Seaborn", "img 1", false, Role.Range));
        SeabornDeck.Add(new Unit(id++, 2, "name seaborn 10", "this is what this card can do", "eff", "Seaborn", "img 1", false, Role.Mele));
        SeabornDeck.Add(new Booster(id++, "name seaborn 11", "this is what this card can do", "eff", "Seaborn", "img 1"));
        SeabornDeck.Add(new Booster(id++, "name seaborn 12", "this is what this card can do", "eff", "Seaborn", "img 1"));
        SeabornDeck.Add(new Decoy(id++, "name seaborn 13", "this is what this card can do", "eff", "Seaborn", "img 1"));
        SeabornDeck.Add(new Decoy(id++, "name seaborn 14", "this is what this card can do", "eff", "Seaborn", "img 1"));
        SeabornDeck.Add(new Weather(id++, "name seaborn 15", "this is what this card can do", "eff", "Seaborn", "img 1"));
        SeabornDeck.Add(new Weather(id++, "name seaborn 16", "this is what this card can do", "eff", "Seaborn", "img 1"));
        SeabornDeck.Add(new Clearing(id++, "name seaborn 17", "this is what this card can do", "eff", "Seaborn", "img 1"));

        WhalerDeck.Add(new Unit(id++, 9, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", true, Role.Mele));
        WhalerDeck.Add(new Unit(id++, 8, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", true, Role.Siege));
        WhalerDeck.Add(new Unit(id++, 10, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", true, Role.Mele));
        WhalerDeck.Add(new Unit(id++, 7, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", true, Role.Agile));
        WhalerDeck.Add(new Unit(id++, 3, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", false, Role.Siege));
        WhalerDeck.Add(new Unit(id++, 4, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", false, Role.Range));
        WhalerDeck.Add(new Unit(id++, 6, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", false, Role.Agile));
        WhalerDeck.Add(new Unit(id++, 5, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", false, Role.Mele));
        WhalerDeck.Add(new Unit(id++, 2, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", false, Role.Range));
        WhalerDeck.Add(new Unit(id++, 2, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", false, Role.Mele));
        WhalerDeck.Add(new Booster(id++, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1"));
        WhalerDeck.Add(new Booster(id++, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1"));
        WhalerDeck.Add(new Decoy(id++, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1"));
        WhalerDeck.Add(new Decoy(id++, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1"));
        WhalerDeck.Add(new Weather(id++, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1"));
        WhalerDeck.Add(new Weather(id++, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1"));
        WhalerDeck.Add(new Clearing(id++, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1"));

        PirateDeck.Add(new Unit(id++, 9, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", true, Role.Mele));
        PirateDeck.Add(new Unit(id++, 8, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", true, Role.Siege));
        PirateDeck.Add(new Unit(id++, 10, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", true, Role.Mele));
        PirateDeck.Add(new Unit(id++, 7, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", true, Role.Agile));
        PirateDeck.Add(new Unit(id++, 3, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", false, Role.Siege));
        PirateDeck.Add(new Unit(id++, 4, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", false, Role.Range));
        PirateDeck.Add(new Unit(id++, 6, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", false, Role.Agile));
        PirateDeck.Add(new Unit(id++, 5, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", false, Role.Mele));
        PirateDeck.Add(new Unit(id++, 2, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", false, Role.Range));
        PirateDeck.Add(new Unit(id++, 2, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1", false, Role.Mele));
        PirateDeck.Add(new Booster(id++, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1"));
        PirateDeck.Add(new Booster(id++, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1"));
        PirateDeck.Add(new Decoy(id++, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1"));
        PirateDeck.Add(new Decoy(id++, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1"));
        PirateDeck.Add(new Weather(id++, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1"));
        PirateDeck.Add(new Weather(id++, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1"));
        PirateDeck.Add(new Clearing(id++, "name pirate", "this is what this card can do", "eff", "Pirate", "img 1"));

        NeutralDeck.Add(new Unit(id++, 9, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1", true, Role.Range));
        NeutralDeck.Add(new Unit(id++, 7, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1", true, Role.Mele));
        NeutralDeck.Add(new Unit(id++, 6, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1", false, Role.Mele));
        NeutralDeck.Add(new Unit(id++, 4, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1", false, Role.Siege));
        NeutralDeck.Add(new Unit(id++, 3, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1", false, Role.Range));
        NeutralDeck.Add(new Unit(id++, 2, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1", false, Role.Range));
        NeutralDeck.Add(new Decoy(id++, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1"));
        NeutralDeck.Add(new Decoy(id++, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1"));
        NeutralDeck.Add(new Weather(id++, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1"));
        NeutralDeck.Add(new Clearing(id++, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1"));
        NeutralDeck.Add(new Booster(id++, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1"));
        NeutralDeck.Add(new Booster(id++, "name neutral", "this is what this card can do", "eff", "Neutral", "img 1"));


        AvailableDecks.Add("Seaborn", SeabornDeck);
        AvailableDecks.Add("Whaler", WhalerDeck);
        AvailableDecks.Add("Pirate", PirateDeck);
        AvailableDecks.Add("Neutral", NeutralDeck);

        //foreach (var item in AvailableDecks) Debug.Log($"{item.Key} deck available");
    }

}
