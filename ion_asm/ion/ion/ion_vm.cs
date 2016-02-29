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
				case (byte)Core.Opcodes.MoveRegReg:
					{
						int value = 0;
						uint sourceAddress = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Source];
						uint destAddress = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Dest];
						Load(sourceAddress, out value);
						Store(destAddress, value);
						break;
					}


				case (byte)Core.Opcodes.MoveLitReg:
					{
						int value = (int)instruction.parameters[(int)Core.OpParamsSourceDest.Source];
						uint destAddress = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Dest];
						Store(destAddress, value);
						break;
					}

				case (byte)Core.Opcodes.AddRegReg:
					{
						int valueA = 0;
						int valueB = 0;
						int valueC = 0;

						//Get addresses
						uint addressA = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Source];
						uint addressB = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Dest];

						//Load values from addresses
						Load(addressA, out valueA);
						Load(addressB, out valueB);

						//Add
						valueC = valueA + valueB;

						//Store result
						Store(addressB, valueC);
						break;
					}


				case (byte)Core.Opcodes.AddLitReg:
					{
						//Get value A
						int valueA = (int)instruction.parameters[(int)Core.OpParamsSourceDest.Source];
						int valueB = 0;
						int valueC = 0;

						//Get address
						uint address = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Dest];

						//Get value B from address
						Load(address, out valueB);

						//Add
						valueC = valueA + valueB;

						//Store result
						Store(address, valueC);
						break;
					}

				case (byte)Core.Opcodes.SubRegReg:
					{
						int valueA = 0;
						int valueB = 0;
						int valueC = 0;

						//Get addresses
						uint addressA = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Source];
						uint addressB = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Dest];

						//Load values from addresses
						Load(addressA, out valueA);
						Load(addressB, out valueB);

						//Sub
						valueC = valueB - valueA;

						//Store result
						Store(addressB, valueC);
						break;
					}


				case (byte)Core.Opcodes.SubLitReg:
					{
						//Get value A
						int valueA = (int)instruction.parameters[(int)Core.OpParamsSourceDest.Source];
						int valueB = 0;
						int valueC = 0;

						//Get address
						uint address = (uint)instruction.parameters[(int)Core.OpParamsSourceDest.Dest];

						//Get value B from address
						Load(address, out valueB);

						//Sub
						valueC = valueB - valueA;

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