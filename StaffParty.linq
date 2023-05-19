<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
</Query>

void Main()
{
	// to do: Put in a mode where it shows days that not everyone is available, but where most people aren't... also put a weighting on people depending on how much fun they are.
	var staffAvailbility = new List<Constraint>{
		new Constraint("Should be Soon", d =>d.Date < new DateTime(2023,6,30)),
		new Constraint("Kim Unavailable", IsKim),
		new Constraint("Mark Unavailable", IsMarkAvailable),
		new Constraint("Damien Unavailable", IsDamienAvailable) // to be adjusted... still need to speak with Bek. 
	};
	
	var results = new List<Result>();	
	
	//for (int i = 0; i < new DateTime(2023,12,28).Subtract(DateTime.Now).Days; i++)
	for (int i = 0; i < 60; i++)
	{
		var unavailable = new List<string>();
		var d = DateTime.Now.AddDays(i).Date;
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
								}).Dump("All Dates With Availability");
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

public bool IsDamienAvailable(DateTime d)
{
	var AvailableWeekNights = (System.DateTime d) => new[] {DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday}.Contains(d.DayOfWeek);

	DateTime[] unavailableDates = new DateTime[]
	{
		new (2023, 12, 25)
	};

	return !unavailableDates.Contains(d) && AvailableWeekNights(d);
}

public bool IsKim(DateTime d) => 
true ||
new DateTime[] {
	new DateTime(2021, 2, 05)}.All(k => k.Date != d.Date)
;


public bool IsMarkAvailable(DateTime d) => 
true || // remove
new DateTime[] {
new DateTime(2021, 2, 05)
}.All(k => k.Date != d.Date)
&& d > new DateTime(2021, 1, 31)
&& new[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday}.Contains(d.DayOfWeek)
;


public bool IsBetweenInclusive(DateTime d, DateTime startDate, DateTime endDate)
{
	return d >= startDate && d <= endDate;
}