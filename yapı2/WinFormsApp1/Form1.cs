using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Packt.Shared;
using static System.Console;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        static Tooop[] dizi = new Tooop[10];
        static int dongusayac = 1;
        static Pen[] kalem= new Pen[10];
        static string password = "Ucan541Ge45miler"; //Sifrein sabit olmasının kötü pratik olduğunu farkındayım ama istenen sadece sifrelenmesi güvenlik açıklarını kapatmak ile ilgilenmiyorum şimdilik.
        string jsonPath = Path.Combine(Directory.GetCurrentDirectory(),"gp_yedek.json");


        
      /*  void Eskitimer()  İlk basta toplarin yerini kontrol eden timeri yapmaya calistim ama farklı threadleri cagirmasi ve farklı hatalardan ziyade daha kolay olana dodndum.
        {
            while (dongusayac == 1)
                for (int j = 0; j < Tooop.i; j++)
            {
                {

                    if (dizi[j] == null)
                        continue;
                    if (dizi[j].a == 1)
                        dizi[j].y += 10;
                    else
                        dizi[j].y = dizi[j].y - 10;
                    if (dizi[j].m == 1)
                        dizi[j].x += 10;
                    else
                        dizi[j].x = dizi[j].x - 10;
                    if (dizi[j].x < 0)
                        dizi[j].m = dizi[j].m * -1;
                    else if (dizi[j].x > 1000)
                        dizi[j].m = dizi[j].m * -1;
                    else if (dizi[j].y < 0 & dizi[j].x < 330)
                        dizi[j].a = dizi[j].a * -1;
                    else if (dizi[j].y < 0 & dizi[j].x > 700)
                        dizi[j].a = dizi[j].a * -1;
                    else if (dizi[j].y - 10 < 0 && dizi[j].x < 745 && dizi[j].x > 350)
                    {
                        dizi[j] = null;
                        Tooop.i--;
                        Tooop.t = Tooop.t + 10;
                        Refresh();
                        continue;
                    }
                    else if (dizi[j].y + 100 >= button2.Location.Y && dizi[j].x > button2.Location.X && dizi[j].x < button2.Location.X + 360)
                    {
                        dizi[j].a = dizi[j].a * -1;
                        Tooop.t++;
                    }
                    else if (dizi[j].y - 2 > button2.Location.Y)
                    {
                        dizi[j] = null;
                        Tooop.i--;
                        Olustur();
                        Olustur();
                        Tooop.t = Tooop.t - 20;
                        Refresh();
                        continue;
                    }
                    button6.Text = "Skor: " + Tooop.t;
                    Thread.Sleep(200);
                    Refresh();

                }
            }

        }*/
        void Eskitimer()
        {
            while (dongusayac==1) {
                Kayıt();
                Thread.Sleep(120000);
            }
        }
            public Form1()
        {
            InitializeComponent();
            Task A = new Task(Eskitimer);
            A.Start();
            MessageBoxManager.Yes = "Evet";
            MessageBoxManager.No = "Hayır";
            MessageBoxManager.Register();
            
            

        }
        public void Kayıt()
        {

            if (Tooop.o == 0)
            {

                Olustur();
                Tooop.o++;
            }
            for (int say = 0; say < 1; say++)
            {
                if(dizi[say]!=null)
                dizi[say].o2 = Tooop.o;
                dizi[say].i2 = Tooop.i; 
                dizi[say].t2 = Tooop.t;
            }
                
                    
            List<Tooop> list = new List<Tooop>(dizi);
            string output2=JsonConvert.SerializeObject(list);
            output2=Encrypt(output2, password);
            File.WriteAllText(jsonPath, output2);

        }
        public void Yükle()
        {
            
            string output2 =File.ReadAllText(jsonPath);
            output2 = Decrypt(output2,password);
            List<Tooop> list = new List<Tooop>();
            list=JsonConvert.DeserializeObject<List<Tooop>>(output2);
            dizi=list.ToArray();
            int sayac = 0;
            while (dizi[sayac] == null)
            {
                sayac++;
                if (sayac == 10)
                    break;
            }
            if (sayac < 10)
            {
                Tooop.t = dizi[sayac].t2;
                Tooop.o = dizi[sayac].o2;
                Tooop.i = dizi[sayac].i2;
            }
        }
        public static string Encrypt(string plaintext, string password)
        {
            byte[] encryptb;
            byte[] plaint = Encoding.Unicode.GetBytes(plaintext);
            var aes = Aes.Create();
            var pdfsa = new Rfc2898DeriveBytes(password, Protector.salt, Protector.iterations);
            aes.Key = pdfsa.GetBytes(32);
            aes.IV = pdfsa.GetBytes(16);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(plaint, 0, plaint.Length);
                }
            
            encryptb = ms.ToArray();
        }
            return Convert.ToBase64String(encryptb);
        }
        public static string Decrypt(string cryptot, string password)
        {
            byte[] plainb;
            byte[] cryptob=Convert.FromBase64String(cryptot);
            var aes = Aes.Create();
            var pbkdf2 = new Rfc2898DeriveBytes(password, Protector.salt, Protector.iterations);
            aes.Key = pbkdf2.GetBytes(32);
            aes.IV = pbkdf2.GetBytes(16);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cryptob, 0, cryptob.Length);
                }

                plainb = ms.ToArray();
            }
            return Encoding.Unicode.GetString(plainb);
        }
       
        public class Tooop
        {
            private Random rnd = new Random();
            public int x, y, a,m,l,q,r1,r2,r3;
            public static int o=0, i=0, t=0;
            public int o2=o, i2=i, t2=t;
           
            public Color renk;
            public SolidBrush brush;
            int[] dizi5= {-1,1};
            public void Yarat()
            {
                
            x = rnd.Next(900);
                y = rnd.Next(400);
                l = rnd.Next(0,1);
                q = rnd.Next(0,1);
                r1 = rnd.Next(256);
                r2 = rnd.Next(256);
                r3 = rnd.Next(256);
                renk = Color.FromArgb(r1,r2,r3);
                kalem[i] = new Pen(renk);
                brush = new SolidBrush(renk);
                a = dizi5[l];
                m = dizi5[q];
            }
            public void Kayıt(int x, int y, int l, int q, int r1, int r2, int r3, int a, int m)
            {
                this.x = x;
                this.y = rnd.Next(400);
                this.l = rnd.Next(0, 1);
                this.q = rnd.Next(0, 1);
                this.r1 = rnd.Next(256);
                this.r2 = rnd.Next(256);
                this.r3 = rnd.Next(256);
                renk = Color.FromArgb(r1, r2, r3);
                kalem[i] = new Pen(renk);
                brush = new SolidBrush(renk);
                this.a = dizi5[l];
                this.m = dizi5[q];
            }
        }
        static void Olustur()
        {
            for (int u = 0; u < 10; u++)
            {
                if (dizi[u] == null)
                {
                    dizi[u] = new Tooop();
                    dizi[u].Yarat();
                    Tooop.i++;
                    break;
                }
            }


        }
        private void timer2_Tick(object sender, EventArgs e)
        {

            Olustur();
        }

        private void Daire(object sender, PaintEventArgs e)
        {
            if (Tooop.i == 10)
            {
                button2.Text = "Kaybettiniz";
                button6.Text = "Skor:" + Tooop.t;
                timer1.Stop();
                timer2.Stop();

            }
            else if (Tooop.i == 0)
            {
                button2.Text = "Kazandınız";
                button6.Text = "Skor:" + Tooop.t;
                timer1.Stop();
                timer2.Stop();

            }


            for (int j = 0; j < Tooop.i; j++)
            {
                if (dizi[j] == null)
                continue;
                e.Graphics.FillEllipse(dizi[j].brush, dizi[j].x, dizi[j].y, 100, 100);
                e.Graphics.DrawEllipse(kalem[j], dizi[j].x, dizi[j].y, 100, 100);


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Kayıt();
        }


        private void button8_Click(object sender, EventArgs e)
        {
            Yükle();
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            
               
                
                    if(e.KeyCode== Keys.A)
                    if (button2.Location.X-30 > panel2.Location.X)
                    {
                        e.Handled = true;
                        button2.Location = new Point(button2.Location.X - 10, button2.Location.Y);
                        this.Refresh();
                        
                    }
                    
            if (e.KeyCode == Keys.D)
                if (button2.Location.X+360 < panel1.Location.X)
                    {
                        e.Handled = true;
                        button2.Location = new Point(button2.Location.X + 10, button2.Location.Y);
                        this.Refresh();
                    }
                        
                
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(jsonPath))
            {
       

 
                var result = MessageBox.Show("Yedekten yükleme yapılsın mı?", "Yükleme", MessageBoxButtons.YesNo);//Gordugun kadarı ile Evet/Hayır demiyor.

                if (result == DialogResult.Yes)
                {
                    Yükle();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int j = 0; j < Tooop.i; j++)
            {
                {

                    if (dizi[j] == null)
                        continue;
                    if (dizi[j].a == 1)
                        dizi[j].y += 10;
                    else
                        dizi[j].y = dizi[j].y - 10;
                    if (dizi[j].m == 1)
                        dizi[j].x += 10;
                    else
                        dizi[j].x = dizi[j].x - 10;
                    if (dizi[j].x < 0)
                        dizi[j].m = dizi[j].m * -1;
                    else if (dizi[j].x > 1000)
                        dizi[j].m = dizi[j].m * -1;
                    else if (dizi[j].y < 0 & dizi[j].x < 330)
                        dizi[j].a = dizi[j].a * -1;
                    else if (dizi[j].y < 0 & dizi[j].x > 700)
                        dizi[j].a = dizi[j].a * -1;
                    else if (dizi[j].y - 10 < 0 && dizi[j].x < 745 && dizi[j].x > 350)
                    {
                        dizi[j] = null;
                        Tooop.i--;
                        Tooop.t = Tooop.t + 10;
                        Refresh();
                        continue;
                    }
                    else if (dizi[j].y + 100 >= button2.Location.Y && dizi[j].x > button2.Location.X && dizi[j].x < button2.Location.X + 360)
                    {
                        dizi[j].a = dizi[j].a * -1;
                        Tooop.t++;
                    }
                    else if (dizi[j].y - 2 > button2.Location.Y)
                    {
                        dizi[j] = null;
                        Tooop.i--;
                        Olustur();
                        Olustur();
                        Tooop.t = Tooop.t - 20;
                        Refresh();
                        continue;
                    }
                    button6.Text = "Skor: " + Tooop.t;
                    Refresh();
                }
            }
        }
    }
    
}

    



