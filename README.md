# 🚁 Teleoperated Aerial Manipulator & Virtual Reality Avatar

[![Paper DOI](https://img.shields.io/badge/IEEE-10.1109%2FICUAS51884.2021.9476884-blue)](https://doi.org/10.1109/ICUAS51884.2021.9476884)
[![Video Demo](https://img.shields.io/badge/YouTube-Watch%20Demo-red)](https://www.youtube.com/watch?v=Ipo-vKNvP8k)
[![ROS Version](https://img.shields.io/badge/ROS-Melodic-brightgreen)](http://wiki.ros.org/melodic)
[![Autopilot](https://img.shields.io/badge/PX4-Autopilot-orange)](https://px4.io/)

Official repository for **"Teleoperated aerial manipulator and its avatar. Communication, system's interconnection, and virtual world"** presented at the *2021 International Conference on Unmanned Aircraft Systems (ICUAS)*.

---

## 📌 Table of Contents
- [Overview](#-overview)
- [System Architecture](#-system-architecture)
- [Prerequisites](#-prerequisites)
- [Installation & Setup](#-installation--setup)
- [Usage & Execution](#-usage--execution)
- [Media & Demos](#-media--demos)
- [Citation](#-citation)
- [Authors & Acknowledgments](#-authors--acknowledgments)

---

## 📖 Overview

This project implements a **human-in-the-loop teleoperation system** for a semi-autonomous aerial manipulator using a **Virtual Reality (VR) avatar**. The complete framework integrates:
- **SITL Simulation**: Typhoon H480 hexarotor model with a mounted robotic manipulator arm simulated in Gazebo Classic.
- **Control Algorithm**: Geometric tracking control implemented in PX4 firmware to stabilize the UAV against dynamic disturbances from the arm and load manipulations.
- **Virtual Reality Immersion**: Unity-driven VR environment providing 2D/3D visual feedback and HTC Vive controller teleoperation.
- **Communication Pipeline**: Bi-directional real-time data transmission over WebSockets using `rosbridge_suite` and MAVROS.

---

## 🏗 System Architecture

The system establishes real-time interconnection between the physical/simulated robot in Linux (Ubuntu) and the VR environment in Windows:

+----------------------------------+            +----------------------------------+
|         VR Environment           |            |       UAV Simulator (Gazebo)     |
|            (Unity)               |            |          & PX4 Firmware          |
|  - 3D Immersion (HTC Vive)       | WebSockets |  - Geometric Control Algorithm   |
|  - 2D/3D View & Avatar           | <========> |  - MAVROS Node                   |
|  - Subscriber: Local Position    | (rosbridge)|  - rospy Nodes (pos_data, brazo) |
|  - Publisher: Arm Angles         |            |  - Typhoon H480 Model            |
+----------------------------------+            +----------------------------------+


---

## ⚙️ Prerequisites

### Software Requirements
| Component | Supported Version / Resource |
| :--- | :--- |
| **Operating System** | Ubuntu 18.04 LTS (Gazebo/PX4) & Windows 10 (Unity VR) |
| **ROS** | [ROS Melodic](http://wiki.ros.org/melodic/Installation/Ubuntu) |
| **Autopilot / Simulator** | PX4 Autopilot & Gazebo Classic |
| **Bridge Protocol** | [rosbridge_suite](http://wiki.ros.org/rosbridge_suite) |
| **Ground Station** | [QGroundControl](http://qgroundcontrol.com/) |
| **VR Engine** | Unity 3D with HTC Vive support |

---

## 🚀 Installation & Setup

### 1. Clone the Firmware Repository
Clone the modified PX4 firmware repository to your local workspace:
```bash
git clone [https://github.com/Rodolfo9706/Firmware.git](https://github.com/Rodolfo9706/Firmware.git)
```

### 2. File Replacement Setup
Before compiling, replace the custom files inside your PX4 workspace by running the following commands:

```bash
# Replace the typhoon_h480 model
cp -r Firmware/typhoon_h480 src/Firmware/tools/sitl_gazebo/models/

# Replace the rate controller script
cp Firmware/rate_control.cpp src/Firmware/src/modules/mc_rate_control/ratecontrol/

# Replace the logger and vmount modules
cp -r Firmware/logger src/Firmware/src/modules/
cp -r Firmware/vmount src/Firmware/src/modules/
```

### 3. Compile Firmware
Open a terminal inside the `Firmware` folder and build the simulation environment:

```bash
sudo make px4_sitl gazebo_typhoon_h480

```

cd <PX4-Autopilot_clone>
DONT_RUN=1 make px4_sitl_default gazebo-classic
source ~/catkin_ws/devel/setup.bash
source Tools/simulation/gazebo-classic/setup_gazebo.bash $(pwd)$(pwd)/build/px4_sitl_default
export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)
export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)/Tools/simulation/gazebo-classic/sitl_gazebo-classic

# Launch the Aerial Manipulator
roslaunch px4 mavros posix_sitl.launch_vehicle:=typhoonh480

# Terminal 1 & 2: Launch telemetry topics
rosrun pos_data pos_data.py
rosrun brazo brazo.py

# Terminal 3: Launch WebSocket Bridge for VR
roslaunch rosbridge_server rosbridge_websocket.launch

### Step 2: Run ROS Nodes & WebSocket Server
In separate terminals, run the communication nodes:

```bash
# Terminal 1: Position telemetry topic
rosrun pos_data pos_data.py

# Terminal 2: Arm control topic
rosrun brazo brazo.py

# Terminal 3: WebSocket Bridge for VR
roslaunch rosbridge_server rosbridge_websocket.launch

```

@INPROCEEDINGS{9476884,
  author={Verdín, Rodolfo and Ramírez, Germán and Rivera, Carlos and Flores, Gerardo},
  booktitle={2021 International Conference on Unmanned Aircraft Systems (ICUAS)}, 
  title={Teleoperated aerial manipulator and its avatar. Communication, system's interconnection, and virtual world}, 
  year={2021},
  volume={},
  number={},
  pages={1488-1493},
  doi={10.1109/ICUAS51884.2021.9476884}
}
