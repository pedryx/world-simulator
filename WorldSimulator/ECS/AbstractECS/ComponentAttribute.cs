using System;

namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Attribute used to mark components. Some ECS libraries may need to know beforehand which
/// types are components
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
internal class ComponentAttribute : Attribute { }
