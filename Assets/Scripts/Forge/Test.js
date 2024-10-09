effect
{
    Name: "Testing the AddPossibilities",
    Params:{
        Auxilio: Bool,
        Numerin: Number,
        Letra: String,
        Letraa: String
    },
        Action: (galletica, context) => 
    {
        inicio=Numerin;
        for target in galletica.Find((unit)=> (unit.Power>3 && (Auxilio|| Letra== Letraa)))
        {
            target.Power += 1;
        };
    }
}



card {
    Type: "Oro",
    Name: "Beluga",
    Faction: "Whaler",
    Range: ["Siege"],
    OnActivation:
    [
        {
            Effect:
            {
                       Name: "Testing the AddPossibilities",
                Auxilio: false|| true,
                Numerin: (2^2)^3,
                Letra: "Ingenieros Celestiales",
                Letraa: "Compilada",
            },
            Selector:
            {
                Source: "board",
                Single: false,
                Predicate: (unit) => true
            },
        }
    ]
}

card {
    Type: "Despeje",
    Name: "Beluga",
    Faction: "Pirate",
	OnActivation: []
}

effect {
    Name: "Damage",
    Params: 
    {
        Amount: Number
    },
    Action: (targets, context) => {
        for target in targets {
            i = 0;
            while (++i < Amount)
		{
                target.Power+=1;
		};
        };
    }
}
effect {
    Name: "Draw",
    Action: (targets, context) => {
        topCard = context.Deck.Pop();
        context.Hand.Add(topCard);
    }
}
effect {
    Name: "Return to deck",
    Action: (targets, context) => {
        for target in targets 
        {
            deck = context.DeckOfPlayer(target.Owner);
            deck.Push(target);
            context.Board.Remove(target);
        };
    }
}
card {
    Type: "Oro",
    Name: "Beluga",
    Faction: "Pirate",
    Power: 8,
    Range: ["Melee", "Ranged"],
    OnActivation: 
    [
        {
            Effect: 
            {
                Name: "Damage",
                Amount: 5,
            },
            Selector: {
                Source: "board",
                Single: false,
                Predicate: (unit) => true
            },
            PostAction: {
                Type: "Return to deck",
                Selector: {
                    Source: "parent",
                    Single: 3 == 6,
                    Predicate: (unit) => true
                },
            }
        },
        {
            Effect: "Draw",
        }

    ]
}