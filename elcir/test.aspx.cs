using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string HostName = Page.Server.MachineName;
        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "f",
            "<script language=\"javascript\" type=\"text/javascript\">alert('" + HostName + "')</SCRIPT>", false);
        string HostName1 = System.Environment.MachineName;
        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "s",
            "<script language=\"javascript\" type=\"text/javascript\">alert('" + HostName1 + "')</SCRIPT>", false);

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        // get the full path of image url
        string path = Server.MapPath(Image1.ImageUrl);

        // create image from the image url
        System.Drawing.Image img = System.Drawing.Image.FromFile(path);

        // rotate Image 90' Degree
        img.RotateFlip(RotateFlipType.Rotate90FlipXY);

        // save it to its actual path
        img.Save(path);

        // release Image File
        img.Dispose();

        // Set Image Control Attribute property to new image(but its old path)
        Image1.Attributes.Add("ImageUrl", path);
    }
    public byte[] imageToByteArray(System.Drawing.Image imageIn)
    {
        MemoryStream ms = new MemoryStream();
        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        return ms.ToArray();
    }

    public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
    {
        MemoryStream ms = new MemoryStream(byteArrayIn);
        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
        return returnImage;
    }

}
