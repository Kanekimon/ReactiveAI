﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

