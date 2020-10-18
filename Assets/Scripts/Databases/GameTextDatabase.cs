using System.Collections.Generic;
using UnityEngine;

public class GameTextDatabase
{
    //create an object of SingleObject
    private static GameTextDatabase singleton = new GameTextDatabase();

    //make the constructor private so that this class cannot be
    //instantiated
    private GameTextDatabase() { }

    //Get the only object available
    public static GameTextDatabase instance
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
    private Dictionary<string, GameTextData> database = new Dictionary<string, GameTextData>
    {
        // Null / Placeholder
        {"",
            new GameTextData(
                ID: "",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "" },
                }
                ) },

        // BATTLE MESSAGES

        {"bpc-run-trainer",
            new GameTextData(
                ID: "bpc-run-trainer",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "You can't run from a trainer battle!" },
                }
                ) },
        {"bpc-switch-unable",
            new GameTextData(
                ID: "bpc-switch-unable",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-pokemon-}} is unable to battle!" },
                }
                ) },

        // POKEMON GENERAL

        {"pokemon-ability-gain",
            new GameTextData(
                ID: "pokemon-ability-gain",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} gained {{-ability-name-}}!" },
                }
                ) },
        {"pokemon-ability-lost",
            new GameTextData(
                ID: "pokemon-ability-lost",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} lost {{-ability-name-}}!" },
                }
                ) },

        {"pokemon-changeform",
            new GameTextData(
                ID: "pokemon-changeform",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} changed form!" },
                }
                ) },
        {"pokemon-changetype",
            new GameTextData(
                ID: "pokemon-changetype",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} transformed into the {{-type-list-}}!" },
                }
                ) },     
                
        {"pokemon-choiced",
            new GameTextData(
                ID: "pokemon-choiced",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is locked into {{-move-name-}}!" },
                }
                ) },
                
        {"pokemon-damage",
            new GameTextData(
                ID: "pokemon-damage",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt!" },
                }
                ) },
        
        {"pokemon-dynamax",
            new GameTextData(
                ID: "pokemon-dynamax",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is reacting to {{-trainer-poss-LC-}} {{-item-name-}}!" },
                }
                ) },
        {"pokemon-dynamax-wild",
            new GameTextData(
                ID: "pokemon-dynamax-wild",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is gathering dynamax energy!" },
                }
                ) },
        {"pokemon-dynamax-form",
            new GameTextData(
                ID: "pokemon-dynamax-form",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} dynamaxed!" },
                }
                ) },
        {"pokemon-dynamax-end",
            new GameTextData(
                ID: "pokemon-dynamax-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} returned to regular size!" },
                }
                ) },
                
        {"pokemon-forcein",
            new GameTextData(
                ID: "pokemon-forcein",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was dragged out!" },
                }
                ) },
                
        {"pokemon-gigantamax",
            new GameTextData(
                ID: "pokemon-gigantamax",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} gigantamaxed!" },
                }
                ) },
                
        {"pokemon-heal",
            new GameTextData(
                ID: "pokemon-heal",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} HP was restored." },
                }
                ) },
        {"pokemon-heal-hp",
            new GameTextData(
                ID: "pokemon-heal-hp",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} recovered {{-int-0-}} HP!" },
                }
                ) },
        
        {"pokemon-heal-fail",
            new GameTextData(
                ID: "pokemon-heal-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already at maximum HP." },
                }
                ) },
        {"pokemon-megaevolve",
            new GameTextData(
                ID: "pokemon-megaevolve",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is reacting to {{-trainer-}}'s {{-item-name-}}!" },
                }
                ) },
        {"pokemon-megaevolve-form",
            new GameTextData(
                ID: "pokemon-megaevolve-form",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} mega-evolved into {{-user-pokemon-form-}}!" },
                }
                ) },
        {"pokemon-revive",
            new GameTextData(
                ID: "pokemon-revive",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was revived!" },
                }
                ) },
        {"pokemon-run",
            new GameTextData(
                ID: "pokemon-run",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} fled from battle!" },
                }
                ) },
        {"pokemon-run-trap",
            new GameTextData(
                ID: "pokemon-run-trap",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is trapped! It can't run!" },
                }
                ) },
        {"pokemon-run-ingrain",
            new GameTextData(
                ID: "pokemon-run-ingrain",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't escape due to {{-move-name-}}!" },
                }
                ) },
        {"pokemon-switch-ingrain",
            new GameTextData(
                ID: "pokemon-switch-ingrain",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't switch out due to {{-move-name-}}!" },
                }
                ) },
                
        {"pokemon-unaffect",
            new GameTextData(
                ID: "pokemon-unaffect",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was unaffected!" },
                }
                ) },
                
        {"pokemon-use-move",
            new GameTextData(
                ID: "pokemon-use-move",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} used\n{{-move-name-}}!" },
                }
                ) },

        // ABILITIES

        {"ability-airlock",
            new GameTextData(
                ID: "ability-airlock",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The effects of weather were eliminated!" },
                }
                ) },
                
        {"ability-anticipation",
            new GameTextData(
                ID: "ability-anticipation",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} shuddered!" },
                }
                ) },
                
        {"ability-aurabreak",
            new GameTextData(
                ID: "ability-aurabreak",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The effects of aura abilities is reversed!" },
                }
                ) },
                
        {"ability-baddreams",
            new GameTextData(
                ID: "ability-baddreams",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt due to {{-user-pokemon-poss-}} {{-ability-name-}}!" },
                }
                ) },
                
        {"ability-ballfetch",
            new GameTextData(
                ID: "ability-ballfetch",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} retrieved one {{-item-name-}}!" },
                }
                ) },
                
        {"ability-battlebond",
            new GameTextData(
                ID: "ability-battlebond",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} became fully charged due to its bond with its Trainer!" },
                }
                ) },
        {"ability-battlebond-form",
            new GameTextData(
                ID: "ability-battlebond-form",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} became {{-user-pokemon-form-}}" },
                }
                ) },
                
        {"ability-clearbody",
            new GameTextData(
                ID: "ability-clearbody",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} stats cannot be lowered!" },
                }
                ) },
                
        {"ability-colorchange",
            new GameTextData(
                ID: "ability-colorchange",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} turned into the {{-type-name-}}!" },
                }
                ) },
                
        {"ability-darkaura",
            new GameTextData(
                ID: "ability-darkaura",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "All {{-type-list-}} moves are increased in power!" },
                }
                ) },
                
        {"ability-disguise",
            new GameTextData(
                ID: "ability-disguise",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} disguise was busted!" },
                }
                ) },
                
        {"ability-forewarn",
            new GameTextData(
                ID: "ability-forewarn",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} was alerted to {{-target-pokemon-poss-}} {{-move-name-}}!" },
                }
                ) },
                
        {"ability-frisk",
            new GameTextData(
                ID: "ability-frisk",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} frisked {{-target-pokemon-}} and found one {{-item-name-}}!" },
                }
                ) },
                
        {"ability-gulpmissile",
            new GameTextData(
                ID: "ability-gulpmissile",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} launched a missile at {{-target-pokemon-}}!" },
                }
                ) },
                
        {"ability-harvest",
            new GameTextData(
                ID: "ability-harvest",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} harvested one {{-item-name-}}!" },
                }
                ) },
                
        {"ability-hypercutter",
            new GameTextData(
                ID: "ability-hypercutter",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} cannot be lowered!" },
                }
                ) },
                
        {"ability-illusion",
            new GameTextData(
                ID: "ability-illusion",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} illusion was broken!" },
                }
                ) },
                
        {"ability-magicbounce",
            new GameTextData(
                ID: "ability-magicbounce",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} was reflected back!" },
                }
                ) },

        {"ability-magician",
            new GameTextData(
                ID: "ability-magician",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} stole {{-target-pokemon-poss-}} {{-item-name-}}!" },
                }
                ) },
        
        {"ability-mirrorarmor-fail",
            new GameTextData(
                ID: "ability-mirrorarmor-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "There was no target to reflect stat changes back to!" },
                }
                ) },
                
        {"ability-moldbreaker",
            new GameTextData(
                ID: "ability-moldbreaker",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} breaks the mold!" },
                }
                ) },
                
        {"ability-mummy",
            new GameTextData(
                ID: "ability-mummy",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was mummified!" },
                }
                ) },

        {"ability-neautralizinggas",
            new GameTextData(
                ID: "ability-neautralizinggas",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Abilities have been neutralized!" },
                }
                ) },
                
        {"ability-oblivious",
            new GameTextData(
                ID: "ability-oblivious",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was protected by {{-ability-name-}}!" },
                }
                ) },
                
        {"ability-pickup",
            new GameTextData(
                ID: "ability-pickup",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} picked up one {{-item-name-}}!" },
                }
                ) },
                
        {"ability-powerofalchemy",
            new GameTextData(
                ID: "ability-powerofalchemy",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} swapped abilities with {{-target-pokemon-}}!" },
                }
                ) },
                
        {"ability-pressure",
            new GameTextData(
                ID: "ability-pressure",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is exerting its Pressure!" },
                }
                ) },
                
        {"ability-protean",
            new GameTextData(
                ID: "ability-protean",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} turned into the {{-type-name-}}!" },
                }
                ) },
                
        {"ability-quickdraw",
            new GameTextData(
                ID: "ability-quickdraw",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} attacked first!" },
                }
                ) },
                
        {"ability-ripen",
            new GameTextData(
                ID: "ability-ripen",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The berry's effect was doubled!" },
                }
                ) },
                
        {"ability-runaway",
            new GameTextData(
                ID: "ability-runaway",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} escaped using {{-ability-name-}}!" },
                }
                ) },
                
        {"ability-shadowtag",
            new GameTextData(
                ID: "ability-shadowtag",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is unable to flee!" },
                }
                ) },
                
        {"ability-slowstart",
            new GameTextData(
                ID: "ability-slowstart",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} has to get it going!" },
                }
                ) },
                
        {"ability-sturdy",
            new GameTextData(
                ID: "ability-sturdy",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} survived the hit!" },
                }
                ) },
                
        {"ability-symbiosis",
            new GameTextData(
                ID: "ability-symbiosis",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} passed one {{-item-name-}} to {{-target-pokemon-}}!" },
                }
                ) },        
                
        {"ability-teravolt",
            new GameTextData(
                ID: "ability-teravolt",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} radiates a bursting aura!" },
                }
                ) },
                
        {"ability-trace",
            new GameTextData(
                ID: "ability-trace",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} traced {{-target-pokemon-poss-}} ability!" },
                }
                ) },
                
        {"ability-truant",
            new GameTextData(
                ID: "ability-truant",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is loafing around!" },
                }
                ) },
                
        {"ability-turboblaze",
            new GameTextData(
                ID: "ability-turboblaze",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} radiates a blazing aura!" },
                }
                ) },
                
        {"ability-wanderingspirit",
            new GameTextData(
                ID: "ability-wanderingspirit",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} swapped abilities with {{-target-pokemon-}}!" },
                }
                ) },

        // Item-Related
        {"ability-unnerve-default",
            new GameTextData(
                ID: "ability-unnerve-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} made {{-target-team-}} too nervous to eat berries!" },
                }
                ) },

        // Protection
        {"ability-sturdy-default",
            new GameTextData(
                ID: "ability-sturdy-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} survived the hit!" },
                }
                ) },

        // ITEMS
        {"item-consume-default",
            new GameTextData(
                ID: "item-consume-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} consumed its {{-item-name-}}!" },
                }
                ) },
                
        {"item-focusband",
            new GameTextData(
                ID: "item-focusband",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} hung onto its {{-item-name-}}!" },
                }
                ) },

        {"item-focusband-default",
            new GameTextData(
                ID: "item-focusband-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} hung onto its {{-item-name-}}!" },
                }
                ) },
                
        {"item-lifeorb",
            new GameTextData(
                ID: "item-lifeorb",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} was hurt by its {{-item-name-}}!" },
                }
                ) },
                
        {"item-quickclaw",
            new GameTextData(
                ID: "item-quickclaw",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-poss-}} {{-item-name-}} activated!" },
                }
                ) },

        {"item-typeberry-default",
            new GameTextData(
                ID: "item-typeberry-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} weakened the {{-type-name-}} attack!" },
                }
                ) },

        {"item-use",
            new GameTextData(
                ID: "item-use",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-trainer-}} used one {{-item-name-}}!" },
                }
                ) },

        {"item-use-fail",
            new GameTextData(
                ID: "item-use-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It will not have any effect." },
                }
                ) },
        {"item-use-fail-battle",
            new GameTextData(
                ID: "item-use-fail-battle",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It may only be used in battle!" },
                }
                ) },
        {"item-use-fail-notenough",
            new GameTextData(
                ID: "item-use-fail-notenough",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Your {{-item-name-}} stock will be empty!" },
                }
                ) },
        {"item-use-fail-ranout",
            new GameTextData(
                ID: "item-use-fail-ranout",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-trainer-}} tried to use a {{-item-name-}}, but ran out!" },
                }
                ) },

        // MOVES
        {"move-FAIL-default",
            new GameTextData(
                ID: "move-FAIL-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "But it failed!" },
                }
                ) },
        {"move-FAIL-effect",
            new GameTextData(
                ID: "move-FAIL-effect",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The {{-move-name-}} failed to work!" },
                }
                ) },
        {"move-FAIL-form",
            new GameTextData(
                ID: "move-FAIL-form",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "But {{-user-pokemon-}} can't use it the way it is now!" },
                }
                ) },
        {"move-FAIL-pokemon",
            new GameTextData(
                ID: "move-FAIL-pokemon",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "But {{-user-pokemon-}} can't use the move!" },
                }
                ) },
        {"move-noeffect-default",
            new GameTextData(
                ID: "move-noeffect-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It had no effect..." },
                }
                ) },
        {"move-noeffect-multi-default",
            new GameTextData(
                ID: "move-noeffect-multi-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It had no effect on {{-target-pokemon-}}..." },
                }
                ) },
        {"move-struggle",
            new GameTextData(
                ID: "move-struggle",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} has no useable moves!" },
                }
                ) },
                
        {"move-absorb",
            new GameTextData(
                ID: "move-absorb",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} had its energy drained!" },
                }
                ) },

        {"move-allyswitch",
            new GameTextData(
                ID: "move-allyswitch",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} and {{-target-pokemon-}} switched places!" },
                }
                ) },

        {"move-aquaring",
            new GameTextData(
                ID: "move-aquaring",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} surrounded itself with a veil of water!" },
                }
                ) },
        {"move-aquaring-heal",
            new GameTextData(
                ID: "move-aquaring-heal",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} recovered HP with its veil of water!" },
                }
                ) },
                
        {"move-auroraveil",
            new GameTextData(
                ID: "move-auroraveil",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} will reduce damage taken for {{-target-team-}}!" },
                }
                ) },
                
        {"move-burnup",
            new GameTextData(
                ID: "move-burnup",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} lost its {{-type-list-}}!" },
                }
                ) },
        {"move-burnup-all",
            new GameTextData(
                ID: "move-burnup-all",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} lost every one of its types!" },
                }
                ) },
                
        {"move-coreenforcer",
            new GameTextData(
                ID: "move-coreenforcer",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-ability-name-}} was suppressed!" },
                }
                ) },

        {"move-coreenforcer-fail",
            new GameTextData(
                ID: "move-coreenforcer-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} ability couldn't be suppressed!" },
                }
                ) },
                
        {"move-corrosivegas",
            new GameTextData(
                ID: "move-corrosivegas",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} was burned away!" },
                }
                ) },
                
        {"move-covet",
            new GameTextData(
                ID: "move-covet",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} stole {{-target-pokemon-poss-}} {{-item-name-}}!" },
                }
                ) },
                
        {"move-doubleedge",
            new GameTextData(
                ID: "move-doubleedge",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is damaged by recoil!" },
                }
                ) },
                
        {"move-endure",
            new GameTextData(
                ID: "move-endure",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} braced itself!" },
                }
                ) },
        {"move-endure-success",
            new GameTextData(
                ID: "move-endure-success",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} endured the hit!" },
                }
                ) },
                
        {"move-feint",
            new GameTextData(
                ID: "move-feint-protect-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} fell for the feint!" },
                }
                ) },
        {"move-feint-matblock",
            new GameTextData(
                ID: "move-feint-matblock-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-poss-}} protection was lifted!" },
                }
                ) },
                
        {"move-forestscurse",
            new GameTextData(
                ID: "move-forestscurse",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-type-list-}} was added to {{-target-pokemon-}}!" },
                }
                ) },
        {"move-forestscurse-all",
            new GameTextData(
                ID: "move-forestscurse-all",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Every type was added to {{-target-pokemon-}}!" },
                }
                ) },
        {"move-forestscurse-loss",
            new GameTextData(
                ID: "move-forestscurse-loss",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} lost its {{-type-list-}} gained from {{-move-name-}}!" },
                }
                ) },

        {"move-gmaxsteelsurge",
            new GameTextData(
                ID: "move-gmaxsteelsurge",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Steel spikes float in the air around {{-target-team-}}!" },
                }
                ) },
        {"move-gmaxsteelsurge-damage",
            new GameTextData(
                ID: "move-gmaxsteelsurge-damage",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Steel spikes dug into {{-target-pokemon-}}!" },
                }
                ) },
        {"move-gmaxsteelsurge-fail",
            new GameTextData(
                ID: "move-gmaxsteelsurge-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The steel spikes couldn't be placed around {{-target-team-}}!" },
                }
                ) },
        {"move-gmaxsteelsurge-remove",
            new GameTextData(
                ID: "move-gmaxsteelsurge-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The steel spikes around {{-target-team-}} disappeared!" },
                }
                ) },

        {"move-guardsplit",
            new GameTextData(
                ID: "move-guardsplit",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} shared its guard with the target!" },
                }
                ) },
        
        {"move-haze",
            new GameTextData(
                ID: "move-haze",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "All stat changes were eliminated!" },
                }
                ) },
        {"move-haze-pokemon",
            new GameTextData(
                ID: "move-haze-pokemon",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} stat changes were eliminated!" },
                }
                ) },
        {"move-haze-team",
            new GameTextData(
                ID: "move-haze-team",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}}'s stat changes were eliminated!" },
                }
                ) },

        {"move-ingrain",
            new GameTextData(
                ID: "move-ingrain",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} planted its roots!" },
                }
                ) },
        {"move-ingrain-heal",
            new GameTextData(
                ID: "move-ingrain-heal",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} absorbed nutrients with its roots!" },
                }
                ) },
                
        {"move-knockoff",
            new GameTextData(
                ID: "move-knockoff",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} knocked off {{-target-pokemon-poss-}} {{-item-name-}}!" },
                }
                ) },
                
        {"move-lightscreen",
            new GameTextData(
                ID: "move-lightscreen",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} will reduce special damage taken for {{-target-team-}}!" },
                }
                ) },

        {"move-lockon",
            new GameTextData(
                ID: "move-lockon",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} took aim at {{-target-pokemon-}}!" },
                }
                ) },
        {"move-lockon-end",
            new GameTextData(
                ID: "move-lockon-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is no longer taking aim at {{-target-pokemon-}}!" },
                }
                ) },
        {"move-lockon-fail",
            new GameTextData(
                ID: "move-lockon-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is already taking aim at {{-target-pokemon-}}!" },
                }
                ) },
                
        {"move-luckychant",
            new GameTextData(
                ID: "move-luckychant",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The {{-move-name-}} shielded {{-target-team-}} from critical hits!" },
                }
                ) },
        {"move-luckychant-fail",
            new GameTextData(
                ID: "move-luckychant-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The lucky chant failed!" },
                }
                ) },
        {"move-luckychant-remove",
            new GameTextData(
                ID: "move-luckychant-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is no longer shielded by {{-move-name-}}!" },
                }
                ) },
                
        {"move-magiccoat",
            new GameTextData(
                ID: "move-magiccoat",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} was reflected back!" },
                }
                ) },
                
        {"move-magnitude",
            new GameTextData(
                ID: "move-magnitude",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} {{-int-0-}}!" },
                }
                ) },
                
        {"move-matblock",
            new GameTextData(
                ID: "move-matblock",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is being protected!" },
                }
                ) },
        {"move-matblock-success",
            new GameTextData(
                ID: "move-matblock-success",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} was protected against the attack!" },
                }
                ) },
        
        {"move-mist",
            new GameTextData(
                ID: "move-mist",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} became shrouded in mist!" },
                }
                ) },
        {"move-mist-fail",
            new GameTextData(
                ID: "move-mist-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} failed to be created for {{-target-team-}}!" },
                }
                ) },
        {"move-mist-protect",
            new GameTextData(
                ID: "move-mist-protect",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mist prevents {{-target-pokemon-poss-}} stats from being lowered!" },
                }
                ) },
        {"move-mist-remove",
            new GameTextData(
                ID: "move-mist-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is no longer shrouded in mist!" },
                }
                ) },
                
        {"move-poltergeist",
            new GameTextData(
                ID: "move-poltergeist",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} attacked using {{-target-pokemon-poss-}} {{-item-name-}}!" },
                }
                ) },

        {"move-powersplit",
            new GameTextData(
                ID: "move-powersplit",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} shared its power with the target!" },
                }
                ) },

        {"move-powerswap",
            new GameTextData(
                ID: "move-powerswap",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} switched all changes to its {{-stat-types-}} with the target!" },
                }
                ) },
                
        {"move-protect",
            new GameTextData(
                ID: "move-protect",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is protecting itself!" },
                }
                ) },
        {"move-protect-success",
            new GameTextData(
                ID: "move-protect-success",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} protected itself!" },
                }
                ) },

        {"move-powertrick",
            new GameTextData(
                ID: "move-powertrick",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} switched its {{-stat-types-}}!" },
                }
                ) },

        {"move-reflect-default",
            new GameTextData(
                ID: "move-reflect-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} was set up for {{-target-team-}}!" },
                }
                ) },
        {"move-reflect",
            new GameTextData(
                ID: "move-reflect",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} will reduce physical damage taken for {{-target-team-}}!" },
                }
                ) },
        {"move-reflect-fail",
            new GameTextData(
                ID: "move-reflect-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} couldn't be set up for {{-target-team-}}!" },
                }
                ) },
        {"move-reflect-remove",
            new GameTextData(
                ID: "move-reflect-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}}'s {{-move-name-}} wore off!" },
                }
                ) },
        
        {"move-refresh-fail",
            new GameTextData(
                ID: "move-refresh-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} couldn't cure anything for {{-target-pokemon-}}!" },
                }
                ) },
        
        {"move-roleplay",
            new GameTextData(
                ID: "move-roleplay",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} copied {{-target-pokemon-poss-}} {{-ability-name-}}!" },
                }
                ) },
                
        {"move-safeguard",
            new GameTextData(
                ID: "move-safeguard",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} became cloaked in a mystical veil!" },
                }
                ) },
        {"move-safeguard-protect",
            new GameTextData(
                ID: "move-safeguard-protect",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was protected by the mystical veil!" },
                }
                ) },
        {"move-safeguard-remove",
            new GameTextData(
                ID: "move-safeguard-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is no longer protected by the mystical veil!" },
                }
                ) },

        {"move-skillswap",
            new GameTextData(
                ID: "move-skillswap",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} swapped abilities with its target!" },
                }
                ) },

        {"move-soak",
            new GameTextData(
                ID: "move-soak",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} transformed into the {{-type-list-}}!" },
                }
                ) },
        {"move-soak-all",
            new GameTextData(
                ID: "move-soak-all",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} became every type!" },
                }
                ) },

        {"move-spikes",
            new GameTextData(
                ID: "move-spikes",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Spikes were scattered all around {{-target-team-}}'s feet!" },
                }
                ) },
        {"move-spikes-damage",
            new GameTextData(
                ID: "move-spikes-damage",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt by the spikes!" },
                }
                ) },
        {"move-spikes-fail",
            new GameTextData(
                ID: "move-spikes-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Spikes couldn't be placed around {{-target-team-}}!" },
                }
                ) },
        {"move-spikes-remove",
            new GameTextData(
                ID: "move-spikes-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The spikes around {{-target-team-}}'s feet disappeared!" },
                }
                ) },
                
        {"move-spikyshield",
            new GameTextData(
                ID: "move-spikyshield",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt!" },
                }
                ) },

        {"move-stealthrock",
            new GameTextData(
                ID: "move-stealthrock",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Pointed stones float in the air around {{-target-team-}}!" },
                }
                ) },
        {"move-stealthrock-damage",
            new GameTextData(
                ID: "move-stealthrock-damage",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Pointed stones dug into {{-target-pokemon-}}!" },
                }
                ) },
        {"move-stealthrock-fail",
            new GameTextData(
                ID: "move-stealthrock-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The pointed stones couldn't be placed around {{-target-team-}}!" },
                }
                ) },
        {"move-stealthrock-remove",
            new GameTextData(
                ID: "move-stealthrock-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The pointed stones around {{-target-team-}} disappeared!" },
                }
                ) },
                
        {"move-steelroller",
            new GameTextData(
                ID: "move-steelroller",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} destroyed the terrain!" },
                }
                ) },

        {"move-stickyweb",
            new GameTextData(
                ID: "move-stickyweb",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "A sticky web was placed near {{-target-team-}}'s feet!" },
                }
                ) },
        {"move-stickyweb-fail",
            new GameTextData(
                ID: "move-stickyweb-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sticky web couldn't be placed near {{-target-team-}}!" },
                }
                ) },
        {"move-stickyweb-remove",
            new GameTextData(
                ID: "move-stickyweb-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sticky web near {{-target-team-}}'s feet disappeared!" },
                }
                ) },

        {"move-toxicspikes",
            new GameTextData(
                ID: "move-toxicspikes",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Poisonous spikes were scattered all around {{-target-team-}}'s feet!" },
                }
                ) },
        {"move-toxicspikes-fail",
            new GameTextData(
                ID: "move-toxicspikes-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The poisonous spikes couldn't be placed around {{-target-team-}}!" },
                }
                ) },
        {"move-toxicspikes-remove",
            new GameTextData(
                ID: "move-toxicspikes-remove",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The poisonous spikes around {{-target-team-}}'s feet disappeared!" },
                }
                ) },
                
        {"move-whirlwind",
            new GameTextData(
                ID: "move-whirlwind",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was blown away!" },
                }
                ) },
        {"move-whirlwind-fail",
            new GameTextData(
                ID: "move-whirlwind-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be forced out!" },
                }
                ) },

        {"zmove-start",
            new GameTextData(
                ID: "zmove-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} surrounded itself with its Z-Power!" },
                }
                ) },
        {"zmove-use",
            new GameTextData(
                ID: "zmove-use",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} unleashes its full-force Z-Move!" },
                }
                ) },
        {"zmove-protect",
            new GameTextData(
                ID: "zmove-protect",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} couldn't fully protect itself!" },
                }
                ) },
                
        {"gmax-wildfire",
            new GameTextData(
                ID: "gmax-wildfire",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-team-status-name-}} started on {{-target-team-}}!" },
                }
                ) },
        {"gmax-wildfire-end",
            new GameTextData(
                ID: "gmax-wildfire-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-team-status-name-}} ended on {{-target-team-}}!" },
                }
                ) },
        {"gmax-wildfire-damage",
            new GameTextData(
                ID: "gmax-wildfire-damage",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} was hurt by {{-team-status-name-}}!" },
                }
                ) },
                
        {"move-worryseed-fail",
            new GameTextData(
                ID: "move-worryseed-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} failed to gain {{-ability-name-}}!" },
                }
                ) },
                
        // STATUS
        {"status-electrify",
            new GameTextData(
                ID: "status-electrify",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} moves became electrified!" },
                }
                ) },
        
        {"status-flinch",
            new GameTextData(
                ID: "status-flinch",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} flinched!" },
                }
                ) },
        {"status-flinch-blink",
            new GameTextData(
                ID: "status-flinch-blink",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} blinked!" },
                }
                ) },
                
        {"status-identification",
            new GameTextData(
                ID: "status-identification",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was identified!" },
                }
                ) },
        {"status-identification-already",
            new GameTextData(
                ID: "status-identification-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was already identified!" },
                }
                ) },
                
        {"status-imprison",
            new GameTextData(
                ID: "status-imprison",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} sealed the opponent's move(s)!" },
                }
                ) },
        {"status-imprison-negate",
            new GameTextData(
                ID: "status-imprison-negate",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-poss-}}'s {{-move-name-}} is sealed! It can't use it!" },
                }
                ) },
        {"status-imprison-choose",
            new GameTextData(
                ID: "status-imprison-choose",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} is sealed!" },
                }
                ) },
                
        {"status-infatuation",
            new GameTextData(
                ID: "status-infatuation",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} fell in love with {{-user-pokemon-}}!" },
                }
                ) },
        {"status-infatuation-end",
            new GameTextData(
                ID: "status-infatuation-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} snapped out of its infatuation!" },
                }
                ) },
        {"status-infatuation-move",
            new GameTextData(
                ID: "status-infatuation-move",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is infatuated with {{-user-pokemon-}}!" },
                }
                ) },
        {"status-infatuation-movefail",
            new GameTextData(
                ID: "status-infatuation-movefail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was immobilized by love!" },
                }
                ) },
        {"status-infatuation-fail",
            new GameTextData(
                ID: "status-infatuation-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} can't become infatuated with {{-user-pokemon-}}!" },
                }
                ) },
        {"status-infatuation-already",
            new GameTextData(
                ID: "status-infatuation-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already infatuated with {{-user-pokemon-}}!" },
                }
                ) },
                
        {"status-perishsong",
            new GameTextData(
                ID: "status-perishsong",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} heard the song!" },
                }
                ) },
        {"status-perishsong-count",
            new GameTextData(
                ID: "status-perishsong-count",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} Perish Song count fell to {{-int-0-}}!" },
                }
                ) },
                
        {"status-tarshot",
            new GameTextData(
                ID: "status-tarshot",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} became weaker to Fire-type moves!" },
                }
                ) },
                
        {"status-yawn",
            new GameTextData(
                ID: "status-yawn",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is feeling drowsy!" },
                }
                ) },
        {"status-yawn-wait",
            new GameTextData(
                ID: "status-yawn-wait",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is still drowsy!" },
                }
                ) },
        

        // Call other moves
        {"move-naturepower-default",
            new GameTextData(
                ID: "move-naturepower-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-0-}} turned into {{-move-name-1-}}!" },
                }
                ) },

        // Damage Multipliers
        {"move-helpinghand-default",
            new GameTextData(
                ID: "move-helpinghand-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is getting ready to help {{-target-pokemon-}}!" },
                }
                ) },

        // Health-Related
        {"move-aromatherapy",
            new GameTextData(
                ID: "move-aromatherapy",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "A soothing aroma wafted through the area!" },
                }
                ) },
        {"move-healbell",
            new GameTextData(
                ID: "move-healbell",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "A bell chimed!" },
                }
                ) },

        {"move-hpdrain-default",
            new GameTextData(
                ID: "move-hpdrain-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} had its energy drained!" },
                }
                ) },

        {"move-jumpkick-fail-default",
            new GameTextData(
                ID: "move-jumpkick-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} injured itself!" },
                }
                ) },
        {"move-jumpkick-fail-jumpkick",
            new GameTextData(
                ID: "move-jumpkick-fail-jumpkick",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} kept going and crashed!" },
                }
                ) },

        {"move-leechseed-seed-default",
            new GameTextData(
                ID: "move-leechseed-seed-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was seeded!" },
                }
                ) },
        {"move-leechseed-drain-default",
            new GameTextData(
                ID: "move-leechseed-drain-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} had its health sapped by {{-move-name-}}!" },
                }
                ) },
        {"move-leechseed-fail-default",
            new GameTextData(
                ID: "move-leechseed-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} can't be seeded!" },
                }
                ) },

        {"move-recoil-default",
            new GameTextData(
                ID: "move-recoil-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is damaged by recoil!" },
                }
                ) },

        {"move-recover-success-default",
            new GameTextData(
                ID: "move-recover-succcess-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} HP was restored." },
                }
                ) },
        {"move-recover-fail-default",
            new GameTextData(
                ID: "move-recover-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already at maximum HP." },
                }
                ) },

        {"move-rest-default",
            new GameTextData(
                ID: "move-rest-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} slept and became fully healthy!" },
                }
                ) },

        {"move-wish-start",
            new GameTextData(
                ID: "move-wish-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} made a wish!" },
                }
                ) },
        {"move-wish-heal",
            new GameTextData(
                ID: "move-wish-heal",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-poss-}} wish came true!" },
                }
                ) },
        {"move-wish-heal-healingwish",
            new GameTextData(
                ID: "move-wish-heal-healingwish",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The healing wish came true!" },
                }
                ) },
        {"move-wish-heal-lunardance",
            new GameTextData(
                ID: "move-wish-heal-lunardance",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-pokemon-list-}} became cloaked in mystical moonlight!" },
                }
                ) },

        // Item-Related
        {"move-bestow-default",
            new GameTextData(
                ID: "move-bestow-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} received {{-item-name-}} from {{-user-pokemon-}}!" },
                }
                ) },

        {"move-bugbite-default",
            new GameTextData(
                ID: "move-bugbite-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} stole and ate {{-target-pokemon-poss-}} {{-item-name-}}!" },
                }
                ) },

        {"move-covet-default",
            new GameTextData(
                ID: "move-covet-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} stole {{-target-pokemon-poss-}} {{-item-name-}}!" },
                }
                ) },

        {"move-embargo-begin-default",
            new GameTextData(
                ID: "move-embargo-begin-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} can't use items anymore!" },
                }
                ) },
        {"move-embargo-end-default",
            new GameTextData(
                ID: "move-embargo-end-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} can now use items again!" },
                }
                ) },
        {"move-embargo-attempt-default",
            new GameTextData(
                ID: "move-embargo-attempt-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-item-name-}} due to its embargo!" },
                }
                ) },

         {"move-fling-default",
            new GameTextData(
                ID: "move-fling-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} flung its {{-item-name-}}!" },
                }
                ) },

        {"move-incinerate-default",
            new GameTextData(
                ID: "move-incinerate-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} was burned up!" },
                }
                ) },

        {"move-knockoff-default",
            new GameTextData(
                ID: "move-knockoff-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} knocked off {{-target-pokemon-poss-}} {{-item-name-}}!" },
                }
                ) },

        {"move-recycle-default",
            new GameTextData(
                ID: "move-recycle-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} found one {{-item-name-}}!" },
                }
                ) },

        {"move-teatime-fail-default",
            new GameTextData(
                ID: "move-recycle-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "But nothing happened!" },
                }
                ) },

        {"move-trick-start-default",
            new GameTextData(
                ID: "move-trick-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} swapped items with {{-target-pokemon-}}!" },
                }
                ) },
        {"move-trick-swap-default",
            new GameTextData(
                ID: "move-trick-swap-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} obtained one {{-item-name-}}!" },
                }
                ) },

        // Multi-turn moves
        {"move-charge-default",
            new GameTextData(
                ID: "move-charge-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is charging its attack!" },
                }
                ) },
        {"move-charge-bounce",
            new GameTextData(
                ID: "move-charge-bounce",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} sprang up!" },
                }
                ) },
        {"move-charge-dig",
            new GameTextData(
                ID: "move-charge-dig",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} burrowed its way underground!" },
                }
                ) },
        {"move-charge-dive",
            new GameTextData(
                ID: "move-charge-dive",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} hid underwater!" },
                }
                ) },
        {"move-charge-fly",
            new GameTextData(
                ID: "move-charge-fly",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} flew up high!" },
                }
                ) },
        {"move-charge-freezeshock",
            new GameTextData(
                ID: "move-charge-freezeshock",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} became cloaked in a freezing light!" },
                }
                ) },
        {"move-charge-geomancy",
            new GameTextData(
                ID: "move-charge-geomancy",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is absorbing power!" },
                }
                ) },
        {"move-charge-iceburn",
            new GameTextData(
                ID: "move-charge-iceburn",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} became cloaked in a freezing air!" },
                }
                ) },
        {"move-charge-phantomforce",
            new GameTextData(
                ID: "move-charge-phantomforce",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} vanished!" },
                }
                ) },
        {"move-charge-razorwind",
            new GameTextData(
                ID: "move-charge-razorwind",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} whipped up a whirlwind!" },
                }
                ) },
        {"move-charge-shadowforce",
            new GameTextData(
                ID: "move-charge-shadowforce",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} vanished instantly!" },
                }
                ) },
        {"move-charge-skullbash",
            new GameTextData(
                ID: "move-charge-skullbash",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} tucked in its head!" },
                }
                ) },
        {"move-charge-skyattack",
            new GameTextData(
                ID: "move-charge-skyattack",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} became cloaked in a harsh light!" },
                }
                ) },
        {"move-charge-solarbeam",
            new GameTextData(
                ID: "move-charge-solarbeam",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} absorbed light!" },
                }
                ) },

        {"move-futuresight-start-default",
            new GameTextData(
                ID: "move-futuresight-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} foresaw an attack!" },
                }
                ) },
        {"move-futuresight-attack-default",
            new GameTextData(
                ID: "move-futuresight-attack-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-pokemon-list-}} took the {{-move-name-}} attack!" },
                }
                ) },
        {"move-futuresight-start-doomdesire",
            new GameTextData(
                ID: "move-futuresight-start-doomdesire",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} chose {{-move-name-}} as its destiny!" },
                }
                ) },

        {"move-skydrop-grab-default",
            new GameTextData(
                ID: "move-skydrop-grab-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} took {{-target-pokemon-}} into the sky!" },
                }
                ) },
        {"move-skydrop-free-default",
            new GameTextData(
                ID: "move-skydrop-free-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was freed from {{-move-name-}}!" },
                }
                ) },
        {"move-skydrop-trap-default",
            new GameTextData(
                ID: "move-skydrop-trap-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is currently held by {{-move-name-}}!" },
                }
                ) },

        {"move-recharge-default",
            new GameTextData(
                ID: "move-recharge-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is recharging!" },
                }
                ) },

        {"move-bide-store-default",
            new GameTextData(
                ID: "move-bide-store-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is storing energy!" },
                }
                ) },
        {"move-bide-end-default",
            new GameTextData(
                ID: "move-bide-end-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} unleashed the stored energy!" },
                }
                ) },

        // Protection

        {"move-endure-start-default",
            new GameTextData(
                ID: "move-endure-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} braced itself!" },
                }
                ) },
        {"move-endure-success-default",
            new GameTextData(
                ID: "move-endure-success-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} endured the hit!" },
                }
                ) },

        {"move-feint-protect-default",
            new GameTextData(
                ID: "move-feint-protect-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} fell for the feint!" },
                }
                ) },
        {"move-feint-matblock-default",
            new GameTextData(
                ID: "move-feint-matblock-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The effects of {{-move-name-}} were lifted from {{-target-team-}}!" },
                }
                ) },

        {"move-matblock-start-default",
            new GameTextData(
                ID: "move-matblock-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is being protected!" },
                }
                ) },
        {"move-matblock-success-default",
            new GameTextData(
                ID: "move-matblock-success-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-poss-}} {{-move-name-}} protected against the attack!" },
                }
                ) },

        {"move-protect-start-default",
            new GameTextData(
                ID: "move-protect-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is protecting itself!" },
                }
                ) },
        {"move-protect-success-default",
            new GameTextData(
                ID: "move-protect-success-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} protected itself!" },
                }
                ) },

        {"move-spikyshield-default",
            new GameTextData(
                ID: "move-spikyshield-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt!" },
                }
                ) },

        // Stat Change-Related

        {"move-rage-default",
            new GameTextData(
                ID: "move-rage-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} rage is building!" },
                }
                ) },

        {"move-spectralthief-default",
            new GameTextData(
                ID: "move-spectralthief-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} stole the stat boosts of {{-target-pokemon-}}!" },
                }
                ) },

        // Status-Related

        {"move-beakblast-default",
            new GameTextData(
                ID: "move-beakblast-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} started heating up its beak!" },
                }
                ) },

        {"move-bind-start-default",
            new GameTextData(
                ID: "move-bind-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was squeezed by {{-user-pokemon-}}!" },
                }
                ) },
        {"move-bind-end-default",
            new GameTextData(
                ID: "move-bind-end-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was freed from {{-move-name-}}!" },
                }
                ) },
        {"move-bind-damage-default",
            new GameTextData(
                ID: "move-bind-damage-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is hurt by {{-move-name-}}!" },
                }
                ) },
        {"move-bind-trap-default",
            new GameTextData(
                ID: "move-bind-trap-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is trapped by {{-move-name-}}! It can't switch out!" },
                }
                ) },

        {"move-bind-start-firespin",
            new GameTextData(
                ID: "move-bind-start-firespin",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was trapped in the fiery vortex!" },
                }
                ) },
        {"move-bind-damage-firespin",
            new GameTextData(
                ID: "move-bind-damage-firespin",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt by the raging vortex!" },
                }
                ) },

        {"move-bind-start-wrap",
            new GameTextData(
                ID: "move-bind-start-wrap",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was wrapped by {{-user-pokemon-}}!" },
                }
                ) },
        {"move-bind-end-wrap",
            new GameTextData(
                ID: "move-bind-end-wrap",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was released from {{-move-name-}}!" },
                }
                ) },

        {"move-block-start-default",
            new GameTextData(
                ID: "move-block-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} can no longer escape!" },
                }
                ) },
        {"move-block-fail-default",
            new GameTextData(
                ID: "move-block-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be trapped!" },
                }
                ) },
        {"move-block-trap-default",
            new GameTextData(
                ID: "move-block-trap-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is trapped! It can't switch out!" },
                }
                ) },

        {"move-disable-attempt-default",
            new GameTextData(
                ID: "move-disable-attempt-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-poss-}} {{-move-name-}} is disabled!" },
                }
                ) },
        {"move-disable-start-default",
            new GameTextData(
                ID: "move-disable-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-move-name-}} was disabled!" },
                }
                ) },
        {"move-disable-end-default",
            new GameTextData(
                ID: "move-disable-end-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is no longer disabled!" },
                }
                ) },
        {"move-disable-already-default",
            new GameTextData(
                ID: "move-disable-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already disabled!" },
                }
                ) },
        {"move-disable-fail-default",
            new GameTextData(
                ID: "move-disable-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be disabled!" },
                }
                ) },

        {"move-encore-start-default",
            new GameTextData(
                ID: "move-encore-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} received an encore!" },
                }
                ) },
        {"move-encore-end-default",
            new GameTextData(
                ID: "move-encore-end-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} encore wore off!" },
                }
                ) },
        {"move-encore-attempt-default",
            new GameTextData(
                ID: "move-encore-attempt-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} may only use {{-move-name-}} due to encore!" },
                }
                ) },
        {"move-encore-already-default",
            new GameTextData(
                ID: "move-encore-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already encored!" },
                }
                ) },
        {"move-encore-fail-default",
            new GameTextData(
                ID: "move-encore-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be encored!" },
                }
                ) },

        {"move-healblock-start-default",
            new GameTextData(
                ID: "move-healblock-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was prevented from healing!" },
                }
                ) },
        {"move-healblock-end-default",
            new GameTextData(
                ID: "move-healblock-end-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was freed from {{-move-name-}}!" },
                }
                ) },
        {"move-healblock-attempt-default",
            new GameTextData(
                ID: "move-healblock-attempt-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-move-name-}}! It is prevented from healing!" },
                }
                ) },
        {"move-healblock-already-default",
            new GameTextData(
                ID: "move-healblock-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already prevented from healing!" },
                }
                ) },
        {"move-healblock-fail-default",
            new GameTextData(
                ID: "move-healblock-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be prevented from healing!" },
                }
                ) },

        {"move-taunt-start-default",
            new GameTextData(
                ID: "move-taunt-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} fell for the taunt!" },
                }
                ) },
        {"move-taunt-end-default",
            new GameTextData(
                ID: "move-taunt-end-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} taunt wore off!" },
                }
                ) },
        {"move-taunt-attempt-default",
            new GameTextData(
                ID: "move-taunt-attempt-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-move-name-}} due to the taunt!" },
                }
                ) },
        {"move-taunt-already-default",
            new GameTextData(
                ID: "move-taunt-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already taunted!" },
                }
                ) },
        {"move-taunt-fail-default",
            new GameTextData(
                ID: "move-taunt-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be taunted!" },
                }
                ) },

        {"move-torment-start-default",
            new GameTextData(
                ID: "move-torment-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was subjected to torment!" },
                }
                ) },
        {"move-torment-end-default",
            new GameTextData(
                ID: "move-torment-end-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is no longer subjected to torment!" },
                }
                ) },
        {"move-torment-attempt-default",
            new GameTextData(
                ID: "move-torment-attempt-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-move-name-}} due to the torment!" },
                }
                ) },
        {"move-torment-already-default",
            new GameTextData(
                ID: "move-torment-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already subjected to torment!" },
                }
                ) },
        {"move-torment-fail-default",
            new GameTextData(
                ID: "move-torment-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be subjected to torment!" },
                }
                ) },

        {"move-uproar-start-default",
            new GameTextData(
                ID: "status-uproar-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is causing an uproar!" },
                }
                ) },
        {"move-uproar-end-default",
            new GameTextData(
                ID: "status-uproar-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} calmed down." },
                }
                ) },
        {"move-uproar-sleep-default",
            new GameTextData(
                ID: "status-uproar-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} can't fall asleep due to {{-user-pokemon-poss-}} uproar!" },
                }
                ) },

        // Type-Related

        {"move-powder-start-default",
            new GameTextData(
                ID: "move-powder-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is covered in powder!" },
                }
                ) },
        {"move-powder-damage-default",
            new GameTextData(
                ID: "move-powder-damage-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "When the flame touched the powder on {{-target-pokemon-}}, it exploded!" },
                }
                ) },

        {"move-smackdown-default",
            new GameTextData(
                ID: "move-smackdown-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} fell straight down!" },
                }
                ) },

        // Misc.
        {"move-afteryou-default",
            new GameTextData(
                ID: "move-afteryou-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} took the kind offer!" },
                }
                ) },
        {"move-celebrate",
            new GameTextData(
                ID: "move-celebrate",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Congratulations {{-player-name-}}!" },
                }
                ) },
        {"move-destinybond-start-default",
            new GameTextData(
                ID: "move-destinybond-start-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is trying to take its attacker down with it!" },
                }
                ) },
        {"move-destinybond-success-default",
            new GameTextData(
                ID: "move-destinybond-success-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} took its attacker down with it!" },
                }
                ) },
        {"move-focuspunch-charge-default",
            new GameTextData(
                ID: "move-focuspunch-charge-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} is tightening its focus!" },
                }
                ) },
        {"move-focuspunch-fail-default",
            new GameTextData(
                ID: "move-focuspunch-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} lost its focus and couldn't move!" },
                }
                ) },

        {"move-followme-default",
            new GameTextData(
                ID: "move-followme-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} became the center of attention!" },
                }
                ) },

        {"move-holdhands-default",
            new GameTextData(
                ID: "move-holdhands-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} and {{-target-pokemon-}} held hands!" },
                }
                ) },

        {"move-magiccoat-magiccoat",
            new GameTextData(
                ID: "move-magiccoat-magiccoat",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} was reflected back!" },
                }
                ) },
        {"move-magiccoat-default",
            new GameTextData(
                ID: "move-magiccoat-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-move-name-}} was reflected back!" },
                }
                ) },

        {"move-quash-default",
            new GameTextData(
                ID: "move-quash-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} move was postponed!" },
                }
                ) },

        {"move-thrash-default",
            new GameTextData(
                ID: "move-thrash-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} became confused due to fatigue!" },
                }
                ) },

        {"move-mimic-success-default",
            new GameTextData(
                ID: "move-mimic-success-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} learned {{-move-name-}}!" },
                }
                ) },

        {"move-shelltrap-charge-default",
            new GameTextData(
                ID: "move-shelltrap-charge-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} set a shell trap!" },
                }
                ) },
        {"move-shelltrap-fail-default",
            new GameTextData(
                ID: "move-shelltrap-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-poss-}} shell trap didn't work!" },
                }
                ) },

        {"move-sketch-success-default",
            new GameTextData(
                ID: "move-sketch-success-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} sketched {{-move-name-}}!" },
                }
                ) },

        {"move-snatch-wait-default",
            new GameTextData(
                ID: "move-snatch-wait-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} waits for a target to make a move!" },
                }
                ) },
        {"move-snatch-success-default",
            new GameTextData(
                ID: "move-snatch-success-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} snatched {{-target-pokemon-poss-}} move!" },
                }
                ) },

        {"move-substitute-create-default",
            new GameTextData(
                ID: "move-substitute-create-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} put in a substitute!" },
                }
                ) },
        {"move-substitute-destroy-default",
            new GameTextData(
                ID: "move-substitute-destroy-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} substitute faded!" },
                }
                ) },
        {"move-substitute-already-default",
            new GameTextData(
                ID: "move-substitute-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already behind a substitute!" },
                }
                ) },
        {"move-substitute-fail-default",
            new GameTextData(
                ID: "move-substitute-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} doesn't have enough HP for a substitute!" },
                }
                ) },
        {"move-substitute-block-default",
            new GameTextData(
                ID: "move-substitute-block-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} substitute blocked the attack!" },
                }
                ) },
        {"move-substitute-damage-default",
            new GameTextData(
                ID: "move-substitute-damage-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} substitute took the damage!" },
                }
                ) },

        {"move-transform-default",
            new GameTextData(
                ID: "move-transform-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} transformed into {{-target-pokemon-}}!" },
                }
                ) },

        {"move-whirlwind-out-whirlwind",
            new GameTextData(
                ID: "move-whirlwind-out-whirlwind",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was blown away!" },
                }
                ) },
        {"move-whirlwind-in-default",
            new GameTextData(
                ID: "move-whirlwind-in-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was dragged out!" },
                }
                ) },
        {"move-whirlwind-out-default",
            new GameTextData(
                ID: "move-whirlwind-out-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was forced out!" },
                }
                ) },
        {"move-whirlwind-fail-default",
            new GameTextData(
                ID: "move-whirlwind-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be forced out!" },
                }
                ) },

        // STATS

        // Stat names
        {"stat-hp",
            new GameTextData(
                ID: "stat-hp",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Hit Points" },
                }
                ) },
        {"stat-attack",
            new GameTextData(
                ID: "stat-attack",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Attack" },
                }
                ) },
        {"stat-defense",
            new GameTextData(
                ID: "stat-defense",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Defense" },
                }
                ) },
        {"stat-special-attack",
            new GameTextData(
                ID: "stat-special-attack",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Special Attack" },
                }
                ) },
        {"stat-special-defense",
            new GameTextData(
                ID: "stat-special-defense",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Special Defense" },
                }
                ) },
        {"stat-speed",
            new GameTextData(
                ID: "stat-speed",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Speed" },
                }
                ) },
        {"stat-accuracy",
            new GameTextData(
                ID: "stat-accuracy",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Accuracy" },
                }
                ) },
        {"stat-evasion",
            new GameTextData(
                ID: "stat-evasion",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Evasion" },
                }
                ) },

        // Stat stage changes
        {"stats-up1",
            new GameTextData(
                ID: "stats-up1",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} rose!" },
                }
                ) },
        {"stats-up2",
            new GameTextData(
                ID: "stats-up2",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} rose sharply!" },
                }
                ) },
        {"stats-up3",
            new GameTextData(
                ID: "stats-up3",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} rose drastically!" },
                }
                ) },
        {"stats-down1",
            new GameTextData(
                ID: "stats-down1",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} fell!" },
                }
                ) },
        {"stats-down2",
            new GameTextData(
                ID: "stats-down2",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} fell sharply!" },
                }
                ) },
        {"stats-down3",
            new GameTextData(
                ID: "stats-down3",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} fell drastically!" },
                }
                ) },
        {"stats-maximize",
            new GameTextData(
                ID: "stats-maximize",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} {{-stat-types-was-}} maximized!" },
                }
                ) },
        {"stats-minimize",
            new GameTextData(
                ID: "stats-minimize",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} {{-stat-types-was-}} minimized!" },
                }
                ) },
        {"stats-max",
            new GameTextData(
                ID: "stats-max",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} cannot go any higher!" },
                }
                ) },
        {"stats-min",
            new GameTextData(
                ID: "stats-min",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-stat-types-}} cannot go any lower!" },
                }
                ) },

        // STATUS CONDITIONS

        // default
        {"status-inflict-default",
            new GameTextData(
                ID: "status-inflict-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was inflicted with {{-status-name-}}!" },
                }
                ) },
        {"status-heal-default",
            new GameTextData(
                ID: "status-heal-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was healed from {{-status-name-}}!" },
                }
                ) },
        {"status-item-default",
            new GameTextData(
                ID: "status-item-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} cured it from {{-status-name-}}!" },
                }
                ) },
        {"status-already-default",
            new GameTextData(
                ID: "status-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already affected by {{-status-name-}}!" },
                }
                ) },
        {"status-fail-default",
            new GameTextData(
                ID: "status-fail-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be induced with {{-status-name-}}!" },
                }
                ) },
        {"status-hploss-default",
            new GameTextData(
                ID: "status-hploss-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt due to {{-status-name-}}!" },
                }
                ) },
        {"status-flinch-default",
            new GameTextData(
                ID: "status-flinch-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} flinched!" },
                }
                ) },
        {"status-free-default",
            new GameTextData(
                ID: "status-free-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was freed from {{-move-name-}}!" },
                }
                ) },
        {"status-sleep",
            new GameTextData(
                ID: "status-sleep",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is fast asleep." },
                }
                ) },
        {"status-sleep-default",
            new GameTextData(
                ID: "status-sleep-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is fast asleep." },
                }
                ) },
        {"status-freeze",
            new GameTextData(
                ID: "status-freeze",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is frozen solid!" },
                }
                ) },
        {"status-freeze-default",
            new GameTextData(
                ID: "status-freeze-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is frozen solid!" },
                }
                ) },
        {"status-paralysis",
            new GameTextData(
                ID: "status-paralysis",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is paralyzed! It can't move!" },
                }
                ) },
        {"status-trap-default",
            new GameTextData(
                ID: "status-trap-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is prevented from switching out due to {{-move-name-}}!" },
                }
                ) },
        {"status-confusion-idle",
            new GameTextData(
                ID: "status-confusion-idle",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is confused!" },
                }
                ) },
        {"status-confusion-hit",
            new GameTextData(
                ID: "status-confusion-hit",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It hurt itself in its confusion!" },
                }
                ) },

        // Burn
        {"status-burn-inflict",
            new GameTextData(
                ID: "status-burn-inflict",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was burned!" },
                }
                ) },
        {"statu-burn-heal",
            new GameTextData(
                ID: "status-burn-heal",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was healed from its burn!" },
                }
                ) },
        {"status-item-burn",
            new GameTextData(
                ID: "status-item-burn",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} cured it of its burn." },
                }
                ) },
        {"status-burn-already",
            new GameTextData(
                ID: "status-burn-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already burned." },
                }
                ) },
        {"statu-burn-fail",
            new GameTextData(
                ID: "status-burn-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be burned!" },
                }
                ) },
        {"status-burn-hploss",
            new GameTextData(
                ID: "status-burn-hploss",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt by its burn!" },
                }
                ) },

        // Freeze
        {"status-inflict-freeze",
            new GameTextData(
                ID: "status-inflict-freeze",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was frozen solid!" },
                }
                ) },
        {"status-heal-freeze",
            new GameTextData(
                ID: "status-heal-freeze",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} thawed out!" },
                }
                ) },
        {"status-item-freeze",
            new GameTextData(
                ID: "status-item-freeze",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} thawed it out." },
                }
                ) },
        {"status-fail-freeze",
            new GameTextData(
                ID: "status-fail-freeze",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be frozen!" },
                }
                ) },

        // Paralysis
        {"status-inflict-paralysis",
            new GameTextData(
                ID: "status-inflict-paralysis",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was paralyzed!" },
                }
                ) },
        {"status-heal-paralysis",
            new GameTextData(
                ID: "status-heal-paralysis",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was healed from its paralysis!" },
                }
                ) },
        {"status-item-paralysis",
            new GameTextData(
                ID: "status-item-paralysis",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} cured it of its paralysis." },
                }
                ) },
        {"status-already-paralysis",
            new GameTextData(
                ID: "status-already-paralysis",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already paralyzed." },
                }
                ) },
        {"status-fail-paralysis",
            new GameTextData(
                ID: "status-fail-paralysis",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be paralyzed!" },
                }
                ) },

        // Poison
        {"status-poison-start",
            new GameTextData(
                ID: "status-poison-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was poisoned!" },
                }
                ) },
        {"status-poison-end",
            new GameTextData(
                ID: "status-poison-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was healed from poison!" },
                }
                ) },
        {"status-item-poison",
            new GameTextData(
                ID: "status-item-poison",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} cured it of its poison." },
                }
                ) },
        {"status-poison-already",
            new GameTextData(
                ID: "status-poison-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already poisoned." },
                }
                ) },
        {"status-poison-fail",
            new GameTextData(
                ID: "status-poison-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be poisoned!" },
                }
                ) },
        {"status-poison-hploss",
            new GameTextData(
                ID: "status-poison-hploss",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was hurt by poison!" },
                }
                ) },

        // Sleep
        {"status-inflict-sleep",
            new GameTextData(
                ID: "status-inflict-sleep",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} fell asleep!" },
                }
                ) },
        {"status-heal-sleep",
            new GameTextData(
                ID: "status-heal-sleep",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} woke up!" },
                }
                ) },
        {"status-item-sleep",
            new GameTextData(
                ID: "status-item-sleep",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} woke it up!" },
                }
                ) },
        {"status-already-sleep",
            new GameTextData(
                ID: "status-already-sleep",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already asleep." },
                }
                ) },
        {"status-fail-sleep",
            new GameTextData(
                ID: "status-fail-sleep",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be put to sleep!" },
                }
                ) },

        // Toxic
        {"status-inflict-poison2",
            new GameTextData(
                ID: "status-inflict-poison2",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was badly poisoned!" },
                }
                ) },

        // Bind
        {"status-inflict-bound",
            new GameTextData(
                ID: "status-inflict-bound",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was squeezed by {{-user-pokemon-}}!" },
                }
                ) },
        {"status-hploss-bound",
            new GameTextData(
                ID: "status-hploss-bound",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was squeezed by {{-user-pokemon-}}!" },
                }
                ) },

        // Confusion
        {"status-inflict-confusion",
            new GameTextData(
                ID: "status-inflict-confusion",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} became confused!" },
                }
                ) },
        {"status-heal-confusion",
            new GameTextData(
                ID: "status-heal-confusion",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} snapped out of its confusion!" },
                }
                ) },
        {"status-item-confusion",
            new GameTextData(
                ID: "status-item-confusion",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-item-name-}} snapped it out of confusion." },
                }
                ) },
        {"status-already-confusion",
            new GameTextData(
                ID: "status-already-confusion",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already confused." },
                }
                ) },

        // Disable
        {"status-disable-attempt",
            new GameTextData(
                ID: "status-disable-attempt",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-poss-}} {{-status-afflict-move-}} is disabled!" },
                }
                ) },
        {"status-disable-start",
            new GameTextData(
                ID: "status-disable-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} {{-status-afflict-move-}} was disabled!" },
                }
                ) },
        {"status-disable-end",
            new GameTextData(
                ID: "status-disable-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is no longer disabled!" },
                }
                ) },
        {"status-disable-already",
            new GameTextData(
                ID: "status-disable-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} is already disabled!" },
                }
                ) },
        {"status-disable-use-default",
            new GameTextData(
                ID: "status-disable-use-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-move-name-}} because it's disabled!" },
                }
                ) },
        {"status-disable-fail",
            new GameTextData(
                ID: "status-disable-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be disabled!" },
                }
                ) },

        // Encore
        {"status-encore-attempt",
            new GameTextData(
                ID: "status-encore-attempt",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} may only use {{-move-name-}} due to encore!" },
                }
                ) },
        {"status-encore-start",
            new GameTextData(
                ID: "status-encore-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} received an encore!" },
                }
                ) },
        {"status-encore-end",
            new GameTextData(
                ID: "status-encore-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is no longer encored!" },
                }
                ) },
        {"status-encore-already",
            new GameTextData(
                ID: "status-encore-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already encored!" },
                }
                ) },
        {"status-encore-fail",
            new GameTextData(
                ID: "status-encore-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be encored!" },
                }
                ) },
                
        // Heal Block
        {"status-healblock-start",
            new GameTextData(
                ID: "status-healblock-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was prevented from healing!" },
                }
                ) },
        {"status-healblock-end",
            new GameTextData(
                ID: "status-healblock-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was freed from {{-move-name-}}!" },
                }
                ) },
        {"status-healblock-attempt",
            new GameTextData(
                ID: "status-healblock-attempt",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-move-name-}}! It is prevented from healing!" },
                }
                ) },
        {"status-healblock-already",
            new GameTextData(
                ID: "status-healblock-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already prevented from healing!" },
                }
                ) },
        {"status-healblock-fail",
            new GameTextData(
                ID: "status-healblock-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be prevented from healing!" },
                }
                ) },
                
        // Taunt
        {"status-taunt-start",
            new GameTextData(
                ID: "status-taunt-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} fell for the taunt!" },
                }
                ) },
        {"status-taunt-end",
            new GameTextData(
                ID: "status-taunt-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-poss-}} taunt wore off!" },
                }
                ) },
        {"status-taunt-attempt",
            new GameTextData(
                ID: "status-taunt-attempt",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-move-name-}} due to the taunt!" },
                }
                ) },
        {"status-taunt-already",
            new GameTextData(
                ID: "status-taunt-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already taunted!" },
                }
                ) },
        {"status-taunt-fail",
            new GameTextData(
                ID: "status-taunt-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be taunted!" },
                }
                ) },
                
        // Torment
        {"status-torment-start",
            new GameTextData(
                ID: "status-torment-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} was subjected to torment!" },
                }
                ) },
        {"status-torment-end",
            new GameTextData(
                ID: "status-torment-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is no longer subjected to torment!" },
                }
                ) },
        {"status-torment-attempt",
            new GameTextData(
                ID: "status-torment-attempt",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-move-name-}} due to the torment!" },
                }
                ) },
        {"status-torment-already",
            new GameTextData(
                ID: "status-torment-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} is already subjected to torment!" },
                }
                ) },
        {"status-torment-fail",
            new GameTextData(
                ID: "status-torment-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} cannot be subjected to torment!" },
                }
                ) },

        // TEAM CONDITIONS
        {"team-status-inflict-default",
            new GameTextData(
                ID: "team-status-inflict-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is now affected by {{-team-status-name-}}!" },
                }
                ) },
        {"team-status-heal-default",
            new GameTextData(
                ID: "team-status-heal-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is no longer affected by {{-team-status-name-}}!" },
                }
                ) },
        {"team-status-already-default",
            new GameTextData(
                ID: "team-status-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is already affected by {{-team-status-name-}}!" },
                }
                ) },
                
        // Aurora Veil
        {"tStatus-auroraveil-start",
            new GameTextData(
                ID: "tStatus-auroraveil-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Aurora Veil will reduce damage taken for {{-target-team-}}!" },
                }
                ) },
        {"tStatus-auroraveil-end",
            new GameTextData(
                ID: "tStatus-auroraveil-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}}'s Aurora Veil will wore off!" },
                }
                ) },
        {"tStatus-auroraveil-already",
            new GameTextData(
                ID: "tStatus-auroraveil-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Aurora Veil is already active for {{-target-team-}}!" },
                }
                ) },
        {"tStatus-auroraveil-fail",
            new GameTextData(
                ID: "tStatus-auroraveil-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Aurora Veil could not be created for {{-target-team-}}!" },
                }
                ) },
                
        // Light Screen
        {"tStatus-lightscreen-start",
            new GameTextData(
                ID: "tStatus-lightscreen-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Light Screen will reduce special damage taken for {{-target-team-}}!" },
                }
                ) },
        {"tStatus-lightscreen-end",
            new GameTextData(
                ID: "tStatus-lightscreen-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}}'s Light Screen will wore off!" },
                }
                ) },
        {"tStatus-lightscreen-already",
            new GameTextData(
                ID: "tStatus-lightscreen-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Light Screen is already active for {{-target-team-}}!" },
                }
                ) },
        {"tStatus-lightscreen-fail",
            new GameTextData(
                ID: "tStatus-lightscreen-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Light Screen could not be created for {{-target-team-}}!" },
                }
                ) },
                
        // Reflect
        {"tStatus-reflect-start",
            new GameTextData(
                ID: "tStatus-lightscreen-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Light Screen will reduce physical damage taken for {{-target-team-}}!" },
                }
                ) },
        {"tStatus-reflect-end",
            new GameTextData(
                ID: "tStatus-reflect-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}}'s Reflect will wore off!" },
                }
                ) },
        {"tStatus-reflect-already",
            new GameTextData(
                ID: "tStatus-reflect-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Reflect is already active for {{-target-team-}}!" },
                }
                ) },
        {"tStatus-reflect-fail",
            new GameTextData(
                ID: "tStatus-reflect-fail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Reflect could not be created for {{-target-team-}}!" },
                }
                ) },

        // Mist
        {"team-status-inflict-mist",
            new GameTextData(
                ID: "team-status-inflict-mist",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} became shrouded in mist!" },
                }
                ) },
        {"team-status-heal-mist",
            new GameTextData(
                ID: "team-status-heal-mist",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mist shrouding {{-target-team-}} has dissipated!" },
                }
                ) },
        {"team-status-mist-default",
            new GameTextData(
                ID: "team-status-mist-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mist prevented {{-target-team-}}'s stats from being lowered!" },
                }
                ) },
        {"team-status-already-mist",
            new GameTextData(
                ID: "team-status-already-mist",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-team-}} is already shrouded in mist!" },
                }
                ) },

        {"team-status-mist-test",
            new GameTextData(
                ID: "team-status-mist-test",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mist prevented {{-target-team-}}'s {{-stat-types-LC-}} from being lowered!" },
                }
                ) },

        // BATTLE CONDITIONS
        // default
        {"battle-status-inflict-default",
            new GameTextData(
                ID: "battle-status-inflict-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The battlefield is now affected by {{-battle-status-name-}}!" },
                }
                ) },
        {"battle-status-nature-default",
            new GameTextData(
                ID: "battle-status-nature-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The battlefield is being affected by {{-battle-status-name-}}!" },
                }
                ) },
        {"battle-status-heal-default",
            new GameTextData(
                ID: "battle-status-heal-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The battlefield is no longer affected by {{-battle-status-name-}}!" },
                }
                ) },
        {"bStatus-default-hploss",
            new GameTextData(
                ID: "battle-status-hploss-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The Pokémon were hurt due to {{-battle-status-name-}}!" },
                }
                ) },
        {"battle-status-strongwinds",
            new GameTextData(
                ID: "battle-status-strongwinds",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The battle conditions altered the attack's power!" },
                }
                ) },
        {"battle-status-already-default",
            new GameTextData(
                ID: "battle-status-already-default",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The battlefield is already affected by {{-battle-status-name-}}!" },
                }
                ) },

        // MOVES
        {"battle-status-inflict-iondeluge",
            new GameTextData(
                ID: "battle-status-inflict-iondeluge",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "A deluge of ions showered the battlefield!" },
                }
                ) },

        // GRAVITY
        {"bStatus-gravity-start",
            new GameTextData(
                ID: "bStatus-gravity-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Gravity intensified!" },
                }
                ) },
        {"bStatus-gravity-end",
            new GameTextData(
                ID: "bStatus-gravity-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Gravity returned to normal!" },
                }
                ) },
        {"bStatus-gravity-intensify",
            new GameTextData(
                ID: "bStatus-gravity-intensify",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The Pokémon were brought back down to the ground!" },
                }
                ) },
        {"bStatus-gravity-movefail",
            new GameTextData(
                ID: "bStatus-gravity-movefail",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} can't use {{-move-name-}} due to the intense gravity!" },
                }
                ) },

        // ROOMS
        {"bStatus-magicroom-start",
            new GameTextData(
                ID: "bStatus-magicroom-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It created a bizarre area in which Pokémon's held items lose their effects!" },
                }
                ) },
        {"bStatus-magicroom-end",
            new GameTextData(
                ID: "bStatus-magicroom-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The bizarre area disappeared. Pokémon's held items regained their effects!" },
                }
                ) },

        {"battle-status-inflict-trickroom",
            new GameTextData(
                ID: "battle-status-inflict-trickroom",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-user-pokemon-}} twisted the dimensions!" },
                }
                ) },
        {"battle-status-heal-trickroom",
            new GameTextData(
                ID: "battle-status-heal-trickroom",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The twisted dimensions returned to normal!" },
                }
                ) },

        {"battle-status-inflict-wonderroom",
            new GameTextData(
                ID: "battle-status-inflict-wonderroom",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It created a bizarre area in which the Defense and Sp. Def stats are swapped!" },
                }
                ) },
        {"battle-status-heal-wonderroom",
            new GameTextData(
                ID: "battle-status-heal-wonderroom",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The bizarre area disappeared. Stats were reverted to normal!" },
                }
                ) },

        // TERRAIN

        // Electric Terrain
        {"terrain-electricterrain-start",
            new GameTextData(
                ID: "terrain-electricterrain-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "An electric current ran across the battlefield!" },
                }
                ) },
        {"terrain-electricterrain-nature",
            new GameTextData(
                ID: "terrain-electricterrain-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "An electric current runs across the battlefield!" },
                }
                ) },
        {"terrain-electricterrain-end",
            new GameTextData(
                ID: "terrain-electricterrain-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The electric current disappeared from the battlefield." },
                }
                ) },
        {"terrain-electricterrain-already",
            new GameTextData(
                ID: "terrain-electricterrain-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "An electric current is already on the battlefield." },
                }
                ) },
        {"terrain-electricterrain-block",
            new GameTextData(
                ID: "terrain-electricterrain-block",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} surrounds itself with electric terrain!" },
                }
                ) },
        
        // Grassy Terrain
        {"terrain-grassyterrain-start",
            new GameTextData(
                ID: "terrain-grassyterrain-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Grass grew to cover the battlefield!" },
                }
                ) },
        {"terrain-grassyterrain-nature",
            new GameTextData(
                ID: "terrain-grassyterrain-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Grass covers the battlefield!" },
                }
                ) },
        {"terrain-grassyterrain-end",
            new GameTextData(
                ID: "terrain-grassyterrain-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The grass disappeared from the battlefield." },
                }
                ) },
        {"terrain-grassyterrain-already",
            new GameTextData(
                ID: "terrain-grassyterrain-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Grass already covers the battlefield." },
                }
                ) },
        {"terrain-grassyterrain-heal",
            new GameTextData(
                ID: "terrain-grassyterrain-heal",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The Pokémon on the grassy terrain had their HP restored." },
                }
                ) },
                
        // Misty Terrain
        {"terrain-mistyterrain-start",
            new GameTextData(
                ID: "terrain-mistyterrain-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Mist swirled around the battlefield!" },
                }
                ) },
        {"terrain-mistyterrain-nature",
            new GameTextData(
                ID: "terrain-mistyterrain-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Mist swirls arounds the battlefield!" },
                }
                ) },
        {"terrain-mistyterrain-end",
            new GameTextData(
                ID: "terrain-mistyterrain-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mist disappeared from the battlefield." },
                }
                ) },
        {"terrain-mistyterrain-already",
            new GameTextData(
                ID: "terrain-mistyterrain-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Mist already swirls around the battlefield." },
                }
                ) },
        {"terrain-mistyterrain-block",
            new GameTextData(
                ID: "terrain-mistyterrain-block",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} surrounds itself with misty terrain!" },
                }
                ) },
        
        // Psychic Terrain
        {"terrain-psychicterrain-start",
            new GameTextData(
                ID: "terrain-psychicterrain-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The battlefield got weird!" },
                }
                ) },
        {"terrain-psychicterrain-nature",
            new GameTextData(
                ID: "terrain-psychicterrain-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The battlefield is weird!" },
                }
                ) },
        {"terrain-psychicterrain-end",
            new GameTextData(
                ID: "terrain-psychicterrain-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The weirdness disappeared from the battlefield." },
                }
                ) },
        {"terrain-psychicterrain-already",
            new GameTextData(
                ID: "terrain-psychicterrain-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The battlefield is already weird." },
                }
                ) },
        {"terrain-psychicterrain-block",
            new GameTextData(
                ID: "terrain-psychicterrain-block",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "{{-target-pokemon-}} surrounds itself with psychic terrain!" },
                }
                ) },
                

        // WEATHER

        {"weather-clearskies-start",
            new GameTextData(
                ID: "weather-clearskies-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The weather cleared up!" },
                }
                ) },

        // Extremely Harsh Sunlight
        {"weather-extremelyharshsunlight-start",
            new GameTextData(
                ID: "weather-extremelyharshsunlight-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sunlight turned extremely harsh!" },
                }
                ) },
        {"weather-extremelyharshsunlight-nature",
            new GameTextData(
                ID: "weather-extremelyharshsunlight-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sunlight is extremely harsh!" },
                }
                ) },
        {"weather-extremelyharshsunlight-end",
            new GameTextData(
                ID: "weather-extremelyharshsunlight-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The extremely harsh sunlight faded." },
                }
                ) },
        {"weather-extremelyharshsunlight-already",
            new GameTextData(
                ID: "weather-extremelyharshsunlight-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sunlight is already extremely harsh!" },
                }
                ) },
        {"weather-extremelyharshsunlight-negate",
            new GameTextData(
                ID: "weather-extremelyharshsunlight-negate",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The {{-type-name-}} attack evaporated in the harsh sunlight!" },
                }
                ) },
        {"weather-extremelyharshsunlight-block",
            new GameTextData(
                ID: "weather-extremelyharshsunlight-block",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The extremely harsh sunlight was not lessened at all!" },
                }
                ) },
                
        // Fog
        {"weather-fog-start",
            new GameTextData(
                ID: "weather-fog-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "A deep fog settled in..." },
                }
                ) },
        {"weather-fog-nature",
            new GameTextData(
                ID: "weather-fog-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The fog is deep..." },
                }
                ) },
        {"weather-fog-end",
            new GameTextData(
                ID: "weather-fog-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The deep fog was dispelled." },
                }
                ) },
        {"weather-fog-already",
            new GameTextData(
                ID: "weather-fog-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "There's already a deep fog!" },
                }
                ) },

        // Hail
        {"weather-hail-start",
            new GameTextData(
                ID: "weather-hail-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It began to hail!" },
                }
                ) },
        {"weather-hail-nature",
            new GameTextData(
                ID: "weather-hail-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It's hailing!" },
                }
                ) },
        {"weather-hail-end",
            new GameTextData(
                ID: "weather-hail-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The hail stopped." },
                }
                ) },
        {"weather-hail-buffet",
            new GameTextData(
                ID: "weather-hail-buffet",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The Pokémon were buffeted by the hail!" },
                }
                ) },
        {"weather-hail-already",
            new GameTextData(
                ID: "weather-hail-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It's already hailing!" },
                }
                ) },

        // Harsh Sunlight
        {"weather-harshsunlight-start",
            new GameTextData(
                ID: "weather-harshsunlight-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sunlight turned harsh!" },
                }
                ) },
        {"weather-harshsunlight-nature",
            new GameTextData(
                ID: "weather-harshsunlight-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sunlight is harsh!" },
                }
                ) },
        {"weather-harshsunlight-end",
            new GameTextData(
                ID: "weather-harshsunlight-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The harsh sunlight faded." },
                }
                ) },
        {"weather-harshsunlight-already",
            new GameTextData(
                ID: "weather-harshsunlight-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sunlight is already harsh!" },
                }
                ) },
                
        // Heavy Rain
        {"weather-heavyrain-start",
            new GameTextData(
                ID: "weather-heavyrain-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "A heavy rain begain to fall!" },
                }
                ) },
        {"weather-heavyrain-nature",
            new GameTextData(
                ID: "weather-heavyrain-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It's raining heavily!" },
                }
                ) },
        {"weather-heavyrain-end",
            new GameTextData(
                ID: "weather-heavyrain-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The heavy rain has lifted!" },
                }
                ) },
        {"weather-heavyrain-already",
            new GameTextData(
                ID: "weather-heavyrain-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It's already raining heavily!" },
                }
                ) },
        {"weather-heavyrain-negate",
            new GameTextData(
                ID: "weather-heavyrain-negate",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The {{-type-name-}} attack fizzled out in the heavy rain!" },
                }
                ) },
        {"weather-heavyrain-block",
            new GameTextData(
                ID: "weather-heavyrain-block",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "There is no relief from this heavy rain!" },
                }
                ) },
                
        // Rain
        {"weather-rain-start",
            new GameTextData(
                ID: "weather-rain-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It started to rain!" },
                }
                ) },
        {"weather-rain-nature",
            new GameTextData(
                ID: "weather-rain-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It's raining!" },
                }
                ) },
        {"weather-rain-end",
            new GameTextData(
                ID: "weather-rain-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The rain stopped." },
                }
                ) },
        {"weather-rain-already",
            new GameTextData(
                ID: "weather-rain-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "It's already raining!" },
                }
                ) },
                
        // Sandstorm
        {"weather-sandstorm-start",
            new GameTextData(
                ID: "weather-sandstorm-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "A sandstorm kicked up!" },
                }
                ) },
        {"weather-sandstorm-nature",
            new GameTextData(
                ID: "weather-sandstorm-nature",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sandstorm is raging!" },
                }
                ) },
        {"weather-sandstorm-end",
            new GameTextData(
                ID: "weather-sandstorm-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The sandstorm subsided." },
                }
                ) },
        {"weather-sandstorm-buffet",
            new GameTextData(
                ID: "weather-sandstorm-buffet",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The Pokémon were buffeted by the sandstorm!" },
                }
                ) },
        {"weather-sandstorm-already",
            new GameTextData(
                ID: "weather-sandstorm-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "A sandstorm is already raging!" },
                }
                ) },
                
        // Strong Winds
        {"weather-strongwinds-start",
            new GameTextData(
                ID: "weather-strongwinds-start",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "Mysterious strong winds are protecting Flying-type Pokémon!" },
                }
                ) },
        {"weather-strongwinds-end",
            new GameTextData(
                ID: "weather-strongwinds-end",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mysterious strong winds have dissipated!" },
                }
                ) },
        {"weather-strongwinds-already",
            new GameTextData(
                ID: "weather-strongwinds-already",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mysterious strong winds continue to blow on!" },
                }
                ) },
        {"weather-strongwinds-weaken",
            new GameTextData(
                ID: "weather-strongwinds-weaken",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mysterious strong winds weakened the attack!" },
                }
                ) },
        {"weather-strongwinds-block",
            new GameTextData(
                ID: "weather-strongwinds-block",
                languageDict: new Dictionary<GameLanguages, string>
                {
                    { GameLanguages.English, "The mysterious strong winds blow on regardless!" },
                }
                ) },


    };

    // Methods
    public GameTextData GetGameTextData(string ID)
    {
        if (database.ContainsKey(ID))
        {
            return database[ID];
        }
        Debug.LogWarning("Could not find move with ID: " + ID);
        return database[""];
    }

    public static string ConvertToString(
        string baseString,
        int playerID = 0,
        int viewPos = 0,
        Pokemon pokemon = null,
        Pokemon userPokemon = null,
        Pokemon targetPokemon = null,
        Pokemon[] pokemonList = null,
        Trainer trainer = null,
        BattleTeam targetTeam = null,
        PokemonStats[] statList = null,

        string typeID = null,
        string moveID = null,
        string abilityID = null,
        string itemID = null,
        string statusID = null,
        string teamStatusID = null,
        string battleStatusID = null,

        string[] moveIDs = null,
        string[] typeIDs = null,

        Battle battleModel = null,

        List<int> intArgs = null
        )
    {
        // set player references
        BattleTeam playerTeam = null;
        Trainer playerTrainer = null;
        if (battleModel != null)
        {
            playerTeam = battleModel.GetTeamFromPosition(viewPos);
            playerTrainer = battleModel.GetTrainerWithID(playerID);
        }

        // set core variables
        string newString = baseString;
        TypeData typeData = (typeID == null) ? null : TypeDatabase.instance.GetTypeData(typeID);
        MoveData moveData = (moveID == null) ? null : MoveDatabase.instance.GetMoveData(moveID);
        AbilityData abilityData = (abilityID == null) ? null : AbilityDatabase.instance.GetAbilityData(abilityID);
        ItemData itemData = (itemID == null) ? null : ItemDatabase.instance.GetItemData(itemID);
        StatusPKData statusData = (statusID == null) ? null 
            : StatusPKDatabase.instance.GetStatusData(statusID);
        StatusTEData teamStatusData = (teamStatusID == null) ? null
            : StatusTEDatabase.instance.GetStatusData(teamStatusID);
        StatusBTLData battleStatusData = (battleStatusID == null) ? null
            : StatusBTLDatabase.instance.GetStatusData(battleStatusID);

        // swapping substrings
        intArgs = (intArgs == null) ? new List<int>() : intArgs;
        for (int i = 0; i < intArgs.Count; i++)
        {
            string partToReplace = "{{-int-" + i + "-}}";
            newString = newString.Replace(partToReplace, intArgs[i].ToString());
        }

        // Replacing

        // player
        newString = newString.Replace("{{-player-name-}}", PlayerSave.instance.name);

        if (pokemon != null)
        {
            newString = newString.Replace("{{-pokemon-}}", pokemon.nickname);
            newString = newString.Replace("{{-pokemon-poss-}}", pokemon.nickname
                + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                );
        }
        if (userPokemon != null)
        {
            string name = userPokemon.nickname;
            string prefix = "";
            if (battleModel != null)
            {
                Trainer ownerTrainer = battleModel.GetPokemonOwner(userPokemon);
                BattleTeam team = battleModel.GetTeam(userPokemon);

                if (playerTeam != team)
                {
                    if (battleModel.battleSettings.isWildBattle)
                    {
                        prefix = "The wild ";
                    }
                    else
                    {
                        prefix = "The foe's ";
                    }
                }
                else if (playerTrainer != ownerTrainer)
                {
                    prefix = "The ally ";
                }
                else
                {
                    prefix = "";
                }
            }
            prefix = newString.StartsWith("{{-user-pokemon-") ? prefix : prefix.ToLower();

            newString = newString.Replace("{{-user-pokemon-}}", prefix + name);
            newString = newString.Replace("{{-user-pokemon-form-}}", userPokemon.data.formName);
            newString = newString.Replace("{{-user-pokemon-poss-}}", prefix + (name
                + (name.EndsWith("s") ? "'" : "'s"))
                );
        }
        if (targetPokemon != null)
        {
            string name = targetPokemon.nickname;
            string prefix = "";
            if (battleModel != null)
            {
                Trainer ownerTrainer = battleModel.GetPokemonOwner(targetPokemon);
                BattleTeam team = battleModel.GetTeam(targetPokemon);
                if (playerTeam != team)
                {
                    if (battleModel.battleSettings.isWildBattle)
                    {
                        prefix = "The wild ";
                    }
                    else
                    {
                        prefix = "The foe's ";
                    }
                }
                else if (playerTrainer != ownerTrainer)
                {
                    prefix = "The ally";
                }
                else
                {
                    prefix = "";
                }
            }
            prefix = newString.StartsWith("{{-target-pokemon-") ? prefix : prefix.ToLower();

            newString = newString.Replace("{{-target-pokemon-}}", prefix + name);
            newString = newString.Replace("{{-target-pokemon-poss-}}", prefix + name
                + (name.EndsWith("s") ? "'" : "'s")
                );
        }
        if (pokemonList != null)
        {
            string pokemonNameList = GetPokemonNames(new List<Pokemon>(pokemonList));
            newString = newString.Replace("{{-pokemon-list-}}", pokemonNameList);
        }
        if (trainer != null)
        {
            string trainerName = trainer.name;
            string trainerNamePoss = trainerName + (trainerName.EndsWith("s") ? "'" : "'s");
            if (trainer == playerTrainer)
            {
                trainerName = "You";
                trainerNamePoss = "Your";
            }
            newString = newString.Replace("{{-trainer-}}", trainerName);
            newString = newString.Replace("{{-trainer-poss-}}", trainerNamePoss);
            if (trainer == playerTrainer)
            {
                newString = newString.Replace("{{-trainer-LC-}}", trainerName.ToLower());
                newString = newString.Replace("{{-trainer-poss-LC-}}", trainerNamePoss.ToLower());
            }
            else
            {
                newString = newString.Replace("{{-trainer-LC-}}", trainerName);
                newString = newString.Replace("{{-trainer-poss-LC-}}", trainerNamePoss);
            }
        }
        if (targetTeam != null)
        {
            string yourTeamStr = (targetTeam == playerTeam) ? "Your team"
                    : "The opposing team";
            yourTeamStr = newString.StartsWith("{{-target-team-") ? yourTeamStr : yourTeamStr.ToLower();
            newString = newString.Replace("{{-target-team-}}", yourTeamStr);
            newString = newString.Replace("{{-target-team-poss-}}", yourTeamStr
                + (yourTeamStr.EndsWith("s") ? "'" : "'s")
                );
        }
        if (statList != null)
        {
            newString = newString.Replace("{{-stat-types-}}", ConvertStatsToString(statList));
            newString = newString.Replace("{{-stat-types-was-}}", (statList.Length == 1)? "was" : "were");
            newString = newString.Replace("{{-stat-types-LC-}}", ConvertStatsToString(statList, false));
        }
        if (typeData != null)
        {
            newString = newString.Replace("{{-type-name-}}", typeData.typeName + "-type");
        }
        if (typeIDs != null)
        {
            newString = newString.Replace("{{-type-list-}}", ConvertTypesToString(typeIDs));
        }
        if (moveData != null)
        {
            newString = newString.Replace("{{-move-name-}}", moveData.moveName);
        }
        if (moveIDs != null)
        {
            for (int i = 0; i < moveIDs.Length; i++)
            {
                MoveData moveXData = MoveDatabase.instance.GetMoveData(moveIDs[i]);
                string partToReplace = "{{-move-name-" + i + "-}}";
                newString = newString.Replace(partToReplace, moveXData.moveName);
            }
        }
        if (abilityData != null)
        {
            newString = newString.Replace("{{-ability-name-}}", abilityData.abilityName);
        }
        if (itemData != null)
        {
            newString = newString.Replace("{{-item-name-}}", itemData.itemName);
        }

        if (statusData != null)
        {
            newString = newString.Replace("{{-status-name-}}", statusData.conditionName);
        }

        if (teamStatusData != null)
        {
            newString = newString.Replace("{{-team-status-name-}}", teamStatusData.conditionName);
        }

        if (battleStatusData != null)
        {
            newString = newString.Replace("{{-battle-status-name-}}", battleStatusData.conditionName);
        }
        
        return newString;
    }

    public static string GetPokemonName(Pokemon pokemon)
    {
        return GetPokemonNames(new List<Pokemon> { pokemon });
    }
    public static string GetPokemonNames(List<Pokemon> pokemonList)
    {
        string names = "";
        if (pokemonList.Count == 1)
        {
            return pokemonList[0].nickname;
        }
        else if (pokemonList.Count == 2)
        {
            return pokemonList[0].nickname + " and " + pokemonList[1].nickname;
        }
        else
        {
            for (int i = 0; i < pokemonList.Count; i++)
            {
                names += (i == pokemonList.Count - 1) ?
                    "and " + pokemonList[i].nickname :
                    pokemonList[i].nickname + ", ";
            }
        }
        return names;
    }

    public static string ConvertTypesToString(string[] typeIDs)
    {
        string names = "";
        if (typeIDs.Length == 1)
        {
            return TypeDatabase.instance.GetTypeData(typeIDs[0]).typeName + "-type";
        }
        else if (typeIDs.Length == 2)
        {
            return TypeDatabase.instance.GetTypeData(typeIDs[0]).typeName 
                + "- and " 
                + TypeDatabase.instance.GetTypeData(typeIDs[1]).typeName + "-type";
        }
        else
        {
            for (int i = 0; i < typeIDs.Length; i++)
            {
                names += (i == typeIDs.Length - 1) ?
                    "and " + TypeDatabase.instance.GetTypeData(typeIDs[i]).typeName + "-type" :
                    TypeDatabase.instance.GetTypeData(typeIDs[i]).typeName + "-, ";
            }
        }
        return names;
    }

    public static PokemonGender ConvertToGender(string genderString)
    {
        return (genderString == "m") ? PokemonGender.Male
            : (genderString == "f") ? PokemonGender.Female
            : PokemonGender.Genderless;
    }
    public static string ConvertGenderToString(PokemonGender gender)
    {
        return (gender == PokemonGender.Male) ? "m"
            : (gender == PokemonGender.Female) ? "f"
            : (gender == PokemonGender.Genderless)? "g"
            : null;
    }

    public static string ConvertStatToString(PokemonStats stat, bool capitalize = true)
    {
        GameTextData textData = (stat == PokemonStats.Attack) ? instance.GetGameTextData("stat-attack")
            : (stat == PokemonStats.Defense) ? instance.GetGameTextData("stat-defense")
            : (stat == PokemonStats.SpecialAttack) ? instance.GetGameTextData("stat-special-attack")
            : (stat == PokemonStats.SpecialDefense) ? instance.GetGameTextData("stat-special-defense")
            : (stat == PokemonStats.Speed) ? instance.GetGameTextData("stat-speed")
            : (stat == PokemonStats.Accuracy) ? instance.GetGameTextData("stat-accuracy")
            : (stat == PokemonStats.Evasion) ? instance.GetGameTextData("stat-evasion")
            : instance.GetGameTextData("stat-hp");
        return textData.GetText();
    }
    private static string ConvertStatsToString(PokemonStats[] statList, bool capitalize = true)
    {
        if (statList.Length == 7)
        {
            string s = "Stats";
            s = (capitalize) ? s : s.ToLower();
            return s;
        }

        string text = "";
        if (statList.Length == 1)
        {
            return ConvertStatToString(statList[0], capitalize);
        }
        else if (statList.Length == 2)
        {
            return ConvertStatToString(statList[0], capitalize) 
                + " and " 
                + ConvertStatToString(statList[1], capitalize);
        }
        else
        {
            for (int i = 0; i < statList.Length; i++)
            {
                text += (i == statList.Length - 1) ? "and " + ConvertStatToString(statList[i], capitalize) 
                    : ConvertStatToString(statList[i], capitalize) + ", ";
            }
        }
        return text;
    }
    public static PokemonStats GetStatFromString(string statString)
    {
        statString = statString.ToLower();
        PokemonStats statType = (statString == "hp") ? PokemonStats.HitPoints
            : (statString == "atk") ? PokemonStats.Attack
            : (statString == "def") ? PokemonStats.Defense
            : (statString == "spa") ? PokemonStats.SpecialAttack
            : (statString == "spd") ? PokemonStats.SpecialDefense
            : (statString == "spe") ? PokemonStats.Speed
            : (statString == "acc") ? PokemonStats.Accuracy
            : (statString == "eva") ? PokemonStats.Evasion
            : PokemonStats.None;

        // random stat
        if (statString == "rnd")
        {
            List<PokemonStats> rndList = new List<PokemonStats>
            {
                PokemonStats.Attack,
                PokemonStats.Defense,
                PokemonStats.SpecialAttack,
                PokemonStats.SpecialDefense,
                PokemonStats.Speed,
                PokemonStats.Accuracy,
                PokemonStats.Evasion
            };
            return rndList[Random.Range(0, rndList.Count)];
        }

        return statType;
    }
    public static List<PokemonStats> GetStatsFromList(string[] statStrings)
    {
        // check if "all" stats
        if (statStrings.Length > 0)
        {
            List<string> statStringList = new List<string>(statStrings); 
            if (statStringList.Contains("ALL") || statStringList.Contains("all"))
            {
                return new List<PokemonStats>
                {
                    PokemonStats.Attack,
                    PokemonStats.Defense,
                    PokemonStats.SpecialAttack,
                    PokemonStats.SpecialDefense,
                    PokemonStats.Speed,
                    PokemonStats.Accuracy,
                    PokemonStats.Evasion
                };
            }
        }

        List<PokemonStats> statList = new List<PokemonStats>();
        for (int i = 0; i < statStrings.Length; i++)
        {
            PokemonStats stat = GetStatFromString(statStrings[i]);
            if (stat != PokemonStats.None)
            {
                statList.Add(stat);
            }
        }
        return statList;
    }

    public static MoveTargetType ConvertToMoveTargetType(string targetString)
    {
        return (targetString == "self") ? MoveTargetType.Self
            : (targetString == "any") ? MoveTargetType.Any
            : (targetString == "adjacent") ? MoveTargetType.Adjacent
            : (targetString == "adjacentopponent") ? MoveTargetType.AdjacentOpponent
            : (targetString == "adjacentally") ? MoveTargetType.AdjacentAlly
            : (targetString == "selforadjacentally") ? MoveTargetType.SelfOrAdjacentAlly
            : (targetString == "alladjacent") ? MoveTargetType.AllAdjacent
            : (targetString == "alladjacentopponents") ? MoveTargetType.AllAdjacentOpponents
            : (targetString == "teamally") ? MoveTargetType.TeamAlly
            : (targetString == "teamopponent") ? MoveTargetType.TeamOpponent
            : (targetString == "battlefield") ? MoveTargetType.Battlefield
            : MoveTargetType.Self;
    }

}
