# Exploring Options of Entity-Component-System Design Pattern: A Case Study

## Overview

This project explores the Entity-Component-System (ECS) design pattern and evaluates the performance of existing ECS libraries for C# on the concrete game. The primary goal is to compare the benefits and drawbacks of ECS and determine each library's relative performance.

## Game
The game used to test the ECS libraries will be a small one with a large number of entities, making it easy to measure performance.

## Shared Code Base

The project includes the shared codebase (namespace WorldSimulator.ECS.AbstractECS) that defines interfaces for ECS libraries. Each library will have its own wrapper classes that use these interfaces. The focus of this project is on measuring the update time of systems, so the performance of creating/destroying entities or adding/removing components is not measured.

The shared codebase for ECS may be changed in the future.

## ECS Libraries

The following ECS libraries will be compared in this project (may change in the future):
- Arch (https://github.com/genaray/Arch)
- DefaultEcs (https://github.com/Doraku/DefaultEcs)
- Entitas (https://github.com/sschmid/Entitas)
- HypEcs (https://github.com/Byteron/HypEcs)
- Leopotam.Ecs (https://github.com/Leopotam/ecs)
- Leopotam.EcsLite (https://github.com/Leopotam/ecslite)
- MonoGame.Exteded.Entities (https://github.com/craftworkgames/MonoGame.Extended)
- RelEcs (https://github.com/Byteron/RelEcs)
- EnttSharp (https://github.com/RabbitStewDio/EnTTSharp)
- Svetlo.ECS (https://github.com/sebas77/Svelto.ECS)

## ECS Comparisons

This project was inspired by the following repositories:
- https://github.com/Doraku/Ecs.CSharp.Benchmark
- https://github.com/Chillu1/CSharpECSComparison
