<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
</Query>

void Main()
{
	// to do: Put in a mode where it shows days that not everyone is available, but where most people aren't... also put a weighting on people depending on how much fun they are.
	var staffAvailbility = new List<Constraint>{
		new Constraint("Should be Soon", d => d.Date < new DateTime(2020,12,31)),
		new Constraint("Damien", (d)=> (d.Date != new DateTime(2020,12,25))), // to be adjusted... need to speak with Bek. 
		new Constraint("Kim", (d)=>true),
		new Constraint("Irina", (d)=>true),
		new Constraint("Mark", (d)=>true),
		//new Constraint("Bill", IsWesAvailable),
	};
	
	var results = new List<Result>();	
	
	for (int i = 0; i < new DateTime(2020,12,31).Subtract(DateTime.Now).Days; i++)
	{
		var unavailable = new List<string>();
		var d = DateTime.Now.AddDays(i);
		bool available = true;
		foreach (var item in staffAvailbility)
		{
			if (!item.IsAvailable(d)) {
				available = false;
				unavailable.Add(item.Name);
			}
		}
		results.Add(new Result{Available = available, UnavailablePeople = unavailable, Date = d});
	}
	
	results.Select (d => new {
								Date =string.Format("{0:ddd dd-MMM-yy}", d.Date),
								Available = d.Available,
								UnavailablePeople = string.Join(", ", d.UnavailablePeople.ToArray()),
								}).Dump("All Available Dates");
}

// Could be a person's or a business constraint
public class Constraint
{
	public Func<DateTime,bool> IsAvailable { get; set; }
	public string Name { get; set; }
	public Constraint(string name, Func<DateTime,bool> isAvailable)
	{
			this.Name = name;
			this.IsAvailable = isAvailable;
	}
}

public class Result{
	public DateTime Date { get; set; }
	public bool Available { get; set; }
	public List<string> UnavailablePeople { get; set; }
}

public bool IsWesAvailable(DateTime d)
{
	if (IsBetweenInclusive(d, new DateTime(2015,8,14), new DateTime(2015,8,15))) return false;
	
    switch(d.DayOfWeek) {
        case DayOfWeek.Monday:   return d.Day <= 7 || d.Day > 14;
        case DayOfWeek.Friday:   return d.Day > 28;
		
        case DayOfWeek.Thursday: 
		{
			switch (d.Month) {
				case 6: return false;
				case 7: return d.Day < 10 || d.Day == 30;
				case 8: return false;
				case 9: return d.Day > 17;
				case 10: return d.Day < 8 && d.Day > 15;
			}
			break;
		}
    }
	
	return true;        
}

public bool IsBetweenInclusive(DateTime d, DateTime startDate, DateTime endDate)
{
	return d >= startDate && d <= endDate;
}