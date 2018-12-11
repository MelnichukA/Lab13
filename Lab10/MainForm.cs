using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab8Lib;
using System.IO;

namespace Lab10
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            gvPass.AutoGenerateColumns = false;
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Name";
            column.Name = "Имя";
            gvPass.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Country";
            column.Name = "Страна";
            column.Width = 50;
            gvPass.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Region";
            column.Name = "Город";
            gvPass.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Place";
            column.Name = "Место выдачи";
            gvPass.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Prop";
            column.Name = "Место прописки";
            gvPass.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "PassSer";
            column.Name = "Серия паспорта";
            gvPass.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "PassNum";
            column.Name = "Номер паспорта";
            gvPass.Columns.Add(column);

            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "HasMarried";
            column.Name = "Женат(Замужем)";
            gvPass.Columns.Add(column);

            bindSrcPass.Add(new Pass("Анна", "Україна", "Николаев", "Центральный район", "Пер.Первомайский 75", 550033, "ЕР", false));
            EventArgs args = new EventArgs();
            OnResize(args);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Pass pass = new Pass();
            PassForm ft = new PassForm(pass);
            if (ft.ShowDialog() == DialogResult.OK)
            {
                bindSrcPass.Add(pass);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Pass pass = (Pass)bindSrcPass.List[bindSrcPass.Position];
            PassForm ft = new PassForm(pass);
            if (ft.ShowDialog() == DialogResult.OK)
            {
                bindSrcPass.List[bindSrcPass.Position] = pass;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Видалити поточний запис?", "Видалення запису", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bindSrcPass.RemoveCurrent();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистити таблицю?\n\nВсі дані будуть втрачені", "Очищення даних", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                bindSrcPass.Clear();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Закрити застосунок?", "Вихід з програми", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btnSaveAsText_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Зберегти дані у текстовому форматі";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            StreamWriter sw;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8);
                try
                {
                    foreach (Pass pass in bindSrcPass.List)
                    {
                        sw.Write(pass.Name + "\t"
                             + pass.Country + "\t"
                            + pass.Region + "\t"
                            + pass.Place + "\t"
                            + pass.Prop + "\t"
                             + pass.PassNum + "\t"
                            + pass.PassSer + "\t"
                            + pass.HasMarried + "\t\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sw.Close();
                }
            }

        }

        private void btnSaveAsBinary_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Файли даних (*.subs)|*.subs|All files (*.*)|*.*";
            saveFileDialog.Title = "Зберегти дані у бінарному форматі";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            BinaryWriter bw;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                bw = new BinaryWriter(saveFileDialog.OpenFile());
                try
                {
                    foreach (Pass sub in bindSrcPass.List)
                    {
                        bw.Write(sub.Name);
                        bw.Write(sub.Country);
                        bw.Write(sub.Region);
                        bw.Write(sub.Place);
                        bw.Write(sub.Prop);
                        bw.Write(sub.PassNum);
                        bw.Write(sub.PassSer);
                        bw.Write(sub.HasMarried);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    bw.Close();
                }
            }
        }

        private void btnOpenFromText_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Прочитати дані у текстовому форматі";
            openFileDialog.InitialDirectory = Application.StartupPath;
            StreamReader sr;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcPass.Clear();
                sr = new StreamReader(openFileDialog.FileName, Encoding.UTF8);
                string s;
                try
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] split = s.Split('\t');
                        Pass pass = new Pass(split[0],
                            split[1],
                            split[2],
                            split[3],
                            split[4],
                            int.Parse(split[5]),
                             split[6],
                            bool.Parse(split[7]));

                        bindSrcPass.Add(pass);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sr.Close();
                }
            }
        }

        private void btnOpenFromBinary_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Файли даних (*.subs)|*.subs|All files (*.*)|*.*";
            openFileDialog.Title = "Прочитати дані у бінарному форматі";
            openFileDialog.InitialDirectory = Application.StartupPath;
            BinaryReader br;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcPass.Clear();
                br = new BinaryReader(openFileDialog.OpenFile());
                try
                {
                    Pass pass;
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        pass = new Pass();
                        for (int i = 1; i <= 8; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    pass.Name = br.ReadString();
                                    break;

                                case 2:
                                    pass.Country = br.ReadString();
                                    break;

                                case 3:
                                    pass.Region = br.ReadString();
                                    break;

                                case 4:
                                    pass.Place = br.ReadString();
                                    break;

                                case 5:
                                    pass.Prop = br.ReadString();
                                    break;

                                case 6:
                                    pass.PassNum = br.ReadInt32();
                                    break;

                                case 7:
                                    pass.PassSer = br.ReadString();
                                    break;

                                case 8:
                                    pass.HasMarried = br.ReadBoolean();
                                    break;
                            }
                        }
                        bindSrcPass.Add(pass);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    br.Close();
                }


            }
        }
    }
}
