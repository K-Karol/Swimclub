using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
	public class MedicalDetails
	{
		public string[] Allergies { get; set; }
		public string[] Immunizations { get; set; }
		public string[] Illnesses { get; set; }
		public string[] Disabilities { get; set; }
		public Contact[] EmergencyContacts { get; set; }
		public string Notes { get; set; }
	}
}
