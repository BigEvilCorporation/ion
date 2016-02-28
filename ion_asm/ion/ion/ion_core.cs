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
			OP_MOV_REG_REG = 0,	// Move reg to reg
			OP_MOV_LIT_REG,		// Move literal to reg
			OP_ADD_REG_REG,		// Add reg1 to reg2, store in reg2
			OP_ADD_LIT_REG,		// Add literal to reg, store in reg
			//OP_SUB_REG_REG,
			//OP_SUB_LIT_REG,
			//OP_CMP_REG_REG,
			//OP_CMP_LIT_REG,
			//OP_JMP_GT_DST,
			//OP_JMP_LT_DST,
		};

		//Move reg to reg
		public enum OpParamsMOV_REG_REG
		{
			OP_MOV_SOURCE = 0,
			OP_MOV_DEST
		}

		//Move literal to reg
		public enum OpParamsMOV_LIT_REG
		{
			OP_MOV_SOURCE = 0,
			OP_MOV_DEST
		}

		//Add reg to reg
		public enum OpParamsADD_REG_REG
		{
			OP_MOV_SOURCE = 0,
			OP_MOV_DEST
		}

		//Add literal to reg
		public enum OpParamsADD_LIT_REG
		{
			OP_MOV_SOURCE = 0,
			OP_MOV_DEST
		}

		//Param types
		public enum ParamType
		{
			PARAM_REGISTER = 0,
			PARAM_LITERAL
		}

		const int maxParams = 3;

		unsafe public struct Instruction
		{
			public byte opcode;
			public fixed int parameters[maxParams];
		};
	}
}