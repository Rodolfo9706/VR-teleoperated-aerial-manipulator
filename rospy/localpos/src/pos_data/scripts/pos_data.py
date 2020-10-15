#!/usr/bin/env python
import rospy
import sys
import time
from std_msgs.msg import String
from geometry_msgs.msg import PoseStamped

def callback(data):

    global b
    global x
    a=data.pose.position.x
    a2=data.pose.position.y
    a3=data.pose.position.z
    a4=data.pose.orientation.z
    x=a+a2+a3+a4
    pos=str(str(a) + " " + str(a2) + " " + str(a3) + " " + str(a4))
    #rospy.loginfo(str(pos))
    if(x < b-.2 or x > b+.2):
        b=x
        print("dafadfalduihfaldjhfladf")
        pub.publish(pos)
        print(pos)
    x=x+1
    #time.sleep(.1)
    #~print("xd")
    #print(x)
    #p.rint(b)

def talker():
    global pub
    pub = rospy.Publisher('servo', String, queue_size=10)
    rospy.init_node('servo', anonymous=False)
    rate = rospy.Rate(10) # 10hz
    while True:
    	rospy.Subscriber("/mavros/local_position/pose",PoseStamped, callback)
        rate.sleep()

print (time.strftime("%H:%M:%S:%M:%M"))
    	
if __name__ == '__main__':
    ##listener()
    b=0
    x=0
    talker()

