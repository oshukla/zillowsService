using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestSharp;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

namespace SwaggerUi.Controllers
{
    public class SearchAddressController : ApiController
    {
        private readonly RestClient client = new RestClient("https://www.zillow.com");
        // GET: api/SearchAddress/5
        /// <summary>
        /// The GetSearchResults API finds a property for a specified address. The content returned contains the address for the property or properties as well as the Zillow Property ID (ZPID) and current Zestimate®. It also includes the date the Zestimate was computed, a valuation range and the Zestimate ranking for the property within its ZIP code.
        /// </summary>
        /// <param name="address">The address of the property to search.</param>
        /// <param name="cityStateZip">The city+state combination and/or ZIP code for which to search. This string should be URL encoded. Note that giving both city and state is required. Using just one will not work.</param>
        /// <param name="rentZestimate">Return Rent Zestimate information if available (boolean true/false, default: false)</param>
        /// <returns>searchresults object</returns>
        [HttpGet]
        [ResponseType(typeof(searchresults))]
        public searchresults SearchAddress(string address,string cityStateZip,bool rentZestimate=false)
        {
            string zws_id = ConfigurationManager.AppSettings["zillowsKey"];
            //string zws_id = Request.GetQueryNameValuePairs().FirstOrDefault(x=>x.Key=="api_key").Value;
            Uri uri = new Uri(string.Format("http://www.zillow.com/webservice/GetSearchResults.htm?zws-id={0}&&address={1}&&citystatezip={2}&&rentzestimate={3}",zws_id,address,cityStateZip,rentZestimate));
            RestRequest request = new RestRequest(uri, Method.GET);
            var response = this.client.Execute<searchresults>(request);
            var responseString = response.Content;

            XmlSerializer serializer = new XmlSerializer(typeof(searchresults));
            using (TextReader reader = new StringReader(responseString))
            {
                searchresults result = (searchresults)serializer.Deserialize(reader);
                return result;
            }
            
        }

      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}