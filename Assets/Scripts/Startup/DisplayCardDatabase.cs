using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCardDatabase : MonoBehaviour
{
    public static Dictionary<string, Sprite> factionImages = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> factionCoats = new Dictionary<string, Sprite>();
    public static Dictionary<int, Sprite> powerImages = new Dictionary<int, Sprite>();
    public static Dictionary<Role, Sprite> roleImages = new Dictionary<Role, Sprite>();
    public static Dictionary<Sprite, Sprite> borderImages = new Dictionary<Sprite, Sprite>();
    public static DisplayCardDatabase Instance { get; private set; }

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAssets();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadAssets()
    {    
        roleImages.Add(global::Role.Leader, Resources.Load<Sprite>("big/role leader"));
        roleImages.Add(global::Role.Mele, Resources.Load<Sprite>("big/role mele"));
        roleImages.Add(global::Role.Range, Resources.Load<Sprite>("big/role range"));
        roleImages.Add(global::Role.Siege, Resources.Load<Sprite>("big/role siege"));
        roleImages.Add(global::Role.Agile, Resources.Load<Sprite>("big/role agile"));
        roleImages.Add(global::Role.Weather, Resources.Load<Sprite>("big/role weather"));
        roleImages.Add(global::Role.Clearing, Resources.Load<Sprite>("big/role clearing"));
        roleImages.Add(global::Role.Booster, Resources.Load<Sprite>("big/role booster"));
        roleImages.Add(global::Role.Decoy, Resources.Load<Sprite>("big/role decoy"));

        factionImages.Add("Seaborn", Resources.Load<Sprite>("big/Seaborn"));
        factionImages.Add("Whaler", Resources.Load<Sprite>("big/Whaler"));
        factionImages.Add("Pirate", Resources.Load<Sprite>("big/Pirate"));

        factionCoats.Add("Seaborn", Resources.Load<Sprite>("big/Seaborn_diamond"));
        factionCoats.Add("Whaler", Resources.Load<Sprite>("big/Whaler_diamond"));
        factionCoats.Add("Pirate", Resources.Load<Sprite>("big/Pirate_diamond"));
        factionCoats.Add("Neutral", Resources.Load<Sprite>("big/Neutral_diamond"));

        powerImages.Add(0, Resources.Load<Sprite>("big/p0"));
        powerImages.Add(1, Resources.Load<Sprite>("big/p1"));
        powerImages.Add(2, Resources.Load<Sprite>("big/p2"));
        powerImages.Add(3, Resources.Load<Sprite>("big/p3"));
        powerImages.Add(4, Resources.Load<Sprite>("big/p4"));
        powerImages.Add(5, Resources.Load<Sprite>("big/p5"));
        powerImages.Add(6, Resources.Load<Sprite>("big/p6"));
        powerImages.Add(7, Resources.Load<Sprite>("big/p7"));
        powerImages.Add(8, Resources.Load<Sprite>("big/p8"));
        powerImages.Add(9, Resources.Load<Sprite>("big/p9"));
        powerImages.Add(10, Resources.Load<Sprite>("big/p10"));
        powerImages.Add(11, Resources.Load<Sprite>("big/p11"));
        
        borderImages.Add(Resources.Load <Sprite>("small/Gold"), Resources.Load <Sprite>("big/Gold"));
        borderImages.Add(Resources.Load <Sprite>("small/Silver"), Resources.Load <Sprite>("big/Silver"));
        borderImages.Add(Resources.Load <Sprite>("small/Bronze"), Resources.Load <Sprite>("big/Bronze"));
    }
}
