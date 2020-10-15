#!/usr/bin/env python
import rospy
import numpy as np

from std_msgs.msg import String
from mavros_msgs.msg import MountControl
from mavros_msgs.msg import ActuatorControl 

def callback(data):
    global bot
    global mid
    global pin
    #rospy.loginfo(rospy.get_caller_id() + "I heard %s", data.data)
    b_d=str(data)
    vals=b_d.split()
    print(b_d)
    bot=vals[2]
    bot=(float(bot))
    mid=vals[3]
    mid=(float(mid))
    pin=vals[4]
    pin=(float(pin))
    
    print(pin, " ", mid, " ", bot)

def odo_data():
    
    pub = rospy.Publisher('/mavros/mount_control/command', MountControl, queue_size=10)
   #pub2 = rospy.Publisher('/mavros/actuator_control/command',ActuatorControl, queue_size=20) 

    rospy.init_node("brazo_data", anonymous=False)
    rospy.Subscriber("brazo_data", String, callback)
    rate = rospy.Rate(10) # 10hz
    mensaje=MountControl()
   #pwm = ActuatorControl() 

    while not rospy.is_shutdown():
     
      mensaje.header.seq = 0
     #pwm.header.seq = 0 
      mensaje.header.stamp = rospy.Time.now()
     #pwm.header.stamp=rospy.Time.now() 
      mensaje.header.frame_id = ''
     #pwm.header.frame_id = '' 

      mensaje.mode=2
  
      #pitch_angle = bot #input ("Ingresa el angulo pitch: ")
      mensaje.pitch = mid #pitch_angle
      #roll_angle = mid #input ("Ingresa el angulo roll: ")
      mensaje.roll = bot#roll_angle
      #yaw_angle = pin #input ("Ingresa el angulo yaw: ")
      mensaje.yaw = pin#yaw_angle
     # mensaje.roll = mid#roll_angle
      #mensaje.yaw = pin#yaw_angle
      print(pin, " ", mid, " ", bot)

       #hello_str = "hello world %s" % rospy.get_time()
       #rospy.loginfo(hello_str)
      pub.publish(mensaje)
      rate.sleep()

if __name__ == '__main__':
     bot=0
     mid=0
     pin=0
     try:
        odo_data()
     except rospy.ROSInterruptException:
       pass


