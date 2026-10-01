# Teleoperated Aerial Manipulator and its Virtual Reality Avatar

[![Paper](https://img.shields.io/badge/IEEE-10.1109%2FICUAS51884.2021.9476884-blue)](https://doi.org/10.1109/ICUAS51884.2021.9476884)
[![Video](https://img.shields.io/badge/YouTube-Demo-red)](https://www.youtube.com/watch?v=Ipo-vKNvP8k)
[![ROS](https://img.shields.io/badge/ROS-Melodic-brightgreen)](http://wiki.ros.org/melodic)
[![PX4](https://img.shields.io/badge/PX4-Autopilot-orange)](https://px4.io/)

Code accompanying the paper **"Teleoperated aerial manipulator and its avatar. Communication, system's interconnection, and virtual world"**, presented at the *2021 International Conference on Unmanned Aircraft Systems (ICUAS)*.

Contact: rverdin@cio.mx

## Contents

- [Overview](#overview)
- [System architecture](#system-architecture)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Results](#results)
- [Future work](#future-work)
- [Citation](#citation)
- [Authors and acknowledgments](#authors-and-acknowledgments)
- [Media](#media)

## Overview

A human-in-the-loop system to teleoperate a semi-autonomous aerial manipulator through a virtual reality (VR) interface, together with an **avatar**: a virtual copy of the robot that replicates its motion in a Unity world as fast as processing and communication allow.

This repository covers the first part of the project, performed in **software-in-the-loop (SITL)** simulation. It includes:

- **Aerial manipulator model** in Gazebo, connected to the PX4 firmware.
- **Geometric tracking control** (SE(3)) implemented in PX4 to stabilize the UAV against the forces and moments exerted by the arm and the payload.
- **Avatar and virtual world** in Unity 3D, operated with an HTC Vive headset and controllers.
- **Communication** between Ubuntu (Gazebo/PX4) and Windows (Unity) over the Internet using MAVROS and `rosbridge_suite` (WebSockets).

The operator commands the position references of the UAV and the joint angles of the manipulator arm.

## System architecture

```text
 Windows 10                                        Ubuntu 18.04
+------------------------------+                 +--------------------------------+
|  Unity 3D (VR environment)   |   WebSocket     |  ROS Melodic + MAVROS          |
|  - HTC Vive teleoperation    | <=============> |  - rosbridge_server            |
|  - Avatar (2D/3D views)      |   (rosbridge)   |  - pos_data.py  (position)     |
|  - Subscribes: vehicle pose  |                 |  - brazo.py     (arm angles)   |
|  - Arm angle commands        |                 |  - Gazebo + PX4 SITL           |
+------------------------------+                 |  - QGroundControl              |
                                                 +--------------------------------+
```

- **Vehicle position:** the `LocalPosition` MAVROS topic is read by `pos_data.py` and republished so Unity can move the avatar.
- **Manipulator:** `brazo.py` publishes three variables per arm joint through the `MountControl` topic; Gazebo moves the arm accordingly and Unity mirrors it.

## Requirements

| Component | Version / resource |
| :-- | :-- |
| Simulation host OS | Ubuntu 18.04 LTS |
| VR host OS | Windows 10 |
| ROS | [Melodic](http://wiki.ros.org/melodic/Installation/Ubuntu) |
| Autopilot and simulator | PX4 Firmware (SITL) and Gazebo |
| ROS-Unity bridge | [rosbridge_suite](http://wiki.ros.org/rosbridge_suite) |
| Ground station | [QGroundControl](http://qgroundcontrol.com/) |
| VR | Unity 3D and HTC Vive |

PX4, MAVROS and Gazebo must already be installed and working. If you have problems, follow the [MAVROS installation guide](https://docs.px4.io/main/en/ros/mavros_installation.html) and the [PX4 Ubuntu development environment guide](https://docs.px4.io/main/en/dev_setup/dev_env_linux_ubuntu.html).

> **Note:** this code was developed against the PX4 Firmware layout of the ROS Melodic era (`Tools/sitl_gazebo`). Newer PX4 releases (`Tools/simulation/gazebo-classic`) use different paths and have not been tested with these files.

## Installation

### 1. Get the modified firmware files

Clone the modified PX4 Firmware into your Ubuntu machine:

```bash
git clone https://github.com/Rodolfo9706/Firmware.git
```

### 2. Copy the custom files into your PX4 workspace

Set `PX4_DIR` to your own PX4 Firmware folder, then copy the modified files from the cloned `Firmware` folder:

```bash
export PX4_DIR=~/src/Firmware   # adjust to your setup

# Aerial manipulator model
cp -r Firmware/typhoon_h480 $PX4_DIR/Tools/sitl_gazebo/models/

# Geometric controller
cp Firmware/rate_control.cpp $PX4_DIR/src/modules/mc_rate_control/ratecontrol/

# Logger and vmount modules
cp -r Firmware/logger $PX4_DIR/src/modules/
cp -r Firmware/vmount $PX4_DIR/src/modules/
```

### 3. Build the firmware

```bash
cd $PX4_DIR
DONT_RUN=1 make px4_sitl_default gazebo
```

Do not use `sudo` unless your installation requires it.

### 4. Build the ROS nodes

Create a catkin workspace with the `pos_data` and `brazo` packages (only the `.py` files, written with rospy) and build it:

```bash
cd ~/catkin_ws
catkin_make
source devel/setup.bash
```

<!-- TODO(author): state where pos_data and brazo live in this repo. -->

## Usage

### 1. Launch the SITL simulation

```bash
cd $PX4_DIR
source ~/catkin_ws/devel/setup.bash
source Tools/setup_gazebo.bash $(pwd) $(pwd)/build/px4_sitl_default
export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)
export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)/Tools/sitl_gazebo

roslaunch px4 mavros_posix_sitl.launch vehicle:=typhoon_h480
```

The aerial manipulator model opens in Gazebo. Use QGroundControl to arm and control the vehicle.

### 2. Start the communication nodes

In three separate terminals:

```bash
# Terminal 1: vehicle position topic
rosrun pos_data pos_data.py

# Terminal 2: manipulator arm topic
rosrun brazo brazo.py

# Terminal 3: WebSocket bridge for Unity
roslaunch rosbridge_server rosbridge_websocket.launch
```

rosbridge listens on port `9090` by default (`ws://<ROS_MASTER_IP>:9090`).

### 3. Run the Unity VR avatar

On the Windows machine, open the Unity project, set the IP address of the Ubuntu (ROS master) machine in the connection script, connect the HTC Vive and press Play.

<!-- TODO(author): add the Unity project folder name and the exact script/field where the IP is set. -->

## Results

Experiments were run in SITL (Gazebo on an Intel i7-7820HK laptop with 32 GB RAM and a GTX 1070; Unity on an AMD A12-9720P laptop with 12 GB RAM and no GPU).

- **Control:** the geometric controller tracked position and attitude references with and without payload, under teleoperated translations, rotations and arm motion.
- **Latency:** the avatar followed the simulated robot with a delay of about **0.5 s** over the network.
- **Pick and place:** a 160 g object was picked up and transported, showing robustness to mass variations and to the forces and moments produced by the arm.

See the [supplementary video](https://www.youtube.com/watch?v=Ipo-vKNvP8k) for the experiments.

## Future work

- Experiments on the real aerial manipulator prototype built at the lab.
- Vision-based SLAM to reconstruct the virtual environment with real dimensional and image data.
- A more intuitive sensorial VR system, plus faster networking and a different communication protocol to reduce latency.

## Citation

```bibtex
@INPROCEEDINGS{9476884,
  author={Verd{\'i}n, Rodolfo and Ram{\'i}rez, Germ{\'a}n and Rivera, Carlos and Flores, Gerardo},
  booktitle={2021 International Conference on Unmanned Aircraft Systems (ICUAS)},
  title={Teleoperated aerial manipulator and its avatar. Communication, system's interconnection, and virtual world},
  year={2021},
  doi={10.1109/ICUAS51884.2021.9476884}
}
```

## Authors and acknowledgments

Laboratorio de Percepción y Robótica, Centro de Investigaciones en Óptica (CIO), León, Guanajuato, México.

Rodolfo Verdín, Germán Ramírez, Carlos Rivera and Gerardo Flores (corresponding author, gflores@cio.mx).

Partially supported by CONACYT-FORDECYT under grant 292399.

If you have any problem with the STL or DAE packages, send me an email.

## Media

![The avatar and the virtual world made in Unity](https://user-images.githubusercontent.com/58195148/111925024-bdc77a80-8a6c-11eb-9424-a3ea9762f9c6.png)

[![Watch the video](https://img.youtube.com/vi/Ipo-vKNvP8k/0.jpg)](https://www.youtube.com/watch?v=Ipo-vKNvP8k)

<!-- Optional: uncomment after uploading the images to an images/ folder in the repo.

<p align="center">
  <img src="images/fig1_vr_immersion.png" width="700" alt="Teleoperation with HTC Vive, Unity world and Gazebo simulation">
  <br>
  <em>Teleoperation with HTC Vive (a), Unity virtual world (b) and Gazebo SITL simulation (c).</em>
</p>

<p align="center">
  <img src="images/fig3_architecture.png" width="800" alt="System architecture diagram">
  <br>
  <em>System architecture.</em>
</p>

<p align="center">
  <img src="images/fig9_pick_and_place.png" width="500" alt="Pick and place experiment">
  <br>
  <em>Pick and place experiment (160 g payload).</em>
</p>

<p align="center">
  <img src="images/fig10_prototype.png" width="400" alt="Aerial manipulator prototype built at the lab">
  <br>
  <em>Aerial manipulator prototype built at the lab.</em>
</p>

-->
