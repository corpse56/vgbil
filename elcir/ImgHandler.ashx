<%@ WebHandler Language="C#" Class="ImgHandler" %>

using System;
using System.Web;
using System.Web.Extensions;
using System.Text;
using System.Security.Cryptography;
using System.IO;
public class ImgHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) 
    {
        string ip = XmlConnections.GetConnection("/Connections/img_ip");

        string pinpath = MyDecrypt(HttpServerUtility.UrlTokenDecode(context.Request["img"]));



        //string imgfile = @"\\" + ip + @"\BookAddInf\" + context.Request["img"];
        string imgfile = @"\\" + ip + @"\BookAddInf\" + pinpath;
        System.IO.FileInfo info = new System.IO.FileInfo(imgfile);
        byte[] imgcontent = GetImageContent(info);
        context.Response.ContentType = "image/jpeg";
        context.Response.BinaryWrite(imgcontent);
        //if (context.Items["rotate"] != null)
        //{
         //   int r = (int)context.Items["rotate"];
       // }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    private byte[] GetImageContent(System.IO.FileInfo info)
    {
        byte[] content = new byte[info.Length];
        System.IO.FileStream imagestream = info.OpenRead();
        imagestream.Read(content, 0, content.Length);
        imagestream.Close();

        //System.Drawing.Image img = byteArrayToImage(content);
        //img.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipXY);
        //content = imageToByteArray(img);
        return content;
    }
    public byte[] imageToByteArray(System.Drawing.Image imageIn)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        return ms.ToArray();
    }

    public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn);
        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
        return returnImage;
    }
    public static string MyDecrypt(byte[] cipherText2)
    {
        //In live, get the persistant passphrase from other isolated source
        //This example has hardcoded passphrase just for demo purpose, obtained by 
        //adding current user's firstname + DOB + email
        //You may generate this string with any logic you want.
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("030320150303");

        //Generate the Salt, with any custom logic and
        //using the above string
        StringBuilder _sbSalt = new StringBuilder();
        for (int i = 0; i < 8; i++)
        {
            _sbSalt.Append("," + sb.Length.ToString());
        }
        byte[] Salt = Encoding.ASCII.GetBytes(_sbSalt.ToString());

        //Key generation:- default iterations is 1000 and recomended is 10000
        System.Security.Cryptography.Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(sb.ToString(), Salt, 1000);

        //The default key size for RijndaelManaged is 256 bits,
        //while the default blocksize is 128 bits.
        RijndaelManaged _RijndaelManaged = new RijndaelManaged();
        _RijndaelManaged.BlockSize = 256; //Increase it to 256 bits- more secure

        byte[] key = pwdGen.GetBytes(_RijndaelManaged.KeySize / 8);   //This will generate a 256 bits key
        byte[] iv = pwdGen.GetBytes(_RijndaelManaged.BlockSize / 8);  //This will generate a 256 bits IV

        //On a given instance of Rfc2898DeriveBytes class,
        //GetBytes() will always return unique byte array.
        _RijndaelManaged.Key = key;
        _RijndaelManaged.IV = iv;

        //Now decrypt
        byte[] plainText2 = null;
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, _RijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherText2, 0, cipherText2.Length);
            }
            plainText2 = ms.ToArray();
        }
        //Decrypted text
        return System.Text.Encoding.Unicode.GetString(plainText2);
    }

}