namespace PBS.Main.Pokemon
{
    public class AbilityEffectPair
    {
        public Ability ability;
        public PBS.Databases.Effects.Abilities.AbilityEffect effect;
        public AbilityEffectPair(
            Ability ability,
            PBS.Databases.Effects.Abilities.AbilityEffect effect)
        {
            this.ability = ability;
            this.effect = effect;
        }
        public AbilityEffectPair Clone()
        {
            return new AbilityEffectPair(
                ability: ability.Clone(),
                effect: effect.Clone()
                );
        }
    }
}