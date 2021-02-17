using Swimclub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Swimclub.REST.Entities
{
	[Table("Students")]
	public class Student
	{
		[Key, Required]
		public int ID { get; set; }
		[Required]
		public string Forename { get; set; }
		public string Surname { get; set; }
		[Required]
		public string SwimEnglandNumber { get; set; }
		[Required]
		public DateTime DateOfBirth { get; set; }
		[Required]
		public int CurrentGradeNumber { get; set; }
		//[Required]
		//public MedicalDetails MedicalDetails { get; set; }
		[NotMapped]
		public MedicalDetails MedicalDetails { get; set; }
		public string MedicalDetailsJson
		{
			get { return JsonSerializer.Serialize(MedicalDetails); }
			set { MedicalDetails = JsonSerializer.Deserialize<MedicalDetails>(value); }
		}

		public static Models.Student GetStudent(Entities.Student _studentEntity)
		{
			return new Models.Student() { ID = _studentEntity.ID, Forename = _studentEntity.Forename, Surname = _studentEntity.Surname,
				SwimEnglandNumber = _studentEntity.SwimEnglandNumber, CurrentGradeNumber = _studentEntity.CurrentGradeNumber,
				DateOfBirth = _studentEntity.DateOfBirth, MedicalDetails = _studentEntity.MedicalDetails
			};
		}

		public static Entities.Student GetEntity(Models.Student _studentModel)
		{
			return new Entities.Student()
			{
				ID = _studentModel.ID,
				Forename = _studentModel.Forename,
				Surname = _studentModel.Surname,
				SwimEnglandNumber = _studentModel.SwimEnglandNumber,
				CurrentGradeNumber = _studentModel.CurrentGradeNumber,
				DateOfBirth = _studentModel.DateOfBirth,
				MedicalDetails = _studentModel.MedicalDetails
			};
		}

		public static void ApplyChanges(Models.Student toApply, ref Entities.Student result)
		{
			result.Forename = toApply.Forename;
			result.Surname = toApply.Surname;
			result.SwimEnglandNumber = toApply.SwimEnglandNumber;
			result.DateOfBirth = toApply.DateOfBirth;
			result.CurrentGradeNumber = toApply.CurrentGradeNumber;
			result.MedicalDetails = toApply.MedicalDetails;
		}

		public static bool CheckRequirements(Entities.Student ent)
		{
			bool check = ent.Forename == null || ent.Surname == null || ent.SwimEnglandNumber == null || ent.MedicalDetails == null;
			return !check;
		}
	}

	
}
