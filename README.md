# SteamVR Assistant (SVRA)
The SteamVR Assistant is a Unity toolkit for assisting developers creating VR applications using Unity and SteamVR. It aims to help streamline the development process and make VR development more accessible to new developers.

Note: No Vive connected - openapi errors (due to lack of Vive)

# Installation
Preliminary Unity Assets: SteamVR Plugin

Assuming the Unity SteamVR Plugin asset is installed in the current project simply download the SVRA project and drag and drop it into the project. This 

# Example Scenes
## svra_default_setup
A simple default setup of the controllers and SteamVR required prefabs on an empty plane.

## svra_object_playground
A random (somewhat chaotic) collection of all the potential interactions SVRA is capable of.

## svra_projectile_range_example
An example featuring 3 floating targets of varying speeds in which the user is tasked to hit with cubes. 
* Showcases SVRA_ObjectSnapZone and SVRA_ObjectSnapZoneLocation scripts

# Training Scenes
## svra_01_grabble_objects
An example to introduce the controller configuration (the ControllerSetup prefab) and making objects grabbable
 * Showcases the SVRA_GrabbableObject script and ControllerSetup prefab

## svra_02_interaction_objects
An example to introduce the interactive objects and the event bridge
 * Showcases the SVRA_InteractiveObject, SVRA_InteractButton, SVRA_ChangeMaterial and SVRA_EventBridge scripts

## svra_03_interaction_objects_2
An example to show off the ability to disable the interactive and grabbable components of objects and to show off the player resizing script
 * Showcases the SVRA_InteractToggle, SVRA_GrabToggle and SVRA_PlayerResize scripts

## svra_04_grabbable_object_events
An example to show of the event bridge with different types of event triggers (grab and highlight)
 * Showcases alternative use of the SVRA_EventBridge script

## svra_05_interaction_objects_3
An example to show off the ability to clone objects and the projectile shooter system
 * Showcases the SVRA_CopyObject and SVRA_ProjectileShooter scripts

## svra_06_target_range
An example to show how to build the target range example scene using the snap zone hover scripts
 * Showcases the SVRA_HoverSnapZone and SVRA_HoverSnapZonePosition scripts
 
## svra_07_scene_transition
An example to show off how to transition between scenes using the scene transition script
 * Showcases the SVRA_SceneTransition script
 
## svra_08_debugging
An example to introduce the play area modifier script and the frame rate display prefab
 * Showcases the FrameRateCounterPrefab prefab and the SVRA_SVRA_PlayAreaModifier script

# Scene Ideas
The following are a list of potential scene ideas in case you need some ideas to get started:
* Carnival games
* Target range
* Basketball hoop
* 10 pin bowling
* 100 pin bowling
* 10 pin bowling hard mode - the pins move around or hover
* Big red button simulator

# Script Overview
## Key Scripts
* SVRA_GrabbableObject: Makes an object grabbable. 
  * Attach to objects you wish to pick up and interact with
  * Attributes:
    * Highlight Effect: Select the highlight script (or none) to be applied to the object.
    * Snap Point: By default snaps the controller position to the centre of the object on pickup. Local Position can be used to change the snap position.
    * Rotation: Enable or disable rotation for the joint which connects the controller to the grabbed object. Apply Grip And Orientation will orientate the object to the specified Local Orientation.
	
* SVRA_InteractiveObject: Makes an object interactive and open to interaction scripts.

* SVRA_EventBridge: On a specified event (interaction start, grab start, etc.) trigger some attached function(s). 
  * https://docs.google.com/document/d/1tY_tQTMylLhXaqLDtHOcNoCwF-T6UQRthw3m4NSzVwE/edit?usp=sharing

## Other Scripts
* SVRA_ChangeColor: Used to toggle the color of an object
  * Attach to the object to change the color of
  * Setup the number of and colors to cycle through
  * Setup the fuction in the event bridge to trigger the color change on the specified event

* SVRA_ChangeMaterial: Used to toggle the material of an object
  * Setup the same as SVRA_ChangeColor

* SVRA_CopyObject: Allows the creation of a copy of a specified object
  * Add to an object
  * Add the object to be copied to the "Copy Object" parameter in the Inspector window
  * Add the transform (empty gameobject) of the position for the object to be spawned to the "Spawn Point" parameter in the Inspector window
  * Setup the fuction in the event bridge to trigger the scene change on the specified event

