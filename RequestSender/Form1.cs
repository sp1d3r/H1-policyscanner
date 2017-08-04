using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace RequestSender
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            Request myreq = new Request();
            XmlNodeList lstname;
            XmlNodeList lsturl;
            XmlDocument doc;
            myreq.method = "GET";
            string json;
            string outs = "<root>";
            for (int i=0;i<=count/100;i++)
            {
                myreq.Url = "https://hackerone.com/programs/search?query=type%3Ahackerone&sort=published_at%3Adescending&page="+(i+1);
                json = myreq.Send();
                doc = JsonConvert.DeserializeXmlNode(json, "root");
                XmlNode root = doc.DocumentElement;
                count = Int32.Parse(root.SelectSingleNode("/root/total").InnerText);
                lstname = root.SelectNodes("/root/results/name");
                lsturl = root.SelectNodes("/root/results/url");
                for (int j=0;j<lstname.Count;j++)
                {
                    outs += "<item>";
                    outs +="<name>"+lstname[j].InnerText+"</name>";
                    outs+="<url>" + lsturl[j].InnerText + "</url>";
                    outs += "</item>";
                }
            }
            outs += "</root>";
            System.IO.File.WriteAllText("out.xml", outs);
            policies();
        }


        private void policies()
        {
            textBox1.Text = "";
            progressBar1.Value = 0;
            Request myreq = new Request();
            myreq.method = "GET";
            XmlNodeList policy;
            XmlNodeList url;
            XmlNodeList scope;
            string json;
            string scp="";
            string tmp = "";
            string outs = "<root>";
            XmlDocument doc = new XmlDocument();
            XmlDocument doc2 = new XmlDocument();
            doc.Load("out.xml");
            url = doc.SelectNodes("/root/item/url");
            float j;
            float uc;
            for (int i = 0; i < url.Count; i++)
            {
               // myreq.Url = "https://hackerone.com" + url[i].InnerText + "/policy_versions";
                //json = CalculateMD5Hash(myreq.Send());
                myreq.Url = "https://hackerone.com" + url[i].InnerText;
                doc2 = JsonConvert.DeserializeXmlNode(myreq.Send(), "root");
                XmlNode root2 = doc2.DocumentElement;
                json = CalculateMD5Hash(root2.SelectSingleNode("/root/last_policy_change_at").InnerText);
                //MessageBox.Show(root2.SelectSingleNode("/root/last_policy_change_at").InnerText);
                scope = root2.SelectNodes("/root/scopes");
                for (int z = 0; z < scope.Count; z++)
                {
                    scp+=scope[z].InnerText;
                    tmp += scope[z].InnerText+Environment.NewLine;
                }
                scp = CalculateMD5Hash(scp);
                verify(url[i].InnerText, json,scp);
                outs += "<item url=\"" + url[i].InnerText + "\">" + json + "</item>";
                outs += "<scope url=\"" + url[i].InnerText + "\">" + scp + "</scope>";
                scp = "";
                j = i;
                uc = url.Count;
                progressBar1.Value = (int)Math.Ceiling((j / uc) * 100);
                //Application.DoEvents();
            }
            outs += "</root>";
            System.IO.File.WriteAllText("policy.xml", outs);
            System.IO.File.WriteAllText("pubscopes.txt", tmp);
            tmp = "";
            MessageBox.Show("Finish!");
            progressBar1.Value = 0;
        }




        public string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public void verify(string url, string json, string scp)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("policy.xml");
                XmlNode root = doc.DocumentElement;
                if ((root.SelectSingleNode("/root/item[@url=\"" + url + "\"]").InnerText != json) || (root.SelectSingleNode("/root/scope[@url=\"" + url + "\"]").InnerText != scp))
                {
                    textBox1.Text += url + "\r\n";
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += url + "\r\n";
            }

        }

        public void verify2(string url, string json, string scp)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("policy_ext.xml");
                XmlNode root = doc.DocumentElement;
                if ((root.SelectSingleNode("/root/item[@url=\"" + url + "\"]").InnerText != json) || (root.SelectSingleNode("/root/scope[@url=\"" + url + "\"]").InnerText != scp))
                {
                    textBox1.Text += url + "\r\n";
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += url + "\r\n";
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = 0;
            Request myreq = new Request();
            XmlNodeList lstname;
            XmlNodeList lsturl;
            XmlDocument doc;
            myreq.method = "GET";
            string json;
            string outs = "<root>";
            for (int i = 0; i <= count / 100; i++)
            {
                myreq.Url = "https://hackerone.com/programs/search?query=type%3Aexternal&sort=name%3Aascending&page=" + (i + 1);
                json = myreq.Send();
                doc = JsonConvert.DeserializeXmlNode(json, "root");
                XmlNode root = doc.DocumentElement;
                count = Int32.Parse(root.SelectSingleNode("/root/total").InnerText);
                lstname = root.SelectNodes("/root/results/name");
                lsturl = root.SelectNodes("/root/results/url");
                
                for (int j = 0; j < lstname.Count; j++)
                {
                    outs += "<item>";
                    outs += "<name>" + System.Security.SecurityElement.Escape(lstname[j].InnerText) + "</name>";
                    outs += "<url>" + System.Security.SecurityElement.Escape(lsturl[j].InnerText) + "</url>";
                    outs += "</item>";
                }
            }
            outs += "</root>";
            System.IO.File.WriteAllText("out_ext.xml", outs);
            policiesext();
        }

        private void policiesext()
        {
            textBox1.Text = "";
            progressBar1.Value = 0;
            Request myreq = new Request();
            myreq.method = "GET";
            XmlNodeList policy;
            XmlNodeList url;
            XmlNodeList scope;
            string lstpol;
            string json;
            string scp = "";
            string tmp = "";
            string outs = "<root>";
            XmlDocument doc = new XmlDocument();
            XmlDocument doc2 = new XmlDocument();
            doc.Load("out_ext.xml");
            url = doc.SelectNodes("/root/item/url");
            float j;
            float uc;
           // System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt");
            
            for (int i = 0; i < url.Count; i++)
            {
                // myreq.Url = "https://hackerone.com" + url[i].InnerText + "/policy_versions";
                //json = CalculateMD5Hash(myreq.Send());
                myreq.Url = "https://hackerone.com" + url[i].InnerText;
                doc2 = JsonConvert.DeserializeXmlNode(myreq.Send(), "root");
                XmlNode root2 = doc2.DocumentElement;
                json = CalculateMD5Hash(root2.SelectSingleNode("/root").InnerText);
                //MessageBox.Show(root2.SelectSingleNode("/root/last_policy_change_at").InnerText);
                scope = root2.SelectNodes("/root/scopes");
                lstpol = root2.SelectSingleNode("/root/external_program/policy").InnerText.ToLower();
                //if (lstpol.Contains("signal") || lstpol.Contains("invit") || lstpol.Contains("bounty") || lstpol.Contains("swag") || lstpol.Contains("private") || lstpol.Contains("swag") || lstpol.Contains("gift") || lstpol.Contains("shirt"))
                // MessageBox.Show(url[i].InnerText);
               // if (lstpol.Contains("signal") || lstpol.Contains("invit"))
                  //  file.WriteLine(url[i].InnerText);
                for (int z = 0; z < scope.Count; z++)
                {
                    scp += scope[z].InnerText;
                    tmp += scope[z].InnerText+Environment.NewLine;
                }
                scp = CalculateMD5Hash(scp);
                verify2(url[i].InnerText, json, scp);
                outs += "<item url=\"" + url[i].InnerText + "\">" + json + "</item>";
                outs += "<scope url=\"" + url[i].InnerText + "\">" + scp + "</scope>";
                scp = "";
                j = i;
                uc = url.Count;
                progressBar1.Value = (int)Math.Ceiling((j / uc) * 100);
                //Application.DoEvents();
            }
           // file.Close();
            outs += "</root>";
            System.IO.File.WriteAllText("policy_ext.xml", outs);
            System.IO.File.WriteAllText("extscopes.txt", tmp);
            tmp = "";
            MessageBox.Show("Finish!");
            progressBar1.Value = 0;
        }
    }
}
