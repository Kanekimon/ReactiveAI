public enum ResourceType
{
    Stone,
    Wood,
    Food
}

public class ResourceTarget : DetectableTarget
{
    public ResourceType ResourceType { get; set; }
}

