﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BucketGo
{
    //1,2,3,4号进入的时间分散为1,2,3,4秒，即1000,2000,3000,4000毫秒
    public class token_bucket
    {
        public int rate = 40;//令牌产生速率
        public int[] color = new int[50];//packet颜色
        public int bucket_size = 50;//桶的大小
        //若packet_size[i]不为0，则有包在i秒进入并被标为color[i]
        public int[] packet_size = new int[50]; //{ 20, 10, 50, 60 };//packet分组的大小
        public int[] token_left_array = new int[50];
        public int[] token_before_array = new int[50];

        public token_bucket()
        {
            //4表示1,2,3,4四个包在1,2,3,4秒进入

            
        }

        public void calculate(int time)
        {
            set_color(time);
            set_left(time);
        }
        public void set_color(int time)
        {
            int token_left = 0;
            for (int i = 0; i < time; i++)
            {

                if (packet_size[i] == 0)
                {
                    color[i] = -1;
                    token_left += rate;
                }
                else
                {
                    token_left += rate;

                    if (token_left > bucket_size)
                    {
                        token_left = bucket_size;
                    }
                    if (token_left < packet_size[i])
                    {
                        color[i] = 0;
                    }
                    else
                    {
                        color[i] = 1;
                        token_left -= packet_size[i];
                    }

                }
            }
        }

        public void set_left(int time)
        {
            int token_left = 0;
            token_before_array[0] = 0;
            for (int i = 0; i < time; i++)
            {

                token_left += rate;

                if (token_left > bucket_size)
                {
                    token_left = bucket_size;
                }
                if (token_left < packet_size[i])
                {
                    token_before_array[i] = token_left;
                    token_left_array[i] = token_left;
                }
                else
                {
                    token_before_array[i] = token_left;
                    token_left -= packet_size[i];
                    token_left_array[i] = token_left;
                }

            }
        }

    };

    public class Timeline
    {
        public const int RED = 101;
        public const int GREEN = 102;
        //目前时间线大小为1000个100毫秒单位即，100秒
        //根据时间线定义每个时刻的Bucket大小
        public int MaxSize = 0;
        public List<int> BucketTimeLine = new List<int>(1000);
        public List<int> BucketShowTimeLine = new List<int>(1000);
        //public List<bool> IfPacketTimeLine = new List<bool>(1000);
        public List<int> PacketGoTimeLine = new List<int>(1000);
        //Size的选择范围是1,2，3
        public List<int> PacketGoTimeLine_Size = new List<int>(1000);
        public Timeline()
        {
            //初始化BucketTimeLine
            for(int i = 0; i < 1000; i++)
            {
                BucketTimeLine.Add(0);
            }
            //getShowTimeLine();
            for (int i = 0; i < 1000; i++)
            {
                PacketGoTimeLine.Add(0);
            }
            for (int i = 0; i < 1000; i++)
            {
                PacketGoTimeLine_Size.Add(0);
            }

        }
        //在原始数列获取好后，调用本函数转化为以1~16大小区间表示的桶大小
        //因为最终展示的时候桶大小的范围只是1~16
        public void getShowTimeLine()
        {
            int max = MaxSize;
            int min = 0;
            double double_div = (double)(max - min) / (double)16;
            int div = (int)double_div;
            for (int i = 0; i < 1000; i++)
            {
                double temp = (double)BucketTimeLine[i] / (double)max * (double)16;
                BucketShowTimeLine.Add((int)temp);
            }
        }
    }
}
