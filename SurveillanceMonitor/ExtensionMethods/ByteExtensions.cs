namespace SurveillanceMonitor.ExtensionMethods
{
    public static class ByteExtensions
    {
        public static byte[] SubArray(this byte[] byteArray , int start, int end)
        {
            var  returnArray = new byte[end-start+1];
            for (int i = start; i <= end; i++)
            {
                returnArray[i - start] = byteArray[i];
            }
            return returnArray;
        }
    }
}
