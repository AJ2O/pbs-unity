using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDatabase
{
    //create an object of SingleObject
    private static EffectDatabase singleton = new EffectDatabase();

    //make the constructor private so that this class cannot be
    //instantiated
    private EffectDatabase() { }

    //Get the only object available
    public static EffectDatabase instance
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


    // ---GENERAL EFFECTS---
    /// <summary>
    /// Collection of all General Effects that aren't specific to one group.
    /// </summary>
    public class General
    {
        /// <summary>
        /// Damages
        /// </summary>
        public class Damage
        {
            public enum DamageMode
            {
                /// <summary>
                /// Makes <seealso cref="value"/> equal to base power.
                /// </summary>
                Calculated,
                /// <summary>
                /// Makes <seealso cref="value"/> equal to a direct Hit Point value.
                /// </summary>
                DirectHP,
                /// <summary>
                /// Makes <seealso cref="value"/> equal to a % of the Pokémon's max HP.
                /// </summary>
                MaxHPPercent,
                /// <summary>
                /// Makes <seealso cref="value"/> equal to a % of the Pokémon's remaining HP.
                /// </summary>
                InnardsOut
            }
            /// <summary>
            /// The method by which damage is dealt.
            /// </summary>
            public DamageMode mode;

            /// <summary>
            /// The damage dealt defined by the <seealso cref="DamageMode"/>.
            /// </summary>
            public float value;

            /// <summary>
            /// Only applicable when the mode is <seealso cref="DamageMode.Calculated"/>, factors in move category
            /// when calculating damage.
            /// </summary>
            public MoveCategory category;
            /// <summary>
            /// Only if applicable when the mode is <seealso cref="DamageMode.Calculated"/>, factors in this move type
            /// when calculating damage.
            /// </summary>
            public string moveType;

            /// <summary>
            /// The text displayed when the Pokémon takes damage.
            /// </summary>
            public string displayText;

            public Damage(
                DamageMode mode = DamageMode.Calculated,
                float value = 1f,
                string moveType = "",
                MoveCategory category = MoveCategory.Physical,
                string displayText = "pokemon-damage"
                )
            {
                this.mode = mode;
                this.value = value;
                this.moveType = moveType;
                this.category = category;
                this.displayText = displayText;
            }
            public Damage Clone()
            {
                return new Damage(
                    mode: mode,
                    value: value,
                    moveType: moveType, category: category,
                    displayText: displayText
                    );
            }
        }

        /// <summary>
        /// Defines the amount of turns left for certain battle mechanics.
        /// </summary>
        public class DefaultTurns
        {
            /// <summary>
            /// If true, the mechanic's turns are determined to be between <seealso cref="lowestTurns"/> (inclusive)
            /// and <seealso cref="highestTurns"/> (inclusive).
            /// </summary>
            public bool useTurnRange;

            /// <summary>
            /// The set amount of mechanic turns. If -1, the status will last indefinitely.
            /// </summary>
            public int turns;
            /// <summary>
            /// The lowest possible mechanic turns.
            /// </summary>
            public int lowestTurns;
            /// <summary>
            /// The highest possible mechanic turns.
            /// </summary>
            public int highestTurns;

            public DefaultTurns(
                bool useTurnRange = false,
                int turns = -1, int lowestTurns = 0, int highestTurns = 0
                )
            {
                this.useTurnRange = useTurnRange;
                this.turns = turns;
                this.lowestTurns = lowestTurns;
                this.highestTurns = highestTurns;
            }
            public DefaultTurns Clone()
            {
                return new DefaultTurns(useTurnRange: useTurnRange,
                    turns: turns, lowestTurns: lowestTurns, highestTurns: highestTurns);
            }
            public int GetTurns()
            {
                if (!useTurnRange)
                {
                    return turns;
                }
                else
                {
                    return Random.Range(lowestTurns, highestTurns);
                }
            }

        }

        /// <summary>
        /// Defines how a Pokémon changes form mid-battle.
        /// </summary>
        public class FormTransformation
        {
            public List<string> preForms;
            public string toForm;

            public FormTransformation(
                IEnumerable<string> preForms = null,
                string toForm = null
                )
            {
                this.preForms = (preForms == null) ? new List<string>() : new List<string>(preForms);
                this.toForm = toForm;
            }
            public FormTransformation(
                string preForm = null, string toForm = null
                )
            {
                this.preForms = (preForm == null) ? new List<string>() : new List<string> { preForm };
                this.toForm = toForm;
            }
            public FormTransformation Clone()
            {
                return new FormTransformation(
                    preForms: preForms,
                    toForm: toForm
                    );
            }
            public bool IsPokemonAPreForm(Pokemon pokemon, bool useBasePokemonID = false)
            {
                string pokemonID = useBasePokemonID ? pokemon.basePokemonID : pokemon.pokemonID;
                PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemonID);

                for (int i = 0; i < preForms.Count; i++)
                {
                    if (pokemonID == preForms[i]
                        || pokemonData.IsABaseID(preForms[i]))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool IsPokemonAToForm(Pokemon pokemon, bool useBasePokemonID = false)
            {
                string pokemonID = useBasePokemonID ? pokemon.basePokemonID : pokemon.pokemonID;
                PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemonID);

                if (pokemonID == toForm)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Defines how a Pokémon recovers HP.
        /// </summary>
        public class HealHP
        {
            public enum HealMode
            {
                HitPoints,
                MaxHPPercent
            }
            /// <summary>
            /// If set to <seealso cref="HealMode.HitPoints"/>, <seealso cref="healValue"/> recovers a set HP amount.
            /// If set to <seealso cref="HealMode.MaxHPPercent"/>, <seealso cref="healValue"/> recovers a percentage
            /// of the Pokémon's max HP.
            /// </summary>
            public HealMode healMode;
            /// <summary>
            /// Determines the amount to heal determined by <seealso cref="healMode"/>.
            /// </summary>
            public float healValue;
            /// <summary>
            /// The text displayed when the Pokémon heals.
            /// </summary>
            public string displayText;

            /// <summary>
            /// If true, this Pokémon is also revived if it is fainted.
            /// </summary>
            public bool revive;
            /// <summary>
            /// The text displayed if the Pokémon is revived.
            /// </summary>
            public string reviveText;

            public HealHP(
                HealMode healMode = HealMode.HitPoints,
                float healValue = 20, string displayText = "pokemon-heal",
                bool revive = false, string reviveText = "pokemon-revive"
                )
            {
                this.healMode = healMode;
                this.healValue = healValue;
                this.displayText = displayText;
                this.revive = revive;
                this.reviveText = reviveText;
            }
            public HealHP Clone()
            {
                return new HealHP(
                    healMode: healMode,
                    healValue: healValue, displayText: displayText,
                    revive: revive, reviveText: reviveText
                    );
            }
        }

        /// <summary>
        /// Induces a status condition on the specified type of target (Pokémon, Team, or Battle).
        /// </summary>
        public class InflictStatus
        {
            public enum EffectMode
            {
                Additive,
                Replace,
            }

            /// <summary>
            /// The specified status type.
            /// </summary>
            public StatusType statusType;

            /// <summary>
            /// The status that will be inflicted. 
            /// </summary>
            public string statusID;

            /// <summary>
            /// If filled out, replaces the name for this status.
            /// </summary>
            public string conditionName;

            /// <summary>
            /// If filled out, replaces the text for when this status starts.
            /// </summary>
            public string startTextID;
            /// <summary>
            /// If specified, replaces the text for when this status ends.
            /// </summary>
            public string endTextID;

            /// <summary>
            /// If true, the status ID that will be inflicted with its default turns left.
            /// </summary>
            public bool useDefaultTurns;

            /// <summary>
            /// If true, the status turns are determined to be between <seealso cref="lowestTurns"/> (inclusive)
            /// and <seealso cref="highestTurns"/> (inclusive).
            /// </summary>
            public bool useTurnRange;

            /// <summary>
            /// The set amount of status turns. If -1, the status will last indefinitely.
            /// </summary>
            public int turns;
            /// <summary>
            /// The lowest possible status turns.
            /// </summary>
            public int lowestTurns;
            /// <summary>
            /// The highest possible status turns.
            /// </summary>
            public int highestTurns;

            /// <summary>
            /// The method by which the custom effects will be added to the status.
            /// </summary>
            public EffectMode effectMode;
            /// <summary>
            /// The Pokémon-based custom effects that will add or replace the existing effects of the base status.
            /// </summary>
            public List<StatusPKEff.PokemonSE> customPokemonEffects;
            /// <summary>
            /// The team-based custom effects that will add or replace the existing effects of the base status.
            /// </summary>
            public List<StatusTEEff.TeamSE> customTeamEffects;
            /// <summary>
            /// The battle-based custom effects that will add or replace the existing effects of the base status.
            /// </summary>
            public List<StatusBTLEff.BattleSE> customBattleEffects;

            public InflictStatus(
                StatusType statusType,
                string statusID,
                string conditionName = null, string startTextID = null, string endTextID = null,

                bool useDefaultTurns = true, bool useTurnRange = false,
                int turns = -1, int lowestTurns = 0, int highestTurns = 0,
                EffectMode effectMode = EffectMode.Additive,

                StatusPKEff.PokemonSE[] customPokemonEffects = null,
                StatusTEEff.TeamSE[] customTeamEffects = null,
                StatusBTLEff.BattleSE[] customBattleEffects = null
                )
            {
                this.statusType = statusType;
                this.statusID = statusID;
                this.conditionName = conditionName;
                this.startTextID = startTextID;
                this.endTextID = endTextID;

                this.useDefaultTurns = useDefaultTurns;
                this.useTurnRange = useTurnRange;
                this.turns = turns;
                this.lowestTurns = lowestTurns;
                this.highestTurns = highestTurns;
                this.effectMode = effectMode;
                
                this.customPokemonEffects = new List<StatusPKEff.PokemonSE>();
                if (customPokemonEffects != null)
                {
                    for (int i = 0; i < customPokemonEffects.Length; i++)
                    {
                        this.customPokemonEffects.Add(customPokemonEffects[i]);
                    }
                }
                
                this.customTeamEffects = new List<StatusTEEff.TeamSE>();
                if (customTeamEffects != null)
                {
                    for (int i = 0; i < customTeamEffects.Length; i++)
                    {
                        this.customTeamEffects.Add(customTeamEffects[i]);
                    }
                }
                
                this.customBattleEffects = new List<StatusBTLEff.BattleSE>();
                if (customBattleEffects != null)
                {
                    for (int i = 0; i < customBattleEffects.Length; i++)
                    {
                        this.customBattleEffects.Add(customBattleEffects[i]);
                    }
                }
            }
            public InflictStatus Clone()
            {
                return new InflictStatus(
                    statusType: statusType,
                    statusID: statusID,
                    conditionName: conditionName, startTextID: startTextID, endTextID: endTextID,

                    useDefaultTurns: useDefaultTurns, useTurnRange: useTurnRange,
                    turns: turns, lowestTurns: lowestTurns, highestTurns: highestTurns,
                    effectMode: effectMode, 
                    
                    customPokemonEffects: customPokemonEffects.ToArray(),
                    customTeamEffects: customTeamEffects.ToArray(),
                    customBattleEffects: customBattleEffects.ToArray()
                    );
            }
        }

        /// <summary>
        /// Reflects certain moves back to the target.
        /// </summary>
        public class MagicCoat
        {
            /// <summary>
            /// Text displayed when the Pokémon successfully reflects a move.
            /// </summary>
            public string reflectText;

            /// <summary>
            /// Reflects moves that are specifically <seealso cref="MoveTag.MagicCoatSusceptible"/>.
            /// </summary>
            public bool magicCoat;

            /// <summary>
            /// If a move has tags contained here, it can be reflected.
            /// </summary>
            public HashSet<MoveTag> moveTags;

            public MagicCoat(
                bool magicCoat = true,
                IEnumerable<MoveTag> moveTags = null,
                string reflectText = "ability-magiccoat"
                )
            {
                this.magicCoat = magicCoat;
                this.moveTags = (moveTags == null) ? null : new HashSet<MoveTag>(moveTags);
                this.reflectText = reflectText;
            }
            public MagicCoat Clone()
            {
                return new MagicCoat(
                    magicCoat: magicCoat,
                    moveTags: moveTags,
                    reflectText: reflectText
                    );
            }
        }

        /// <summary>
        /// Protects the user from most moves. Less likely to succeed with consecutive uses.
        /// </summary>
        public class Protect
        {
            /// <summary>
            /// Inflicts status conditions against attackers who hit the protected Pokémon.
            /// </summary>
            public class BanefulBunker
            {
                /// <summary>
                /// The status condition inflicted upon the attacker.
                /// </summary>
                public InflictStatus inflictStatus;
                /// <summary>
                /// If true, this effect is only applied on contact moves.
                /// </summary>
                public bool onlyContact;

                public BanefulBunker(InflictStatus inflictStatus, bool onlyContact = true)
                {
                    this.inflictStatus = inflictStatus.Clone();
                    this.onlyContact = onlyContact;
                }
                public BanefulBunker Clone()
                {
                    return new BanefulBunker(inflictStatus, onlyContact);
                }
            }
            /// <summary>
            /// Applies stat stage changes to attackers who hit the protected Pokémon.
            /// </summary>
            public class KingsShield
            {
                /// <summary>
                /// The stat stage changes inflicted upon the attacker.
                /// </summary>
                public StatStageMod statStageMod;
                /// <summary>
                /// If true, this effect is only applied on contact moves.
                /// </summary>
                public bool onlyContact;

                public KingsShield(StatStageMod statStageMod, bool onlyContact = true)
                {
                    this.statStageMod = statStageMod.Clone();
                    this.onlyContact = onlyContact;
                }
                public KingsShield Clone()
                {
                    return new KingsShield(statStageMod, onlyContact);
                }
            }
            /// <summary>
            /// Inflicts damage against attackers who hit the protected Pokémon.
            /// </summary>
            public class SpikyShield
            {
                /// <summary>
                /// Defines how damage is dealt.
                /// </summary>
                public Damage damage;

                /// <summary>
                /// The percentage of the attacker's maximum HP that is lost upon protection.
                /// </summary>
                public float damagePercent;
                /// <summary>
                /// If true, this effect is only applied on contact moves.
                /// </summary>
                public bool onlyContact;
                /// <summary>
                /// The text displayed when the attacker loses HP.
                /// </summary>
                public string displayText;

                public SpikyShield(
                    Damage damage,
                    float damagePercent = 1f / 8,
                    bool onlyContact = true,
                    string displayText = "move-spikyshield")
                {
                    this.damage = damage.Clone();
                    this.damagePercent = damagePercent;
                    this.onlyContact = onlyContact;
                    this.displayText = displayText;
                }
                public SpikyShield Clone()
                {
                    return new SpikyShield(
                        damage: damage,
                        damagePercent: damagePercent,
                        onlyContact: onlyContact,
                        displayText: displayText);
                }
            }

            /// <summary>
            /// Text displayed when the Pokémon starts protection.
            /// </summary>
            public string displayText;
            /// <summary>
            /// Text displayed when the Pokémon successfully protects itself from a move.
            /// </summary>
            public string protectText;

            /// <summary>
            /// If true, only protects against damaging moves.
            /// </summary>
            public bool damagingOnly;
            /// <summary>
            /// If true, this limits protection against opposing Pokémon's moves.
            /// </summary>
            public bool opposingOnly;
            /// <summary>
            /// If true, limits protection to status moves only.
            /// </summary>
            public bool craftyShield;
            /// <summary>
            /// If true, limits protection to heightened priority moves only.
            /// </summary>
            public bool quickGuard;
            /// <summary>
            /// If true, limits protection to multi-targeting moves only.
            /// </summary>
            public bool wideGuard;
            /// <summary>
            /// If true, adds full protection against Max Moves & Z-Moves.
            /// </summary>
            public bool maxGuard;
            /// <summary>
            /// If true, this move can be used consecutively without a chance of failing.
            /// </summary>
            public bool consecutiveUse;

            /// <summary>
            /// <seealso cref="BanefulBunker"/>.
            /// </summary>
            public BanefulBunker banefulBunker;
            /// <summary>
            /// <seealso cref="KingsShield"/>.
            /// </summary>
            public KingsShield kingsShield;
            /// <summary>
            /// <seealso cref="SpikyShield"/>.
            /// </summary>
            public SpikyShield spikyShield;

            public Protect(
                string displayText = "move-protect", string protectText = "move-protect-success",
                bool damagingOnly = false, bool opposingOnly = false,
                bool craftyShield = false, bool quickGuard = false, bool wideGuard = false, bool maxGuard = false,
                bool consecutiveUse = false,

                BanefulBunker banefulBunker = null,
                KingsShield kingsShield = null,
                SpikyShield spikyShield = null
                )
            {
                this.displayText = displayText;
                this.protectText = protectText;

                this.damagingOnly = damagingOnly;
                this.opposingOnly = opposingOnly;
                this.craftyShield = craftyShield;
                this.quickGuard = quickGuard;
                this.wideGuard = wideGuard;
                this.maxGuard = maxGuard;
                this.consecutiveUse = consecutiveUse;

                this.banefulBunker = (banefulBunker == null) ? null : banefulBunker.Clone();
                this.kingsShield = (kingsShield == null) ? null : kingsShield.Clone();
                this.spikyShield = (spikyShield == null) ? null : spikyShield.Clone();
            }
            public Protect Clone()
            {
                return new Protect(
                    displayText: displayText, protectText: protectText,
                    damagingOnly: damagingOnly, opposingOnly: opposingOnly,
                    craftyShield: craftyShield, quickGuard: quickGuard, wideGuard: wideGuard, maxGuard: maxGuard,
                    consecutiveUse: consecutiveUse,

                    banefulBunker: banefulBunker, kingsShield: kingsShield, spikyShield: spikyShield
                    );
            }
        }

        /// <summary>
        /// Scales Pokemon stats.
        /// </summary>
        public class StatScale
        {
            public Dictionary<PokemonStats, float> scaleMap;

            public StatScale(
                Dictionary<PokemonStats, float> scaleMap
                )
            {
                this.scaleMap = scaleMap;
            }
            public StatScale Clone()
            {
                return new StatScale(scaleMap: scaleMap);
            }
            public float GetPokemonStatScale(PokemonStats statType)
            {
                if (scaleMap.ContainsKey(statType))
                {
                    return scaleMap[statType];
                }
                return 1;
            }
        }

        /// <summary>
        /// Adds or subtracts the stat stages of a given Pokémon.
        /// </summary>
        public class StatStageMod
        {
            public int ATKMod, DEFMod, SPAMod, SPDMod, SPEMod, ACCMod, EVAMod;
            public bool maxATK, maxDEF, maxSPA, maxSPD, maxSPE, maxACC, maxEVA;
            public bool minATK, minDEF, minSPA, minSPD, minSPE, minACC, minEVA;

            public StatStageMod(
                int ATKMod = 0, int DEFMod = 0, int SPAMod = 0, int SPDMod = 0, int SPEMod = 0, 
                int ACCMod = 0, int EVAMod = 0,
                bool maxATK = false, bool maxDEF = false, bool maxSPA = false, bool maxSPD = false, bool maxSPE = false,
                bool maxACC = false, bool maxEVA = false,
                bool minATK = false, bool minDEF = false, bool minSPA = false, bool minSPD = false, bool minSPE = false,
                bool minACC = false, bool minEVA = false
                )
            {
                this.ATKMod = ATKMod;
                this.DEFMod = DEFMod;
                this.SPAMod = SPAMod;
                this.SPDMod = SPDMod;
                this.SPEMod = SPEMod;
                this.ACCMod = ACCMod;
                this.EVAMod = EVAMod;

                this.maxATK = maxATK;
                this.maxDEF = maxDEF;
                this.maxSPA = maxSPA;
                this.maxSPD = maxSPD;
                this.maxSPE = maxSPE;
                this.maxACC = maxACC;
                this.maxEVA = maxEVA;

                this.minATK = minATK;
                this.minDEF = minDEF;
                this.minSPA = minSPA;
                this.minSPD = minSPD;
                this.minSPE = minSPE;
                this.minACC = minACC;
                this.minEVA = minEVA;
            }
            public StatStageMod Clone()
            {
                return new StatStageMod(
                    ATKMod: ATKMod, DEFMod: DEFMod, SPAMod: SPAMod, SPDMod: SPDMod, SPEMod: SPEMod,
                    ACCMod: ACCMod, EVAMod: EVAMod,
                    maxATK: maxATK, maxDEF: maxDEF, maxSPA: maxSPA, maxSPD: maxSPD, maxSPE: maxSPE,
                    maxACC: maxACC, maxEVA: maxEVA,
                    minATK: minATK, minDEF: minDEF, minSPA: minSPA, minSPD: minSPD, minSPE: minSPE,
                    minACC: minACC, minEVA: minEVA
                    );
            }

            public bool IsNoChange()
            {
                return
                    ATKMod == 0 && DEFMod == 0 && SPAMod == 0 && SPDMod == 0 && SPEMod == 0
                    && ACCMod == 0 && EVAMod == 0
                    && maxATK == false && maxDEF == false && maxSPA == false && maxSPD == false
                    && maxACC == false && maxEVA == false
                    && minATK == false && minDEF == false && minSPA == false && minSPD == false
                    && minACC == false && minEVA == false;
            }
            public bool RaisesAStat()
            {
                return maxATK || maxDEF || maxSPA || maxSPD || maxSPE || maxACC || maxEVA
                    || ATKMod > 0 || DEFMod > 0 || SPAMod > 0 || SPDMod > 0 || SPEMod > 0 
                    || ACCMod > 0 || EVAMod > 0;
            }
            public bool LowersAStat()
            {
                return minATK || minDEF || minSPA || minSPD || minSPE || minACC || minEVA
                    || ATKMod < 0 || DEFMod < 0 || SPAMod < 0 || SPDMod < 0 || SPEMod < 0
                    || ACCMod < 0 || EVAMod < 0;
            }
            public void AddOther(StatStageMod other)
            {
                ATKMod += other.ATKMod;
                DEFMod += other.DEFMod;
                SPAMod += other.SPAMod;
                SPDMod += other.SPDMod;
                SPEMod += other.SPEMod;

                maxATK = other.maxATK ? true : maxATK;
                maxDEF = other.maxDEF ? true : maxDEF;
                maxSPA = other.maxSPA ? true : maxSPA;
                maxSPD = other.maxSPD ? true : maxSPD;
                maxSPE = other.maxSPE ? true : maxSPE;
                maxACC = other.maxACC ? true : maxACC;
                maxEVA = other.maxEVA ? true : maxEVA;

                minATK = other.minATK ? true : minATK;
                minDEF = other.minDEF ? true : minDEF;
                minSPA = other.minSPA ? true : minSPA;
                minSPD = other.minSPD ? true : minSPD;
                minSPE = other.minSPE ? true : minSPE;
                minACC = other.minACC ? true : minACC;
                minEVA = other.minEVA ? true : minEVA;
            }
            public void AddStatMod(PokemonStats statType, bool maxVal = false,  bool minVal = false, int statVal = 0)
            {
                switch (statType)
                {
                    case PokemonStats.Attack:
                        maxATK = maxATK ? maxATK : maxVal;
                        minATK = minATK ? minATK : minVal;
                        ATKMod += statVal;
                        break;
                    case PokemonStats.Defense:
                        maxDEF = maxDEF ? maxDEF : maxVal;
                        minDEF = minDEF ? minDEF : minVal;
                        DEFMod += statVal;
                        break;
                    case PokemonStats.SpecialAttack:
                        maxSPA = maxSPA ? maxSPA : maxVal;
                        minSPA = minSPA ? minSPA : minVal;
                        SPAMod += statVal;
                        break;
                    case PokemonStats.SpecialDefense:
                        maxSPD = maxSPD ? maxSPD : maxVal;
                        minSPD = minSPD ? minSPD : minVal;
                        SPDMod += statVal;
                        break;
                    case PokemonStats.Speed:
                        maxSPE = maxSPE ? maxSPE : maxVal;
                        minSPE = minSPE ? minSPE : minVal;
                        SPEMod += statVal;
                        break;
                    case PokemonStats.Accuracy:
                        maxACC = maxACC ? maxACC : maxVal;
                        minACC = minACC ? minACC : minVal;
                        ACCMod += statVal;
                        break;
                    case PokemonStats.Evasion:
                        maxEVA = maxEVA ? maxEVA : maxVal;
                        minEVA = minEVA ? minEVA : minVal;
                        EVAMod += statVal;
                        break;
                }
            }
            public void ScaleStatMod(PokemonStats statType, float scaleVal)
            {
                switch (statType)
                {
                    case PokemonStats.Attack:
                        if (!maxATK && !minATK)
                        {
                            ATKMod *= Mathf.FloorToInt(scaleVal);
                        }
                        break;
                    case PokemonStats.Defense:
                        if (!maxDEF && !minDEF)
                        {
                            DEFMod *= Mathf.FloorToInt(scaleVal);
                        }
                        break;
                    case PokemonStats.SpecialAttack:
                        if (!maxSPA && !minSPA)
                        {
                            SPAMod *= Mathf.FloorToInt(scaleVal);
                        }
                        break;
                    case PokemonStats.SpecialDefense:
                        if (!maxSPD && !minSPD)
                        {
                            SPDMod *= Mathf.FloorToInt(scaleVal);
                        }
                        break;
                    case PokemonStats.Speed:
                        if (!maxSPE && !minSPE)
                        {
                            SPEMod *= Mathf.FloorToInt(scaleVal);
                        }
                        break;
                    case PokemonStats.Accuracy:
                        if (!maxACC && !minACC)
                        {
                            ACCMod *= Mathf.FloorToInt(scaleVal);
                        }
                        break;
                    case PokemonStats.Evasion:
                        if (!maxEVA && !minEVA)
                        {
                            EVAMod *= Mathf.FloorToInt(scaleVal);
                        }
                        break;
                }
            }
            public List<PokemonStats> GetMaximizedStats()
            {
                List<PokemonStats> maximizedStats = new List<PokemonStats>();
                if (maxATK) maximizedStats.Add(PokemonStats.Attack);
                if (maxDEF) maximizedStats.Add(PokemonStats.Defense);
                if (maxSPA) maximizedStats.Add(PokemonStats.SpecialAttack);
                if (maxSPD) maximizedStats.Add(PokemonStats.SpecialDefense);
                if (maxSPE) maximizedStats.Add(PokemonStats.Speed);
                if (maxACC) maximizedStats.Add(PokemonStats.Accuracy);
                if (maxEVA) maximizedStats.Add(PokemonStats.Evasion);
                return maximizedStats;
            }
            public List<PokemonStats> GetMinimizedStats()
            {
                List<PokemonStats> minimizedStats = new List<PokemonStats>();
                if (minATK) minimizedStats.Add(PokemonStats.Attack);
                if (minDEF) minimizedStats.Add(PokemonStats.Defense);
                if (minSPA) minimizedStats.Add(PokemonStats.SpecialAttack);
                if (minSPD) minimizedStats.Add(PokemonStats.SpecialDefense);
                if (minSPE) minimizedStats.Add(PokemonStats.Speed);
                if (minACC) minimizedStats.Add(PokemonStats.Accuracy);
                if (minEVA) minimizedStats.Add(PokemonStats.Evasion);
                return minimizedStats;
            }
            public Dictionary<int, List<PokemonStats>> GetLegacyStatStageMap()
            {
                Dictionary<int, List<PokemonStats>> statStageMap = new Dictionary<int, List<PokemonStats>>();
                if (ATKMod != 0)
                {
                    if (statStageMap.ContainsKey(ATKMod)) statStageMap[ATKMod].Add(PokemonStats.Attack);
                    else statStageMap[ATKMod] = new List<PokemonStats> { PokemonStats.Attack };
                }
                if (DEFMod != 0)
                {
                    if (statStageMap.ContainsKey(DEFMod)) statStageMap[DEFMod].Add(PokemonStats.Defense);
                    else statStageMap[DEFMod] = new List<PokemonStats> { PokemonStats.Defense };
                }
                if (SPAMod != 0)
                {
                    if (statStageMap.ContainsKey(SPAMod)) statStageMap[SPAMod].Add(PokemonStats.Defense);
                    else statStageMap[SPAMod] = new List<PokemonStats> { PokemonStats.SpecialAttack };
                }
                if (SPDMod != 0)
                {
                    if (statStageMap.ContainsKey(SPDMod)) statStageMap[SPDMod].Add(PokemonStats.SpecialDefense);
                    else statStageMap[SPDMod] = new List<PokemonStats> { PokemonStats.SpecialDefense };
                }
                if (SPEMod != 0)
                {
                    if (statStageMap.ContainsKey(SPEMod)) statStageMap[SPEMod].Add(PokemonStats.Speed);
                    else statStageMap[SPEMod] = new List<PokemonStats> { PokemonStats.Speed };
                }
                if (ACCMod != 0)
                {
                    if (statStageMap.ContainsKey(ACCMod)) statStageMap[ACCMod].Add(PokemonStats.Accuracy);
                    else statStageMap[ACCMod] = new List<PokemonStats> { PokemonStats.Accuracy };
                }
                if (EVAMod != 0)
                {
                    if (statStageMap.ContainsKey(EVAMod)) statStageMap[EVAMod].Add(PokemonStats.Evasion);
                    else statStageMap[EVAMod] = new List<PokemonStats> { PokemonStats.Evasion };
                }

                return statStageMap;
            }
            public Dictionary<PokemonStats, int> GetStatStageMap()
            {
                Dictionary<PokemonStats, int> statMap = new Dictionary<PokemonStats, int>();

                statMap[PokemonStats.Attack] = maxATK ? GameSettings.btlMaxStatBoost
                    : minATK ? GameSettings.btlMinStatBoost
                    : ATKMod;

                statMap[PokemonStats.Defense] = maxDEF ? GameSettings.btlMaxStatBoost
                    : minDEF ? GameSettings.btlMinStatBoost
                    : DEFMod;

                statMap[PokemonStats.SpecialAttack] = maxSPA ? GameSettings.btlMaxStatBoost
                    : minSPA ? GameSettings.btlMinStatBoost
                    : SPAMod;

                statMap[PokemonStats.SpecialDefense] = maxSPD ? GameSettings.btlMaxStatBoost
                    : minSPD ? GameSettings.btlMinStatBoost
                    : SPDMod;

                statMap[PokemonStats.Speed] = maxSPE ? GameSettings.btlMaxStatBoost
                    : minSPE ? GameSettings.btlMinStatBoost
                    : SPEMod;

                statMap[PokemonStats.Accuracy] = maxACC ? GameSettings.btlMaxStatBoost
                    : minACC ? GameSettings.btlMinStatBoost
                    : ACCMod;

                statMap[PokemonStats.Evasion] = maxEVA ? GameSettings.btlMaxStatBoost
                    : minEVA ? GameSettings.btlMinStatBoost
                    : EVAMod;

                return statMap;
            }
        }
    }


    // ---FILTER EFFECTS---
    /// <summary>
    /// Collection of all Filter Effects that can be used to restrict other effects.
    /// </summary>
    public class Filter
    {
        public class FilterEffect
        {
            /// <summary>
            /// The type of effect that this is.
            /// </summary>
            public FilterEffectType effectType;
            /// <summary>
            /// Inverts the checks of this filter if true.
            /// </summary>
            public bool invert;

            public FilterEffect(
                FilterEffectType effectType,
                bool invert = false
                )
            {
                this.effectType = effectType;
                this.invert = invert;
            }
            public FilterEffect Clone()
            {
                return 
                    (this is BurningJealousy) ? (this as BurningJealousy).Clone()
                    : (this is Harvest) ? (this as Harvest).Clone()
                    : (this is MoveCheck) ? (this as MoveCheck).Clone()
                    : (this is PollenPuff) ? (this as PollenPuff).Clone()
                    : (this is TypeList) ? (this as TypeList).Clone()
                    : new FilterEffect(
                        effectType: effectType, invert: invert
                        );
            }
        }
        
        /// <summary>
        /// Causes the effect to fail if the target if the target didn't have their stats raised during the turn.
        /// </summary>
        public class BurningJealousy : FilterEffect
        {
            public enum TargetType
            {
                Self,
                AllyTeam,
                Target,
                TargetTeam,
            }
            /// <summary>
            /// The target being considered for stat changes.
            /// </summary>
            public TargetType targetType;

            public enum StatChangeType
            {
                Unchecked,
                NoChange,
                Raise,
                Lower,
            }
            /// <summary>
            /// The effect is enabled if this stat was modified in its specified way. 
            /// </summary>
            public StatChangeType ATKMod, DEFMod, SPAMod, SPDMod, SPEMod, ACCMod, EVAMod;

            public BurningJealousy(
                TargetType targetType = TargetType.Target,
                StatChangeType ATKMod = StatChangeType.Raise, StatChangeType DEFMod = StatChangeType.Raise,
                StatChangeType SPAMod = StatChangeType.Raise, StatChangeType SPDMod = StatChangeType.Raise,
                StatChangeType SPEMod = StatChangeType.Raise,
                StatChangeType ACCMod = StatChangeType.Raise, StatChangeType EVAMod = StatChangeType.Raise,

                bool invert = false
                )
                : base(effectType: FilterEffectType.BurningJealousy, invert: invert)
            {
                this.targetType = targetType;
                this.ATKMod = ATKMod;
                this.DEFMod = DEFMod;
                this.SPAMod = SPAMod;
                this.SPDMod = SPDMod;
                this.SPEMod = SPEMod;
                this.ACCMod = ACCMod;
                this.EVAMod = EVAMod;
            }
            public new BurningJealousy Clone()
            {
                return new BurningJealousy(
                    targetType: targetType,
                    ATKMod: ATKMod, DEFMod: DEFMod, SPAMod: SPAMod, SPDMod: SPDMod, SPEMod: SPEMod,
                    ACCMod: ACCMod, EVAMod: EVAMod,

                    invert: invert
                    );
            }
            public bool DoesPokemonPassStatCheck(Pokemon pokemon)
            {
                bool success = false;

                // ATK
                if (ATKMod == StatChangeType.Raise && pokemon.bProps.ATKRaised) return true;
                if (ATKMod == StatChangeType.Lower && pokemon.bProps.ATKLowered) return true;
                if (ATKMod == StatChangeType.NoChange && (!pokemon.bProps.ATKRaised && !pokemon.bProps.ATKLowered)) return true;

                // DEF
                if (DEFMod == StatChangeType.Raise && pokemon.bProps.DEFRaised) return true;
                if (DEFMod == StatChangeType.Lower && pokemon.bProps.DEFLowered) return true;
                if (DEFMod == StatChangeType.NoChange && (!pokemon.bProps.DEFRaised && !pokemon.bProps.DEFLowered)) return true;

                // SPA
                if (SPAMod == StatChangeType.Raise && pokemon.bProps.SPARaised) return true;
                if (SPAMod == StatChangeType.Lower && pokemon.bProps.SPALowered) return true;
                if (SPAMod == StatChangeType.NoChange && (!pokemon.bProps.SPARaised && !pokemon.bProps.SPALowered)) return true;

                // SPD
                if (SPDMod == StatChangeType.Raise && pokemon.bProps.SPDRaised) return true;
                if (SPDMod == StatChangeType.Lower && pokemon.bProps.SPDLowered) return true;
                if (SPDMod == StatChangeType.NoChange && (!pokemon.bProps.SPDRaised && !pokemon.bProps.SPDLowered)) return true;

                // SPE
                if (SPEMod == StatChangeType.Raise && pokemon.bProps.SPERaised) return true;
                if (SPEMod == StatChangeType.Lower && pokemon.bProps.SPELowered) return true;
                if (SPEMod == StatChangeType.NoChange && (!pokemon.bProps.SPERaised && !pokemon.bProps.SPELowered)) return true;

                // ACC
                if (ACCMod == StatChangeType.Raise && pokemon.bProps.ACCRaised) return true;
                if (ACCMod == StatChangeType.Lower && pokemon.bProps.ACCLowered) return true;
                if (ACCMod == StatChangeType.NoChange && (!pokemon.bProps.ACCRaised && !pokemon.bProps.ACCLowered)) return true;

                // EVA
                if (EVAMod == StatChangeType.Raise && pokemon.bProps.EVARaised) return true;
                if (EVAMod == StatChangeType.Lower && pokemon.bProps.EVALowered) return true;
                if (EVAMod == StatChangeType.NoChange && (!pokemon.bProps.EVARaised && !pokemon.bProps.EVALowered)) return true;

                return success;
            }
        }

        /// <summary>
        /// Causes the effect to fail if the given status conditions are not present.
        /// </summary>
        public class Harvest : FilterEffect
        {
            public enum ConditionType
            {
                Pokemon,
                Team,
                Battle
            }
            public ConditionType conditionType;
            public List<string> conditions;

            /// <summary>
            /// If non-empty, this effect fails if the status doesn't have the specified effect type.
            /// </summary>
            public HashSet<PokemonSEType> statusPKTypes;
            /// <summary>
            /// If non-empty, this effect fails if the status doesn't have the specified effect type.
            /// </summary>
            public HashSet<TeamSEType> statusTETypes;
            /// <summary>
            /// If non-empty, this effect fails if the status doesn't have the specified effect type.
            /// </summary>
            public HashSet<BattleSEType> statusBTLTypes;

            public Harvest(
                ConditionType conditionType = ConditionType.Pokemon,
                IEnumerable<string> conditions = null,
                IEnumerable<PokemonSEType> statusPKTypes = null,
                IEnumerable<TeamSEType> statusTETypes = null,
                IEnumerable<BattleSEType> statusBTLTypes = null,
                bool invert = false
                )
                : base(effectType: FilterEffectType.Harvest, invert: invert)
            {
                this.conditionType = conditionType;
                this.conditions = (conditions == null) ? new List<string>()
                    : new List<string>(conditions);
                this.statusPKTypes = (statusPKTypes == null) ? new HashSet<PokemonSEType>()
                    : new HashSet<PokemonSEType>(statusPKTypes);
                this.statusTETypes = (statusTETypes == null) ? new HashSet<TeamSEType>()
                    : new HashSet<TeamSEType>(statusTETypes);
                this.statusBTLTypes = (statusBTLTypes == null) ? new HashSet<BattleSEType>()
                    : new HashSet<BattleSEType>(statusBTLTypes);
            }
            public new Harvest Clone()
            {
                return new Harvest(
                    conditionType: conditionType,
                    conditions: conditions,
                    statusPKTypes: statusPKTypes, statusTETypes: statusTETypes, statusBTLTypes: statusBTLTypes,
                    invert: invert
                    );
            }

            public bool DoesStatusSatisfy(StatusPKData statusData)
            {
                if (conditionType == ConditionType.Pokemon)
                {
                    if (conditions.Count > 0)
                    {
                        for (int i = 0; i < conditions.Count; i++)
                        {
                            if (statusData.ID == conditions[i]
                                || statusData.IsABaseID(conditions[i]))
                            {
                                return true;
                            }
                        }
                        return false;
                    }

                    if (statusPKTypes.Count > 0)
                    {
                        List<PokemonSEType> listTypes = new List<PokemonSEType>(statusPKTypes);
                        for (int i = 0; i < listTypes.Count; i++)
                        {
                            if (statusData.GetEffectNew(listTypes[i]) != null)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
                return false;
            }
            public bool DoesStatusSatisfy(StatusTEData statusData)
            {
                if (conditionType == ConditionType.Team)
                {
                    if (conditions.Count > 0)
                    {
                        for (int i = 0; i < conditions.Count; i++)
                        {
                            if (statusData.ID == conditions[i]
                                || statusData.IsABaseID(conditions[i]))
                            {
                                return true;
                            }
                        }
                        return false;
                    }

                    if (statusTETypes.Count > 0)
                    {
                        List<TeamSEType> listTypes = new List<TeamSEType>(statusTETypes);
                        for (int i = 0; i < listTypes.Count; i++)
                        {
                            if (statusData.GetEffectNew(listTypes[i]) != null)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
                return false;
            }
            public bool DoesStatusSatisfy(StatusBTLData statusData)
            {
                if (conditionType == ConditionType.Battle)
                {
                    if (conditions.Count > 0)
                    {
                        for (int i = 0; i < conditions.Count; i++)
                        {
                            if (statusData.ID == conditions[i]
                                || statusData.IsABaseID(conditions[i]))
                            {
                                return true;
                            }
                        }
                        return false;
                    }

                    if (statusBTLTypes.Count > 0)
                    {
                        List<BattleSEType> listTypes = new List<BattleSEType>(statusBTLTypes);
                        for (int i = 0; i < listTypes.Count; i++)
                        {
                            if (statusData.GetEffectNew(listTypes[i]) != null)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Causes the effect to fail if the item parameter doesn't satisfy the given conditions.
        /// </summary>
        public class ItemCheck : FilterEffect
        {
            /// <summary>
            /// If non-empty, this effect will fail if the item is not one of the listed item IDs.
            /// </summary>
            public List<string> specificItemIDs;
            /// <summary>
            /// If non-empty, this effect will fail if the item is classified as one of the listed pockets.
            /// </summary>
            public HashSet<ItemPocket> pockets;
            /// <summary>
            /// If non-empty, this effect will fail if the item doesn't have any effects listed here.
            /// </summary>
            public HashSet<ItemEffectType> effects;
            /// <summary>
            /// If non-empty, this effect will fail if the item doesn't have any tags listed here.
            /// </summary>
            public HashSet<ItemTag> tags;

            public ItemCheck(
                IEnumerable<string> specificItemIDs = null,
                IEnumerable<ItemPocket> pockets = null,
                IEnumerable<ItemEffectType> effects = null,
                IEnumerable<ItemTag> tags = null,
                bool damagingOnly = false
                )
                : base(effectType: FilterEffectType.ItemCheck)
            {
                this.specificItemIDs = (specificItemIDs == null) ? new List<string>()
                    : new List<string>(specificItemIDs);
                this.pockets = (pockets == null) ? new HashSet<ItemPocket>()
                    : new HashSet<ItemPocket>(pockets);
                this.effects = (effects == null) ? new HashSet<ItemEffectType>()
                    : new HashSet<ItemEffectType>(effects);
                this.tags = (tags == null) ? new HashSet<ItemTag>() : new HashSet<ItemTag>(tags);
            }
            public new ItemCheck Clone()
            {
                return new ItemCheck(
                    specificItemIDs: specificItemIDs,
                    pockets: pockets,
                    effects: effects,
                    tags: tags
                    );
            }
            public bool DoesItemPassFilter(Item item)
            {
                bool success = true;

                // Specific Items
                if (specificItemIDs.Count > 0)
                {
                    success = false;
                    for (int i = 0; i < specificItemIDs.Count && !success; i++)
                    {
                        if (item.data.ID == specificItemIDs[i])
                        {
                            success = true;
                        }
                    }
                }

                // Pockets

                // Effects

                // Tags

                return success;
            }
        }

        /// <summary>
        /// Causes the effect to fail if the move parameter doesn't satisfy the given conditions.
        /// </summary>
        public class MoveCheck : FilterEffect
        {
            /// <summary>
            /// If non-empty, this effect will fail if the move is not one of the listed move IDs.
            /// </summary>
            public List<string> specificMoveIDs;
            /// <summary>
            /// If non-empty, this effect will fail if the move is not one of the listed categories.
            /// </summary>
            public HashSet<MoveCategory> moveCategories;
            /// <summary>
            /// If non-empty, this effect will fail if the move doesn't have any effects listed here.
            /// </summary>
            public HashSet<MoveEffectType> moveEffects;
            /// <summary>
            /// If non-empty, this effect will fail if the move doesn't have any tags listed here.
            /// </summary>
            public HashSet<MoveTag> moveTags;

            /// <summary>
            /// If true, the effect will fail if the move is not a damaging move.
            /// </summary>
            public bool damagingOnly;
            /// <summary>
            /// If true, the effect will fail if the move is not a healing move.
            /// </summary>
            public bool healingOnly;
            /// <summary>
            /// If true, the effect will fail if the move would not trigger Sheer Force.
            /// </summary>
            public bool sheerForceOnly;

            public MoveCheck(
                IEnumerable<string> specificMoveIDs = null,
                IEnumerable<MoveCategory> moveCategories = null,
                IEnumerable<MoveEffectType> moveEffects = null,
                IEnumerable<MoveTag> moveTags = null,
                bool damagingOnly = false, bool healingOnly = false,
                bool sheerForceOnly = false
                )
                : base(effectType: FilterEffectType.MoveCheck)
            {
                this.specificMoveIDs = (specificMoveIDs == null) ? new List<string>() 
                    : new List<string>(specificMoveIDs);
                this.moveCategories = (moveCategories == null) ? new HashSet<MoveCategory>()
                    : new HashSet<MoveCategory>(moveCategories);
                this.moveEffects = (moveEffects == null) ? new HashSet<MoveEffectType>() 
                    : new HashSet<MoveEffectType>(moveEffects);
                this.moveTags = (moveTags == null) ? new HashSet<MoveTag>() : new HashSet<MoveTag>(moveTags);
                this.damagingOnly = damagingOnly;
                this.healingOnly = healingOnly;
                this.sheerForceOnly = sheerForceOnly;
            }
            public new MoveCheck Clone()
            {
                return new MoveCheck(
                    specificMoveIDs: specificMoveIDs,
                    moveCategories: moveCategories,
                    moveEffects: moveEffects,
                    moveTags: moveTags,
                    damagingOnly: damagingOnly, healingOnly: healingOnly,
                    sheerForceOnly: sheerForceOnly
                    );
            }
        }

        /// <summary>
        /// Causes the effect to fail if the target isn't a specified type of target.
        /// </summary>
        public class PollenPuff : FilterEffect
        {
            public enum TargetType
            {
                Self,
                Ally,
                Enemy,
            }
            public HashSet<TargetType> targetTypes;

            public PollenPuff(IEnumerable<TargetType> targetTypes, bool invert = false) 
                : base(effectType: FilterEffectType.PollenPuff, invert: invert)
            {
                this.targetTypes = (targetTypes == null) ? new HashSet<TargetType> { TargetType.Ally }
                    : new HashSet<TargetType>(targetTypes);
            }
            public new PollenPuff Clone()
            {
                return new PollenPuff(targetTypes: targetTypes, invert: invert);
            }
        }

        /// <summary>
        /// Causes the effect to fail if the target isn't a specified type.
        /// </summary>
        public class TypeList : FilterEffect
        {
            public enum TargetType
            {
                Pokemon,
                Move
            }
            /// <summary>
            /// The mechanic that we are checking for type.
            /// </summary>
            public TargetType targetType;

            /// <summary>
            /// The types specified.
            /// </summary>
            public List<string> types;
            /// <summary>
            /// If true, the effect fails if the target doesn't contain all the specified types.
            /// </summary>
            public bool exact;

            public TypeList(
                TargetType targetType = TargetType.Pokemon,
                IEnumerable<string> types = null, bool exact = false, 
                
                bool invert = false)
                : base(effectType: FilterEffectType.TypeList, invert: invert)
            {
                this.targetType = targetType;
                this.types = (types == null) ? new List<string>() : new List<string>(types);
                this.exact = exact;
                this.invert = invert;
            }
            public new TypeList Clone()
            {
                return new TypeList(
                    targetType: targetType, types: types, exact: exact,
                    invert: invert);
            }
        }
    }

    
    // ---TYPE EFFECTS---
    public class TypeEff
    {
        public class TypeEffect
        {
            /// <summary>
            /// The type of effect that this is.
            /// </summary>
            public TypeEffectType effectType;

            /// <summary>
            /// Additional restrictions on how the effect is applied.
            /// </summary>
            public List<Filter.FilterEffect> filters;

            public TypeEffect(
                TypeEffectType effectType = TypeEffectType.None,
                IEnumerable<Filter.FilterEffect> filters = null
                )
            {
                this.effectType = effectType;

                this.filters = new List<Filter.FilterEffect>();
                if (filters != null)
                {
                    List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                    for (int i = 0; i < tempFilters.Count; i++)
                    {
                        this.filters.Add(tempFilters[i].Clone());
                    }
                }
            }
            public TypeEffect Clone()
            {
                return
                    new TypeEffect(
                        effectType: effectType,
                        filters: filters
                        );
            }
        }

        
    }


    // ---MOVE EFFECTS---
    /// <summary>
    /// Collection of all Move Effects.
    /// </summary>
    public class MoveEff
    {
        public class MoveEffect
        {
            /// <summary>
            /// The type of effect that this is.
            /// </summary>
            public MoveEffectType effectType;
            /// <summary>
            /// The timing that triggers this effect.
            /// </summary>
            public MoveEffectTiming timing;
            /// <summary>
            /// Who or what the effect is applied to.
            /// </summary>
            public MoveEffectTargetType targetType;
            /// <summary>
            /// The amount of times the effect is executed.
            /// </summary>
            public MoveEffectOccurrence occurrence;

            /// <summary>
            /// Additional restrictions on how the effect is applied.
            /// </summary>
            public List<Filter.FilterEffect> filters;

            /// <summary>
            /// The chance of the effect working.
            /// </summary>
            public float chance;
            /// <summary>
            /// Does not check the effect's chance against each target, but uses one check.
            /// </summary>
            public bool oneTimeChance;
            /// <summary>
            /// If applicable, always show when this effect fails or succeeds.
            /// </summary>
            public bool forceEffectDisplay;

            public MoveEffect(
                MoveEffectType effectType = MoveEffectType.None,
                MoveEffectTiming timing = MoveEffectTiming.Unique,
                MoveEffectTargetType targetType = MoveEffectTargetType.Unique,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.None,
                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false)
            {
                this.effectType = effectType;
                this.timing = timing;
                this.targetType = targetType;
                this.occurrence = occurrence;
                this.filters = new List<Filter.FilterEffect>();
                if (filters != null)
                {
                    List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                    for (int i = 0; i < tempFilters.Count; i++)
                    {
                        this.filters.Add(tempFilters[i].Clone());
                    }
                }

                this.chance = chance;
                this.oneTimeChance = oneTimeChance;
                this.forceEffectDisplay = forceEffectDisplay;
            }
            public MoveEffect Clone()
            {
                return
                    (this is Absorb) ? (this as Absorb).Clone()
                    : (this is AuraWheel) ? (this as AuraWheel).Clone()
                    : (this is BasePowerMultiplier) ?
                        (
                        (this is LashOut) ? (this as LashOut).Clone()
                        : (this as BasePowerMultiplier).Clone()
                        )

                    : (this is BeatUp) ? (this as BeatUp).Clone()
                    : (this is BurningJealousy) ? (this as BurningJealousy).Clone()
                    : (this is CoreEnforcer) ? (this as CoreEnforcer).Clone()
                    : (this is CorrosiveGas) ? (this as CorrosiveGas).Clone()
                    : (this is Covet) ? (this as Covet).Clone()
                    : (this is DamageMultiplier) ? (this as DamageMultiplier).Clone()
                    : (this is DoubleEdge) ? (this as DoubleEdge).Clone()
                    : (this is DoubleKick) ? (this as DoubleKick).Clone()
                    : (this is DragonRage) ? (this as DragonRage).Clone()
                    : (this is Endure) ? (this as Endure).Clone()
                    : (this is Eruption) ? (this as Eruption).Clone()
                    : (this is ExpandingForce) ? (this as ExpandingForce).Clone()
                    : (this is ExpandingForcePower) ? (this as ExpandingForcePower).Clone()
                    : (this is FailNotPokemon) ? (this as FailNotPokemon).Clone()
                    : (this is FakeOut) ? (this as FakeOut).Clone()
                    : (this is Feint) ? (this as Feint).Clone()
                    : (this is FuryAttack) ? (this as FuryAttack).Clone()
                    : (this is GrassyGlide) ? (this as GrassyGlide).Clone()
                    : (this is Guillotine) ? (this as Guillotine).Clone()
                    : (this is GuillotineAccuracy) ? (this as GuillotineAccuracy).Clone()
                    : (this is HealBeforeUse) ? (this as HealBeforeUse).Clone()
                    : (this is HeavySlam) ? (this as HeavySlam).Clone()
                    : (this is HiddenPower) ? (this as HiddenPower).Clone()
                    : (this is InflictStatus) ? (this as InflictStatus).Clone()
                    : (this is Judgment) ? (this as Judgment).Clone()
                    : (this is KarateChop) ? (this as KarateChop).Clone()
                    : (this is KnockOff) ? (this as KnockOff).Clone()
                    : (this is LowKick) ? (this as LowKick).Clone()
                    : (this is MagicCoat) ? (this as MagicCoat).Clone()
                    : (this is Magnitude) ? (this as Magnitude).Clone()
                    : (this is NaturalGift) ? (this as NaturalGift).Clone()
                    : (this is PhotonGeyser) ? (this as PhotonGeyser).Clone()
                    : (this is Poltergeist) ? (this as Poltergeist).Clone()
                    : (this is Protect) ? (this as Protect).Clone()
                    : (this is Psywave) ? (this as Psywave).Clone()
                    : (this is Punishment) ? (this as Punishment).Clone()
                    : (this is Pursuit) ? (this as Pursuit).Clone()
                    : (this is Refresh) ? (this as Refresh).Clone()
                    : (this is RelicSong) ? (this as RelicSong).Clone()
                    : (this is Reversal) ? (this as Reversal).Clone()
                    : (this is RisingVoltage) ? (this as RisingVoltage).Clone()
                    : (this is Rollout) ? (this as Rollout).Clone()
                    : (this is SecretPower) ? (this as SecretPower).Clone()
                    : (this is SeismicToss) ? (this as SeismicToss).Clone()
                    : (this is ShellSideArm) ? (this as ShellSideArm).Clone()
                    : (this is Snore) ? (this as Snore).Clone()
                    : (this is StatStageMod) ? (this as StatStageMod).Clone()
                    : (this is SteelRoller) ? (this as SteelRoller).Clone()
                    : (this is StoredPower) ? (this as StoredPower).Clone()
                    : (this is SuckerPunch) ? (this as SuckerPunch).Clone()
                    : (this is SunsteelStrike) ? (this as SunsteelStrike).Clone()
                    : (this is SuperFang) ? (this as SuperFang).Clone()
                    : (this is Synchronoise) ? (this as Synchronoise).Clone()
                    : (this is TerrainPulse) ? (this as TerrainPulse).Clone()
                    : (this is TripleKick) ? (this as TripleKick).Clone()
                    : (this is WeatherBall) ? (this as WeatherBall).Clone()
                    : (this is Whirlwind) ? (this as Whirlwind).Clone()
                    : (this is WorrySeed) ? (this as WorrySeed).Clone()
                    : new MoveEffect(
                        effectType: effectType, timing: timing, targetType: targetType, occurrence: occurrence,
                        filters: filters,
                        chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                        );
            }
        }

        /// <summary>
        /// Recovers a % of damage dealt as HP.
        /// </summary>
        public class Absorb : MoveEffect
        {
            /// <summary>
            /// The % of damage dealt that is absorbed.
            /// </summary>
            public float healPercent;

            /// <summary>
            /// The text displayed when the user recovers HP.
            /// </summary>
            public string displayText;

            public Absorb(
                float healPercent = 0.5f,
                string displayText = "move-absorb"
                )
                : base(effectType: MoveEffectType.HPDrain)
            {
                this.healPercent = healPercent;
                this.displayText = displayText;
            }
            public new Absorb Clone()
            {
                return new Absorb(
                    healPercent: healPercent,
                    displayText: displayText
                    );
            }
        }

        /// <summary>
        /// Changes the move's type depending on the user's form.
        /// </summary>
        public class AuraWheel : MoveEffect
        {
            /// <summary>
            /// The type that this move is changed to.
            /// </summary>
            public string type;
            /// <summary>
            /// If the user's Pokémon ID is in this list, the move's type is changed to <seealso cref="type"/>.
            /// </summary>
            public List<string> pokemonIDs;

            public AuraWheel(
                string type,
                IEnumerable<string> pokemonIDs
                ) : base(effectType: MoveEffectType.AuraWheel)
            {
                this.type = type;
                this.pokemonIDs = (pokemonIDs == null) ? new List<string>() : new List<string>(pokemonIDs);
            }
            public new AuraWheel Clone()
            {
                return new AuraWheel(type: type, pokemonIDs: pokemonIDs);
            }
        }

        /// <summary>
        /// Scales base power if the accompanying filters are satisfied.
        /// </summary>
        public class BasePowerMultiplier : MoveEffect
        {
            /// <summary>
            /// The amount by which base power is scaled.
            /// </summary>
            public float powerScale;

            public BasePowerMultiplier(
                float powerScale = 1f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: MoveEffectType.BasePowerMultiplier, filters: filters)
            {
                this.powerScale = powerScale;
            }
            public new BasePowerMultiplier Clone()
            {
                return new BasePowerMultiplier(powerScale: powerScale, filters: filters);
            }
        }

        /// <summary>
        /// Hits for as many able party members there are.
        /// </summary>
        public class BeatUp : MoveEffect
        {
            public BeatUp() : base(effectType: MoveEffectType.BeatUp)
            {

            }
            public new BeatUp Clone()
            {
                return new BeatUp();
            }
        }

        /// <summary>
        /// Inflicts a status condition on the target if they had their stats raised during the turn.
        /// </summary>
        public class BurningJealousy : InflictStatus
        {
            public BurningJealousy(
                General.InflictStatus inflictStatus,
                MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
                MoveEffectTargetType targetType = MoveEffectTargetType.Target,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,

                float chance = -1
                )
                : base(inflictStatus: inflictStatus,
                      timing: timing, targetType: targetType, occurrence: occurrence, 
                      chance: chance,
                      filters: new Filter.FilterEffect[] 
                      { 
                          new Filter.BurningJealousy()
                      })
            { }
        }

        /// <summary>
        /// Suppresses the target's ability as long as it remains in battle (if possible).
        /// </summary>
        public class CoreEnforcer : MoveEffect
        {
            /// <summary>
            /// The text to be displayed when the ability is suppressed.
            /// </summary>
            public string displayText;
            /// <summary>
            /// The text to be displayed when the ability cannot be suppressed.
            /// </summary>
            public string failText;

            public CoreEnforcer(
                string displayText = "move-coreenforcer",
                string failText = "move-coreenforcer-fail"
                )
                : base(effectType: MoveEffectType.CoreEnforcer)
            {
                this.displayText = displayText;
                this.failText = failText;
            }
            public new CoreEnforcer Clone()
            {
                return new CoreEnforcer(
                    displayText: displayText,
                    failText: failText
                    );
            }
        }

        /// <summary>
        /// Renders the target's held item unusable for the remainder of the battle.
        /// </summary>
        public class CorrosiveGas : MoveEffect
        {
            /// <summary>
            /// The text displayed when an item is made useless.
            /// </summary>
            public string displayText;

            public CorrosiveGas(
                string displayText = "move-corrosivegas",
                float powerScale = 2f
                )
                : base(effectType: MoveEffectType.CorrosiveGas, occurrence: MoveEffectOccurrence.OnceForEachTarget)
            {
                this.displayText = displayText;
            }
            public new CorrosiveGas Clone()
            {
                return new CorrosiveGas(displayText: displayText);
            }
        }

        /// <summary>
        /// May steal the target's held item.
        /// </summary>
        public class Covet : MoveEffect
        {
            /// <summary>
            /// The text displayed when an item is stolen.
            /// </summary>
            public string displayText;

            public Covet(
                string displayText = "move-covet",
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: MoveEffectType.Covet, occurrence: MoveEffectOccurrence.OnceForEachTarget, filters: filters)
            {
                this.displayText = displayText;
            }
            public new Covet Clone()
            {
                return new Covet(
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Scales damage if the accompanying filters are satisfied.
        /// </summary>
        public class DamageMultiplier : MoveEffect
        {
            /// <summary>
            /// The amount by which damage is scaled.
            /// </summary>
            public float damageScale;

            public DamageMultiplier(
                float damageScale = 1f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: MoveEffectType.DamageMultiplier, filters: filters)
            {
                this.damageScale = damageScale;
            }
            public new DamageMultiplier Clone()
            {
                return new DamageMultiplier(damageScale: damageScale, filters: filters);
            }
        }

        /// <summary>
        /// Deals proportional damage to the user after the move's use.
        /// </summary>
        public class DoubleEdge : MoveEffect
        {
            public enum RecoilMode
            {
                /// <summary>
                /// Damage dealt to the user is proportional to the total damage dealt/
                /// </summary>
                Damage,
                /// <summary>
                /// Damage dealt to the user is proportional to the user's max HP.
                /// </summary>
                MaxHP
            }
            /// <summary>
            /// The type of recoil the user takes.
            /// </summary>
            public RecoilMode recoilMode;
            /// <summary>
            /// Damage dealt to the user determined by <seealso cref="recoilMode"/>.
            /// </summary>
            public float hpLossPercent;
            /// <summary>
            /// If false, the user will not take recoil if they have the <seealso cref="AbilityEff.RockHead"/>
            /// ability effect.
            /// </summary>
            public bool bypassRockHead;

            /// <summary>
            /// The text displayed when the user takes recoil damage.
            /// </summary>
            public string displayText;

            public DoubleEdge(
                RecoilMode recoilMode = RecoilMode.Damage,
                float hpLossPercent = 0.25f, bool bypassRockHead = false,
                string displayText = "move-doubleedge"
                )
                : base(effectType: MoveEffectType.Recoil)
            {
                this.recoilMode = recoilMode;
                this.hpLossPercent = hpLossPercent;
                this.bypassRockHead = bypassRockHead;
                this.displayText = displayText;
            }
            public new DoubleEdge Clone()
            {
                return new DoubleEdge(
                    recoilMode: recoilMode,
                    hpLossPercent: hpLossPercent, bypassRockHead: bypassRockHead,
                    displayText: displayText
                    );
            }
        }

        /// <summary>
        /// Hits multiple times.
        /// </summary>
        public class DoubleKick : MoveEffect
        {
            /// <summary>
            /// The amount of hits for this attack.
            /// </summary>
            public int hits;

            public DoubleKick(int hits = 2)
                : base(effectType: MoveEffectType.DoubleKick)
            {
                this.hits = hits;;
            }
            public new TripleKick Clone()
            {
                return new TripleKick(hits: hits);
            }

        }

        /// <summary>
        /// Makes this move deal a set amount of damage.
        /// </summary>
        public class DragonRage : MoveEffect
        {
            /// <summary>
            /// The exact damage dealt.
            /// </summary>
            public int damage;

            public DragonRage(int damage = 40) : base(effectType: MoveEffectType.DragonRage)
            {
                this.damage = damage;
            }
            public new DragonRage Clone()
            {
                return new DragonRage(damage: damage);
            }
        }

        /// <summary>
        /// Allows the user to survive direct attacks with 1 HP for the rest of the turn. 
        /// Less likely to succeed with consecutive uses.
        /// </summary>
        public class Endure : MoveEffect
        {
            /// <summary>
            /// Text displayed when the Pokémon starts enduring.
            /// </summary>
            public string displayText;
            /// <summary>
            /// Text displayed when the Pokémon successfully survives due to Endure.
            /// </summary>
            public string protectText;

            /// <summary>
            /// If true, this move can be used consecutively without a chance of failing.
            /// </summary>
            public bool consecutiveUse;

            public Endure(
                string displayText = "move-endure", string protectText = "move-endure-success",
                bool consecutiveUse = false,

                MoveEffectTargetType targetType = MoveEffectTargetType.Self,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.Once,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: MoveEffectType.Endure,
                      timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                      targetType: targetType, occurrence: occurrence, filters: filters)
            {
                this.displayText = displayText;
                this.protectText = protectText;
                this.consecutiveUse = consecutiveUse;
            }
            public new Endure Clone()
            {
                return new Endure(
                    displayText: displayText, protectText: protectText,
                    consecutiveUse: consecutiveUse,

                    targetType: targetType, occurrence: occurrence, filters: filters
                    );
            }
        }

        /// <summary>
        /// Scales base power according to the user's HP %.
        /// </summary>
        public class Eruption : MoveEffect
        {
            public Eruption() : base(effectType: MoveEffectType.Eruption) { }
            public new Eruption Clone()
            {
                return new Eruption();
            }
        }

        /// <summary>
        /// Changes the move target type if the given battle conditions are active.
        /// </summary>
        public class ExpandingForce : MoveEffect
        {
            /// <summary>
            /// The new target type if the conditions exist.
            /// </summary>
            public MoveTargetType newTargetType;
            /// <summary>
            /// The relevant battle conditions.
            /// </summary>
            public List<string> conditions;

            public ExpandingForce(
                MoveTargetType newTargetType,
                IEnumerable<string> conditions = null
                )
                : base(effectType: MoveEffectType.ExpandingForce)
            {
                this.newTargetType = newTargetType;
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
            }
            public new ExpandingForce Clone()
            {
                return new ExpandingForce(newTargetType: newTargetType, conditions: conditions);
            }
        }

        /// <summary>
        /// Increases power if the user is affected by the given battle conditions.
        /// </summary>
        public class ExpandingForcePower : MoveEffect
        {
            /// <summary>
            /// The amount that damage is scaled by.
            /// </summary>
            public float damageScale;
            /// <summary>
            /// The relevant battle conditions.
            /// </summary>
            public List<string> conditions;

            public ExpandingForcePower(
                float damageScale = 1f,
                IEnumerable<string> conditions = null
                )
                : base(effectType: MoveEffectType.ExpandingForcePower)
            {
                this.damageScale = damageScale;
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
            }
            public new ExpandingForcePower Clone()
            {
                return new ExpandingForcePower(damageScale: damageScale, conditions: conditions);
            }
        }

        /// <summary>
        /// The move immediately fails to be used if the user isn't a specified Pokémon.
        /// </summary>
        public class FailNotPokemon : MoveEffect
        {
            /// <summary>
            /// The text that displays if the user can't use the move.
            /// </summary>
            public string failText;
            /// <summary>
            /// The text that displays if the user can't use the move because it wasn't a specific listed form.
            /// </summary>
            public string failFormText;

            /// <summary>
            /// Invert filters to invert the allowable Pokémon, including additional filters set.
            /// </summary>
            public bool invert;
            /// <summary>
            /// Allow Pokémon forms derived from those listed to use the move.
            /// </summary>
            public bool allowDerivatives;
            /// <summary>
            /// Allow Pokémon transformed into one of those listed to be able to use the move.
            /// </summary>
            public bool allowTransform;

            /// <summary>
            /// The list of Pokémon that are allowed to use the move.
            /// </summary>
            public List<string> pokemonIDs;

            public FailNotPokemon(
                IEnumerable<string> pokemonIDs,
                string failText = "move-FAIL-pokemon", string failFormText = "move-FAIL-form",
                bool invert = false, bool allowDerivatives = false, bool allowTransform = true
                ) : base(effectType: MoveEffectType.FailNotPokemon)
            {
                this.failText = failText;
                this.failFormText = failFormText;
                this.invert = invert;
                this.allowDerivatives = allowDerivatives;
                this.allowTransform = allowTransform;
                this.pokemonIDs = (pokemonIDs == null) ? new List<string>() : new List<string>(pokemonIDs);
            }
            public new FailNotPokemon Clone()
            {
                return new FailNotPokemon(
                    pokemonIDs: pokemonIDs,
                    failText: failText, failFormText: failFormText,
                    invert: invert, allowDerivatives: allowDerivatives, allowTransform: allowTransform
                    );
            }
        }

        /// <summary>
        /// This move automatically fails if used after the first turn that the user is in battle.
        /// </summary>
        public class FakeOut : MoveEffect
        {
            /// <summary>
            /// After this turn, this move will fail if used.
            /// </summary>
            public int maxTurn;

            public FakeOut(int maxTurn = 1) : base(effectType: MoveEffectType.FakeOut)
            {
                this.maxTurn = maxTurn;
            }
            public new FakeOut Clone()
            {
                return new FakeOut(maxTurn: maxTurn);
            }
        }

        /// <summary>
        /// Lifts the effects of protection moves from the target and/or target team.
        /// </summary>
        public class Feint : MoveEffect
        {
            /// <summary>
            /// The text displayed when protection effects are lifted.
            /// </summary>
            public string displayText;
            /// <summary>
            /// The text displayed when mat block effects are lifted.
            /// </summary>
            public string displayTextMatBlock;
            /// <summary>
            /// If false, this move cannot lift the effects of Max Guard (though it may still hit
            /// through Max Guard.
            /// </summary>
            public bool liftMaxGuard;

            public Feint(
                string displayText = "move-feint", string displayTextMatBlock = "move-feint-matblock",
                bool liftMaxGuard = false
                )
                : base(effectType: MoveEffectType.Feint)
            {
                this.displayText = displayText;
                this.displayTextMatBlock = displayTextMatBlock;
                this.liftMaxGuard = liftMaxGuard;
            }
            public new Feint Clone()
            {
                return new Feint(
                    displayText: displayText, displayTextMatBlock: displayTextMatBlock,
                    liftMaxGuard: liftMaxGuard
                    );
            }
        }

        /// <summary>
        /// Hits between a range of hits by chance.
        /// </summary>
        public class FuryAttack : MoveEffect
        {
            /// <summary>
            /// The lowest amount of hits possible for this move;
            /// </summary>
            public int lowestHits;
            /// <summary>
            /// The length of this + <seealso cref="lowestHits"/> determines the highest possible hits. Each index i
            /// represents the probability of getting hits: i + <seealso cref="lowestHits"/>. These probabilities
            /// are automatically normalized.
            /// </summary>
            public List<float> hitChances;

            public FuryAttack(
                int lowestHits = 2,
                IEnumerable<float> hitChances = null
                )
                : base(effectType: MoveEffectType.FuryAttack)
            {
                this.lowestHits = lowestHits;
                this.hitChances = (hitChances != null) ? new List<float>(hitChances)
                    : new List<float> { 1f / 3, 1f / 3, 1f / 6, 1f / 6 };
            }
            public new FuryAttack Clone()
            {
                return new FuryAttack(lowestHits: lowestHits, hitChances: hitChances);
            }

        }

        /// <summary>
        /// Modifies this move's priority during certain battle conditions. 
        /// </summary>
        public class GrassyGlide : MoveEffect
        {
            public enum PriorityMode
            {
                Add,
                Set,
            }
            /// <summary>
            /// The mode by which priority is determined.
            /// </summary>
            public PriorityMode mode;
            /// <summary>
            /// Is added onto existing priority with <seealso cref="PriorityMode.Add"/>, and is hard set
            /// with <seealso cref="PriorityMode.Set"/>.
            /// </summary>
            public int priority;

            /// <summary>
            /// The relevant battle conditions.
            /// </summary>
            public List<string> conditions;

            public GrassyGlide(
                PriorityMode mode = PriorityMode.Add, int priority = 0,
                IEnumerable<string> conditions = null
                )
                : base(effectType: MoveEffectType.GrassyGlide)
            {
                this.mode = mode;
                this.priority = priority;
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
            }
            public new GrassyGlide Clone()
            {
                return new GrassyGlide(mode: mode, priority: priority, conditions: conditions);
            }
        }

        /// <summary>
        /// This move is a one-hit KO (OHKO), instantly causing the target to faint.
        /// </summary>
        public class Guillotine : MoveEffect
        {
            /// <summary>
            /// The text displayed when this move connects.
            /// </summary>
            public string displayText;
            /// <summary>
            /// If true, this move does not work against higher-leveled targets.
            /// </summary>
            public bool upperBoundLevel;

            public Guillotine(
                string displayText = "move-guillotine",
                bool upperBoundLevel = true
                )
                : base(effectType: MoveEffectType.Guillotine)
            {
                this.displayText = displayText;
                this.upperBoundLevel = upperBoundLevel;
            }
            public new Guillotine Clone()
            {
                return new Guillotine(displayText: displayText, upperBoundLevel: upperBoundLevel);
            }
        }
        /// <summary>
        /// Overrides accuracy calculation as: (user.level - target.level) + baseAccuracy.
        /// </summary>
        public class GuillotineAccuracy : MoveEffect
        {
            public GuillotineAccuracy() : base(effectType: MoveEffectType.GuillotineAccuracy) { }
            public new GuillotineAccuracy Clone() { return new GuillotineAccuracy(); }
        }

        /// <summary>
        /// Immediately heals the user's specified status conditions right before the move is used.
        /// </summary>
        public class HealBeforeUse : MoveEffect
        {
            /// <summary>
            /// The statuses listed here are healed immediately before use.
            /// </summary>
            public List<string> statuses;

            public HealBeforeUse(
                IEnumerable<string> statuses = null
                )
                : base(effectType: MoveEffectType.HealBeforeUse)
            {
                this.statuses = (statuses == null) ? new List<string>() : new List<string>(statuses);
            }
            public new HealBeforeUse Clone()
            {
                return new HealBeforeUse(statuses: statuses);
            }
        }

        /// <summary>
        /// Overwrites base power depending on the user's weight relative to the target's weight.
        /// </summary>
        public class HeavySlam : MoveEffect
        {
            /// <summary>
            /// The lowest base power that this move can be - i.e. weight strictly lower than the first threshold
            /// in <seealso cref="relativeWeightThresholds"/>.
            /// </summary>
            public int lowestPower;
            /// <summary>
            /// The highest base power that this move can be - i.e. weight at least as high as the last threshold
            /// in <seealso cref="relativeWeightThresholds"/>.
            /// </summary>
            public int highestPower;

            /// <summary>
            /// The range of power between each threshold in <seealso cref="relativeWeightThresholds"/>.
            /// This is one item less as long as <seealso cref="relativeWeightThresholds"/>.
            /// </summary>
            public List<int> powerRange;
            /// <summary>
            /// The relative weight thresholds determining this move's base power.
            /// </summary>
            public List<float> relativeWeightThresholds;

            public HeavySlam(
                int lowestPower = 40, int highestPower = 120,
                IEnumerable<int> powerRange = null,
                IEnumerable<float> relativeWeightThresholds = null
                )
                : base(effectType: MoveEffectType.LowKick)
            {
                this.lowestPower = lowestPower;
                this.highestPower = highestPower;
                this.powerRange = (powerRange == null) ? new List<int>()
                    : new List<int> { 60, 80, 100 };
                this.powerRange.Sort();
                this.relativeWeightThresholds = (relativeWeightThresholds == null) ? new List<float>()
                    : new List<float> { 0.5f, 1f/3, .25f, .2f };
                this.relativeWeightThresholds.Sort();
                this.relativeWeightThresholds.Reverse();
            }
            public new HeavySlam Clone()
            {
                return new HeavySlam(
                    lowestPower: lowestPower, highestPower: highestPower,
                    powerRange: powerRange, relativeWeightThresholds: relativeWeightThresholds
                    );
            }
        }

        /// <summary>
        /// Changes the move's type and base power according to the user's IVs. Both these features
        /// and types available are fully customizable.
        /// </summary>
        public class HiddenPower : MoveEffect
        {
            /// <summary>
            /// Set to true if this move will change the base power of the move. This is done using
            /// the user's IVs.
            /// </summary>
            public bool calculateDamage;
            /// <summary>
            /// Set to true if this move will change the type of the move. This is done using
            /// the user's IVs.
            /// </summary>
            public bool calculateType;

            /// <summary>
            /// If <seealso cref="calculateDamage"/> is true, this is the lowest base power possible.
            /// </summary>
            public int lowestBasePower;
            /// <summary>
            /// If <seealso cref="calculateDamage"/> is true, this is the highest base power possible.
            /// </summary>
            public int highestBasePower;

            /// <summary>
            /// If <seealso cref="calculateType"/> is true, these are the possible types that the move can be.
            /// The order of the types listed matters.
            /// </summary>
            public List<string> types;

            public HiddenPower(
                bool calculateDamage = false, bool calculateType = true,
                int lowestBasePower = 30, int highestBasePower = 70,
                IEnumerable<string> types = null
                )
                : base(effectType: MoveEffectType.HiddenPower)
            {
                this.calculateDamage = calculateDamage;
                this.calculateType = calculateType;
                this.lowestBasePower = lowestBasePower;
                this.highestBasePower = highestBasePower;
                this.types = (types != null) ? new List<string>(types)
                    : new List<string>
                    {
                        // Default Hidden Power
                        "fighting", "flying", "poison", "ground", "rock", "bug", "ghost", "steel",
                        "fire", "water", "grass", "electric", "psychic", "ice", "dragon", "dark",
                    };
            }
            public new HiddenPower Clone()
            {
                return new HiddenPower(
                    calculateDamage: calculateDamage, calculateType: calculateType,
                    lowestBasePower: lowestBasePower, highestBasePower: highestBasePower,
                    types: types
                    );
            }
        }

        /// <summary>
        /// Induces a status condition on the specified type of target (Pokémon, Team, or Battle).
        /// </summary>
        public class InflictStatus : MoveEffect
        {
            public General.InflictStatus inflictStatus;

            public InflictStatus(
                General.InflictStatus inflictStatus,

                MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
                MoveEffectTargetType targetType = MoveEffectTargetType.Target,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,
                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false
                )
                : base(
                      effectType: MoveEffectType.InflictStatus, timing: timing, targetType: targetType,
                      occurrence: occurrence, filters: filters,
                      chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay)
            {
                this.inflictStatus = inflictStatus.Clone();
            }
            public new InflictStatus Clone()
            {
                return new InflictStatus(
                    inflictStatus: inflictStatus,
                    timing: timing, targetType: targetType,  filters: filters, occurrence: occurrence,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                    );
            }
        }

        /// <summary>
        /// Changes the move's type if the user's held item has the effect <seealso cref="ItemEff.Judgment"/>.
        /// </summary>
        public class Judgment : MoveEffect
        {
            public Judgment() : base(effectType: MoveEffectType.Judgment) { }
            public new Judgment Clone()
            {
                return new Judgment();
            }
        }

        /// <summary>
        /// Increases the base critical hit rate for this move.
        /// </summary>
        public class KarateChop : MoveEffect
        {
            /// <summary>
            /// If true, this move always results in critical hits.
            /// </summary>
            public bool alwaysCritical;
            /// <summary>
            /// The critical level added on to this move.
            /// </summary>
            public int criticalBoost;

            public KarateChop(
                bool alwaysCritical = false, int criticalBoost = 1
                )
                : base(effectType: MoveEffectType.KarateChop)
            {
                this.alwaysCritical = alwaysCritical;
                this.criticalBoost = criticalBoost;
            }
            public new KarateChop Clone()
            {
                return new KarateChop(alwaysCritical: alwaysCritical, criticalBoost: criticalBoost);
            }
        }

        /// <summary>
        /// Removes the target's held item (if possible). If the item is able to be removed, this attack gets
        /// a boost in power.
        /// </summary>
        public class KnockOff : MoveEffect
        {
            /// <summary>
            /// The text displayed when an item is knocked off.
            /// </summary>
            public string displayText;
            /// <summary>
            /// The amount by which the base power is scaled if the item can be knocked off.
            /// </summary>
            public float damageScale;
        
            public KnockOff(
                string displayText = "move-knockoff",
                float damageScale = 1.5f
                )
                : base(effectType: MoveEffectType.KnockOff)
            {
                this.displayText = displayText;
                this.damageScale = damageScale;
            }
            public new KnockOff Clone()
            {
                return new KnockOff(displayText: displayText, damageScale: damageScale);
            }
        }

        /// <summary>
        /// Scales base power if the user's stats were lowered during the turn.
        /// </summary>
        public class LashOut : BasePowerMultiplier
        {
            public LashOut(float powerScale = 2f)
                : base(powerScale: powerScale,
                      filters: new Filter.FilterEffect[]
                      {
                          new Filter.BurningJealousy(
                              targetType: Filter.BurningJealousy.TargetType.Self,
                              ATKMod: Filter.BurningJealousy.StatChangeType.Lower, DEFMod: Filter.BurningJealousy.StatChangeType.Lower,
                              SPAMod: Filter.BurningJealousy.StatChangeType.Lower, SPDMod: Filter.BurningJealousy.StatChangeType.Lower,
                              SPEMod: Filter.BurningJealousy.StatChangeType.Lower,
                              ACCMod: Filter.BurningJealousy.StatChangeType.Lower, EVAMod: Filter.BurningJealousy.StatChangeType.Lower
                              ),
                      }
                      )
            { }
        }

        /// <summary>
        /// Overwrites base power depending on the target's weight
        /// </summary>
        public class LowKick : MoveEffect
        {
            /// <summary>
            /// The lowest base power that this move can be - i.e. weight strictly lower than the first threshold
            /// in <seealso cref="weightThresholds"/>.
            /// </summary>
            public int lowestPower;
            /// <summary>
            /// The highest base power that this move can be - i.e. weight at least as high as the last threshold
            /// in <seealso cref="weightThresholds"/>.
            /// </summary>
            public int highestPower;

            /// <summary>
            /// The range of power between each threshold in <seealso cref="weightThresholds"/>.
            /// This is one item less as long as <seealso cref="weightThresholds"/>.
            /// </summary>
            public List<int> powerRange;
            /// <summary>
            /// The weight thresholds determining this move's base power.
            /// </summary>
            public List<float> weightThresholds;

            public LowKick(
                int lowestPower = 20, int highestPower = 120,
                IEnumerable<int> powerRange = null,
                IEnumerable<float> weightThresholds = null
                )
                : base(effectType: MoveEffectType.LowKick)
            {
                this.lowestPower = lowestPower;
                this.highestPower = highestPower;
                this.powerRange = (powerRange == null) ? new List<int>()
                    : new List<int> { 40, 60, 80, 100 };
                this.powerRange.Sort();
                this.weightThresholds = (weightThresholds == null) ? new List<float>()
                    : new List<float> { 10f, 25f, 50f, 100f, 200f };
                this.weightThresholds.Sort();
            }
            public new LowKick Clone()
            {
                return new LowKick(
                    lowestPower: lowestPower, highestPower: highestPower,
                    powerRange: powerRange, weightThresholds: weightThresholds
                    );
            }
        }

        /// <summary>
        /// Reflects certain moves back to their attackers.
        /// </summary>
        public class MagicCoat : MoveEffect
        {
            /// <summary>
            /// Defines how moves are reflected.
            /// </summary>
            public General.MagicCoat magicCoat;

            public MagicCoat(
                General.MagicCoat magicCoat,

                MoveEffectTargetType targetType = MoveEffectTargetType.Self,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.Once,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: MoveEffectType.MagicCoat,
                      timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                      targetType: targetType, occurrence: occurrence,
                      filters: filters)
            {
                this.magicCoat = magicCoat.Clone();
            }
            public new MagicCoat Clone()
            {
                return new MagicCoat(
                    magicCoat: magicCoat,

                    targetType: targetType, occurrence: occurrence,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Randomly determines the strength for a move from a selection of levels, each level resulting in a different
        /// base power.
        /// </summary>
        public class Magnitude : MoveEffect
        {
            /// <summary>
            /// The different magnitude levels, each with their own power and probability.
            /// </summary>
            public List<MagnitudeLevel> magnitudeLevels;
            /// <summary>
            /// The text displayed for a magnitude level.
            /// </summary>
            public string displayText;

            public Magnitude(
                IEnumerable<MagnitudeLevel> magnitudeLevels = null, string displayText = "move-magnitude"
                )
                : base(effectType: MoveEffectType.Magnitude)
            {
                this.magnitudeLevels = new List<MagnitudeLevel>();
                this.displayText = displayText;
                if (magnitudeLevels != null)
                {
                    this.magnitudeLevels = new List<MagnitudeLevel>(magnitudeLevels);
                }
                else
                {
                    this.magnitudeLevels = new List<MagnitudeLevel>
                    {
                        new MagnitudeLevel(level: 4, basePower: 10, chance: 0.05f),
                        new MagnitudeLevel(level: 5, basePower: 30, chance: 0.1f),
                        new MagnitudeLevel(level: 6, basePower: 50, chance: 0.2f),
                        new MagnitudeLevel(level: 7, basePower: 70, chance: 0.3f),
                        new MagnitudeLevel(level: 8, basePower: 90, chance: 0.2f),
                        new MagnitudeLevel(level: 9, basePower: 110, chance: 0.1f),
                        new MagnitudeLevel(level: 10, basePower: 150, chance: 0.05f),
                    };
                }
            }
            public new Magnitude Clone()
            {
                return new Magnitude(magnitudeLevels: magnitudeLevels, displayText: displayText);
            }
            public MagnitudeLevel GetAMagnitudeLevel()
            {
                MagnitudeLevel level = (magnitudeLevels.Count == 0) ? null : magnitudeLevels[0];

                List<float> levelChances = new List<float>();
                float totalChance = 0;
                for (int i = 0; i < magnitudeLevels.Count; i++)
                {
                    totalChance += magnitudeLevels[i].chance;
                    levelChances.Add(totalChance);
                }

                // Normalize level chances
                if (totalChance > 0)
                {
                    for (int i = 0; i < levelChances.Count; i++)
                    {
                        levelChances[i] /= totalChance;
                    }

                    // Calculate level by chance
                    float randValue = Random.value;
                    for (int i = 0; i < levelChances.Count; i++)
                    {
                        if (randValue <= levelChances[i])
                        {
                            return magnitudeLevels[i];
                        }
                        else if (i == levelChances.Count - 1)
                        {
                            return magnitudeLevels[i];
                        }
                    }
                }

                return level;
            }
            public class MagnitudeLevel
            {
                public int level;
                public int basePower;
                public float chance;
                public MagnitudeLevel(int level = 4, int basePower = 10, float chance = 0.05f)
                {
                    this.level = level;
                    this.basePower = basePower;
                    this.chance = chance;
                }
                public MagnitudeLevel Clone()
                {
                    return new MagnitudeLevel(level: level, basePower: basePower, chance: chance);
                }
            }
        }

        /// <summary>
        /// Specialized version of protect that is suited for team-based protection.
        /// </summary>
        public class MatBlock : Protect
        {
            public MatBlock(
                General.Protect protect,
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(
                      protect: protect,
                      targetType: MoveEffectTargetType.SelfTeam, occurrence: MoveEffectOccurrence.Once,
                      filters: filters)
            {

            }
        }

        /// <summary>
        /// Changes the move's type and power based on the berry held. Fails if the user is not
        /// holding a berry, or can't use its berry.
        /// </summary>
        public class NaturalGift : MoveEffect
        {
            public NaturalGift() : base(effectType: MoveEffectType.NaturalGift)
            { }
            public new NaturalGift Clone()
            {
                return new NaturalGift();
            }
        }

        /// <summary>
        /// Becomes a <seealso cref="MoveCategory.Physical"/> move if the user's Attack is higher than its Special
        /// Attack. Becomes <seealso cref="MoveCategory.Special"/> move if the user's Special Attack is higher.
        /// </summary>
        public class PhotonGeyser : MoveEffect
        {
            public PhotonGeyser() : base(effectType: MoveEffectType.PhotonGeyser) 
            { }
            public new PhotonGeyser Clone()
            {
                return new PhotonGeyser();
            }
        }

        /// <summary>
        /// Attacks using the target's item.
        /// </summary>
        public class Poltergeist : MoveEffect
        {
            /// <summary>
            /// Set this flag to cause this move to fail if the target doesn't have any held item.
            /// </summary>
            public bool failOnNoItem;
            /// <summary>
            /// Text displayed when using the target's item.
            /// </summary>
            public string displayText;

            public Poltergeist(
                bool failOnNoItem = true, 
                string displayText = "move-poltergeist"
                )
                : base(effectType: MoveEffectType.Poltergeist)
            {
                this.failOnNoItem = failOnNoItem;
                this.displayText = displayText;
            }
            public new Poltergeist Clone()
            {
                return new Poltergeist(failOnNoItem: failOnNoItem, displayText: displayText);
            }
        }

        /// <summary>
        /// Protects the user from most moves. Less likely to succeed with consecutive uses.
        /// </summary>
        public class Protect : MoveEffect
        {
            /// <summary>
            /// Defines how the user is protected.
            /// </summary>
            public General.Protect protect;

            public Protect(
                General.Protect protect,

                MoveEffectTargetType targetType = MoveEffectTargetType.Self,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.Once,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: MoveEffectType.Protect,
                      timing: MoveEffectTiming.AfterSuccessfulMoveUse, 
                      targetType: targetType, occurrence: occurrence, filters: filters)
            {
                this.protect = protect.Clone();
            }
            public new Protect Clone()
            {
                return new Protect(
                    protect: protect,
                    targetType: targetType, occurrence: occurrence, filters: filters
                    );
            }
        }

        /// <summary>
        /// Inflicts a random set amount of damage that factors in the user's level.
        /// </summary>
        public class Psywave : MoveEffect
        {
            /// <summary>
            /// The lowest scaling value to apply to the damage.
            /// </summary>
            public int lowestScaleValue;

            public Psywave(int lowestScaleValue = 50) : base(effectType: MoveEffectType.Psywave)
            {
                this.lowestScaleValue = lowestScaleValue;
            }
            public new Psywave Clone()
            {
                return new Psywave(lowestScaleValue: lowestScaleValue);
            }
        }

        /// <summary>
        /// Increases base power for each positive stat boost.
        /// </summary>
        public class Punishment : MoveEffect
        {
            /// <summary>
            /// The lowest base power for this move.
            /// </summary>
            public int minimumPower;
            /// <summary>
            /// The highest base power for this move.
            /// </summary>
            public int maximumPower;

            /// <summary>
            /// The base power boost given for positive stat stage changes for this stat.
            /// </summary>
            public int ATKPlus, DEFPlus, SPAPlus, SPDPlus, SPEPlus, ACCPlus, EVAPlus;
            /// <summary>
            /// The base power boost given for negative stat stage changes for this stat.
            /// </summary>
            public int ATKMinus, DEFMinus, SPAMinus, SPDMinus, SPEMinus, ACCMinus, EVAMinus;

            public Punishment(
                int minimumPower = 60, int maximumPower = 200,
                int ATKPlus = 20, int DEFPlus = 20, int SPAPlus = 20, int SPDPlus = 20,
                int SPEPlus = 20, int ACCPlus = 20, int EVAPlus = 20,
                int ATKMinus = 0, int DEFMinus = 0, int SPAMinus = 0, int SPDMinus = 0,
                int SPEMinus = 0, int ACCMinus = 0, int EVAMinus = 0
                )
                : base(effectType: MoveEffectType.Punishment)
            {
                this.minimumPower = minimumPower;
                this.maximumPower = maximumPower;

                this.ATKPlus = ATKPlus;
                this.DEFPlus = DEFPlus;
                this.SPAPlus = SPAPlus;
                this.SPDPlus = SPDPlus;
                this.SPEPlus = SPEPlus;
                this.ACCPlus = ACCPlus;
                this.EVAPlus = EVAPlus;

                this.ATKMinus = ATKMinus;
                this.DEFMinus = DEFMinus;
                this.SPAMinus = SPAMinus;
                this.SPDMinus = SPDMinus;
                this.SPEMinus = SPEMinus;
                this.ACCMinus = ACCMinus;
                this.EVAMinus = EVAMinus;
            }
            public new Punishment Clone()
            {
                return new Punishment(
                    minimumPower: minimumPower, maximumPower: maximumPower,
                    ATKPlus: ATKPlus, DEFPlus: DEFPlus, SPAPlus: SPAPlus, SPDPlus: SPDPlus,
                    SPEPlus: SPEPlus, ACCPlus: ACCPlus, EVAPlus: EVAPlus,
                    ATKMinus: ATKMinus, DEFMinus: DEFMinus, SPAMinus: SPAMinus, SPDMinus: SPDMinus,
                    SPEMinus: SPEMinus, ACCMinus: ACCMinus, EVAMinus: EVAMinus
                    );
            }
            public int GetBasePower(Pokemon pokemon)
            {
                int basePower = minimumPower;

                // Attack
                if (pokemon.bProps.ATKStage > 0)
                {
                    basePower += ATKPlus * pokemon.bProps.ATKStage;
                }
                else if (pokemon.bProps.ATKStage < 0)
                {
                    basePower += ATKMinus * Mathf.Abs(pokemon.bProps.ATKStage);
                }

                // Defense
                if (pokemon.bProps.DEFStage > 0)
                {
                    basePower += DEFPlus * pokemon.bProps.DEFStage;
                }
                else if (pokemon.bProps.DEFStage < 0)
                {
                    basePower += DEFMinus * Mathf.Abs(pokemon.bProps.DEFStage);
                }

                // Special Attack
                if (pokemon.bProps.SPAStage > 0)
                {
                    basePower += SPAPlus * pokemon.bProps.SPAStage;
                }
                else if (pokemon.bProps.SPAStage < 0)
                {
                    basePower += SPAMinus * Mathf.Abs(pokemon.bProps.SPAStage);
                }

                // Special Defense
                if (pokemon.bProps.SPDStage > 0)
                {
                    basePower += SPDPlus * pokemon.bProps.SPDStage;
                }
                else if (pokemon.bProps.SPDStage < 0)
                {
                    basePower += SPDMinus * Mathf.Abs(pokemon.bProps.SPDStage);
                }

                // Speed
                if (pokemon.bProps.SPEStage > 0)
                {
                    basePower += SPEPlus * pokemon.bProps.SPEStage;
                }
                else if (pokemon.bProps.SPEStage < 0)
                {
                    basePower += SPEMinus * Mathf.Abs(pokemon.bProps.SPEStage);
                }

                // Accuracy
                if (pokemon.bProps.ACCStage > 0)
                {
                    basePower += ACCPlus * pokemon.bProps.ACCStage;
                }
                else if (pokemon.bProps.ACCStage < 0)
                {
                    basePower += ACCMinus * Mathf.Abs(pokemon.bProps.ACCStage);
                }

                // Evasion
                if (pokemon.bProps.EVAStage > 0)
                {
                    basePower += EVAPlus * pokemon.bProps.EVAStage;
                }
                else if (pokemon.bProps.EVAStage < 0)
                {
                    basePower += EVAMinus * Mathf.Abs(pokemon.bProps.EVAStage);
                }

                return Mathf.Clamp(basePower, minimumPower, maximumPower);
            }
        }

        /// <summary>
        /// Allows for this move to hit its target right before it can switch out.
        /// </summary>
        public class Pursuit : MoveEffect
        {
            /// <summary>
            /// The amount by which base power is scaled for a target switching out.
            /// </summary>
            public float damageScale;
            /// <summary>
            /// This effect can apply to enemy Pokémon.
            /// </summary>
            public bool applyToEnemies;
            /// <summary>
            /// This effect can apply to ally Pokémon.
            /// </summary>
            public bool applyToAllies;

            public Pursuit(
                float damageScale = 2f,
                bool applyToEnemies = true, bool applyToAllies = false)
                : base(effectType: MoveEffectType.Pursuit)
            {
                this.damageScale = damageScale;
                this.applyToEnemies = applyToEnemies;
                this.applyToAllies = applyToAllies;
            }
            public new Pursuit Clone()
            {
                return new Pursuit(
                    damageScale: damageScale,
                    applyToEnemies: applyToEnemies, applyToAllies: applyToAllies);
            }
        }

        /// <summary>
        /// Heals the target's specified status conditions.
        /// </summary>
        public class Refresh : MoveEffect
        {
            /// <summary>
            /// The statuses that are healed.
            /// </summary>
            public List<string> statuses;

            /// <summary>
            /// Statuses with the specified effect types are healed.
            /// </summary>
            public HashSet<PokemonSEType> statusEffectTypes;

            /// <summary>
            /// The text displayed when refresh fails to heal any status.
            /// </summary>
            public string failText;

            public Refresh(
                IEnumerable<string> statuses = null,
                IEnumerable<PokemonSEType> statusEffectTypes = null,
                string failText = "move-refresh-fail",

                MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
                MoveEffectTargetType targetType = MoveEffectTargetType.Target,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,
                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false
                )
                : base(effectType: MoveEffectType.Refresh, timing: timing, targetType: targetType,
                      occurrence: occurrence, filters: filters,
                      chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay)
            {
                this.statuses = (statuses == null) ? new List<string>() : new List<string>(statuses);
                this.statusEffectTypes = (statusEffectTypes == null) ? new HashSet<PokemonSEType>()
                    : new HashSet<PokemonSEType>(statusEffectTypes);
                this.failText = failText;
            }
            public new Refresh Clone()
            {
                return new Refresh(
                    statuses: statuses,
                    statusEffectTypes: statusEffectTypes,
                    failText: failText,
                    timing: timing, targetType: targetType, filters: filters, occurrence: occurrence,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                    );
            }
        }

        /// <summary>
        /// Alternates the user's forms after successful use, if it is one of the given forms.
        /// </summary>
        public class RelicSong : MoveEffect
        {
            /// <summary>
            /// Pokémon ID 1.
            /// </summary>
            public string form1;
            /// <summary>
            /// Pokémon ID 2.
            /// </summary>
            public string form2;
            /// <summary>
            /// The text that displays after the form change.
            /// </summary>
            public string afterText;

            public RelicSong(
                string form1, string form2, 
                string afterText = "pokemon-changeform"
                )
                : base(effectType: MoveEffectType.RelicSong)
            {
                this.form1 = form1;
                this.form2 = form2;
                this.afterText = afterText;
            }
            public new RelicSong Clone()
            {
                return new RelicSong(
                    form1: form1, form2: form2, 
                    afterText: afterText
                    );
            }
        }

        /// <summary>
        /// Determines base power based on the user's remaining HP %. The HP % must be below specified thresholds
        /// to determine base power.
        /// </summary>
        public class Reversal : MoveEffect
        {
            public class ReversalPower
            {
                public int basePower;
                public float HPThreshold;

                public ReversalPower(int basePower = 40, float HPThreshold = 0.6875f)
                {
                    this.basePower = basePower;
                    this.HPThreshold = HPThreshold;
                }
                public ReversalPower Clone()
                {
                    return new ReversalPower(basePower: basePower, HPThreshold: HPThreshold);
                }
            }

            /// <summary>
            /// The lowest possible base power for this move.
            /// </summary>
            public int lowestBasePower;

            /// <summary>
            /// An list of reversal base powers ordered descending by the HP threshold needed to obtain that power.
            /// </summary>
            public List<ReversalPower> reversalPowers;

            public Reversal(
                int lowestBasePower = 20,
                IEnumerable<ReversalPower> reversalPowers = null
                )
                : base(effectType: MoveEffectType.Reversal)
            {
                this.reversalPowers = (reversalPowers != null) ? new List<ReversalPower>(reversalPowers)
                    : new List<ReversalPower>
                    {
                        new ReversalPower(basePower: 40, HPThreshold: 0.6875f),
                        new ReversalPower(basePower: 80, HPThreshold: 0.3542f),
                        new ReversalPower(basePower: 100, HPThreshold: 0.2083f),
                        new ReversalPower(basePower: 150, HPThreshold: 0.1042f),
                        new ReversalPower(basePower: 200, HPThreshold: 0.0417f),
                    };
            }
            public new Reversal Clone()
            {
                return new Reversal(lowestBasePower: lowestBasePower, reversalPowers: reversalPowers);
            }
        }

        /// <summary>
        /// Increases power if the target is affected by the given battle conditions.
        /// </summary>
        public class RisingVoltage : MoveEffect
        {
            /// <summary>
            /// The amount that damage is scaled by.
            /// </summary>
            public float damageScale;
            /// <summary>
            /// The relevant battle conditions.
            /// </summary>
            public List<string> conditions;

            public RisingVoltage(
                float damageScale = 1f,
                IEnumerable<string> conditions = null
                )
                : base(effectType: MoveEffectType.RisingVoltage)
            {
                this.damageScale = damageScale;
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
            }
            public new RisingVoltage Clone()
            {
                return new RisingVoltage(damageScale: damageScale, conditions: conditions);
            }
        }

        /// <summary>
        /// The move is used consecutively for a set amount of turns, and consecutively scales
        /// for each consecutive execution.
        /// </summary>
        public class Rollout : MoveEffect
        {
            /// <summary>
            /// The amount by which damage is scaled for a consecutive hit.
            /// </summary>
            public float damageScale;
            /// <summary>
            /// The maximum amount of move executions before the damage scale resets, or the move ends.
            /// WARNING: Setting it to -1 will use the move for an infinite amount of turns.
            /// </summary>
            public int maxExecutions;

            /// <summary>
            /// Unsets this move if the move was disrupted.
            /// </summary>
            public bool endOnFail;
            /// <summary>
            /// Unsets this move if it is on the maximum execution.
            /// </summary>
            public bool endOnMaxHits;

            public Rollout(
                float damageScale = 2f,
                int maxExecutions = 5,
                bool endOnFail = true, bool endOnMaxHits = false
                )
                : base(effectType: MoveEffectType.Rollout)
            {
                this.damageScale = damageScale;
                this.maxExecutions = maxExecutions;
                this.endOnFail = endOnFail;
                this.endOnMaxHits = endOnMaxHits;
            }
            public new Rollout Clone()
            {
                return new Rollout(
                    damageScale: damageScale,
                    maxExecutions: maxExecutions,
                    endOnFail: endOnFail, endOnMaxHits: endOnMaxHits
                    );
            }
        }

        /// <summary>
        /// Adds secondary effects to this move depending on the environment or terrain.
        /// </summary>
        public class SecretPower : MoveEffect
        {
            /// <summary>
            /// The chance for each additional effect to occur.
            /// </summary>
            public float secondaryEffectChance;

            public SecretPower(
                float secondaryEffectChance = -1,

                MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
                MoveEffectTargetType targetType = MoveEffectTargetType.Target,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,
                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false
                ) 
                : base(
                      effectType: MoveEffectType.InflictPokemonSC, timing: timing, targetType: targetType,
                      filters: filters,
                      chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay)
            {
                this.secondaryEffectChance = secondaryEffectChance;
            }
            public new SecretPower Clone()
            {
                return new SecretPower(
                    secondaryEffectChance: secondaryEffectChance,
                    timing: timing, targetType: targetType, filters: filters,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                    );
            }
        }

        /// <summary>
        /// Makes this move deal a set amount of damage equal to the user's level.
        /// </summary>
        public class SeismicToss : MoveEffect
        {
            public SeismicToss() : base(effectType: MoveEffectType.SeismicToss) { }
            public new SeismicToss Clone()
            {
                return new SeismicToss();
            }
        }

        /// <summary>
        /// Becomes a <seealso cref="MoveCategory.Physical"/> move if the target's Defense is lower than its Special
        /// Defense. Becomes <seealso cref="MoveCategory.Special"/> move if the target's Special Defense is lower.
        /// </summary>
        public class ShellSideArm : MoveEffect
        {
            public ShellSideArm() : base(effectType: MoveEffectType.ShellSideArm)
            { }
            public new ShellSideArm Clone()
            {
                return new ShellSideArm();
            }
        }

        /// <summary>
        /// Enables the use of a move when the Pokémon is asleep.
        /// </summary>
        public class Snore : MoveEffect
        {
            /// <summary>
            /// If true, this move can only be used when the Pokémon is asleep.
            /// </summary>
            public bool onlyAsleep;

            public Snore(
                bool onlyAsleep = true
                )
                : base(effectType: MoveEffectType.Snore)
            {
                this.onlyAsleep = onlyAsleep;
            }
            public new Snore Clone()
            {
                return new Snore(onlyAsleep: onlyAsleep);
            }
        }

        /// <summary>
        /// Adds or Subtracts the stat stages of the chosen target Pokémon.
        /// </summary>
        public class StatStageMod : MoveEffect
        {
            public General.StatStageMod statStageMod;

            public StatStageMod(
                General.StatStageMod statStageMod,

                MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
                MoveEffectTargetType targetType = MoveEffectTargetType.Target,
                MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,
                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false
                )
                : base(
                      effectType: MoveEffectType.InflictPokemonSC, timing: timing, targetType: targetType,
                      occurrence: occurrence, filters: filters,
                      chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new StatStageMod Clone()
            {
                return new StatStageMod(
                    statStageMod: statStageMod,

                    timing: timing, targetType: targetType, filters: filters, occurrence: occurrence,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                    );
            }
        }
    
        /// <summary>
        /// Immediately destroys the existing terrain.
        /// </summary>
        public class SteelRoller : MoveEffect
        {
            /// <summary>
            /// The text displayed when the terrain is destroyed.
            /// </summary>
            public string displayText;
            /// <summary>
            /// If true, this move fails when there's no terrain.
            /// </summary>
            public bool failOnNoTerrain;

            public SteelRoller(
                string displayText = "move-steelroller",
                bool failOnNoTerrain = true
                )
                : base(effectType: MoveEffectType.SteelRoller, 
                      targetType: MoveEffectTargetType.Battlefield, 
                      timing: MoveEffectTiming.AfterTargetImpact, 
                      occurrence: MoveEffectOccurrence.Once)
            {
                this.displayText = displayText;
                this.failOnNoTerrain = failOnNoTerrain;
            }
            public new SteelRoller Clone()
            {
                return new SteelRoller(displayText: displayText, failOnNoTerrain: failOnNoTerrain);
            }
        }

        /// <summary>
        /// Increases base power for each positive stat boost.
        /// </summary>
        public class StoredPower : MoveEffect
        {
            /// <summary>
            /// The base power boost given for positive stat stage changes for this stat.
            /// </summary>
            public int ATKPlus, DEFPlus, SPAPlus, SPDPlus, SPEPlus, ACCPlus, EVAPlus;
            /// <summary>
            /// The base power boost given for negative stat stage changes for this stat.
            /// </summary>
            public int ATKMinus, DEFMinus, SPAMinus, SPDMinus, SPEMinus, ACCMinus, EVAMinus;

            public StoredPower(
                int ATKPlus = 20, int DEFPlus = 20, int SPAPlus = 20, int SPDPlus = 20,
                int SPEPlus = 20, int ACCPlus = 20, int EVAPlus = 20,
                int ATKMinus = 0, int DEFMinus = 0, int SPAMinus = 0, int SPDMinus = 0,
                int SPEMinus = 0, int ACCMinus = 0, int EVAMinus = 0
                )
                : base(effectType: MoveEffectType.StoredPower)
            {
                this.ATKPlus = ATKPlus;
                this.DEFPlus = DEFPlus;
                this.SPAPlus = SPAPlus;
                this.SPDPlus = SPDPlus;
                this.SPEPlus = SPEPlus;
                this.ACCPlus = ACCPlus;
                this.EVAPlus = EVAPlus;

                this.ATKMinus = ATKMinus;
                this.DEFMinus = DEFMinus;
                this.SPAMinus = SPAMinus;
                this.SPDMinus = SPDMinus;
                this.SPEMinus = SPEMinus;
                this.ACCMinus = ACCMinus;
                this.EVAMinus = EVAMinus;
            }
            public new StoredPower Clone()
            {
                return new StoredPower(
                    ATKPlus: ATKPlus, DEFPlus: DEFPlus, SPAPlus: SPAPlus, SPDPlus: SPDPlus,
                    SPEPlus: SPEPlus, ACCPlus: ACCPlus, EVAPlus: EVAPlus,
                    ATKMinus: ATKMinus, DEFMinus: DEFMinus, SPAMinus: SPAMinus, SPDMinus: SPDMinus,
                    SPEMinus: SPEMinus, ACCMinus: ACCMinus, EVAMinus: EVAMinus
                    );
            }
            public int GetPowerBoost(Pokemon pokemon)
            {
                int basePower = 0;

                // Attack
                if (pokemon.bProps.ATKStage > 0)
                {
                    basePower += ATKPlus * pokemon.bProps.ATKStage;
                }
                else if (pokemon.bProps.ATKStage < 0)
                {
                    basePower += ATKMinus * Mathf.Abs(pokemon.bProps.ATKStage);
                }

                // Defense
                if (pokemon.bProps.DEFStage > 0)
                {
                    basePower += DEFPlus * pokemon.bProps.DEFStage;
                }
                else if (pokemon.bProps.DEFStage < 0)
                {
                    basePower += DEFMinus * Mathf.Abs(pokemon.bProps.DEFStage);
                }

                // Special Attack
                if (pokemon.bProps.SPAStage > 0)
                {
                    basePower += SPAPlus * pokemon.bProps.SPAStage;
                }
                else if (pokemon.bProps.SPAStage < 0)
                {
                    basePower += SPAMinus * Mathf.Abs(pokemon.bProps.SPAStage);
                }

                // Special Defense
                if (pokemon.bProps.SPDStage > 0)
                {
                    basePower += SPDPlus * pokemon.bProps.SPDStage;
                }
                else if (pokemon.bProps.SPDStage < 0)
                {
                    basePower += SPDMinus * Mathf.Abs(pokemon.bProps.SPDStage);
                }

                // Speed
                if (pokemon.bProps.SPEStage > 0)
                {
                    basePower += SPEPlus * pokemon.bProps.SPEStage;
                }
                else if (pokemon.bProps.SPEStage < 0)
                {
                    basePower += SPEMinus * Mathf.Abs(pokemon.bProps.SPEStage);
                }

                // Accuracy
                if (pokemon.bProps.ACCStage > 0)
                {
                    basePower += ACCPlus * pokemon.bProps.ACCStage;
                }
                else if (pokemon.bProps.ACCStage < 0)
                {
                    basePower += ACCMinus * Mathf.Abs(pokemon.bProps.ACCStage);
                }

                // Evasion
                if (pokemon.bProps.EVAStage > 0)
                {
                    basePower += EVAPlus * pokemon.bProps.EVAStage;
                }
                else if (pokemon.bProps.EVAStage < 0)
                {
                    basePower += EVAMinus * Mathf.Abs(pokemon.bProps.EVAStage);
                }

                return basePower;
            }
        }

        /// <summary>
        /// Specialized version of Karate Chop that guarantees critical hits.
        /// </summary>
        public class StormThrow : KarateChop
        {
            public StormThrow() : base(alwaysCritical: true) { }
        }

        /// <summary>
        /// The move immediately fails to be used if the target does not use a physical or special attack
        /// or has already moved this turn.
        /// </summary>
        public class SuckerPunch : MoveEffect
        {
            /// <summary>
            /// The target must select a move in one of these categories.
            /// </summary>
            public HashSet<MoveCategory> categories;
            /// <summary>
            /// Any moves used here will also make Sucker Punch valid.
            /// </summary>
            public List<string> allowableMoves;
            /// <summary>
            /// Invert filters to invert the allowable moves, including additional filters set.
            /// </summary>
            public bool invertFilter;

            public SuckerPunch(
                IEnumerable<MoveCategory> categories = null,
                IEnumerable<string> allowableMoves = null,
                bool invertFilter = false
                )
                : base(effectType: MoveEffectType.SuckerPunch)
            {
                this.categories = (categories == null) 
                    ? new HashSet<MoveCategory> { MoveCategory.Physical, MoveCategory.Special }
                    : new HashSet<MoveCategory>(categories);
                this.allowableMoves = (allowableMoves == null) 
                    ? new List<string> { "mefirst" } 
                    : new List<string>(allowableMoves);
                this.invertFilter = invertFilter;
            }
            public new SuckerPunch Clone()
            {
                return new SuckerPunch(
                    categories: categories,
                    allowableMoves: allowableMoves,
                    invertFilter: invertFilter
                    );
            }
        }

        /// <summary>
        /// This move ignores the effects of ignorable abilities.
        /// </summary>
        public class SunsteelStrike : MoveEffect
        {
            public SunsteelStrike() : base(effectType: MoveEffectType.SunteelStrike)
            { }
            public new SunsteelStrike Clone()
            {
                return new SunsteelStrike();
            }
        }

        /// <summary>
        /// Makes this move deal a set amount of damage equal to a percentage of the target's remaining HP.
        /// </summary>
        public class SuperFang : MoveEffect
        {
            /// <summary>
            /// The percentage of the target's remaining HP that is dealt as damage.
            /// </summary>
            public float damagePercent;
        
            public SuperFang(float damagePercent = 0.5f) : base(effectType: MoveEffectType.SuperFang)
            {
                this.damagePercent = damagePercent;
            }
            public new SuperFang Clone()
            {
                return new SuperFang(damagePercent: damagePercent);
            }
        }

        /// <summary>
        /// This move fails if the target doesn't share types with the user.
        /// </summary>
        public class Synchronoise : MoveEffect
        {
            /// <summary>
            /// Set to true to fail the move if the target's types don't exactly match the user's.
            /// </summary>
            public bool exactMatch;
            /// <summary>
            /// Set to true to invert the checks.
            /// </summary>
            public bool invert;
            /// <summary>
            /// Special text that displays on move failure.
            /// </summary>
            public string failTextID;
        
            public Synchronoise(
                bool exactMatch = false, bool invert = false,
                string failTextID = null
                ) : base(effectType: MoveEffectType.Synchronoise)
            {
                this.exactMatch = exactMatch;
                this.invert = invert;
                this.failTextID = failTextID;
            }
            public new Synchronoise Clone()
            {
                return new Synchronoise(
                    exactMatch: exactMatch, invert: invert, failTextID: failTextID
                    );
            }
        }

        /// <summary>
        /// Scales damage if the terrain allows for it.
        /// </summary>
        public class TerrainPulse : MoveEffect
        {
            /// <summary>
            /// The amount by which damage is scaled.
            /// </summary>
            public float damageScale;

            public TerrainPulse(float damageScale = 2f)
                : base(effectType: MoveEffectType.TerrainPulse)
            {
                this.damageScale = damageScale;
            }
            public new TerrainPulse Clone()
            {
                return new TerrainPulse(damageScale: damageScale);
            }
        }

        /// <summary>
        /// To be used with status moves. Forces this move to account for type immunities.
        /// </summary>
        public class ThunderWave : MoveEffect
        {
            public ThunderWave() : base(effectType: MoveEffectType.ThunderWave) { }
            public new ThunderWave Clone()
            {
                return new ThunderWave();
            }
        }

        /// <summary>
        /// Hits multiple times, each consecutive hit increasing in base power.
        /// </summary>
        public class TripleKick : MoveEffect
        {
            /// <summary>
            /// The amount of hits for this attack.
            /// </summary>
            public int hits;

            public TripleKick(int hits = 3)
                : base(effectType: MoveEffectType.TripleKick)
            {
                this.hits = hits;
            }
            public new TripleKick Clone()
            {
                return new TripleKick(hits: hits);
            }

        }

        /// <summary>
        /// Scales damage if the weather allows for it.
        /// </summary>
        public class WeatherBall : MoveEffect
        {
            /// <summary>
            /// The amount by which damage is scaled.
            /// </summary>
            public float damageScale;

            public WeatherBall(float damageScale = 2f)
                : base(effectType: MoveEffectType.WeatherBall)
            {
                this.damageScale = damageScale;
            }
            public new WeatherBall Clone()
            {
                return new WeatherBall(damageScale: damageScale);
            }
        }

        /// <summary>
        /// Renders the target's held item unusable for the remainder of the battle.
        /// </summary>
        public class Whirlwind : MoveEffect
        {
            /// <summary>
            /// The text displayed when the target is forced out of battle.
            /// </summary>
            public string forceOutText;
            /// <summary>
            /// The text displayed when a Pokémon is forced into battle.
            /// </summary>
            public string forceInText;
            /// <summary>
            /// The text displayed when a Pokémon fails to be switched out.
            /// </summary>
            public string failText;

            public Whirlwind(
                string forceOutText = "move-whirlwind", string forceInText = "pokemon-forcein",
                string failText = "move-whirlwind-fail",
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: MoveEffectType.Whirlwind, filters: filters)
            {
                this.forceOutText = forceOutText;
                this.forceInText = forceInText;
            }
            public new Whirlwind Clone()
            {
                return new Whirlwind(
                    forceOutText: forceOutText,
                    forceInText: forceInText,
                    failText: failText,
                    filters: filters);
            }
        }

        /// <summary>
        /// Sets the target's ability to the specified abilities.
        /// </summary>
        public class WorrySeed : MoveEffect
        {
            /// <summary>
            /// The abilities to set.
            /// </summary>
            public List<string> abilities;
            /// <summary>
            /// If true, the abilities set are the same as the users.
            /// </summary>
            public bool entrainment;

            /// <summary>
            /// The text displayed for the abilities the target gains.
            /// </summary>
            public string gainText;
            /// <summary>
            /// The text displayed for the abilities that are replaced on the target.
            /// </summary>
            public string loseText;
            /// <summary>
            /// The text displayed for when the target fails to acquire these abilities.
            /// </summary>
            public string failText;

            public WorrySeed(
                IEnumerable<string> abilities = null,
                bool entrainment = false,
                string gainText = "pokemon-ability-gain", 
                string loseText = "pokemon-ability-lose",
                string failText = "move-worryseed-fail"
                )
                : base(effectType: MoveEffectType.WorrySeed)
            {
                this.abilities = (abilities == null) ? new List<string>() : new List<string>(abilities);
                this.entrainment = entrainment;
                this.gainText = gainText;
                this.loseText = loseText;
                this.failText = failText;
            }
            public new WorrySeed Clone()
            {
                return new WorrySeed(
                    abilities: abilities,
                    entrainment: entrainment,
                    gainText: gainText, loseText: loseText, failText: failText
                    );
            }
        }
    }


    // ---ABILITY EFFECTS---
    /// <summary>
    /// Collection of all Ability Effects.
    /// </summary>
    public class AbilityEff
    {
        public class AbilityEffect
        {
            /// <summary>
            /// The type of effect that this is.
            /// </summary>
            public AbilityEffectType effectType;
            /// <summary>
            /// Additional restrictions on how the effect is applied.
            /// </summary>
            public List<Filter.FilterEffect> filters;

            /// <summary>
            /// The chance of the effect working.
            /// </summary>
            public float chance;

            public AbilityEffect(
                AbilityEffectType effectType,
                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1)
            {
                this.effectType = effectType;
                this.filters = new List<Filter.FilterEffect>();
                if (filters != null)
                {
                    List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                    for (int i = 0; i < tempFilters.Count; i++)
                    {
                        this.filters.Add(tempFilters[i].Clone());
                    }
                }

                this.chance = chance;
            }
            public AbilityEffect Clone()
            {
                return
                    (this is Adaptability) ? (this as Adaptability).Clone()
                    : (this is Aerilate) ? (this as Aerilate).Clone()
                    : (this is Aftermath) ? (this as Aftermath).Clone()
                    : (this is AirLock) ? (this as AirLock).Clone()
                    : (this is Analytic) ? (this as Analytic).Clone()
                    : (this is AngerPoint) ? (this as AngerPoint).Clone()
                    : (this is Anticipation) ? (this as Anticipation).Clone()
                    : (this is AuraBreak) ? (this as AuraBreak).Clone()
                    : (this is BadDreams) ? (this as BadDreams).Clone()
                    : (this is BallFetch) ? (this as BallFetch).Clone()
                    : (this is Battery) ? (this as Battery).Clone()
                    : (this is BattleBond) ? (this as BattleBond).Clone()
                    : (this is BattleArmor) ? (this as BattleArmor).Clone()
                    : (this is BeastBoost) ? (this as BeastBoost).Clone()
                    : (this is Berserk) ? (this as Berserk).Clone()
                    : (this is Cacophony) ? (this as Cacophony).Clone()
                    : (this is CheekPouch) ? (this as CheekPouch).Clone()
                    : (this is ColorChange) ? (this as ColorChange).Clone()
                    : (this is Comatose) ? (this as Comatose).Clone()
                    : (this is CompoundEyes) ? (this as CompoundEyes).Clone()
                    : (this is Contrary) ? (this as Contrary).Clone()
                    : (this is Corrosion) ? (this as Corrosion).Clone()
                    : (this is Damp) ? (this as Damp).Clone()
                    : (this is Dancer) ? (this as Dancer).Clone()
                    : (this is DarkAura) ? (this as DarkAura).Clone()
                    : (this is Defiant) ? (this as Defiant).Clone()
                    : (this is Disguise) ? (this as Disguise).Clone()
                    : (this is Download) ? (this as Download).Clone()
                    : (this is Drought) ? (this as Drought).Clone()
                    : (this is DrySkin) ? (this as DrySkin).Clone()
                    : (this is EarlyBird) ? (this as EarlyBird).Clone()
                    : (this is FlameBody) ? (this as FlameBody).Clone()
                    : (this is Forecast) ? (this as Forecast).Clone()
                    : (this is Forewarn) ? (this as Forewarn).Clone()
                    : (this is FriendGuard) ? (this as FriendGuard).Clone()
                    : (this is Frisk) ? (this as Frisk).Clone()
                    : (this is Gluttony) ? (this as Gluttony).Clone()
                    : (this is Gooey) ? (this as Gooey).Clone()
                    : (this is GorillaTactics) ? (this as GorillaTactics).Clone()
                    : (this is GulpMissile) ? (this as GulpMissile).Clone()
                    : (this is Guts) ? (this as Guts).Clone()
                    : (this is Harvest) ? (this as Harvest).Clone()
                    : (this is Healer) ? (this as Healer).Clone()
                    : (this is HeavyMetal) ? (this as HeavyMetal).Clone()
                    : (this is HoneyGather) ? (this as HoneyGather).Clone()
                    : (this is HungerSwitch) ? (this as HungerSwitch).Clone()
                    : (this is Hustle) ? (this as Hustle).Clone()
                    : (this is Hydration) ? (this as Hydration).Clone()
                    : (this is HyperCutter) ? (this as HyperCutter).Clone()
                    : (this is IceScales) ? (this as IceScales).Clone()
                    : (this is Illusion) ? (this as Illusion).Clone()
                    : (this is Infiltrator) ? (this as Infiltrator).Clone()
                    : (this is Intimidate) ? (this as Intimidate).Clone()
                    : (this is IntimidateBlock) ? (this as IntimidateBlock).Clone()
                    : (this is IntimidateTrigger) ? (this as IntimidateTrigger).Clone()
                    : (this is IntrepidSword) ? (this as IntrepidSword).Clone()
                    : (this is IronFist) ? (this as IronFist).Clone()
                    : (this is Justified) ? (this as Justified).Clone()
                    : (this is Klutz) ? (this as Klutz).Clone()
                    : (this is Levitate) ? (this as Levitate).Clone()
                    : (this is LightningRod) ? (this as LightningRod).Clone()
                    : (this is Limber) ? (this as Limber).Clone()
                    : (this is LiquidOoze) ? (this as LiquidOoze).Clone()
                    : (this is LongReach) ? (this as LongReach).Clone()
                    : (this is MagicBounce) ? (this as MagicBounce).Clone()
                    : (this is MagicGuard) ? (this as MagicGuard).Clone()
                    : (this is Magician) ? (this as Magician).Clone()
                    : (this is Mimicry) ? (this as Mimicry).Clone()
                    : (this is Minus) ? (this as Minus).Clone()
                    : (this is MirrorArmor) ? (this as MirrorArmor).Clone()
                    : (this is MoldBreaker) ? (this as MoldBreaker).Clone()
                    : (this is Moody) ? (this as Moody).Clone()
                    : (this is Moxie) ? (this as Moxie).Clone()
                    : (this is Multitype) ? (this as Multitype).Clone()
                    : (this is Multiscale) ? (this as Multiscale).Clone()
                    : (this is Mummy) ? (this as Mummy).Clone()
                    : (this is NaturalCure) ? (this as NaturalCure).Clone()
                    : (this is NeutralizingGas) ? (this as NeutralizingGas).Clone()
                    : (this is NoGuard) ? (this as NoGuard).Clone()
                    : (this is Oblivious) ? (this as Oblivious).Clone()
                    : (this is Overcoat) ? (this as Overcoat).Clone()
                    : (this is ParentalBond) ? (this as ParentalBond).Clone()
                    : (this is Pickpocket) ? (this as Pickpocket).Clone()
                    : (this is Pickup) ? (this as Pickup).Clone()
                    : (this is PoisonHeal) ? (this as PoisonHeal).Clone()
                    : (this is PoisonPoint) ? (this as PoisonPoint).Clone()
                    : (this is PoisonTouch) ? (this as PoisonTouch).Clone()
                    : (this is PowerOfAlchemy) ? (this as PowerOfAlchemy).Clone()
                    : (this is Prankster) ? (this as Prankster).Clone()
                    : (this is PropellerTail) ? (this as PropellerTail).Clone()
                    : (this is Protean) ? (this as Protean).Clone()
                    : (this is Pressure) ? (this as Pressure).Clone()
                    : (this is QueenlyMajesty) ? (this as QueenlyMajesty).Clone()
                    : (this is QuickDraw) ? (this as QuickDraw).Clone()
                    : (this is Ripen) ? (this as Ripen).Clone()
                    : (this is Rivalry) ? (this as Rivalry).Clone()
                    : (this is RKSSystem) ? (this as RKSSystem).Clone()
                    : (this is RockHead) ? (this as RockHead).Clone()
                    : (this is RoughSkin) ? (this as RoughSkin).Clone()
                    : (this is Scrappy) ? (this as Scrappy).Clone()
                    : (this is ScreenCleaner) ? (this as ScreenCleaner).Clone()
                    : (this is SereneGrace) ? (this as SereneGrace).Clone()
                    : (this is ShadowTag) ? (this as ShadowTag).Clone()
                    : (this is ShieldDust) ? (this as ShieldDust).Clone()
                    : (this is ShieldsDown) ? (this as ShieldsDown).Clone()
                    : (this is Simple) ? (this as Simple).Clone()
                    : (this is SkillLink) ? (this as SkillLink).Clone()
                    : (this is SlowStart) ? (this as SlowStart).Clone()
                    : (this is Sniper) ? (this as Sniper).Clone()
                    : (this is SolidRock) ? (this as SolidRock).Clone()
                    : (this is SoulHeart) ? (this as SoulHeart).Clone()
                    : (this is SpeedBoost) ? (this as SpeedBoost).Clone()
                    : (this is Stall) ? (this as Stall).Clone()
                    : (this is Stakeout) ? (this as Stakeout).Clone()
                    : (this is StanceChange) ? (this as StanceChange).Clone()
                    : (this is Steadfast) ? (this as Steadfast).Clone()
                    : (this is StickyHold) ? (this as StickyHold).Clone()
                    : (this is Sturdy) ? (this as Sturdy).Clone()
                    : (this is SuctionCups) ? (this as SuctionCups).Clone()
                    : (this is SuperLuck) ? (this as SuperLuck).Clone()
                    : (this is Symbiosis) ? (this as Symbiosis).Clone()
                    : (this is Synchronize) ? (this as Synchronize).Clone()
                    : (this is Technician) ? (this as Technician).Clone()
                    : (this is Telepathy) ? (this as Telepathy).Clone()
                    : (this is TintedLens) ? (this as TintedLens).Clone()
                    : (this is Trace) ? (this as Trace).Clone()
                    : (this is Truant) ? (this as Truant).Clone()
                    : (this is Unaware) ? (this as Unaware).Clone()
                    : (this is Unburden) ? (this as Unburden).Clone()
                    : (this is UnseenFist) ? (this as UnseenFist).Clone()
                    : (this is VoltAbsorb) ? (this as VoltAbsorb).Clone()
                    : (this is WimpOut) ? (this as WimpOut).Clone()
                    : (this is WonderGuard) ? (this as WonderGuard).Clone()
                    : (this is WonderSkin) ? (this as WonderSkin).Clone()
                    : (this is ZenMode) ? (this as ZenMode).Clone()
                    : new AbilityEffect(
                        effectType: effectType,
                        chance: chance,
                        filters: filters);
            }
        }

        
        /// <summary>
        /// Modifies the user's STAB multiplier.
        /// </summary>
        public class Adaptability : AbilityEffect
        {
            /// <summary>
            /// The new STAB multiplier.
            /// </summary>
            public float STABMultiplier;

            public Adaptability(float STABMultiplier = 2f) : base (effectType: AbilityEffectType.Adaptability)
            {
                this.STABMultiplier = STABMultiplier;
            }
            public new Adaptability Clone()
            {
                return new Adaptability(STABMultiplier: STABMultiplier);
            }
        }
        
        /// <summary>
        /// Changes the types of certain moves, and modifies their base power.
        /// </summary>
        public class Aerilate : AbilityEffect
        {
            /// <summary>
            /// The power multiplier to apply to affected moves.
            /// </summary>
            public float powerMultiplier;
            /// <summary>
            /// The move type that will be changed.
            /// </summary>
            public string baseMoveType;
            /// <summary>
            /// The type that the move is transformed into.
            /// </summary>
            public string toMoveType;

            public Aerilate(
                float powerMultiplier = 1.2f,
                string baseMoveType = "normal", string toMoveType = "flying",

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Aerilate, filters: filters)
            {
                this.powerMultiplier = powerMultiplier;
                this.baseMoveType = baseMoveType;
                this.toMoveType = toMoveType;
            }
            public new Aerilate Clone()
            {
                return new Aerilate(
                    powerMultiplier: powerMultiplier,
                    baseMoveType: baseMoveType, toMoveType: toMoveType,
                    filters: filters);
            }
        }

        /// <summary>
        /// When this Pokémon faints by a direct attack, the attacker loses a portion of their HP.
        /// </summary>
        public class Aftermath : AbilityEffect
        {
            /// <summary>
            /// Defines how damage is dealt.
            /// </summary>
            public General.Damage damage;

            /// <summary>
            /// This effect is only triggered if the Pokémon faints due to a contact move.
            /// </summary>
            public bool onlyContact;
            /// <summary>
            /// If true, this ability is blocked if a Pokémon on the field has the Damp ability.
            /// </summary>
            public bool blockedByDamp;

            public Aftermath(
                General.Damage damage,
                bool onlyContact = true, bool blockedByDamp = true
                )
                : base(effectType: AbilityEffectType.Aftermath)
            {
                this.damage = damage.Clone();
                this.onlyContact = onlyContact;
                this.blockedByDamp = blockedByDamp;
            }
            public new Aftermath Clone()
            {
                return new Aftermath(
                    damage: damage,
                    onlyContact: onlyContact, blockedByDamp: blockedByDamp
                    );
            }
        }

        /// <summary>
        /// Eliminates the effects of weather.
        /// </summary>
        public class AirLock : AbilityEffect
        {
            /// <summary>
            /// The text that displays when weather is negated.
            /// </summary>
            public string displayText;

            public AirLock(string displayText = "ability-airlock") : base(effectType: AbilityEffectType.AirLock)
            {
                this.displayText = displayText;
            }
            public new AirLock Clone()
            {
                return new AirLock(displayText: displayText);
            }
        }

        /// <summary>
        /// Increases move power against targets that have already acted during the turn.
        /// </summary>
        public class Analytic : AbilityEffect
        {
            /// <summary>
            /// The power multiplier applied to moves.
            /// </summary>
            public float powerMultiplier;

            public Analytic(float powerMultiplier = 1.3f) : base(effectType: AbilityEffectType.Analytic)
            {
                this.powerMultiplier = powerMultiplier;
            }
            public new Analytic Clone()
            {
                return new Analytic(powerMultiplier: powerMultiplier);
            }
        }

        /// <summary>
        /// Modifies stats if the user is struck by a critical hit.
        /// </summary>
        public class AngerPoint : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied when critically hit.
            /// </summary>
            public General.StatStageMod statStageMod;

            public AngerPoint(General.StatStageMod statStageMod) 
                : base(effectType: AbilityEffectType.AngerPoint)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new AngerPoint Clone()
            {
                return new AngerPoint(statStageMod: statStageMod);
            }
        }

        /// <summary>
        /// Causes this Pokémon to shudder if the opposing Pokémon have super-effective or OHKO moves.
        /// </summary>
        public class Anticipation : AbilityEffect
        {
            /// <summary>
            /// Text displayed when Anticipation triggers.
            /// </summary>
            public string displayText;
            /// <summary>
            /// If true, this ability will trigger if the opposing Pokémon has OHKO moves.
            /// </summary>
            public bool notifyOHKO;

            public Anticipation(
                string displayText = "ability-anticipation",
                bool notifyOHKO = true
                )
                : base(effectType: AbilityEffectType.Anticipation)
            {
                this.displayText = displayText;
                this.notifyOHKO = notifyOHKO;
            }
            public new Anticipation Clone()
            {
                return new Anticipation(displayText: displayText, notifyOHKO: notifyOHKO);
            }
        }

        /// <summary>
        /// Reverses the effect of <seealso cref="DarkAura"/> abilities that are active.
        /// </summary>
        public class AuraBreak : AbilityEffect
        {
            /// <summary>
            /// The text displayed when this Pokémon enters battle.
            /// </summary>
            public string displayText;

            public AuraBreak(
                string displayText = "ability-aurabreak"
                ) : base(effectType: AbilityEffectType.AuraBreak)
            {
                this.displayText = displayText;
            }
            public new AuraBreak Clone()
            {
                return new AuraBreak(displayText: displayText);
            }
        }

        /// <summary>
        /// Reduces the HP of all sleeping opposing Pokémon every turn.
        /// </summary>
        public class BadDreams : AbilityEffect
        {
            /// <summary>
            /// The affected non-volatile statuses that opposing Pokémon must have in order to take damage.
            /// </summary>
            public List<string> affectedStatuses;
            /// <summary>
            /// The percentage of HP that the opposing Pokémon loses each turn.
            /// </summary>
            public float hpLossPercent;
            /// <summary>
            /// The text displayed when opposing Pokémon lose their HP.
            /// </summary>
            public string displayText;

            public BadDreams(
                float hpLossPercent = 1f/8,
                string displayText = "ability-baddreams",
                IEnumerable<string> affectedStatuses = null
                )
                : base(effectType: AbilityEffectType.BadDreams)
            {
                this.hpLossPercent = hpLossPercent;
                this.displayText = displayText;
                this.affectedStatuses = (affectedStatuses != null) ? new List<string>(affectedStatuses)
                    : new List<string>
                    {
                        "sleep"
                    };
            }
            public new BadDreams Clone()
            {
                return new BadDreams(
                    hpLossPercent: hpLossPercent, displayText: displayText,
                    affectedStatuses: affectedStatuses
                    );
            }
        }

        /// <summary>
        /// If holding no item, the user retrieves its trainer's last failed thrown Poké Ball.
        /// </summary>
        public class BallFetch : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the user retrieves a Poké Ball.
            /// </summary>
            public string displayText;

            public BallFetch(
                string displayText = "ability-ballfetch"
                )
                : base(effectType: AbilityEffectType.BallFetch)
            {
                this.displayText = displayText;
            }
            public new BallFetch Clone()
            {
                return new BallFetch(displayText: displayText);
            }
        }

        /// <summary>
        /// Increases the power of ally Pokémon's attacks if they satisfy filters.
        /// </summary>
        public class Battery : AbilityEffect
        {
            /// <summary>
            /// If true, this boost also affects the user.
            /// </summary>
            public bool affectsSelf;

            /// <summary>
            /// The power multiplier applied to moves.
            /// </summary>
            public float powerMultiplier;

            public Battery(
                bool affectsSelf = false,
                float powerMultiplier = 1.3f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Battery, filters: filters)
            {
                this.affectsSelf = affectsSelf;
                this.powerMultiplier = powerMultiplier;
            }
            public new Battery Clone()
            {
                return new Battery(
                    affectsSelf: affectsSelf, 
                    powerMultiplier: powerMultiplier,
                    filters: filters);
            }
        }

        /// <summary>
        /// Prevents this Pokémon from being struck by critical hits.
        /// </summary>
        public class BattleArmor : AbilityEffect
        {
            public BattleArmor() : base(effectType: AbilityEffectType.BattleArmor) { }
            public new BattleArmor Clone()
            {
                return new BattleArmor();
            }
        }

        /// <summary>
        /// Changes the Pokémon's form after defeating an opposing Pokémon.
        /// </summary>
        public class BattleBond : AbilityEffect
        {
            public class BattleBondTransformation
            {
                public string preForm;
                public string toForm;
                
                public BattleBondTransformation(string preForm, string toForm)
                {
                    this.preForm = preForm;
                    this.toForm = toForm;
                }
                public BattleBondTransformation Clone()
                {
                    return new BattleBondTransformation(preForm: preForm, toForm: toForm);
                }
            }

            /// <summary>
            /// Accompanying text before the form change.
            /// </summary>
            public string beforeText;
            /// <summary>
            /// Accompanying text after the form change.
            /// </summary>
            public string afterText;

            /// <summary>
            /// The list of transformations.
            /// </summary>
            public List<BattleBondTransformation> transformations;

            public BattleBond(
                string beforeText = "ability-battlebond", string afterText = "ability-battlebond-form",
                IEnumerable<BattleBondTransformation> transformations = null
                )
                : base(effectType: AbilityEffectType.BattleBond)
            {
                this.beforeText = beforeText;
                this.afterText = afterText;
                this.transformations = new List<BattleBondTransformation>();
                if (transformations != null)
                {
                    List<BattleBondTransformation> preList = new List<BattleBondTransformation>(transformations);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.transformations.Add(preList[i].Clone());
                    }
                }
            }
            public new BattleBond Clone()
            {
                return new BattleBond(
                    beforeText: beforeText, afterText: afterText,
                    transformations: transformations
                    );
            }
        }

        /// <summary>
        /// Modifies the user's highest stat after knocking out an opposing Pokémon.
        /// </summary>
        public class BeastBoost : AbilityEffect
        {
            /// <summary>
            /// The value to modify the highest stat by.
            /// </summary>
            public int statMod;
            public BeastBoost(int statMod = 1) : base(effectType: AbilityEffectType.BeastBoost)
            {
                this.statMod = statMod;
            }
            public new BeastBoost Clone()
            {
                return new BeastBoost(statMod: statMod);
            }
        }

        /// <summary>
        /// Modifies stats if the user's HP falls below a certain threshold by a direct hit.
        /// </summary>
        public class Berserk : AbilityEffect
        {
            /// <summary>
            /// The HP threshold that the user's HP must fall below.
            /// </summary>
            public float hpThreshold;

            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            public Berserk(
                General.StatStageMod statStageMod,
                float hpThreshold = 0.5f)
                : base(effectType: AbilityEffectType.Berserk)
            {
                this.hpThreshold = hpThreshold;
                this.statStageMod = statStageMod.Clone();
            }
            public new Berserk Clone()
            {
                return new Berserk(
                    hpThreshold: hpThreshold,
                    statStageMod: statStageMod);
            }
        }

        /// <summary>
        /// Negates the effect of moves on the user that are of the specific tags.
        /// </summary>
        public class Cacophony : AbilityEffect
        {
            /// <summary>
            /// If a move has a tag contained here, it is blocked.
            /// </summary>
            public HashSet<MoveTag> blockedMoveTags;

            /// <summary>
            /// The text displayed if the move is blocked.
            /// </summary>
            public string displayText;

            public Cacophony(
                IEnumerable<MoveTag> blockedMoves = null,
                string displayText = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Cacophony, filters: filters)
            {
                this.blockedMoveTags = (blockedMoves == null) ? new HashSet<MoveTag>()
                    : new HashSet<MoveTag>(blockedMoves);
                this.displayText = displayText;
            }
            public new Cacophony Clone()
            {
                return new Cacophony(
                    blockedMoves: blockedMoveTags,
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Restores HP upon consumption of items.
        /// </summary>
        public class CheekPouch : AbilityEffect
        {
            /// <summary>
            /// The HP gained as a % of the user's max HP.
            /// </summary>
            public float hpGainPercent;

            /// <summary>
            /// If true, this ability activates on consumed berries.
            /// </summary>
            public bool onBerry;

            public CheekPouch(
                float hpGainPercent = 1f/3,
                bool onBerry = true
                )
                : base(effectType: AbilityEffectType.CheekPouch)
            {
                this.hpGainPercent = hpGainPercent;
                this.onBerry = onBerry;
            }
            public new CheekPouch Clone()
            {
                return new CheekPouch(
                    hpGainPercent: hpGainPercent,
                    onBerry: onBerry
                    );
            }
        }

        /// <summary>
        /// Changes the user's type to the type of the move it is was just hit by.
        /// </summary>
        public class ColorChange : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the user's type is changed;
            /// </summary>
            public string displayText;
            /// <summary>
            /// Type only changes when hit by damaging attacks.
            /// </summary>
            public bool onlyDamaging;

            public ColorChange(
                string displayText = "ability-colorchange",
                bool onlyDamaging = true
                )
                : base(effectType: AbilityEffectType.ColorChange)
            {
                this.displayText = displayText;
                this.onlyDamaging = onlyDamaging;
            }
            public new ColorChange Clone()
            {
                return new ColorChange(
                    displayText: displayText,
                    onlyDamaging: onlyDamaging);
            }
        }

        /// <summary>
        /// Simulates a status condition, without actually having the condition.
        /// </summary>
        public class Comatose : AbilityEffect
        {
            /// <summary>
            /// The status condition being simulated.
            /// </summary>
            public string statusID;

            public Comatose(
                string statusID = "sleep"
                )
                : base(effectType: AbilityEffectType.Comatose)
            {
                this.statusID = statusID;
            }
            public new Comatose Clone()
            {
                return new Comatose(statusID: statusID);
            }
        }

        /// <summary>
        /// Scales the user's stats.
        /// </summary>
        public class CompoundEyes : AbilityEffect
        {
            /// <summary>
            /// The stats to scale.
            /// </summary>
            public General.StatScale statScale;
            /// <summary>
            /// If true, the stat scales apply to all ally Pokémon on the field.
            /// </summary>
            public bool victoryStar;
            /// <summary>
            /// If non-negative, the user's HP must be at or below this percentage to apply the stat scaling.
            /// </summary>
            public float defeatistThreshold;

            public CompoundEyes(
                General.StatScale statScale,
                bool victoryStar = false,
                float defeatistThreshold = -1,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.CompoundEyes, filters: filters)
            {
                this.statScale = statScale.Clone();
                this.victoryStar = victoryStar;
                this.defeatistThreshold = defeatistThreshold;
            }
            public new CompoundEyes Clone()
            {
                return new CompoundEyes(
                    statScale: statScale,
                    victoryStar: victoryStar,
                    defeatistThreshold: defeatistThreshold,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Reverses the user's stat stage changes.
        /// </summary>
        public class Contrary : AbilityEffect
        {
            public Contrary() : base(effectType: AbilityEffectType.Contrary) { }
            public new Contrary Clone()
            {
                return new Contrary();
            }
        }

        /// <summary>
        /// Can inflict status conditions regardless of type-based immunities.
        /// </summary>
        public class Corrosion : AbilityEffect
        {
            /// <summary>
            /// Inflicted statuses that ignore type-based immunities.
            /// </summary>
            public List<string> statuses;

            public Corrosion(
                IEnumerable<string> statuses = null
                )
                : base(effectType: AbilityEffectType.Corrosion)
            {
                this.statuses = (statuses == null) ? new List<string>() : new List<string>(statuses);
            }
            public new Corrosion Clone()
            {
                return new Corrosion(statuses: statuses);
            }
        }

        /// <summary>
        /// Prevents the use of certain moves while the user is in battle.
        /// </summary>
        public class Damp : AbilityEffect
        {
            /// <summary>
            /// Moves with tags contained here are blocked.
            /// </summary>
            public HashSet<MoveTag> moveTags;

            public Damp(IEnumerable<MoveTag> moveTags = null) : base(effectType: AbilityEffectType.Damp)
            {
                this.moveTags = (moveTags == null) ? new HashSet<MoveTag>() : new HashSet<MoveTag>(moveTags);
            }
            public new Damp Clone()
            {
                return new Damp(moveTags: moveTags);
            }
        }

        /// <summary>
        /// Immediately copies the the use of a move if it is tagged with certain <seealso cref="MoveTag"/>s.
        /// </summary>
        public class Dancer : AbilityEffect
        {
            /// <summary>
            /// Moves with tags contained here are copied.
            /// </summary>
            public HashSet<MoveTag> moveTags;

            public Dancer(IEnumerable<MoveTag> moveTags = null) : base(effectType: AbilityEffectType.Dancer)
            {
                this.moveTags = (moveTags == null) ? new HashSet<MoveTag>() : new HashSet<MoveTag>(moveTags);
            }
            public new Dancer Clone()
            {
                return new Dancer(moveTags: moveTags);
            }
        }

        /// <summary>
        /// Modifies the damage dealt by specific-typed moves by all Pokémon on the field while this ability
        /// is active. Does not stack with itself. Effect is reversed if a Pokémon with <seealso cref="AuraBreak"/>
        /// is on the field.
        /// </summary>
        public class DarkAura : AbilityEffect
        {
            /// <summary>
            /// The multiplier to add to affected moves.
            /// </summary>
            public float damageMultiplier;
            /// <summary>
            /// The text displayed when this Pokémon enters battle.
            /// </summary>
            public string displayText;

            public DarkAura(
                float damageMultiplier = 4f/3,
                string displayText = "ability-darkaura",

                IEnumerable<string> moveTypes = null,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.DarkAura, filters: filters)
            {
                this.damageMultiplier = damageMultiplier;
                this.displayText = displayText;

                Filter.TypeList typeList = new Filter.TypeList(
                    targetType: Filter.TypeList.TargetType.Move,
                    types: moveTypes);
                this.filters.Add(typeList);
            }
            public new DarkAura Clone()
            {
                return new DarkAura(
                    damageMultiplier: damageMultiplier, displayText: displayText,
                    filters: filters);
            }
        }

        /// <summary>
        /// Triggers stat stage changes when the user's stats are lowered.
        /// </summary>
        public class Defiant : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            /// <summary>
            /// Defiant activated if the specified stats were lowered. 
            /// </summary>
            public HashSet<PokemonStats> lowerTriggers;
            /// <summary>
            /// Defiant activated if the specified stats were raised. 
            /// </summary>
            public HashSet<PokemonStats> raiseTriggers;

            /// <summary>
            /// If true, this can only be activated if the stats were changed by an opponent.
            /// </summary>
            public bool onlyOpposing;

            public Defiant(
                General.StatStageMod statStageMod = null,
                IEnumerable<PokemonStats> lowerTriggers = null, IEnumerable<PokemonStats> raiseTriggers = null,
                bool onlyOpposing = true
                )
                : base(effectType: AbilityEffectType.Defiant)
            {
                this.statStageMod = (statStageMod == null) ? null : statStageMod.Clone();
                this.lowerTriggers = (lowerTriggers == null) ? new HashSet<PokemonStats>()
                    : new HashSet<PokemonStats>(lowerTriggers);
                this.raiseTriggers = (raiseTriggers == null) ? new HashSet<PokemonStats>()
                    : new HashSet<PokemonStats>(raiseTriggers);
                this.onlyOpposing = onlyOpposing;
            }
            public new Defiant Clone()
            {
                return new Defiant(
                    statStageMod: statStageMod,
                    lowerTriggers: lowerTriggers, raiseTriggers: raiseTriggers,
                    onlyOpposing: onlyOpposing
                    );
            }
        }

        /// <summary>
        /// The user is immune to direct hits for a turn, and the disguise breaks afterward for the rest of battle,
        /// changing the Pokémon's form.
        /// </summary>
        public class Disguise : AbilityEffect
        {

            /// <summary>
            /// The % of HP the user loses when its disguise is broken.
            /// </summary>
            public float hpLossPercent;
            /// <summary>
            /// The disguise forms that this ability applies to.
            /// </summary>
            public List<General.FormTransformation> disguiseForms;

            /// <summary>
            /// The text displayed when the Pokémon changes form.
            /// </summary>
            public string displayText;

            public Disguise(
                IEnumerable<General.FormTransformation> disguiseForms = null,
                float hpLossPercent = 1f/8, string displayText = "ability-disguise",

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Disguise, filters: filters)
            {
                this.disguiseForms = new List<General.FormTransformation>();
                if (disguiseForms != null)
                {
                    List<General.FormTransformation> preList = new List<General.FormTransformation>(disguiseForms);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.disguiseForms.Add(preList[i].Clone());
                    }
                }
                this.hpLossPercent = hpLossPercent;
                this.displayText = displayText;
            }
            public new Disguise Clone()
            {
                return new Disguise(
                    disguiseForms: disguiseForms,
                    hpLossPercent: hpLossPercent, displayText: displayText,
                    filters: filters
                    );
            }
            public bool IsPokemonDisguised(Pokemon pokemon)
            {
                return GetDisguiseForm(pokemon) != null;
            }
            public General.FormTransformation GetDisguiseForm(Pokemon pokemon)
            {
                for (int i = 0; i < disguiseForms.Count; i++)
                {
                    if (disguiseForms[i].IsPokemonAPreForm(pokemon))
                    {
                        return disguiseForms[i];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Modifies stats the user's stats when it enters battle depending on the opponent's stats.
        /// </summary>
        public class Download : AbilityEffect
        {
            public class DownloadCompare
            {
                public General.StatStageMod statStageMod1, statStageMod2;
                public PokemonStats stats1, stats2;

                public DownloadCompare(
                    General.StatStageMod statStageMod1,
                    General.StatStageMod statStageMod2,
                    PokemonStats stats1 = PokemonStats.Defense, PokemonStats stats2 = PokemonStats.SpecialDefense
                    )
                {
                    this.statStageMod1 = statStageMod1.Clone();
                    this.statStageMod2 = statStageMod2.Clone();
                    this.stats1 = stats1;
                    this.stats2 = stats2;
                }
                public DownloadCompare Clone()
                {
                    return new DownloadCompare(
                        statStageMod1: statStageMod1, statStageMod2: statStageMod2,
                        stats1: stats1, stats2: stats2
                        );
                }
            }

            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public List<DownloadCompare> downloadComparisons;

            public Download(
                IEnumerable<DownloadCompare> downloadComparisons = null,
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(effectType: AbilityEffectType.Download, filters: filters)
            {
                this.downloadComparisons = new List<DownloadCompare>();
                if (downloadComparisons != null)
                {
                    List<DownloadCompare> preList = new List<DownloadCompare>(downloadComparisons);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.downloadComparisons.Add(preList[i].Clone());
                    }
                }
            }
            public new Download Clone()
            {
                return new Download(
                    downloadComparisons: downloadComparisons,
                    filters: filters);
            }
        }

        /// <summary>
        /// Starts a battle condition when the user enters battle.
        /// </summary>
        public class Drought : AbilityEffect
        {
            /// <summary>
            /// The condition inflicted.
            /// </summary>
            public General.InflictStatus inflictStatus;
            /// <summary>
            /// This condition disappears once there are no other Drought users in-battle that cast the same
            /// condition.
            /// </summary>
            public bool desolateLand;

            public Drought(
                General.InflictStatus inflictStatus,
                bool desolateLand = false
                )
                : base(effectType: AbilityEffectType.Drought)
            {
                this.inflictStatus = inflictStatus.Clone();
                this.desolateLand = desolateLand;
            }
            public new Drought Clone()
            {
                return new Drought(
                    inflictStatus: inflictStatus,
                    desolateLand: desolateLand
                    );
            }
        }

        /// <summary>
        /// The user loses or recovers HP every turn if the specified battle conditions are active.
        /// </summary>
        public class DrySkin : AbilityEffect
        {
            public class DrySkinCondition
            {
                public List<string> conditions;
                public float hpGainPercent;
                public float hpLosePercent;

                public DrySkinCondition(
                    IEnumerable<string> conditions,
                    float hpGainPercent = 0f, float hpLosePercent = 0f
                    )
                {
                    this.conditions = new List<string>(conditions);
                    this.hpGainPercent = hpGainPercent;
                    this.hpLosePercent = hpLosePercent;
                }
                public DrySkinCondition Clone()
                {
                    return new DrySkinCondition(
                        conditions: conditions,
                        hpGainPercent: hpGainPercent, hpLosePercent: hpLosePercent
                        );
                }
            }

            /// <summary>
            /// The specified dry skin conditions.
            /// </summary>
            public List<DrySkinCondition> conditions;

            public DrySkin(
                IEnumerable<DrySkinCondition> conditions = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.DrySkin, filters: filters)
            {
                this.conditions = new List<DrySkinCondition>();
                if (conditions != null)
                {
                    List<DrySkinCondition> preList = new List<DrySkinCondition>(conditions);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.conditions.Add(preList[i].Clone());
                    }
                }
            }
            public new DrySkin Clone()
            {
                return new DrySkin(
                    conditions: conditions,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Modifies the turns applied for status conditions
        /// </summary>
        public class EarlyBird : AbilityEffect
        {
            /// <summary>
            /// The status conditions cut.
            /// </summary>
            public List<string> conditions;
            /// <summary>
            /// The amount to modify the turns the status is inflicted for.
            /// </summary>
            public float turnModifier;

            public EarlyBird(
                IEnumerable<string> conditions = null,
                float turnModifier = 0.5f
                )
                : base(effectType: AbilityEffectType.EarlyBird)
            {
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
                this.turnModifier = turnModifier;
            }
            public new EarlyBird Clone()
            {
                return new EarlyBird(
                    conditions: conditions,
                    turnModifier: turnModifier
                    );
            }
        }

        /// <summary>
        /// Inflicts the specified status upon the user being attacked.
        /// </summary>
        public class FlameBody : AbilityEffect
        {
            public class EffectSporeCondition
            {
                public General.InflictStatus inflictStatus;
                public float chance;

                public EffectSporeCondition(
                    General.InflictStatus inflictStatus,
                    float chance = 1f / 3
                    )
                {
                    this.inflictStatus = inflictStatus.Clone();
                    this.chance = chance;
                }
                public EffectSporeCondition Clone()
                {
                    return new EffectSporeCondition(
                        inflictStatus: inflictStatus,
                        chance: chance
                        );
                }
            }
            public List<EffectSporeCondition> effectSpores;

            public General.InflictStatus inflictStatus;

            /// <summary>
            /// If true, the user also applies the status to itself. 
            /// </summary>
            public bool perishBody;
            /// <summary>
            /// If true, this effect only occurs on damaging moves.
            /// </summary>
            public bool onlyDamaging;
            /// <summary>
            /// If set to non-empty, this effect only occurs if the move has one of the specified tags.
            /// </summary>
            public HashSet<MoveTag> triggerTags;

            public FlameBody(
                General.InflictStatus inflictStatus = null,
                IEnumerable<EffectSporeCondition> effectSpores = null,
                bool perishBody = false,
                bool onlyDamaging = false,
                IEnumerable<MoveTag> triggerTags = null,

                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1
                )
                : base(effectType: AbilityEffectType.FlameBody,
                      filters: filters, chance: chance
                      )
            {
                this.inflictStatus = (inflictStatus == null) ? null : inflictStatus.Clone();
                this.effectSpores = new List<EffectSporeCondition>();
                if (effectSpores != null)
                {
                    List<EffectSporeCondition> preList = new List<EffectSporeCondition>(effectSpores);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.effectSpores.Add(preList[i].Clone());
                    }
                }

                this.perishBody = perishBody;
                this.onlyDamaging = onlyDamaging;
                this.triggerTags = (triggerTags == null) ? new HashSet<MoveTag>()
                    : new HashSet<MoveTag>(triggerTags);
            }
            public new FlameBody Clone()
            {
                return new FlameBody(
                    inflictStatus: inflictStatus,
                    effectSpores: effectSpores,
                    perishBody: perishBody,
                    onlyDamaging: onlyDamaging, triggerTags: triggerTags,

                    filters: filters, chance: chance
                    );
            }
            public General.InflictStatus GetAnEffectSporeStatus()
            {
                General.InflictStatus status = (effectSpores.Count == 0)? null : effectSpores[0].inflictStatus;

                List<float> levelChances = new List<float>();
                float totalChance = 0;
                for (int i = 0; i < effectSpores.Count; i++)
                {
                    totalChance += effectSpores[i].chance;
                    levelChances.Add(totalChance);
                }

                // Normalize level chances
                if (totalChance > 0)
                {
                    for (int i = 0; i < levelChances.Count; i++)
                    {
                        levelChances[i] /= totalChance;
                    }

                    // Calculate level by chance
                    float randValue = Random.value;
                    for (int i = 0; i < levelChances.Count; i++)
                    {
                        if (randValue <= levelChances[i])
                        {
                            return effectSpores[i].inflictStatus;
                        }
                        else if (i == levelChances.Count - 1)
                        {
                            return effectSpores[i].inflictStatus;
                        }
                    }
                }

                return status;
            }
        }

        /// <summary>
        /// Scales the user's move's power if it has the specified status conditions.
        /// </summary>
        public class FlareBoost : AbilityEffect
        {
            /// <summary>
            /// The boost to damage dealt.
            /// </summary>
            public float powerMultiplier;

            /// <summary>
            /// The status conditions that trigger this effect.
            /// </summary>
            public Filter.Harvest conditionCheck;

            public FlareBoost(
                Filter.Harvest conditionCheck,
                float powerMultiplier = 1.5f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.FlareBoost, filters: filters)
            {
                this.conditionCheck = conditionCheck.Clone();
                this.powerMultiplier = powerMultiplier;
            }
            public new FlareBoost Clone()
            {
                return new FlareBoost(
                    powerMultiplier: powerMultiplier,
                    conditionCheck: conditionCheck,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Changes the Pokémon's form depending on the weather.
        /// </summary>
        public class Forecast : AbilityEffect
        {
            public class ForecastTransformation
            {
                public List<string> conditions;
                public General.FormTransformation transformation;

                public ForecastTransformation(
                    General.FormTransformation transformation,
                    IEnumerable<string> conditions = null)
                {
                    this.transformation = transformation.Clone();
                    this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
                }
                public ForecastTransformation Clone()
                {
                    return new ForecastTransformation(
                        transformation: transformation,
                        conditions: conditions
                        );
                }
            }
            
            /// <summary>
            /// Accompanying text after the form change.
            /// </summary>
            public string displayText;

            /// <summary>
            /// The list of transformations.
            /// </summary>
            public List<ForecastTransformation> transformations;
            /// <summary>
            /// If non-null, the Pokémon reverts to this form if no <seealso cref="ForecastTransformation"/>
            /// was satisfied.
            /// </summary>
            public General.FormTransformation defaultTransformation;

            public Forecast(
                string displayText = null,
                IEnumerable<ForecastTransformation> transformations = null,
                General.FormTransformation defaultTransformation = null
                )
                : base(effectType: AbilityEffectType.Forecast)
            {
                this.displayText = displayText;
                this.transformations = new List<ForecastTransformation>();
                if (transformations != null)
                {
                    List<ForecastTransformation> preList = new List<ForecastTransformation>(transformations);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.transformations.Add(preList[i].Clone());
                    }
                }
                this.defaultTransformation = (defaultTransformation == null) ? null : defaultTransformation.Clone();
            }
            public new Forecast Clone()
            {
                return new Forecast(
                    displayText: displayText,
                    transformations: transformations, defaultTransformation: defaultTransformation
                    );
            }
        }

        /// <summary>
        /// Indicates the opposing Pokémon's highest-powered moves.
        /// </summary>
        public class Forewarn : AbilityEffect
        {
            /// <summary>
            /// Text displayed when Forewarn triggers.
            /// </summary>
            public string displayText;

            public Forewarn(
                string displayText = "ability-forewarn"
                )
                : base(effectType: AbilityEffectType.Forewarn)
            {
                this.displayText = displayText;
            }
            public new Forewarn Clone()
            {
                return new Forewarn(displayText: displayText);
            }
        }

        /// <summary>
        /// Scales damage dealt to allies.
        /// </summary>
        public class FriendGuard : AbilityEffect
        {
            /// <summary>
            /// Damage scaled for allies.
            /// </summary>
            public float damageModifier;

            public FriendGuard(
                float damageModifier = 0.75f,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.FriendGuard, filters: filters)
            {
                this.damageModifier = damageModifier;
            }
            public new FriendGuard Clone()
            {
                return new FriendGuard(
                    damageModifier: damageModifier,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Indicates the opposing Pokémon's highest-powered moves.
        /// </summary>
        public class Frisk : AbilityEffect
        {
            /// <summary>
            /// Text displayed when Forewarn triggers.
            /// </summary>
            public string displayText;

            public Frisk(
                string displayText = "ability-frisk"
                )
                : base(effectType: AbilityEffectType.Frisk)
            {
                this.displayText = displayText;
            }
            public new Frisk Clone()
            {
                return new Frisk(displayText: displayText);
            }
        }

        /// <summary>
        /// Modifies this move's priority depending on the move's type. 
        /// </summary>
        public class GaleWings : AbilityEffect
        {
            public enum PriorityMode
            {
                Add,
                Set,
            }
            /// <summary>
            /// The mode by which priority is determined.
            /// </summary>
            public PriorityMode mode;
            /// <summary>
            /// Is added onto existing priority with <seealso cref="PriorityMode.Add"/>, and is hard set
            /// with <seealso cref="PriorityMode.Set"/>.
            /// </summary>
            public int priority;

            public GaleWings(
                PriorityMode mode = PriorityMode.Add, int priority = 1,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.GaleWings, filters: filters)
            {
                this.mode = mode;
                this.priority = priority;
            }
            public new GaleWings Clone()
            {
                return new GaleWings(
                    mode: mode, 
                    priority: priority, 
                    filters: filters);
            }
        }

        /// <summary>
        /// Scales held berries' HP-threshold value.
        /// </summary>
        public class Gluttony : AbilityEffect
        {
            /// <summary>
            /// The scale applied to HP-thresholds.
            /// </summary>
            public float thresholdScale;
            /// <summary>
            /// The minimum item HP-threshold this effect applies to.
            /// </summary>
            public float minItemHPThreshold;
            /// <summary>
            /// The maximum item HP-threshold this effect applies to.
            /// </summary>
            public float maxItemHPThreshold;

            public Gluttony(
                float thresholdScale = 2f,
                float minItemHPThreshold = 0.25f,
                float maxItemHPThreshold = 0.25f
                )
                : base(effectType: AbilityEffectType.Gluttony)
            {
                this.thresholdScale = thresholdScale;
                this.minItemHPThreshold = minItemHPThreshold;
                this.maxItemHPThreshold = maxItemHPThreshold;
            }
            public new Gluttony Clone()
            {
                return new Gluttony(
                    thresholdScale: thresholdScale,
                    minItemHPThreshold: minItemHPThreshold, maxItemHPThreshold: maxItemHPThreshold
                    );
            }
        }

        /// <summary>
        /// Modifies the attacker's stat stages when the user is struck by an attack.
        /// </summary>
        public class Gooey : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;
            /// <summary>
            /// If true, the stat stages are applied to all other Pokémon on the field.
            /// </summary>
            public bool cottonDown;

            /// <summary>
            /// If true, this effect only occurs on damaging moves.
            /// </summary>
            public bool onlyDamaging;
            /// <summary>
            /// If set to non-empty, this effect only occurs if the move has one of the specified tags.
            /// </summary>
            public HashSet<MoveTag> triggerTags;

            public Gooey(
                General.StatStageMod statStageMod,
                bool cottonDown = false,
                bool onlyDamaging = false,
                IEnumerable<MoveTag> triggerTags = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Gooey, filters: filters)
            {
                this.statStageMod = statStageMod.Clone();
                this.cottonDown = cottonDown;
                this.onlyDamaging = onlyDamaging;
                this.triggerTags = (triggerTags == null) ? new HashSet<MoveTag>() : new HashSet<MoveTag>(triggerTags);
            }
            public new Gooey Clone()
            {
                return new Gooey(
                    statStageMod: statStageMod,
                    cottonDown: cottonDown, onlyDamaging: onlyDamaging,
                    triggerTags: triggerTags,

                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Locks the user into its first-selected move as long the user remains in battle.
        /// </summary>
        public class GorillaTactics : AbilityEffect
        {
            public GorillaTactics(
                )
                : base(effectType: AbilityEffectType.GorillaTactics)
            {

            }
            public new GorillaTactics Clone()
            {
                return new GorillaTactics();
            }
        }

        /// <summary>
        /// Changes the Pokémon's form depending on the weather.
        /// </summary>
        public class GulpMissile : AbilityEffect
        {
            public class Missile
            {
                public float hpThreshold;
                public float hpLossPercent;
                public string displayText;
                public General.InflictStatus inflictStatus;
                public General.StatStageMod statStageMod;

                public Missile(
                    float hpThreshold = 0.5f,
                    float hpLossPercent = 0.25f,
                    string displayText = "ability-gulpmissile",
                    General.InflictStatus inflictStatus = null, General.StatStageMod statStageMod = null
                    )
                {
                    this.hpThreshold = hpThreshold;
                    this.hpLossPercent = hpLossPercent;
                    this.displayText = displayText;
                    this.inflictStatus = (inflictStatus == null) ? null : inflictStatus.Clone();
                    this.statStageMod = (statStageMod == null) ? null : statStageMod.Clone();
                }
                public Missile Clone()
                {
                    return new Missile(
                        hpThreshold: hpThreshold,
                        hpLossPercent: hpLossPercent,
                        displayText: displayText,
                        inflictStatus: inflictStatus, statStageMod: statStageMod
                        );
                }
            }
            public class GulpTransformation
            {
                public float hpThreshold;
                public List<string> moves;
                public General.FormTransformation transformation;
                public List<Missile> missiles;

                public GulpTransformation(
                    General.FormTransformation transformation,
                    IEnumerable<Missile> missiles,
                    IEnumerable<string> moves = null)
                {
                    this.transformation = transformation.Clone();
                    this.missiles = new List<Missile>();
                    if (missiles != null)
                    {
                        List<Missile> preList = new List<Missile>(missiles);
                        for (int i = 0; i < preList.Count; i++)
                        {
                            this.missiles.Add(preList[i].Clone());
                        }
                    }
                    this.moves = (moves == null) ? new List<string>() : new List<string>(moves);
                }
                public GulpTransformation Clone()
                {
                    return new GulpTransformation(
                        transformation: transformation,
                        missiles: missiles,
                        moves: moves
                        );
                }
            }

            /// <summary>
            /// Accompanying text after the "gulping" form change.
            /// </summary>
            public string gulpText;
            /// <summary>
            /// Accompanying text after "spit up" form change.
            /// </summary>
            public string spitUpText;

            /// <summary>
            /// The list of transformations done when using "gulping" moves.
            /// </summary>
            public List<GulpTransformation> gulpTransformations;
            /// <summary>
            /// The list of transformations done when spitting up Gulp Missiles.
            /// </summary>
            public List<General.FormTransformation> spitUpTransformations;

            public GulpMissile(
                string gulpText = "pokemon-changeform",
                string spitUpText = "pokemon-changeform",
                IEnumerable<GulpTransformation> gulpTransformations = null,
                IEnumerable<General.FormTransformation> spitUpTransformations = null
                )
                : base(effectType: AbilityEffectType.GulpMissile)
            {
                this.gulpText = gulpText;
                this.spitUpText = spitUpText;

                this.gulpTransformations = new List<GulpTransformation>();
                if (gulpTransformations != null)
                {
                    List<GulpTransformation> preList = new List<GulpTransformation>(gulpTransformations);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.gulpTransformations.Add(preList[i].Clone());
                    }
                }
                this.spitUpTransformations = new List<General.FormTransformation>();
                if (spitUpTransformations != null)
                {
                    List<General.FormTransformation> preList = new List<General.FormTransformation>(spitUpTransformations);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.spitUpTransformations.Add(preList[i].Clone());
                    }
                }
            }
            public new GulpMissile Clone()
            {
                return new GulpMissile(
                    gulpText: gulpText, spitUpText: spitUpText,
                    gulpTransformations: gulpTransformations, 
                    spitUpTransformations: spitUpTransformations
                    );
            }
        }

        /// <summary>
        /// Scales the user's stats if they have the specified status conditions.
        /// </summary>
        public class Guts : AbilityEffect
        {
            /// <summary>
            /// The stat scaling to be applied.
            /// </summary>
            public General.StatScale statScale;

            /// <summary>
            /// The status conditions that trigger this effect.
            /// </summary>
            public Filter.Harvest conditionCheck;

            public Guts(
                General.StatScale statScale,
                Filter.Harvest conditionCheck,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Guts, filters: filters)
            {
                this.statScale = statScale.Clone();
                this.conditionCheck = conditionCheck.Clone();
            }
            public new Guts Clone()
            {
                return new Guts(
                    statScale: statScale,
                    conditionCheck: conditionCheck,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Has a chance to recover consumed items at the end of the turn.
        /// </summary>
        public class Harvest : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the item is recovered.
            /// </summary>
            public string displayText;

            /// <summary>
            /// If the consumed item's pocket is listed here, it is eligible to be recovered.
            /// </summary>
            public HashSet<ItemPocket> pockets;

            public Harvest(
                string displayText = "ability-harvest",
                IEnumerable<ItemPocket> pockets = null,
                float chance = 0.5f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Harvest, chance: chance, filters: filters)
            {
                this.displayText = displayText;
                this.pockets = (pockets == null) ? new HashSet<ItemPocket>()
                    : new HashSet<ItemPocket>(pockets);
            }
            public new Harvest Clone()
            {
                return new Harvest(
                    displayText: displayText,
                    pockets: pockets,
                    chance: chance, filters: filters
                    );
            }
        }

        /// <summary>
        /// Has a chance to heal teammates of their status conditions.
        /// </summary>
        public class Healer : AbilityEffect
        {
            /// <summary>
            /// If true, there is one chance check. If satisfied, all teammates status conditions' are healed.
            /// </summary>
            public bool oneTimeAll;

            /// <summary>
            /// If the teammate has a status condition listed here, it can be healed.
            /// </summary>
            public List<string> conditions;
            /// <summary>
            /// If the teammate's status condition has an effect listed here, it can be healed.
            /// </summary>
            public HashSet<PokemonSEType> statusTypes;

            public Healer(
                bool oneTimeAll = false,
                IEnumerable<string> conditions = null,
                IEnumerable<PokemonSEType> statusTypes = null,

                float chance = 0.3f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Healer, chance: chance, filters: filters)
            {
                this.oneTimeAll = oneTimeAll;
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
                this.statusTypes = (statusTypes == null) ? new HashSet<PokemonSEType>() 
                    : new HashSet<PokemonSEType>(statusTypes);
            }
            public new Healer Clone()
            {
                return new Healer(
                    oneTimeAll: oneTimeAll,
                    conditions: conditions,
                    statusTypes: statusTypes,
                    chance: chance, filters: filters
                    );
            }
        }

        /// <summary>
        /// Factors a multiplier for the user's weight.
        /// </summary>
        public class HeavyMetal : AbilityEffect
        {
            /// <summary>
            /// The multiplier applied to the user's weight.
            /// </summary>
            public float weightMultiplier;

            public HeavyMetal(
                float weightMultiplier = 2f
                )
                : base(effectType: AbilityEffectType.HeavyMetal)
            {
                this.weightMultiplier = weightMultiplier;
            }
            public new HeavyMetal Clone()
            {
                return new HeavyMetal(weightMultiplier: weightMultiplier);
            }
        }

        /// <summary>
        /// May pick up an item after battle.
        /// </summary>
        public class HoneyGather : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the user picks up an item.
            /// </summary>
            public string displayText;

            public HoneyGather(
                string displayText = "ability-honeygather",
                float baseChance = 0.05f 
                )
                : base(effectType: AbilityEffectType.HoneyGather)
            {
                this.displayText = displayText;
            }
            public new HoneyGather Clone()
            {
                return new HoneyGather(displayText: displayText);
            }
        }

        /// <summary>
        /// Changes the Pokémon's form at the end of the turn.
        /// </summary>
        public class HungerSwitch : AbilityEffect
        {
            public enum ChangeMode
            {
                /// <summary>
                /// If the Pokémon is of the form <seealso cref="pokemonID1"/>, it will switch into 
                /// <seealso cref="pokemonID2"/> and vice versa.
                /// </summary>
                Alternating,
                /// <summary>
                /// The Pokémon will only switch from <seealso cref="pokemonID1"/> into
                /// <seealso cref="pokemonID2"/>.
                /// </summary>
                Consecutive
            }
            
            /// <summary>
            /// The method by switch to switch form.
            /// </summary>
            public ChangeMode mode;

            /// <summary>
            /// First Pokémon ID.
            /// </summary>
            public string pokemonID1;
            /// <summary>
            /// Second Pokémon ID.
            /// </summary>
            public string pokemonID2;
            /// <summary>
            /// Accompanying text for the form change.
            /// </summary>
            public string displayText;
            
            public HungerSwitch(
                string pokemonID1, string pokemonID2, string displayText = null,
                ChangeMode mode = ChangeMode.Alternating
                ) : base(effectType: AbilityEffectType.HungerSwitch)
            {
                this.mode = mode;
                this.pokemonID1 = pokemonID1;
                this.pokemonID2 = pokemonID2;
                this.displayText = displayText;
            }
            public new HungerSwitch Clone()
            {
                return new HungerSwitch(
                    pokemonID1: pokemonID1, pokemonID2: pokemonID2, displayText: displayText,
                    mode: mode
                    );
            }

        }

        /// <summary>
        /// Scales accuracy and evasion while using certain moves.
        /// </summary>
        public class Hustle : AbilityEffect
        {
            /// <summary>
            /// The stats to scale.
            /// </summary>
            public General.StatScale statScale;
            /// <summary>
            /// If true, the stat scales apply to all ally Pokémon on the field.
            /// </summary>
            public bool victoryStar;
            /// <summary>
            /// If non-negative, the user's HP must be at or below this percentage to apply the stat scaling.
            /// </summary>
            public float defeatistThreshold;

            /// <summary>
            /// If the move's category is contained here, the stat scales apply.
            /// </summary>
            public HashSet<MoveCategory> moveCategories;

            public Hustle(
                General.StatScale statScale,
                bool victoryStar = false,
                float defeatistThreshold = -1,

                IEnumerable<MoveCategory> moveCategories = null
                )
                : base(effectType: AbilityEffectType.Hustle)
            {
                this.statScale = statScale.Clone();
                this.victoryStar = victoryStar;
                this.defeatistThreshold = defeatistThreshold;
                this.moveCategories = (moveCategories == null) ? new HashSet<MoveCategory>()
                    : new HashSet<MoveCategory>(moveCategories);
            }
            public new Hustle Clone()
            {
                return new Hustle(
                    statScale: statScale,
                    victoryStar: victoryStar,
                    defeatistThreshold: defeatistThreshold,
                    moveCategories: moveCategories
                    );
            }
        }

        /// <summary>
        /// Has a chance to heal the user or its teammates of their status conditions.
        /// </summary>
        public class Hydration : AbilityEffect
        {
            /// <summary>
            /// If true, there is one chance check. If satisfied, all teammates status conditions' are healed.
            /// </summary>
            public bool oneTimeAll;
            /// <summary>
            /// If true, the user can heal itself.
            /// </summary>
            public bool healSelf;
            /// <summary>
            /// If true, the user can heal its allies.
            /// </summary>
            public bool healer;

            /// <summary>
            /// If the teammate has a status condition listed here, it can be healed.
            /// </summary>
            public List<string> conditions;
            /// <summary>
            /// If the teammate's status condition has an effect listed here, it can be healed.
            /// </summary>
            public HashSet<PokemonSEType> statusTypes;

            public Hydration(
                bool oneTimeAll = false,
                bool healSelf = true, bool healer = false,
                IEnumerable<string> conditions = null,
                IEnumerable<PokemonSEType> statusTypes = null,

                float chance = 0.3f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Hydration, chance: chance, filters: filters)
            {
                this.oneTimeAll = oneTimeAll;
                this.healSelf = healSelf;
                this.healer = healer;
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
                this.statusTypes = (statusTypes == null) ? new HashSet<PokemonSEType>()
                    : new HashSet<PokemonSEType>(statusTypes);
            }
            public new Hydration Clone()
            {
                return new Hydration(
                    oneTimeAll: oneTimeAll,
                    healSelf: healSelf, healer: healer,
                    conditions: conditions,
                    statusTypes: statusTypes,
                    chance: chance, filters: filters
                    );
            }
        }

        /// <summary>
        /// Prevents the lowering of the specified Pokémon's stats.
        /// </summary>
        public class HyperCutter : AbilityEffect
        {
            /// <summary>
            /// The stats protected by this ability.
            /// </summary>
            public HashSet<PokemonStats> affectedStats;
            /// <summary>
            /// If true, all stats are protected.
            /// </summary>
            public bool clearBody;

            /// <summary>
            /// The text displayed when the stat changes are blocked.
            /// </summary>
            public string displayText;

            /// <summary>
            /// If true, the specified stats cannot be lowered.
            /// </summary>
            public bool preventLower;
            /// <summary>
            /// If true, the specified stats cannot be raised.
            /// </summary>
            public bool preventRaise;
            /// <summary>
            /// If true, the the user cannot lower its own stats.
            /// </summary>
            public bool affectSelf;

            public HyperCutter(
                IEnumerable<PokemonStats> affectedStats = null,
                bool clearBody = false,
                string displayText = "ability-hypercutter",
                bool preventLower = true, bool preventRaise = false, bool affectSelf = false
                )
                : base(effectType: AbilityEffectType.HyperCutter)
            {
                this.affectedStats = (affectedStats != null) ? new HashSet<PokemonStats>(affectedStats)
                    : new HashSet<PokemonStats>();
                this.clearBody = clearBody;
                this.displayText = displayText;
                this.preventLower = preventLower;
                this.preventRaise = preventRaise;
                this.affectSelf = affectSelf;
            }
            public new HyperCutter Clone()
            {
                return new HyperCutter(
                    affectedStats: affectedStats,
                    clearBody: clearBody,
                    displayText: displayText,
                    preventLower: preventLower, preventRaise: preventRaise, affectSelf: affectSelf
                    );
            }
        }

        /// <summary>
        /// Scales the damage taken from certain move types.
        /// </summary>
        public class IceScales : AbilityEffect
        {
            /// <summary>
            /// The amount to scale taken damage by.
            /// </summary>
            public float damageModifier;

            /// <summary>
            /// If true, the move category check will occur.
            /// </summary>
            public bool useCategory;
            /// <summary>
            /// If <seealso cref="useCategory"/> is set to true, the move must match this category.
            /// </summary>
            public MoveCategory category;

            /// <summary>
            /// If non-empty, the move must have a tag contained here.
            /// </summary>
            public HashSet<MoveTag> tags;

            public IceScales(
                float damageModifier = 1f,
                bool useCategory = false, MoveCategory category = MoveCategory.Special,
                IEnumerable<MoveTag> tags = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.IceScales, filters: filters)
            {
                this.damageModifier = damageModifier;
                this.useCategory = useCategory;
                this.category = category;
                this.tags = (tags == null) ? new HashSet<MoveTag>() : new HashSet<MoveTag>(tags);
            }
            public new IceScales Clone()
            {
                return new IceScales(
                    damageModifier: damageModifier,
                    useCategory: useCategory, category: category,
                    tags: tags,

                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Changes the appearance of the Pokémon to that of the last conscious, non-Egg Pokémon 
        /// in its trainer's party. Breaks once the user takes direct damage.
        /// </summary>
        public class Illusion : AbilityEffect
        {
            /// <summary>
            /// Text displayed when Illusion breaks.
            /// </summary>
            public string displayText;

            public Illusion(
                string displayText = "ability-illusion"
                )
                : base(effectType: AbilityEffectType.Illusion)
            {
                this.displayText = displayText;
            }
            public new Illusion Clone()
            {
                return new Illusion(displayText: displayText);
            }
        }

        /// <summary>
        /// Ignores substitutes and screens when attacking.
        /// </summary>
        public class Infiltrator : AbilityEffect
        {
            /// <summary>
            /// If true, substitutes are ignored.
            /// </summary>
            public bool bypassSubstitute;
            /// <summary>
            /// If true, screens are ignored.
            /// </summary>
            public bool bypassScreens;

            public Infiltrator(
                bool bypassSubstitute = true,
                bool bypassScreens = true
                )
                : base(effectType: AbilityEffectType.Infiltrator)
            {
                this.bypassSubstitute = bypassSubstitute;
                this.bypassScreens = bypassScreens;
            }
            public new Infiltrator Clone()
            {
                return new Infiltrator(
                    bypassSubstitute: bypassSubstitute,
                    bypassScreens: bypassScreens
                    );
            }
        }

        /// <summary>
        /// Modifies stats the opposing team's stats when it enters battle.
        /// </summary>
        public class Intimidate : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            public Intimidate(
                General.StatStageMod statStageMod,
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(effectType: AbilityEffectType.Intimidate, filters: filters)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new Intimidate Clone()
            {
                return new Intimidate(
                    statStageMod: statStageMod,
                    filters: filters);
            }
        }
        /// <summary>
        /// Blocks the effect of specified <seealso cref="Intimidate"/> abilities.
        /// </summary>
        public class IntimidateBlock : AbilityEffect
        {
            /// <summary>
            /// The <seealso cref="Intimidate"/> abilities blocked.
            /// </summary>
            public HashSet<string> abilitiesBlocked;

            /// <summary>
            /// The text displayed when an ability is blocked.
            /// </summary>
            public string displayText;

            public IntimidateBlock(
                IEnumerable<string> abilitiesBlocked = null,
                string displayText = "pokemon-unaffect"
                )
                : base(effectType: AbilityEffectType.IntimidateBlock)
            {
                this.abilitiesBlocked = (abilitiesBlocked == null) ? new HashSet<string>()
                    : new HashSet<string>(abilitiesBlocked);
                this.displayText = displayText;
            }
            public new IntimidateBlock Clone()
            {
                return new IntimidateBlock(
                    abilitiesBlocked: abilitiesBlocked,
                    displayText: displayText);
            }
        }
        /// <summary>
        /// Modifies stats the opposing team's stats when it enters battle.
        /// </summary>
        public class IntimidateTrigger : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            /// <summary>
            /// The <seealso cref="Intimidate"/> abilities blocked.
            /// </summary>
            public HashSet<string> abilityTriggers;

            public IntimidateTrigger(
                General.StatStageMod statStageMod,
                IEnumerable<string> abilityTriggers = null,
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(effectType: AbilityEffectType.IntimidateTrigger, filters: filters)
            {
                this.statStageMod = statStageMod.Clone();
                this.abilityTriggers = (abilityTriggers == null) ? new HashSet<string>()
                    : new HashSet<string>(abilityTriggers);
            }
            public new IntimidateTrigger Clone()
            {
                return new IntimidateTrigger(
                    statStageMod: statStageMod,
                    abilityTriggers: abilityTriggers,
                    filters: filters);
            }
        }

        /// <summary>
        /// Modifies stats the user's stats when it enters battle.
        /// </summary>
        public class IntrepidSword : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            public IntrepidSword(
                General.StatStageMod statStageMod,
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(effectType: AbilityEffectType.Justified, filters: filters)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new IntrepidSword Clone()
            {
                return new IntrepidSword(
                    statStageMod: statStageMod,
                    filters: filters);
            }
        }

        /// <summary>
        /// Boosts the power of the user's moves that match filter criteria.
        /// </summary>
        public class IronFist : AbilityEffect
        {
            /// <summary>
            /// The power multiplier applied to moves.
            /// </summary>
            public float powerMultiplier;

            /// <summary>
            /// If positive, the user's HP must be under this threshold to activate Iron Fist.
            /// </summary>
            public float blazeThreshold;

            /// <summary>
            /// If true, this effect also applies to teammate's moves as well.
            /// </summary>
            public bool steelySpirit;

            public IronFist(
                float powerMultiplier = 1.2f,
                float blazeThreshold = 0f,
                bool steelySpirit = false,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.IronFist, filters: filters)
            {
                this.powerMultiplier = powerMultiplier;
                this.blazeThreshold = blazeThreshold;
                this.steelySpirit = steelySpirit;
            }
            public new IronFist Clone()
            {
                return new IronFist(
                    powerMultiplier: powerMultiplier,
                    blazeThreshold: blazeThreshold,
                    steelySpirit: steelySpirit,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Modifies stats if the user is hit by the specified attacks.
        /// </summary>
        public class Justified : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            /// <summary>
            /// This effect only triggers on damaging moves.
            /// </summary>
            public bool onlyDamaging;

            /// <summary>
            /// If true, the move must match the specified category.
            /// </summary>
            public bool mustMatchCategory;
            /// <summary>
            /// The category the move must match, if <seealso cref="mustMatchCategory"/> is true.
            /// </summary>
            public MoveCategory category;

            public Justified(
                General.StatStageMod statStageMod,
                bool onlyDamaging = true,
                bool mustMatchCategory = false,
                MoveCategory category = MoveCategory.Physical,
                
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(effectType: AbilityEffectType.Justified, filters: filters)
            {
                this.statStageMod = statStageMod.Clone();
                this.onlyDamaging = onlyDamaging;
                this.mustMatchCategory = mustMatchCategory;
                this.category = category;
            }
            public new Justified Clone()
            {
                return new Justified(
                    statStageMod: statStageMod,
                    onlyDamaging: onlyDamaging,
                    mustMatchCategory: mustMatchCategory,
                    category: category,
                    filters: filters);
            }
        }

        /// <summary>
        /// Forces this Pokémon to become airborne.
        /// </summary>
        public class Levitate : AbilityEffect
        {
            public Levitate(
                )
                : base(effectType: AbilityEffectType.Levitate)
            {

            }
            public new Levitate Clone()
            {
                return new Levitate();
            }
        }

        /// <summary>
        /// Prevents the user from using any held items.
        /// </summary>
        public class Klutz : AbilityEffect
        {
            public Klutz(
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Klutz, filters: filters)
            {

            }
            public new Klutz Clone()
            {
                return new Klutz(
                    filters: filters);
            }
        }

        /// <summary>
        /// Draws the specified moves to this Pokémon.
        /// </summary>
        public class LightningRod : AbilityEffect
        {
            /// <summary>
            /// If true, this ability will also draw ally moves.
            /// </summary>
            public bool affectAlly;

            public LightningRod(
                bool affectAlly = true,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.LightningRod, filters: filters)
            {
                this.affectAlly = affectAlly;
            }
            public new LightningRod Clone()
            {
                return new LightningRod(
                    affectAlly: affectAlly,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Prevents this Pokémon from being afflicted with the given status conditions.
        /// </summary>
        public class Limber : AbilityEffect
        {
            /// <summary>
            /// If the Pokémon has a status condition listed here, it can be blocked.
            /// </summary>
            public List<string> conditions;
            /// <summary>
            /// If the teammate's status condition has an effect listed here, it can be healed.
            /// </summary>
            public HashSet<PokemonSEType> statusTypes;

            /// <summary>
            /// If true, the user can heal itself.
            /// </summary>
            public bool healSelf;
            /// <summary>
            /// If true, the conditions are blocked for the Pokémon's team.
            /// </summary>
            public bool pastelVeil;

            public Limber(
                IEnumerable<string> conditions = null,
                IEnumerable<PokemonSEType> statusTypes = null,
                bool healSelf = true, bool pastelVeil = false,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Limber, filters: filters)
            {
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
                this.statusTypes = (statusTypes == null) ? new HashSet<PokemonSEType>()
                    : new HashSet<PokemonSEType>(statusTypes);
                this.healSelf = healSelf;
                this.pastelVeil = pastelVeil;
            }
            public new Limber Clone()
            {
                return new Limber(
                    conditions: conditions,
                    statusTypes: statusTypes,
                    healSelf: healSelf, pastelVeil: pastelVeil,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Inverts the effects of HP-draining moves against this Pokemon.
        /// </summary>
        public class LiquidOoze : AbilityEffect
        {
            /// <summary>
            /// The percentage of absorbed damage dealt.
            /// </summary>
            public float damagePercent;

            public LiquidOoze(
                float damagePercent = 1f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.LiquidOoze, filters: filters)
            {
                this.damagePercent = damagePercent;
            }
            public new LiquidOoze Clone()
            {
                return new LiquidOoze(
                    damagePercent: damagePercent,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Removes move tags from used moves.
        /// </summary>
        public class LongReach : AbilityEffect
        {
            /// <summary>
            /// The tags that are removed from used moves.
            /// </summary>
            public HashSet<MoveTag> moveTags;

            public LongReach(
                IEnumerable<MoveTag> moveTags = null,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.LongReach, filters: filters)
            {
                this.moveTags = (moveTags == null) ? null : new HashSet<MoveTag>(moveTags);
            }
            public new LongReach Clone()
            {
                return new LongReach(
                    moveTags: moveTags,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Reflects certain moves back to their attackers.
        /// </summary>
        public class MagicBounce : AbilityEffect
        {
            /// <summary>
            /// Defines how moves are reflected.
            /// </summary>
            public General.MagicCoat magicCoat;

            public MagicBounce(
                General.MagicCoat magicCoat,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.MagicBounce, filters: filters)
            {
                this.magicCoat = magicCoat.Clone();
            }
            public new MagicBounce Clone()
            {
                return new MagicBounce(
                    magicCoat: magicCoat,

                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Prevents damage from all sources except direct hits.
        /// </summary>
        public class MagicGuard : AbilityEffect
        {
            public MagicGuard(
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.MagicGuard, filters: filters)
            {

            }
            public new MagicGuard Clone()
            {
                return new MagicGuard(
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// May steal the target's held item when attacking.
        /// </summary>
        public class Magician : AbilityEffect
        {
            /// <summary>
            /// The text displayed when an item is stolen.
            /// </summary>
            public string displayText;

            public Magician(
                string displayText = "ability-magician",
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Magician, filters: filters)
            {
                this.displayText = displayText;
            }
            public new Magician Clone()
            {
                return new Magician(
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Specialized case of <seealso cref="ShadowTag"/> where specific-typed Pokémon are trapped.
        /// </summary>
        public class MagnetPull : ShadowTag
        {
            public MagnetPull(
                IEnumerable<string> types = null,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(filters: filters)
            {
                Filter.TypeList typeList = new Filter.TypeList(types: types);
                this.filters.Add(typeList);
            }
        }

        /// <summary>
        /// Depending on the terrain, Mimicry will change the type of a Pokémon with this Ability.
        /// </summary>
        public class Mimicry : AbilityEffect
        {
            public class MimicryCondition
            {
                public List<string> conditions;
                public List<string> types;
                public string displayText;

                public MimicryCondition(
                    IEnumerable<string> conditions,
                    IEnumerable<string> types,
                    string displayText = "pokemon-changetype"
                    )
                {
                    this.conditions = new List<string>(conditions);
                    this.types = new List<string>(types);
                    this.displayText = displayText;
                }
                public MimicryCondition Clone()
                {
                    return new MimicryCondition(
                        conditions: conditions,
                        types: types,
                        displayText: displayText
                        );
                }
            }
            /// <summary>
            /// The conditions for which Mimicry changes the user's type.
            /// </summary>
            public List<MimicryCondition> conditions;

            /// <summary>
            /// The text displayed when the user reverts back to its original typing.
            /// </summary>
            public string revertText;

            public Mimicry(
                IEnumerable<MimicryCondition> conditions,
                string revertText = "pokemon-changetype",

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Mimicry, filters: filters)
            {
                this.conditions = new List<MimicryCondition>(conditions);
                this.revertText = revertText;
            }
            public new Mimicry Clone()
            {
                return new Mimicry(
                    conditions: conditions,
                    revertText: revertText,

                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Scales the user's stats if it has an ally in battle with a specific ability.
        /// </summary>
        public class Minus : AbilityEffect
        {
            /// <summary>
            /// The stats to scale.
            /// </summary>
            public General.StatScale statScale;

            /// <summary>
            /// The abilities that an ally may have that triggers stat scaling.
            /// </summary>
            public List<string> allyAbilities;

            public Minus(
                General.StatScale statScale,
                IEnumerable<string> allyAbilities,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Minus, filters: filters)
            {
                this.statScale = statScale.Clone();
                this.allyAbilities = new List<string>(allyAbilities);
            }
            public new Minus Clone()
            {
                return new Minus(
                    statScale: statScale,
                    allyAbilities: allyAbilities,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Reflects the stat-lowering effects of moves and abilities back at the user.
        /// </summary>
        public class MirrorArmor : AbilityEffect
        {
            /// <summary>
            /// Defiant activated if the specified stats were lowered. 
            /// </summary>
            public HashSet<PokemonStats> lowerTriggers;
            /// <summary>
            /// Defiant activated if the specified stats were raised. 
            /// </summary>
            public HashSet<PokemonStats> raiseTriggers;

            /// <summary>
            /// If true, this can only be activated if the stats were changed by an opponent.
            /// </summary>
            public bool onlyOpposing;

            public MirrorArmor(
                IEnumerable<PokemonStats> lowerTriggers = null, IEnumerable<PokemonStats> raiseTriggers = null,
                bool onlyOpposing = true
                ) 
                : base(effectType: AbilityEffectType.MirrorArmor)
            {
                this.lowerTriggers = (lowerTriggers == null) ? new HashSet<PokemonStats>()
                    : new HashSet<PokemonStats>(lowerTriggers);
                this.raiseTriggers = (raiseTriggers == null) ? new HashSet<PokemonStats>()
                    : new HashSet<PokemonStats>(raiseTriggers);
                this.onlyOpposing = onlyOpposing;
            }
            public new MirrorArmor Clone()
            {
                return new MirrorArmor(
                    lowerTriggers: lowerTriggers, raiseTriggers: raiseTriggers,
                    onlyOpposing: onlyOpposing
                    );
            }
        }

        /// <summary>
        /// Ignores ignorable abilities when attacking.
        /// </summary>
        public class MoldBreaker : AbilityEffect
        {
            /// <summary>
            /// The text that displays when the Pokémon enters battle.
            /// </summary>
            public string displayText;

            public MoldBreaker(
                string displayText = "ability-moldbreaker"
                )
                : base(effectType: AbilityEffectType.MoldBreaker)
            {
                this.displayText = displayText;
            }
            public new MoldBreaker Clone()
            {
                return new MoldBreaker(displayText: displayText);
            }
        }

        /// <summary>
        /// Randomly modifies 2 of the user's stats at the end of each turn.
        /// </summary>
        public class Moody : AbilityEffect
        {
            public List<General.StatStageMod> statStageMods1;
            public List<General.StatStageMod> statStageMods2;

            public Moody(
                IEnumerable<General.StatStageMod> statStageMods1 = null,
                IEnumerable<General.StatStageMod> statStageMods2 = null)
                : base(effectType: AbilityEffectType.Moody)
            {
                this.statStageMods1 = (statStageMods1 == null) ? null 
                    : new List<General.StatStageMod>(statStageMods1);
                this.statStageMods2 = (statStageMods2 == null) ? null 
                    : new List<General.StatStageMod>(statStageMods2);
            }
            public new Moody Clone()
            {
                return new Moody(
                    statStageMods1: statStageMods1,
                    statStageMods2: statStageMods2
                    );
            }
        }

        /// <summary>
        /// Modifies stats if the user knocks out an opposing Pokémon.
        /// </summary>
        public class Moxie : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            public Moxie(General.StatStageMod statStageMod)
                : base(effectType: AbilityEffectType.Moxie)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new Moxie Clone()
            {
                return new Moxie(statStageMod: statStageMod);
            }
        }

        /// <summary>
        /// Can change the user's form if it is holding an item with the effect <seealso cref="ItemEff.ArceusPlate"/>.
        /// </summary>
        public class Multitype : AbilityEffect
        {
            public Multitype(
                )
                : base(effectType: AbilityEffectType.Multitype)
            {

            }
            public new Multitype Clone()
            {
                return new Multitype();
            }
        }

        /// <summary>
        /// Scales damage against the user if they are at least at the specified HP Threshold.
        /// </summary>
        public class Multiscale : AbilityEffect
        {
            /// <summary>
            /// Scaled damage.
            /// </summary>
            public float damageModifier;
            /// <summary>
            /// The HP % the user needs to be to activate Multiscale.
            /// </summary>
            public float hpThreshold;

            public Multiscale(
                float damageModifier = 0.5f,
                float hpThreshold = 1f,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Multiscale, filters: filters)
            {
                this.damageModifier = damageModifier;
                this.hpThreshold = hpThreshold;
            }
            public new Multiscale Clone()
            {
                return new Multiscale(
                    damageModifier: damageModifier,
                    hpThreshold: hpThreshold,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// If attacked, the attacker's ability changes to this one.
        /// </summary>
        public class Mummy : AbilityEffect
        {
            /// <summary>
            /// If true, the user will switch this ability with the target's ability.
            /// </summary>
            public bool wanderingSpirit;

            /// <summary>
            /// The text displayed when the attacker gains the ability.
            /// </summary>
            public string displayText;

            /// <summary>
            /// If non-empty, then the attacker's abilities are replaced by these listed abilities.
            /// </summary>
            public List<string> setAbilities;

            public Mummy(
                bool wanderingSpirit = false,
                IEnumerable<string> setAbilities = null,
                string displayText = "ability-mummy",

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Mummy, filters: filters)
            {
                this.wanderingSpirit = wanderingSpirit;
                this.setAbilities = (setAbilities == null) ? null : new List<string>(setAbilities);
                this.displayText = displayText;
                
            }
            public new Mummy Clone()
            {
                return new Mummy(
                    wanderingSpirit: wanderingSpirit,
                    setAbilities: setAbilities,
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Heals the user's status conditions upon switch out.
        /// </summary>
        public class NaturalCure : AbilityEffect
        {
            /// <summary>
            /// Defines the status conditions to be healed.
            /// </summary>
            public List<Filter.Harvest> conditions;

            /// <summary>
            /// If non-null, recovers the user's HP upon switch out.
            /// </summary>
            public General.HealHP regenerator;

            public NaturalCure(
                IEnumerable<Filter.Harvest> conditions = null,
                General.HealHP regenerator = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.NaturalCure, filters: filters)
            {
                this.conditions = new List<Filter.Harvest>();
                if (conditions != null)
                {
                    List<Filter.Harvest> tempConditions = new List<Filter.Harvest>(conditions);
                    for (int i = 0; i < tempConditions.Count; i++)
                    {
                        this.conditions.Add(tempConditions[i].Clone());
                    }
                }
                this.regenerator = (regenerator == null) ? null : regenerator.Clone();
            }
            public new NaturalCure Clone()
            {
                return new NaturalCure(
                    conditions: conditions,
                    regenerator: regenerator,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Nullifies all other abilities while this ability is active.
        /// </summary>
        public class NeutralizingGas : AbilityEffect
        {
            /// <summary>
            /// Text displayed when this ability becomes active.
            /// </summary>
            public string displayText;

            public NeutralizingGas(
                string displayText = "ability-neutralizinggas",

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.NeutralizingGas, filters: filters)
            {
                this.displayText = displayText;
            }
            public new NeutralizingGas Clone()
            {
                return new NeutralizingGas(
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Bypasses accuracy checks when attacking or being attacked.
        /// </summary>
        public class NoGuard : AbilityEffect
        {
            /// <summary>
            /// If true, the user bypasses accuracy checks when attacking.
            /// </summary>
            public bool bypassWhenAttacking;
            /// <summary>
            /// If true, attacks targeting the user bypass accuracy checks.
            /// </summary>
            public bool bypassWhenAttacked;

            public NoGuard(
                bool bypassWhenAttacking = true,
                bool bypassWhenAttacked = true,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.NoGuard, filters: filters)
            {
                this.bypassWhenAttacking = bypassWhenAttacking;
                this.bypassWhenAttacked = bypassWhenAttacked;
            }
            public new NoGuard Clone()
            {
                return new NoGuard(
                    bypassWhenAttacking: bypassWhenAttacking,
                    bypassWhenAttacked: bypassWhenAttacked,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// The Pokémon is immune to certain status effect types.
        /// </summary>
        public class Oblivious : AbilityEffect
        {
            /// <summary>
            /// Accompanying text to display when blocking a status.
            /// </summary>
            public string displayText;
            /// <summary>
            /// If true, the effects are blocked for the Pokémon's team.
            /// </summary>
            public bool aromaVeil;
            /// <summary>
            /// Status effects that are blocked.
            /// </summary>
            public HashSet<PokemonSEType> effectsBlocked;

            public Oblivious(
                string displayText = "ability-oblivious",
                bool aromaVeil = false,
                IEnumerable<PokemonSEType> effectsBlocked = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base (effectType: AbilityEffectType.Oblivious, filters: filters)
            {
                this.displayText = displayText;
                this.aromaVeil = aromaVeil;
                this.effectsBlocked = (effectsBlocked == null)
                    ? new HashSet<PokemonSEType> { PokemonSEType.Infatuation }
                    : new HashSet<PokemonSEType>(effectsBlocked);
            }
            public new Oblivious Clone()
            {
                return new Oblivious(
                    displayText: displayText,
                    aromaVeil: aromaVeil,
                    effectsBlocked: effectsBlocked);
            }
        }

        /// <summary>
        /// The Pokémon is immune to passive damage from weather.
        /// </summary>
        public class Overcoat : AbilityEffect
        {
            /// <summary>
            /// If true, all weather damage is blocked.
            /// </summary>
            public bool allWeather;

            /// <summary>
            /// The battle conditions defined here have weather damage blocked.
            /// </summary>
            public List<string> conditions;

            public Overcoat(
                bool allWeather = false,
                IEnumerable<string> conditions = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Overcoat, filters: filters)
            {
                this.allWeather = allWeather;
                this.conditions = (conditions == null) ? new List<string>() : new List<string>(conditions);
            }
            public new Overcoat Clone()
            {
                return new Overcoat(
                    allWeather: allWeather,
                    conditions: conditions,
                    filters: filters);
            }
        }

        /// <summary>
        /// The user's moves have their hit count multiplied.
        /// </summary>
        public class ParentalBond : AbilityEffect
        {
            public class BondedHit
            {
                public float damageModifier;
                public BondedHit(float damageModifier = 0.25f)
                {
                    this.damageModifier = damageModifier;
                }
                public BondedHit Clone()
                {
                    return new BondedHit(damageModifier: damageModifier);
                }
            }
            public List<BondedHit> bondedHits;

            public ParentalBond(
                IEnumerable<BondedHit> bondedHits = null,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.ParentalBond, filters: filters)
            {
                this.bondedHits = new List<BondedHit>();
                if (bondedHits != null)
                {
                    List<BondedHit> preList = new List<BondedHit>();
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.bondedHits.Add(preList[i].Clone());
                    }
                }
            }
            public new ParentalBond Clone()
            {
                return new ParentalBond(
                    bondedHits: bondedHits,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// May steal the attackers's held item when being attacked.
        /// </summary>
        public class Pickpocket : AbilityEffect
        {
            /// <summary>
            /// The text displayed when an item is stolen.
            /// </summary>
            public string displayText;

            public Pickpocket(
                string displayText = "ability-magician",
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Pickpocket, filters: filters)
            {
                this.displayText = displayText;
            }
            public new Pickpocket Clone()
            {
                return new Pickpocket(
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Has a chance to recover used items at the end of the turn.
        /// </summary>
        public class Pickup : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the item is picked up.
            /// </summary>
            public string displayText;

            public Pickup(
                string displayText = "ability-pickup",

                float chance = 1f, 
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Pickup, chance: chance, filters: filters)
            {
                this.displayText = displayText;
            }
            public new Pickup Clone()
            {
                return new Pickup(
                    displayText: displayText,
                    chance: chance, filters: filters
                    );
            }
        }

        /// <summary>
        /// Heals the user's HP instead of losing HP if the user is afflicted by certain status conditions.
        /// </summary>
        public class PoisonHeal : AbilityEffect
        {
            public class HealCondition
            {
                public List<Filter.Harvest> conditions;
                public General.HealHP heal;

                public HealCondition(
                    IEnumerable<Filter.Harvest> conditions = null,
                    General.HealHP heal = null
                    )
                {
                    this.conditions = new List<Filter.Harvest>();
                    if (conditions != null)
                    {
                        List<Filter.Harvest> preList = new List<Filter.Harvest>(conditions);
                        for (int i = 0; i < preList.Count; i++)
                        {
                            this.conditions.Add(preList[i].Clone());
                        }
                    }
                    this.heal = (heal == null) ? null : heal.Clone();
                }
                public HealCondition Clone()
                {
                    return new HealCondition(
                        conditions: conditions,
                        heal: heal
                        );
                }
            }
            public List<HealCondition> conditions;

            public PoisonHeal(
                IEnumerable<HealCondition> conditions = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.PoisonHeal, filters: filters)
            {
                this.conditions = new List<HealCondition>();
                if (conditions != null)
                {
                    List<HealCondition> tempConditions = new List<HealCondition>(conditions);
                    for (int i = 0; i < tempConditions.Count; i++)
                    {
                        this.conditions.Add(tempConditions[i].Clone());
                    }
                }
            }
            public new PoisonHeal Clone()
            {
                return new PoisonHeal(
                    conditions: conditions,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Inflicts status conditions or effects upon attackers.
        /// </summary>
        public class PoisonPoint : AbilityEffect
        {
            /// <summary>
            /// The status that is inflicted upon the target when attacking this Pokémon.
            /// </summary>
            public General.InflictStatus inflictStatus;
            /// <summary>
            /// If true, the status will only be inflicted if the attacker makes contact with this Pokémon.
            /// </summary>
            public bool requiresContact;

            public PoisonPoint(
                General.InflictStatus inflictStatus = null,
                bool requiresContact = true,

                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1)
                : base(effectType: AbilityEffectType.PoisonPoint, filters: filters, chance: chance)
            {
                this.inflictStatus = inflictStatus;
                this.requiresContact = requiresContact;
            }
            public new PoisonPoint Clone()
            {
                return new PoisonPoint(
                    inflictStatus: inflictStatus,
                    requiresContact: requiresContact,
                    chance: chance
                    );
            }
        }

        /// <summary>
        /// Inflicts the specified status upon the target when attacking.
        /// </summary>
        public class PoisonTouch : AbilityEffect
        {
            public class EffectSporeCondition
            {
                public General.InflictStatus inflictStatus;
                public float chance;

                public EffectSporeCondition(
                    General.InflictStatus inflictStatus,
                    float chance = 1f / 3
                    )
                {
                    this.inflictStatus = inflictStatus.Clone();
                    this.chance = chance;
                }
                public EffectSporeCondition Clone()
                {
                    return new EffectSporeCondition(
                        inflictStatus: inflictStatus,
                        chance: chance
                        );
                }
            }
            public List<EffectSporeCondition> effectSpores;

            public General.InflictStatus inflictStatus;

            public PoisonTouch(
                General.InflictStatus inflictStatus = null,
                IEnumerable<EffectSporeCondition> effectSpores = null,

                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1
                )
                : base(effectType: AbilityEffectType.PoisonTouch,
                      filters: filters, chance: chance
                      )
            {
                this.inflictStatus = (inflictStatus == null) ? null : inflictStatus.Clone();
                this.effectSpores = new List<EffectSporeCondition>();
                if (effectSpores != null)
                {
                    List<EffectSporeCondition> preList = new List<EffectSporeCondition>(effectSpores);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.effectSpores.Add(preList[i].Clone());
                    }
                }
            }
            public new PoisonTouch Clone()
            {
                return new PoisonTouch(
                    inflictStatus: inflictStatus,
                    effectSpores: effectSpores,
                    filters: filters, chance: chance
                    );
            }
            public General.InflictStatus GetAnEffectSporeStatus()
            {
                General.InflictStatus status = (effectSpores.Count == 0) ? null : effectSpores[0].inflictStatus;

                List<float> levelChances = new List<float>();
                float totalChance = 0;
                for (int i = 0; i < effectSpores.Count; i++)
                {
                    totalChance += effectSpores[i].chance;
                    levelChances.Add(totalChance);
                }

                // Normalize level chances
                if (totalChance > 0)
                {
                    for (int i = 0; i < levelChances.Count; i++)
                    {
                        levelChances[i] /= totalChance;
                    }

                    // Calculate level by chance
                    float randValue = Random.value;
                    for (int i = 0; i < levelChances.Count; i++)
                    {
                        if (randValue <= levelChances[i])
                        {
                            return effectSpores[i].inflictStatus;
                        }
                        else if (i == levelChances.Count - 1)
                        {
                            return effectSpores[i].inflictStatus;
                        }
                    }
                }

                return status;
            }
        }

        /// <summary>
        /// When an ally faints, the user gains the ally's ability.
        /// </summary>
        public class PowerOfAlchemy : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the user gains its ally's ability.
            /// </summary>
            public string displayText;

            public PowerOfAlchemy(
                string displayText = "ability-powerofalchemy",

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.PowerOfAlchemy, filters: filters)
            {
                this.displayText = displayText;

            }
            public new PowerOfAlchemy Clone()
            {
                return new PowerOfAlchemy(
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Provides an immunity to target Pokémon determined by this effect's filters.
        /// </summary>
        public class Prankster : AbilityEffect
        {
            public Prankster(
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Prankster, filters: filters)
            {

            }
            public new Prankster Clone()
            {
                return new Prankster(filters: filters);
            }
        }

        /// <summary>
        /// Scales PP usage of attackers when attacking this Pokémon.
        /// </summary>
        public class Pressure : AbilityEffect
        {
            public enum DeductionMode
            {
                Scale,
                Flat
            }
            public DeductionMode mode;

            /// <summary>
            /// The PP usage multiplier to apply.
            /// </summary>
            public float ppLoss;
            /// <summary>
            /// The text that displays when the Pokémon exerts Pressure.
            /// </summary>
            public string displayText;

            public Pressure(
                DeductionMode mode = DeductionMode.Flat,
                float ppLoss = 1f,
                string displayText = "ability-pressure",
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Pressure, filters: filters)
            {
                this.mode = mode;
                this.ppLoss = ppLoss;
                this.displayText = displayText;
            }
            public new Pressure Clone()
            {
                return new Pressure(
                    mode: mode,
                    ppLoss: ppLoss,
                    displayText: displayText,
                    filters: filters);
            }
        }

        /// <summary>
        /// Ignore the target-redirecting effects of moves and abilities when attacking.
        /// </summary>
        public class PropellerTail : AbilityEffect
        {
            public PropellerTail()
                : base(effectType: AbilityEffectType.PropellerTail)
            {
            }
            public new PropellerTail Clone()
            {
                return new PropellerTail();
            }
        }

        /// <summary>
        /// Changes the user's type to the type of the move it is about to use.
        /// </summary>
        public class Protean : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the user's type is changed;
            /// </summary>
            public string displayText;

            public Protean(
                string displayText = "ability-protean"
                )
                : base(effectType: AbilityEffectType.Protean)
            {
                this.displayText = displayText;
            }
            public new Protean Clone()
            {
                return new Protean(displayText: displayText);
            }
        }

        /// <summary>
        /// The user is immune to opponent elevated-priority moves.
        /// </summary>
        public class QueenlyMajesty : AbilityEffect
        {
            /// <summary>
            /// If true, this protection affects teammates as well.
            /// </summary>
            public bool affectsTeam;

            public QueenlyMajesty(
                bool affectsTeam = true
                )
                : base(effectType: AbilityEffectType.QueenlyMajesty)
            {
                this.affectsTeam = affectsTeam;
            }
            public new QueenlyMajesty Clone()
            {
                return new QueenlyMajesty(affectsTeam: affectsTeam);
            }
        }

        /// <summary>
        /// The user has a chance of attacking first within its priority bracket.
        /// </summary>
        public class QuickDraw : AbilityEffect
        {
            /// <summary>
            /// The text displayed if the effect is triggered.
            /// </summary>
            public string displayText;

            public QuickDraw(
                string displayText = "ability-quickdraw",
                float chance = 0.3f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.QuickDraw, chance: chance, filters: filters)
            {
                this.displayText = displayText;
            }
            public new QuickDraw Clone()
            {
                return new QuickDraw(
                    displayText: displayText,
                    chance: chance, filters: filters
                    );
            }
        }

        /// <summary>
        /// Doubles the effect of berries when consumed.
        /// </summary>
        public class Ripen : AbilityEffect
        {
            /// <summary>
            /// The factor by which effects are scaled.
            /// </summary>
            public float effectMultiplier;
            /// <summary>
            /// The text that displays when Ripen triggers.
            /// </summary>
            public string displayText;

            public Ripen(
                float effectMultiplier = 2f,
                string displayText = "ability-ripen",
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Ripen, filters: filters)
            {
                this.effectMultiplier = effectMultiplier;
                this.displayText = displayText;
            }
            public new Ripen Clone()
            {
                return new Ripen(
                    effectMultiplier: effectMultiplier,
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Scales move power according to the target's gender compared to the user's own.
        /// </summary>
        public class Rivalry : AbilityEffect
        {
            /// <summary>
            /// The power multiplier applied when the target is of the same gender as the user.
            /// </summary>
            public float sameMultiplier;
            /// <summary>
            /// The power multiplier applied when the target is of the opposite gender to the user.
            /// </summary>
            public float oppositeMultiplier;

            public Rivalry(
                float sameMultiplier = 1.25f, float oppositeMultiplier = 0.75f
                )
                : base(effectType: AbilityEffectType.Rivalry)
            {
                this.sameMultiplier = sameMultiplier;
                this.oppositeMultiplier = oppositeMultiplier;
            }
            public new Rivalry Clone()
            {
                return new Rivalry(
                    sameMultiplier: sameMultiplier, oppositeMultiplier: oppositeMultiplier
                    );
            }
        }

        /// <summary>
        /// Can change the user's form if it is holding an item with the effect <seealso cref="ItemEff.RKSMemory"/>.
        /// </summary>
        public class RKSSystem : AbilityEffect
        {
            public RKSSystem(
                )
                : base(effectType: AbilityEffectType.RKSSystem)
            {

            }
            public new RKSSystem Clone()
            {
                return new RKSSystem();
            }
        }

        /// <summary>
        /// Prevents the user from taking recoil damage.
        /// </summary>
        public class RockHead : AbilityEffect
        {
            public RockHead(
                )
                : base(effectType: AbilityEffectType.RockHead)
            {

            }
            public new RockHead Clone()
            {
                return new RockHead();
            }
        }

        /// <summary>
        /// When this Pokémon is hit by a direct attack, the attacker loses a portion of their HP.
        /// </summary>
        public class RoughSkin : AbilityEffect
        {
            /// <summary>
            /// Defines how damage is dealt.
            /// </summary>
            public General.Damage damage;

            /// <summary>
            /// This effect is only triggered if the Pokémon is hit by a contact move.
            /// </summary>
            public bool onlyContact;

            public RoughSkin(
                General.Damage damage,
                bool onlyContact = true,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.RoughSkin, filters: filters)
            {
                this.damage = damage.Clone();
                this.onlyContact = onlyContact;
            }
            public new RoughSkin Clone()
            {
                return new RoughSkin(
                    damage: damage,
                    onlyContact: onlyContact,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Ensures the user can always flee regardless of trapping moves and abilities (does not affect switching).
        /// </summary>
        public class RunAway : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the user flees using this ability.
            /// </summary>
            public string displayText;

            public RunAway(
                string displayText = "ability-runaway"
                )
                : base(effectType: AbilityEffectType.RunAway)
            {
                this.displayText = displayText;
            }
            public new RunAway Clone()
            {
                return new RunAway(displayText: displayText);
            }
        }

        /// <summary>
        /// Bypasses the given type immunities.
        /// </summary>
        public class Scrappy : AbilityEffect
        {
            /// <summary>
            /// The immunities that are bypassed.
            /// </summary>
            public List<string> bypassImmunities;

            public Scrappy(
                IEnumerable<string> bypassImmunities = null
                )
                : base(effectType: AbilityEffectType.Scrappy)
            {
                this.bypassImmunities = (bypassImmunities == null) ? new List<string>()
                    : new List<string>(bypassImmunities);
            }
            public new Scrappy Clone()
            {
                return new Scrappy(bypassImmunities: bypassImmunities);
            }
        }

        /// <summary>
        /// Ends the effects of screens when sent into battle.
        /// </summary>
        public class ScreenCleaner : AbilityEffect
        {
            /// <summary>
            /// If true, screens end for the ally team.
            /// </summary>
            public bool affectAlly;
            /// <summary>
            /// If true, screens end for the opposing team.
            /// </summary>
            public bool affectOpposing;

            public ScreenCleaner(
                bool affectAlly = true, bool affectOpposing = true
                )
                : base(effectType: AbilityEffectType.ScreenCleaner)
            {
                this.affectAlly = affectAlly;
                this.affectOpposing = affectOpposing;
            }
            public new ScreenCleaner Clone()
            {
                return new ScreenCleaner(
                    affectAlly: affectAlly, affectOpposing: affectOpposing
                    );
            }
        }

        /// <summary>
        /// Scales the user's chances for additional move effects.
        /// </summary>
        public class SereneGrace : AbilityEffect
        {
            /// <summary>
            /// The factor by which to multiply chances for additional effects.
            /// </summary>
            public float chanceMultiplier;

            public SereneGrace(
                float chanceMultiplier = 2f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.SereneGrace, filters: filters)
            {
                this.chanceMultiplier = chanceMultiplier;
            }
            public new SereneGrace Clone()
            {
                return new SereneGrace(
                    chanceMultiplier: chanceMultiplier,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Prevents opposing Pokémon from switching out or escaping. 
        /// </summary>
        public class ShadowTag : AbilityEffect
        {
            /// <summary>
            /// If true, this ability cannot trap others with the same ability.
            /// </summary>
            public bool immuneToSelf;
            /// <summary>
            /// If true, Pokémon are only trapped if they are adjacent to this Pokémon.
            /// </summary>
            public bool mustBeAdjacent;
            /// <summary>
            /// If true, this ability traps grounded Pokémon.
            /// </summary>
            public bool arenaTrap;

            /// <summary>
            /// The text displayed when the Pokémon is prevented from escaping.
            /// </summary>
            public string displayText;

            public ShadowTag(
                bool immuneToSelf = false, bool mustBeAdjacent = true, bool arenaTrap = false,
                string displayText = "ability-shadowtag",
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.ShadowTag, filters: filters)
            {
                this.immuneToSelf = immuneToSelf;
                this.mustBeAdjacent = mustBeAdjacent;
                this.arenaTrap = arenaTrap;
                this.displayText = displayText;
            }
            public new ShadowTag Clone()
            {
                return new ShadowTag(
                    immuneToSelf: immuneToSelf, mustBeAdjacent: mustBeAdjacent, arenaTrap: arenaTrap,
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Removes additional move effects in favour of scaling move power.
        /// </summary>
        public class SheerForce : AbilityEffect
        {
            /// <summary>
            /// The power multiplier applied to moves.
            /// </summary>
            public float powerMultiplier;

            public SheerForce(
                float powerMultiplier = 1.3f
                )
                : base(effectType: AbilityEffectType.SheerForce)
            {
                this.powerMultiplier = powerMultiplier;
            }
            public new SheerForce Clone()
            {
                return new SheerForce(powerMultiplier: powerMultiplier);
            }
        }

        /// <summary>
        /// The user is unaffected by additional affects of moves.
        /// </summary>
        public class ShieldDust : AbilityEffect
        {
            public ShieldDust() : base(effectType: AbilityEffectType.ShieldDust)
            {
            }
            public new ShieldDust Clone()
            {
                return new ShieldDust();
            }
        }

        /// <summary>
        /// Blocks statuses depending on the user's form.
        /// </summary>
        public class ShieldsDown : AbilityEffect
        {
            public class MeteorForm
            {
                public List<string> forms;
                public List<Filter.Harvest> blockedStatuses;

                public MeteorForm(
                    IEnumerable<string> forms,
                    IEnumerable<Filter.Harvest> blockedStatuses
                    )
                {
                    this.forms = new List<string>(forms);
                    this.blockedStatuses = new List<Filter.Harvest>();
                    if (blockedStatuses != null)
                    {
                        List<Filter.Harvest> preList = new List<Filter.Harvest>(blockedStatuses);
                        for (int i = 0; i < preList.Count; i++)
                        {
                            this.blockedStatuses.Add(preList[i].Clone());
                        }
                    }
                }
                public MeteorForm Clone()
                {
                    return new MeteorForm(
                        forms: forms,
                        blockedStatuses: blockedStatuses
                        );
                }
                public bool IsAForm(Pokemon pokemon)
                {
                    for (int i = 0; i < forms.Count; i++)
                    {
                        if (pokemon.pokemonID == forms[i])
                        {
                            return true;
                        }
                    }
                    return false;
                }
                public bool IsStatusBlocked(StatusPKData statusData)
                {
                    for (int i = 0; i < blockedStatuses.Count; i++)
                    {
                        if (blockedStatuses[i].DoesStatusSatisfy(statusData))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            public List<MeteorForm> meteorForms;

            public ShieldsDown(
                IEnumerable<MeteorForm> meteorForms = null
                )
                : base(effectType: AbilityEffectType.ShieldsDown)
            {
                this.meteorForms = new List<MeteorForm>();
                if (meteorForms != null)
                {
                    List<MeteorForm> preList = new List<MeteorForm>();
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.meteorForms.Add(preList[i].Clone());
                    }
                }
            }
            public new ShieldsDown Clone()
            {
                return new ShieldsDown(
                    meteorForms: meteorForms
                    );
            }
        }

        /// <summary>
        /// Reverses the user's stat stage changes.
        /// </summary>
        public class Simple : AbilityEffect
        {
            /// <summary>
            /// The amount by which stat stages are scaled.
            /// </summary>
            public int statModScale;

            public Simple(
                int statModScale = 2
                ) 
                : base(effectType: AbilityEffectType.Simple)
            {
                this.statModScale = statModScale;
            }
            public new Simple Clone()
            {
                return new Simple(statModScale: statModScale);
            }
        }

        /// <summary>
        /// Maximizes the number of hits for the user's <seealso cref="MoveEff.FuryAttack"/> moves.
        /// </summary>
        public class SkillLink : AbilityEffect
        {
            public SkillLink() : base(effectType: AbilityEffectType.SkillLink)
            {
            }
            public new SkillLink Clone()
            {
                return new SkillLink();
            }
        }

        /// <summary>
        /// Scales the user's stats for the first X turns of the battle.
        /// </summary>
        public class SlowStart : AbilityEffect
        {
            /// <summary>
            /// The stats to scale.
            /// </summary>
            public General.StatScale statScale;
            /// <summary>
            /// The amount of turns this effect stays active for.
            /// </summary>
            public int turnsActive;
            /// <summary>
            /// The text displayed when the user enters battle.
            /// </summary>
            public string displayText;

            public SlowStart(
                General.StatScale statScale,
                int turnsActive = 5,
                string displayText = "ability-slowstart",

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.SlowStart, filters: filters)
            {
                this.statScale = statScale.Clone();
                this.turnsActive = turnsActive;
                this.displayText = displayText;
            }
            public new SlowStart Clone()
            {
                return new SlowStart(
                    statScale: statScale,
                    turnsActive: turnsActive,
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Scales the damage of critical hits by the user's moves.
        /// </summary>
        public class Sniper : AbilityEffect
        {
            /// <summary>
            /// The boost to apply to critical damage.
            /// </summary>
            public float criticalBoost;

            public Sniper(
                float criticalBoost = 1.5f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Sniper, filters: filters)
            {
                this.criticalBoost = criticalBoost;
            }
            public new Sniper Clone()
            {
                return new Sniper(
                    criticalBoost: criticalBoost,
                    filters: filters);
            }
        }

        /// <summary>
        /// Scales super-effective damage dealt to the user. 
        /// </summary>
        public class SolidRock : AbilityEffect
        {
            /// <summary>
            /// Damage scaled for super-effective attacks.
            /// </summary>
            public float superEffectiveModifier;
            /// <summary>
            /// Damage scaled for not-very effective attacks.
            /// </summary>
            public float notVeryEffectiveModifier;

            public SolidRock(
                float superEffectiveModifier = 0.75f,
                float notVeryEffectiveModifier = 1f,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.SolidRock, filters: filters)
            {
                this.superEffectiveModifier = superEffectiveModifier;
                this.notVeryEffectiveModifier = notVeryEffectiveModifier;
            }
            public new SolidRock Clone()
            {
                return new SolidRock(
                    superEffectiveModifier: superEffectiveModifier,
                    notVeryEffectiveModifier: notVeryEffectiveModifier,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Modifies stats if an opposing Pokémon faints.
        /// </summary>
        public class SoulHeart : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            public SoulHeart(General.StatStageMod statStageMod)
                : base(effectType: AbilityEffectType.SoulHeart)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new SoulHeart Clone()
            {
                return new SoulHeart(statStageMod: statStageMod);
            }
        }

        /// <summary>
        /// Modifies the user's stats at the end of each turn.
        /// </summary>
        public class SpeedBoost : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            public SpeedBoost(General.StatStageMod statStageMod)
                : base(effectType: AbilityEffectType.SpeedBoost)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new SpeedBoost Clone()
            {
                return new SpeedBoost(statStageMod: statStageMod);
            }
        }

        /// <summary>
        /// Increases move power against targets that have switched in this turn.
        /// </summary>
        public class Stakeout : AbilityEffect
        {
            /// <summary>
            /// The power multiplier applied to moves.
            /// </summary>
            public float powerMultiplier;

            public Stakeout(float powerMultiplier = 2f) : base(effectType: AbilityEffectType.Stakeout)
            {
                this.powerMultiplier = powerMultiplier;
            }
            public new Stakeout Clone()
            {
                return new Stakeout(powerMultiplier: powerMultiplier);
            }
        }

        /// <summary>
        /// The user always moves last within its priority bracket.
        /// </summary>
        public class Stall : AbilityEffect
        {
            public Stall(
                IEnumerable<Filter.FilterEffect> filters = null
                ) : base(effectType: AbilityEffectType.Stall, filters: filters)
            {
            }
            public new Stall Clone()
            {
                return new Stall(filters: filters);
            }
        }

        /// <summary>
        /// Changes the Pokémon's form right before moves are used, depending on the type of move used.
        /// </summary>
        public class StanceChange : AbilityEffect
        {
            public class Transformation
            {
                public Filter.MoveCheck moveCheck;
                public General.FormTransformation transformation;
                public string displayText;

                public Transformation(
                    Filter.MoveCheck moveCheck,
                    General.FormTransformation transformation,
                    string displayText = null)
                {
                    this.moveCheck = moveCheck.Clone();
                    this.transformation = transformation.Clone();
                    this.displayText = displayText;
                }
                public Transformation Clone()
                {
                    return new Transformation(
                        moveCheck: moveCheck,
                        transformation: transformation,
                        displayText: displayText
                        );
                }
            }

            /// <summary>
            /// The list of transformations.
            /// </summary>
            public List<Transformation> transformations;

            public StanceChange(
                IEnumerable<Transformation> transformations = null
                )
                : base(effectType: AbilityEffectType.StanceChange)
            {
                this.transformations = new List<Transformation>();
                if (transformations != null)
                {
                    List<Transformation> preList = new List<Transformation>(transformations);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.transformations.Add(preList[i].Clone());
                    }
                }
            }
            public new StanceChange Clone()
            {
                return new StanceChange(
                    transformations: transformations
                    );
            }
        }

        /// <summary>
        /// Modifies stats if the user when flinched.
        /// </summary>
        public class Steadfast : AbilityEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            public Steadfast(
                General.StatStageMod statStageMod,
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(effectType: AbilityEffectType.Steadfast, filters: filters)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new Steadfast Clone()
            {
                return new Steadfast(
                    statStageMod: statStageMod,
                    filters: filters);
            }
        }

        /// <summary>
        /// The user cannot lose their held item.
        /// </summary>
        public class StickyHold : AbilityEffect
        {
            public StickyHold(
                IEnumerable<Filter.FilterEffect> filters = null
                ) : base(effectType: AbilityEffectType.StickyHold, filters: filters)
            {
            }
            public new StickyHold Clone()
            {
                return new StickyHold(filters: filters);
            }
        }

        /// <summary>
        /// Prevents this pokemon from fainting from a direct hit at a certain HP threshold.
        /// </summary>
        public class Sturdy : AbilityEffect
        {
            /// <summary>
            /// The HP % the user's HP must at least be at to trigger this effect.
            /// </summary>
            public float hpThreshold;

            /// <summary>
            /// The text displayed when the user hangs on.
            /// </summary>
            public string displayText;

            public Sturdy(
                float hpThreshold = 1f,
                string displayText = "ability-sturdy",

                float chance = 1f,
                IEnumerable<Filter.FilterEffect> filters = null
                ) : base(effectType: AbilityEffectType.Sturdy, chance: chance, filters: filters)
            {
                this.displayText = displayText;
                this.hpThreshold = hpThreshold;
            }
            public new Sturdy Clone()
            {
                return new Sturdy(
                    hpThreshold: hpThreshold, 
                    displayText: displayText,
                    chance: chance, filters: filters);
            }
        }

        /// <summary>
        /// The user cannot be forced out.
        /// </summary>
        public class SuctionCups : AbilityEffect
        {
            public SuctionCups(
                IEnumerable<Filter.FilterEffect> filters = null
                ) : base(effectType: AbilityEffectType.SuctionCups, filters: filters)
            {
            }
            public new SuctionCups Clone()
            {
                return new SuctionCups(filters: filters);
            }
        }

        /// <summary>
        /// Increases the base critical hit rate for this Pokémon's moves.
        /// </summary>
        public class SuperLuck : AbilityEffect
        {
            /// <summary>
            /// If true, this move always results in critical hits.
            /// </summary>
            public bool alwaysCritical;
            /// <summary>
            /// The critical level added on to this move.
            /// </summary>
            public int criticalBoost;

            public SuperLuck(
                bool alwaysCritical = false, int criticalBoost = 1,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.SuperLuck, filters: filters)
            {
                this.alwaysCritical = alwaysCritical;
                this.criticalBoost = criticalBoost;
            }
            public new SuperLuck Clone()
            {
                return new SuperLuck(
                    alwaysCritical: alwaysCritical, 
                    criticalBoost: criticalBoost,
                    filters: filters);
            }
        }

        /// <summary>
        /// Scales stats based on the current battle conditions (if the user would be affected).
        /// </summary>
        public class SwiftSwim : AbilityEffect
        {
            public class SwiftSwimCondition
            {
                public List<string> conditions;
                public General.StatScale statScale;
                /// <summary>
                /// If true, the stat scales apply to all ally Pokémon on the field.
                /// </summary>
                public bool flowerGift;

                public SwiftSwimCondition(
                    IEnumerable<string> conditions,
                    General.StatScale statScale,
                    bool flowerGift = false
                    )
                {
                    this.conditions = new List<string>(conditions);
                    this.statScale = statScale.Clone();
                    this.flowerGift = flowerGift;
                }
                public SwiftSwimCondition Clone()
                {
                    return new SwiftSwimCondition(
                        conditions: conditions,
                        statScale: statScale,
                        flowerGift: flowerGift
                        );
                }
            }
            public List<SwiftSwimCondition> conditions;

            public SwiftSwim(
                IEnumerable<SwiftSwimCondition> conditions
                )
                : base (effectType: AbilityEffectType.SwiftSwim)
            {
                this.conditions = new List<SwiftSwimCondition>();
                if (conditions != null)
                {
                    List<SwiftSwimCondition> preList = new List<SwiftSwimCondition>(conditions);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.conditions.Add(preList[i].Clone());
                    }
                }
            }
        }

        /// <summary>
        /// The user passes its item to an ally that has used up an item.
        /// </summary>
        public class Symbiosis : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the user's item is passed.
            /// </summary>
            public string displayText;

            public Symbiosis(
                string displayText = "ability-symbiosis",
                IEnumerable<Filter.FilterEffect> filters = null
                ) : base(effectType: AbilityEffectType.Symbiosis, filters: filters)
            {
                this.displayText = displayText;
            }
            public new Symbiosis Clone()
            {
                return new Symbiosis(
                    displayText: displayText,
                    filters: filters);
            }
        }

        /// <summary>
        /// Reflects status conditions back at the user.
        /// </summary>
        public class Synchronize : AbilityEffect
        {
            public Filter.Harvest conditionCheck;

            public Synchronize(
                Filter.Harvest conditionCheck,
                IEnumerable<string> statuses = null
                )
                : base(effectType: AbilityEffectType.Synchronize)
            {
                this.conditionCheck = conditionCheck.Clone();
            }
            public new Synchronize Clone()
            {
                return new Synchronize(conditionCheck: conditionCheck);
            }
        }

        /// <summary>
        /// Increases move power if the base power of the move is at or below a specific threshold.
        /// </summary>
        public class Technician : AbilityEffect
        {
            /// <summary>
            /// Base power threshold (after move multipliers).
            /// </summary>
            public int threshold;
            /// <summary>
            /// The power multiplier applied to moves.
            /// </summary>
            public float powerMultiplier;

            public Technician(
                int threshold = 60, float powerMultiplier = 1.5f
                ) 
                : base(effectType: AbilityEffectType.Technician)
            {
                this.threshold = threshold;
                this.powerMultiplier = powerMultiplier;
            }
            public new Technician Clone()
            {
                return new Technician(threshold: threshold, powerMultiplier: powerMultiplier);
            }
        }

        /// <summary>
        /// The user is immune to ally moves.
        /// </summary>
        public class Telepathy : AbilityEffect
        {

            public Telepathy(
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(effectType: AbilityEffectType.Telepathy, filters: filters)
            {
            }
            public new Telepathy Clone()
            {
                return new Telepathy(filters: filters);
            }
        }

        /// <summary>
        /// Scales super-effective damage dealt to the user. 
        /// </summary>
        public class TintedLens : AbilityEffect
        {
            /// <summary>
            /// Damage scaled for super-effective attacks.
            /// </summary>
            public float neuroforceModifier;
            /// <summary>
            /// Damage scaled for not-very effective attacks.
            /// </summary>
            public float notVeryEffectiveModifier;

            public TintedLens(
                float neuroforceModifier = 0.75f,
                float notVeryEffectiveModifier = 1f,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.TintedLens, filters: filters)
            {
                this.neuroforceModifier = neuroforceModifier;
                this.notVeryEffectiveModifier = notVeryEffectiveModifier;
            }
            public new TintedLens Clone()
            {
                return new TintedLens(
                    neuroforceModifier: neuroforceModifier,
                    notVeryEffectiveModifier: notVeryEffectiveModifier,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// The user copies an opponent's ability.
        /// </summary>
        public class Trace : AbilityEffect
        {
            /// <summary>
            /// The text displayed when the user traces a target's ability.
            /// </summary>
            public string displayText;

            public Trace(
                string displayText = "ability-trace",

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Trace, filters: filters)
            {
                this.displayText = displayText;

            }
            public new Trace Clone()
            {
                return new Trace(
                    displayText: displayText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// The user does not attack every second turn.
        /// </summary>
        public class Truant : AbilityEffect
        {
            /// <summary>
            /// The turns waiting between attacking.
            /// </summary>
            public int turnsWaiting;
            /// <summary>
            /// The text that displays when the Pokémon enters battle.
            /// </summary>
            public string displayText;

            public Truant(
                int turnsWaiting = 1,
                string displayText = "ability-truant"
                )
                : base(effectType: AbilityEffectType.Truant)
            {
                this.turnsWaiting = turnsWaiting;
                this.displayText = displayText;
            }
            public new Truant Clone()
            {
                return new Truant(turnsWaiting: turnsWaiting, displayText: displayText);
            }
        }

        /// <summary>
        /// Ignores the stat changes of the target when attacking.
        /// </summary>
        public class Unaware : AbilityEffect
        {
            /// <summary>
            /// The stat changes of the target Pokémon that are ignored when the user is attacking.
            /// </summary>
            public HashSet<PokemonStats> targetStatsIgnored;
            /// <summary>
            /// The stat changes of the attacking Pokémon that are ignored when the user is being attacked.
            /// </summary>
            public HashSet<PokemonStats> attackerStatsIgnored;

            public Unaware(
                IEnumerable<PokemonStats> targetStatsIgnored = null,
                IEnumerable<PokemonStats> attackerStatsIgnored = null
                )
                : base(effectType: AbilityEffectType.Unaware)
            {
                this.targetStatsIgnored = (targetStatsIgnored != null) 
                    ? new HashSet<PokemonStats>(targetStatsIgnored)
                    : new HashSet<PokemonStats>(GameSettings.btlPkmnStats);
                this.attackerStatsIgnored = (attackerStatsIgnored != null) 
                    ? new HashSet<PokemonStats>(attackerStatsIgnored)
                    : new HashSet<PokemonStats>(GameSettings.btlPkmnStats);
            }
            public new Unaware Clone()
            {
                return new Unaware(
                    targetStatsIgnored: targetStatsIgnored,
                    attackerStatsIgnored: attackerStatsIgnored
                    );
            }
        }

        /// <summary>
        /// Scales the user's stats once its held item is lost.
        /// </summary>
        public class Unburden : AbilityEffect
        {
            /// <summary>
            /// The stats to scale.
            /// </summary>
            public General.StatScale statScale;

            public Unburden(
                General.StatScale statScale,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.Unburden, filters: filters)
            {
                this.statScale = statScale.Clone();
            }
            public new Unburden Clone()
            {
                return new Unburden(
                    statScale: statScale,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Allows the user's attack to bypass protection moves.
        /// </summary>
        public class UnseenFist : AbilityEffect
        {
            public bool ignoreProtect;
            public bool ignoreCraftyShield;
            public bool ignoreMaxGuard;

            public UnseenFist(
                bool ignoreProtect = true,
                bool ignoreCraftyShield = true,
                bool ignoreMaxGuard = true,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.UnseenFist, filters: filters)
            {
                this.ignoreProtect = ignoreProtect;
                this.ignoreCraftyShield = ignoreCraftyShield;
                this.ignoreMaxGuard = ignoreMaxGuard;
            }
            public new UnseenFist Clone()
            {
                return new UnseenFist(
                    ignoreProtect: ignoreProtect,
                    ignoreCraftyShield: ignoreCraftyShield,
                    ignoreMaxGuard: ignoreMaxGuard,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Provides an immunity to the given types, and extra effects can be specified.
        /// </summary>
        public class VoltAbsorb : AbilityEffect
        {
            public class VoltAbsorbCondition
            {
                /// <summary>
                /// The move types the ability protects against.
                /// </summary>
                public List<string> moveTypes;

                /// <summary>
                /// The % of HP recovered when hit by a move in <seealso cref="moveTypes"/>.
                /// </summary>
                public float absorbPercent;
                /// <summary>
                /// If positive, this boost is applied to the given move-types as long as the user
                /// remains in battle.
                /// </summary>
                public float flashFireBoost;
                /// <summary>
                /// The stat stage applied when hit by a move in <seealso cref="moveTypes"/>.
                /// </summary>
                public General.StatStageMod motorDrive;

                public VoltAbsorbCondition(
                    IEnumerable<string> moveTypes = null,
                    float absorbPercent = 0f, float flashFireBoost = 0f,
                    General.StatStageMod motorDrive = null
                    )
                {
                    this.moveTypes = (moveTypes == null) ? new List<string>() : new List<string>(moveTypes);
                    this.absorbPercent = absorbPercent;
                    this.flashFireBoost = flashFireBoost;
                    this.motorDrive = (motorDrive == null) ? null : motorDrive.Clone();
                }
                public VoltAbsorbCondition Clone()
                {
                    return new VoltAbsorbCondition(
                        moveTypes: moveTypes,
                        absorbPercent: absorbPercent, flashFireBoost: flashFireBoost,
                        motorDrive: motorDrive
                        );
                }
            }

            /// <summary>
            /// The volt absorb conditions
            /// </summary>
            public List<VoltAbsorbCondition> conditions;

            /// <summary>
            /// The text displayed when the move is blocked.
            /// </summary>
            public string displayText;

            public VoltAbsorb(
                IEnumerable<VoltAbsorbCondition> conditions = null,
                string displayText = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.VoltAbsorb, filters: filters)
            {
                this.conditions = new List<VoltAbsorbCondition>();
                if (conditions != null)
                {
                    List<VoltAbsorbCondition> preList = new List<VoltAbsorbCondition>(conditions);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.conditions.Add(preList[i].Clone());
                    }
                }
                this.displayText = displayText;
            }
            public new VoltAbsorb Clone()
            {
                return new VoltAbsorb(
                    conditions: conditions,
                    displayText: displayText,

                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Prompts the Pokémon to switch out if it's HP drops below a certain threshold.
        /// </summary>
        public class WimpOut : AbilityEffect
        {
            /// <summary>
            /// The HP threshold needed to fall below to allow doe switching out.
            /// </summary>
            public float hpThreshold;

            public WimpOut(
                float hpThreshold = 0.5f
                )
                : base(effectType: AbilityEffectType.WimpOut)
            {
                this.hpThreshold = hpThreshold;
            }
            public new WimpOut Clone()
            {
                return new WimpOut(
                    hpThreshold: hpThreshold
                    );
            }
        }

        /// <summary>
        /// Causes immunity to moves unless they have certain effectiveness.
        /// </summary>
        public class WonderGuard : AbilityEffect
        {
            /// <summary>
            /// If true, attacks can hit if they hit neutrally.
            /// </summary>
            public bool affectNeutral;
            /// <summary>
            /// If true, attacks can hit if they hit super-effectively.
            /// </summary>
            public bool affectSuperEffective;
            /// <summary>
            /// If true, attacks can hit if they hit not-very effectively.
            /// </summary>
            public bool affectNotVeryEffective;

            public WonderGuard(
                bool affectNeutral = false,
                bool affectSuperEffective = true,
                bool affectNotVeryEffective = false,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.WonderGuard, filters: filters)
            {
                this.affectNeutral = affectNeutral;
                this.affectSuperEffective = affectSuperEffective;
                this.affectNotVeryEffective = affectNotVeryEffective;
            }
            public new WonderGuard Clone()
            {
                return new WonderGuard(
                    affectNeutral: affectNeutral,
                    affectSuperEffective: affectSuperEffective,
                    affectNotVeryEffective: affectNotVeryEffective,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Changes the accuracy of status moves used against this Pokémon.
        /// </summary>
        public class WonderSkin : AbilityEffect
        {
            public enum AccuracyMode
            {
                /// <summary>
                /// Forces the accuracy of moves to be equal to <seealso cref="accuracyValue"/>.
                /// </summary>
                Set,
                /// <summary>
                /// Scales the accuracy of moves by <seealso cref="accuracyValue"/>.
                /// </summary>
                Multiplier
            }
            public AccuracyMode mode;

            /// <summary>
            /// The accuracy value.
            /// </summary>
            public float accuracyValue;

            public WonderSkin(
                AccuracyMode mode = AccuracyMode.Set,
                float accuracyValue = 0.5f,
                bool steelySpirit = false,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: AbilityEffectType.WonderSkin, filters: filters)
            {
                this.mode = mode;
                this.accuracyValue = accuracyValue;
            }
            public new WonderSkin Clone()
            {
                return new WonderSkin(
                    mode: mode,
                    accuracyValue: accuracyValue,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Changes the Pokemon's form depending on its HP.
        /// </summary>
        public class ZenMode : AbilityEffect
        {
            /// <summary>
            /// The transformation that occurs.
            /// </summary>
            public General.FormTransformation transformation;
            /// <summary>
            /// The HP threshold check for the form change to trigger.
            /// </summary>
            public float hpThreshold;
            /// <summary>
            /// If true, the user's HP must be below the threshold. If false, its HP must be above the threshold.
            /// </summary>
            public bool checkBelow;
            /// <summary>
            /// Accompanying text after the form change.
            /// </summary>
            public string displayText;

            public ZenMode(
                General.FormTransformation transformation,
                float hpThreshold = 0.5f, bool checkBelow = true,
                string displayText = "pokemon-changeform"
                )
                : base(effectType: AbilityEffectType.ZenMode)
            {
                this.displayText = displayText;
                this.hpThreshold = hpThreshold;
                this.checkBelow = checkBelow;
                this.transformation = transformation.Clone();
            }
            public new ZenMode Clone()
            {
                return new ZenMode(
                    transformation: transformation,
                    hpThreshold: hpThreshold, checkBelow: checkBelow,
                    displayText: displayText
                    );
            }
        }
    }


    // ---ITEM EFFECTS---
    public class ItemEff
    {
        public class ItemEffect
        {
            /// <summary>
            /// The type of effect that this is.
            /// </summary>
            public ItemEffectType effectType;

            /// <summary>
            /// Additional restrictions on how the effect is applied.
            /// </summary>
            public List<Filter.FilterEffect> filters;

            /// <summary>
            /// The chance of the effect working.
            /// </summary>
            public float chance;

            /// <summary>
            /// If set to true, this effect can be applied if the item is consumed.
            /// </summary>
            public bool applyOnConsume;
            /// <summary>
            /// If set to true, this effect can be applied if the item is used.
            /// </summary>
            public bool applyOnUse;

            public ItemEffect(
                ItemEffectType effectType,
                IEnumerable<Filter.FilterEffect> filters = null,
                float chance = -1,
                bool applyOnConsume = false, bool applyOnUse = false
                )
            {
                this.effectType = effectType;
                this.filters = (filters == null) ? new List<Filter.FilterEffect>()
                    : new List<Filter.FilterEffect>(filters);
                this.chance = chance;
                this.applyOnConsume = applyOnConsume;
                this.applyOnUse = applyOnUse;
            }
            public ItemEffect Clone()
            {
                return
                    (this is Charcoal) ? (this as Charcoal).Clone()
                    : (this is ChoiceBand) ? (this as ChoiceBand).Clone()
                    : (this is ChoiceBandStats) ? (this as ChoiceBandStats).Clone()
                    : (this is FocusBand) ? (this as FocusBand).Clone()
                    : (this is GriseousOrb) ?
                        (
                        (this is ArceusPlate) ? (this as ArceusPlate).Clone()
                        : (this is MegaStone) ? (this as MegaStone).Clone()
                        : (this is RKSMemory) ? (this as RKSMemory).Clone()
                        : (this as GriseousOrb).Clone()
                        )
                    : (this is Judgment) ? (this as Judgment).Clone()
                    : (this is LifeOrb) ? (this as LifeOrb).Clone() 
                    : (this is MegaStone) ? (this as MegaStone).Clone()
                    : (this is NaturalGift) ? (this as NaturalGift).Clone()
                    : (this is PokeBall) ? (this as PokeBall).Clone()
                    : (this is Potion) ? (this as Potion).Clone()
                    : (this is QuickClaw) ? (this as QuickClaw).Clone()
                    : (this is ShedShell) ? (this as ShedShell).Clone()
                    : (this is TriggerSitrusBerry) ? (this as TriggerSitrusBerry).Clone()
                    : (this is ZCrystal) ? (this as ZCrystal).Clone()
                    : (this is ZCrystalSignature) ? (this as ZCrystalSignature).Clone()
                    : new ItemEffect(
                        effectType: effectType,
                        filters: filters,
                        chance: chance, applyOnConsume: applyOnConsume, applyOnUse: applyOnUse
                        );
            }
        }

        // Form-Changing Items
        /// <summary>
        /// To be used alongside <seealso cref="GriseousOrb"/>. Will only enable FormChange if the user has an 
        /// ability with the effect <seealso cref="AbilityEff.Multitype"/>.
        /// </summary>
        public class ArceusPlate : GriseousOrb
        {
            public ArceusPlate(string baseFormID, string formID) : base(baseFormID: baseFormID, formID: formID) 
            { }
            public new ArceusPlate Clone()
            {
                return new ArceusPlate(baseFormID: baseFormID, formID: formID);
            }
        }
        /// <summary>
        /// Changes eligible Pokémon into another form if this item is held. Cannot be forcibly removed under most 
        /// circumstances if the holder is a valid Pokémon.
        /// </summary>
        public class GriseousOrb : ItemEffect
        {
            /// <summary>
            /// The base Pokémon ID that can change into <seealso cref="formID"/>.
            /// </summary>
            public string baseFormID;
            /// <summary>
            /// The Pokémon ID that <seealso cref="baseFormID"/> is changed into.
            /// </summary>
            public string formID;

            public GriseousOrb(string baseFormID, string formID) : base(effectType: ItemEffectType.GriseousOrb)
            {
                this.baseFormID = baseFormID;
                this.formID = formID;
            }
            public new GriseousOrb Clone()
            {
                return new GriseousOrb(baseFormID: baseFormID, formID: formID);
            }
        }
        /// <summary>
        /// Similar to <seealso cref="GriseousOrb"/>, but requires a turn of activation before form is changed.
        /// </summary>
        public class MegaStone : GriseousOrb
        {
            public MegaStone(string baseFormID, string formID) : base(baseFormID: baseFormID, formID: formID)
            { }
            public new MegaStone Clone()
            {
                return new MegaStone(baseFormID: baseFormID, formID: formID);
            }
        }
        /// <summary>
        /// To be used alongside <seealso cref="GriseousOrb"/>. Will only enable FormChange if the user has an 
        /// ability with the effect <seealso cref="AbilityEff.RKSSystem"/>.
        /// </summary>
        public class RKSMemory : GriseousOrb
        {
            public RKSMemory(string baseFormID, string formID) : base(baseFormID: baseFormID, formID: formID)
            { }
            public new RKSMemory Clone()
            {
                return new RKSMemory(baseFormID: baseFormID, formID: formID);
            }
        }

        /// <summary>
        /// Scales the power of the holder's moves if they satisfy the specified conditions.
        /// </summary>
        public class Charcoal : ItemEffect
        {
            /// <summary>
            /// The amount to scale move power by.
            /// </summary>
            public float powerModifier;

            public Charcoal(
                float powerModifier = 1.2f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: ItemEffectType.Charcoal, filters: filters)
            {
                this.powerModifier = powerModifier;
            }
            public new Charcoal Clone()
            {
                return new Charcoal(
                    powerModifier: powerModifier,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Locks the holder into its first-selected move until it leaves the field.
        /// </summary>
        public class ChoiceBand : ItemEffect
        {
            public ChoiceBand(
                )
                : base(effectType: ItemEffectType.ChoiceBand)
            {

            }
            public new ChoiceBand Clone()
            {
                return new ChoiceBand();
            }
        }

        /// <summary>
        /// Scales the user's stats.
        /// </summary>
        public class ChoiceBandStats : ItemEffect
        {
            /// <summary>
            /// The stats to scale.
            /// </summary>
            public General.StatScale statScale;
            /// <summary>
            /// If true, the stat scales apply to all ally Pokémon on the field.
            /// </summary>
            public bool victoryStar;
            /// <summary>
            /// If non-negative, the user's HP must be at or below this percentage to apply the stat scaling.
            /// </summary>
            public float defeatistThreshold;

            public ChoiceBandStats(
                General.StatScale statScale,
                bool victoryStar = false,
                float defeatistThreshold = -1
                )
                : base(effectType: ItemEffectType.ChoiceBandStats)
            {
                this.statScale = statScale.Clone();
                this.victoryStar = victoryStar;
                this.defeatistThreshold = defeatistThreshold;
            }
            public new ChoiceBandStats Clone()
            {
                return new ChoiceBandStats(
                    statScale: statScale,
                    victoryStar: victoryStar,
                    defeatistThreshold: defeatistThreshold
                    );
            }
        }

        /// <summary>
        /// Prevents this pokemon from fainting from a direct hit at a certain HP threshold.
        /// </summary>
        public class FocusBand : ItemEffect
        {
            /// <summary>
            /// The HP % the user's HP must at least be at to trigger this effect.
            /// </summary>
            public float hpThreshold;

            /// <summary>
            /// The text displayed when the user hangs on.
            /// </summary>
            public string displayText;

            public FocusBand(
                float hpThreshold = 1f,
                string displayText = "item-focusband",

                float chance = 1f,
                IEnumerable<Filter.FilterEffect> filters = null
                ) : base(effectType: ItemEffectType.FocusBand, chance: chance, filters: filters)
            {
                this.displayText = displayText;
                this.hpThreshold = hpThreshold;
            }
            public new FocusBand Clone()
            {
                return new FocusBand(
                    hpThreshold: hpThreshold,
                    displayText: displayText,
                    chance: chance, filters: filters);
            }
        }

        /// <summary>
        /// Changes the type of the holder's moves with the <seealso cref="MoveEff.Judgment"/> effect.
        /// </summary>
        public class Judgment : ItemEffect
        {
            /// <summary>
            /// The type that eligible moves become.
            /// </summary>
            public string moveType;

            public Judgment(string moveType) : base(effectType: ItemEffectType.Judgment)
            {
                this.moveType = moveType;
            }
            public new Judgment Clone()
            {
                return new Judgment(moveType: moveType);
            }
        }

        /// <summary>
        /// Modifies the user's stats.
        /// </summary>
        public class LiechiBerry : ItemEffect
        {
            /// <summary>
            /// The stat stage changes applied.
            /// </summary>
            public General.StatStageMod statStageMod;

            public LiechiBerry(
                General.StatStageMod statStageMod)
                : base(effectType: ItemEffectType.LiechiBerry)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new LiechiBerry Clone()
            {
                return new LiechiBerry(statStageMod: statStageMod);
            }
        }

        /// <summary>
        /// Deals proportional damage to the user after move use.
        /// </summary>
        public class LifeOrb : ItemEffect
        {
            public enum RecoilMode
            {
                /// <summary>
                /// Damage dealt to the user is proportional to the total damage dealt/
                /// </summary>
                Damage,
                /// <summary>
                /// Damage dealt to the user is proportional to the user's max HP.
                /// </summary>
                MaxHP
            }
            /// <summary>
            /// The type of recoil the user takes.
            /// </summary>
            public RecoilMode recoilMode;
            /// <summary>
            /// Damage dealt to the user determined by <seealso cref="recoilMode"/>.
            /// </summary>
            public float hpLossPercent;
            /// <summary>
            /// If false, the user will not take recoil if they have the <seealso cref="AbilityEff.RockHead"/>
            /// ability effect.
            /// </summary>
            public bool bypassRockHead;

            /// <summary>
            /// The text displayed when the user takes recoil damage.
            /// </summary>
            public string displayText;

            public LifeOrb(
                RecoilMode recoilMode = RecoilMode.MaxHP,
                float hpLossPercent = 0.25f, bool bypassRockHead = false,
                string displayText = "item-lifeorb"
                )
                : base(effectType: ItemEffectType.LifeOrb)
            {
                this.recoilMode = recoilMode;
                this.hpLossPercent = hpLossPercent;
                this.bypassRockHead = bypassRockHead;
                this.displayText = displayText;
            }
            public new LifeOrb Clone()
            {
                return new LifeOrb(
                    recoilMode: recoilMode,
                    hpLossPercent: hpLossPercent, bypassRockHead: bypassRockHead,
                    displayText: displayText
                    );
            }
        }
        
        /// <summary>
        /// This item can be used by Natural Gift and transforms its type and power.
        /// </summary>
        public class NaturalGift : ItemEffect
        {
            /// <summary>
            /// If null, the move's type does not change.
            /// </summary>
            public string moveType;
            /// <summary>
            /// If less than 1, the move's base power does not change.
            /// </summary>
            public int basePower;

            public NaturalGift(
                string moveType = "normal", int basePower = 80
                )
                : base(effectType: ItemEffectType.NaturalGift)
            {
                this.moveType = moveType;
                this.basePower = basePower;
            }
            public new NaturalGift Clone()
            {
                return new NaturalGift(
                    moveType: moveType, basePower: basePower
                    );
            }
        }

        /// <summary>
        /// Contains all effects relating to Poké Ball catch rates and modifiers.
        /// </summary>
        public class PokeBall : ItemEffect
        {
            /// <summary>
            /// The base catch rate modifier for this 
            /// </summary>
            public float catchRateModifier;

            public PokeBall(
                float catchRateModifier = 1f
                )
                : base(effectType: ItemEffectType.PokeBall)
            {
                this.catchRateModifier = catchRateModifier;
            }
            public new PokeBall Clone()
            {
                return new PokeBall(
                    catchRateModifier: catchRateModifier
                    );
            }
        }

        /// <summary>
        /// Heals the Pokémon by a given amount of HP.
        /// </summary>
        public class Potion : ItemEffect
        {
            /// <summary>
            /// Defines how HP is recovered.
            /// </summary>
            public General.HealHP healHP;

            public Potion(
                General.HealHP healHP,
                bool applyOnConsume = true, bool applyOnUse = true
                )
                : base(effectType: ItemEffectType.Potion, applyOnConsume: applyOnConsume, applyOnUse: applyOnUse)
            {
                this.healHP = healHP.Clone();
            }
            public new Potion Clone()
            {
                return new Potion(
                    healHP: healHP,
                    applyOnConsume: applyOnConsume, applyOnUse: applyOnUse
                    );
            }
        }

        /// <summary>
        /// The holder has a chance of attacking first within its priority bracket.
        /// </summary>
        public class QuickClaw : ItemEffect
        {
            /// <summary>
            /// The text displayed if the effect is triggered.
            /// </summary>
            public string displayText;

            public QuickClaw(
                string displayText = "item-quickclaw",
                float chance = 0.2f,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: ItemEffectType.QuickClaw, chance: chance, filters: filters)
            {
                this.displayText = displayText;
            }
            public new QuickClaw Clone()
            {
                return new QuickClaw(
                    displayText: displayText,
                    chance: chance, filters: filters
                    );
            }
        }

        /// <summary>
        /// Allows the holder to switch out, regardless of trapping moves or ability.
        /// </summary>
        public class ShedShell : ItemEffect
        {
            public ShedShell() : base(effectType: ItemEffectType.ShedShell)
            {

            }
            public new ShedShell Clone()
            {
                return new ShedShell();
            }
        }

        /// <summary>
        /// Enables this item to be consumed if the holder's HP falls below the given threshold.
        /// </summary>
        public class TriggerSitrusBerry : ItemEffect
        {
            /// <summary>
            /// Activates this item if the holder's HP falls below this threshold.
            /// </summary>
            public float hpThreshold;

            public TriggerSitrusBerry(
                float hpThreshold = 0.25f
                )
                : base(effectType: ItemEffectType.TriggerOnHPLoss)
            {
                this.hpThreshold = hpThreshold;
            }
            public new TriggerSitrusBerry Clone()
            {
                return new TriggerSitrusBerry(
                    hpThreshold: hpThreshold
                    );
            }
        }

        /// <summary>
        /// Transforms damaging moves of a given type into Z-Moves. Activates Z-Effects for moves of the given type.
        /// </summary>
        public class ZCrystal : ItemEffect
        {
            /// <summary>
            /// The type of moves that are eligible.
            /// </summary>
            public string moveType;
            /// <summary>
            /// The Z-Move that eligible moves are transformed into.
            /// </summary>
            public string ZMove;

            public ZCrystal(
                string moveType,
                string ZMove
                )
                : base(effectType: ItemEffectType.ZCrystal)
            {
                this.moveType = moveType;
                this.ZMove = ZMove;
            }
            public new ZCrystal Clone()
            {
                return new ZCrystal(moveType: moveType, ZMove: ZMove);
            }
        }
        
        /// <summary>
        /// Transforms eligible moves into a Z-Move for eligible Pokémon only.
        /// </summary>
        public class ZCrystalSignature : ItemEffect
        {
            /// <summary>
            /// The Z-Move that eligible moves will be turned into.
            /// </summary>
            public string ZMove;
            /// <summary>
            /// The eligible moves that can be transformed into <seealso cref="zMove"/>.
            /// </summary>
            public List<string> eligibleMoves;
            /// <summary>
            /// The eligible Pokémon IDs that are able to use this Z-Crystal.
            /// </summary>
            public List<string> pokemonIDs;

            public ZCrystalSignature(
                string ZMove,
                IEnumerable<string> eligibleMoves,
                IEnumerable<string> eligiblePokemonIDs
                )
                : base(effectType: ItemEffectType.ZCrystalSignature)
            {
                this.ZMove = ZMove;
                this.eligibleMoves = (eligibleMoves == null) ? new List<string>() : new List<string>(eligibleMoves);
                this.pokemonIDs = (eligiblePokemonIDs == null) ? new List<string>()
                    : new List<string>(eligiblePokemonIDs);
            }
            public new ZCrystalSignature Clone()
            {
                return new ZCrystalSignature(
                    ZMove: ZMove, 
                    eligibleMoves: eligibleMoves, 
                    eligiblePokemonIDs: pokemonIDs);
            }
        }
    }


    // ---POKEMON STATUS EFFECTS---
    /// <summary>
    /// Collection of all Pokémon-based Status Effects.
    /// </summary>
    public class StatusPKEff
    {
        public class PokemonSE
        {
            /// <summary>
            /// The type of Pokémon status effect it is.
            /// </summary>
            public PokemonSEType effectType;
            /// <summary>
            /// The time that this effect applies itself.
            /// </summary>
            public PokemonSETiming timing;
            /// <summary>
            /// If true, this effect can stack where applicable.
            /// </summary>
            public bool isStackable;

            /// <summary>
            /// Additional restrictions on how the effect is applied.
            /// </summary>
            public List<Filter.FilterEffect> filters;

            public PokemonSE(
                PokemonSEType effectType,
                PokemonSETiming timing = PokemonSETiming.Unique,
                bool isStackable = false,
                IEnumerable<Filter.FilterEffect> filters = null
                )
            {
                this.effectType = effectType;
                this.timing = timing;
                this.isStackable = isStackable;
                this.filters = new List<Filter.FilterEffect>();
                if (filters != null)
                {
                    List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                    for (int i = 0; i < tempFilters.Count; i++)
                    {
                        this.filters.Add(tempFilters[i].Clone());
                    }
                }
            }
            public PokemonSE Clone()
            {
                return
                    (this is Bound) ? (this as Bound).Clone()
                    : (this is Burn) ? (this as Burn).Clone()
                    : (this is DefenseCurl) ? (this as DefenseCurl).Clone()
                    : (this is Disable) ? (this as Disable).Clone()
                    : (this is Electrify) ? (this as Electrify).Clone()
                    : (this is Flinch) ? (this as Flinch).Clone()
                    : (this is Freeze) ? (this as Freeze).Clone()
                    : (this is HPLoss) ? (this as HPLoss).Clone()
                    : (this is Identification) ? (this as Identification).Clone()
                    : (this is Imprison) ? (this as Imprison).Clone()
                    : (this is Infatuation) ? (this as Infatuation).Clone()
                    : (this is NonVolatile) ? (this as NonVolatile).Clone()
                    : (this is Octolock) ? (this as Octolock).Clone()
                    : (this is Paralysis) ? (this as Paralysis).Clone()
                    : (this is PerishSong) ? (this as PerishSong).Clone()
                    : (this is Sleep) ? (this as Sleep).Clone()
                    : (this is TarShot) ? (this as TarShot).Clone()
                    : (this is TypeImmunity) ? (this as TypeImmunity).Clone()
                    : (this is Volatile) ?
                        (this is Disable) ? (this as Disable).Clone()
                        : (this is Encore) ? (this as Encore).Clone()
                        : (this is Embargo) ? (this as Embargo).Clone()
                        : (this is HealBlock) ? (this as HealBlock).Clone()
                        : (this is Taunt) ? (this as Taunt).Clone()
                        : (this is Torment) ? (this as Torment).Clone()
                        : (this as Volatile).Clone()
                    : (this is Yawn) ? (this as Yawn).Clone()
                    : new PokemonSE(
                        effectType: effectType,
                        timing: timing,
                        isStackable: isStackable,
                        filters: filters
                        );
            }
        }

        /// <summary>
        /// Flags this status as a bound status, preventing this Pokémon from switching out as long as its
        /// trapper is still in battle.
        /// </summary>
        public class Bound : PokemonSE
        {
            /// <summary>
            /// The priority value of this status condition. Low priority bound statuses can't overwrite 
            /// higher priority ones.
            /// </summary>
            public int priority;

            /// <summary>
            /// The text that displays when this status condition negates a lower priority one.
            /// </summary>
            public string negateTextID;

            public Bound(
                int priority = 1,
                string negateTextID = null
                )
                : base(PokemonSEType.Bound)
            {
                this.priority = priority;
                this.negateTextID = negateTextID;
            }
            public new Bound Clone()
            {
                return new Bound(priority: priority, negateTextID: negateTextID);
            }
        }

        /// <summary>
        /// Scales the user's stats.
        /// </summary>
        public class Burn : PokemonSE
        {
            /// <summary>
            /// The stats to scale.
            /// </summary>
            public General.StatScale statScale;

            public Burn(
                General.StatScale statScale
                )
                : base(effectType: PokemonSEType.Burn)
            {
                this.statScale = statScale.Clone();
            }
            public new Burn Clone()
            {
                return new Burn(
                    statScale: statScale
                    );
            }
        }

        /// <summary>
        /// Scales the damage of the user's rolling attacks.
        /// </summary>
        public class DefenseCurl : PokemonSE
        {
            /// <summary>
            /// The factor by which to scale damage.
            /// </summary>
            public float damageScale;

            public DefenseCurl(
                float damageScale = 2f
                ) 
                : base(effectType: PokemonSEType.DefenseCurl)
            {
                this.damageScale = damageScale;
            }
            public new DefenseCurl Clone()
            {
                return new DefenseCurl(damageScale: damageScale);
            }
        }

        /// <summary>
        /// Prevents this Pokemon from using its last used move for a few turns.
        /// </summary>
        public class Disable : MoveLimiting
        {
            public Disable(
                string startText = "status-disable-start", string endText = "status-disable-end",
                string alreadyText = "status-disable-already", string failText = "status-disable-fail",
                string attemptText = "status-disable-attempt",
                General.DefaultTurns defaultTurns = null
                )
                : base(
                      effectType: PokemonSEType.Disable,
                      startText: startText, endText: endText,
                      alreadyText: alreadyText, failText: failText,
                      attemptText: attemptText,
                      defaultTurns: defaultTurns
                      )
            {

            }
            public new Disable Clone()
            {
                return new Disable(
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    );
            }
        }

        /// <summary>
        /// Forces moves used by this Pokémon to be a certain type.
        /// </summary>
        public class Electrify : PokemonSE
        {
            /// <summary>
            /// The type that this Pokémon's moves are forced to be.
            /// </summary>
            public string moveType;
            /// <summary>
            /// The text displayed when this effect is successful.
            /// </summary>
            public string displayText;

            public Electrify(
                string moveType = "electric", 
                string displayText = "status-electrify") 
                : base(effectType: PokemonSEType.Electrify)
            {
                this.moveType = moveType;
                this.displayText = displayText;
            }
            public new Electrify Clone()
            {
                return new Electrify(moveType: moveType, displayText: displayText);
            }
        }

        /// <summary>
        /// Prevents this Pokemon from any other move but its encored move for a few turns.
        /// </summary>
        public class Embargo : Volatile
        {
            /// <summary>
            /// The text displayed when an item is blocked from being used.
            /// </summary>
            public string attemptText;

            public Embargo(
                string startText = "status-embargo-start", string endText = "status-embargo-end",
                string alreadyText = "status-embargo-already", string failText = "status-embargo-fail",
                string attemptText = "status-embargo-attempt",
                General.DefaultTurns defaultTurns = null
                )
                : base(
                      effectType: PokemonSEType.Embargo,
                      startText: startText, endText: endText,
                      alreadyText: alreadyText, failText: failText,
                      defaultTurns: defaultTurns
                      )
            {
                this.attemptText = attemptText;
            }
            public new Embargo Clone()
            {
                return new Embargo(
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    );
            }
        }

        /// <summary>
        /// Prevents this Pokemon from any other move but its encored move for a few turns.
        /// </summary>
        public class Encore : MoveLimiting
        {
            public Encore(
                string startText = "status-encore-start", string endText = "status-encore-end",
                string alreadyText = "status-encore-already", string failText = "status-encore-fail",
                string attemptText = "status-encore-attempt",
                General.DefaultTurns defaultTurns = null
                )
                : base(
                      effectType: PokemonSEType.Encore,
                      startText: startText, endText: endText,
                      alreadyText: alreadyText, failText: failText,
                      attemptText: attemptText,
                      defaultTurns: defaultTurns
                      )
            {

            }
            public new Encore Clone()
            {
                return new Encore(
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    );
            }
        }

        /// <summary>
        /// Prevents the Pokémon from using their move during the rest of the turn.
        /// </summary>
        public class Flinch : PokemonSE
        {
            /// <summary>
            /// The text that displays when the Pokémon flinches.
            /// </summary>
            public string flinchText;

            public Flinch(string displayText = "status-flinch") : base(effectType: PokemonSEType.Flinch)
            {
                this.flinchText = displayText;
            }
            public new Flinch Clone()
            {
                return new Flinch(displayText: flinchText);
            }
        }

        /// <summary>
        /// May prevent this Pokémon from attacking.
        /// </summary>
        public class Freeze : PokemonSE
        {
            /// <summary>
            /// The chance of full paralysis when attacking.
            /// </summary>
            public float thawChance;
            /// <summary>
            /// The text displayed when frozen.
            /// </summary>
            public string displayText;

            /// <summary>
            /// The move types that thaw
            /// </summary>
            public List<string> thawMoveTypes;

            public Freeze(
                float chance = 0.2f,
                string displayText = "status-freeze",
                IEnumerable<string> thawMoveTypes = null
                )
                : base(effectType: PokemonSEType.Freeze)
            {
                this.thawChance = chance;
                this.displayText = displayText;
                this.thawMoveTypes = (thawMoveTypes == null) ? new List<string>() : new List<string>(thawMoveTypes);
            }
            public new Freeze Clone()
            {
                return new Freeze(
                    chance: thawChance, 
                    displayText: displayText,
                    thawMoveTypes: thawMoveTypes);
            }
        }

        /// <summary>
        /// Prevents this Pokemon from using healing moves for a few turns.
        /// </summary>
        public class HealBlock : MoveLimiting
        {
            public HealBlock(
                string startText = "status-healblock-start", string endText = "status-healblock-end",
                string alreadyText = "status-healblock-already", string failText = "status-healblock-fail",
                string attemptText = "status-healblock-attempt",
                General.DefaultTurns defaultTurns = null
                )
                : base(
                      effectType: PokemonSEType.HealBlock,
                      startText: startText, endText: endText,
                      alreadyText: alreadyText, failText: failText,
                      attemptText: attemptText,
                      defaultTurns: defaultTurns
                      )
            {

            }
            public new HealBlock Clone()
            {
                return new HealBlock(
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    );
            }
        }

        /// <summary>
        /// Damages affected Pokémon by a percentage of their maximum HP.
        /// </summary>
        public class HPLoss : PokemonSE
        {
            /// <summary>
            /// The text to display if Pokémon lose HP.
            /// </summary>
            public string displayText;

            /// <summary>
            /// The percentage of HP lost from this effect.
            /// </summary>
            public float hpLossPercent;
            /// <summary>
            /// If true, the HP Loss stacks in a similar effect every turn like Toxic.
            /// </summary>
            public bool toxicStack;

            public HPLoss(
                string displayText = null,
                float hpLossPercent = 1f / 16,
                bool toxicStack = false,

                PokemonSETiming timing = PokemonSETiming.EndOfTurn,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: PokemonSEType.HPLoss, timing: timing, filters: filters)
            {
                this.displayText = displayText;
                this.hpLossPercent = hpLossPercent;
                this.toxicStack = toxicStack;
            }
            public new HPLoss Clone()
            {
                return new HPLoss(
                    displayText: displayText, hpLossPercent: hpLossPercent, toxicStack: toxicStack,

                    timing: timing, filters: filters
                    );
            }
        }

        /// <summary>
        /// Removes specified type immunities from the Pokémon. Also ignores the Pokémon's positive
        /// evasion stat changes.
        /// </summary>
        public class Identification : PokemonSE
        {
            /// <summary>
            /// The text that displays when the target is identified.
            /// </summary>
            public string identifiedText;
            /// <summary>
            /// The text that displays if this Pokémon was already identified.
            /// </summary>
            public string alreadyText;
            /// <summary>
            /// The types that have their immunities removed.
            /// </summary>
            public List<string> types;

            public Identification(
                string identifiedText = "status-identification", string alreadyText = "status-identification-already",
                IEnumerable<string> types = null
                ) : base(effectType: PokemonSEType.Identified)
            {
                this.identifiedText = identifiedText;
                this.alreadyText = alreadyText;
                this.types = (types == null) ? new List<string>() : new List<string>(types);
            }
            public new Identification Clone()
            {
                return new Identification(
                    identifiedText: identifiedText, alreadyText: alreadyText,
                    types: types
                    );
            }
        }

        /// <summary>
        /// While this status is active, opposing Pokémon cannot use any move that this pokemon knows.
        /// </summary>
        public class Imprison : PokemonSE
        {
            /// <summary>
            /// The text that displays when the pokemon starts imprison.
            /// </summary>
            public string startText;
            /// <summary>
            /// The text that displays when imprison blocks the use of a move.
            /// </summary>
            public string negateText;
            /// <summary>
            /// The text that displays to notify the player that a move can't be used due to Imprison.
            /// </summary>
            public string chooseText;

            public Imprison(
                string startText = "status-imprison",
                string negateText = "status-imprison-negate",
                string chooseText = "status-imprison-choose"
                ) 
                : base(effectType: PokemonSEType.Imprison)
            {
                this.startText = startText;
                this.negateText = negateText;
                this.chooseText = chooseText;
            }
            public new Imprison Clone()
            {
                return new Imprison(startText: startText, negateText: negateText, chooseText: chooseText);
            }
        }

        /// <summary>
        /// The Pokémon can't attack part of the time, even against the Pokémon it is infatuated by.
        /// </summary>
        public class Infatuation : PokemonSE
        {
            /// <summary>
            /// The Pokémon causing infatuation.
            /// </summary>
            public string infatuator;

            /// <summary>
            /// Text displayed when the Pokémon becomes infatuated.
            /// </summary>
            public string startText;
            /// <summary>
            /// Text displayed when the Pokémon is longer infatuated.
            /// </summary>
            public string endText;
            /// <summary>
            /// Text displayed when the Pokémon is about to make a move.
            /// </summary>
            public string moveText;
            /// <summary>
            /// Text displayed when the Pokémon fails to make a move.
            /// </summary>
            public string moveFailText;
            /// <summary>
            /// Text displayed when the Pokémon fails to become infatuated.
            /// </summary>
            public string failText;
            /// <summary>
            /// Text displayed when the Pokémon is already infatuated.
            /// </summary>
            public string alreadyText;

            /// <summary>
            /// The chance of the move failing.
            /// </summary>
            public float moveFailChance;
        
            public Infatuation(
                string infatuator = null,

                string startText = "status-infatuation",
                string endText = "status-infatuation-end",
                string moveText = "status-infatuation-move", string moveFailText = "status-infatuation-movefail",
                string failText = "status-infatuation-fail", string alreadyText = "status-infatuation-already",
                float moveFailChance = 0.5f
                )
                : base(effectType: PokemonSEType.Infatuation)
            {
                this.infatuator = infatuator;
                this.startText = startText;
                this.endText = endText;
                this.moveText = moveText;
                this.moveFailText = moveFailText;
                this.failText = failText;
                this.alreadyText = alreadyText;
                this.moveFailChance = moveFailChance;
            }
            public new Infatuation Clone()
            {
                return new Infatuation(
                    infatuator: infatuator,
                    startText: startText, endText: endText,
                    moveText: moveText, moveFailText: moveFailText, failText: failText, alreadyText: alreadyText,
                    moveFailChance: moveFailChance
                    );
            }
        }

        /// <summary>
        /// Flags this status effect as a move-limiting type status effect (ex. Taunt, Torment, etc.)
        /// </summary>
        public class MoveLimiting : Volatile
        {
            /// <summary>
            /// If true, this move can be used during the turn it becomes limited (but not after).
            /// </summary>
            public bool canUseMiddleOfTurn;

            /// <summary>
            /// The text displayed when the Pokémon attempts to use the blocked move.
            /// </summary>
            public string attemptText;

            public MoveLimiting(
                PokemonSEType effectType,
                string startText = null, string endText = null,
                string alreadyText = null, string failText = null,
                string attemptText = null,
                bool canUseMiddleOfTurn = false,
                General.DefaultTurns defaultTurns = null
                )
                : base(
                      effectType: effectType,
                      startText: startText, endText: endText,
                      alreadyText: attemptText, failText: failText,
                      defaultTurns: defaultTurns)
            {
                this.attemptText = attemptText;
                this.canUseMiddleOfTurn = canUseMiddleOfTurn;
            }
            public new MoveLimiting Clone()
            {
                return new MoveLimiting(
                    effectType: effectType,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    canUseMiddleOfTurn: canUseMiddleOfTurn,
                    defaultTurns: defaultTurns
                    );
            }
        }

        /// <summary>
        /// Flags this status as non-volatile.
        /// </summary>
        public class NonVolatile : PokemonSE
        {
            /// <summary>
            /// The priority value of this status condition. Low priority statuses can't overwrite 
            /// higher priority ones.
            /// </summary>
            public int priority;

            /// <summary>
            /// The text that displays when this status condition negates a lower priority one.
            /// </summary>
            public string negateTextID;

            public NonVolatile(
                int priority = 1,
                string negateTextID = null
                ) 
                : base(PokemonSEType.NonVolatile)
            {
                this.priority = priority;
                this.negateTextID = negateTextID;
            }
            public new NonVolatile Clone()
            {
                return new NonVolatile(priority: priority, negateTextID: negateTextID);
            }
        }

        /// <summary>
        /// Causes stat stage changes every turn that this status is active.
        /// </summary>
        public class Octolock : PokemonSE
        {
            public General.StatStageMod statStageMod;

            public Octolock(
                General.StatStageMod statStageMod,
                PokemonSETiming timing = PokemonSETiming.EndOfTurn,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(
                      effectType: PokemonSEType.Octolock, timing: timing, filters: filters)
            {
                this.statStageMod = statStageMod.Clone();
            }
            public new Octolock Clone()
            {
                return new Octolock(
                    statStageMod: statStageMod,
                    timing: timing, filters: filters
                    );
            }
        }

        /// <summary>
        /// May prevent this Pokémon from attacking.
        /// </summary>
        public class Paralysis : PokemonSE
        {
            /// <summary>
            /// The chance of full paralysis when attacking.
            /// </summary>
            public float chance;
            /// <summary>
            /// The text displayed when fully paralyzed.
            /// </summary>
            public string displayText;

            public Paralysis(
                float chance = 0.25f,
                string displayText = "status-paralysis"
                )
                : base(effectType: PokemonSEType.Paralysis)
            {
                this.chance = chance;
                this.displayText = displayText;
            }
            public new Paralysis Clone()
            {
                return new Paralysis(chance: chance, displayText: displayText);
            }
        }

        /// <summary>
        /// Causes the Pokémon to instantly faint after a certain amount of turns. 
        /// </summary>
        public class PerishSong : PokemonSE
        {
            /// <summary>
            /// The amount of turns left before this Pokémon faints.
            /// </summary>
            public int turnsLeft;

            /// <summary>
            /// The text displayed when Perish Song is inflicted.
            /// </summary>
            public string startText;
            /// <summary>
            /// The text displayed when Perish Song counts down.
            /// </summary>
            public string countText;

            public PerishSong(
                int turnsLeft = 3,
                string startText = "status-perishsong",
                string countText = "status-perishsong-count"
                )
                : base(effectType: PokemonSEType.PerishSong)
            {
                this.turnsLeft = Mathf.Max(0, turnsLeft);
                this.startText = startText;
                this.countText = countText;
            }
            public new PerishSong Clone()
            {
                return new PerishSong(
                    turnsLeft: turnsLeft, 
                    startText: startText, countText: countText);
            }
        }

        /// <summary>
        /// May prevent this Pokémon from attacking.
        /// </summary>
        public class Sleep : PokemonSE
        {
            /// <summary>
            /// The text displayed when fully paralyzed.
            /// </summary>
            public string displayText;

            public Sleep(
                string displayText = "status-sleep"
                )
                : base(effectType: PokemonSEType.Sleep)
            {
                this.displayText = displayText;
            }
            public new Sleep Clone()
            {
                return new Sleep(displayText: displayText);
            }
        }

        /// <summary>
        /// Adds additional type resistances, weaknesses, or immunities to the afflicted Pokémon.
        /// </summary>
        public class TarShot : PokemonSE
        {
            /// <summary>
            /// The unique identifier for this instance of Tar Shot. A Pokémon cannot have multiple Tar Shots
            /// with the same ID.
            /// </summary>
            public string tarShotID;
            /// <summary>
            /// The text displayed when Tar Shot is afflicted to a Pokémon.
            /// </summary>
            public string startText;

            /// <summary>
            /// The additional resistances.
            /// </summary>
            public List<string> resistances;
            /// <summary>
            /// The additional weaknesses.
            /// </summary>
            public List<string> weaknesses;
            /// <summary>
            /// The additional immunities.
            /// </summary>
            public List<string> immunities;

            public TarShot(
                string tarShotID = "tarshot",
                string startText = null,
                IEnumerable<string> resistances = null,
                IEnumerable<string> weaknesses = null,
                IEnumerable<string> immunities = null
                )
                : base(effectType: PokemonSEType.TarShot)
            {
                this.tarShotID = tarShotID;
                this.startText = startText;
                this.resistances = (resistances == null) ? new List<string>() : new List<string>(resistances);
                this.weaknesses = (weaknesses == null) ? new List<string>() : new List<string>(weaknesses);
                this.immunities = (immunities == null) ? new List<string>() : new List<string>(immunities);
            }
            public new TarShot Clone()
            {
                return new TarShot(
                    tarShotID: tarShotID, startText: startText,
                    resistances: resistances, weaknesses: weaknesses, immunities: immunities
                    );
            }

        }

        /// <summary>
        /// Prevents this Pokemon from using status moves for a few turns.
        /// </summary>
        public class Taunt : MoveLimiting
        {
            /// <summary>
            /// The move category that is blocked by Taunt.
            /// </summary>
            public MoveCategory category;

            public Taunt(
                MoveCategory category = MoveCategory.Status,
                string startText = "status-taunt-start", string endText = "status-taunt-end",
                string alreadyText = "status-taunt-already", string failText = "status-taunt-fail",
                string attemptText = "status-taunt-attempt",
                General.DefaultTurns defaultTurns = null
                )
                : base(
                      effectType: PokemonSEType.Taunt,
                      startText: startText, endText: endText,
                      alreadyText: alreadyText, failText: failText,
                      attemptText: attemptText,
                      defaultTurns: defaultTurns
                      )
            {
                this.category = category;
            }
            public new Taunt Clone()
            {
                return new Taunt(
                    category: category,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    );
            }
        }

        /// <summary>
        /// Prevents this Pokemon from using the same move twice in a row.
        /// </summary>
        public class Torment : MoveLimiting
        {
            public Torment(
                string startText = "status-torment-start", string endText = "status-torment-end",
                string alreadyText = "status-torment-already", string failText = "status-torment-fail",
                string attemptText = "status-torment-attempt",
                General.DefaultTurns defaultTurns = null
                )
                : base(
                      effectType: PokemonSEType.Torment,
                      startText: startText, endText: endText,
                      alreadyText: alreadyText, failText: failText,
                      attemptText: attemptText,
                      defaultTurns: defaultTurns
                      )
            {

            }
            public new Torment Clone()
            {
                return new Torment(
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    );
            }
        }

        /// <summary>
        /// Specifies types immune to this status.
        /// </summary>
        public class TypeImmunity : PokemonSE
        {
            public TypeImmunity(
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: PokemonSEType.TypeImmunity, filters: filters)
            {

            }
            public new TypeImmunity Clone()
            {
                return new TypeImmunity(
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Flags this status as volatile. This effect will be applied independently from its parent status condition.
        /// </summary>
        public class Volatile : PokemonSE
        {
            public string startText;
            public string endText;
            public string alreadyText;
            public string failText;
            public General.DefaultTurns defaultTurns;

            public Volatile(
                PokemonSEType effectType,
                string startText = null, string endText = null,
                string alreadyText = null, string failText = null,
                General.DefaultTurns defaultTurns = null
                )
                : base(effectType: effectType)
            {
                this.startText = startText;
                this.endText = endText;
                this.alreadyText = alreadyText;
                this.failText = failText;
                this.defaultTurns = (defaultTurns == null) ? new General.DefaultTurns()
                    : defaultTurns.Clone();
            }
            public new Volatile Clone()
            {
                return new Volatile(
                    effectType: effectType,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    defaultTurns: defaultTurns
                    );
            }
        }

        /// <summary>
        /// The Pokémon becomes drowsy, and falls asleep on the next turn.
        /// </summary>
        public class Yawn : PokemonSE
        {
            /// <summary>
            /// The status inflicted at the end of the Yawn turns.
            /// </summary>
            public string statusID;

            /// <summary>
            /// The turns left for this status effect.
            /// </summary>
            public int turnsLeft;

            /// <summary>
            /// The text displayed when the Pokémon becomes drowsy.
            /// </summary>
            public string startText;
            /// <summary>
            /// The text displayed on the turns the Pokémon is drowsy but is not yet inflicted with the status.
            /// </summary>
            public string waitText;

            public Yawn(
                string statusID = null,
                int turnsLeft = 0,
                string startText = "status-yawn", string waitText = null
                )
                : base(effectType: PokemonSEType.Yawn)
            {
                this.statusID = statusID;
                this.turnsLeft = turnsLeft;
                this.startText = startText;
                this.waitText = waitText;
            }
            public new Yawn Clone()
            {
                return new Yawn(statusID: statusID, turnsLeft: turnsLeft, startText: startText, waitText: waitText);
            }
        }

    }


    // ---TEAM STATUS EFFECTS---
    /// <summary>
    /// Collection of all team-based Status Effects.
    /// </summary>
    public class StatusTEEff
    {
        public class TeamSE
        {
            /// <summary>
            /// The type of battle status effect it is.
            /// </summary>
            public TeamSEType effectType;
            /// <summary>
            /// The time that this effect applies itself.
            /// </summary>
            public TeamSETiming timing;
            /// <summary>
            /// If true, this effect can stack where applicable.
            /// </summary>
            public bool isStackable;

            /// <summary>
            /// Additional restrictions on how the effect is applied.
            /// </summary>
            public List<Filter.FilterEffect> filters;

            public TeamSE(
                TeamSEType effectType,
                TeamSETiming timing = TeamSETiming.Unique,
                bool isStackable = false,
                IEnumerable<Filter.FilterEffect> filters = null
                )
            {
                this.effectType = effectType;
                this.timing = timing;
                this.isStackable = isStackable;
                this.filters = new List<Filter.FilterEffect>();
                if (filters != null)
                {
                    List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                    for (int i = 0; i < tempFilters.Count; i++)
                    {
                        this.filters.Add(tempFilters[i].Clone());
                    }
                }
            }
            public TeamSE Clone()
            {
                return
                    (this is GMaxWildfirePriority) ? (this as GMaxWildfirePriority).Clone()
                    : (this is LightScreen) ? (this as LightScreen).Clone()
                    : (this is HPLoss) ? (this as HPLoss).Clone()
                    :
                    new TeamSE(
                        effectType: effectType,
                        timing: timing,
                        isStackable: isStackable,
                        filters: filters
                        );
            }
        }


        public class GMaxWildfirePriority : TeamSE
        {
            /// <summary>
            /// The priority value of this status condition. Low priority statuses can't overwrite 
            /// higher priority ones.
            /// </summary>
            public int priority;

            /// <summary>
            /// The text that displays when this status condition negates a lower priority one.
            /// </summary>
            public string negateTextID;

            public GMaxWildfirePriority(
                int priority = 1,
                string negateTextID = null
                )
                : base(TeamSEType.GMaxWildfirePriority)
            {
                this.priority = priority;
                this.negateTextID = negateTextID;
            }
            public new GMaxWildfirePriority Clone()
            {
                return new GMaxWildfirePriority(priority: priority, negateTextID: negateTextID);
            }
        }

        /// <summary>
        /// Damages affected Pokémon by a percentage of their maximum HP.
        /// </summary>
        public class HPLoss : TeamSE
        {
            /// <summary>
            /// The text to display if Pokémon lose HP.
            /// </summary>
            public string displayText;

            /// <summary>
            /// The percentage of HP lost from this effect.
            /// </summary>
            public float hpLossPercent;

            public HPLoss(
                string displayText = null,
                float hpLossPercent = 1f / 16,

                TeamSETiming timing = TeamSETiming.EndOfTurn,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: TeamSEType.HPLoss, timing: timing, filters: filters)
            {
                this.displayText = displayText;
                this.hpLossPercent = hpLossPercent;
            }
            public new HPLoss Clone()
            {
                return new HPLoss(
                    displayText: displayText, hpLossPercent: hpLossPercent,

                    timing: timing, filters: filters
                    );
            }
        }

        /// <summary>
        /// Scales damage dealt to Pokémon on this team.
        /// </summary>
        public class LightScreen : TeamSE
        {
            /// <summary>
            /// The amount that damage is scaled by when used against a single target.
            /// </summary>
            public float damageMultiplier;
            /// <summary>
            /// The amount that damage is scaled by when used against multiple targets.
            /// </summary>
            public float damageMultiMultiplier;
            /// <summary>
            /// If true, this effect can be removed by <seealso cref="AbilityEff.ScreenCleaner"/>.
            /// </summary>
            public bool canBeScreenCleaned;
            /// <summary>
            /// If true, this effect can be bypassed by <seealso cref="AbilityEff.Infiltrator"/>.
            /// </summary>
            public bool canBeInfiltrated;

            public LightScreen(
                float damageMultiplier = 0.5f,
                float damageMultiMultiplier = 2f/3,
                bool canBeScreenCleaned = true, bool canBeInfiltrated = true,
                IEnumerable<Filter.FilterEffect> filters = null)
                : base(effectType: TeamSEType.LightScreen, filters: filters)
            {
                this.damageMultiplier = damageMultiplier;
                this.damageMultiMultiplier = damageMultiMultiplier;
                this.canBeScreenCleaned = canBeScreenCleaned;
                this.canBeInfiltrated = canBeInfiltrated;
            }
            public new LightScreen Clone()
            {
                return new LightScreen(
                    damageMultiplier: damageMultiplier,
                    damageMultiMultiplier: damageMultiMultiplier,
                    canBeScreenCleaned: canBeScreenCleaned, canBeInfiltrated: canBeInfiltrated,
                    filters: filters
                    );
            }
        }
    }


    // ---BATTLE STATUS EFFECTS---
    /// <summary>
    /// Collection of all battle-based Status Effects.
    /// </summary>
    public class StatusBTLEff
    {
        public class BattleSE
        {
            /// <summary>
            /// The type of battle status effect it is.
            /// </summary>
            public BattleSEType effectType;
            /// <summary>
            /// The time that this effect applies itself.
            /// </summary>
            public BattleSETiming timing;
            /// <summary>
            /// If true, this effect can stack where applicable.
            /// </summary>
            public bool isStackable;

            /// <summary>
            /// Additional restrictions on how the effect is applied.
            /// </summary>
            public List<Filter.FilterEffect> filters;

            public BattleSE(
                BattleSEType effectType,
                BattleSETiming timing = BattleSETiming.Unique,
                bool isStackable = false,
                IEnumerable<Filter.FilterEffect> filters = null
                )
            {
                this.effectType = effectType;
                this.timing = timing;
                this.isStackable = isStackable;
                this.filters = new List<Filter.FilterEffect>();
                if (filters != null)
                {
                    List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                    for (int i = 0; i < tempFilters.Count; i++)
                    {
                        this.filters.Add(tempFilters[i].Clone());
                    }
                }
            }
            public BattleSE Clone()
            {
                return 
                    (this is BattleEnvironment) ? 
                        (
                        (this is MagicRoom) ? (this as MagicRoom).Clone()
                        : (this is Gravity) ? (this as Gravity).Clone()
                        : (this is Terrain) ? (this as Terrain).Clone()
                        : (this is TrickRoom) ? (this as TrickRoom).Clone()
                        : (this is Weather) ? (this as Weather).Clone()
                        : (this is WonderRoom) ? (this as WonderRoom).Clone()
                        : (this as BattleEnvironment).Clone()
                        )
                    : (this is BlockMoves) ? (this as BlockMoves).Clone()
                    : (this is BlockStatus) ? (this as BlockStatus).Clone()
                    : (this is DesolateLand) ? (this as DesolateLand).Clone()
                    : (this is HPGain) ? (this as HPGain).Clone()
                    : (this is HPLoss) ? (this as HPLoss).Clone()
                    : (this is IonDeluge) ? (this as IonDeluge).Clone()
                    : (this is MoveDamageModifier) ? (this as MoveDamageModifier).Clone()
                    : (this is StatScale) ? (this as StatScale).Clone()
                    : (this is StrongWinds) ? (this as StrongWinds).Clone()
                    : (this is TypeDamageModifier) ? (this as TypeDamageModifier).Clone()
                    : new BattleSE(
                        effectType: effectType,
                        timing: timing,
                        isStackable: isStackable,
                        filters: filters
                        );
            }
        }
    
    
        /// <summary>
        /// Defines a battle environmental factor, such as <seealso cref="Weather"/>, <see cref="Gravity"/>,
        /// or <seealso cref="TrickRoom"/>.
        /// </summary>
        public class BattleEnvironment : BattleSE
        {
            /// <summary>
            /// The priority that this environmental condition has. This condition cannot overwrite conditions
            /// of higher priority. Set to -1 to overwrite any other priority.
            /// </summary>
            public int priority;
            /// <summary>
            /// Text that displays if this condition has negated a lesser priority condition.
            /// </summary>
            public string negateText;
            /// <summary>
            /// The move that Nature Power becomes during this condition. Leaving it to null
            /// makes Nature Power ignore this condition when considering its transformed move.
            /// </summary>
            public string naturePowerMove;

            public BattleEnvironment(
                BattleSEType conditionType = BattleSEType.BattleEnvironment,
                string naturePowerMove = null, int priority = 0, string negateText = null
                )
                : base(effectType: conditionType)
            {
                this.naturePowerMove = naturePowerMove;
                this.priority = priority;
                this.negateText = negateText;
            }
            public new BattleEnvironment Clone()
            {
                return new BattleEnvironment(
                    naturePowerMove: naturePowerMove, 
                    conditionType: effectType, 
                    priority: priority, 
                    negateText: negateText);
            }
        }

        /// <summary>
        /// Makes the Pokémon affected by this battle condition immune to certain moves.
        /// </summary>
        public class BlockMoves : BattleSE
        {
            /// <summary>
            /// Specific moves that are blocked during this condition.
            /// </summary>
            public List<string> moves;
            /// <summary>
            /// Blocks priority moves against affected Pokémon.
            /// </summary>
            public bool psychicTerrain;

            /// <summary>
            /// The text played when a move is successfully blocked by this effect.
            /// </summary>
            public string blockText;

            public BlockMoves(
                IEnumerable<string> moves = null,
                bool psychicTerrain = false,
                string blockText = null,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: BattleSEType.BlockMoves, filters: filters)
            {
                this.moves = (moves == null) ? new List<string>() : new List<string>(moves);
                this.psychicTerrain = psychicTerrain;
                this.blockText = blockText;
            }
            public new BlockMoves Clone()
            {
                return new BlockMoves(
                    moves: moves, psychicTerrain: psychicTerrain, blockText: blockText,
                    filters: filters
                    );
            }
        }

        /// <summary>
        /// Makes the Pokémon affected by this battle condition immune to the given status conditions.
        /// </summary>
        public class BlockStatus : BattleSE
        {
            /// <summary>
            /// The statuses blocked for Pokémon affected by this battle condition.
            /// </summary>
            public List<string> statusIDs;
            /// <summary>
            /// The status effect types blocked for Pokémon affected by this battle condition.
            /// </summary>
            public HashSet<PokemonSEType> SETypes;

            /// <summary>
            /// The text played when a status is successfully blocked by this effect.
            /// </summary>
            public string blockText;

            public BlockStatus(
                IEnumerable<string> statusIDs = null,
                IEnumerable<PokemonSEType> SETypes = null,
                string blockText = null,

                BattleSETiming timing = BattleSETiming.Unique, 
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: BattleSEType.BlockStatus, timing: timing, filters: filters)
            {
                this.statusIDs = (statusIDs == null) ? new List<string>() : new List<string>(statusIDs);
                this.SETypes = (SETypes == null) ? new HashSet<PokemonSEType>() : new HashSet<PokemonSEType>(SETypes);
                this.blockText = blockText;
            }
            public new BlockStatus Clone()
            {
                return new BlockStatus(
                    statusIDs: statusIDs, SETypes: SETypes, blockText: blockText,
                    timing: timing, filters: filters
                    );
            }

        }

        /// <summary>
        /// Negates the use of moves of the given types.
        /// </summary>
        public class DesolateLand : BattleSE
        {
            /// <summary>
            /// The text displayed when a move is negated.
            /// </summary>
            public string negateText;

            /// <summary>
            /// The move types negated.
            /// </summary>
            public List<string> types;
            /// <summary>
            /// Set to true to invert the move types that would be affected.
            /// </summary>
            public bool invert;


            public DesolateLand(
                string negateText,
                IEnumerable<string> types = null, bool invert = false
                )
                : base(effectType: BattleSEType.DesolateLand)
            {
                this.negateText = negateText;
                this.types = (types == null) ? new List<string>() : new List<string>(types);
                this.invert = invert;
            }
            public new DesolateLand Clone()
            {
                return new DesolateLand(negateText: negateText, types: types, invert: invert);
            }
        }

        /// <summary>
        /// Defines a type of gravity setting, such as Gravity.
        /// </summary>
        public class Gravity : BattleEnvironment
        {
            /// <summary>
            /// If true, gravity is intensified while this effect is active.
            /// </summary>
            public bool intensifyGravity;
            /// <summary>
            /// Text displayed when Pokémon become forcefully grounded.
            /// </summary>
            public string groundedText;
            /// <summary>
            /// Text displayed when a move is blocked by gravity.
            /// </summary>
            public string moveFailText;

            public Gravity(
                bool intensifyGravity = false,
                string groundedText = "bStatus-gravity-intensify", string moveFailText = "bStatus-gravity-movefail",

                int priority = 0, string negateText = null
                ) : base(conditionType: BattleSEType.Gravity, priority: priority, negateText: negateText)
            {
                this.intensifyGravity = intensifyGravity;
                this.groundedText = groundedText;
                this.moveFailText = moveFailText;
            }
            public new Gravity Clone()
            {
                return new Gravity(
                    intensifyGravity: intensifyGravity, 
                    groundedText: groundedText, moveFailText: moveFailText,
                    priority: priority, negateText: negateText);
            }
        }

        /// <summary>
        /// Heals affected Pokémon by a percentage of their maximum HP.
        /// </summary>
        public class HPGain : BattleSE
        {
            /// <summary>
            /// The text to display if Pokémon gain HP.
            /// </summary>
            public string displayText;

            /// <summary>
            /// The percentage of HP gained from this effect.
            /// </summary>
            public float hpGainPercent;

            public HPGain(
                string displayText = null,
                float hpGainPercent = 1f / 16,

                BattleSETiming timing = BattleSETiming.EndOfTurn,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: BattleSEType.HPGain, timing: timing, filters: filters)
            {
                this.displayText = displayText;
                this.hpGainPercent = hpGainPercent;
            }
            public new HPGain Clone()
            {
                return new HPGain(
                    displayText: displayText, hpGainPercent: hpGainPercent,

                    timing: timing, filters: filters
                    );
            }
        }

        /// <summary>
        /// Damages affected Pokémon by a percentage of their maximum HP.
        /// </summary>
        public class HPLoss : BattleSE
        {
            /// <summary>
            /// The text to display if Pokémon lose HP.
            /// </summary>
            public string displayText;

            /// <summary>
            /// The percentage of HP lost from this effect.
            /// </summary>
            public float hpLossPercent;

            public HPLoss(
                string displayText = null,
                float hpLossPercent = 1f/16,

                BattleSETiming timing = BattleSETiming.EndOfTurn,
                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: BattleSEType.HPLoss, timing: timing, filters: filters)
            {
                this.displayText = displayText;
                this.hpLossPercent = hpLossPercent;
            }
            public new HPLoss Clone()
            {
                return new HPLoss(
                    displayText: displayText, hpLossPercent: hpLossPercent,

                    timing: timing, filters: filters
                    );
            }

        }

        /// <summary>
        /// While active, changes moves of the given types into a new type.
        /// </summary>
        public class IonDeluge : BattleSE
        {
            /// <summary>
            /// The type that moves are changed into.
            /// </summary>
            public string toType;
            /// <summary>
            /// The types that moves
            /// </summary>
            public List<string> fromTypes;

            public IonDeluge(
                string toType = "electric",
                IEnumerable<string> fromTypes = null
                )
                : base(effectType: BattleSEType.IonDeluge)
            {
                this.toType = toType;
                this.fromTypes = (fromTypes == null) ? new List<string>() : new List<string>(fromTypes);
            }
            public new IonDeluge Clone()
            {
                return new IonDeluge(toType: toType, fromTypes: fromTypes);
            }
        }

        /// <summary>
        /// Defines a type of environment that has an effect on held items, such as Magic Room.
        /// </summary>
        public class MagicRoom : BattleEnvironment
        {
            /// <summary>
            /// If true, held items have their effects suppressed (if possible).
            /// </summary>
            public bool suppressItems;

            public MagicRoom(
                bool suppressItems = false,

                int priority = 0, string negateText = null
                ) : base(conditionType: BattleSEType.MagicRoom, priority: priority, negateText: negateText)
            {
                this.suppressItems = suppressItems;
            }
            public new MagicRoom Clone()
            {
                return new MagicRoom(
                    suppressItems: suppressItems,
                    priority: priority, negateText: negateText
                    );
            }
        }

        /// <summary>
        /// Scales damage done by moves of the specified moves.
        /// </summary>
        public class MoveDamageModifier : BattleSE
        {
            /// <summary>
            /// The amount to scale damage for affected types by.
            /// </summary>
            public float damageScale;
            /// <summary>
            /// The moves affected by the damage scale.
            /// </summary>
            public List<string> moves;

            /// <summary>
            /// If true, a check is performed first to see if the user is affected by the condition.
            /// </summary>
            public bool offensiveCheck;
            /// <summary>
            /// If true, a check is performed first to see if the target is affected by the condition.
            /// </summary>
            public bool defensiveCheck;

            public MoveDamageModifier(
                float damageScale = 1f,
                IEnumerable<string> moves = null,
                bool offensiveCheck = false, bool defensiveCheck = false,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: BattleSEType.MoveDamageModifier, filters: filters)
            {
                this.damageScale = damageScale;
                this.moves = (moves == null) ? new List<string>() : new List<string>(moves);
                this.offensiveCheck = offensiveCheck;
                this.defensiveCheck = defensiveCheck;
            }
            public new MoveDamageModifier Clone()
            {
                return new MoveDamageModifier(
                    damageScale: damageScale, moves: moves,
                    offensiveCheck: offensiveCheck, defensiveCheck: defensiveCheck,
                    filters: filters);
            }
        }

        /// <summary>
        /// Scales certain stats during a battle condition.
        /// </summary>
        public class StatScale : BattleSE
        {
            /// <summary>
            /// The stat being scaled.
            /// </summary>
            public float ATKMod, DEFMod, SPAMod, SPDMod, SPEMod, ACCMod, EVAMod;

            public StatScale(
                float ATKMod = 1f, float DEFMod = 1f, float SPAMod = 1f, float SPDMod = 1f,
                float SPEMod = 1f, float ACCMod = 1f, float EVAMod = 1f,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: BattleSEType.StatScale, filters: filters)
            {
                this.ATKMod = ATKMod;
                this.DEFMod = DEFMod;
                this.SPAMod = SPAMod;
                this.SPDMod = SPDMod;
                this.SPEMod = SPEMod;
                this.ACCMod = ACCMod;
                this.EVAMod = EVAMod;
            }
            public new StatScale Clone()
            {
                return new StatScale(
                    ATKMod: ATKMod, DEFMod: DEFMod, SPAMod: SPAMod, SPDMod: SPDMod,
                    SPEMod: SPEMod, ACCMod: ACCMod, EVAMod: EVAMod,

                    filters: filters
                    );
            }
            public float GetStatMod(PokemonStats statType)
            {
                return (statType == PokemonStats.Attack) ? ATKMod
                    : (statType == PokemonStats.Defense) ? DEFMod
                    : (statType == PokemonStats.SpecialAttack) ? SPAMod
                    : (statType == PokemonStats.SpecialDefense) ? SPDMod
                    : (statType == PokemonStats.Speed) ? SPEMod
                    : (statType == PokemonStats.Accuracy) ? ACCMod
                    : (statType == PokemonStats.Evasion) ? EVAMod
                    : 1f;
            }
        }

        /// <summary>
        /// Changes the effectiveness of moves against the listed types.
        /// </summary>
        public class StrongWinds : BattleSE
        {
            /// <summary>
            /// The text displayed when a move is affected.
            /// </summary>
            public string changeText;

            /// <summary>
            /// The move types affected.
            /// </summary>
            public List<string> types;
            /// <summary>
            /// Set to true to invert the move types that would be affected.
            /// </summary>
            public bool invert;

            /// <summary>
            /// This effect is triggered if the listed types had an effectiveness contained here.
            /// </summary>
            public HashSet<TypeEffectiveness> effectivenessFilter;
            /// <summary>
            /// The effeciveness that overwrites the effectiveness in the effectiveness filter.
            /// </summary>
            public TypeEffectiveness forceEffectiveness;

            public StrongWinds(
                string changeText = null,
                IEnumerable<string> types = null, bool invert = false,
                IEnumerable<TypeEffectiveness> effectivenessFilter = null,
                TypeEffectiveness forceEffectiveness = TypeEffectiveness.Neutral
                )
                : base(effectType: BattleSEType.StrongWinds)
            {
                this.changeText = changeText;
                this.types = (types == null) ? new List<string>() : new List<string>(types);
                this.invert = invert;
                this.effectivenessFilter = (effectivenessFilter == null) ? new HashSet<TypeEffectiveness>()
                    : new HashSet<TypeEffectiveness>(effectivenessFilter);
                this.forceEffectiveness = forceEffectiveness;
            }
            public new StrongWinds Clone()
            {
                return new StrongWinds(
                    changeText: changeText, types: types, invert: invert,
                    effectivenessFilter: effectivenessFilter, forceEffectiveness: forceEffectiveness
                    );
            }
            public float GetEffectiveness()
            {
                return (forceEffectiveness == TypeEffectiveness.Neutral) ? 1f
                    : (forceEffectiveness == TypeEffectiveness.SuperEffective) ? GameSettings.btlSuperEffectivenessMultiplier
                    : (forceEffectiveness == TypeEffectiveness.NotVeryEffective) ? GameSettings.btlNotVeryEffectivenessMultiplier
                    : (forceEffectiveness == TypeEffectiveness.Immune) ? GameSettings.btlImmuneEffectivenessMultiplier
                    : 1f;
            }
        }

        /// <summary>
        /// Defines a type of battle terrain, such as Electric Terrain or Misty Terrain.
        /// </summary>
        public class Terrain : BattleEnvironment
        {
            /// <summary>
            /// The type that a move with the TerranPulse effect becomes. Set to null for no change.
            /// </summary>
            public string terrainPulseType;
            public Terrain(
                string terrainPulseType = null,
                string naturePowerMove = null, int priority = 0, string negateText = null
                ) : base(
                    conditionType: BattleSEType.Terrain,
                    naturePowerMove: naturePowerMove,
                    priority: priority, 
                    negateText: negateText)
            {
                this.terrainPulseType = terrainPulseType;
            }
            public new Terrain Clone()
            {
                return new Terrain(
                    terrainPulseType: terrainPulseType, naturePowerMove: naturePowerMove, 
                    priority: priority, negateText: negateText);
            }
        }

        /// <summary>
        /// Defines a type of environment to determines turn order, such as Trick Room.
        /// </summary>
        public class TrickRoom : BattleEnvironment
        {
            /// <summary>
            /// The stat used to determine turn order.
            /// </summary>
            public PokemonStats speedStat;
            /// <summary>
            /// Set to true to reverse the resulting order.
            /// </summary>
            public bool reverse;

            public TrickRoom(
                PokemonStats speedStat = PokemonStats.Speed,
                bool reverse = false,

                int priority = 0, string negateText = null
                ) : base(conditionType: BattleSEType.TrickRoom, priority: priority, negateText: negateText)
            {
                this.speedStat = speedStat;
                this.reverse = reverse;
            }
            public new TrickRoom Clone()
            {
                return new TrickRoom(
                    speedStat: speedStat, reverse: reverse, 
                    priority: priority, negateText: negateText
                    );
            }
        }

        /// <summary>
        /// Scales damage done by moves of the specified types.
        /// </summary>
        public class TypeDamageModifier : BattleSE
        {
            /// <summary>
            /// The amount to scale damage for affected types by.
            /// </summary>
            public float damageScale;
            /// <summary>
            /// The types affected by the damage scale.
            /// </summary>
            public List<string> types;
            /// <summary>
            /// Set to true to invert the types that would be affected by the damage scale.
            /// </summary>
            public bool invert;

            /// <summary>
            /// If true, a check is performed first to see if the user is affected by the condition.
            /// </summary>
            public bool offensiveCheck;
            /// <summary>
            /// If true, a check is performed first to see if the target is affected by the condition.
            /// </summary>
            public bool defensiveCheck;
            

            public TypeDamageModifier(
                float damageScale = 1f,
                IEnumerable<string> types = null,
                bool invert = false,
                bool offensiveCheck = false, bool defensiveCheck = false,

                IEnumerable<Filter.FilterEffect> filters = null
                )
                : base(effectType: BattleSEType.TypeDamageModifier, filters: filters)
            {
                this.damageScale = damageScale;
                this.types = (types == null) ? new List<string>() : new List<string>(types);
                this.invert = invert;
                this.offensiveCheck = offensiveCheck;
                this.defensiveCheck = defensiveCheck;
            }
            public new TypeDamageModifier Clone()
            {
                return new TypeDamageModifier(
                    damageScale: damageScale, types: types, invert: invert,
                    offensiveCheck: offensiveCheck, defensiveCheck: defensiveCheck,
                    filters: filters);
            }
        }

        /// <summary>
        /// Defines a type of battle weather, such as Harsh Sunlight, Heavy Rain, or Sandstorm.
        /// </summary>
        public class Weather : BattleEnvironment
        {
            /// <summary>
            /// The type that a move with the WeatherBall effect becomes. Set to null for no change.
            /// </summary>
            public string weatherBallType;
            /// <summary>
            /// Set to true to activate any applicable boosts to Weather Ball.
            /// </summary>
            public bool weatherBallBoost;

            public Weather(
                string weatherBallType = null, bool weatherBallBoost = true,
                int priority = 0, string negateText = null
                ) : base(conditionType: BattleSEType.Weather, priority: priority, negateText: negateText)
            {
                this.weatherBallType = weatherBallType;
                this.weatherBallBoost = weatherBallBoost;
            }
            public new Weather Clone()
            {
                return new Weather(
                    weatherBallType: weatherBallType, weatherBallBoost: weatherBallBoost,
                    priority: priority, negateText: negateText);
            }
        }

        /// <summary>
        /// Defines a type of environment that remaps stats, such as Wonder Room.
        /// </summary>
        public class WonderRoom : BattleEnvironment
        {
            public PokemonStats ATKMap, DEFMap, SPAMap, SPDMap, SPEMap;

            public WonderRoom(
                PokemonStats ATKMap = PokemonStats.Attack, PokemonStats DEFMap = PokemonStats.Defense,
                PokemonStats SPAMap = PokemonStats.SpecialAttack, PokemonStats SPDMap = PokemonStats.SpecialDefense,
                PokemonStats SPEMap = PokemonStats.Speed,

                int priority = 0, string negateText = null
                ) : base(conditionType: BattleSEType.WonderRoom, priority: priority, negateText: negateText)
            {
                this.ATKMap = ATKMap;
                this.DEFMap = DEFMap;
                this.SPAMap = SPAMap;
                this.SPDMap = SPDMap;
                this.SPEMap = SPEMap;
            }
            public new WonderRoom Clone()
            {
                return new WonderRoom(
                    ATKMap: ATKMap, DEFMap: DEFMap, 
                    SPAMap: SPAMap, SPDMap: SPDMap, 
                    SPEMap: SPEMap,
                    priority: priority, negateText: negateText);
            }
            public PokemonStats GetMappedStat(PokemonStats statType)
            {
                return (statType == PokemonStats.Attack) ? ATKMap
                    : (statType == PokemonStats.Defense) ? DEFMap
                    : (statType == PokemonStats.SpecialAttack) ? SPAMap
                    : (statType == PokemonStats.SpecialDefense) ? SPDMap
                    : (statType == PokemonStats.Speed) ? SPEMap
                    : statType;
            }
        }
    }

}
