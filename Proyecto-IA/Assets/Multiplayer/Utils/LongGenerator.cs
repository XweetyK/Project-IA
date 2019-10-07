using System;

public static class LongGenerator 
{
    static private Random rand;
    static byte[] buffer = new byte[8];
    static LongGenerator() {
        rand = new System.Random(DateTime.Now.Millisecond);
    }
    public static long Range(long min, long max) {
        rand.NextBytes(buffer);
        long longRand = BitConverter.ToInt64(buffer, 0);

        return (Math.Abs(longRand % (max - min)) + min);
    }
}
