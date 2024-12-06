﻿namespace BDAS_2_dog_shelter
{
    internal enum Permissions : ulong
    {
        PES_SELECT =            0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001,
        PES_UPDATE =            0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000010,
        PES_INSERT =            0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000100,
        PES_DELETE =            0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000,

        KARANTENA_SELECT =      0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00010000,
        KARANTENA_UPDATE =      0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00100000,
        KARANTENA_INSERT =      0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_01000000,
        KARANTENA_DELETE =      0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000000,

        HRACKA_SELECT =         0b00000000_00000000_00000000_00000000_00000000_00000000_00000001_00000000,
        HRACKA_UPDATE =         0b00000000_00000000_00000000_00000000_00000000_00000000_00000010_00000000,
        HRACKA_INSERT =         0b00000000_00000000_00000000_00000000_00000000_00000000_00000100_00000000,
        HRACKA_DELETE =         0b00000000_00000000_00000000_00000000_00000000_00000000_00001000_00000000,

        UTULEK_PAVILON_SELECT = 0b00000000_00000000_00000000_00000000_00000000_00000000_00010000_00000000,
        UTULEK_PAVILON_UPDATE = 0b00000000_00000000_00000000_00000000_00000000_00000000_00100000_00000000,
        UTULEK_PAVILON_INSERT = 0b00000000_00000000_00000000_00000000_00000000_00000000_01000000_00000000,
        UTULEK_PAVILON_DELETE = 0b00000000_00000000_00000000_00000000_00000000_00000000_10000000_00000000,
        
        KRMIVO_SELECT =         0b00000000_00000000_00000000_00000000_00000000_00000001_00000000_00000000,
        KRMIVO_UPDATE =         0b00000000_00000000_00000000_00000000_00000000_00000010_00000000_00000000,
        KRMIVO_INSERT =         0b00000000_00000000_00000000_00000000_00000000_00000100_00000000_00000000,
        KRMIVO_DELETE =         0b00000000_00000000_00000000_00000000_00000000_00001000_00000000_00000000,

        UTULEK_SELECT =         0b00000000_00000000_00000000_00000000_00000000_00010000_00000000_00000000,
        UTULEK_UPDATE =         0b00000000_00000000_00000000_00000000_00000000_00100000_00000000_00000000,
        UTULEK_INSERT =         0b00000000_00000000_00000000_00000000_00000000_01000000_00000000_00000000,
        UTULEK_DELETE =         0b00000000_00000000_00000000_00000000_00000000_10000000_00000000_00000000,

        // LOGS

        PAVILON_SELECT =        0b00000000_00000000_00000000_00000000_00000001_00000000_00000000_00000000,
        PAVILON_UPDATE =        0b00000000_00000000_00000000_00000000_00000010_00000000_00000000_00000000,
        PAVILON_INSERT =        0b00000000_00000000_00000000_00000000_00000100_00000000_00000000_00000000,
        PAVILON_DELETE =        0b00000000_00000000_00000000_00000000_00001000_00000000_00000000_00000000,

        ADRESA_SELECT =         0b00000000_00000000_00000000_00000000_00010000_00000000_00000000_00000000,
        ADRESA_UPDATE =         0b00000000_00000000_00000000_00000000_00100000_00000000_00000000_00000000,
        ADRESA_INSERT =         0b00000000_00000000_00000000_00000000_01000000_00000000_00000000_00000000,
        ADRESA_DELETE =         0b00000000_00000000_00000000_00000000_10000000_00000000_00000000_00000000,
    
        ZDRAVOTNICKY_MATERIAL_SELECT=0b00000000_00000000_00000000_00000000_00000001_00000000_00000000_00000000,
        ZDRAVOTNICKY_MATERIAL_UPDATE=0b00000000_00000000_00000000_00000000_00000010_00000000_00000000_00000000,
        ZDRAVOTNICKY_MATERIAL_INSERT=0b00000000_00000000_00000000_00000000_00000100_00000000_00000000_00000000,
        ZDRAVOTNICKY_MATERIAL_DELETE=0b00000000_00000000_00000000_00000000_00001000_00000000_00000000_00000000,

        MAJITEL_SELECT =         0b00000000_00000000_00000000_00000000_00010000_00000000_00000000_00000000,
        MAJITEL_UPDATE =         0b00000000_00000000_00000000_00000000_00100000_00000000_00000000_00000000,
        MAJITEL_INSERT =         0b00000000_00000000_00000000_00000000_01000000_00000000_00000000_00000000,
        MAJITEL_DELETE =         0b00000000_00000000_00000000_00000000_10000000_00000000_00000000_00000000,
        
        SKLAD_SELECT =           0b00000000_00000000_00000000_00000001_00000000_00000000_00000000_00000000,
        SKLAD_UPDATE =           0b00000000_00000000_00000000_00000010_00000000_00000000_00000000_00000000,
        SKLAD_INSERT =           0b00000000_00000000_00000000_00000100_00000000_00000000_00000000_00000000,
        SKLAD_DELETE =           0b00000000_00000000_00000000_00001000_00000000_00000000_00000000_00000000,

        UTULEK_SKLAD_SELECT =    0b00000000_00000000_00000000_00010000_00000000_00000000_00000000_00000000,
        UTULEK_SKLAD_UPDATE =    0b00000000_00000000_00000000_00100000_00000000_00000000_00000000_00000000,
        UTULEK_SKLAD_INSERT =    0b00000000_00000000_00000000_01000000_00000000_00000000_00000000_00000000,
        UTULEK_SKLAD_DELETE =    0b00000000_00000000_00000000_10000000_00000000_00000000_00000000_00000000,

        //DOG_IMAGES

        //USERS 

        REZERVACE_SELECT =       0b00000000_00000000_00000001_00000000_00000000_00000000_00000000_00000000,
        REZERVACE_UPDATE =       0b00000000_00000000_00000010_00000000_00000000_00000000_00000000_00000000,
        REZERVACE_INSERT =       0b00000000_00000000_00000100_00000000_00000000_00000000_00000000_00000000,
        REZERVACE_DELETE =       0b00000000_00000000_00001000_00000000_00000000_00000000_00000000_00000000,

        ZDR_ZAZNAM_SELECT =      0b00000000_00000000_00010000_00000000_00000000_00000000_00000000_00000000,
        ZDR_ZAZNAM_UPDATE =      0b00000000_00000000_00100000_00000000_00000000_00000000_00000000_00000000,
        ZDR_ZAZNAM_INSERT =      0b00000000_00000000_01000000_00000000_00000000_00000000_00000000_00000000,
        ZDR_ZAZNAM_DELETE =      0b00000000_00000000_10000000_00000000_00000000_00000000_00000000_00000000,

        HISTORIE_PSA_SELECT =    0b00000000_00000001_00000000_00000000_00000000_00000000_00000000_00000000,
        HISTORIE_PSA_UPDATE =    0b00000000_00000010_00000000_00000000_00000000_00000000_00000000_00000000,
        HISTORIE_PSA_INSERT =    0b00000000_00000100_00000000_00000000_00000000_00000000_00000000_00000000,
        HISTORIE_PSA_DELETE =    0b00000000_00001000_00000000_00000000_00000000_00000000_00000000_00000000,

        PROCEDURA_SELECT =       0b00000000_00010000_00000000_00000000_00000000_00000000_00000000_00000000,
        PROCEDURA_UPDATE =       0b00000000_00100000_00000000_00000000_00000000_00000000_00000000_00000000,
        PROCEDURA_INSERT =       0b00000000_01000000_00000000_00000000_00000000_00000000_00000000_00000000,
        PROCEDURA_DELETE =       0b00000000_10000000_00000000_00000000_00000000_00000000_00000000_00000000,
        
        TYPES_SELECT =           0b00000001_00000000_00000000_00000000_00000000_00000000_00000000_00000000,

        ADMIN =                  0b10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000
    }
    class Permission
    {
        internal static bool HasAnyOf(ulong userPermissions, params Permissions[] permission)
        {
            return (permission.Aggregate<Permissions,ulong,ulong>(0ul, (b, c) =>  b | ((ulong)c),c=>c ) & userPermissions) > 0;
        }
        internal static bool HasAllOf(ulong userPermissions, params Permissions[] permission)
        {
            return (permission.Aggregate<Permissions,ulong,ulong>(0ul, (b, c) =>  b | ((ulong)c),c=>c ) | userPermissions) == userPermissions;
        }
} }