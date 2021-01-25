# WeeTank
a project similar to wii tanks


ToDo:
- AI: stop pushing walls. options/parts
  - detect collision to wall/obstacle or being held back, and do something about it
  - detect visibility to target
  - use navmesh
- stop tanks turning over
  - change movement to use physics forces instead of small teleports
- separate MOVE target and AIM target for tanks :) 

- keep working on new AI
- hits to actually deal damage, destroy tanks
- win/lose (game over) condition and (re)start (next) level
  - fade in, fade out, UI feedback ("GAME OVER", "VICTORY!")
  - multiple levels
  
- Graphics 
  - models: for tank bodies, turrets, barrels
  - textures (at least ground)
- Audio: firing, explosion, bounce, tank engine, tracks, turret turn, reload, ...
- VFX: bullet trails, smoke, sparks, dynamic lights (e.g explosion), etc
- destructable walls
- powerups OR upgrades between rounds
  - tank: max speed, acceleration, deceleration, turn rate, turn acceleration(momentum), max speed while turning(!)
  - turret: turn rate, turn acceleration, fire rate (reload)
  - cannon?
  - bullet: speed, lifetime, bounces, damage, AOE, ...

- same screen multiplayer (co-op vs AIs, PvP)
  - gamepad input
  - test in Parsec

- DAMAGE SYSTEM DESIGN!
  - Should bullets remain single-hit kill? Probably not. 
  - Should they have infinite ammo? Probably yes. :)
  - It would be interesting if bullets wouldn't always deal the same damage, but have a chance* to deal damage, like in an RPG: hit/miss/critical, random* value, so upgrades (to both bullets/cannon and to armor) could affect the range and probabilities of how much damage an impact deals :)
  - Should tank have a simple total "health", or should they receive damage to specific parts/functionalities? Could create more heroic fights and "near misses" :) Decreased movement speed, turn speed, turn limited to one way, no reverse or no forwards, getting immobilised, turret rotation speed, reload time, etc :)
  - Should they have some kind of repair? Either built into the tank ("self-heal"), powerup, or repair points between levels, etc?

