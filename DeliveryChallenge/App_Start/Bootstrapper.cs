using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using DeliveryChallenge.Models;
using DeliveryChallenge.Models.Repository;

namespace DeliveryChallenge.App_Start
{
	public class Bootstrapper
	{
		public static IUnityContainer Initialise()
		{
			var container = BuildUnityContainer();
			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
			return container;
		}
		private static IUnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			// register all your components with the container here  
			//This is the important line to edit  
			container.RegisterType<IModelDbContext, ModelDbContext>();
			container.RegisterType<IEmployeeRepository, EmployeeRepository>();
			container.RegisterType<ISkillRepository, SkillRepository>();
			container.RegisterType<IDeliveryRepository, DeliveryRepository>();
			container.RegisterType<IDeliveryDetailRepository, DeliveryDetailRepository>();
			container.RegisterType<IDeliveryTypeRepository, DeliveryTypeRepository>();

			return container;
		}
	}
}
