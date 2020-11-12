using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public BattleSettings battleSettings;
    public BTLManager btlManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerSave.instance.name);
        TestBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestBattle()
    {
        BattleTeam.TeamMode teamMode = (battleSettings.battleType == BattleType.Single) ? BattleTeam.TeamMode.Single
            : BattleTeam.TeamMode.Double;

        Trainer playerTrainer = CreateTrainerUsingTeamNo("Player 1");
        playerTrainer.playerID = 1;
        BattleTeam playerTeam = new BattleTeam(
            teamMode: teamMode,
            trainers: new List<Trainer> { playerTrainer }
            );

        Trainer aiTrainer = CreateTrainer2("Player 2");
        BattleTeam aiTeam = new BattleTeam(
            teamMode: teamMode,
            trainers: new List<Trainer> { aiTrainer }
            );

        List<BattleTeam> teams = new List<BattleTeam> { playerTeam, aiTeam };

        StartCoroutine(btlManager.StartBattle(battleSettings, teams));
    }

    public Trainer CreateTrainer(string trainerName)
    {

        // party
        List<Pokemon> party = new List<Pokemon>();

        Pokemon pokemon1 = new Pokemon(
            pokemonID: "bulbasaur",
            level: 15,
            hpPercent: 1f,
            natureID: "hardy",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("karatechop"),
                new Pokemon.Moveslot("forestscurse"),
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("tackle"),
            },
            //nonVolatileStatus: new StatusCondition("burn"),
            abilityNo: 1,
            isHiddenAbility: true,

            item: new Item("wacanberry")
            );

        Pokemon pokemon2 = new Pokemon(
            pokemonID: "charmander",
            level: 15,
            hpPercent: 1f,
            natureID: "adamant",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("yawn"),
                new Pokemon.Moveslot("tackle"),
                new Pokemon.Moveslot("doubleedge", PPUps: GameSettings.pkmnMaxPPUps),
            },
            //nonVolatileStatus: new StatusCondition("poison2"),
            abilityNo: 1,
            isHiddenAbility: true,

            item: new Item("occaberry")
            );

        Pokemon pokemon3 = new Pokemon(
            pokemonID: "squirtle",
            level: 15,
            hpPercent: 0.75f,
            natureID: "sassy",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("watergun"),
                new Pokemon.Moveslot("withdraw"),
                new Pokemon.Moveslot("healingwish"),
                new Pokemon.Moveslot("recover"),
            }
            //nonVolatileStatus: new StatusCondition("paralysis")
            );

        Pokemon pokemon4 = new Pokemon(
            pokemonID: "pikachu",
            level: 18,
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("thunderpunch"),
                new Pokemon.Moveslot("poisontail"),
                new Pokemon.Moveslot("recover"),
                new Pokemon.Moveslot("batonpass")
            },
            //nonVolatileStatus: new StatusCondition("poison2"),
            abilityNo: 0,
            isHiddenAbility: false
            );

        Pokemon melotta = new Pokemon(
            pokemonID: "meloetta-aria",
            level: 10,
            hpPercent: 1f,
            natureID: "relaxed",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("grassyterrain"),
                new Pokemon.Moveslot("fakeout"),
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("storedpower"),
            }
            //item: new Item("psychiumz")
            );

        Pokemon hoopa = new Pokemon(
            pokemonID: "hoopa-confined",
            level: 10,
            hpPercent: 1f,
            natureID: "timid",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("karatechop"),
                new Pokemon.Moveslot("raindance2"),
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("hyperspacehole2"),
            }
            );

        Pokemon hoopaU = new Pokemon(
            pokemonID: "hoopa-unbound",
            level: 10,
            hpPercent: 1f,
            natureID: "rash",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("knockoff"),
                new Pokemon.Moveslot("hyperspacefury"),
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("terrainpulse"),
            }
            );

        Pokemon arceus = new Pokemon(
            pokemonID: "arceus",
            level: 15,
            natureID: "hardy",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("liquidation"),
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("dragondance"),
                new Pokemon.Moveslot("growl"),
            },
            //item: new Item("psychiumz"),
            item: new Item("ironplate"),
            checkForm: true
            );

        Pokemon tapukoko = new Pokemon(
            pokemonID: "tapukoko",
            level: 10,
            hpPercent: 1f,
            natureID: "jolly",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("knockoff"),
                new Pokemon.Moveslot("dragondance"),
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("tripleaxel"),
            },
            item: new Item("tapuniumz")
            );

        Pokemon blastoise = new Pokemon(
            pokemonID: "blastoise",
            level: 10,
            hpPercent: 1f,
            natureID: "modest",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("hydropump"),
                new Pokemon.Moveslot("stormthrow"),
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("magnitude"),
            },
            dynamaxProps: new Pokemon.DynamaxProperties(
                dynamaxLevel: 10,
                GMaxForm: "blastoise-gmax",
                GMaxMove: "gmaxcannonade",
                moveType: "water"
                )
            //item: new Item("blastoisinite")
            );

        Pokemon greninja = new Pokemon(
            pokemonID: "greninja-battlebond",
            level: 12,
            hpPercent: 1f,
            natureID: "naive",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("stormthrow"),
                new Pokemon.Moveslot("liquidation"),
                new Pokemon.Moveslot("roost"),
                new Pokemon.Moveslot("icebeam"),
            }
            );

        party.AddRange(new List<Pokemon> { tapukoko, blastoise, hoopa, pokemon2, pokemon3, pokemon4, });

        // items
        List<Item> items = new List<Item>();

        Item item1 = new Item("potion");
        Item item2 = new Item("potion");
        Item item3 = new Item("potion");
        Item item4 = new Item("antidote");
        Item item5 = new Item("oranberry");
        
        Item itemx1 = new Item("xattack");
        Item itemx2 = new Item("xdefense");
        Item itemx3 = new Item("xspatk");
        Item itemx4 = new Item("xspdef");
        Item itemx5 = new Item("xspeed");
        Item itemx6 = new Item("xattack");
        Item itemx7 = new Item("xspeed");

        items.AddRange(new List<Item> { item1, item2, item3, item4, item5, itemx1, itemx2, itemx3, itemx4, itemx5, itemx6, itemx7 });

        Trainer trainer = new Trainer(
            name: trainerName,
            party: party,
            items: items,

            megaRing: new Item("megaring"),
            ZRing: new Item("zring"),
            dynamaxBand: new Item("dynamaxband")
            );
        return trainer;
    }
    public Trainer CreateTrainer2(string trainerName)
    {
        // party
        List<Pokemon> party = new List<Pokemon>();

        Pokemon pokemon1 = new Pokemon(
            pokemonID: "bulbasaur",
            level: 10,
            natureID: "hardy",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("swordsdance", PPUps: GameSettings.pkmnMaxPPUps),
            },

            item: new Item("chilanberry")
            );

        Pokemon pokemon2 = new Pokemon(
            pokemonID: "charmander",
            level: 10,
            natureID: "adamant",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("tackle"),
            },
            item: new Item("occaberry")
            );

        Pokemon pokemon3 = new Pokemon(
            pokemonID: "squirtle",
            level: 10,
            natureID: "sassy",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("watergun"),
            },

            item: new Item("passhoberry")
            );

        Pokemon morpeko = new Pokemon(
            pokemonID: "morpeko",
            level: 5,
            natureID: "modest",
            moveslots: new Pokemon.Moveslot[]
            {
                new Pokemon.Moveslot("aurawheel"),
            }
            );

        party.AddRange(new List<Pokemon> { pokemon1, pokemon2, pokemon3 });

        // items
        List<Item> items = new List<Item>();

        Item item1 = new Item("potion");
        Item item2 = new Item("potion");
        Item item3 = new Item("potion");
        Item item4 = new Item("antidote");
        Item item5 = new Item("oranberry");
        Item item6 = new Item("xattack");

        items.AddRange(new List<Item> { item1, item2, item3, item4, item5, item6 });

        Trainer trainer = new Trainer(
            name: trainerName,
            party: party,
            items: items,

            megaRing: new Item("megaring"),
            ZRing: new Item("zring"),
            dynamaxBand: new Item("dynamaxband")
            );
        return trainer;
    }

    public static Trainer CreateTrainerUsingTeamNo(string trainerName = "Red", int teamNo = 1)
    {
        // party
        List<Pokemon> party = new List<Pokemon>();
        List<Item> items = new List<Item>();
        Item megaRing = new Item("megaring");
        Item ZRing = new Item("zring");
        Item dynamaxBand = new Item("dynamaxband");

        items.AddRange(new List<Item>
        {
            new Item("potion"),
            new Item("antidote"),
            new Item("sitrusberry"),
            new Item("xattack"),
            new Item("xspeed")
        });
        switch (teamNo)
        {
            // Starters
            case 1:
                party.AddRange(new List<Pokemon>
                {
                    new Pokemon(
                        pokemonID: "bulbasaur",
                        level: 15,
                        hpPercent: 1f,
                        natureID: "hardy",
                        moveslots: new Pokemon.Moveslot[]
                        {
                            new Pokemon.Moveslot("karatechop"),
                            new Pokemon.Moveslot("forestscurse"),
                            new Pokemon.Moveslot("roost"),
                            new Pokemon.Moveslot("tackle"),
                        },
                        //nonVolatileStatus: new StatusCondition("burn"),
                        abilityNo: 1,
                        isHiddenAbility: true,
                        item: new Item("occaberry")
                        ),

                    new Pokemon(
                        pokemonID: "charmander",
                        level: 15,
                        hpPercent: 1f,
                        natureID: "adamant",
                        moveslots: new Pokemon.Moveslot[]
                        {
                            new Pokemon.Moveslot("roost"),
                            new Pokemon.Moveslot("yawn"),
                            new Pokemon.Moveslot("tackle"),
                            new Pokemon.Moveslot("flamethrower", PPUps: GameSettings.pkmnMaxPPUps),
                        },
                        //nonVolatileStatus: new StatusCondition("poison2"),
                        abilityNo: 1,
                        isHiddenAbility: true,

                        item: new Item("wacanberry")
                        ),

                    new Pokemon(
                        pokemonID: "squirtle",
                        level: 15,
                        hpPercent: 0.75f,
                        natureID: "sassy",
                        moveslots: new Pokemon.Moveslot[]
                        {
                            new Pokemon.Moveslot("watergun"),
                            new Pokemon.Moveslot("explosion"),
                            new Pokemon.Moveslot("healingwish"),
                            new Pokemon.Moveslot("recover"),
                        }
                        //nonVolatileStatus: new StatusCondition("paralysis")
                        ),

                    new Pokemon(
                        pokemonID: "tapukoko",
                        level: 15,
                        hpPercent: 1f,
                        natureID: "jolly",
                        moveslots: new Pokemon.Moveslot[]
                        {
                            new Pokemon.Moveslot("knockoff"),
                            new Pokemon.Moveslot("dragondance"),
                            new Pokemon.Moveslot("roost"),
                            new Pokemon.Moveslot("naturesmadness"),
                        },
                        item: new Item("tapuniumz")
                        ),

                    new Pokemon(
                        pokemonID: "blastoise",
                        level: 15,
                        hpPercent: 1f,
                        natureID: "modest",
                        moveslots: new Pokemon.Moveslot[]
                        {
                            new Pokemon.Moveslot("hydropump"),
                            new Pokemon.Moveslot("stormthrow"),
                            new Pokemon.Moveslot("roost"),
                            new Pokemon.Moveslot("magnitude"),
                        },
                        dynamaxProps: new Pokemon.DynamaxProperties(
                            dynamaxLevel: 10,
                            GMaxForm: "blastoise-gmax",
                            GMaxMove: "gmaxcannonade",
                            moveType: "water"
                            ),
                        item: new Item("blastoisinite")
                        )
                });
                break;

            case 2:

                break;

            case 3:

                break;

            default:
                party.AddRange(new List<Pokemon>
                {
                    new Pokemon(
                        pokemonID: "pikachu",
                        level: 18,
                        moveslots: new Pokemon.Moveslot[]
                        {
                            new Pokemon.Moveslot("thunderpunch"),
                            new Pokemon.Moveslot("poisontail"),
                            new Pokemon.Moveslot("recover"),
                            new Pokemon.Moveslot("batonpass")
                        },
                        abilityNo: 0,
                        isHiddenAbility: false
                        ),
                });
                break;
        }

        Trainer trainer = new Trainer(
            name: trainerName,
            party: party,
            items: items,

            megaRing: megaRing, ZRing: ZRing, dynamaxBand: dynamaxBand
            );
        return trainer;

    }

}
