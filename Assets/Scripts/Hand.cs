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
        layoutGroup.spacing = 60;
        CardsInHand = 10;
    }

    public void ArrangeCards()
    {
        if(CardsInHand < 7) layoutGroup.spacing = 30;
        else if (CardsInHand < 13) layoutGroup.spacing = 20;
        else layoutGroup.spacing = 10;
    }
}
