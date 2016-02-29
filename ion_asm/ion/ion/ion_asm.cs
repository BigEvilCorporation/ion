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

		//Error count
		private int m_errorCount;

		//Log
		List<string> m_log;

		public int Assemble(string source, out List<Core.Instruction> byteCode, out List<string> log)
		{
			m_instructions = new List<Core.Instruction>();
			m_log = new List<string>();
			m_parsingExpression = 0;
			m_errorCount = 0;

			string[] expressions = source.Split(expressionSeparators);

			foreach (string expression in expressions)
			{
				string strippedExpression = SanitiseExpression(expression);
				AssembleExpression(strippedExpression);
				m_parsingExpression++;
			}

			if (m_errorCount > 0)
			{
				AssembleMessage(String.Format("Failed with {0} errors.", m_errorCount));
				byteCode = null;
			}
			else
			{
				AssembleMessage(String.Format("Finished. Assembled {0} instructions.", m_instructions.Count()));
				byteCode = m_instructions;
			}

			log = m_log;

			return m_errorCount;
		}

		protected void AssembleMessage(string messageText)
		{
			m_log.Add(messageText);
		}

		protected void AssembleError(string errorText)
		{
			m_log.Add(errorText);
			m_errorCount++;
		}

		protected string SanitiseExpression(string expression)
		{
			if(expression.Count() > 0)
			{
				//Strip comments
				int commentPos = expression.IndexOf('/');
				if (commentPos >= 0)
					expression = expression.Remove(commentPos);

				//Strip whitespace
				expression = expression.Replace(' ', '\0');
				expression = expression.Replace('\t', '\0');
			}

			return expression;
		}

		protected void AssembleExpression(string expression)
		{
			if (expression.Count() > 0)
			{
				switch (expression[0])
				{
					case '/':
						//Comment
						break;
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
						ParseSub(expression.Substring(1));
						break;
					case 'I':
						//If
						break;
					case 'J':
						//JUMP
						break;
					default:
						break;
				}
			}
		}

		unsafe protected void ParseMove(string expression)
		{
			string[] tokens = expression.Split(',');

			int numParamsProvided = tokens.Count();
			int numParamsRequiredRegReg = Enum.GetNames(typeof(Core.OpParamsSourceDest)).Count();
			int numParamsRequiredLitReg = Enum.GetNames(typeof(Core.OpParamsSourceDest)).Count();

			if (numParamsProvided == numParamsRequiredRegReg || numParamsProvided == numParamsRequiredLitReg)
			{
				Core.Instruction instruction = new Core.Instruction();
				Core.ParamType sourceParamType;

				//Parse source param
				if (!ParseSourceParam(tokens[0], out instruction.parameters[0], out sourceParamType))
				{
					AssembleError(String.Format("MOV - Error parsing parameter 0"));
				}

				//Parse dest param
				if (!int.TryParse(tokens[1], out instruction.parameters[1]))
				{
					AssembleError(String.Format("MOV - Error parsing parameter 1"));
				}

				if (sourceParamType == Core.ParamType.Register)
				{
					//Register to register MOV
					instruction.opcode = (byte)Core.Opcodes.MoveRegReg;
				}
				else if (sourceParamType == Core.ParamType.Literal)
				{
					//Literal to register MOV
					instruction.opcode = (byte)Core.Opcodes.MoveLitReg;
				}

				m_instructions.Add(instruction);
			}
			else
			{
				AssembleError(String.Format("MOV - Instruction takes {0} parameters, not {1}", numParamsRequiredRegReg, numParamsProvided));
			}
		}

		unsafe protected void ParseAdd(string expression)
		{
			string[] tokens = expression.Split(',');

			int numParamsProvided = tokens.Count();
			int numParamsRequiredRegReg = Enum.GetNames(typeof(Core.OpParamsSourceDest)).Count();
			int numParamsRequiredLitReg = Enum.GetNames(typeof(Core.OpParamsSourceDest)).Count();

			if (numParamsProvided == numParamsRequiredRegReg || numParamsProvided == numParamsRequiredLitReg)
			{
				Core.Instruction instruction = new Core.Instruction();
				Core.ParamType sourceParamType;

				//Parse source param
				if (!ParseSourceParam(tokens[0], out instruction.parameters[0], out sourceParamType))
				{
					AssembleError(String.Format("ADD - Error parsing parameter 0"));
				}

				//Parse dest param
				if (!int.TryParse(tokens[1], out instruction.parameters[1]))
				{
					AssembleError(String.Format("ADD - Error parsing parameter 1"));
				}

				if (sourceParamType == Core.ParamType.Register)
				{
					//Register to register ADD
					instruction.opcode = (byte)Core.Opcodes.AddRegReg;
				}
				else if (sourceParamType == Core.ParamType.Literal)
				{
					//Literal to register ADD
					instruction.opcode = (byte)Core.Opcodes.AddLitReg;
				}

				m_instructions.Add(instruction);
			}
			else
			{
				AssembleError(String.Format("ADD - Instruction takes {0} parameters, not {1}", numParamsRequiredRegReg, numParamsProvided));
			}
		}

		unsafe protected void ParseSub(string expression)
		{
			string[] tokens = expression.Split(',');

			int numParamsProvided = tokens.Count();
			int numParamsRequiredRegReg = Enum.GetNames(typeof(Core.OpParamsSourceDest)).Count();
			int numParamsRequiredLitReg = Enum.GetNames(typeof(Core.OpParamsSourceDest)).Count();

			if (numParamsProvided == numParamsRequiredRegReg || numParamsProvided == numParamsRequiredLitReg)
			{
				Core.Instruction instruction = new Core.Instruction();
				Core.ParamType sourceParamType;

				//Parse source param
				if (!ParseSourceParam(tokens[0], out instruction.parameters[0], out sourceParamType))
				{
					AssembleError(String.Format("SUB - Error parsing parameter 0"));
				}

				//Parse dest param
				if (!int.TryParse(tokens[1], out instruction.parameters[1]))
				{
					AssembleError(String.Format("SUB - Error parsing parameter 1"));
				}

				if (sourceParamType == Core.ParamType.Register)
				{
					//Register to register SUB
					instruction.opcode = (byte)Core.Opcodes.SubRegReg;
				}
				else if (sourceParamType == Core.ParamType.Literal)
				{
					//Literal to register SUB
					instruction.opcode = (byte)Core.Opcodes.SubLitReg;
				}

				m_instructions.Add(instruction);
			}
			else
			{
				AssembleError(String.Format("ADD - Instruction takes {0} parameters, not {1}", numParamsRequiredRegReg, numParamsProvided));
			}
		}

		protected bool ParseSourceParam(string param, out int number, out Core.ParamType paramType)
		{
			if (param[0] == '#')
			{
				//Literal number
				paramType = Core.ParamType.Literal;
				return int.TryParse(param.Substring(1), out number);
			}
			else
			{
				//Register
				paramType = Core.ParamType.Register;
				return int.TryParse(param, out number);
			}
		}
	}
}