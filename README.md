# Stage Fright
Game Jam Fall 2015 Project by Guigl Party

Amnesia-style first person horror game controlled by playing the piano.

Making use of three different keijiro Unity plugins, cause this guy's awesome: MidiJack, KinoGlitch and Retro3D.

-----------------------

Brainstormy ideas go here.

*Map*
- player is trapped in a maze with a monster, and has to find the exit
- spooky castle
- player has a map of the maze they can look at
	- maybe fog of war?

*Controls*
- diminished triads to run
	- move higher to turn right, lower to turn left
- press lowest keys to look behind you/ at monster
- press highest keys to look away from monster/180 turn
- arpeggio up/down to go up/down stairs/ladders

*Monster AI*
- cheating Amnesia movement? (spawn near player on trigger, despawn if escaped)
- chase using line-of-sight?


*Graphics*
- ps1 graphics courtesy of keijiro
- might mean our wall meshes will need a few bumps to make sure we get that weird vertex shaking

- also have a few different keijiro glitch things to use:
	- scan line jitter, color drift, vertical jump, horizontal shake, block damage
	- use these when the player sees the monster, to further obscure it?
	- or on other triggers for spooky moments
	
