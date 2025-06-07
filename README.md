# My RL Projects (2D)

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Unity Version](https://img.shields.io/badge/Unity-2022.3.22f1-blue)
![ML-Agents](https://img.shields.io/badge/ML--Agents-0.30.0-orange)
![Python Version](https://img.shields.io/badge/python-3.9-blue)
![Status](https://img.shields.io/badge/status-active-brightgreen)

This repository contains a collection of reinforcement learning environments (2d) and agents developed for training and testing simple navigation behaviors.

## Projects

### MoveToGoal

In **FlappyPixel**, the agent must learn to navigate through gaps between pipes by flapping at the right moment. The goal is to stay airborne, avoid obstacles, and score points by passing through pipes.

**Reward Mechanics**
* Positive Reward:
  * +0.01: A small reward given at each step the agent stays alive, encouraging sustained flight.
  * +2: For successfully passing through a set of pipes (scoring).
* Negative Reward (Penalties):
  * -1: For colliding with an obstacle or the ground, which also ends the episode.

- 📂 [**Go to project folder**](ML-Agents/Examples/MoveToGoal)
<p align="center">
  <img src="Examples/video_and_graphs/FlappyPixel/flappy_pixel_graph.png" alt="FlappyPixel Eval" width="800"/>
</p>

- 🎥 **Demo:**

<p align="center">
  <img src="Examples/video_and_graphs/FlappyPixel/flappy_pixel_gif.gif" alt="FlappyPixel Demo" width="600"/>
</p>
