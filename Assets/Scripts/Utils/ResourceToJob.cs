﻿public static class ResourceToJob
{
    public static JobType GetJobForResource(ResourceType res)
    {
        switch (res)
        {
            case ResourceType.rock:
                return JobType.Miner;
            case ResourceType.tree:
                return JobType.Lumberjack;
            case ResourceType.mushroom:
                return JobType.Farmer;
            case ResourceType.bush:
                return JobType.Farmer;
            case ResourceType.none:
                return JobType.None;
            default:
                return JobType.None;
        }
    }

}

