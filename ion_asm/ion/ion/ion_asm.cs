using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ion
{
	public class Assembler
	{
		//Compiled bytecode
		private List<Core.Instruction> m_instructions;

		//Language
		private char[] expressionSeparators = { '\n', ';' };

		//Currently assembling instruction
		private int m_parsingExpression = 0;

		public List<Core.Instruction> Assemble(string source)
		{
			m_instructions = new List<Core.Instruction>();
			m_parsingExpression = 0;

			string[] expressions = source.Split(expressionSeparators);

			foreach (string expression in expressions)
			{
				AssembleExpression(expression);
				m_parsingExpression++;
			}

			CompileMessage("Finished.");

			return m_instructions;
		}

		protected bool InterpretExpression(string expression, string error)
		{
			return false;
		}

		protected void CompileMessage(string messageText)
		{

		}

		protected void CompileError(string errorText)
		{

		}

		protected void AssembleExpression(string expression)
		{
			switch (expression[0])
			{
				case 'M':
					//MOVE
					ParseMove(expression.Substring(1));
					break;
				case 'A':
					//ADD
					ParseAdd(expression.Substring(1));
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
			int numParamsRequired_REG_REG = Enum.GetNames(typeof(Core.OpParamsMOV_REG_REG)).Count();
			int numParamsRequired_LIT_REG = Enum.GetNames(typeof(Core.OpParamsMOV_LIT_REG)).Count();

			if (numParamsProvided == numParamsRequired_REG_REG || numParamsProvided == numParamsRequired_LIT_REG)
			{
				Core.Instruction instruction = new Core.Instruction();
				Core.ParamType sourceParamType;

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

				if (sourceParamType == Core.ParamType.PARAM_REGISTER)
				{
					//Register to register MOV
					instruction.opcode = (byte)Core.Opcodes.OP_MOV_REG_REG;
				}
				else if (sourceParamType == Core.ParamType.PARAM_LITERAL)
				{
					//Literal to register MOV
					instruction.opcode = (byte)Core.Opcodes.OP_MOV_LIT_REG;
				}

				m_instructions.Add(instruction);
			}
			else
			{
				CompileError(String.Format("MOV - Instruction takes {0} parameters, not {1}", numParamsRequired_REG_REG, numParamsProvided));
			}
		}

		unsafe protected void ParseAdd(string expression)
		{
			string[] tokens = expression.Split(',');

			int numParamsProvided = tokens.Count();
			int numParamsRequired_REG_REG = Enum.GetNames(typeof(Core.OpParamsADD_REG_REG)).Count();
			int numParamsRequired_LIT_REG = Enum.GetNames(typeof(Core.OpParamsADD_LIT_REG)).Count();

			if (numParamsProvided == numParamsRequired_REG_REG || numParamsProvided == numParamsRequired_LIT_REG)
			{
				Core.Instruction instruction = new Core.Instruction();
				Core.ParamType sourceParamType;

				//Parse source param
				if (!ParseSourceParam(tokens[0], out instruction.parameters[0], out sourceParamType))
				{
					CompileError(String.Format("ADD - Error parsing parameter 0"));
				}

				//Parse dest param
				if (!int.TryParse(tokens[1], out instruction.parameters[1]))
				{
					CompileError(String.Format("ADD - Error parsing parameter 1"));
				}

				if (sourceParamType == Core.ParamType.PARAM_REGISTER)
				{
					//Register to register ADD
					instruction.opcode = (byte)Core.Opcodes.OP_ADD_REG_REG;
				}
				else if (sourceParamType == Core.ParamType.PARAM_LITERAL)
				{
					//Literal to register ADD
					instruction.opcode = (byte)Core.Opcodes.OP_ADD_LIT_REG;
				}

				m_instructions.Add(instruction);
			}
			else
			{
				CompileError(String.Format("ADD - Instruction takes {0} parameters, not {1}", numParamsRequired_REG_REG, numParamsProvided));
			}
		}

		protected bool ParseSourceParam(string param, out int number, out Core.ParamType paramType)
		{
			if (param[0] == '#')
			{
				//Literal number
				paramType = Core.ParamType.PARAM_LITERAL;
				return int.TryParse(param.Substring(1), out number);
			}
			else
			{
				//Register
				paramType = Core.ParamType.PARAM_REGISTER;
				return int.TryParse(param, out number);
			}
		}
	}
}