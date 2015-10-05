# simserver
Unity3d Ros Interface

installs this packages if you have problem with unity exe
1) sudo apt-get install libxcursor1:i386
2) sudo apt-get install libxrandr2:i386
3) sudo apt-get install libxcursor
4) sudo apt-get install libxrandr2
5) sudo apt-get install mono-complete

Release Notes :
v1 => initial commit version
v2 => add ros node
v3 => add camera , quad get / set , IMU / GPS sim
v4 => add car animation service
v5 => Fix some bugs , auto connection , memory crash problems solved

Run Commands
1) roscore
2) rosrun simserver simserver
3) run catkin_ws/src/UNITYSIM/exe/v1.x86_64 with ./v1.x86_64

Unity Outputs

simserver/uav1/cam1 => cam1 output
simserver/uav1/cam2 => cam2 output
simserver/uav1/cam3 => cam3 output
simserver/uav1/posimu 
 x => position.x
 y => position.y
 z => position.z
 mx => rotation.x
 my => rotation.y
 mz => rotation.z

Unity Inputs
simserver/uav1/cmd_vel
 linear (x,y,z) => (X,Y,Hight)
 angular (z) => ( YAW )

anim_service
 play => play car animation
 stop => stop car animation
 speed,X => X [0,3] => car animaton speed
 goto,X => X [0,16] => set car to new location





