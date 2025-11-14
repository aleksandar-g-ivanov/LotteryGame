namespace Lottery.Lib.Utils
{

    public class RangeRandomizer : IRangeRandomizer
    {
        Random _rnd;
        public RangeRandomizer()
        {
            _rnd = new Random();
        }      

        public int GetRandomInRange(int from, int to)
        {
            if (from >= to)
            {
                throw new ArgumentException("'from' must be lest then 'to'");
            }

            return _rnd.Next(from, to + 1); //+1 because interval is [from, to)
        }
    }
}
