using System;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Text;

public partial class UserDefinedFunctions
{
  [Microsoft.SqlServer.Server.SqlFunction]
  public static SqlBytes FBaixaArquivosUrl(SqlString Url, SqlString Path)
  {
    //here is where we get the file extension
    string[] SplitsUrl = Url.ToString().Split('.');

    //here the name of the file is created as something that is impossible to repeat
    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + SplitsUrl[SplitsUrl.Length - 1];

    //Path format: C:\Repository\
    string FilePath = Path + FileName;

    //here is where the file is downloaded
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
    WebClient Client = new WebClient();
    Client.DownloadFile(Url.ToString(), FilePath);

    //Read data
    FileInfo File = new FileInfo(FilePath);
    BinaryReader BinaryReader = new BinaryReader(File.OpenRead(FilePath));
    byte[] BinaryFile = BinaryReader.ReadBytes((int)File.Length);

    //Convert to SqlBytes for SQL
    var Sqlbytes = new SqlBytes(BinaryFile);  

    return Sqlbytes;
  }
}
