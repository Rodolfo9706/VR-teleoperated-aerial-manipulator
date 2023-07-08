# Teleoperated aerial manipulator and its avatar.

 [rverdin@cio.mx] 

 aerial manípulator paper --- [DOI: 10.1109/ICUAS51884.2021.947688]
 
To run the gazebo simulation part it is necessary to have the following packages:

• gazebo - px4 Firmware - mavros: https://docs.px4.io/main/en/ros/mavros_installation.html

• ROS melodic: http://wiki.ros.org/melodic/Installation/Ubuntu

• rosbridge weboscket:  http://wiki.ros.org/rosbridge_suite

• qground control http://qgroundcontrol.com/

please clone the following firmware into your ubuntu repository: with git clone
 ```https://github.com/Rodolfo9706/Firmware.git```

Once cloned, open in the terminal the address of the firmware folder and run the following line: ```make px4_sitl gazebo``` add sudo if is necessary.

 
Skip this step, only continue if you had problem installing mavros-px4-gazebo: ```https://dev.px4.io/master/en/setup/dev_env_linux_ubuntu.html``` #rosgazebo
 
------------------------------------------------------------------------------------

#Compile PX4 Firmware

Once the px4 Firmware is downloaded replace the following files:
replace the 

•Replace the typhoon_h480 folder located in ```src / Firmware / tools / sitl_gazebo / models``` with the one located in ```Firmware```.

•Replace rate_control.cpp located in ```src / Firmware / src / modules / mc_rate_control / ratecontrol```.

•Do the same with logger and vmount located in ```src / Firmware / src / modules /```

Once the folders have been replaced, run the following line in the terminal inside the Firmware folder:

```sudo make px4_sitl gazebo_typhoon_h480```

--------------------------------------------------------------------------------------
##ROSLAUNCH execution 

workspaces for rospy
For this step, you need the knowledge to create a workspace using rospy 
Once created you will only need the .py files

To run roslaunch it is necessary to run in the terminal:


```cd <PX4-Autopilot_clone>
DONT_RUN=1 make px4_sitl_default gazebo-classic
source ~/catkin_ws/devel/setup.bash    # (optional)
source Tools/simulation/gazebo-classic/setup_gazebo.bash $(pwd) $(pwd)/build/px4_sitl_default
export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)
export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)/Tools/simulation/gazebo-classic/sitl_gazebo-classic
roslaunch px4 posix_sitl.launch
```
To run aerialmanipulator please:
```roslaunch px4 mavros posix_sitl.launch_vehicle:=typhoonh480```

The aerial-manipulator deal model will open immediately,  to control the vehicle you can use qgroundcontrol and arm.
--------------------------------------------------------------------------------------

#### VR comunnication:

To control it with the virtual environment you need to run the model in unity, once downloaded.
Run the topics: 

```rosrun pos_data pos_data.py```
```rosrun brazo brazo.py```

Runwebsocket:
```roslaunch rosbridge_server rosbridge_websocket.launch```



If you have any problem with stl package or dae package send me a mail.


![The-avatar-and-the-virtual-world-made-in-Unity](https://user-images.githubusercontent.com/58195148/111925024-bdc77a80-8a6c-11eb-9424-a3ea9762f9c6.png)




