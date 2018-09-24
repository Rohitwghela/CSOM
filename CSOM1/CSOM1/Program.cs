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
            //p.CreateContentType(context);
            p.DeleteContentTypeCode(context);
            //p.ContentTypeCode(context);
        }

        private void DeleteContentTypeCode(ClientContext context)
        {
            
            Web rootWeb = context.Site.RootWeb;
            ContentTypeCollection ctCollection = rootWeb.ContentTypes;
            context.Load(ctCollection);
            context.ExecuteQuery();

            foreach (ContentType c in ctCollection)
            {
                if (c.Name == "CT1")
                {
                    //cid = c.Id;
                }
            }

            ContentType ct = context.Web.ContentTypes.GetById(cid.ToString());

            
        }

        private void ContentTypeCode(ClientContext context)
        {
            Guid guid = Guid.NewGuid();
            try
            {
                Web rootWeb = context.Site.RootWeb;
                var field1 = rootWeb.Fields.AddFieldAsXml("<Field DisplayName='TestSiteColumn' Name='SessionName' ID='" + guid + "' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                //context.ExecuteQuery();

                ContentTypeCollection ctCollection = context.Web.ContentTypes;
                context.Load(ctCollection);
                context.ExecuteQuery();

                // create by reference
                //ContentType itemContentTypes = context.LoadQuery(rootWeb.ContentTypes.Where(ct => ct.Name == "Item"));

                ContentType itemContentTypes = ctCollection.GetById("0x0101");
                context.ExecuteQuery();
                ContentTypeCreationInformation cti = new ContentTypeCreationInformation();
                cti.Name = "CT1";
                cti.Description = "test content type CSOM";
                cti.ParentContentType = itemContentTypes;
                cti.Group = "RohitCustomCT";
                ContentType myContentType = ctCollection.Add(cti);
                context.ExecuteQuery();
                //myContentType.Fields.Add(field1);
                myContentType.FieldLinks.Add(new FieldLinkCreationInformation
                {
                    Field = field1
                });
                myContentType.Update(true);
                context.ExecuteQuery();


                ListCreationInformation lct = new ListCreationInformation();
                lct.Title = "LogList";
                lct.Description = "this is test list";
                lct.TemplateType = (int)ListTemplateType.GenericList;
                List logList = context.Web.Lists.Add(lct);
                context.Load(logList);
                context.ExecuteQuery();

                if (!logList.ContentTypesEnabled)
                {
                    logList.ContentTypesEnabled = true;
                    logList.Update();
                    context.ExecuteQuery();
                }
                logList.ContentTypes.AddExistingContentType(myContentType);
                context.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CreateSiteColumn :  " + ex.Message.ToString());
            }
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
