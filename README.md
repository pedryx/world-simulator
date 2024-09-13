# ECS Benchmark

## Overview

This project measures the performance of different Entity-Component-System (ECS) libraries for C# on concrete game. The project has two parts: the game and the benchmark.

## Game

The game simulates villagers in an open world. Villagers gather resources, and the villages use these resources to build new structures. Villagers have basic AI powered by behavior trees. The world is generated at runtime using fragment and compute shaders.

## Shared Code Base

The project has a shared codebase (namespace `WorldSimulator.ECS.AbstractECS`) that defines interfaces for the ECS libraries. Each library has its own wrapper classes that implement these interfaces. The project focuses on measuring system update times, not the performance of entity creation/destruction or component addition/removal.

The shared codebase may be updated in the future.

## Benchmark

The benchmark measures how fast the game can simulate worlds of different sizes using various ECS libraries. Results are available in `thesis.pdf` and `poster.pdf` (currently in Czech).

## Future Plans

Future updates will include:
- Adding new ECS library variants (e.g., parallel systems)
- Including more ECS libraries
- Expanding game features
- Refactoring code
- Creating a tutorial for adding ECS libraries

## ECS Libraries

Currently, the following ECS libraries are compared (may change in the future):
- [Arch](https://github.com/genaray/Arch)
- [DefaultEcs](https://github.com/Doraku/DefaultEcs)
- [Entitas](https://github.com/sschmid/Entitas)
- [HypEcs](https://github.com/Byteron/HypEcs)
- [Leopotam.Ecs](https://github.com/Leopotam/ecs)
- [Leopotam.EcsLite](https://github.com/Leopotam/ecslite)
- [MonoGame.Extended.Entities](https://github.com/craftworkgames/MonoGame.Extended)
- [RelEcs](https://github.com/Byteron/RelEcs)
- [Svelto.ECS](https://github.com/sebas77/Svelto.ECS)

## ECS Comparisons

This project was inspired by the following repositories:
- [Ecs.CSharp.Benchmark](https://github.com/Doraku/Ecs.CSharp.Benchmark)
- [CSharpECSComparison](https://github.com/Chillu1/CSharpECSComparison)
