using System;
using System.Collections.Generic;
using System.Data;

namespace ion
{
	public class Core
	{
		//Opcodes
		public enum Opcodes
		{
			MoveRegReg = 0,	// Move reg to reg
			MoveLitReg,		// Move literal to reg
			AddRegReg,		// Add reg1 to reg2, store in reg2
			AddLitReg,		// Add literal to reg, store in reg
			SubRegReg,		// Sub reg1 from reg2, store in reg2
			SubLitReg,		// Sub literal from reg, store in reg
			//OP_CMP_REG_REG,
			//OP_CMP_LIT_REG,
			//OP_JMP_GT_DST,
			//OP_JMP_LT_DST,
		};

		//Source + dest params
		public enum OpParamsSourceDest
		{
			Source = 0,
			Dest
		}

		//Param types
		public enum ParamType
		{
			Register = 0,
			Literal
		}

		const int maxParams = 3;

		unsafe public struct Instruction
		{
			public byte opcode;
			public fixed int parameters[maxParams];
		};
	}
}