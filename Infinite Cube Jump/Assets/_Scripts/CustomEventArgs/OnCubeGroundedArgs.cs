using System;

public class OnCubeGroundedArgs : EventArgs
{
    public int ScoreToAdd;
    public PlatformLine GroundedPlatformLine;

    public OnCubeGroundedArgs(int scoreToAdd, PlatformLine groundedPlatformLine)
    {
        ScoreToAdd = scoreToAdd;
        GroundedPlatformLine = groundedPlatformLine;
    }
}
