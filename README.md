# Final Project!

## Design Doc
Start off by forking this repository. In your README, write a design doc to outline your project goals and implementation plan. It must include the following sections:

### Introduction

For my final project is about using what we've learned in this class about noise functions and toolbox functions
(ease-in/ease-out/sin/cos) to make some characters procedurally dance. I'm interested in creating a web demo in 
unity that let's the user pick a few different dances for a character, and adjust sliders to adjust various aspects
of the dancing. As a stretch, I also hope that the rhythm of the dancing is driven by the beat of music. 

### Goal

The goal of this project is to create a unique animated environment / music visualizer that involves
characters that can procedurally dance based on a combination of noise and wave functions. 

### Inspiration/reference:

#### Procedural dancing generated from noise functions in Unity

* https://www.youtube.com/watch?v=9KmuHY-SwUU 
* https://github.com/keijiro/PuppetTest

This is a unity project with an associated video which goes into some depth
about driving a unity IK rig to dance with various noise functions. The base
unity project offers some examples of how this is done with one or two dances. 
I was hoping to maybe use this as reference and create 3-4 more dances. 

![gif](https://i.imgur.com/F8wrLm9.gif)
![gif](https://i.imgur.com/XLNbNfT.gif)

#### Interactive dance party with sliders

* https://dddance.party/
		+ password: TOOFUZZYTOSTOP

This demo was my main inspiration, however the dances here are driven my mocap and not 
procedural noise.

<img src="https://github.com/user-attachments/assets/f002e83b-5498-4b3d-8751-f88edcc4a6af" width=600px>

As you can see it offers different sliders to affect the placement of the character, but you can't 
actually affect the dancing animation. 

<img src="https://github.com/user-attachments/assets/f4304789-d379-43a0-b396-ddc800ef7bb6" width=600px>

### Specification:
- A Unity Demo that runs on the web (I'll have to look into how this is done?)
- User can pick a dance for a character or blend dances together
- 3-4 different dances to choose from, generated from different noise functions
- A User Interface with sliders that affects aspects / intensity of the dancing
- Ability to dance to user-defined music (maybe... or I'll settle for supplying my own music)
- Some basic environments for the character to dance in? 

### Techniques:
- Will start with the tutorials / example project that I have already shown in my inspiration as reference:
	* https://www.youtube.com/watch?v=9KmuHY-SwUU 
	* https://github.com/keijiro/PuppetTest
 * Content from lectures will come in handy. Specifically:
	* noise functions: https://www.dropbox.com/scl/fi/535yevnzwa8pmzlcev2j1/noise-copy.pdf?rlkey=jyu9qji6eo8eighjknukp6zbb&e=1&dl=0
 	* toolbox functions: https://www.dropbox.com/scl/fi/4zpr4ew9eiu28hsxnte63/toolbox_functions.pdf?rlkey=yxdblfr5937w5ztwdilaqw37x&e=1&dl=0
* Thinking of the possibility of doing crowds to support a whole club of dancing humanoids: https://learn.unity.com/project/crowd-simulation

### Design:
- How will your program fit together? Make a simple free-body diagram illustrating the pieces.

... 

### Timeline:

#### Milestone 1

* Setup Unity Project and make sure it can deploy on web
* Create one dance which will be the basis for creating other dances
* Set-up basic UI with sliders

#### Milestone 2

* Create 3-4 (or more?) more dances
* In the UI, setup sliders that is able to drive aspects of each dance
* In the UI, add an option to change the music
* Functionality to find the beat in a song and adjust wave functions based on song

#### Milestone 3 (Final)

* Add some interesting options for camera angles / animation
* Options for different types of characters (if using unity's IK rig I assume this is easy, but I could be wrong)
* Some interesting back-drops / environments for characters to dance in. 

Submit your Design doc as usual via pull request against this repository.
## Milestone 1: Implementation part 1 (due 11/13)
Begin implementing your engine! Don't worry too much about polish or parameter tuning -- this week is about getting together the bulk of your generator implemented. By the end of the week, even if your visuals are crude, the majority of your generator's functionality should be done.

Put all your code in your forked repository.

Submission: Add a new section to your README titled: Milestone #1, which should include
- written description of progress on your project goals. If you haven't hit all your goals, what's giving you trouble?
- Examples of your generators output so far
We'll check your repository for updates. No need to create a new pull request.
## Milestone 2: Implementation part 2 (due 11/25)
We're over halfway there! This week should be about fixing bugs and extending the core of your generator. Make sure by the end of this week _your generator works and is feature complete._ Any core engine features that don't make it in this week should be cut! Don't worry if you haven't managed to exactly hit your goals. We're more interested in seeing proof of your development effort than knowing your planned everything perfectly. 

Put all your code in your forked repository.

Submission: Add a new section to your README titled: Milestone #3, which should include
- written description of progress on your project goals. If you haven't hit all your goals, what did you have to cut and why? 
- Detailed output from your generator, images, video, etc.
We'll check your repository for updates. No need to create a new pull request.

Come to class on the due date with a WORKING COPY of your project. We'll be spending time in class critiquing and reviewing your work so far.

## Final submission (due 12/2)
Time to polish! Spen this last week of your project using your generator to produce beautiful output. Add textures, tune parameters, play with colors, play with camera animation. Take the feedback from class critques and use it to take your project to the next level.

Submission:
- Push all your code / files to your repository
- Come to class ready to present your finished project
- Update your README with two sections 
  - final results with images and a live demo if possible
  - post mortem: how did your project go overall? Did you accomplish your goals? Did you have to pivot?

## Topic Suggestions

### Create a generator in Houdini

### A CLASSIC 4K DEMO
- In the spirit of the demo scene, create an animation that fits into a 4k executable that runs in real-time. Feel free to take inspiration from the many existing demos. Focus on efficiency and elegance in your implementation.
- Example: 
  - [cdak by Quite & orange](https://www.youtube.com/watch?v=RCh3Q08HMfs&list=PLA5E2FF8E143DA58C)

### A RE-IMPLEMENTATION
- Take an academic paper or other pre-existing project and implement it, or a portion of it.
- Examples:
  - [2D Wavefunction Collapse Pokémon Town](https://gurtd.github.io/566-final-project/)
  - [3D Wavefunction Collapse Dungeon Generator](https://github.com/whaoran0718/3dDungeonGeneration)
  - [Reaction Diffusion](https://github.com/charlesliwang/Reaction-Diffusion)
  - [WebGL Erosion](https://github.com/LanLou123/Webgl-Erosion)
  - [Particle Waterfall](https://github.com/chloele33/particle-waterfall)
  - [Voxelized Bread](https://github.com/ChiantiYZY/566-final)

### A FORGERY
Taking inspiration from a particular natural phenomenon or distinctive set of visuals, implement a detailed, procedural recreation of that aesthetic. This includes modeling, texturing and object placement within your scene. Does not need to be real-time. Focus on detail and visual accuracy in your implementation.
- Examples:
  - [The Shrines](https://github.com/byumjin/The-Shrines)
  - [Watercolor Shader](https://github.com/gracelgilbert/watercolor-stylization)
  - [Sunset Beach](https://github.com/HanmingZhang/homework-final)
  - [Sky Whales](https://github.com/WanruZhao/CIS566FinalProject)
  - [Snail](https://www.shadertoy.com/view/ld3Gz2)
  - [Journey](https://www.shadertoy.com/view/ldlcRf)
  - [Big Hero 6 Wormhole](https://2.bp.blogspot.com/-R-6AN2cWjwg/VTyIzIQSQfI/AAAAAAAABLA/GC0yzzz4wHw/s1600/big-hero-6-disneyscreencaps.com-10092.jpg)

### A GAME LEVEL
- Like generations of game makers before us, create a game which generates an navigable environment (eg. a roguelike dungeon, platforms) and some sort of goal or conflict (eg. enemy agents to avoid or items to collect). Aim to create an experience that will challenge players and vary noticeably in different playthroughs, whether that means procedural dungeon generation, careful resource management or an interesting AI model. Focus on designing a system that is capable of generating complex challenges and goals.
- Examples:
  - [Rhythm-based Mario Platformer](https://github.com/sgalban/platformer-gen-2D)
  - [Pokémon Ice Puzzle Generator](https://github.com/jwang5675/Ice-Puzzle-Generator)
  - [Abstract Exploratory Game](https://github.com/MauKMu/procedural-final-project)
  - [Tiny Wings](https://github.com/irovira/TinyWings)
  - Spore
  - Dwarf Fortress
  - Minecraft
  - Rogue

### AN ANIMATED ENVIRONMENT / MUSIC VISUALIZER
- Create an environment full of interactive procedural animation. The goal of this project is to create an environment that feels responsive and alive. Whether or not animations are musically-driven, sound should be an important component. Focus on user interactions, motion design and experimental interfaces.
- Examples:
  - [The Darkside](https://github.com/morganherrmann/thedarkside)
  - [Music Visualizer](https://yuruwang.github.io/MusicVisualizer/)
  - [Abstract Mesh Animation](https://github.com/mgriley/cis566_finalproj)
  - [Panoramical](https://www.youtube.com/watch?v=gBTTMNFXHTk)
  - [Bound](https://www.youtube.com/watch?v=aE37l6RvF-c)

### YOUR OWN PROPOSAL
- You are of course welcome to propose your own topic . Regardless of what you choose, you and your team must research your topic and relevant techniques and come up with a detailed plan of execution. You will meet with some subset of the procedural staff before starting implementation for approval.
