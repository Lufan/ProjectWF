﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;


using web.Infrastructure;
using DomainLayer.Contact;
using DomainLayer.Identity;
using web.Models;

namespace web.Controllers
{
    [Authorize]
    public class ApiOrganizationsController : ApiController
    {
        public ApiOrganizationsController(
            IQueryManager<Organization> organizationQM,
            IRecordManager<Organization, IAppUser> organizationRM
            )
        {
            this.organizationQM = organizationQM;
            this.organizationRM = organizationRM;
        }

        // GET: api/ApiOrganizations
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            var result = GetViewModel();
            return Ok(result);
        }

        // GET: api/ApiOrganizations/5
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(string id)
        {
            var organization = await organizationQM.FindByIdAsync(id);
            if (organization != null)
            {
                var result = new OrganizationsViewModel
                {
                    Id = organization.Id,
                    OrganizationName = organization.OrganizationName,
                    Adress = organization.Adress,
                    Remarks = organization.Remarks
                };
                return Ok(result);
            }

            return NotFound();
        }

        // POST: api/ApiOrganizations
        public void Post([FromBody]OrganizationsViewModel organization)
        {
            if (organization != null)
            {
                var ot = organizationQM.FindByIdAsync(organization.Id).Result;
                if (ot == null)
                {
                    ot = new Organization
                    {
                        OrganizationName = organization.OrganizationName,
                        Adress = organization.Adress,
                        Remarks = organization.Remarks
                    };
                    var user_query = UserManager.Users.Select(e => e).Where(e => e.Id == User.Identity.GetUserId());
                    if (user_query.Count() == 0) return;
                    organizationRM.CreateAsync(ot, user_query.First());
                }
                else
                {
                    ot = organizationQM.FindByIdAsync(organization.Id).Result;
                    if (ot == null) return;
                    ot.OrganizationName = organization.OrganizationName;
                    ot.Adress = organization.Adress;
                    ot.Remarks = organization.Remarks;

                    var user_query = UserManager.Users.Select(e => e).Where(e => e.Id == User.Identity.GetUserId());
                    if (user_query.Count() == 0) return;

                    organizationRM.UpdateAsync(ot, user_query.First());
                }
            }
        }

        // PUT: api/ApiOrganizations/5
        // Only update the contact with id and never create new one.
        public void Put(string id, [FromBody]OrganizationsViewModel organization)
        {
            if (id == null || organization == null) return;

            var ot = organizationQM.FindByIdAsync(id).Result;
            if (ot == null) return;
            ot.OrganizationName = organization.OrganizationName;
            ot.Adress = organization.Adress;
            ot.Remarks = organization.Remarks;

            var user_query = UserManager.Users.Select(e => e).Where(e => e.Id == User.Identity.GetUserId());
            if (user_query.Count() == 0) return;

            organizationRM.UpdateAsync(ot, user_query.First());
        }

        // DELETE: api/ApiOrganizations/5
        public void Delete(string id)
        {
            if (id == null) return;
            var ot = organizationQM.FindByIdAsync(id).Result;
            if (ot == null) return;

            var user_query = UserManager.Users.Select(e => e).Where(e => e.Id == User.Identity.GetUserId());
            if (user_query.Count() == 0) return;

            // TO DO - maybe best to be don't delete the contact and only mark it as deleted?
            organizationRM.DeleteAsync(ot, user_query.First());
        }

        private async Task<IEnumerable<OrganizationsViewModel>> GetViewModel(int count = 10, int start_pos = 0)
        {
            var organizations = await organizationQM.TakeNAsync(count, start_pos);
            IList<OrganizationsViewModel> result = new List<OrganizationsViewModel>(count);
            foreach (var organization in organizations)
            {
                result.Add(
                    new OrganizationsViewModel
                    {
                        Id = organization.Id,
                        OrganizationName = organization.OrganizationName,
                        Adress = organization.Adress,
                        Remarks = organization.Remarks
                    });
            }
            return result.AsEnumerable();
        }
        
        private IQueryManager<Organization> organizationQM;
        private IRecordManager<Organization, IAppUser> organizationRM;

        private UserManager<AppUser> UserManager
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<UserManager<AppUser>>();
            }
        }
    }
}
