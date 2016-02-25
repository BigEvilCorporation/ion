using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

namespace ion
{
	public partial class _Default : Page
	{
		//System memory
		const int memoryTotalBytes = 64;
		public int[] m_memory;

		//Memory visualiser
		DataTable memoryDataTable;

		//Bytecode
		enum Opcodes
		{
			OP_MOV_REG_REG = 0,	// Move reg to reg
			OP_MOV_LIT_REG,		// Move literal to reg
			OP_ADD_REG_REG,		// Add reg1 to reg2, store in reg3
			OP_ADD_LIT_REG,		// Add literal to reg, store in reg
			OP_ADD_REG_REG_REG,	// Add reg1 to reg2, store in reg3
			OP_ADD_LIT_REG_REG,	// Add literal to reg1, store in reg2
			OP_SUB_REG_REG,
			OP_SUB_LIT_REG,
			OP_SUB_REG_REG_REG,
			OP_SUB_LIT_REG_REG,
			OP_CMP_REG_REG,
			OP_CMP_LIT_REG,
			OP_JMP_GT_DST,
			OP_JMP_LT_DST,
		};

		//Move reg to reg
		enum OpParamsMOV_REG_REG
		{
			OP_MOV_SOURCE = 0,
			OP_MOV_DEST
		}

		//Move literal to reg
		enum OpParamsMOV_LIT_REG
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

		unsafe protected struct Instruction
		{
			public byte opcode;
			public fixed int parameters[maxParams];
		};

		//Compiled bytecode
		List<Instruction> m_instructions;

		//Language
		char[] expressionSeparators = {'\n', ';'};

		//Compiler
		int m_parsingExpression = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			memoryDataTable = new DataTable();

			int memoryTableSizeSqrt = (int)Math.Sqrt(memoryTotalBytes);

			//Create all rows and columns
			for (int y = 0; y < memoryTableSizeSqrt; y++)
			{
				memoryDataTable.Columns.Add();
				memoryDataTable.Rows.Add();
			}

			//Fill default data
			for (int x = 0; x < memoryTableSizeSqrt; x++)
			{
				for (int y = 0; y < memoryTableSizeSqrt; y++)
				{
					memoryDataTable.Rows[y][x] = "0";
				}
			}

			//Bind data to grid view
			gridMemory.DataSource = memoryDataTable;
			gridMemory.DataBind();
		}

		protected void txtEditor_TextChanged(object sender, EventArgs e)
		{

		}

		protected void gridMemory_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		protected void PopulateMemoryVisualiser()
		{
			int memoryTableSizeSqrt = (int)Math.Sqrt(memoryTotalBytes);

			for (int x = 0; x < memoryTableSizeSqrt; x++)
			{
				for (int y = 0; y < memoryTableSizeSqrt; y++)
				{
					memoryDataTable.Rows[y][x] = m_memory[(y * memoryTableSizeSqrt) + x].ToString();
				}
			}

			gridMemory.DataSource = memoryDataTable;
			gridMemory.DataBind();
		}

		protected bool InterpretExpression(string expression, string error)
		{
			return false;
		}

		protected void Compile()
		{
			m_instructions = new List<Instruction>();
			m_parsingExpression = 0;

			string[] expressions = txtEditor.Text.Split(expressionSeparators);

			foreach(string expression in expressions)
			{
				CompileExpression(expression);
				m_parsingExpression++;
			}

			CompileMessage("Finished.");
		}

		protected void CompileMessage(string messageText)
		{

		}

		protected void CompileError(string errorText)
		{

		}

		protected void CompileExpression(string expression)
		{
			switch (expression[0])
			{
				case 'M':
					//MOVE
					ParseMove(expression.Substring(1));
					break;
				case 'A':
					//ADD
					break;
				case 'S':
					//SUBTRACT
					break;
				case 'I':
					//If
					break;
				case 'J':
					//JUMP
					break;
			}
		}

		unsafe protected void ParseMove(string expression)
		{
			string[] tokens = expression.Split(',');

			int numParamsProvided = tokens.Count();
			int numParamsRequired_REG_REG = Enum.GetNames(typeof(OpParamsMOV_REG_REG)).Count();
			int numParamsRequired_LIT_REG = Enum.GetNames(typeof(OpParamsMOV_LIT_REG)).Count();

			if (numParamsProvided == numParamsRequired_REG_REG || numParamsProvided == numParamsRequired_LIT_REG)
			{
				Instruction instruction = new Instruction();
				ParamType sourceParamType;

				//Parse source param
				if (!ParseSourceParam(tokens[0], out instruction.parameters[0], out sourceParamType))
				{
					CompileError(String.Format("MOV - Error parsing parameter 0"));
				}

				//Parse dest param
				if (!int.TryParse(tokens[1], out instruction.parameters[1]))
				{
					CompileError(String.Format("MOV - Error parsing parameter 1"));
				}

				if (sourceParamType == ParamType.PARAM_REGISTER)
				{
					//Register to register MOV
					instruction.opcode = (byte)Opcodes.OP_MOV_REG_REG;
				}
				else if (sourceParamType == ParamType.PARAM_LITERAL)
				{
					//Literal to register MOV
					instruction.opcode = (byte)Opcodes.OP_MOV_LIT_REG;
				}

				m_instructions.Add(instruction);
			}
			else
			{
				CompileError(String.Format("MOV - Instruction takes {0} parameters, not {1}", numParamsRequired_REG_REG, numParamsProvided));
			}
		}

		protected bool ParseSourceParam(string param, out int number, out ParamType paramType)
		{
			if(param[0] == '#')
			{
				//Literal number
				paramType = ParamType.PARAM_LITERAL;
				return int.TryParse(param.Substring(1), out number);
			}
			else
			{
				//Register
				paramType = ParamType.PARAM_REGISTER;
				return int.TryParse(param, out number);
			}
		}

		protected void Execute()
		{
			//Initialise virtual machine
			m_memory = new int[memoryTotalBytes];

			foreach(Instruction instruction in m_instructions)
			{
				ExecuteInstruction(instruction);
			}
		}

		unsafe protected void ExecuteInstruction(Instruction instruction)
		{
			switch(instruction.opcode)
			{
				case (byte)Opcodes.OP_MOV_REG_REG:
				{
					int value = 0;
					uint sourceAddress = (uint)instruction.parameters[(int)OpParamsMOV_REG_REG.OP_MOV_SOURCE];
					uint destAddress = (uint)instruction.parameters[(int)OpParamsMOV_REG_REG.OP_MOV_DEST];
					Load(sourceAddress, out value);
					Store(destAddress, value);
					break;
				}
					

				case (byte)Opcodes.OP_MOV_LIT_REG:
				{
					int value = (int)instruction.parameters[(int)OpParamsMOV_REG_REG.OP_MOV_SOURCE];
					uint destAddress = (uint)instruction.parameters[(int)OpParamsMOV_REG_REG.OP_MOV_DEST];
					Store(destAddress, value);
					break;
				}
			}
		}

		protected void Load(uint address, out int value)
		{
			value = m_memory[address];
		}

		protected void Store(uint address, int value)
		{
			m_memory[address] = value;
		}

		protected void btnCompile_Click(object sender, EventArgs e)
		{
			Compile();
			Execute();
			PopulateMemoryVisualiser();
		}
	}
}