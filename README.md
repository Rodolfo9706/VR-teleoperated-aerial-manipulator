# Teleoperated Aerial Manipulator and its Avatar

[![Paper](https://img.shields.io/badge/IEEE-10.1109%2FICUAS51884.2021.9476884-blue)](https://doi.org/10.1109/ICUAS51884.2021.9476884)
[![Video](https://img.shields.io/badge/YouTube-Demo-red)](https://www.youtube.com/watch?v=Ipo-vKNvP8k)
[![ROS](https://img.shields.io/badge/ROS-Melodic-brightgreen)](http://wiki.ros.org/melodic)
[![PX4](https://img.shields.io/badge/PX4-Autopilot-orange)](https://px4.io/)

Code for the paper **"Teleoperated aerial manipulator and its avatar. Communication, system's interconnection, and virtual world"**, presented at the *2021 International Conference on Unmanned Aircraft Systems (ICUAS)*.

Paper: [DOI 10.1109/ICUAS51884.2021.9476884](https://doi.org/10.1109/ICUAS51884.2021.9476884)
Contact: rverdin@cio.mx

![The avatar and the virtual world made in Unity](https://user-images.githubusercontent.com/58195148/111925024-bdc77a80-8a6c-11eb-9424-a3ea9762f9c6.png)

[![Watch the video](https://img.youtube.com/vi/Ipo-vKNvP8k/0.jpg)](https://www.youtube.com/watch?v=Ipo-vKNvP8k)

## Overview

A human-in-the-loop system to teleoperate a semi-autonomous aerial manipulator with a virtual reality (VR) interface (HTC Vive), plus an **avatar** that replicates the robot in a Unity virtual world. This repository covers the software-in-the-loop (SITL) part: a Gazebo model controlled by PX4 (geometric tracking control) and connected to Unity through MAVROS and rosbridge (WebSockets).

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

## Requirements

To run the Gazebo simulation you need:

- Gazebo, PX4 Firmware and MAVROS: [MAVROS installation guide](https://docs.px4.io/main/en/ros/mavros_installation.html)
- [ROS Melodic](http://wiki.ros.org/melodic/Installation/Ubuntu)
- [rosbridge_suite](http://wiki.ros.org/rosbridge_suite) (WebSocket server)
- [QGroundControl](http://qgroundcontrol.com/)
- Unity 3D with HTC Vive support (on Windows 10) for the VR avatar

If you have problems installing MAVROS, PX4 or Gazebo, follow the [PX4 Ubuntu development environment guide](https://docs.px4.io/main/en/dev_setup/dev_env_linux_ubuntu.html).

## Installation

### 1. Get the modified firmware

Clone the modified PX4 Firmware into your Ubuntu machine:

```bash
git clone https://github.com/Rodolfo9706/Firmware.git
```

### 2. Replace the files in your PX4 workspace

Copy the following files from the cloned `Firmware` folder into your own PX4 workspace (adjust the paths to your setup):

- Replace the `typhoon_h480` folder in `src/Firmware/Tools/sitl_gazebo/models`.
- Replace `rate_control.cpp` in `src/Firmware/src/modules/mc_rate_control/ratecontrol`.
- Do the same with `logger` and `vmount` in `src/Firmware/src/modules/`.

### 3. Compile the firmware

Open a terminal in your PX4 Firmware folder and run:

```bash
make px4_sitl gazebo_typhoon_h480
```

Do not use `sudo` unless your installation requires it.

## Usage

### 1. ROS workspace

You need a catkin workspace with the `pos_data` and `brazo` packages (they only need the `.py` files, written with rospy). Build it and source it:

```bash
cd ~/catkin_ws
catkin_make
source devel/setup.bash
```

### 2. Launch the simulation

From your PX4 Firmware folder:

```bash
DONT_RUN=1 make px4_sitl_default gazebo
source ~/catkin_ws/devel/setup.bash
source Tools/setup_gazebo.bash $(pwd) $(pwd)/build/px4_sitl_default
export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)
export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)/Tools/sitl_gazebo

roslaunch px4 mavros_posix_sitl.launch vehicle:=typhoon_h480
```

The aerial manipulator model opens in Gazebo. Use QGroundControl to arm and control the vehicle.

### 3. VR communication

Run the Unity model, then start the topics and the WebSocket server in separate terminals:

```bash
# Terminal 1: vehicle position topic
rosrun pos_data pos_data.py

# Terminal 2: manipulator arm topic
rosrun brazo brazo.py

# Terminal 3: WebSocket bridge
roslaunch rosbridge_server rosbridge_websocket.launch
```

rosbridge listens on port `9090` by default. In Unity, set the IP address of the Ubuntu machine (`ws://<ROS_MASTER_IP>:9090`).

## Results

- Geometric controller tested with and without payload under teleoperated motion.
- Avatar delay of about 0.5 s over the network.
- Pick and place of a 160 g object in Gazebo, mirrored by the avatar in Unity.

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


If you have any problem with the STL or DAE packages, send me an email.
