using System;
using XRL.Rules;

namespace XRL.World.Parts
{
	[Serializable]
	public class Thorns : IPart
	{
		//
		// Constructors
		//
		public Thorns ()
		{
			base.Name = "Thorns";
		}

		//
		// Methods
		//
		public override bool FireEvent (Event E)
		{
			if (E.ID == "ObjectEnteredCell") {
				GameObject gameObject = E.GetParameter ("Object") as GameObject;
				if (gameObject != null && gameObject.HasPart ("Combat") && Factions.GetFeelingFactionToObject ("Succulents", gameObject) < 50 && gameObject.PhaseAndFlightMatches (this.ParentObject) && gameObject != this.ParentObject) {
					Damage value = new Damage (Stat.Roll ("1d1"));
					Event @event = Event.New ("TakeDamage", 0, 0, 0);
					@event.AddParameter ("Damage", value);
					@event.AddParameter ("Owner", null);
					@event.AddParameter ("Attacker", this.ParentObject);
					@event.AddParameter ("Message", "from %o thorns.");
					if (gameObject.Statistics.ContainsKey ("Energy")) {
						gameObject.Energy.BaseValue -= 500;
					}
					if (gameObject.FireEvent (@event)) {
						gameObject.Bloodsplatter ();
					}
				}
			}
			return base.FireEvent (E);
		}

		public override void Register (GameObject Object)
		{
			Object.RegisterPartEvent (this, "ObjectEnteredCell");
			base.Register (Object);
		}
	}
}
