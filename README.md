# Platformer 2D

![Game Demo](https://github.com/Ivaxamaks/SmallPlatformer2D/blob/Develop/Assets/ReadMe/VideoGamePlay.mp4)

## Project Description

**Platformer 2D** is a 2D platformer game where the player controls a character moving through levels, defeating enemies, and collecting ammo. The game features simple physics, procedural level generation, and animations.

## UML Diagram

Below is the UML diagram representing the main structure and relationships of the game components:

![UML Diagram](https://github.com/Ivaxamaks/SmallPlatformer2D/blob/Develop/Assets/ReadMe/UML.png)

## Technologies and Patterns

- **VContainer**: Used for dependency management and injection.
- **UniTask**: Employed for asynchronous programming.
- **EventBus**: Handles events between components.
- **MVC**: Architectural pattern for separating logic, interface, and data.
- **ObjectPool**: Manages the reuse of objects, such as bullets and enemies, to enhance performance.
- **Factory**: Pattern for creating objects, providing flexibility and encapsulating the creation process.
- **StateMachine**: Manages game states, such as initial, running, and ending states.

## Main Classes

### GameLifetimeScope

- **Description**: Responsible for registering and configuring all necessary dependencies in the game. This class serves as the initialization point for all components used throughout the game's lifecycle.

### GameStateMachine

- **Description**: Manages the game states and transitions between them. This allows for the separation of logic for each state and ensures clear game flow management.

#### States:

- **BootstrapState**: Handles the initialization and injection of key scripts such as `UIController`, `Player`, `MapGenerator`, and `SoundManager`.
- **RunGameState**: Contains the main game logic. Initializes the player and all game processes, including enemy spawning, level generation, and `HealthBarManager` control.
- **EndGameState**: Displays the game over screen, including restart and exit options.

### MapGenerator

- **Description**: Provides procedural level generation, adding and removing map chunks based on player movement.

### Enemy

- **Description**: Enemies are created via a Factory that injects dependencies and manages the `MonoBehaviourPool`. This allows efficient management of enemy creation and destruction, utilizing resources effectively.

### GameController

- **Description**: Manages level completion logic. This script handles major events such as `EnemyDiedEvent`, `PlayerAmmoAmountChangedEvent`, and `PlayerDiedEvent`. It is responsible for transitioning between game states, responding to key gameplay events.
