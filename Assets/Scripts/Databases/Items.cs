using PBS.Data;
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
        private Dictionary<string, PBS.Data.Item> database = new Dictionary<string, PBS.Data.Item>
    {
        // Null / Placeholder
        {"",
            new PBS.Data.Item(
                ID: ""
                ) },

        // HELD ITEMS
        {"focussash",
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
                ID: "antidote",
                itemName: "Antidote",
                pocket: ItemPocket.Medicine,
                battlePocket: ItemBattlePocket.StatusRestore,

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.LumBerry(
                        statuses: new string[]{ "poison", "poison2" }
                        )
                }
                ) },

        // BERRIES
        {"chilanberry",
            new PBS.Data.Item(
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
                        ItemEffectType.YacheBerry,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[]{ "DEFAULT", "normal", }
                        ),
                },
                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.NaturalGift(
                        moveType: "normal", basePower: 80
                        ),
                    new Effects.Items.YacheBerry(
                        mustBeSuperEffective: false,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[]{ "normal" })
                        })
                }
                ) },

        {"lumberry",
            new PBS.Data.Item(
                ID: "lumberry",
                itemName: "Lum Berry",
                pocket: ItemPocket.Berries,
                battlePocket: ItemBattlePocket.StatusRestore,

                tags: new ItemTag[]
                {
                    ItemTag.Consumable
                },

                effectsNew: new Effects.Items.ItemEffect[]
                {
                    new Effects.Items.LumBerry(
                        statuses: new string[]{ "burn", "confusion", "freeze", "paralysis", "poison", "poison2", "sleep" }
                        ),
                    new Effects.Items.LumBerryTrigger(
                        statuses: new string[]{ "burn", "confusion", "freeze", "paralysis", "poison", "poison2", "sleep" }
                        ),
                    new Effects.Items.NaturalGift(
                        moveType: "flying", basePower: 80
                        ),
                }
                ) },

        {"occaberry",
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
                ID: "dynamaxband",
                itemName: "Dynamax Band",
                pocket: ItemPocket.KeyItems,
                battlePocket: ItemBattlePocket.None
                ) },
        {"megaring",
            new PBS.Data.Item(
                ID: "megaring",
                itemName: "Mega Ring",
                pocket: ItemPocket.KeyItems,
                battlePocket: ItemBattlePocket.None
                ) },
        {"zring",
            new PBS.Data.Item(
                ID: "zring",
                itemName: "Z-Ring",
                pocket: ItemPocket.KeyItems,
                battlePocket: ItemBattlePocket.None
                ) },

        // MEGA STONES
        {"blastoisinite",
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
            new PBS.Data.Item(
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
        public PBS.Data.Item GetItemData(string ID)
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