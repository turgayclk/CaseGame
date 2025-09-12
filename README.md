# Case Project

This is a **case study project** for KFA Entertainment.  
The game is a simple tower defense prototype made with Unity.

## Assumptions
- Enemies only damage the player when they reach the end of the path.  
- Enemies do not attack the player directly.  
- Player automatically attacks the nearest enemy (ranged attack).  
- Placeholder assets are used for sprites, sounds, and effects.  

## Engine & Version
- Unity 6.0 (URP)

## Notes
- Player HP decreases when enemies finish the path.  
- Waves spawn automatically, but a button could be added to call waves earlier.  

### Extra: Player Death & Revive
- When the player dies, two options are presented:
  - **Restart**: Reloads the scene and restarts the game loop from wave 1.  
  - **Revive**: Player respawns on the same wave with full HP.  
    - A short invulnerability window (I-Frame) is granted after reviving.  
    - This state is visually represented with a blinking effect (implemented with DOTween).  
