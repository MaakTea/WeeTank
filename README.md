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
  
- Audio: firing, explosion, bounce, tank engine, tracks, turret turn, reload, ...
- VFX: bullet trails, smoke, sparks, dynamic lights (e.g explosion), etc
- destructable walls
- powerups OR upgrades between rounds
  - tank: max speed, acceleration, deceleration, turn rate, turn acceleration(momentum), max speed while turning(!)
  - turret: turn rate, turn acceelration, fire rate (reload)
  - bullet: speed, lifetime, bounces, damage, AOE, ...

- same screen multiplayer (co-op vs AIs, PvP)
  - gamepad input
  - test in Parsec
