using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases.Effects.General
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
            this.preForms = preForms == null ? new List<string>() : new List<string>(preForms);
            this.toForm = toForm;
        }
        public FormTransformation(
            string preForm = null, string toForm = null
            )
        {
            preForms = preForm == null ? new List<string>() : new List<string> { preForm };
            this.toForm = toForm;
        }
        public FormTransformation Clone()
        {
            return new FormTransformation(
                preForms: preForms,
                toForm: toForm
                );
        }
        public bool IsPokemonAPreForm(Main.Pokemon.Pokemon pokemon, bool useBasePokemonID = false)
        {
            string pokemonID = useBasePokemonID ? pokemon.basePokemonID : pokemon.pokemonID;
            PokemonData pokemonData = Pokemon.instance.GetPokemonData(pokemonID);

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
        public bool IsPokemonAToForm(Main.Pokemon.Pokemon pokemon, bool useBasePokemonID = false)
        {
            string pokemonID = useBasePokemonID ? pokemon.basePokemonID : pokemon.pokemonID;
            PokemonData pokemonData = Pokemon.instance.GetPokemonData(pokemonID);

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
        public List<PokemonStatuses.PokemonSE> customPokemonEffects;
        /// <summary>
        /// The team-based custom effects that will add or replace the existing effects of the base status.
        /// </summary>
        public List<TeamStatuses.TeamSE> customTeamEffects;
        /// <summary>
        /// The battle-based custom effects that will add or replace the existing effects of the base status.
        /// </summary>
        public List<BattleStatuses.BattleSE> customBattleEffects;

        public InflictStatus(
            StatusType statusType,
            string statusID,
            string conditionName = null, string startTextID = null, string endTextID = null,

            bool useDefaultTurns = true, bool useTurnRange = false,
            int turns = -1, int lowestTurns = 0, int highestTurns = 0,
            EffectMode effectMode = EffectMode.Additive,

            PokemonStatuses.PokemonSE[] customPokemonEffects = null,
            TeamStatuses.TeamSE[] customTeamEffects = null,
            BattleStatuses.BattleSE[] customBattleEffects = null
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

            this.customPokemonEffects = new List<PokemonStatuses.PokemonSE>();
            if (customPokemonEffects != null)
            {
                for (int i = 0; i < customPokemonEffects.Length; i++)
                {
                    this.customPokemonEffects.Add(customPokemonEffects[i]);
                }
            }

            this.customTeamEffects = new List<TeamStatuses.TeamSE>();
            if (customTeamEffects != null)
            {
                for (int i = 0; i < customTeamEffects.Length; i++)
                {
                    this.customTeamEffects.Add(customTeamEffects[i]);
                }
            }

            this.customBattleEffects = new List<BattleStatuses.BattleSE>();
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
            this.moveTags = moveTags == null ? null : new HashSet<MoveTag>(moveTags);
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

            this.banefulBunker = banefulBunker == null ? null : banefulBunker.Clone();
            this.kingsShield = kingsShield == null ? null : kingsShield.Clone();
            this.spikyShield = spikyShield == null ? null : spikyShield.Clone();
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
        public void AddStatMod(PokemonStats statType, bool maxVal = false, bool minVal = false, int statVal = 0)
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