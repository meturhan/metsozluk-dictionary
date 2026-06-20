using System;
using System.Windows.Forms;
using System.IO;

namespace MetSozlukl
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            dosyayiYukle();            
        }
        // class tan�mlama
        class harf
        {
            public char h;
            public harf yan;
            public harf alt;
            public int goster;

            public harf(char c) // constructor..
            {
                h = c;
                yan = null;
                alt = null;
                goster = -1;
            }
        }

        harf ILK; // k�rd���m�m�z�n ilk eleman�n� g�sterecek olan pointer

        private void textBox1_TextChanged(object sender, EventArgs e) // textbox de�eri de�i�ti�inde tetiklenen fonksiyon
        {
            try // ne olur ne olmaz
            {
                StreamReader sr = new StreamReader(Environment.CurrentDirectory.ToString() + "\\sozluk.txt"); // s�zl�k i�in kullan�lacak olan txt dosyas�
                
                // �s�nma hareketleri...
                string oku = textBox1.Text;
                oku = oku.ToLower();
                string anlam;
                int satirSay = ara(oku);

                if (satirSay != -1) // bulunamad���nda -1 d�ner e�er -1 gelirse else ye git
                {
                    // bulunan anlam� ekrana yazd�r..
                    while (satirSay > 1)
                    {
                        sr.ReadLine();
                        satirSay--;
                    }
                    anlam = sr.ReadLine();
                    anlam = anlam.Substring(anlam.IndexOf('$') + 1);
                    richTextBox1.Text = anlam;
                    richTextBox1.Update();
                    textBox2.Text = "";
                    textBox2.Update();                    
                }
                else
                {
                    // bulunamad�ysa ac� haberi ver..
                    textBox2.Text = oku + " Bulunamad�...";
                    richTextBox1.Clear();
                    textBox2.Update();
                    richTextBox1.Update();
                }
            }
            catch
            {
                // dosyay� bulamam���z
                MessageBox.Show("S�zluk dosyas� bulunamad� !\nS�zl�k dosyas� programla ayn� klas�rde olmal�d�r. (sozluk.txt olarak)");
            }
            

            

        }

        public void dosyayiYukle() // a��l��ta yap�m�z� in�a edelim..
        {
            try 
            {
                // dosya i�lemleri
                StreamReader sr = new StreamReader(Environment.CurrentDirectory.ToString() + "\\sozluk.txt"); // s�zl�k i�in kullan�lacak olan txt dosyas�
                string satir; // sat�rlar� tutaca��m�z string
                int satirSay = 1;
                textBox2.Text = "Y�kleniyor...L�tfen Bekleyin...";

                // her sat�r� teker teker ekle fonksiyonuna yollayal�m..
                while ((satir = sr.ReadLine()) != null) // son sat�rda sr.readline() null de�eri d�nd�recek b�ylece d�ng�den ��kacak
                {
                    // oku
                    satir = satir.Substring(satir.IndexOf('$') + 1, satir.IndexOf(':') - satir.IndexOf('$') - 1); // s�zl�kten sadece ad� �ekebilmek i�in yap�lm�� k�rpma i�lemi
                    ekle(satir, satirSay);
                    satirSay++;
                }
                textBox2.Text = "Y�klendi..."; // bingo
            }
            catch
            {
                // kara haber
                MessageBox.Show("S�zluk dosyas� bulunamad� !\nS�zl�k dosyas� programla ayn� klas�rde olmal�d�r.(sozluk.txt olarak)");                
            }
            
        }

        // s�zl�kteki kelimeleri s�rayla trie ye ekleyen fonksiyon
        public void ekle(string s,int satirSay)
        {
            char[] c = s.ToCharArray(); // parametre olarak gelen string karakter dizisi olsun
            harf eklenen; // eklenen harfi tutacak olan tutankamon
            harf tmp; // ayak i�lerine bakacak olan pointer getir-g�t�r

            if (ILK == null) //ilk eleman� ekleme ko�ulu
            {
                eklenen = new harf(c[0]);
                ILK = eklenen; // ilk adresimizi de alm�� olduk hay�rl� u�urlu olsun
            }

            int l = s.Length; // stringin uzunlu�unu l de�i�kenine
            int i = 0; // i de�i�keni zorlu yol boyunca lokmalar�m�z� sayacak
            tmp = ILK; // ge�ici herzaman ilkten ba�las�n

            while (i < l)
            {
                if ((tmp.yan == null) && (c[i] != tmp.h))
                {
                    eklenen = new harf(c[i]);
                    tmp.yan = eklenen;
                    tmp = tmp.yan;
                }
                // i. seviyedeki yerini bulal�m
                while ((tmp.yan != null) && (c[i] != tmp.h))
                {
                    tmp = tmp.yan;
                    if ((tmp.yan == null) && (c[i] != tmp.h))
                    {
                        eklenen = new harf(c[i]);
                        tmp.yan = eklenen;
                        tmp = tmp.yan;
                    }
                }
                //i. seviyedeki yeri bulduk

                if (tmp.alt == null)
                {
                    i++;
                    while (i < l)
                    {
                        eklenen = new harf(c[i]); // yeni harfi ekle
                        tmp.alt = eklenen; // ekleneni ba�la
                        tmp = tmp.alt; // tmp yeni harfe odaklan
                        i++;
                    }
                    tmp.goster = satirSay;
                }
                else
                {
                    tmp = tmp.alt;
                    i++;
                }
            }    
        }

        // girilen kelimeyi trie'de  arayacak olan fonksiyon
        public int ara(string s)
        {
            harf tmp = ILK;
            char[] c = s.ToCharArray();
            int l = c.Length;
            int i = 0;
            int satirSay=-1;
                        
            while (i < l)
            {
                if (tmp.h == c[i]) // e�er harf e�itse
                {
                    satirSay = tmp.goster; // indexi alal�m
                    if (tmp.alt != null) // a�a��da eleman varsa
                    {
                        tmp = tmp.alt;   // a�a�� git                                         
                    }
                    i++;
                                               
                }

                else // velev ki harf e�it de�il
                {
                    if (tmp.yan != null) // yan bo�sa
                    {
                        tmp = tmp.yan; // yana kay
                    }
                    else // yan bo� de�ilse
                    {
                        satirSay = -1; // malum
                        i++;    // bir sonraki karaktere ge�elim
                    }
                }
            }
            return satirSay; // en son hangi sat�rda oldu�unu d�nd�r muhterem kelimenin
        }     
        
    }
}
