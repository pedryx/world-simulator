using Svelto.ECS;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WorldSimulator.Components;
using WorldSimulator.Components.Villages;

namespace WorldSimulator.ECS.SveltoECS;

internal static class DescriptorMapper
{
    private static readonly List<(Type[], Type)> map = new()
    {
        (
            new Type[]
            {
                typeof(Location),
                typeof(Appearance),
                typeof(Owner),
                typeof(Health),
                typeof(ItemDrop),
                typeof(Resource),
            },
            typeof(BasicResourceEntityDescriptor)
        ),
        (
            new Type[]
            {
                typeof(Location),
                typeof(Appearance),
                typeof(Owner),
                typeof(Health),
                typeof(ItemDrop),
                typeof(Resource),
                typeof(Movement),
                typeof(AnimalBehavior),
            },
            typeof(DeerEntityDescriptor)
        ),
        (
            new Type[]
            {
                typeof(Location),
                typeof(Appearance),
                typeof(Owner),
                typeof(Building),
                typeof(Inventory),
                typeof(ResourceProcessor),
                typeof(VillagerSpawner),
            },
            typeof(ResourceProcesingBuildingEntityDescriptor)
        ),
        (
            new Type[]
            {
                typeof(Location),
                typeof(Appearance),
                typeof(Owner),
                typeof(Building),
            },
            typeof(MainBuildingEntityDescriptor)
        ),
        (
            new Type[]
            {
                typeof(Location),
                typeof(Appearance),
                typeof(Owner),
                typeof(Building),
                typeof(Inventory),
            },
            typeof(StockpileEntityDescriptor)
        ),
        (
            new Type[]
            {
                typeof(Location),
                typeof(Appearance),
            },
            typeof(TerrainEntityDescriptor)
        ),
        (
            new Type[]
            {
                typeof(Location),
                typeof(Appearance),
                typeof(Owner),
                typeof(Movement),
                typeof(Behavior),
                typeof(VillagerBehavior),
                typeof(Villager),
                typeof(PathFollow),
                typeof(Inventory),
                typeof(DamageDealer),
                typeof(Health),
                typeof(Hunger),
            },
            typeof(VillagerDescriptor)
        ),
        (
            new Type[]
            {
                typeof(Location),
                typeof(Village),
                typeof(Owner),
            },
            typeof(VillageDescriptor)
        ),
    };

    private static readonly Dictionary<Type, ExclusiveGroup> groupMap = new();

    public static void Init()
    {
        groupMap.Clear();

        foreach (var (_, type) in map)
        {
            groupMap.Add(type, new ExclusiveGroup());
        }
    }

    public static Type GetDescriptorType(Type[] types)
    {
        foreach (var (componentTypes, descriptorType) in map)
        {
            if (componentTypes.Length != types.Length)
                continue;

            bool success = true;

            for (int i = 0; i < componentTypes.Length; i++)
            {
                if (componentTypes[i] != types[i])
                {
                    success = false;
                    break;
                }
            }

            if (success)
                return descriptorType;
        }

        throw new InvalidOperationException("No valid entity descriptor for specified component types.");
    }

    public static ExclusiveGroup GetExclusiveGroup(Type descriptorType)
        => groupMap[descriptorType];
}

internal class BasicResourceEntityDescriptor : IEntityDescriptor
{
    public IComponentBuilder[] componentsToBuild { get; } = Array.Empty<IComponentBuilder>();
}

internal class DeerEntityDescriptor :  IEntityDescriptor
{
    public IComponentBuilder[] componentsToBuild { get; } = Array.Empty<IComponentBuilder>();
}

internal class ResourceProcesingBuildingEntityDescriptor : IEntityDescriptor
{
    public IComponentBuilder[] componentsToBuild { get; } = Array.Empty<IComponentBuilder>();
}

internal class MainBuildingEntityDescriptor : IEntityDescriptor
{
    public IComponentBuilder[] componentsToBuild { get; } = Array.Empty<IComponentBuilder>();
}

internal class StockpileEntityDescriptor : IEntityDescriptor
{
    public IComponentBuilder[] componentsToBuild { get; } = Array.Empty<IComponentBuilder>();
}

internal class TerrainEntityDescriptor : IEntityDescriptor
{
    public IComponentBuilder[] componentsToBuild { get; } = Array.Empty<IComponentBuilder>();
}

internal class VillagerDescriptor : IEntityDescriptor
{
    public IComponentBuilder[] componentsToBuild { get; } = Array.Empty<IComponentBuilder>();
}

internal class VillageDescriptor : IEntityDescriptor
{
    public IComponentBuilder[] componentsToBuild { get; } = Array.Empty<IComponentBuilder>();
}