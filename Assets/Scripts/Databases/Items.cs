using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases
{
    public class Items
    {
        //create an object of SingleObject
        private static Items singleton = new Items();

        //make the constructor private so that this class cannot be
        //instantiated
        private Items() { }

        //Get the only object available
        public static Items instance
        {
            get
            {
                return singleton;
            }
            private set
            {
                singleton = value;
            }
        }

        // Database
        private Dictionary<string, ItemData> database = new Dictionary<string, ItemData>
    {
        // Null / Placeholder
        {"",
            new ItemData(
                ID: ""
                ) },

        // HELD ITEMS
        {"focussash",
            new ItemData(
                ID: "focussash",
                itemName: "Focus Sash",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,
                tags: new ItemTag[]
                {
                    ItemTag.Consumable,
                },
                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.FocusBand(),
                }
                ) },


        // MEDICINE
        {"potion",
            new ItemData(
                ID: "potion",
                itemName: "Potion",
                pocket: ItemPocket.Medicine,
                battlePocket: ItemBattlePocket.HPRestore,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.Potion(
                        applyOnConsume: false, applyOnUse: true,
                        healHP: new Effects.General.HealHP(
                            healMode: Effects.General.HealHP.HealMode.HitPoints,
                            healValue: 20,
                            displayText: "pokemon-heal-hp"
                            )
                        )
                }
                ) },

        {"antidote",
            new ItemData(
                ID: "antidote",
                itemName: "Antidote",
                pocket: ItemPocket.Medicine,
                battlePocket: ItemBattlePocket.StatusRestore
                ) },

        // BERRIES
        {"chilanberry",
            new ItemData(
                ID: "chilanberry",
                itemName: "Chilan Berry",
                pocket: ItemPocket.Berries,

                tags: new ItemTag[]
                {
                    ItemTag.Consumable
                },

                effects: new ItemEffect[]
                {
                    new ItemEffect(
                        ItemEffectType.TypeBerry,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[]{ "DEFAULT", "normal", }
                        ),
                },
                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.NaturalGift(
                        moveType: "normal", basePower: 80
                        ),
                }
                ) },

        {"lumberry",
            new ItemData(
                ID: "lumberry",
                itemName: "Lum Berry",
                pocket: ItemPocket.Berries,
                battlePocket: ItemBattlePocket.StatusRestore,

                tags: new ItemTag[]
                {
                    ItemTag.Consumable
                },

                effects: new ItemEffect[]
                {
                    new ItemEffect(
                        ItemEffectType.HealStatus,
                        stringParams: new string[]{ "burn", "confusion", "freeze", "paralysis", "poison", "poison2", "sleep" }
                        ),
                    new ItemEffect(
                        ItemEffectType.TriggerOnStatus,
                        boolParams: new bool[] { true },
                        stringParams: new string[]{  }
                        ),
                },
                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.NaturalGift(
                        moveType: "flying", basePower: 80
                        ),
                }
                ) },

        {"occaberry",
            new ItemData(
                ID: "occaberry",
                itemName: "Occa Berry",
                pocket: ItemPocket.Berries,

                tags: new ItemTag[]
                {
                    ItemTag.Consumable
                },

                effects: new ItemEffect[]
                {
                    new ItemEffect(
                        ItemEffectType.TypeBerrySuperEffective,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[]{ "DEFAULT", "fire", }
                        ),
                    new ItemEffect(
                        ItemEffectType.Fling,
                        floatParams: new float[] { 10 }
                        ),
                },
                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.NaturalGift(
                        moveType: "fire", basePower: 80
                        ),
                }
                ) },

        {"oranberry",
            new ItemData(
                ID: "oranberry",
                itemName: "Oran Berry",
                pocket: ItemPocket.Berries,
                battlePocket: ItemBattlePocket.HPRestore,

                tags: new ItemTag[]
                {
                    ItemTag.Consumable
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.NaturalGift(
                        moveType: "poison", basePower: 80
                        ),
                    new Effects.Items.Potion(
                        healHP: new Effects.General.HealHP(
                            healMode: Effects.General.HealHP.HealMode.HitPoints,
                            healValue: 10,
                            displayText: "pokemon-heal-hp"
                            )
                        ),
                    new Effects.Items.TriggerSitrusBerry(hpThreshold: 0.5f),
                }
                ) },

        {"passhoberry",
            new ItemData(
                ID: "passhoberry",
                itemName: "Passho Berry",
                pocket: ItemPocket.Berries,

                tags: new ItemTag[]
                {
                    ItemTag.Consumable
                },

                effects: new ItemEffect[]
                {
                    new ItemEffect(
                        ItemEffectType.TypeBerrySuperEffective,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[]{ "DEFAULT", "water", }
                        ),
                },
                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.NaturalGift(
                        moveType: "water", basePower: 80
                        ),
                }
                ) },

        {"sitrusberry",
            new ItemData(
                ID: "sitrusberry",
                itemName: "Sitrus Berry",
                pocket: ItemPocket.Berries,
                battlePocket: ItemBattlePocket.HPRestore,
                tags: new ItemTag[]
                {
                    ItemTag.Consumable
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.NaturalGift(
                        moveType: "psychic", basePower: 80
                        ),
                    new Effects.Items.Potion(
                        healHP: new Effects.General.HealHP(
                            healMode: Effects.General.HealHP.HealMode.MaxHPPercent,
                            healValue: 0.25f
                            )
                        ),
                    new Effects.Items.TriggerSitrusBerry(hpThreshold: 0.5f),
                }
                ) },

        {"wacanberry",
            new ItemData(
                ID: "wacanberry",
                itemName: "Wacan Berry",
                pocket: ItemPocket.Berries,

                tags: new ItemTag[]
                {
                    ItemTag.Consumable
                },

                effects: new ItemEffect[]
                {
                    new ItemEffect(
                        ItemEffectType.TypeBerrySuperEffective,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[]{ "DEFAULT", "electric", }
                        ),
                },
                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.NaturalGift(
                        moveType: "electric", basePower: 80
                        ),
                }
                ) },

        // BATTLE ITEMS
        {"xattack",
            new ItemData(
                ID: "xattack",
                itemName: "X Attack",
                pocket: ItemPocket.BattleItems,
                battlePocket: ItemBattlePocket.BattleItems,

                tags: new ItemTag[]
                {
                    ItemTag.OnlyUseableInBattle,
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.LiechiBerry(
                        statStageMod: new Effects.General.StatStageMod(ATKMod: 1)
                        )
                }
                ) },

        {"xdefense",
            new ItemData(
                ID: "xdefense",
                itemName: "X Defense",
                pocket: ItemPocket.BattleItems,
                battlePocket: ItemBattlePocket.BattleItems,

                tags: new ItemTag[]
                {
                    ItemTag.OnlyUseableInBattle,
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.LiechiBerry(
                        statStageMod: new Effects.General.StatStageMod(DEFMod: 1)
                        )
                }
                ) },

        {"xspatk",
            new ItemData(
                ID: "xspatk",
                itemName: "X Sp. Atk",
                pocket: ItemPocket.BattleItems,
                battlePocket: ItemBattlePocket.BattleItems,

                tags: new ItemTag[]
                {
                    ItemTag.OnlyUseableInBattle,
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.LiechiBerry(
                        statStageMod: new Effects.General.StatStageMod(SPAMod: 1)
                        )
                }
                ) },

        {"xspdef",
            new ItemData(
                ID: "xspdef",
                itemName: "X Sp. Def",
                pocket: ItemPocket.BattleItems,
                battlePocket: ItemBattlePocket.BattleItems,

                tags: new ItemTag[]
                {
                    ItemTag.OnlyUseableInBattle,
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.LiechiBerry(
                        statStageMod: new Effects.General.StatStageMod(SPDMod: 1)
                        )
                }
                ) },

        {"xspeed",
            new ItemData(
                ID: "xspeed",
                itemName: "X Speed",
                pocket: ItemPocket.BattleItems,
                battlePocket: ItemBattlePocket.BattleItems,

                tags: new ItemTag[]
                {
                    ItemTag.OnlyUseableInBattle,
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.LiechiBerry(
                        statStageMod: new Effects.General.StatStageMod(SPEMod: 1)
                        )
                }
                ) },

        {"xaccuracy",
            new ItemData(
                ID: "xaccuracy",
                itemName: "X Accuracy",
                pocket: ItemPocket.BattleItems,
                battlePocket: ItemBattlePocket.BattleItems,

                tags: new ItemTag[]
                {
                    ItemTag.OnlyUseableInBattle,
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.LiechiBerry(
                        statStageMod: new Effects.General.StatStageMod(ACCMod: 1)
                        )
                }
                ) },

        // KEY ITEMS
        {"dynamaxband",
            new ItemData(
                ID: "dynamaxband",
                itemName: "Dynamax Band",
                pocket: ItemPocket.KeyItems,
                battlePocket: ItemBattlePocket.None
                ) },
        {"megaring",
            new ItemData(
                ID: "megaring",
                itemName: "Mega Ring",
                pocket: ItemPocket.KeyItems,
                battlePocket: ItemBattlePocket.None
                ) },
        {"zring",
            new ItemData(
                ID: "zring",
                itemName: "Z-Ring",
                pocket: ItemPocket.KeyItems,
                battlePocket: ItemBattlePocket.None
                ) },

        // MEGA STONES
        {"blastoisinite",
            new ItemData(
                ID: "blastoisinite",
                itemName: "Blastoisinite",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.MegaStone(baseFormID: "blastoise", formID: "blastoise-mega"),
                }
                ) },

        // PLATES
        {"dracoplate",
            new ItemData(
                ID: "dracoplate",
                itemName: "Draco Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-dragon"),
                    new Effects.Items.Judgment(moveType: "dragon"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "dragon" }
                                ),
                        })
                }
                ) },
        {"dreadplate",
            new ItemData(
                ID: "dreadplate",
                itemName: "Dread Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-dark"),
                    new Effects.Items.Judgment(moveType: "dark"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "dark" }
                                ),
                        })
                }
                ) },
        {"earthplate",
            new ItemData(
                ID: "earthplate",
                itemName: "Earth Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-ground"),
                    new Effects.Items.Judgment(moveType: "ground"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "ground" }
                                ),
                        })
                }
                ) },
        {"electricplate",
            new ItemData(
                ID: "electricplate",
                itemName: "Electric Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-electric"),
                    new Effects.Items.Judgment(moveType: "electric"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "electric" }
                                ),
                        })
                }
                ) },
        {"fistplate",
            new ItemData(
                ID: "fistplate",
                itemName: "Fist Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-fighting"),
                    new Effects.Items.Judgment(moveType: "fighting"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fighting" }
                                ),
                        })
                }
                ) },
        {"flameplate",
            new ItemData(
                ID: "flameplate",
                itemName: "Flame Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-fire"),
                    new Effects.Items.Judgment(moveType: "fire"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fire" }
                                ),
                        })
                }
                ) },
        {"icicleplate",
            new ItemData(
                ID: "icicleplate",
                itemName: "Icicle Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-ice"),
                    new Effects.Items.Judgment(moveType: "ice"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "ice" }
                                ),
                        })
                }
                ) },
        {"insectplate",
            new ItemData(
                ID: "insectplate",
                itemName: "Insect Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-bug"),
                    new Effects.Items.Judgment(moveType: "bug"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "bug" }
                                ),
                        })
                }
                ) },
        {"ironplate",
            new ItemData(
                ID: "ironplate",
                itemName: "Iron Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-steel"),
                    new Effects.Items.Judgment(moveType: "steel"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "steel" }
                                ),
                        })
                }
                ) },
        {"meadowplate",
            new ItemData(
                ID: "meadowplate",
                itemName: "Meadow Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-grass"),
                    new Effects.Items.Judgment(moveType: "grass"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "grass" }
                                ),
                        })
                }
                ) },
        {"mindplate",
            new ItemData(
                ID: "mindplate",
                itemName: "Mind Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-psychic"),
                    new Effects.Items.Judgment(moveType: "psychic"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "psychic" }
                                ),
                        })
                }
                ) },
        {"pixieplate",
            new ItemData(
                ID: "pixieplate",
                itemName: "Pixie Plate",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,
                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-fairy"),
                    new Effects.Items.Judgment(moveType: "fairy"),
                    new Effects.Items.Charcoal(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fairy" }
                                ),
                        })
                }
                ) },


        // ---Z-CRYSTALS---

        // --Regular

        {"normaliumz",
            new ItemData(
                ID: "normaliumz",
                itemName: "Normalium Z",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ZCrystal(moveType: "normal", ZMove: "breakneckblitz"),
                }
                ) },
        {"psychiumz",
            new ItemData(
                ID: "psychiumz",
                itemName: "Psychium Z",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ArceusPlate(baseFormID: "arceus", formID: "arceus-psychic"),
                    new Effects.Items.ZCrystal(moveType: "psychic", ZMove: "shatteredpsyche"),
                }
                ) },

        // --Signature

        {"tapuniumz",
            new ItemData(
                ID: "tapuniumz",
                itemName: "Tapunium Z",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ZCrystalSignature(
                        ZMove: "guardianofalola",
                        eligibleMoves: new string[] { "naturesmadness" },
                        eligiblePokemonIDs: new string[] { "tapukoko", "tapulele", "tapubulu", "tapufini" }
                        ),
                }
                ) },
        {"ultranecroziumz",
            new ItemData(
                ID: "ultranecroziumz",
                itemName: "Ultranecrozium Z",
                pocket: ItemPocket.OtherItems,
                battlePocket: ItemBattlePocket.None,

                tags: new ItemTag[]
                {

                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.ZCrystalSignature(
                        ZMove: "lightthatburnsthesky",
                        eligibleMoves: new string[] { "photongeyser" },
                        eligiblePokemonIDs: new string[] { "necrozma-ultra" }
                        ),
                }
                ) },

    };

        // Methods
        public ItemData GetItemData(string ID)
        {
            if (database.ContainsKey(ID))
            {
                return database[ID];
            }
            Debug.LogWarning("Could not find item with ID: " + ID);
            return database[""];
        }
    }
}