using System;
using System.Collections.Generic;
using System.Data;

namespace ion
{
	public class VirtualMachine
	{
		//System memory
		private int m_memoryTotalBytes;
		private int[] m_memory;

		public int[] Memory
		{
			get { return m_memory; }
			private set { m_memory = value; }
		}

		//Compiled bytecode
		List<Core.Instruction> m_byteCode;

		public VirtualMachine(int memoryTotalBytes, List<Core.Instruction> byteCode)
		{
			m_memoryTotalBytes = memoryTotalBytes;
			m_byteCode = byteCode;
		}

		public void Execute()
		{
			//Initialise virtual machine
			m_memory = new int[m_memoryTotalBytes];

			foreach (Core.Instruction instruction in m_byteCode)
			{
				ExecuteInstruction(instruction);
			}
		}

		unsafe protected void ExecuteInstruction(Core.Instruction instruction)
		{
			switch (instruction.opcode)
			{
				case (byte)Core.Opcodes.OP_MOV_REG_REG:
					{
						int value = 0;
						uint sourceAddress = (uint)instruction.parameters[(int)Core.OpParamsMOV_REG_REG.OP_MOV_SOURCE];
						uint destAddress = (uint)instruction.parameters[(int)Core.OpParamsMOV_REG_REG.OP_MOV_DEST];
						Load(sourceAddress, out value);
						Store(destAddress, value);
						break;
					}


				case (byte)Core.Opcodes.OP_MOV_LIT_REG:
					{
						int value = (int)instruction.parameters[(int)Core.OpParamsMOV_REG_REG.OP_MOV_SOURCE];
						uint destAddress = (uint)instruction.parameters[(int)Core.OpParamsMOV_REG_REG.OP_MOV_DEST];
						Store(destAddress, value);
						break;
					}

				case (byte)Core.Opcodes.OP_ADD_REG_REG:
					{
						int valueA = 0;
						int valueB = 0;
						int valueC = 0;

						//Get addresses
						uint addressA = (uint)instruction.parameters[(int)Core.OpParamsADD_REG_REG.OP_MOV_SOURCE];
						uint addressB = (uint)instruction.parameters[(int)Core.OpParamsADD_REG_REG.OP_MOV_DEST];

						//Load values from addresses
						Load(addressA, out valueA);
						Load(addressB, out valueB);

						//Add
						valueC = valueA + valueB;

						//Store result
						Store(addressB, valueC);
						break;
					}


				case (byte)Core.Opcodes.OP_ADD_LIT_REG:
					{
						//Get value A
						int valueA = (int)instruction.parameters[(int)Core.OpParamsADD_REG_REG.OP_MOV_SOURCE];
						int valueB = 0;
						int valueC = 0;

						//Get address
						uint address = (uint)instruction.parameters[(int)Core.OpParamsADD_REG_REG.OP_MOV_DEST];

						//Get value B from address
						Load(address, out valueB);

						//Add
						valueC = valueA + valueB;

						//Store result
						Store(address, valueC);
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
	}
}