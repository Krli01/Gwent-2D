using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static Dictionary<string,List<Card>> AvailableDecks;
    public List<Card> PirateDeck = new List<Card>();
    public List<Card> WhalerDeck = new List<Card>();
    public List<Card> SeabornDeck = new List<Card>();
    
    public static Dictionary<string, Sprite> factionImages = new Dictionary<string, Sprite>();
    public static Dictionary<int, Sprite> powerImages = new Dictionary<int, Sprite>();
    public static Dictionary<Role, Sprite> roleImages = new Dictionary<Role, Sprite>();


    private void Awake() 
    {
        // indexar lideres en 0 (por el momento hay un solo lider)
        
        SeabornDeck.Add(new Unit(00, 3, "name seaborn", "this is what this card can do", "eff", "Seaborn", Resources.Load<Sprite>("DisplayTerstCard"), false, Role.Melee));

        WhalerDeck.Add(new Unit(01, 5, "name whaler", "this is what this card can do", "eff", "Whaler", Resources.Load<Sprite>("DisplayTerstCard"), false, Role.Melee));

        AvailableDecks.Add("Seaborn",SeabornDeck);
        AvailableDecks.Add("Whaler",WhalerDeck);
    }

}
