using System.Collections.Generic;

namespace RPG.Stats
{
    interface IModifiterProvider
    {
        IEnumerable<float> GetAdditiveModifiters(Stats stats);
        IEnumerable<float> GetPercentageModifiters(Stats stats);
    }
}
