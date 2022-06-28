using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kurs
{
    public partial class statistic : Form
    {
        public Form1.Opis stat;
        public statistic(Form1.Opis x)
        {
            
            InitializeComponent();
            stat = x;
        }

        double proc;
        int x1, x2, x3, x4;

    

        private void statistic_Load(object sender, EventArgs e)
        { 

           stat.stat();

           label1.Text = "Cередне значення:"+  Convert.ToString( Math.Round(stat.mid,2)) ;
           label5.Text = "Мінімальне значення:"+ Convert.ToString( stat.min);
           label3.Text = "Нижній квартиль:"+ Convert.ToString( stat.lqua);
           label2.Text = "Медіана:"+ Convert.ToString( stat.med);
           label4.Text = "Верхній квартиль:" + Convert.ToString( stat.hqua);
           label6.Text = "Максимальне значення:"+ Convert.ToString( stat.max);

           chart1.Series.Add("s");
           chart1.Series["s"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;

           chart1.Series.Add("q1");
           chart1.Series["q1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
           chart1.Series["q1"].BorderWidth = 2;
           chart1.Series["q1"].Color = Color.Red;
           chart1.Series["q1"].Label = "Q1";



           chart1.Series.Add("q2");
           chart1.Series["q2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
           chart1.Series["q2"].BorderWidth = 2;
           chart1.Series["q2"].Color = Color.Black;
           chart1.Series["q2"].Label = "Q2";
          

           chart1.Series.Add("q3");
           chart1.Series["q3"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
           chart1.Series["q3"].BorderWidth = 2;
           chart1.Series["q3"].Color = Color.Blue;
           chart1.Series["q3"].Label = "Q3";

            foreach (string[] x in stat.name)
            {
         
                chart1.Series["s"].Points.AddXY(x[1], x[0]);
                chart1.Series["q1"].Points.AddXY(x[1], stat.lqua);
                chart1.Series["q2"].Points.AddXY(x[1], stat.med);
                chart1.Series["q3"].Points.AddXY(x[1], stat.hqua);
          
            }

           proc = stat.max * 0.25;
            foreach (int x in stat.data)
            {
                if (x < proc)
                    x1++;
                else if (x >= proc && x < proc * 2)
                    x2++;
                else if (x >= proc * 2 && x < proc * 3)
                    x3++;
                else
                    x4++;
            }
            int sum = x1 + x2 + x3 + x4;

            dataGridView1.Rows.Add(3);
           dataGridView1[0,0].Value = "0-" + Convert.ToString(proc);
           dataGridView1[0,1].Value = Convert.ToString(proc)+"-" + Convert.ToString(proc * 2 );
           dataGridView1[0,2].Value = Convert.ToString(proc * 2) + "-" + Convert.ToString(proc * 3);
           dataGridView1[0,3].Value = Convert.ToString(proc * 3 ) + "-" + Convert.ToString(proc * 4);
           dataGridView1[1,0].Value = Convert.ToString(x1);
           dataGridView1[1,1].Value = Convert.ToString(x2);
           dataGridView1[1,2].Value = Convert.ToString(x3);
           dataGridView1[1,3].Value = Convert.ToString(x4);
           dataGridView1[2, 0].Value = "0."+Convert.ToString((x1*100)/sum);
           dataGridView1[2, 1].Value = "0."+Convert.ToString((x2 * 100) / sum);
           dataGridView1[2, 2].Value = "0."+Convert.ToString((x3 * 100) / sum);
           dataGridView1[2, 3].Value = "0."+Convert.ToString((x4 * 100) / sum);

        }
    }
    
}
