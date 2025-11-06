# IMG_420_Assignment_5 #

### Shader ###

The shader works by using a time_passed, color_start, and color_end variable and doing mixing operations on those in order to get the gradient to move across the surface of the particle. This mixing operation is done to generate a new temporary start and end color for the top and bottom of the particle that is then smoothly transitioned between in order to create the effect that the gradient itself is traveling over and looping across the top and bottom of the particle.'

The wave distortion is done by offsetting the uv.x variable by a certain amount based on a sine wave, and then checking if that uv.x exits the bounds of the typical uv range minus a tolerance. UV goes from 0.0 to 1.0 and the actual wave determines that 0.1 to 0.9 is the actual bounds in order to make the waves on the side of the particle smoother.

### Alarm System ###

This first will create a raycast that shoots straight down for a set distance. This raycast will check for collisions every physics update through the ForceRaycastUpdate() function, and then the raycast is check for colliders, and if there is one it will update the laser to only go the distance of up until it hits an object. This object is then compared against the player to see if it is the player that it is intersecting or something else. If it is the player, the alarm will be triggered, and we will use a color rect that covers the full screen and another shader to make a flashing red effect across the screen along with changing the color of the laser to show the alarm has been triggered. This starts a timer for the alarm, and when the alarm time is over the alarm will be reset and the laser color and flashing lights will go back to normal.

### Chain ###

The chain creates a bunch of segments from a scene and connects them using pin joints. Each of these segments have a custom function that will be called if there is a collision detected and apply a force to the segment that had collision against it, allowing the chain to be pushed around by the player. I chose the pin joints because the damped springs did not feel stiff enough, and I think that it reasonably recreates the type of hinge behavior that you would expect out of a chain, where the spring doesn't do that.
