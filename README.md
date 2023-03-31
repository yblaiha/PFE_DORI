# Project DORI: Drones for Operational Rescue and Intervention, Unity Demonstrator
DORI is a student project aimed at developing a system coordinating a swarm of drones to aid firefighters in rescue operations in enclosed environments. This project scope fits especially well in events such as natural disasters and war destructions.

The demonstrator for this project is developed using Unity Simulation.

<center>
<h3>Languages and Tools:</h3>
<p> <a href="https://www.w3schools.com/cs/" target="_blank" rel="noreferrer"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/> </a> <a href="https://unity.com/" target="_blank" rel="noreferrer"> <img src="https://www.vectorlogo.zone/logos/unity3d/unity3d-icon.svg" alt="unity" width="40" height="40"/> </a> </p>
</center>

## Requirements
This project uses the following software and libraries. Previous or later versions support is not guaranteed.

- Unity version 2021.3.16.f1
- OpenCV 4.7.0


## Installation

To install this package, just a few steps are needed :

1. Download this project.
2. Open a compatible Unity Editor.
3. Go to File > Open Project...
4. Select your download location.

## Current contents of this version

#### Simulation of Thermal Camera
This part of the project simulates the functionality of a thermal camera. Add TemperatureManager.cs to all objects that need thermal rendering in the scene, then add a ThermalRenderer.cs to an object.

#### Simple Drone Simulation
This part of the project simulates the movement and control of a single drone. Provides a GoTo function.

#### Communication Simulation between Drones
This part of the project simulates the connection and communication between multiple drones. This simulation takes only into account absorption issues of communications, putting aside reflexion, refraction and diffraction. Allows simple simulated communications with obstacles within a building.

#### LiDAR simulation
This part of the project simulated a 3D LiDAR using raycasting and a simple occupancy map.

### Future Work
Future components to be added to the project include:

Mapping and navigation system for drones ( SLAM )
Improved AI and pathfinding algorithms for the drone swarm
