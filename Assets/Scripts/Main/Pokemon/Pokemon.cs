using PBS.Databases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Main.Pokemon
{
    /// <summary>
    /// This class represents a unique instance of a Pokemon. A Pokemon is uniquely identifiable by 
    /// its <seealso cref="uniqueID"/> value.
    /// </summary>
    public partial class Pokemon
    {

        public enum MegaState
        {
            None,
            Mega,
            Primal
        }
        public enum DynamaxState
        {
            None,
            Dynamax,
            Gigantamax
        }

        // General
        public string uniqueID;
        public string uniqueIDTrunc
        {
            get
            {
                return uniqueID.Substring(0, 8);
            }
        }
        public string basePokemonID;
        public string pokemonID;
        private string p_nickname;
        public string nickname
        {
            get
            {
                if (string.IsNullOrEmpty(p_nickname))
                {
                    return Databases.Pokemon.instance.GetPokemonData(pokemonID).speciesName;
                }
                return p_nickname;
            }
            set
            {
                p_nickname = value;
            }
        }
        public int level;
        public string natureID;
        public PokemonGender gender;
        public PokemonData data
        {
            get
            {
                return Databases.Pokemon.instance.GetPokemonData(pokemonID);
            }
        }

        // Stats
        public int ivHP;
        public int ivATK;
        public int ivDEF;
        public int ivSPA;
        public int ivSPD;
        public int ivSPE;

        public int evHP;
        public int evATK;
        public int evDEF;
        public int evSPA;
        public int evSPD;
        public int evSPE;

        public int currentHP;
        public int maxHP
        {
            get
            {
                PokemonData pokemonData = Databases.Pokemon.instance.GetPokemonData(pokemonID);

                float numerator = (2 * pokemonData.baseHP + ivHP + evHP / 4) * level;
                float result = (numerator / 100 + level + 10) * PokemonNatures.instance.GetNatureData(natureID).HPMod;

                if (dynamaxState != DynamaxState.None)
                {
                    result = result * (1.5f + dynamaxProps.dynamaxLevel * GameSettings.pkmnDynamaxHPLevelBoost);
                }
                return Mathf.FloorToInt(result);
            }
        }
        public int ATK
        {
            get
            {
                if (bProps.ATKOverride > 0)
                {
                    return bProps.ATKOverride;
                }
                if (bProps.tProps != null)
                {
                    return bProps.tProps.ATK;
                }

                float numerator = (2 * data.baseATK + ivATK + evATK / 4) * level;
                float result = (numerator / 100 + 5) * PokemonNatures.instance.GetNatureData(natureID).ATKMod;
                return Mathf.FloorToInt(result);
            }
        }
        public int DEF
        {
            get
            {
                if (bProps.DEFOverride > 0)
                {
                    return bProps.DEFOverride;
                }
                if (bProps.tProps != null)
                {
                    return bProps.tProps.DEF;
                }

                float numerator = (2 * data.baseDEF + ivDEF + evDEF / 4) * level;
                float result = (numerator / 100 + 5) * PokemonNatures.instance.GetNatureData(natureID).DEFMod;
                return Mathf.FloorToInt(result);
            }
        }
        public int SPA
        {
            get
            {
                if (bProps.SPAOverride > 0)
                {
                    return bProps.SPAOverride;
                }
                if (bProps.tProps != null)
                {
                    return bProps.tProps.SPA;
                }

                float numerator = (2 * data.baseSPA + ivSPA + evSPA / 4) * level;
                float result = (numerator / 100 + 5) * PokemonNatures.instance.GetNatureData(natureID).SPAMod;
                return Mathf.FloorToInt(result);
            }
        }
        public int SPD
        {
            get
            {
                if (bProps.SPDOverride > 0)
                {
                    return bProps.SPDOverride;
                }
                if (bProps.tProps != null)
                {
                    return bProps.tProps.SPD;
                }

                float numerator = (2 * data.baseSPD + ivSPD + evSPD / 4) * level;
                float result = (numerator / 100 + 5) * PokemonNatures.instance.GetNatureData(natureID).SPDMod;
                return Mathf.FloorToInt(result);
            }
        }
        public int SPE
        {
            get
            {
                if (bProps.SPEOverride > 0)
                {
                    return bProps.SPEOverride;
                }
                if (bProps.tProps != null)
                {
                    return bProps.tProps.SPE;
                }

                float numerator = (2 * data.baseSPE + ivSPE + evSPE / 4) * level;
                float result = (numerator / 100 + 5) * PokemonNatures.instance.GetNatureData(natureID).SPEMod;
                return Mathf.FloorToInt(result);
            }
        }

        public int HPdifference
        {
            get
            {
                return maxHP - currentHP;
            }
        }
        public float HPPercent
        {
            get
            {
                return (float)currentHP / maxHP;
            }
        }

        // Moves
        public Moveslot[] moveslots;

        // Ability
        public int abilityNo;
        public bool isHiddenAbility;

        // Status
        public StatusCondition nonVolatileStatus;

        // Items
        public Item item
        {
            get
            {
                return items.Count > 0 ? items[0] : null;
            }
        }
        public List<Item> items;

        // Mega Properties
        public MegaState megaState;

        // Dynamax Properties
        public DynamaxState dynamaxState;
        public DynamaxProperties dynamaxProps;
        public void ResetDynamaxProps()
        {
            dynamaxState = DynamaxState.None;
            dynamaxProps.turnsLeft = 0;
        }

        // Battle-only Properties
        public bool isInBattleMode;
        public int teamPos;
        public int battlePos;
        public int faintPos;
        public BattleProperties bProps;

        // Constructor
        public Pokemon(
            string pokemonID,
            string basePokemonID = null,
            string uniqueID = "",
            string nickname = null,
            int level = 1,
            string natureID = null,
            string genderID = null,
            float hpPercent = -1f,
            int currentHP = -1,
            int ivHP = -1, int ivATK = -1, int ivDEF = -1, int ivSPA = -1, int ivSPD = -1, int ivSPE = -1,
            int evHP = 0, int evATK = 0, int evDEF = 0, int evSPA = 0, int evSPD = 0, int evSPE = 0,
            Moveslot[] moveslots = null,
            int abilityNo = -1, bool isHiddenAbility = false,
            StatusCondition nonVolatileStatus = null,
            Item item = null,
            MegaState megaState = MegaState.None,
            DynamaxState dynamaxState = DynamaxState.None,
            DynamaxProperties dynamaxProps = null,
            bool checkForm = false
            )
        {
            // general
            if (string.IsNullOrEmpty(uniqueID))
            {
                this.uniqueID = System.Guid.NewGuid().ToString();
            }
            else
            {
                this.uniqueID = uniqueID;
            }
            this.pokemonID = pokemonID;
            this.basePokemonID = string.IsNullOrEmpty(basePokemonID) ? pokemonID : basePokemonID;
            this.nickname = nickname;
            this.level = Mathf.Max(1, level);

            if (natureID == null)
            {
                this.natureID = PokemonNatures.instance.GetRandomNature().ID;
            }
            else
            {
                this.natureID = natureID;
            }

            if (string.IsNullOrEmpty(genderID))
            {
                gender = data.GetRandomGender();
            }
            else
            {
                gender = GameText.ConvertToGender(genderID);
            }

            // stats
            this.ivHP = ivHP < 0 ? GameSettings.pkmnMaxIVValue : ivHP;
            this.ivATK = ivATK < 0 ? GameSettings.pkmnMaxIVValue : ivATK;
            this.ivDEF = ivDEF < 0 ? GameSettings.pkmnMaxIVValue : ivDEF;
            this.ivSPA = ivSPA < 0 ? GameSettings.pkmnMaxIVValue : ivSPA;
            this.ivSPD = ivSPD < 0 ? GameSettings.pkmnMaxIVValue : ivSPD;
            this.ivSPE = ivSPE < 0 ? GameSettings.pkmnMaxIVValue : ivSPE;

            this.evHP = evHP;
            this.evATK = evATK;
            this.evDEF = evDEF;
            this.evSPA = evSPA;
            this.evSPD = evSPD;
            this.evSPE = evSPE;

            if (currentHP < 0)
            {
                this.currentHP = maxHP;
            }
            else
            {
                this.currentHP = currentHP;
            }

            if (hpPercent >= 0)
            {
                this.currentHP = GetHPByPercent(hpPercent);
            }

            this.moveslots = new Moveslot[4];
            if (moveslots != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (moveslots.Length > i)
                    {
                        if (moveslots[i] != null)
                        {
                            this.moveslots[i] = Moveslot.Clone(moveslots[i]);
                        }
                    }
                }
            }

            this.isHiddenAbility = isHiddenAbility;
            this.abilityNo = abilityNo;
            if (abilityNo < 0)
            {
                this.abilityNo = data.GetRandomAbilityIndex(this.isHiddenAbility);
            }

            this.nonVolatileStatus = nonVolatileStatus == null ? new StatusCondition("healthy")
                : StatusCondition.Clone(nonVolatileStatus);

            items = new List<Item>();
            if (item != null)
            {
                items.Add(item.Clone());
            }

            // battle
            isInBattleMode = false;
            faintPos = -1;
            battlePos = -1;
            teamPos = -1;
            bProps = new BattleProperties(this);

            // mega / dynamax
            this.megaState = megaState;
            this.dynamaxState = dynamaxState;
            this.dynamaxProps = dynamaxProps == null ? new DynamaxProperties() : dynamaxProps.Clone();

            if (checkForm)
            {
                CheckForm();
            }
        }

        // Clone
        public static Pokemon Clone(Pokemon original)
        {
            Pokemon clonePokemon = new Pokemon(
                pokemonID: original.pokemonID,
                basePokemonID: original.pokemonID,
                uniqueID: original.uniqueID,
                nickname: original.nickname,
                level: original.level,
                natureID: original.natureID,
                genderID: GameText.ConvertGenderToString(original.gender),

                currentHP: original.currentHP,
                ivHP: original.ivHP, ivATK: original.ivATK, ivDEF: original.ivDEF,
                ivSPA: original.ivSPA, ivSPD: original.ivSPD, ivSPE: original.ivSPE,

                moveslots: original.moveslots,
                abilityNo: original.abilityNo, isHiddenAbility: original.isHiddenAbility,

                nonVolatileStatus: original.nonVolatileStatus,

                item: original.item,
                megaState: original.megaState,
                dynamaxState: original.dynamaxState,
                dynamaxProps: original.dynamaxProps,
                checkForm: false
                );

            // Battle-Only Properties
            clonePokemon.isInBattleMode = original.isInBattleMode;
            clonePokemon.battlePos = original.battlePos;
            clonePokemon.teamPos = original.teamPos;
            clonePokemon.faintPos = original.faintPos;
            clonePokemon.bProps = BattleProperties.Clone(original.bProps, clonePokemon);

            return clonePokemon;
        }

        // Forms
        public void CheckForm()
        {
            AbilityData abilityData = Abilities.instance.GetAbilityData(GetAbility());
            // Items
            if (item != null)
            {
                List<EffectDatabase.ItemEff.ItemEffect> griseousOrbs_ = item.data.GetEffectsNew(ItemEffectType.GriseousOrb);
                for (int i = 0; i < griseousOrbs_.Count; i++)
                {
                    EffectDatabase.ItemEff.GriseousOrb griseousOrb = griseousOrbs_[i] as EffectDatabase.ItemEff.GriseousOrb;
                    PokemonData basePokemonData = Databases.Pokemon.instance.GetPokemonData(griseousOrb.baseFormID);
                    PokemonData toPokemonData = Databases.Pokemon.instance.GetPokemonData(griseousOrb.formID);

                    // Validate if the pokemon is contained
                    if (pokemonID == basePokemonData.ID
                        || pokemonID == toPokemonData.ID
                        || data.IsABaseID(basePokemonData.ID))
                    {
                        bool canTransform = true;

                        // Arceus Plate = Multitype Check
                        if (griseousOrb is EffectDatabase.ItemEff.ArceusPlate)
                        {
                            if (abilityData.GetEffectNew(AbilityEffectType.Multitype) == null)
                            {
                                canTransform = false;
                            }
                        }

                        // RKS Memory = RKS System Check
                        if (griseousOrb is EffectDatabase.ItemEff.RKSMemory)
                        {
                            if (abilityData.GetEffectNew(AbilityEffectType.RKSSystem) != null)
                            {
                                canTransform = false;
                            }
                        }

                        // Mega Stone Requires in-battle activation (not automatic)
                        if (griseousOrb is EffectDatabase.ItemEff.MegaStone)
                        {
                            canTransform = false;
                        }

                        if (canTransform)
                        {
                            if (pokemonID != toPokemonData.ID)
                            {
                                basePokemonID = toPokemonData.ID;
                                pokemonID = toPokemonData.ID;
                                bProps.ResetOverrides(this);
                            }
                        }
                    }
                }
            }

        }

        // Characteristics
        public static bool IsGenderOpposite(PokemonGender gender1, PokemonGender gender2)
        {
            return gender1 == PokemonGender.Male && gender2 == PokemonGender.Female
                || gender1 == PokemonGender.Female && gender2 == PokemonGender.Male;
        }

        // Battle
        public void DisruptBide()
        {
            bProps.bideDamageTaken = 0;
            bProps.bideTurnsLeft = -1;
        }

        // General Methods
        public bool IsTheSameAs(Pokemon other)
        {
            if (other == null) return false;
            return uniqueID == other.uniqueID;
        }
        public bool IsTheSameAs(string uniqueID)
        {
            return this.uniqueID == uniqueID;
        }

        // Health Methods
        public bool IsAbleToBattle()
        {
            return currentHP > 0;
        }
        public void RestoreEverything()
        {
            currentHP = maxHP;
            for (int i = 0; i < moveslots.Length; i++)
            {
                moveslots[i].PP = moveslots[i].maxPP;
            }
        }
        public int GetHPByPercent(float percent)
        {
            if (percent >= 1f)
            {
                return maxHP;
            }
            else if (percent <= 0)
            {
                return 0;
            }
            return Mathf.RoundToInt(maxHP * percent);
        }

        // Stats Methods
        public string GetStatSpreadString()
        {
            return maxHP + "/" + ATK + "/" + DEF + "/" + SPA + "/" + SPD + "/" + SPE;
        }

        // Moveset Methods
        public int GetNumberOfMoves()
        {
            int count = 0;
            for (int i = 0; i < moveslots.Length; i++)
            {
                if (moveslots[i] != null)
                {
                    count++;
                }
            }
            return count;
        }
        public List<Moveslot> GetMoveset()
        {
            List<Moveslot> moveset = new List<Moveslot>();
            for (int i = 0; i < moveslots.Length; i++)
            {
                if (moveslots[i] != null)
                {
                    moveset.Add(moveslots[i]);
                }
            }
            return moveset;
        }
        public int ConsumePP(string moveID, int PPLost)
        {
            for (int i = 0; i < moveslots.Length; i++)
            {
                if (moveslots[i] != null)
                {
                    if (moveslots[i].moveID == moveID)
                    {
                        return ConsumePPFromSlot(moveslots[i], PPLost);
                    }
                }
            }
            return 0;
        }
        public int ConsumePPFromSlot(Moveslot moveslot, int PPLost)
        {
            int realPPLost = PPLost;
            if (PPLost >= moveslot.PP)
            {
                realPPLost = moveslot.PP;
                moveslot.PP = 0;
            }
            else
            {
                moveslot.PP -= PPLost;
            }
            return realPPLost;
        }
        public bool HasMove(string moveID)
        {
            for (int i = 0; i < moveslots.Length; i++)
            {
                if (moveslots[i] != null)
                {
                    if (moveslots[i].moveID == moveID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Ability Methods
        public string GetAbility()
        {
            return data.GetAbilityAtIndex(abilityNo, isHiddenAbility);
        }

        // Status Condition Methods
        public void ApplyStatusCondition(StatusCondition condition)
        {
            if (condition.data.HasTag(PokemonSTag.NonVolatile))
            {
                SetNonVolatileStatus(condition);
            }
            else
            {
                AddStatusCondition(condition);
            }
        }
        public void SetNonVolatileStatus(StatusCondition condition)
        {
            nonVolatileStatus = condition;
        }
        public void UnsetNonVolatileStatus()
        {
            nonVolatileStatus = new StatusCondition("healthy");
        }
        public bool AddStatusCondition(StatusCondition condition)
        {
            if (!HasStatusCondition(condition.statusID))
            {
                bProps.statusConditions.Add(condition);
                return true;
            }
            return false;
        }
        public bool HasStatusCondition(string statusID)
        {
            if (nonVolatileStatus.statusID == statusID)
            {
                return true;
            }
            for (int i = 0; i < bProps.statusConditions.Count; i++)
            {
                if (bProps.statusConditions[i].statusID == statusID)
                {
                    return true;
                }
            }
            return false;
        }
        public void RemoveStatusCondition(string statusID)
        {
            List<StatusCondition> newStatusConditions = new List<StatusCondition>();
            for (int i = 0; i < bProps.statusConditions.Count; i++)
            {
                if (bProps.statusConditions[i].statusID != statusID)
                {
                    newStatusConditions.Add(bProps.statusConditions[i]);
                }
            }
            bProps.statusConditions.Clear();
            bProps.statusConditions.AddRange(newStatusConditions);
        }
        public void UnsetVolatileStatuses()
        {
            bProps.statusConditions.Clear();
        }
        public List<StatusCondition> GetAllStatusConditions()
        {
            List<StatusCondition> conditions = new List<StatusCondition>();
            conditions.Add(nonVolatileStatus);
            conditions.AddRange(bProps.statusConditions);
            return conditions;
        }

        // Item Methods
        public void SetItem(Item item)
        {
            items.Clear();
            if (item != null)
            {
                items.Add(item);
                bProps.unburdenItem = null;
            }
        }
        public void UnsetItem(Item item)
        {
            items.Remove(item);
            if (item == null)
            {
                bProps.unburdenItem = item.itemID;
            }
        }

        // Next Command Properties
        public void SetNextCommand(BattleCommand command)
        {
            bProps.nextCommand = PokemonSavedCommand.CreateSavedCommand(command);
        }
        public void UnsetNextCommand()
        {
            bProps.nextCommand = null;
        }
        public BattleCommand ConvertNextCommand()
        {
            if (bProps.nextCommand != null)
            {
                BattleCommand command = BattleCommand.CreateMoveCommand(
                    this,
                    bProps.nextCommand.moveID,
                    bProps.nextCommand.targetPositions,
                    bProps.nextCommand.isPlayerCommand
                    );
                command.iteration = bProps.nextCommand.iteration;
                command.consumePP = bProps.nextCommand.consumePP;
                command.displayMove = bProps.nextCommand.displayMove;
                return command;
            }
            return null;
        }




    }
}