﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1lightcycle.UART
{
    public struct LcProtocolStruct
    {
        public byte address;
        public byte command;
        public ushort parameter;
    }

    public class LcProtocol
    {

        /* Address definitions */
        public const byte ADDRESS_SERVER = 0x00;    // locical address of lightcycle base
        public const byte ADDRESS_ROBOT_1 = 0x01;    // locical address of lightcycle bike 1
        public const byte ADDRESS_ROBOT_2 = 0x02;    // locical address of lightcycle bike 2
        public const byte ADDRESS_SPARE_1 = 0x03;    //
        public const byte ADDRESS_SPARE_2 = 0x04;    //
        public const byte ADDRESS_SPARE_3 = 0x05;    //
        public const byte ADDRESS_SPARE_4 = 0x06;    //
        public const byte ADDRESS_BROADCAST = 0x07;    // address all devices (broadcast)


        /* Command definitions */
        public const byte CMD_HEARTBEAT = 0x00;    // cyclic heartbeat from server
        public const byte CMD_FORWARD = 0x01;    // 
        public const byte CMD_REVERSE = 0x02;    //
        public const byte CMD_STOP = 0x03;    //
        public const byte CMD_TURN_RIGHT_STATIC = 0x04;    // turn in place; parm = 0...360 degrees
        public const byte CMD_TURN_LEFT_STATIC = 0x05;    // turn in place; parm = 0...360 degrees
        public const byte CMD_TURN_RIGHT_DYNAMIC = 0x06;    // turn byteo; parm = 0...90 degrees
        public const byte CMD_TURN_LEFT_DYNAMIC = 0x07;    // turn byteo; parm = 0...90 degrees
        public const byte CMD_SET_SPEED = 0x08;    // set speed which can be activated with forward or reverse
        public const byte CMD_SPARE_1 = 0x09;    // 
        public const byte CMD_SPARE_2 = 0x0A;    // 
        public const byte CMD_SPARE_3 = 0x0B;    // 
        public const byte CMD_SPARE_4 = 0x0C;    // 
        public const byte CMD_SPARE_5 = 0x0D;    // 
        public const byte CMD_SPARE_6 = 0x0E;    // 
        public const byte CMD_SPARE_7 = 0x0F;    // 

        public const byte HI = 0;
        public const byte LO = 1;

        // Bitmasks
        private static byte MASK_ADDRESS = 0xE0; // 0b11100000 address fields
        private static byte MASK_COMMAND = 0x1E; // 0b00011110 command fields
        private static byte MASK_PARAMETER_HI = 0x01;  // 0b00000001 parameter fields
        private static byte MASK_PARAMETER_LO = 0x00FF;  // 0b00000001 parameter fields
        private static ushort MASK_PARAMETER = 0x01FF;


        /*
        parse from the raw package the protocol struct

        package
        hi -----------> lo
        111 1111 1|11111111
        adr|c md|parameter
        */
        static LcProtocolStruct getProtocolStruct(char hi, char lo)
        {
            LcProtocolStruct protocol;
            protocol.address = (byte)((hi & MASK_ADDRESS) >> 5);
            protocol.command = (byte)((hi & MASK_COMMAND) >> 1);
            protocol.parameter = (ushort)(lo | ((hi & MASK_PARAMETER_HI) << 8));
            return protocol;
        }

        /*
        build a raw package by address, command and parameter
        data[HI] and data[LO] are the two bytes of the protocol which are returned
        */
        static byte[] buildProtocolData(byte address, byte command, ushort parameter)
        {
            byte[] data = new byte[2];
            byte hi = (byte)((parameter & MASK_PARAMETER) >> 8);
            hi |= (byte)(command << 1);
            hi |= (byte)(address << 5);
            data[HI] = hi;
            data[LO] = (byte)(parameter & MASK_PARAMETER_LO);
            return data;
        }

        public static byte[] buildProtocolData(LcProtocolStruct package)
        {
            return buildProtocolData(package.address, package.command, package.parameter);
        }
    }
}
