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
		//ion virtual machine memory size
		const int m_vmMemorySizeBytes = 64;

		//ion virtual machine
		VirtualMachine m_virtualMachine = null;

		//Assembled ion bytecode
		List<Core.Instruction> m_byteCode = null;

		//Memory visualiser
		DataTable memoryDataTable;

		protected void Page_Load(object sender, EventArgs e)
		{
			//Retrieve session
			m_byteCode = (List<Core.Instruction>)Session["Bytecode"];

			memoryDataTable = new DataTable();

			int memoryTableSizeSqrt = (int)Math.Sqrt(m_vmMemorySizeBytes);

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

		protected void PopulateMemoryVisualiser(int[] memory)
		{
			int memoryTableSizeSqrt = (int)Math.Sqrt(m_vmMemorySizeBytes);

			for (int x = 0; x < memoryTableSizeSqrt; x++)
			{
				for (int y = 0; y < memoryTableSizeSqrt; y++)
				{
					memoryDataTable.Rows[y][x] = memory[(y * memoryTableSizeSqrt) + x].ToString();
				}
			}

			gridMemory.DataSource = memoryDataTable;
			gridMemory.DataBind();
		}

		protected void btnCompile_Click(object sender, EventArgs e)
		{
			//Create assembler
			Assembler assembler = new Assembler();

			//Assemble source to bytecode
			m_byteCode = assembler.Assemble(txtSourceCode.Text);

			//Store in session
			Session["Bytecode"] = m_byteCode;
		}

		protected void btnExecute_Click(object sender, EventArgs e)
		{
			if(m_byteCode != null)
			{
				//Create new virtual machine
				m_virtualMachine = new VirtualMachine(m_vmMemorySizeBytes, m_byteCode);

				//Execute
				m_virtualMachine.Execute();

				//Display memory
				PopulateMemoryVisualiser(m_virtualMachine.Memory);
			}
		}
	}
}