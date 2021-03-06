﻿using PBS.Databases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Data
{
    public class PokemonStatus
    {
        // General
        public string ID { get; private set; }
        public string baseID { get; private set; }
        private string p_conditionName;
        public string conditionName
        {
            get
            {
                if (string.IsNullOrEmpty(p_conditionName) && !string.IsNullOrEmpty(baseID))
                {
                    return PokemonStatuses.instance.GetStatusData(baseID).conditionName;
                }
                return p_conditionName;
            }
            private set
            {
                p_conditionName = value;
            }
        }
        public string shortName { get; set; }

        public string inflictTextID { get; set; }
        public string healTextID { get; set; }
        public string alreadyTextID { get; set; }
        public string failTextID { get; set; }

        // Turns
        private Databases.Effects.General.DefaultTurns p_defaultTurns { get; set; }
        public Databases.Effects.General.DefaultTurns defaultTurns
        {
            get
            {
                if (p_defaultTurns == null && !string.IsNullOrEmpty(baseID))
                {
                    return BattleStatuses.instance.GetStatusData(baseID).defaultTurns;
                }
                return p_defaultTurns;
            }
            set
            {
                p_defaultTurns = value;
            }
        }

        // Tags
        private bool combineBaseTags;
        private HashSet<PokemonSTag> p_tags { get; set; }
        public HashSet<PokemonSTag> statusTags
        {
            get
            {
                if (combineBaseTags && !string.IsNullOrEmpty(baseID))
                {
                    HashSet<PokemonSTag> unionTags = new HashSet<PokemonSTag>(p_tags);
                    unionTags.UnionWith(PokemonStatuses.instance.GetStatusData(baseID).statusTags);
                    return unionTags;
                }
                return p_tags;
            }
            private set
            {
                p_tags = value;
            }
        }
        // Status Condition Effects
        public PokemonCEff[] conditionEffects { get; private set; }

        // New Effects
        private bool combineBaseEffects;
        private List<Databases.Effects.PokemonStatuses.PokemonSE> p_effectsNew { get; set; }
        public List<Databases.Effects.PokemonStatuses.PokemonSE> effectsNew
        {
            get
            {
                if (combineBaseEffects && !string.IsNullOrEmpty(baseID))
                {
                    List<Databases.Effects.PokemonStatuses.PokemonSE> unionEffects = new List<Databases.Effects.PokemonStatuses.PokemonSE>();
                    unionEffects.AddRange(p_effectsNew);
                    unionEffects.AddRange(PokemonStatuses.instance.GetStatusData(baseID).effectsNew);
                    return unionEffects;
                }
                return p_effectsNew;
            }
            private set
            {
                p_effectsNew = value;
            }
        }

        // Constructor
        public PokemonStatus(
            string ID,
            string baseID = null,
            string conditionName = null,
            string shortName = null,
            string inflictTextID = null,
            string healTextID = null,
            string alreadyTextID = null,
            string failTextID = null,
            Databases.Effects.General.DefaultTurns defaultTurns = null,

            bool combineBaseTags = false, IEnumerable<PokemonSTag> statusTags = null,
            PokemonCEff[] conditionEffects = null,

            bool combineBaseEffects = false, Databases.Effects.PokemonStatuses.PokemonSE[] effectsNew = null)
        {
            this.ID = ID;
            this.baseID = baseID;
            this.conditionName = conditionName;
            this.shortName = shortName;
            this.inflictTextID = inflictTextID;
            this.healTextID = healTextID;
            this.alreadyTextID = alreadyTextID;
            this.failTextID = failTextID;

            this.defaultTurns = defaultTurns == null ? null : defaultTurns.Clone();

            this.combineBaseTags = combineBaseTags;
            this.statusTags = statusTags == null ? new HashSet<PokemonSTag>() : new HashSet<PokemonSTag>(statusTags);

            this.conditionEffects = conditionEffects == null ? new PokemonCEff[0]
                : new PokemonCEff[conditionEffects.Length];
            if (conditionEffects != null)
            {
                for (int i = 0; i < conditionEffects.Length; i++)
                {
                    this.conditionEffects[i] = PokemonCEff.Clone(conditionEffects[i]);
                }
            }

            this.combineBaseEffects = combineBaseEffects;
            this.effectsNew = new List<Databases.Effects.PokemonStatuses.PokemonSE>();
            if (effectsNew != null)
            {
                List<Databases.Effects.PokemonStatuses.PokemonSE> addableEffects = new List<Databases.Effects.PokemonStatuses.PokemonSE>();
                for (int i = 0; i < effectsNew.Length; i++)
                {
                    addableEffects.Add(effectsNew[i].Clone());
                }
                this.effectsNew = addableEffects;
            }
        }

        // Clone
        public PokemonStatus Clone()
        {
            PokemonStatus cloneData = new PokemonStatus(
                ID: ID,
                baseID: baseID,
                conditionName: conditionName,
                shortName: shortName,
                inflictTextID: inflictTextID,
                alreadyTextID: alreadyTextID,
                healTextID: healTextID,
                failTextID: failTextID,

                defaultTurns: defaultTurns,

                statusTags: statusTags,
                conditionEffects: conditionEffects,

                combineBaseEffects: combineBaseEffects,
                effectsNew: effectsNew.ToArray()
                );
            return cloneData;
        }

        public bool IsABaseID(string tryBaseID)
        {
            if (baseID == tryBaseID)
            {
                return baseID != null;
            }
            if (baseID != null)
            {
                PokemonStatus baseData = PokemonStatuses.instance.GetStatusData(baseID);
                return baseData.IsABaseID(tryBaseID);
            }
            return false;
        }

        public bool HasTag(PokemonSTag statusTag)
        {
            return statusTags.Contains(statusTag);
        }

        public List<PokemonCEff> GetEffectsFiltered(PokemonSETiming timing)
        {
            List<PokemonCEff> effects = new List<PokemonCEff>();
            for (int i = 0; i < conditionEffects.Length; i++)
            {
                if (conditionEffects[i].effectTiming == timing
                    || conditionEffects[i].effectTiming == PokemonSETiming.Any)
                {
                    effects.Add(conditionEffects[i]);
                }
            }
            return effects;
        }
        public PokemonCEff GetEffect(PokemonSEType effectType)
        {
            for (int i = 0; i < conditionEffects.Length; i++)
            {
                if (conditionEffects[i].effectType == effectType)
                {
                    return conditionEffects[i];
                }
            }
            return null;
        }
        public List<PokemonCEff> GetEffects(PokemonSEType effectType)
        {
            List<PokemonCEff> effects = new List<PokemonCEff>();
            for (int i = 0; i < conditionEffects.Length; i++)
            {
                if (conditionEffects[i].effectType == effectType)
                {
                    effects.Add(conditionEffects[i]);
                }
            }
            return effects;
        }

        public List<Databases.Effects.PokemonStatuses.PokemonSE> GetEffectsNewFiltered(PokemonSETiming timing)
        {
            List<Databases.Effects.PokemonStatuses.PokemonSE> effects = new List<Databases.Effects.PokemonStatuses.PokemonSE>();
            for (int i = 0; i < effectsNew.Count; i++)
            {
                if (effectsNew[i].timing == timing)
                {
                    effects.Add(effectsNew[i]);
                }
            }
            return effects;
        }
        public List<Databases.Effects.PokemonStatuses.PokemonSE> GetEffectsNew(PokemonSEType effectType)
        {
            List<Databases.Effects.PokemonStatuses.PokemonSE> effects = new List<Databases.Effects.PokemonStatuses.PokemonSE>();
            for (int i = 0; i < effectsNew.Count; i++)
            {
                if (effectsNew[i].effectType == effectType)
                {
                    effects.Add(effectsNew[i]);
                }
            }
            return effects;
        }
        public Databases.Effects.PokemonStatuses.PokemonSE GetEffectNew(PokemonSEType effectType)
        {
            for (int i = 0; i < effectsNew.Count; i++)
            {
                if (effectsNew[i].effectType == effectType)
                {
                    return effectsNew[i];
                }
            }
            return null;
        }

        public void AddEffects(IEnumerable<Databases.Effects.PokemonStatuses.PokemonSE> effects, bool before = true)
        {
            if (before)
            {
                List<Databases.Effects.PokemonStatuses.PokemonSE> unionEffects = new List<Databases.Effects.PokemonStatuses.PokemonSE>(effects);
                unionEffects.AddRange(p_effectsNew);
                effectsNew = unionEffects;
            }
            else
            {
                p_effectsNew.AddRange(effects);
            }
        }
        public void SetEffects(IEnumerable<Databases.Effects.PokemonStatuses.PokemonSE> effects)
        {
            effectsNew = new List<Databases.Effects.PokemonStatuses.PokemonSE>(effects);
        }
    }
}