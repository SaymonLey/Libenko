using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.LoadData();
        }

        private void LoadData()
        {
            dgwTable.Rows.Clear();
            if (File.Exists("Data.xml")) // если существует данный файл
            {
                DataSet dataSet = new DataSet(); // создаем новый пустой кэш данных
                dataSet.ReadXml("Data.xml"); // записываем в него XML-данные из файла
                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow item in dataSet.Tables["Employee"].Rows)
                    {
                        int n = dgwTable.Rows.Add(); // добавляем новую сроку в dgv
                        dgwTable.Rows[n].Cells[0].Value = item["Номер заказа"]; // заносим в первый столбец созданной строки данные из первого столбца таблицы ds.
                        dgwTable.Rows[n].Cells[1].Value = item["Название техники"];
                        dgwTable.Rows[n].Cells[2].Value = item["Стоимость заказа"];
                        dgwTable.Rows[n].Cells[3].Value = item["Дата принятия заказа"];
                        dgwTable.Rows[n].Cells[4].Value = item["Срок доставки"];
                        dgwTable.Rows[n].Cells[5].Value = item["Статус"];                      
                    }
                    FilterData();
                }
            }
            else
            {
                MessageBox.Show("XML файл не найден.", "Ошибка.");
            }
        }
        private void FilterData()
        {
            for (int i = 0; i < dgwTable.RowCount; i++)
            {
                for (int j = 0; j < dgwTable.ColumnCount; j++)
                    if (dgwTable.Rows[i].Cells[j].Value != null)
                        if (dgwTable.Rows[i].Cells[j].Value.ToString().Contains(tbPoisk.Text))
                        {
                            dgwTable.Rows[i].Visible = true;
                            break;
                        }
                        else
                        {
                            dgwTable.Rows[i].Visible = false;
                        }
            }
        }
        
        private void butNew_Click(object sender, EventArgs e)
        {
            Create c = new Create(dgwTable);
            c.ShowDialog();
            LoadData();
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            if (dgwTable.CurrentRow != null )
            {
                int a = dgwTable.CurrentRow.Index;
                Create c = new Create(dgwTable);
                c.setStart(dgwTable[0, a].Value.ToString(), dgwTable[1, a].Value.ToString(), dgwTable[2, a].Value.ToString(), dgwTable[3, a].Value.ToString(), dgwTable[4, a].Value.ToString(),  Convert.ToBoolean(dgwTable[5, a].Value), a);
                c.ShowDialog();
                LoadData();
            }
        }

        private void bDelite_Click(object sender, EventArgs e)
        {
            
            dgwTable.Rows.Remove(dgwTable.CurrentRow);

            DataSet ds = new DataSet(); // создаем пока что пустой кэш данных
            DataTable dt = new DataTable(); // создаем пока что пустую таблицу данных
            dt.TableName = "Employee"; // название таблицы
            dt.Columns.Add("Номер заказа"); // название колонок
            dt.Columns.Add("Название техники");
            dt.Columns.Add("Стоимость заказа");
            dt.Columns.Add("Дата принятия заказа");
            dt.Columns.Add("Срок доставки");
            dt.Columns.Add("Статус");
            ds.Tables.Add(dt); //в ds создается таблица

            foreach (DataGridViewRow r in dgwTable.Rows) // пока в dgv есть строки
            {
                DataRow row = ds.Tables["Employee"].NewRow(); // создаем новую строку в таблице, занесенной в ds
                row["Номер заказа"] = r.Cells[0].Value;  //в столбец этой строки заносим данные из первого столбца dgv
                row["Название техники"] = r.Cells[1].Value;
                row["Стоимость заказа"] = r.Cells[2].Value;
                row["Дата принятия заказа"] = r.Cells[3].Value;
                row["Срок доставки"] = r.Cells[4].Value;
                row["Статус"] = r.Cells[5].Value;
                ds.Tables["Employee"].Rows.Add(row); //добавление всей этой строки в таблицу ds.
            }
            ds.WriteXml("Data.xml");
        }

        private void bPoisk_Click(object sender, EventArgs e)
        {
            this.FilterData();
        }

        private void bReady_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgwTable.RowCount; i++)
            {
                if (Convert.ToBoolean(dgwTable.Rows[i].Cells[5].Value) == true)
                {
                    dgwTable.Rows[i].Visible = true;
                }
                else
                {
                    dgwTable.Rows[i].Visible = false;
                }
            }
        }

        private void bDontReady_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgwTable.RowCount; i++)
            {
                        if (Convert.ToBoolean(dgwTable.Rows[i].Cells[5].Value)==false)
                        {
                            dgwTable.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgwTable.Rows[i].Visible = false;
                        }
            }
        }

        private void bRemuve_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgwTable.RowCount; i++)
            {

                dgwTable.Rows[i].Visible = true;
                if (dgwTable.Rows[i].Cells[5].Value.Equals(true))
                    dgwTable.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }


        private void dgwTable_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                bool bl = Convert.ToBoolean(dgwTable.Rows[e.RowIndex].Cells[5].Value);
                if (bl)
                {
                    dgwTable.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                }
                else
                {
                    dgwTable.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void tbPoisk_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }
    }
}
