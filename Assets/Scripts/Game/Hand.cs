using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public HorizontalLayoutGroup layoutGroup;
    public int CardsInHand;

    // Start is called before the first frame update
    void Start()
    {
        layoutGroup = (HorizontalLayoutGroup)this.GetComponent("HorizontalLayoutGroup");
        layoutGroup.spacing = 15;
        CardsInHand = 10;
    }

    public void ArrangeCards()
    {
        if(CardsInHand < 9) layoutGroup.spacing = 20;
        else layoutGroup.spacing = 15;
    }

    public void Clear()
    {
        foreach (GameCard c in transform.GetComponentsInChildren<GameCard>())
        {
            TurnSystem.Instance.Active.graveyard.SendToGraveyard(c);
        }
    }
}
