﻿namespace BDAS_2_dog_shelter
{
    internal enum Permissions : ulong
    {
                DOGS_SELECT = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001,
                DOGS_UPDATE = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000010,
                DOGS_INSERT = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000100,
                DOGS_DELETE = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000,

                USER_SELECT = 0b00010000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
                USER_UPDATE = 0b00100000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
                USER_INSERT = 0b01000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
                USER_DELETE = 0b10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000
    }
}