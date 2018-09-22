using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;


namespace CSOM1
{
    class Program
    {
        //string siteUrl = "";
        //string userName = "";
        //var password = "";


        static void Main(string[] args)
        {
            Program p = new Program();

            //string siteUrl = "";
            //string userName = "";
            //var password = "";

            SecureString secPassword = GetPasswordFromConsoleInput(password);
            ClientContext context = new ClientContext(siteUrl);
            context.Credentials = new SharePointOnlineCredentials(userName, secPassword);

            //p.GetSiteTitle(context);
            //p.CreateSiteColumn(context);
            p.CreateContentType(context);
        }

        private void CreateContentType(ClientContext context)
        {
            //Reference - 
            //http://www.srinisistla.com/blog/Lists/Posts/Post.aspx?ID=100
            //https://msdn.microsoft.com/library/office/microsoft.sharepoint.client.contenttypecollection.add.aspx
            try
            {
                //ContentTypeCollection cTCollection = context.Site.RootWeb.AvailableContentTypes;  // AvailableContentTypes is read only. It returns all the CT from web as well as root site
                ContentTypeCollection cTCollection = context.Site.RootWeb.ContentTypes;  // ContentTypes returns all the CT from current web site only
                //context.Load(cTCollection);
                //context.ExecuteQuery();
                //foreach (ContentType ct in cTCollection)
                //{
                //    Console.WriteLine(ct.Name + "   :   " + ct.Parent);
                //}

                ContentTypeCreationInformation cti = new ContentTypeCreationInformation();
                cti.Description = "this is a test Content Type";
                cti.Name = "TestContentTypeRohit";
                cti.Group = "RohitCTGroup";
                ContentType ct = cTCollection.Add(cti);
                context.Load(cTCollection);
                context.ExecuteQuery();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CreateContent Type :  " + ex.Message.ToString());
            }
        }

        private void CreateSiteColumn(ClientContext context)
        {
            Guid guid = Guid.NewGuid();
            try
            {
                Web rootWeb = context.Site.RootWeb;
                //Web rootWeb = context.Web;
                rootWeb.Fields.AddFieldAsXml("<Field DisplayName='TestSiteColumn' Name='SessionName' ID='" + guid + "' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                context.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CreateSiteColumn :  " + ex.Message.ToString());
            }
        }

        private void GetSiteTitle(ClientContext context)
        {
            try
            {
                Web web = context.Web;
                context.Load(web);
                context.ExecuteQuery();

                Console.WriteLine("Web Name is : " + web.Title);
            }
            catch (Exception ex)
            {

            }

        }

        private static SecureString GetPasswordFromConsoleInput(string password)
        {
            SecureString securePassword = new SecureString();
            foreach (char c in password.ToCharArray())
                securePassword.AppendChar(c);
            return securePassword;
        }
    }
}
