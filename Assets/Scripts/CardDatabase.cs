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
    
    public static Dictionary<string, Sprite> factionImages = new Dictionary<string, Sprite>();
    public static Dictionary<int, Sprite> powerImages = new Dictionary<int, Sprite>();
    public static Dictionary<Role, Sprite> roleImages = new Dictionary<Role, Sprite>();


    private void Awake() 
    {
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
        
        SeabornDeck.Add(new Unit(00, 3, "name seaborn", "this is what this card can do", "eff", "Seaborn", "img 1", false, Role.Mele));
        SeabornDeck.Add(new Booster(00, "name seaborn", "this is what this card can do", "eff", "Seaborn", "img 1"));

        WhalerDeck.Add(new Unit(01, 5, "name whaler", "this is what this card can do", "eff", "Whaler", "img 1", true, Role.Range));

        AvailableDecks.Add("Seaborn", SeabornDeck);
        AvailableDecks.Add("Whaler", WhalerDeck);

        //foreach (var item in AvailableDecks) Debug.Log($"{item.Key} deck available");
    }

}
