using PBS.Data;
using PBS.Databases;

namespace PBS.Main.Pokemon
{
    public class Ability
    {
        public string abilityID;
        public bool activated;
        public bool isSuppressed;
        public int turnsActive;
        public AbilityData data
        {
            get
            {
                return Abilities.instance.GetAbilityData(abilityID);
            }
        }

        public Ability(string abilityID, bool activated = false, bool isSuppressed = false, int turnsActive = 0)
        {
            this.abilityID = abilityID;
            this.activated = activated;
            this.isSuppressed = isSuppressed;
            this.turnsActive = turnsActive;
        }
        public Ability Clone()
        {
            return new Ability(
                abilityID: abilityID,
                activated: activated,
                isSuppressed: isSuppressed,
                turnsActive: turnsActive);
        }
        public Ability TransformClone()
        {
            return new Ability(abilityID: abilityID);
        }

    }
}