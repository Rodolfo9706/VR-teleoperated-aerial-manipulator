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

 
