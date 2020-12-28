using PBS.Databases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Data
{
    public class BattleStatus
    {
        public string ID { get; private set; }
        public string baseID { get; private set; }
        private string p_conditionName;
        public string conditionName
        {
            get
            {
                if (string.IsNullOrEmpty(p_conditionName) && !string.IsNullOrEmpty(baseID))
                {
                    return BattleStatuses.instance.GetStatusData(baseID).conditionName;
                }
                return p_conditionName;
            }
            private set
            {
                p_conditionName = value;
            }
        }

        private string p_inflictTextID { get; set; }
        public string inflictTextID
        {
            get
            {
                if (string.IsNullOrEmpty(p_inflictTextID) && !string.IsNullOrEmpty(baseID))
                {
                    return BattleStatuses.instance.GetStatusData(baseID).inflictTextID;
                }
                return p_inflictTextID;
            }
            private set
            {
                p_inflictTextID = value;
            }
        }
        private string p_natureTextID { get; set; }
        public string natureTextID
        {
            get
            {
                if (string.IsNullOrEmpty(p_natureTextID))
                {
                    if (!string.IsNullOrEmpty(baseID))
                    {
                        return BattleStatuses.instance.GetStatusData(baseID).natureTextID;
                    }
                    else
                    {
                        return inflictTextID;
                    }
                }
                return p_natureTextID;
            }
            private set
            {
                p_natureTextID = value;
            }
        }
        private string p_endTextID { get; set; }
        public string endTextID
        {
            get
            {
                if (string.IsNullOrEmpty(p_endTextID) && !string.IsNullOrEmpty(baseID))
                {
                    return BattleStatuses.instance.GetStatusData(baseID).endTextID;
                }
                return p_endTextID;
            }
            private set
            {
                p_endTextID = value;
            }
        }
        private string p_alreadyTextID { get; set; }
        public string alreadyTextID
        {
            get
            {
                if (string.IsNullOrEmpty(p_alreadyTextID) && !string.IsNullOrEmpty(baseID))
                {
                    return BattleStatuses.instance.GetStatusData(baseID).alreadyTextID;
                }
                return p_alreadyTextID;
            }
            private set
            {
                p_alreadyTextID = value;
            }
        }

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
        private HashSet<BattleSTag> p_tags { get; set; }
        public HashSet<BattleSTag> tags
        {
            get
            {
                if (combineBaseTags && !string.IsNullOrEmpty(baseID))
                {
                    HashSet<BattleSTag> unionTags = new HashSet<BattleSTag>(p_tags);
                    unionTags.UnionWith(BattleStatuses.instance.GetStatusData(baseID).tags);
                    return unionTags;
                }
                return p_tags;
            }
            private set
            {
                p_tags = value;
            }
        }

        // Condition Effects
        private List<BattleCEff> p_effects { get; set; }
        public List<BattleCEff> effects
        {
            get
            {
                if (combineBaseEffects && !string.IsNullOrEmpty(baseID))
                {
                    List<BattleCEff> unionEffects = new List<BattleCEff>(p_effects);
                    unionEffects.AddRange(BattleStatuses.instance.GetStatusData(baseID).effects);
                    return unionEffects;
                }
                return p_effects;
            }
            private set
            {
                p_effects = value;
            }
        }

        // New Effects
        private bool combineBaseEffects;
        private List<Databases.Effects.BattleStatuses.BattleSE> p_effectsNew { get; set; }
        public List<Databases.Effects.BattleStatuses.BattleSE> effectsNew
        {
            get
            {
                if (combineBaseEffects && !string.IsNullOrEmpty(baseID))
                {
                    List<Databases.Effects.BattleStatuses.BattleSE> unionEffects = new List<Databases.Effects.BattleStatuses.BattleSE>();
                    unionEffects.AddRange(p_effectsNew);
                    unionEffects.AddRange(BattleStatuses.instance.GetStatusData(baseID).effectsNew);
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
        public BattleStatus(
            string ID,
            string baseID = null,
            string conditionName = null,
            string startTextID = null,
            string natureTextID = null,
            string endTextID = null,
            string alreadyTextID = null,
            Databases.Effects.General.DefaultTurns defaultTurns = null,

            bool combineBaseTags = false, IEnumerable<BattleSTag> tags = null,
            bool combineBaseEffects = false, BattleCEff[] effects = null, Databases.Effects.BattleStatuses.BattleSE[] effectsNew = null)
        {
            this.ID = ID;
            this.baseID = baseID;
            this.conditionName = conditionName;
            inflictTextID = startTextID;
            this.natureTextID = natureTextID;
            this.endTextID = endTextID;
            this.alreadyTextID = alreadyTextID;

            this.defaultTurns = defaultTurns == null ? null : defaultTurns.Clone();

            this.combineBaseTags = combineBaseTags;
            this.tags = tags == null ? new HashSet<BattleSTag>() : new HashSet<BattleSTag>(tags);

            this.combineBaseEffects = combineBaseEffects;
            this.effects = new List<BattleCEff>();
            if (effects != null)
            {
                List<BattleCEff> newEffects = new List<BattleCEff>();
                for (int i = 0; i < effects.Length; i++)
                {
                    newEffects.Add(BattleCEff.Clone(effects[i]));
                }
                this.effects = new List<BattleCEff>(newEffects);
            }

            this.effectsNew = new List<Databases.Effects.BattleStatuses.BattleSE>();
            if (effectsNew != null)
            {
                List<Databases.Effects.BattleStatuses.BattleSE> addableEffects = new List<Databases.Effects.BattleStatuses.BattleSE>();
                for (int i = 0; i < effectsNew.Length; i++)
                {
                    addableEffects.Add(effectsNew[i].Clone());
                }
                this.effectsNew = addableEffects;
            }
        }

        // Clone
        public BattleStatus Clone()
        {
            BattleStatus cloneData = new BattleStatus(
                ID: ID,
                baseID: baseID,
                conditionName: conditionName,
                startTextID: inflictTextID,
                natureTextID: natureTextID,
                endTextID: endTextID,
                alreadyTextID: alreadyTextID,

                defaultTurns: defaultTurns,

                tags: tags,
                effects: effects.ToArray(),

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
                BattleStatus baseData = BattleStatuses.instance.GetStatusData(baseID);
                return baseData.IsABaseID(tryBaseID);
            }
            return false;
        }

        public bool HasTag(BattleSTag statusTag)
        {
            return tags.Contains(statusTag);
        }

        public List<BattleCEff> GetEffectsFiltered(BattleSETiming timing)
        {
            List<BattleCEff> effects = new List<BattleCEff>();
            for (int i = 0; i < this.effects.Count; i++)
            {
                if (this.effects[i].effectTiming == timing
                    || this.effects[i].effectTiming == BattleSETiming.Any)
                {
                    effects.Add(this.effects[i]);
                }
            }
            return effects;
        }
        public BattleCEff GetEffect(BattleSEType effectType)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].effectType == effectType)
                {
                    return effects[i];
                }
            }
            return null;
        }
        public List<BattleCEff> GetEffects(BattleSEType effectType)
        {
            List<BattleCEff> effects = new List<BattleCEff>();
            for (int i = 0; i < this.effects.Count; i++)
            {
                if (this.effects[i].effectType == effectType)
                {
                    effects.Add(this.effects[i]);
                }
            }
            return effects;
        }

        public List<Databases.Effects.BattleStatuses.BattleSE> GetEffectsNewFiltered(BattleSETiming timing)
        {
            List<Databases.Effects.BattleStatuses.BattleSE> effects = new List<Databases.Effects.BattleStatuses.BattleSE>();
            for (int i = 0; i < effectsNew.Count; i++)
            {
                if (effectsNew[i].timing == timing)
                {
                    effects.Add(effectsNew[i]);
                }
            }
            return effects;
        }
        public List<Databases.Effects.BattleStatuses.BattleSE> GetEffectsNew(BattleSEType effectType)
        {
            List<Databases.Effects.BattleStatuses.BattleSE> effects = new List<Databases.Effects.BattleStatuses.BattleSE>();
            for (int i = 0; i < effectsNew.Count; i++)
            {
                if (effectsNew[i].effectType == effectType)
                {
                    effects.Add(effectsNew[i]);
                }
            }
            return effects;
        }
        public Databases.Effects.BattleStatuses.BattleSE GetEffectNew(BattleSEType effectType)
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

        public void AddEffects(IEnumerable<Databases.Effects.BattleStatuses.BattleSE> effects, bool before = true)
        {
            if (before)
            {
                List<Databases.Effects.BattleStatuses.BattleSE> unionEffects = new List<Databases.Effects.BattleStatuses.BattleSE>(effects);
                unionEffects.AddRange(p_effectsNew);
                effectsNew = unionEffects;
            }
            else
            {
                p_effectsNew.AddRange(effects);
            }
        }
        public void SetEffects(IEnumerable<Databases.Effects.BattleStatuses.BattleSE> effects)
        {
            effectsNew = new List<Databases.Effects.BattleStatuses.BattleSE>(effects);
        }
    }
}