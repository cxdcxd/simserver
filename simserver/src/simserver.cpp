
//ROS
#include <ros/package.h>
#include <image_transport/image_transport.h>
#include "ros/ros.h"
#include <tf/tf.h>
#include <tf/transform_listener.h>
#include "std_msgs/Int32.h"
#include "std_msgs/String.h"
#include "std_msgs/Float64.h"
#include "std_msgs/Bool.h"
#include "geometry_msgs/Twist.h"
#include <sensor_msgs/LaserScan.h>
#include <sensor_msgs/image_encodings.h>

#include <limits>
#include <fstream>
#include <vector>
#include <Eigen/Core>

#include <stdio.h>
#include <stdlib.h>
#include <tcpacceptor.h>
#include <tcpacceptor.hpp>
#include <tcpstream.hpp>
#include <string.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <iostream>
#include <fstream>
#include <errno.h>

#include <iostream>
#include <fstream>
#include <cstdlib>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <iostream>
#include <dirent.h>

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <iostream>

#include <ros/ros.h>
#include <image_transport/image_transport.h>
#include <opencv2/highgui/highgui.hpp>
#include <cv_bridge/cv_bridge.h>
#include <boost/algorithm/string.hpp>
#include <athomerobot_msgs/pos2.h>
#include <athomerobot_msgs/imu.h>
#include <sstream>
#include <stdio.h>
#include <iostream>
#include <vector>
#include <string>
#include <athomerobot_msgs/command.h>

ros::Publisher chatter_pub[20];

TCPStream* stream = NULL;
TCPAcceptor* acceptor1 = NULL;

bool mutex = false;
bool tcp_can = false;
float ratio_X = 15.2;
float ratio_Y = 15.3;
float ratio_W = 1800;

typedef unsigned char BYTE;

static const std::string base64_chars =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        "abcdefghijklmnopqrstuvwxyz"
        "0123456789+/";


static inline bool is_base64(BYTE c) {
    return (isalnum(c) || (c == '+') || (c == '/'));
}

std::vector<BYTE> base64_decode(std::string const& encoded_string) {
    int in_len = encoded_string.size();
    int i = 0;
    int j = 0;
    int in_ = 0;
    BYTE char_array_4[4], char_array_3[3];
    std::vector<BYTE> ret;

    while (in_len-- && ( encoded_string[in_] != '=') && is_base64(encoded_string[in_])) {
        char_array_4[i++] = encoded_string[in_]; in_++;
        if (i ==4) {
            for (i = 0; i <4; i++)
                char_array_4[i] = base64_chars.find(char_array_4[i]);

            char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
            char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
            char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

            for (i = 0; (i < 3); i++)
                ret.push_back(char_array_3[i]);
            i = 0;
        }
    }

    if (i) {
        for (j = i; j <4; j++)
            char_array_4[j] = 0;

        for (j = 0; j <4; j++)
            char_array_4[j] = base64_chars.find(char_array_4[j]);

        char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
        char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
        char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

        for (j = 0; (j < i - 1); j++) ret.push_back(char_array_3[j]);
    }

    return ret;
}

void process_cam1(string input)
{
    if ( mutex  ) return;
    mutex = true;

    vector<string> strs2;
    boost::split(strs2,input,boost::is_any_of(","));

    if ( strs2.size() > 1)
    {
        if ( strs2.at(0) == "CAM1" ||  strs2.at(0) == "CAM2" ||  strs2.at(0) == "CAM3" )
        {
            string data = strs2.at(1);
            //cout<<"GET CAM :" <<data<<endl;
            //=================================
            //base 64 to byte[]
            std::vector<BYTE> data_byte = base64_decode(data);
            //cout<<"GET SIZE :" << data_byte.size() << endl;


            cv::Mat data_mat(cv::imdecode(data_byte,1));
            cv_bridge::CvImage out_msg;
            out_msg.encoding = sensor_msgs::image_encodings::BGR8;
            out_msg.image = data_mat;
            if ( strs2.at(0) == "CAM1" )
                chatter_pub[3].publish(out_msg);
            if ( strs2.at(0) == "CAM2" )
                chatter_pub[4].publish(out_msg);
            if ( strs2.at(0) == "CAM3" )
                chatter_pub[5].publish(out_msg);


            //=================================

        }
    }


    mutex = false;
}

