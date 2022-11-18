namespace ResponseCache.Service
{
    public class NumberService : INumberService
    {
        public int GetRandomNumber()
        {
            TimeSpan interval = new TimeSpan(0, 0, 2);
            Thread.Sleep(interval);

            Random rnd = new Random();
            int number = rnd.Next();

            return number;
        }
    }
}
