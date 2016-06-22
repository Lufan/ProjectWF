using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Ninject;

using DomainLayer.Identity;
using DomainLayer.DataAccess;
using DomainLayer.DataAccess.MongoDb;
using DomainLayer.DataAccess.Query;
using DomainLayer.DataAccess.Record;
using DomainLayer.Contact;

namespace web.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // TO DO: add bindings here
            #region Identity bindings
            // Identity MongoDbContext bindings
            kernel.Bind<IIdentityDbContext>().To<MongoIdentityDbContext>().InSingletonScope();
            #endregion Identity bindings

            #region Query bindings
            //**************** QUERY *************** BINDINGS *****************
            //QueryStore bindings for QueryManagers
            kernel.Bind<IQueryManager<Contact>>().To<ContactsQueryManager>();
            kernel.Bind<IQueryManager<IOrganization>>().To<OrganizationsQueryManager>();

            kernel.Bind<IDocumentQueryStore<Contact>>().To<ContactQueryStore>().InSingletonScope();
            kernel.Bind<IDocumentQueryStore<IOrganization>>().To<OrganizationQueryStore>().InSingletonScope();
            #endregion Query bindings

            #region Record bindings
            //**************** RECORD *************** BINDINGS *****************
            //RecordStore bindings for RecordManagers
            kernel.Bind<IRecordManager<IContact, IAppUser>>().To<ContactsRecordManager>();
            kernel.Bind<IRecordManager<IOrganization, IAppUser>>().To<OrganizationsRecordManager>();

            kernel.Bind<IDocumentRecordStore<IContact, IAppUser>>().To<ContactRecordStore>().InSingletonScope();
            kernel.Bind<IDocumentRecordStore<IOrganization, IAppUser>>().To<OrganizationRecordStore>().InSingletonScope();
            #endregion Query bindings

            //MongoDatabase bindings for QueryStore
            //Contacts and Organizations collections placed into ContactDb database
            kernel.Bind<IContactDbContext>().To<MongoContactDbContext>().InSingletonScope();
        }
    }
}