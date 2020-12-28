using PBS.Databases;

namespace PBS.Main.Pokemon
{
    public class AbilityEffectPair
    {
        public Ability ability;
        public Effects.AbilityEff.AbilityEffect effect;
        public AbilityEffectPair(
            Ability ability,
            Effects.AbilityEff.AbilityEffect effect)
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