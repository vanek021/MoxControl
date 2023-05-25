using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Interfaces.Factories
{
	public interface IConnectServiceItem
	{
		public VirtualizationSystem VirtualizationSystem { get; set; }
		public IConnectService Service { get; set; }
	}
}
