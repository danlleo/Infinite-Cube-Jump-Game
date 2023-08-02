using System;

public class OnCubeGroundedArgs : EventArgs
{
    public int ScoreToAdd;

    public OnCubeGroundedArgs(int scoreToAdd)
    {
        ScoreToAdd = scoreToAdd;
    }
}