* SVRA_GrabToggle: Used to toggle if an can be grabbed or not
  * Attach to the object to toggle grabbable or not
  * Setup the fuction in the event bridge to trigger the grabbable toggle on the specified event   

* SVRA_HoverSnapZone : Used with SVRA_HoverSnapZonePosition
  * Setup object to setup snap zone on 
  * Create a copy of the object to represent the snap zone location (see SVRA_HoverSnapZonePosition for setting up the copy object)
  * Attach the SVRA_HoverSnapZone script to the original object
    * HoverCharacteristics: Can be used to make the object oscillate for a set distance, speed and direction 
  
* SVRA_HoverSnapZonePosition: Used with SVRA_HoverSnapZone
  * On the object copy (mentioned above) remove any rigid bodies and additional scripts attached to the object
  * Disable the Mesh Renderer of the object
  * Enable the "Is Trigger" checkbox of the collider of the object
  * Position the object where you want the object to snap to
  * Attach the SVRA_HoverSnapZonePosition script
  * Drag and drop the SVRA_HoverSnapZone object into the "Snap Zone Transform" setting

* SVRA_InteractButton: Used to attach an animation motion to an interact button
  * Attach to the part of the object to animate
  * Setup animation properties in the Inspector window

* SVRA_InteractSwitch: Used to setup a light switch style toggle for a button
  * Attach to the part of the object to animate
 
 * SVRA_InteractToggle: Used to toggle if an object can be interacted with or not
  * Setup the same as SVRA_GrabToggle

* SVRA_PlayAreaModifier: Used to modify the play area dynamically while the game is running via the keyboard
  * Attach to the SteamVR CameraRig prefab
  * Features include rotate, scaling and movement of the play area
  
* SVRA_PlayerResize: Resizes the play area and player to specfied parameter
  * Attach to the object to trigger the size change on interacting with
  * Setup the fuction in the event bridge to trigger the resize on the specified event

* SVRA_ProjectileShooter: A simple projectile shooter system
  * Attach to the object to fire the projectile from
  * Drag a prefab or gameobject into the "Projectile" option in the Inspector window
  * The projectile characteristics determine the direction, angle, frequency, speed and size of the projectile that is fired
  * The "Vibration" toggle is used to determine whether or not to trigger a small vibration burst on firing the projectile

* SVRA_SceneTransition: Used to transition between 2 Unity scenes via the event bridge
  * Ensure the scene to transition to is include in the project build settings
  * Attach to the object to transition scenes when interacted with
  * Add the name of the scene to the "Level Name" attribute in the Inspector window
  * Setup the fuction in the event bridge to trigger the scene change on the specified event

* SVRA_SliderVibration: Used to trigger a vibration on sliding or grabbing an object
  * Attach to the object to trigger the vibration on grabbing
  
* SVRA_TriggerAudio: Used to trigger audio on triggering an interaction
  * Attach to the object and setup the audio to be triggered
  * Setup the fuction in the event bridge to trigger the audio on the specified interaction event 
  
# Prefab Guide
## ControllerSetup
Used to setup the controllers, setup the grip and interaction button and object collision parameters
### Setup:
* Drag and drop a ControllerManger onto both of the controller components of the SteamVR [CameraRig]
* Set the "Tracked Controller Object" to track the particular controller. 
* For Controller (left) set it to Controller (left) and for Controller (right) set it to Controller (right)
### Button Configuration:
* Grip Button: Setup the action to be triggered upon pressing the grip button on the Vive controller
* Trigger Button: Setup the action to be triggered upon pressing the trigger button on the Vive controller
* Touchpad Button: Setup the action to be triggered upon pressing the touchpad button on the Vive controller
* Application Menu Button: Setup the action to be triggered upon pressing the application menu button on the Vive controller
* Virtual Buttons: Creates a virtual d-pad on the Vive controller's touchpad. This allows actions to be mapped to the equivalent of an "up", "down", "left", "right" or "centre" button on the touchpad
### Addtional Details:
* Touch Radius: Set the radius of the collision sphere collider
* Hold Radius: Set the distance you remain holding onto objects blocked by other objects / colliders before breaking the joint connection
* Visible Collider: Make the collision zone for object interaction visible
* Toggle Grab Mode: Switch between holding the grab button down to grab onto objects or press it once to grab and once again to release

## FrameRateCounterPrefab
Used to display the current frame rate of the scene
### Setup:
* Drag and drop into scene
* Toggle on and off with f key on keyboard
