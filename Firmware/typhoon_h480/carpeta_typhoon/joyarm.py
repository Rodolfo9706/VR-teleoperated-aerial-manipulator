#!/usr/bin/env python
# license removed for brevity

import rospy
import numpy as np
from std_msgs.msg import Header
from sensor_msgs.msg import Joy
from mavros_msgs.msg import MountControl
from mavros_msgs.msg import *
from mavros_msgs.srv import *


def control():
      sub = rospy.Subscriber('joy', Joy, control.joyCb) 
      mensaje1= Joy
    

      while not rospy.is_shutdown():
            mensaje1.header.seq=0
            mensaje1.header.stamp=rospy.Time.now()
            mensaje1.header.frame_id = ' ' 

            mensaje1.axes=[0.0,0.0,0.0]
            mensaje1.buttons=[0,0,0,0,0,0,0,0,0,0,0]
                      
            y=mensaje1.axes[0]
            z=mensaje1.axes[1]
            x=mensaje1.axes[2]

       #    hello_str = "hello world %s" % rospy.get_time()
        #   rospy.loginfo(hello_str)
            pub.publish(mensaje1)
            rate.sleep()

def odo_data():
       pub = rospy.Publisher('/mavros/mount_control/command', MountControl, queue_size=20)  
       rospy.init_node('odo_data', anonymous=False)
       rate = rospy.Rate(30) # 10hz
       mensaje=MountControl()

       while not rospy.is_shutdown():


            mensaje.header.seq=0
            mensaje.header.stamp=rospy.Time.now()
            mensaje.header.frame_id = ' ' 

            mensaje.mode=2
            mensaje.pitch= y
            mensaje.roll = z
            mensaje.yaw = x

       #    hello_str = "hello world %s" % rospy.get_time()
        #   rospy.loginfo(hello_str)
            pub.publish(mensaje)
            rate.sleep()
   
if __name__ == '__main__':
       try:
           odo_data()
       except rospy.ROSInterruptException:
           pass
