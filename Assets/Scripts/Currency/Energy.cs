namespace ZombieSurvival.General
{
    [System.Serializable]
    public class Energy : Currency
    {
        public Energy(CurrencyData data, int value) : base(data, value)
        {
        }
    }
}