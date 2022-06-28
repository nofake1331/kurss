using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net.Http;
 
 
namespace kurs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       public struct data
        {

            public int full_time_budget { get; set; }
            public int full_time_contract { get; set; }
            public int part_time_budget { get; set; }
            public int part_time_contract { get; set; }
            public int evening_budget { get; set; }
            public int evening_contract { get; set; }
        }

        public class Opis
        {
            public List<int> data = new List<int>();
            public double lqua,hqua, med,mid;
            public int max,min;
            public List<string[]> name = new List<string[]>();

            public void stat()
            {
               sort();
               this.min = data[0];
               this.max = data[data.Count-1];
               this.lqua = lowquart();
               this.med = median();
               this.hqua = highquart();
               this.mid = Mid();
            }

            double Mid()
            {
                double sum = 0;
                foreach (int x in data)
                {
                     sum += x;
                }
                return sum/data.Count;
            }

            double lowquart()
            {
               return Percentile(data, 0.25);
            }

            double highquart()
            {
              return  Percentile(data, 0.75);
            }
            double median()
            {
               return Percentile(data, 0.5);
            }


            double Percentile(List<int> x, double percentile)
            {

                 double n = (x.Count - 1) * percentile + 1;
                 int k = (int)n; 
                 double d = n - k;
                 return x[k - 1] + d * (x[k] - x[k - 1]);   
                
            }



               void sort()
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    for (int j = 0; j < data.Count(); j++)
                    {
                        if (data[i] < data[j])
                        {
                            (data[i], data[j]) = (data[j], data[i]);
                            (name[i], name[j]) = (name[j], name[i]);
                        }
                    }
                }
                
            }
        }

        public class warnin
        {
            public void warning(string x )
            {
                MessageBox.Show(x);
            }
        }

        public class err: warnin
        {
            public void warning(string x , Exception ex)
            {
               
                if (MessageBox.Show(x, "Помилка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
       

        public Opis stat = new Opis();
        public err error = new err();
        public IEnumerable<data> emp;
        

      

        public bool requst(string y ,string b, string s)
        {
            HttpClient clnt = new HttpClient();
            clnt.BaseAddress = new Uri("https://registry.edbo.gov.ua/");
            HttpResponseMessage response = clnt.GetAsync("api/university-entrant/?y="+y+"&eb="+b+"&sp="+s+"&exp=json").Result;
       
            try
            {
                emp = response.Content.ReadAsAsync<IEnumerable<data>>().Result;
                return true;
            }
            catch (Exception ex)
            {
                error.warning("Данні не знайдено \r \r Продивитись подробиці?", ex);
                return false;
            }
          
        }

        public int sum(IEnumerable<data> x)
        {
            int sum = 0;
            foreach (var i in x)
            {
                sum += i.full_time_budget;
                sum += i.full_time_contract;
                sum += i.part_time_budget;
                sum += i.part_time_contract;
                sum += i.evening_budget;
                sum += i.evening_contract;
            }
            stat.data.Add(sum);
             stat.name.Add(new string[] { Convert.ToString(sum), comboBox1.Text + "\r\n" + comboBox3.Text + "\r\n" + comboBox2.SelectedValue  });
            return sum;
        }
   
        private void Form1_Load(object sender, EventArgs e)
        {
            this.specTableAdapter.Fill(this.kursDataSet.spec);
            this.bazaTableAdapter.Fill(this.kursDataSet.baza);
            this.osvit_stupTableAdapter.Fill(this.kursDataSet.osvit_stup);
            this.yearTableAdapter.Fill(this.kursDataSet.year);
 
        }

       





        private void button1_Click(object sender, EventArgs e)
        {

            if (requst(Convert.ToString(comboBox2.SelectedValue), Convert.ToString(comboBox3.SelectedValue), Convert.ToString(comboBox1.SelectedValue)))
            {
                try
                {
                    chart1.Series.Add(comboBox1.Text + "\r\n" + comboBox3.Text + "\r\n" + comboBox2.SelectedValue);
                    chart1.Series[comboBox1.Text + "\r\n" + comboBox3.Text + "\r\n" + comboBox2.SelectedValue].Points.AddXY("", sum(emp));
         
                }
                catch (Exception ex)
                {
                    error.warning("Данні вже знаходятся на графіку \r \r Продивитись подробиці?", ex);

                }
                }
           
         
        }

   

        private void button2_Click(object sender, EventArgs e)
        {
         
             while (chart1.Series.Count > 0) { chart1.Series.RemoveAt(0); }
             stat.data.Clear();
        }

        private void розробникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboit a = new aboit();
            a.ShowDialog();
        }

        private void якКористуватисьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            koris k = new koris();
            k.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (stat.data.Count >= 3)
            {
                stat.stat();
                statistic statis = new statistic(stat);
                statis.Show();
            }
            else
            {
                error.warning("у вибірці повина бути не менш 3 обєктів");
            }
        }
    }
}
