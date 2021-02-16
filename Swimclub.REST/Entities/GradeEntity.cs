using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Entities
{
	[Table("Grades")]
	public class Grade
	{
		[Key, Required]
		public int ID { get; set; }
		[Required]
		public string Forename { get; set; }

		//public string Surname { get; set; }
		//[Required]
		//public string SwimEnglandNumber { get; set; }
		//[Required]
		//public DateTime DateOfBirth { get; set; }
		//[Required]
		//public int CurrentGradeNumber { get; set; }
		////[Required]
		////public MedicalDetails MedicalDetails { get; set; }
		//[NotMapped]
		//public MedicalDetails MedicalDetails { get; set; }
		//public string MedicalDetailsJson
		//{
		//	get { return JsonSerializer.Serialize(MedicalDetails); }
		//	set { MedicalDetails = JsonSerializer.Deserialize<MedicalDetails>(value); }
		//}

		//public static Models.Student GetStudent(Entities.Student _studentEntity)
		//{
		//	return new Models.Student()
		//	{
		//		ID = _studentEntity.ID,
		//		Forename = _studentEntity.Forename,
		//		Surname = _studentEntity.Surname,
		//		SwimEnglandNumber = _studentEntity.SwimEnglandNumber,
		//		CurrentGradeNumber = _studentEntity.CurrentGradeNumber,
		//		DateOfBirth = _studentEntity.DateOfBirth,
		//		MedicalDetails = _studentEntity.MedicalDetails
		//	};
		//}
	}
}
