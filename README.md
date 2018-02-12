# SteamVR Assistant 
The SteamVR Assistant is a Unity toolkit for assisting developers creating VR applications using Unity and SteamVR. It aims to help streamline the development process and make VR development more accessible to new developers.

Note: No Vive connected - openapi errors (due to lack of Vive)

# Example Scenes
## svra_default_setup
A simple default setup of the controllers and SteamVR required prefabs on an empty plane.

## svra_object_playground
A random (somewhat chaotic) collection of all the potential interactions SVRA is capable of.

## svra_projectile_range_example
An example featuring 3 floating targets of varying speeds in which the user is tasked to hit with cubes. 
* Showcases SVRA_ObjectSnapZone and SVRA_ObjectSnapZoneLocation scripts

## svra_grabbable_object_example
A very simple example scene containing four objects setup to be grabbable.
* Showcases SVRA_Grabbable script

## svra_interaction_example
I too have a red button and my button works!
* Showcases SVRA_InteractiveObject and SVRA_EventBridge scripts for enabling some other script's function (ToggleMaterial in this example) by interacting with an object 

# Scene Ideas
The following are a list of potential scene ideas in case you need some ideas to get started:
* Carnival games
* Target range
* Basketball hoop
* 10 pin bowling
* 100 pin bowling
* 10 pin bowling hard mode - the pins move around or hover
* Big red button simulator

# Challenge Scenes
The following are a list of challenge scenes which are setup but have none of the script logic attached. Can you make them functional?
## The Drawer Challenge : svra_drawer_challenge
* Test your knowledge of the Unity joint system! 
* This scene contains a drawer to setup as you expect a drawer to operate. 
* You pull the handle and the drawer comes out and you push it back in and it goes back in. 
* You know because its a drawer. 
* The drawer itself need not operate with gravity but bonus points if it does.

# Some Instructions (I'll make a PDF version sometime)
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
* SVRA_ObjectSnapZone: Used with SVRA_ObjectSnapZoneLocation. 
  * Setup object to setup snap zone on 
  * Create a copy of the object to represent the snap zone location (see SVRA_ObjectSnapZoneLocation for setting up the copy object)
  * Attach the SVRA_ObjectSnapZone script to the original object
    * HoverCharacteristics: Can be used to make the object oscillate for a set distance, speed and direction 
  
* SVRA_ObjectSnapZoneLocation: Used with SVRA_ObjectSnapZone. 
  * On the object copy (mentioned above) remove any rigid bodies and additional scripts attached to the object
  * Disable the Mesh Renderer of the object
  * Enable the "Is Trigger" checkbox of the collider of the object
  * Position the object where you want the object to snap to
  * Attach the SVRA_ObjectSnapZoneLocation script
  * Drag and drop the SVRA_ObjectSnapZone object into the "Snap Zone Transform" setting

# Prefab Guide
## ControllerMangerPrefab
Used to setup the controllers, setup the grip and interaction button and object collision parameters
### Setup:
* Drag and drop a ControllerManger onto both of the controller components of the SteamVR [CameraRig]
* Set the "Tracked Controller Object" to track the particular controller. 
* For Controller (left) set it to Controller (left) and for Controller (right) set it to Controller (right)
### Addtional Details:
* Grip Input: Sets the button used on the Vive controller to initiate "grip interactions" (object grabbing, etc.)
* Interaction Input: Sets the button used on the Vive controller to initiate "interactive interactions" (click buttons, etc.)
* Touch Radius: Set the radius of the collision sphere collider
* Hold Radius: Set the distance you remain holding onto objects blocked by other objects / colliders before breaking the joint connection
* Visible Collider: Make the collision zone for object interaction visible
* Toggle Grab Mode: Switch between holding the grab button down to grab onto objects or press it once to grab and once again to release

## FrameRateCounterPrefab
Used to display the current frame rate of the scene
### Setup:
* Drag and drop into scene
* Toggle on and off with f key on keyboard
