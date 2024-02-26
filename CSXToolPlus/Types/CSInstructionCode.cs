﻿namespace CSXToolPlus.Types
{
    public enum CSInstructionCode
    {
        csicNew = 0,
        csicFree = 1,
        csicLoad = 2,
        csicStore = 3,
        csicEnter = 4,
        csicLeave = 5,
        csicJump = 6,
        csicCJump = 7,
        csicCall = 8,
        csicReturn = 9,
        csicElement = 10,
        csicElementIndirect = 11,
        csicOperate = 12,
        csicUniOperate = 13,
        csicCompare = 14,
        csicExOperate = 15,
        csicExUniOperate = 16,
        csicExCall = 17,
        csicExReturn = 18,
        csicCallMember = 19,
        csicCallNativeMember = 20,
        csicSwap = 21,
        csicReferenceForPointer = 26,
        csicCallNativeFunction = 29,

        // Shell
        codeLoadMem = 0x80,
        codeLoadMemBase = codeLoadMem,
        codeLoadMemBaseImm32,
        codeLoadMemBaseIndex,
        codeLoadMemBaseIndexImm32,
        codeStoreMem = 0x84,
        codeStoreMemBase = codeStoreMem,
        codeStoreMemBaseImm32,
        codeStoreMemBaseIndex,
        codeStoreMemBaseIndexImm32,
        codeLoadLocal = 0x88,
        codeLoadLocalImm32 = codeLoadLocal,
        codeLoadLocalIndexImm32,
        codeStoreLocal = 0x8A,
        codeStoreLocalImm32 = codeStoreLocal,
        codeStoreLocalIndexImm32,
        codeMoveReg = 0x90,
        codeCvtFloat2Int = 0x92,
        codeCvtInt2Float = 0x93,
        codeSrlImm8 = 0x94,
        codeSraImm8,
        codeSllImm8,
        codeMaskMove,
        codeAddImm32 = 0x98,
        codeMulImm32,
        codeAddSPImm32 = 0x9A,
        codeLoadImm64 = 0x9B,
        codeNegInt = 0x9C,
        codeNotInt,
        codeNegFloat,
        codeAddReg = 0xA0,
        codeSubReg,
        codeMulReg,
        codeDivReg,
        codeModReg,
        codeAndReg,
        codeOrReg,
        codeXorReg,
        codeSrlReg,
        codeSraReg,
        codeSllReg,
        codeMoveSx32Reg = 0xAB,
        codeMoveSx16Reg,
        codeMoveSx8Reg,
        codeFAddReg = 0xB0,
        codeFSubReg,
        codeFMulReg,
        codeFDivReg,
        codeMul32Reg = 0xB8,
        codeIMul32Reg,
        codeDiv32Reg,
        codeIDiv32Reg,
        codeMod32Reg,
        codeIMod32Reg,
        codeCmpNeReg = 0xC0,
        codeCmpEqReg,
        codeCmpLtReg,
        codeCmpLeReg,
        codeCmpGtReg,
        codeCmpGeReg,
        codeCmpCReg,
        codeCmpCZReg,
        codeFCmpNeReg = 0xC8,
        codeFCmpEqReg,
        codeFCmpLtReg,
        codeFCmpLeReg,
        codeFCmpGtReg,
        codeFCmpGeReg,
        codeJumpOffset32 = 0xD0,
        codeJumpReg = 0xD1,
        codeCNJumpOffset32 = 0xD2,
        codeCJumpOffset32,
        codeCallImm32 = 0xD4,
        codeCallReg = 0xD5,
        codeSysCallImm32 = 0xD6,
        codeSysCallReg = 0xD7,
        codeReturn = 0xD8,
        codePushReg = 0xDC,
        codePopReg,
        codePushRegs,
        codePopRegs,
        codeMemoryHint = 0xE0,
        codeFloatExtension,
        codeSIMD64Extension2Op,
        codeSIMD64Extension3Op,
        codeSIMD128Extension2Op,
        codeSIMD128Extension3Op,
        codeEscape = 0xFD,
        codeNoOperation = 0xFE,
        codeSystemReserved = 0xFF,
    }
}