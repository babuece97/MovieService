﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Xsl;
using System.Data.SqlClient;

namespace MovieServiceFinalProject
{
    public partial class SearchIndex : System.Web.UI.Page

    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            show();
            //just for test
            TransformXslt();
        }
        public void show()
        {
            MovieContainer action1 = new MovieContainer();
            action1.ActionMovie(ListBoxMovieDisplay);
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            string result = "";

            // substitute " " with "+"
            string myselection = TextBoxSearch.Text.Replace(' ', '+');
            result = client.DownloadString("http://www.omdbapi.com/?t=" + myselection + "&r=xml&apikey=" + Token.token);
            File.WriteAllText(Server.MapPath("~/MyFiles/Latestresult.xml"), result);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

            if (doc.SelectSingleNode("/root/@response").InnerText == "True")
            {
                LabelMessage.Text = "Movie found";
                XmlNodeList nodelist = doc.SelectNodes("/root/movie");
                foreach (XmlNode node in nodelist)
                {
                    string id = node.SelectSingleNode("@poster").InnerText;
                    ImagePoster.ImageUrl = id;
                }
                var Title = nodelist[0].SelectSingleNode("@title").InnerText;
                var ImageLink = nodelist[0].SelectSingleNode("@poster").InnerText;
                LabelResult.Text = " Rating " + nodelist[0].SelectSingleNode("@imdbRating").InnerText;
                LabelResult.Text += " from " + nodelist[0].SelectSingleNode("@imdbVotes").InnerText + "votes";
                LabelYear.Text += " " + nodelist[0].SelectSingleNode("@year").InnerText;
                LabelActors.Text += " " + nodelist[0].SelectSingleNode("@actors").InnerText;
                LabelDirector.Text += " " + nodelist[0].SelectSingleNode("@director").InnerText;
                LabelWriter.Text += " " + nodelist[0].SelectSingleNode("@writer").InnerText;

                //INCREASE NUMBER OF VISITED MOVIE AND DOWNLOAD POSTER OF MOVIE
                SqlConnection conn = new SqlConnection(@"data source = .\Sqlexpress; integrated security = true; database = MovieFlex");
                SqlCommand cmd = null;
                SqlCommand cmd1 = null;
                SqlDataReader rdr = null;
                string sqlsel = "";
                string sqlsel1 = "";
                try
                {
                    conn.Open();

                    sqlsel = "update Movie set Visit_Counter=Visit_Counter+1 where MovieName=@MovieName ";
                    sqlsel1 = "update Movie set Picture=@Picture where MovieName=@MovieName";
                    cmd = new SqlCommand(sqlsel, conn);
                    cmd.Parameters.AddWithValue("@MovieName", Title);
                    cmd1 = new SqlCommand(sqlsel1, conn);
                    cmd1.Parameters.AddWithValue("@Picture", ImageLink);
                    cmd1.Parameters.AddWithValue("@MovieName", Title);
                    cmd.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();
                    rdr.Close();
                }
                catch (Exception)
                {
                    Title = "not found";
                    //LabelMessages.Text = ex.Message;
                }
                finally
                {
                    conn.Close();
                }
                
            }


            else
            {
                LabelMessage.Text = "Movie not found";
                ImagePoster.ImageUrl = "~/MyFiles/titanic.jpg";
                // LabelResult.Text = "Result";
            }
        }

        //Transform XSLT
        public void TransformXslt()
        {
            string sourcefile1 = Server.MapPath("XMLCommercial.xml");
            string xsltfile1 = Server.MapPath("XSLTCommercial.xslt");
            string destinationfile1 = Server.MapPath("CommercialTransformed.xml");
            XML xslt1 = new XML(sourcefile1, xsltfile1, destinationfile1);
            xslt1.Transform();
        }


        protected void ButtonMovieAction_Click(object sender, EventArgs e)
        {


        }

        protected void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
           // TextBoxSearch.Text = ListBoxMovieDisplay.SelectedValue;
        }

        protected void ListBoxMovieDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxSearch.Text = ListBoxMovieDisplay.SelectedValue;
        }
    }
}


    
