namespace DataMatrixGenerator
{
    public static class Extensions
    {
        public static bool IsNumber(this char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}
