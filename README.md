# Teleoperated aerial manipulator and its avatar.
 
 rverdin@cio.mx
 
 To run the gazebo part it is necessary to have the following packages:

gazebo simulation:
https://raw.githubusercontent.com/PX4/Devguide/master/build_scripts/ubuntu_sim_ros_melodic.sh
gazebo
px4 Firmware
ROS melodic 
mavros
rosbridge weboscket  http://wiki.ros.org/rosbridge_suite
qground control http://qgroundcontrol.com/

You need to have knowledge of how to create a rospy workspace
 
 https://github.com/Rodolfo9706/Firmware.git
 https://dev.px4.io/master/en/setup/dev_env_linux_ubuntu.html#rosgazebo
 
Unity VR:
unity 
visual studio
c#

RUN program
Once the px4 Firmware is downloaded replace the following files:
replace the 

Replace the typhoon_h480 folder located in src / Firmware / tools / sitl_gazebo / models with the one located in Firmware_verdin of the same name.
Replace rate_control.cpp located at src / Firmware / src / modules / mc_rate_control / ratecontrol
do the same with logger and vmount


workspaces for rospy
For this step, you need the knowledge to create a workspace using rospy 
Once created you will only need the .py files

To run roslaunch it is necessary to run in the terminal:

$source Tools/setup_gazebo.bash $(pwd) $(pwd)/build/px4_sitl_default
$export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)
$export ROS_PACKAGE_PATH=$ROS_PACKAGE_PATH:$(pwd)/Tools/sitl_gazebo

$roslaunch px4 mavros posix sitl.launch vehicle:=typhoon h480

The aerial-manipulator deal model will open immediately,  to control the vehicle you can use qgroundcontrol and arm.


VR comunnication:
To control it with the virtual environment you need to run the model in unity, once downloaded.
Run the topics: $rosrun pos_data pos_data.py
                $rosrun brazo brazo.py
Runwebsocket:
$roslaunch rosbridge_server rosbridge_websocket.launch