void process_cam2(string input)
{
    if ( mutex  ) return;
    mutex = true;

    vector<string> strs2;
    boost::split(strs2,input,boost::is_any_of(","));

    if ( strs2.size() > 1)
    {
        if ( strs2.at(0) == "CAM1" ||  strs2.at(0) == "CAM2" ||  strs2.at(0) == "CAM3" )
        {
            string data = strs2.at(1);
            //cout<<"GET CAM :" <<data<<endl;
            //=================================
            //base 64 to byte[]
            std::vector<BYTE> data_byte = base64_decode(data);
            //cout<<"GET SIZE :" << data_byte.size() << endl;


            cv::Mat data_mat(cv::imdecode(data_byte,1));
            cv_bridge::CvImage out_msg;
            out_msg.encoding = sensor_msgs::image_encodings::BGR8;
            out_msg.image = data_mat;
            if ( strs2.at(0) == "CAM1" )
                chatter_pub[3].publish(out_msg);
            if ( strs2.at(0) == "CAM2" )
                chatter_pub[4].publish(out_msg);
            if ( strs2.at(0) == "CAM3" )
                chatter_pub[5].publish(out_msg);


            //=================================

        }
    }


    mutex = false;
}

void process_cam3(string input)
{
    if ( mutex  ) return;
    mutex = true;

    vector<string> strs2;
    boost::split(strs2,input,boost::is_any_of(","));

    if ( strs2.size() > 1)
    {
        if ( strs2.at(0) == "CAM1" ||  strs2.at(0) == "CAM2" ||  strs2.at(0) == "CAM3" )
        {
            string data = strs2.at(1);
            //cout<<"GET CAM :" <<data<<endl;
            //=================================
            //base 64 to byte[]
            std::vector<BYTE> data_byte = base64_decode(data);
            //cout<<"GET SIZE :" << data_byte.size() << endl;


            cv::Mat data_mat(cv::imdecode(data_byte,1));
            cv_bridge::CvImage out_msg;
            out_msg.encoding = sensor_msgs::image_encodings::BGR8;
            out_msg.image = data_mat;
            if ( strs2.at(0) == "CAM1" )
                chatter_pub[3].publish(out_msg);
            if ( strs2.at(0) == "CAM2" )
                chatter_pub[4].publish(out_msg);
            if ( strs2.at(0) == "CAM3" )
                chatter_pub[5].publish(out_msg);


            //=================================

        }
    }


    mutex = false;
}

void process_commandx(string input)
{
    
    vector<string> strs2;
    boost::split(strs2,input,boost::is_any_of(","));

    if ( strs2.size() > 1)
    {

        if ( strs2.at(0) == "gps")
        {
            cout<<"GET GPS"<<endl;
            athomerobot_msgs::pos2 pos_msg;
            string xx = strs2.at(1);
            string yy = strs2.at(2);
            string zz = strs2.at(3);
            string xxx = strs2.at(4);
            string yyy = strs2.at(5);
            string zzz = strs2.at(6);

            pos_msg.x = atof(xx.c_str());
            pos_msg.y = atof(yy.c_str());
            pos_msg.z = atof(zz.c_str());
            pos_msg.mx = atof(xxx.c_str());
            pos_msg.my = atof(yyy.c_str());
            pos_msg.mz = atof(zzz.c_str());
            chatter_pub[0].publish(pos_msg);
        }
    }

}

void tcpsendX(string message)
{
    message =  message;
    if ( stream != NULL && tcp_can)
    {
        stream->send(message.c_str(),message.size());
        cout<<"TCP SEND DONE"<<endl;
    }
    else
    {
        cout<<"TCP SEND Failed"<<endl;
    }
}

int tcpserver_mainX()
{
    cout<<"SIM SERVER SERVER STARTED DONE"<<endl;
    //listener
    acceptor1 = new TCPAcceptor(4001);

    if (acceptor1->start() == 0) {


        while (1) {
            stream = acceptor1->accept();

            if (stream != NULL) {
                ssize_t len;
                char line[100];

                cout<<"Unity Sim main Connected"<<endl;
                tcp_can = true;
                int header = 0;
                string valid_data = "";

                //read
                while ((len = stream->receive(line, sizeof(line))) > 0) {
                    line[len] = 0;
                    

                    // %data$
                    for ( int i = 0 ; i < len ; i++)
                    {
                        if ( line[i] == '%' && header == 0)
                        {
                            header++;
                        }
                        else
                            if ( header == 1)
                            {
                                if ( line[i] != '$')
                                    valid_data += line[i];
                                else
                                {
                                    string temp = valid_data;

                                    //cout<<"GET :"<<temp<<endl;
                                    process_commandx(temp);
                                    valid_data = "";
                                    header = 0;
                                }
                            }
                    }
                }
                tcp_can = false;
                delete stream;
                cout<<"Unity Sim main Disconnected"<<endl;
            }
        }
    }

}

