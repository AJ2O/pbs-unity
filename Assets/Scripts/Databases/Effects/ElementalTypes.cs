using System.Collections.Generic;

namespace PBS.Databases.Effects.ElementalTypes
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