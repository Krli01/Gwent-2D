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

    // Update is called once per frame
    void Update()
    {
        if(CardsInHand < 7) layoutGroup.spacing = 70;
        else if (CardsInHand < 13) layoutGroup.spacing = 60;
        else layoutGroup.spacing = 50;
    }
}
