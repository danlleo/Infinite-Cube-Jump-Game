using System;

public static class Score
{
    public static int Current { get; private set; }

    public static void Add(int scoreValue)
    {
        if (scoreValue <= 0)
            throw new ArgumentException("Score value cannot be less or equal to zero!");

        Current += scoreValue;
    }
}
