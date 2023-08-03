using System;

public class OnPlatformDisappearedArgs : EventArgs
{
    public Platform RecievedPlatform;

    public OnPlatformDisappearedArgs(Platform recievedPlatform) 
        => RecievedPlatform = recievedPlatform;
}
