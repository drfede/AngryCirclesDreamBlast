# Angry Circles Dream Blast

# Dev Log

## First Analysis

After a couple of gameplay videos I started to think how I should implement the main mechanics.

**Main Idea:** The game is a match-3 without a fixed grid with some physics. The number of elements in game can become high, this made me think that in the future this may require some optimization, but for a prototype instantiation or a simple pooling should be enough. The system to count and find the correct pop that I will try to implement will be a recursive count type, checking for each not already checked circle that is colliding with a circle of the same type.

**Input System:** a simple tap on objects to make the object and the neighbors pop.

**Others**:  About the map, it’s a sprite with a 2D collider. Extra systems will be needed for popped elements count and moves counter.

---

## First Implementation

Created a map, created a StandardCircle prefabs with a collider, rigidbody and a Circle Class.

---

## Collision check

While implementing the collision check I prefered a recursive implementation with raycasts on tapped circle over a costant OnCollision check on all circles. I think, since right now we have no need to check what other circles are colliding with, this might be the best approach.

---

## Some Improvements

Added a UI with a button to tell the player when he wins/loses. Added physic materials to make the movement of the circles smoother.

Some objects are now scriptable objects:

- The settings are a scriptable object, so they can be changed easily (number of necessary circles to pop, etc.)
- Level Settings are a scriptable object, put them in resources so if for some reason they have to be loaded at runtime they can, it’s not a requirement but maybe in the future it could be useful.

I added a small explosion-popup tween. 

---

## Special Circles

To integrate the special circles some methods changed and are now virtual, like the way neighbors calculation and how the raycast is done.

---

## External Assets

Text Mesh Pro

Tween Library:

[LeanTween | Animation Tools | Unity Asset Store](https://assetstore.unity.com/packages/tools/animation/leantween-3595)

Unity Inspector extension library:

[https://github.com/dbrizov/NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes)

## Final result

Realization time: 6-8hours

In the game there are 4 standard circles (blue, yellow, white, red) and a special one (black), the special one is a bomb that destroys the nearby circles. The special circle spawns every time at least 5 circles pop.

[https://drive.google.com/file/d/14QqKN6dwd9vYTq-HJAMBKvJoC-00e_ye/view?usp=sharing](https://drive.google.com/file/d/14QqKN6dwd9vYTq-HJAMBKvJoC-00e_ye/view?usp=sharing)
