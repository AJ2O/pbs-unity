using PBS.Databases;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Data
{
    public class Pokemon
    {
        // General
        public string ID { get; private set; }
        public string baseID { get; private set; }
        public bool isDistinct { get; private set; }
        private string p_speciesName { get; set; }
        public string speciesName
        {
            get
            {
                if (string.IsNullOrEmpty(p_speciesName) && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).speciesName;
                }
                return p_speciesName;
            }
            private set
            {
                p_speciesName = value;
            }
        }
        private int p_pokedexNo { get; set; }
        public int pokedexNo
        {
            get
            {
                if (p_pokedexNo < 0 && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).pokedexNo;
                }
                return p_pokedexNo;
            }
            private set
            {
                p_pokedexNo = value;
            }
        }
        private string p_pokedexCategory { get; set; }
        public string pokedexCategory
        {
            get
            {
                if (string.IsNullOrEmpty(p_pokedexCategory) && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).pokedexCategory;
                }
                return p_pokedexCategory;
            }
            private set
            {
                p_pokedexCategory = value;
            }
        }
        private string p_formName { get; set; }
        public string formName
        {
            get
            {
                if (string.IsNullOrEmpty(p_formName))
                {
                    if (!string.IsNullOrEmpty(baseID))
                    {
                        return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).formName;
                    }
                    else
                    {
                        return "Standard";
                    }
                }
                return p_formName;
            }
            private set
            {
                p_formName = value;
            }
        }
        private bool useBaseMaleRatio;
        private float p_maleRatio { get; set; }
        public float maleRatio
        {
            get
            {
                if (useBaseMaleRatio && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).maleRatio;
                }
                return p_maleRatio;
            }
            private set
            {
                p_maleRatio = value;
            }
        }


        // Physical Traits
        private float p_height { get; set; }
        public float height
        {
            get
            {
                if (p_height < 0 && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).height;
                }
                return p_height;
            }
            private set
            {
                p_height = value;
            }
        }
        private float p_weight { get; set; }
        public float weight
        {
            get
            {
                if (p_weight < 0 && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).weight;
                }
                return p_weight;
            }
            private set
            {
                p_weight = value;
            }
        }

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

        private float p_xOffset2DNear { get; set; }
        public float xOffset2DNear
        {
            get
            {
                if (useBaseAesthetic && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).xOffset2DNear;
                }
                return p_xOffset2DNear;
            }
            private set
            {
                p_xOffset2DNear = value;
            }
        }
        private float p_yOffset2DNear { get; set; }
        public float yOffset2DNear
        {
            get
            {
                if (useBaseAesthetic && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).yOffset2DNear;
                }
                return p_yOffset2DNear;
            }
            private set
            {
                p_yOffset2DNear = value;
            }
        }

        private float p_xOffset2DFar { get; set; }
        public float xOffset2DFar
        {
            get
            {
                if (useBaseAesthetic && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).xOffset2DFar;
                }
                return p_xOffset2DFar;
            }
            private set
            {
                p_xOffset2DFar = value;
            }
        }
        private float p_yOffset2DFar { get; set; }
        public float yOffset2DFar
        {
            get
            {
                if (useBaseAesthetic && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).yOffset2DFar;
                }
                return p_yOffset2DFar;
            }
            private set
            {
                p_yOffset2DFar = value;
            }
        }

        // Type
        private bool useBaseTypes;
        private List<string> p_types { get; set; }
        public List<string> types
        {
            get
            {
                if (useBaseTypes && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).types;
                }
                return p_types;
            }
            private set
            {
                p_types = value;
            }
        }

        // Abilities
        private bool useBaseAbilities;
        private List<string> p_abilities { get; set; }
        public List<string> abilities
        {
            get
            {
                if (useBaseAbilities && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).abilities;
                }
                return p_abilities;
            }
            private set
            {
                p_abilities = value;
            }
        }
        private bool useBaseHiddenAbilities;
        private List<string> p_hiddenAbilities { get; set; }
        public List<string> hiddenAbilities
        {
            get
            {
                if (useBaseHiddenAbilities && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).hiddenAbilities;
                }
                return p_hiddenAbilities;
            }
            private set
            {
                p_hiddenAbilities = value;
            }
        }

        // Stats
        private bool useBaseBaseStats;
        private int p_baseHP { get; set; }
        public int baseHP
        {
            get
            {
                if ((p_baseHP < 0 || useBaseBaseStats) && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).baseHP;
                }
                return p_baseHP;
            }
            private set
            {
                p_baseHP = value;
            }
        }
        private int p_baseATK { get; set; }
        public int baseATK
        {
            get
            {
                if ((p_baseATK < 0 || useBaseBaseStats) && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).baseATK;
                }
                return p_baseATK;
            }
            private set
            {
                p_baseATK = value;
            }
        }
        private int p_baseDEF { get; set; }
        public int baseDEF
        {
            get
            {
                if ((p_baseDEF < 0 || useBaseBaseStats) && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).baseDEF;
                }
                return p_baseDEF;
            }
            private set
            {
                p_baseDEF = value;
            }
        }
        private int p_baseSPA { get; set; }
        public int baseSPA
        {
            get
            {
                if ((p_baseSPA < 0 || useBaseBaseStats) && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).baseSPA;
                }
                return p_baseSPA;
            }
            private set
            {
                p_baseSPA = value;
            }
        }
        private int p_baseSPD { get; set; }
        public int baseSPD
        {
            get
            {
                if ((p_baseSPD < 0 || useBaseBaseStats) && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).baseSPD;
                }
                return p_baseSPD;
            }
            private set
            {
                p_baseSPD = value;
            }
        }
        private int p_baseSPE { get; set; }
        public int baseSPE
        {
            get
            {
                if ((p_baseSPE < 0 || useBaseBaseStats) && !string.IsNullOrEmpty(baseID))
                {
                    return PBS.Databases.Pokemon.instance.GetPokemonData(baseID).baseSPE;
                }
                return p_baseSPE;
            }
            private set
            {
                p_baseSPE = value;
            }
        }

        // Tags
        private bool combineBaseTags;
        private HashSet<PokemonTag> p_tags { get; set; }
        public HashSet<PokemonTag> tags
        {
            get
            {
                if (combineBaseTags && !string.IsNullOrEmpty(baseID))
                {
                    HashSet<PokemonTag> unionTags = new HashSet<PokemonTag>(p_tags);
                    unionTags.UnionWith(PBS.Databases.Pokemon.instance.GetPokemonData(baseID).tags);
                    return unionTags;
                }
                return p_tags;
            }
            private set
            {
                p_tags = value;
            }
        }

        public Pokemon(
            string ID,
            string baseID = null, bool isDistinct = false,
            string speciesName = null,
            int pokedexNo = -1, string pokedexCategory = null,
            string formName = null,
            bool useBaseMaleRatio = false, float maleRatio = 0.5f,
            float height = -1f, float weight = -1f,

            bool useBaseAesthetic = false, string displayID = null,
            float xOffset2DNear = 0, float yOffset2DNear = 0,
            float xOffset2DFar = 0, float yOffset2DFar = 0,

            bool useBaseTypes = false, List<string> types = null,
            bool useBaseAbilities = false, List<string> abilities = null,
            bool useBaseHiddenAbilities = false, List<string> hiddenAbilities = null,

            bool useBaseBaseStats = false,
            int baseHP = -1, int baseATK = -1, int baseDEF = -1, int baseSPA = -1, int baseSPD = -1, int baseSPE = -1,

            bool combineBaseTags = false, IEnumerable<PokemonTag> tags = null
            )
        {
            this.ID = ID;
            this.baseID = baseID;
            this.isDistinct = isDistinct;

            this.speciesName = speciesName;
            this.pokedexNo = pokedexNo;
            this.pokedexCategory = pokedexCategory;
            this.formName = formName;

            this.useBaseMaleRatio = useBaseMaleRatio;
            this.maleRatio = maleRatio;

            this.height = height;
            this.weight = weight;

            this.useBaseAesthetic = useBaseAesthetic;
            this.displayID = displayID;
            this.xOffset2DNear = xOffset2DNear;
            this.yOffset2DNear = yOffset2DNear;
            this.xOffset2DFar = xOffset2DFar;
            this.yOffset2DFar = yOffset2DFar;

            this.useBaseTypes = useBaseTypes;
            this.types = types == null ? new List<string> { "" } : new List<string>(types);
            this.useBaseAbilities = useBaseAbilities;
            this.abilities = abilities == null ? new List<string> { "" } : new List<string>(abilities);
            this.useBaseHiddenAbilities = useBaseHiddenAbilities;
            this.hiddenAbilities = hiddenAbilities == null ? new List<string> { "" } : new List<string>(hiddenAbilities);

            this.useBaseBaseStats = useBaseBaseStats;
            this.baseHP = baseHP;
            this.baseATK = baseATK;
            this.baseDEF = baseDEF;
            this.baseSPA = baseSPA;
            this.baseSPD = baseSPD;
            this.baseSPE = baseSPE;

            this.combineBaseTags = combineBaseTags;
            this.tags = tags == null ? new HashSet<PokemonTag>() : new HashSet<PokemonTag>(tags);
        }

        // Aesthetic Form
        public static Pokemon AestheticVariant(
            string ID,
            string baseID, bool isDistinct = false,
            string formName = null,
            float height = -1f, float weight = -1f,

            bool useBaseAesthetic = false, string displayID = null,
            float xOffset2DNear = 0, float yOffset2DNear = 0,
            float xOffset2DFar = 0, float yOffset2DFar = 0,

            IEnumerable<PokemonTag> tags = null
            )
        {
            Pokemon aestheticData = new Pokemon(
                ID: ID,
                baseID: baseID, isDistinct: isDistinct,
                formName: formName,
                height: height, weight: weight,

                // Aesthetic Differences
                useBaseAesthetic: useBaseAesthetic, displayID: displayID,
                xOffset2DNear: xOffset2DNear, yOffset2DNear: yOffset2DNear,
                xOffset2DFar: xOffset2DFar, yOffset2DFar: yOffset2DFar,

                // Other properties are exactly the same
                useBaseMaleRatio: true,
                useBaseTypes: true,
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true,

                // Tags
                tags: tags
                );
            return aestheticData;
        }

        public bool IsABaseID(string tryBaseID)
        {
            if (isDistinct) { return false; }

            if (baseID == tryBaseID)
            {
                return baseID != null;
            }
            if (baseID != null)
            {
                Pokemon baseData = PBS.Databases.Pokemon.instance.GetPokemonData(baseID);
                return baseData.IsABaseID(tryBaseID);
            }
            return false;
        }

        public PokemonGender GetRandomGender()
        {
            if (maleRatio < 0)
            {
                return PokemonGender.Genderless;
            }
            else if (maleRatio == 0)
            {
                return PokemonGender.Female;
            }
            else if (maleRatio == 1)
            {
                return PokemonGender.Male;
            }
            else
            {
                if (Random.value < maleRatio)
                {
                    return PokemonGender.Male;
                }
                else
                {
                    return PokemonGender.Female;
                }
            }
        }

        public string GetPrimaryType()
        {
            return types.Count > 0 ? types[0] : "";
        }
        public string GetSecondaryType()
        {
            return types.Count > 1 ? types[1] : GetPrimaryType();
        }

        public string GetAbilityAtIndex(int index, bool isHidden)
        {
            List<string> abilityList = isHidden ? hiddenAbilities : abilities;
            if (index < abilityList.Count)
            {
                return abilityList[index];
            }
            return "";
        }
        public int GetRandomAbilityIndex(bool isHidden)
        {
            List<string> abilityList = isHidden ? hiddenAbilities : abilities;
            return Random.Range(0, abilityList.Count);
        }

        public bool HasTag(PokemonTag tag)
        {
            return tags.Contains(tag);
        }
    }
}