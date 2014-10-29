using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;
using System.Web.Helpers;   // phải check vào mới add -> ~.~

namespace DemoJSON
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string str = txtLink.Text;
            MessageBox.Show(GetWebServices(str));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyJSonParse();
        }

        private void MyJSonParse()
        {
            WebClient wb = new WebClient();

            var data = "";

            if (txtLink.Text == "")
                data = "http://api.geonames.org/citiesJSON?north=44.1&south=-9.9&east=-22.4&west=55.2&lang=de&username=demo";
            
            else
                data = txtLink.Text;

            data = wb.DownloadString(data);


            dynamic json;
            if (txtLink.Text == "")
            {
                data = @"{ ""Name"": ""Jon Smith"", ""Address"": { ""City"": ""New York"", ""State"": ""NY"" }, ""Age"": 42 }";
                json = System.Web.Helpers.Json.Decode(data);
            }
            else
            {

                
                // ok
                // data += "{\"flag\": 1, \"result\": { \"ITEM_ID\": \"CNSK\", \"NAME\": \"Cẩm nang sự kiện\",\"PRODUCT_CODE\": \"FS\",\"ID_INGAME\": \"6,1,5572\", \"TYPE\": \"1\", \"START_TIME\": null, \"END_TIME\": null } }";
                
                // data = "{ \"1\": { \"result\": {   \"ITEM_ID\":\"CNSK\", \"NAME\":\"C\u1ea9m nang s\u1ef1 ki\u1ec7n\", \"PRODUCT_CODE\":\"FS\", \"ID_INGAME\":\"6,1,5572\", \"TYPE\":\"1\", \"START_TIME\":null, \"END_TIME\":null} }, \"0\":1 }";
                


                // data = "{ \"results\": [ { \"SwiftCode\":\"\", \"City\": \"\", \"BankName\": \"Deutsche Bank\", \"Bankkey\":\"10020030\", \"Bankcountry\":\"DE\" }, { \"SwiftCode\":\"\", \"City\":\"10891 Berlin\", \"BankName\": \"Commerzbank Berlin (West)\", \"Bankkey\": \"10040000\", \"Bankcountry\":\"DE\" } ]}";


                json = System.Web.Helpers.Json.Decode(data);
            }

            /*
            {
                "flag": 1,
                "result": {
                    "ITEM_ID": "CNSK",
                    "NAME": "Cẩm nang sự kiện",
                    "PRODUCT_CODE": "FS",
                    "ID_INGAME": "6,1,5572",
                    "TYPE": "1",
                    "START_TIME": null,
                    "END_TIME": null
                }
            }
            
            json.flag
            json.result.ITEM_ID
            
            */

            /*
            {   "Name": "Jon Smith", 
                "Address": 
                { 
                    "City": "New York", 
                    "State": "NY" 
                }, 
                "Age": 42
            }
            -> json.Name
            -> json.Address.City
             
            */

            /*
            {
               "1": {
                   "result":
                    {  
                        "ITEM_ID":"CNSK",
                        "NAME":"C\u1ea9m nang s\u1ef1 ki\u1ec7n",
                        "PRODUCT_CODE":"FS",
                        "ID_INGAME":"6,1,5572",
                        "TYPE":"1",
                        "START_TIME":null,
                        "END_TIME":null}
                    },
                "0":1
            }
             * 
             -> json["1"].result.ITEM_ID = "CNSK" ;
             -> json["1"].result.NAME = "Cẩm nang sự kiện";
             -> json["1"].START_TIME = "";
             */

            /*
            {
                'results':[
                    {
                        'SwiftCode':'',
                        'City':'',
                        'BankName':'Deutsche    Bank',
                        'Bankkey':'10020030',
                        'Bankcountry':'DE'
                    },
                    {
                        'SwiftCode':'',
                        'City':'10891    Berlin',
                        'BankName':'Commerzbank Berlin (West)',
                        'Bankkey':'10040000',
                        'Bankcountry':'DE'
                    }
            ]};
             
            -> json.results[0].SwiftCode = ""
            -> json.results[0].BankName = "Deutsche    Bank"
            -> json.results[1].BankKey = "10040000"
            
            */

            
            MessageBox.Show(json["1"].result.ITEM_ID);
            MessageBox.Show(json["1"].result.NAME);
            MessageBox.Show(json["1"].START_TIME);
        }

        private String GetWebServices(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch(WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responStream = errorResponse.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responStream, Encoding.GetEncoding("utf-8"));
                    String errorText = sr.ReadToEnd();
                    // log errorText;
                }
                throw;
            }
        }       
    }
}
