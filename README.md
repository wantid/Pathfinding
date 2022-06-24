# Pathfinding

Simple programm, that draws map and finds the best way to a random point

## Repository information

### Demo
In project files you can find a demo scene, that represents how this implementation of pathfinding works. 
Main objects of this scene are:
* "1st person" - has an attached script "Navigation.cs". "Navigation.cs" was written for drawing map and for computing the best way to desired point. This object has also child components, **Agent** and **Agent Camera**.
* "Agent" - has an attached script "AgentWalk.cs". "AgentWalk.cs" calls "Navigation.cs" to draw a map, and to draw a way. *Map* and current *Way* are lists, that consists of *Points* (fig.1).
<p align="center">
  <img src="/GithubMedia/point.PNG" alt="point class">
</p>

## Links

[Telegram] [Youtube]

[Youtube]: https://www.youtube.com/channel/UC3kV-wnqBE3Y2tdtdSrjvGQ
[Telegram]: https://t.me/exeersitus