using PBS.Databases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Data
{
    public class Item
    {
        // General
        public string ID { get; set; }
        public string baseID { get; private set; }
        private string p_itemName;
        public string itemName
        {
            get
            {
                if (string.IsNullOrEmpty(p_itemName) && !string.IsNullOrEmpty(baseID))
                {
                    return Items.instance.GetItemData(baseID).itemName;
                }
                return p_itemName;
            }
            private set
            {
                p_itemName = value;
            }
        }
        public ItemPocket pocket { get; set; }
        public ItemBattlePocket battlePocket { get; set; }

        // Aesthetics
        private bool useBaseAesthetic;

        private string p_displayID;
        public string displayID
        {
            get
            {
                if (useBaseAesthetic && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).displayID;
                }
                return string.IsNullOrEmpty(p_displayID) ? ID : p_displayID;
            }
            private set
            {
                p_displayID = value;
            }
        }

        private string p_bagDisplayID;
        public string bagDisplayID
        {
            get
            {
                if (useBaseAesthetic && !string.IsNullOrEmpty(baseID))
                {
                    return Items.instance.GetItemData(baseID).bagDisplayID;
                }
                return string.IsNullOrEmpty(p_bagDisplayID) ? p_displayID : p_bagDisplayID;
            }
            private set
            {
                p_bagDisplayID = value;
            }
        }

        // Tags
        private bool combineBaseTags;
        private HashSet<ItemTag> p_tags { get; set; }
        public HashSet<ItemTag> tags
        {
            get
            {
                if (combineBaseTags && !string.IsNullOrEmpty(baseID))
                {
                    HashSet<ItemTag> unionTags = new HashSet<ItemTag>(p_tags);
                    unionTags.UnionWith(Items.instance.GetItemData(baseID).tags);
                    return unionTags;
                }
                return p_tags;
            }
            set
            {
                p_tags = value;
            }
        }

        // Move Effects
        public ItemEffect[] effects { get; private set; }

        // New Effects
        private bool combineBaseEffects;
        private List<Databases.Effects.Items.ItemEffect> p_effectsNew { get; set; }
        public List<Databases.Effects.Items.ItemEffect> effectsNew
        {
            get
            {
                if (combineBaseEffects && !string.IsNullOrEmpty(baseID))
                {
                    List<Databases.Effects.Items.ItemEffect> unionEffects = new List<Databases.Effects.Items.ItemEffect>();
                    unionEffects.AddRange(p_effectsNew);
                    unionEffects.AddRange(Items.instance.GetItemData(baseID).effectsNew);
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
        public Item(string ID,
            string baseID = null,
            string itemName = null,
            ItemPocket pocket = ItemPocket.None,
            ItemBattlePocket battlePocket = ItemBattlePocket.None,

            bool useBaseAesthetic = false, string displayID = null, string bagDisplayID = null,

            bool combineBaseTags = false,
            IEnumerable<ItemTag> tags = null,
            ItemEffect[] effects = null,

            bool combineBaseEffects = false,
            Databases.Effects.Items.ItemEffect[] effectsNew = null)
        {
            this.ID = ID;
            this.baseID = baseID;
            this.itemName = itemName;
            this.pocket = pocket;
            this.battlePocket = battlePocket;

            this.useBaseAesthetic = useBaseAesthetic;
            this.displayID = displayID;
            this.bagDisplayID = bagDisplayID;

            this.combineBaseTags = combineBaseTags;
            this.tags = new HashSet<ItemTag>();
            if (tags != null)
            {
                this.tags.UnionWith(tags);
            }

            this.effects = effects == null ? new ItemEffect[0] : new ItemEffect[effects.Length];
            if (effects != null)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    this.effects[i] = ItemEffect.Clone(effects[i]);
                }
            }

            this.combineBaseEffects = combineBaseEffects;
            this.effectsNew = new List<Databases.Effects.Items.ItemEffect>();
            if (effectsNew != null)
            {
                List<Databases.Effects.Items.ItemEffect> addableEffects = new List<Databases.Effects.Items.ItemEffect>();
                for (int i = 0; i < effectsNew.Length; i++)
                {
                    addableEffects.Add(effectsNew[i].Clone());
                }
                this.effectsNew = addableEffects;
            }
        }

        public bool HasTag(ItemTag tag)
        {
            return tags.Contains(tag);
        }

        public ItemEffect GetEffect(ItemEffectType effectType)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].effectType == effectType)
                {
                    return effects[i];
                }
            }
            return null;
        }

        public List<ItemEffect> GetEffects(ItemEffectType effectType)
        {
            List<ItemEffect> effects = new List<ItemEffect>();
            for (int i = 0; i < this.effects.Length; i++)
            {
                if (this.effects[i].effectType == effectType)
                {
                    effects.Add(this.effects[i]);
                }
            }
            return effects;
        }

        public List<Databases.Effects.Items.ItemEffect> GetEffectsNew(ItemEffectType effectType)
        {
            List<Databases.Effects.Items.ItemEffect> effects = new List<Databases.Effects.Items.ItemEffect>();
            for (int i = 0; i < effectsNew.Count; i++)
            {
                if (effectsNew[i].effectType == effectType)
                {
                    effects.Add(effectsNew[i]);
                }
            }
            return effects;
        }
        public Databases.Effects.Items.ItemEffect GetEffectNew(ItemEffectType effectType)
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
        public List<Databases.Effects.Items.ItemEffect> GetEffectsOnConsume()
        {
            List<Databases.Effects.Items.ItemEffect> consumeEffects = new List<Databases.Effects.Items.ItemEffect>();
            for (int i = 0; i < effectsNew.Count; i++)
            {
                if (effectsNew[i].applyOnConsume)
                {
                    consumeEffects.Add(effectsNew[i]);
                }
            }
            return consumeEffects;
        }
        public List<Databases.Effects.Items.ItemEffect> GetEffectsOnUse()
        {
            List<Databases.Effects.Items.ItemEffect> useEffects = new List<Databases.Effects.Items.ItemEffect>();
            for (int i = 0; i < effectsNew.Count; i++)
            {
                if (effectsNew[i].applyOnUse)
                {
                    useEffects.Add(effectsNew[i]);
                }
            }
            return useEffects;
        }
    }
}