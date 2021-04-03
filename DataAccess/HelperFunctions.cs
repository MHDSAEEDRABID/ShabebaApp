using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace DataAccess
{
    public static class HelperFunctions
    {
        public static DataTable GetFromDataGridView(DataGridView dgv)
        {
            var dt = new DataTable();
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = column.HeaderText;
                dt.Columns.Add(dataColumn);
            }

            object[] cellValues = new object[dgv.Columns.Count];
            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    cellValues[i] = row.Cells[i].Value;
                }
                dt.Rows.Add(cellValues);
            }

            return dt;
        }
        public static void Filldgv(DataTable table, DataGridView dgv)
        {
            dgv.Rows.Clear();
            foreach (DataRow row in table.Rows)
            {
                dgv.Rows.Add(row.ItemArray);
            }
        }
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
