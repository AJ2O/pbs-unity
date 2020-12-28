namespace PBS.Main.Pokemon
{
    public class AbilityEffectPair
    {
        public Ability ability;
        public EffectDatabase.AbilityEff.AbilityEffect effect;
        public AbilityEffectPair(
            Ability ability,
            EffectDatabase.AbilityEff.AbilityEffect effect)
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