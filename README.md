:GDD2200 — Choose Your Own Adventure Project

This repository contains an interactive narrative project developed for GDD 2200: Game Design & Development. The purpose of the project is to explore saving, loading,and scriptible objects in the unity game engine

:Project Overview:
This project features a sudo-modular, choice-based text adventure where player decisions determine the direction and outcome of the story. The system is built to support rapid iteration and narrative expansion through data-driven design.

:Key goals include:
- Demonstrating branching narrative design
- Implementing a reusable and extensible dialogue system
- Tracking flags and checkpoints
- Enabling save/load functionality for persistent progress
  
:Core Features:
- Branching Dialogue System:
  Structured node-based system that allows players to select between multiple choices, leading to different dialogue paths and story outcomes.
- Character Profiles
  Each character uses distinct text styling to improve readability and clarity.
- Flag and Checkpoint Management
  A centralized flag system records player decisions and major narrative milestones. Flags serve as both progression variables and save/load checkpoints.
- JSON Save and Load System
  The game automatically restores the player’s most recent checkpoint upon relaunch. All relevant data, including flags and the active dialogue node, is persisted through JSON.


:System is built in the Unity Game engine and includes:
- A Dialogue Manager for controlling narrative flow
- ScriptableObject-based dialogue databases
- UI integration for dialogue text and choice presentation
- Support for dynamic content updates and scene transitions

:Technical Implementation:
Technology Stack
Unity (202x)
C#
ScriptableObjects 
JSON
Custom managers

:Project Structure:

/Assets

  /Scripts
  
    DialogueManager.cs
    DialogueNode.cs
    DialogueChoice.cs
    FlagManager.cs
    SaveSystem.cs
  /Data
  
    DialogueDatabase.asset
    Characters/
    Flags/
  /UI
  
    DialogueCanvas.prefab


:Future Development:
- Voice-over support
- Animated character portraits
- Additional narrative branches

Credits:

Developer: CouldBeDead

Course: GDD 2200 – Game Design & Development

Instructor: JMiller {Toaster Arcade}
               
                     
                                                                    
