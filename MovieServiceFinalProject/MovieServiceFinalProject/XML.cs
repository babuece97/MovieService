﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Xsl;
using System.IO;
using Microsoft.SqlServer;
using System.Xml;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Net;


namespace MovieServiceFinalProject
{
    public class XML
    {
        private string sourcefile { get; set; }
        private string xsltfile { get; set; }
        private string destinationfile { get; set; 
    }
    public  XML()
        {
            // If the XML uses a namespace, the XSLT must refer to this namespace
            string sourcefile = Server.MapPath("XMLCommercial.xml");
            string xsltfile = Server.MapPath("XMLCommercial.xslt");
            string destinationfile = Server.MapPath("XMLCommercialTransformed.xml");

            FileStream fist = new FileStream(destinationfile, FileMode.Create);
            XslCompiledTransform xct = new XslCompiledTransform();
            xct.Load(xsltfile);
            xct.Transform(sourcefile, null, fist);
            fist.Close();
        }
    }
}