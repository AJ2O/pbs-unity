namespace PBS.Main.Trainer
{
    public class BattleProperties
    {
        public bool usedMegaEvolution;
        public bool usedZMove;
        public bool usedDynamax;

        public bool usedBallFetch;

        public string failedPokeball;
        public int payDayMoney;

        // Constructor
        public BattleProperties()
        {
            Reset();
        }
        public BattleProperties(Trainer trainer)
        {
            Reset(trainer);
        }

        // Clone
        public BattleProperties Clone(Trainer original)
        {
            BattleProperties clone = new BattleProperties(original);
            clone.usedMegaEvolution = usedMegaEvolution;
            clone.usedZMove = usedZMove;
            clone.usedDynamax = usedDynamax;

            clone.usedBallFetch = usedBallFetch;

            clone.failedPokeball = failedPokeball;
            clone.payDayMoney = payDayMoney;

            return clone;
        }

        public void Reset(Trainer trainer = null)
        {
            usedMegaEvolution = false;
            usedZMove = false;
            usedDynamax = false;

            usedBallFetch = false;

            failedPokeball = null;
            payDayMoney = 0;
        }
    }
}