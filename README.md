# SteamVR Assistant 
The SteamVR Assistant is a Unity toolkit for assisting developers creating VR applications using Unity and SteamVR. It aims to help streamline the development process and make VR development more accessible to new developers.

Note: No Vive connected - openapi errors (due to lack of Vive)

# Example Scenes
## svra_default_setup
A simple default setup of the controllers and SteamVR required prefabs on an empty plane.

## svra_object_playground
A random (somewhat chaotic) collection of all the potential interactions SVRA is capable of.

## svra_projectile_range_test
An example featuring 3 floating targets of varying speeds in which the user is tasked to hit with cubes. Showcases the object snap zone and hover scripts in particular.

## svra_example_grabbable_objects
TODO : Simple example showing grabbable objects

## svra_example_interactive_objects
TODO : Simple example showing interactive objects via buttons and changing colours / materials (possibly enable grabbable script as well)

# Some Instructions (I'll make a PDF version sometime)
## Key Scripts
* SVRA_GrabbableObject: Makes an object grabbable. Attach to objects you wish to pick up and interact with.
* SVRA_InteractiveObject: Makes an object interactive and open to interaction scripts.
* SVRA_EventBridge: On a specified event (interaction start, grab start, etc.) trigger some attached function(s). [Possibly add link to additional example Google doc with screenshots to make clearer]

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
* Toggle on and off with 'f' key on keyboard
