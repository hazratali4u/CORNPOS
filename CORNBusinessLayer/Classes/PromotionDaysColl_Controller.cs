using System;


namespace CORNBusinessLayer.Classes
{
    public class PromotionDaysColl_Controller : System.Collections.CollectionBase   
    {
        #region Contructor
        public PromotionDaysColl_Controller()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Public Methods
        public void Add(PromotionDays_Collection p_PCustVolClassCol)
		{
            List.Add(p_PCustVolClassCol);			
		}
        public void Insert(int p_i, PromotionDays_Collection p_PCustVolClassCol)
		{
            List.Insert(p_i, p_PCustVolClassCol);
		}
		public void RemoveOn(int p_i)
		{
			List.RemoveAt (p_i);
		}
        public PromotionDays_Collection Get(int p_Index)
		{
            return (PromotionDays_Collection)List[p_Index];
		}
		#endregion
    }
}