int tcpserver_main_cam1()
{
    TCPStream* stream = NULL;
    TCPAcceptor* acceptor = NULL;
    cout<<"SIM SERVER CAM1 SERVER STARTED DONE"<<endl;
    //listener
    acceptor = new TCPAcceptor(4010);

    if (acceptor->start() == 0) {

        while (1) {
            stream = acceptor->accept();

            if (stream != NULL) {
                ssize_t len;
                char line[100];

                cout<<"Unity Sim c1 Connected"<<endl;
                tcp_can = true;
                int header = 0;
                string valid_data = "";

                //read
                while ((len = stream->receive(line, sizeof(line))) > 0) {
                    line[len] = 0;


                    // %data$
                    for ( int i = 0 ; i < len ; i++)
                    {
                        if ( line[i] == '%' && header == 0)
                        {
                            header++;
                        }
                        else
                            if ( header == 1)
                            {
                                if ( line[i] != '$')
                                    valid_data += line[i];
                                else
                                {
                                    string temp = valid_data;
                                    process_cam1(temp);
                                    valid_data = "";
                                    header = 0;
                                }
                            }
                    }
                }
                tcp_can = false;
                delete stream;
                cout<<"Unity Sim c1 Disconnected"<<endl;
            }
        }
    }

}

int tcpserver_main_cam2()
{
    TCPStream* stream = NULL;
    TCPAcceptor* acceptor = NULL;
    cout<<"SIM SERVER CAM2 SERVER STARTED DONE"<<endl;
    //listener
    acceptor = new TCPAcceptor(4011);

    if (acceptor->start() == 0) {

        while (1) {
            stream = acceptor->accept();

            if (stream != NULL) {
                ssize_t len;
                char line[100];

                cout<<"Unity Sim c2 Connected"<<endl;
                tcp_can = true;
                int header = 0;
                string valid_data = "";

                //read
                while ((len = stream->receive(line, sizeof(line))) > 0) {
                    line[len] = 0;


                    // %data$
                    for ( int i = 0 ; i < len ; i++)
                    {
                        if ( line[i] == '%' && header == 0)
                        {
                            header++;
                        }
                        else
                            if ( header == 1)
                            {
                                if ( line[i] != '$')
                                    valid_data += line[i];
                                else
                                {
                                    string temp = valid_data;
                                    process_cam2(temp);
                                    valid_data = "";
                                    header = 0;
                                }
                            }
                    }
                }
                tcp_can = false;
                delete stream;
                cout<<"Unity Sim c2 Disconnected"<<endl;
            }
        }
    }

}

int tcpserver_main_cam3()
{
    TCPStream* stream = NULL;
    TCPAcceptor* acceptor = NULL;
    cout<<"SIM SERVER CAM3 SERVER STARTED DONE"<<endl;
    //listener
    acceptor = new TCPAcceptor(4012);

    if (acceptor->start() == 0) {

        while (1) {
            stream = acceptor->accept();

            if (stream != NULL) {
                ssize_t len;
                char line[100];

                cout<<"Unity Sim c3 Connected"<<endl;
                tcp_can = true;
                int header = 0;
                string valid_data = "";

                //read
                while ((len = stream->receive(line, sizeof(line))) > 0) {
                    line[len] = 0;


                    // %data$
                    for ( int i = 0 ; i < len ; i++)
                    {
                        if ( line[i] == '%' && header == 0)
                        {
                            header++;
                        }
                        else
                            if ( header == 1)
                            {
                                if ( line[i] != '$')
                                    valid_data += line[i];
                                else
                                {
                                    string temp = valid_data;
                                    process_cam3(temp);
                                    valid_data = "";
                                    header = 0;
                                }
                            }
                    }
                }
                tcp_can = false;
                delete stream;
                cout<<"Unity Sim c3 Disconnected"<<endl;
            }
        }
    }

}

