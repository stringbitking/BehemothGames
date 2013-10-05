using Behemoth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Behemoth.Games.Controllers
{
    public class VotesApiController : ApiController
    {
        protected IUowData Data { get; set; }

        public VotesApiController()
            : this(new UowData())
        {
        }

        public VotesApiController(IUowData data)
        {
            this.Data = data;
        }
    }
}
