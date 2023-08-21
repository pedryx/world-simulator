using System;

namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// The attribute used to mark components. Some ECS libraries may need to know beforehand which types are components.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public class ComponentAttribute : Attribute { }