int tcpserver_main_posimu()
{
    TCPStream* stream = NULL;
    TCPAcceptor* acceptor = NULL;
    cout<<"SIM SERVER CAM3 SERVER STARTED DONE"<<endl;
    //listener
    acceptor = new TCPAcceptor(4012);

    if (acceptor->start() == 0) {

        while (1) {
            stream = acceptor->accept();

            if (stream != NULL) {
                ssize_t len;
                char line[100];

                cout<<"Unity Sim c3 Connected"<<endl;
                tcp_can = true;
                int header = 0;
                string valid_data = "";

                //read
                while ((len = stream->receive(line, sizeof(line))) > 0) {
                    line[len] = 0;


                    // %data$
                    for ( int i = 0 ; i < len ; i++)
                    {
                        if ( line[i] == '%' && header == 0)
                        {
                            header++;
                        }
                        else
                            if ( header == 1)
                            {
                                if ( line[i] != '$')
                                    valid_data += line[i];
                                else
                                {
                                    string temp = valid_data;
                                    process_cam3(temp);
                                    valid_data = "";
                                    header = 0;
                                }
                            }
                    }
                }
                tcp_can = false;
                delete stream;
                cout<<"Unity Sim c3 Disconnected"<<endl;
            }
        }
    }

}

float convert_mps_vy(float mps) {
    return mps * (ratio_Y*100);
}

float convert_mps_vx(float mps) {
    return mps * (ratio_X*100);
}

float convert_radps_w(float w) {
    return w * (ratio_W);
}

void chatterCallback_cmd_vel(const geometry_msgs::Twist &twist_aux)
{
    double vel_x = twist_aux.linear.x;
    double vel_y =  twist_aux.linear.y;
    double vel_z =  twist_aux.linear.z;
    double vel_th =  twist_aux.angular.z;

    int xx = 0;
    int yy = 0;
    int ww = 0;

    //xx = (int)convert_mps_vx(vel_x);
    //yy = (int)convert_mps_vy(vel_y);
    //ww = (int)convert_radps_w(vel_th);

    std::stringstream ss;
    ss << vel_x  << "," << vel_y << "," << vel_z << "," << vel_th;
    string out = "set_control," + ss.str();
    //===================================================
    tcpsendX(out);
}

void get_loop()
{
    while(true)
    {
        if ( tcp_can )
        {
            //tcpsendX("get_gps");
            //boost::this_thread::sleep(boost::posix_time::milliseconds(20));

            //tcpsendX("get_imu");
            //boost::this_thread::sleep(boost::posix_time::milliseconds(20));
        }

        boost::this_thread::sleep(boost::posix_time::milliseconds(50));
    }
}

bool anim(athomerobot_msgs::command::Request  &req, athomerobot_msgs::command::Response &res)
{
  string a = req.command;
  tcpsendX(a);
  return true;
}

int main (int argc, char** argv)
{
    ros::init(argc, argv, "simserver");
    ros::Time::init();
    cout<<"Sim server started done V 1.0.0"<<endl;

    ros::Rate loop_rate(20);

    ros::NodeHandle node_handles[50];
    ros::Subscriber sub_handles[15];

    //=======================================

    //advertise
    chatter_pub[0] = node_handles[0].advertise<athomerobot_msgs::pos2>("simserver/uav1/pos", 10);
    chatter_pub[1] = node_handles[1].advertise<std_msgs::Float64>("simserver/uav1/ultrasound", 10);
    chatter_pub[2] = node_handles[2].advertise<athomerobot_msgs::imu>("simserver/uav1/imu", 10);
    chatter_pub[3] = node_handles[3].advertise<sensor_msgs::Image>("simserver/uav1/cam1", 10);
    chatter_pub[4] = node_handles[4].advertise<sensor_msgs::Image>("simserver/uav1/cam2", 10);
    chatter_pub[5] = node_handles[5].advertise<sensor_msgs::Image>("simserver/uav1/cam3", 10);
    //subscribe
    sub_handles[0] = node_handles[6].subscribe("simserver/uav1/cmd_vel", 10, chatterCallback_cmd_vel);


    boost::thread _thread_logic1(&tcpserver_main_cam1);
    boost::thread _thread_logic2(&tcpserver_main_cam2);
    boost::thread _thread_logic3(&tcpserver_main_cam3);
    boost::thread _thread_logic22(&tcpserver_mainX);
    boost::thread _thread_logic33(&get_loop);

    ros::NodeHandle n;
    ros::ServiceServer service = n.advertiseService("anim_service", anim);

    while (ros::ok())
    {
        ros::spinOnce();
        loop_rate.sleep();
    }


}
