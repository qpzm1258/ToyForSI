using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ToyForSI.ViewModels
{
    [DataContract]
	public class DataPoint
	{
		public DataPoint(string indexLabel, double y)
		{
			this.IndexLabel = indexLabel;
			this.Y = y;
		}
 
		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "indexLabel")]
		public string IndexLabel = null;
 
		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;	
	}
}
